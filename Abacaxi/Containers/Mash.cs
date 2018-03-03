/* Copyright 2017-2018 by Alexandru Ciobanu (alex+git@ciobanu.org)
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation 
 * files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, 
 * modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software 
 * is furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF 
 * MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE 
 * FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION 
 * WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */

namespace Abacaxi.Containers
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using Internal;
    using JetBrains.Annotations;

    /// <summary>
    /// Class implements a Dictionary of Dictionaries plus List and such. Serves as a Swiss army knife container that can
    /// be used to store anything.
    /// </summary>
    [PublicAPI, DebuggerDisplay("Count = {" + nameof(Count) + "} Children = {" + nameof(LinkedCount) + "}")]
    public sealed class Mash<TKey, TValue> : IList<TValue>
    {
        [Flags]
        private enum StorageState : byte
        {
            HasOneChildInATuple = 1,
            HasTwoChildrenInKeyValuePairArray = 2,
            HasThreeChildrenInKeyValuePairArray = 3,
            HasFourChildrenInKeyValuePairArray = 4,
            HasFiveChildrenInKeyValuePairArray = 5,
            HasManyChildrenInAHashTable = 6,
            ValueIsOneObject = 1 << 3,
            ValuesInTwoElementArray = 2 << 3,
            ValuesInList = 3 << 3,

            ChildrenMask = 7,
            ValueMask = 3 << 3
        }

        [NotNull] private readonly IEqualityComparer<TKey> _equalityComparer;
        [CanBeNull] private object _childrenObj;
        [CanBeNull] private object _valueObj;
        private StorageState _state;
        private int _ver;

        [ContractAnnotation("=> halt")]
        private static void ThrowCollectionChangedDuringEnumeration()
        {
            throw new InvalidOperationException("Collection changed during enumeration. Cannot continue.");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Mash{TKey, TValue}"/> class.
        /// </summary>
        /// <param name="equalityComparer">The equality comparer used for sub-mash indexing.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="equalityComparer"/> is <c>null</c>.</exception>
        public Mash([NotNull] IEqualityComparer<TKey> equalityComparer)
        {
            Validate.ArgumentNotNull(nameof(equalityComparer), equalityComparer);

            _equalityComparer = equalityComparer;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Mash{TKey, TValue}"/> class using the default <see cref="IEqualityComparer{T}"/>.
        /// </summary>
        public Mash() : this(EqualityComparer<TKey>.Default)
        {
        }

        /// <summary>
        /// Returns an enumerator that iterates through the values stored in this <see cref="Mash{TKey,TValue}"/>.
        /// </summary>
        /// <returns>
        /// An enumerator that can be used to iterate through the collection.
        /// </returns>
        /// <exception cref="NotImplementedException"></exception>
        public IEnumerator<TValue> GetEnumerator()
        {
            var ver = _ver;
            // ReSharper disable once SwitchStatementMissingSomeCases
            switch (_state & StorageState.ValueMask)
            {
                case 0:
                    Debug.Assert(_valueObj == null);
                    if (ver != _ver)
                    {
                        ThrowCollectionChangedDuringEnumeration();
                    }
                    yield break;
                case StorageState.ValueIsOneObject:
                    Debug.Assert(_valueObj == null || _valueObj is TValue);
                    if (ver != _ver)
                    {
                        ThrowCollectionChangedDuringEnumeration();
                    }
                    yield return (TValue) _valueObj;

                    break;
                case StorageState.ValuesInTwoElementArray:
                    Debug.Assert(_valueObj is TValue[]);
                    var array = (TValue[]) _valueObj;
                    Debug.Assert(array.Length == 2);
                    if (ver != _ver)
                    {
                        ThrowCollectionChangedDuringEnumeration();
                    }
                    yield return array[0];

                    if (ver != _ver)
                    {
                        ThrowCollectionChangedDuringEnumeration();
                    }
                    yield return array[1];
                    break;
                case StorageState.ValuesInList:
                    Debug.Assert(_valueObj is IList<TValue>);
                    var list = (IList<TValue>) _valueObj;
                    Debug.Assert(list.Count > 2);

                    // ReSharper disable once ForCanBeConvertedToForeach
                    for (var i = 0; i < list.Count; i++)
                    {
                        if (ver != _ver)
                        {
                            ThrowCollectionChangedDuringEnumeration();
                        }
                        yield return list[i];
                    }
                    break;
                default:
                    Debug.Fail($"Invalid mash state detected: {_state}.");
                    break;
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the values stored in this <see cref="Mash{TKey,TValue}"/>.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Adds an item to the <see cref="Mash{TKey,TValue}" />'s value collection.
        /// </summary>
        /// <param name="item">The object to add to this <see cref="Mash{TKey,TValue}"/>.</param>
        public void Add(TValue item)
        {
            // ReSharper disable once SwitchStatementMissingSomeCases
            switch (_state & StorageState.ValueMask)
            {
                case 0:
                    Debug.Assert(_valueObj == null);

                    _ver++;
                    _state = (_state & StorageState.ChildrenMask) | StorageState.ValueIsOneObject;
                    _valueObj = item;

                    break;
                case StorageState.ValueIsOneObject:
                    Debug.Assert(_valueObj == null || _valueObj is TValue);

                    _ver++;
                    _state = (_state & StorageState.ChildrenMask) | StorageState.ValuesInTwoElementArray;
                    _valueObj = new[]
                    {
                        (TValue) _valueObj,
                        item
                    };

                    break;
                case StorageState.ValuesInTwoElementArray:
                    Debug.Assert(_valueObj is TValue[]);
                    var array = (TValue[]) _valueObj;
                    Debug.Assert(array.Length == 2);

                    _ver++;
                    _state = (_state & StorageState.ChildrenMask) | StorageState.ValuesInList;
                    _valueObj = new List<TValue>
                    {
                        array[0],
                        array[1],
                        item
                    };

                    break;
                case StorageState.ValuesInList:
                    Debug.Assert(_valueObj is IList<TValue>);
                    var list = (IList<TValue>) _valueObj;
                    Debug.Assert(list.Count > 2);

                    _ver++;
                    list.Add(item);

                    break;
                default:
                    Debug.Fail($"Invalid mash state detected: {_state}.");
                    break;
            }
        }

        /// <summary>
        /// Removes all items from the this <see cref="Mash{TKey,TValue}"/>'s value collection.
        /// </summary>
        public void Clear()
        {
            _ver++;
            _state = _state & StorageState.ChildrenMask;
            _valueObj = null;

            Debug.Assert(Count == 0);
        }

        /// <summary>
        /// Determines whether the <see cref="Mash{TKey,TValue}"/> contains a specific value.
        /// </summary>
        /// <param name="item">The object to locate in the mash.</param>
        /// <returns>
        /// <c>true</c> if <paramref name="item" /> is found in the <see cref="Mash{TKey,TValue}" />; otherwise, <c>false</c>.
        /// </returns>
        public bool Contains(TValue item) => IndexOf(item) > -1;

        /// <summary>
        /// Copies the elements of the <see cref="Mash{TKey,TValue}" />'s value collection to an <see cref="T:System.Array" />, 
        /// starting at a particular <see cref="T:System.Array" /> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied 
        /// from <see cref="Mash{TKey,TValue}" />. The <see cref="T:System.Array" /> must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in <paramref name="array" /> at which copying begins.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="array"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the destination <paramref name="array"/> does not have enough 
        /// space to hold the contents of the set.</exception>
        public void CopyTo([NotNull] TValue[] array, int arrayIndex)
        {
            Validate.ArgumentNotNull(nameof(array), array);
            Validate.ArgumentGreaterThanOrEqualToZero(nameof(arrayIndex), arrayIndex);

            // ReSharper disable once SwitchStatementMissingSomeCases
            switch (_state & StorageState.ValueMask)
            {
                case 0:
                    Debug.Assert(_valueObj == null);
                    Validate.ArgumentLessThanOrEqualTo(nameof(arrayIndex), 0, array.Length - arrayIndex);

                    break;
                case StorageState.ValueIsOneObject:
                    Debug.Assert(_valueObj == null || _valueObj is TValue);
                    Validate.ArgumentLessThanOrEqualTo(nameof(arrayIndex), 1, array.Length - arrayIndex);

                    array[arrayIndex] = (TValue) _valueObj;
                    break;
                case StorageState.ValuesInTwoElementArray:
                    Debug.Assert(_valueObj is TValue[]);
                    var oArray = (TValue[]) _valueObj;
                    Debug.Assert(oArray.Length == 2);
                    Validate.ArgumentLessThanOrEqualTo(nameof(arrayIndex), 2, array.Length - arrayIndex);

                    array[arrayIndex] = oArray[0];
                    array[arrayIndex + 1] = oArray[1];
                    break;
                case StorageState.ValuesInList:
                    Debug.Assert(_valueObj is IList<TValue>);
                    var list = (IList<TValue>) _valueObj;
                    Debug.Assert(list.Count > 2);
                    Validate.ArgumentLessThanOrEqualTo(nameof(arrayIndex), list.Count, array.Length - arrayIndex);
                    list.CopyTo(array, arrayIndex);
                    break;
                default:
                    Debug.Fail($"Invalid mash state detected: {_state}.");
                    break;
            }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="Mash{TKey,TValue}"/>'s value collection.
        /// </summary>
        /// <param name="item">The object to remove from the value collection.</param>
        /// <returns>
        /// <c>true</c> if <paramref name="item" /> was successfully removed; otherwise, <c>false</c>.
        /// </returns>
        public bool Remove(TValue item)
        {
            // ReSharper disable once SwitchStatementMissingSomeCases
            switch (_state & StorageState.ValueMask)
            {
                case 0:
                    Debug.Assert(_valueObj == null);
                    return false;
                case StorageState.ValueIsOneObject:
                    Debug.Assert(_valueObj == null || _valueObj is TValue);
                    if (!Equals((TValue) _valueObj, item))
                    {
                        return false;
                    }

                    _ver++;
                    _state = _state & StorageState.ChildrenMask;
                    _valueObj = null;

                    return true;
                case StorageState.ValuesInTwoElementArray:
                    Debug.Assert(_valueObj is TValue[]);
                    var array = (TValue[]) _valueObj;
                    Debug.Assert(array.Length == 2);
                    if (Equals(array[0], item))
                    {
                        _ver++;
                        _state = (_state & StorageState.ChildrenMask) | StorageState.ValueIsOneObject;
                        _valueObj = array[1];

                        return true;
                    }
                    if (Equals(array[1], item))
                    {
                        _ver++;
                        _state = (_state & StorageState.ChildrenMask) | StorageState.ValueIsOneObject;
                        _valueObj = array[0];

                        return true;
                    }
                    return false;
                case StorageState.ValuesInList:
                    Debug.Assert(_valueObj is IList<TValue>);
                    var list = (IList<TValue>) _valueObj;
                    Debug.Assert(list.Count > 2);
                    var removedFromList = list.Remove(item);
                    Debug.Assert(list.Count >= 2);
                    if (removedFromList)
                    {
                        _ver++;
                        if (list.Count == 2)
                        {
                            _state = (_state & StorageState.ChildrenMask) | StorageState.ValuesInTwoElementArray;
                            _valueObj = new[]
                            {
                                list[0],
                                list[1]
                            };
                        }
                    }
                    return removedFromList;
                default:
                    Debug.Fail($"Invalid mash state detected: {_state}.");
                    break;
            }

            return false;
        }

        /// <summary>
        /// Gets the number of elements contained in the <see cref="Mash{TKey,TValue}" />'s value collection.
        /// </summary>
        public int Count
        {
            get
            {
                // ReSharper disable once SwitchStatementMissingSomeCases
                switch (_state & StorageState.ValueMask)
                {
                    case 0:
                        Debug.Assert(_valueObj == null);
                        return 0;
                    case StorageState.ValueIsOneObject:
                        Debug.Assert(_valueObj == null || _valueObj is TValue);
                        return 1;
                    case StorageState.ValuesInTwoElementArray:
                        Debug.Assert(_valueObj is TValue[]);
                        var array = (TValue[]) _valueObj;
                        Debug.Assert(array.Length == 2);
                        return 2;
                    case StorageState.ValuesInList:
                        Debug.Assert(_valueObj is IList<TValue>);
                        var list = (IList<TValue>) _valueObj;
                        Debug.Assert(list.Count > 2);
                        return list.Count;
                    default:
                        Debug.Fail($"Invalid mash state detected: {_state}.");
                        break;
                }

                return 0;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="Mash{TKey,TValue}" /> is read-only.
        /// </summary>
        /// <value>
        /// This property is always <c>false</c>.
        /// </value>
        public bool IsReadOnly => false;

        /// <summary>
        /// Determines the index of a specific item in the <see cref="Mash{TKey,TValue}" />'s value collection.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="Mash{TKey,TValue}" />'s value collection.</param>
        /// <returns>
        /// The index of <paramref name="item" /> if found in the list; otherwise, -1.
        /// </returns>
        public int IndexOf(TValue item)
        {
            // ReSharper disable once SwitchStatementMissingSomeCases
            switch (_state & StorageState.ValueMask)
            {
                case 0:
                    Debug.Assert(_valueObj == null);
                    return -1;
                case StorageState.ValueIsOneObject:
                    Debug.Assert(_valueObj == null || _valueObj is TValue);
                    if (Equals((TValue) _valueObj, item))
                    {
                        return 0;
                    }
                    return -1;
                case StorageState.ValuesInTwoElementArray:
                    Debug.Assert(_valueObj is TValue[]);
                    var array = (TValue[]) _valueObj;
                    Debug.Assert(array.Length == 2);
                    if (Equals(array[0], item))
                    {
                        return 0;
                    }
                    if (Equals(array[1], item))
                    {
                        return 1;
                    }
                    return -1;
                case StorageState.ValuesInList:
                    Debug.Assert(_valueObj is IList<TValue>);
                    var list = (IList<TValue>) _valueObj;
                    Debug.Assert(list.Count > 2);
                    return list.IndexOf(item);
                default:
                    Debug.Fail($"Invalid mash state detected: {_state}.");
                    break;
            }

            return -1;
        }

        /// <summary>
        /// Inserts an item to the <see cref="Mash{TKey,TValue}" />'s value collection at the specified <paramref name="index"/>.
        /// </summary>
        /// <param name="index">The zero-based index at which <paramref name="item" /> should be inserted.</param>
        /// <param name="item">The object to insert into the <see cref="Mash{TKey,TValue}" />'s value collection.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="index"/> is out of bounds.</exception>
        public void Insert(int index, TValue item)
        {
            Validate.ArgumentGreaterThanOrEqualToZero(nameof(index), index);

            // ReSharper disable once SwitchStatementMissingSomeCases
            switch (_state & StorageState.ValueMask)
            {
                case 0:
                    Debug.Assert(_valueObj == null);
                    Validate.ArgumentLessThanOrEqualTo(nameof(index), index, 0);

                    _ver++;
                    _state = (_state & StorageState.ChildrenMask) | StorageState.ValueIsOneObject;
                    _valueObj = item;

                    break;
                case StorageState.ValueIsOneObject:
                    Debug.Assert(_valueObj == null || _valueObj is TValue);

                    Validate.ArgumentLessThanOrEqualTo(nameof(index), index, 1);

                    _ver++;
                    _state = (_state & StorageState.ChildrenMask) | StorageState.ValuesInTwoElementArray;

                    if (index == 0)
                    {
                        _valueObj = new[]
                        {
                            item,
                            (TValue) _valueObj
                        };
                    }
                    else
                    {
                        Debug.Assert(index == 1);
                        _valueObj = new[]
                        {
                            (TValue) _valueObj,
                            item
                        };
                    }

                    break;
                case StorageState.ValuesInTwoElementArray:
                    Debug.Assert(_valueObj is TValue[]);
                    var array = (TValue[]) _valueObj;
                    Debug.Assert(array.Length == 2);

                    Validate.ArgumentLessThanOrEqualTo(nameof(index), index, 2);

                    _ver++;
                    _state = (_state & StorageState.ChildrenMask) | StorageState.ValuesInList;

                    switch (index)
                    {
                        case 0:
                            _valueObj = new List<TValue>
                            {
                                item,
                                array[0],
                                array[1]
                            };
                            break;
                        case 1:
                            _valueObj = new List<TValue>
                            {
                                array[0],
                                item,
                                array[1]
                            };
                            break;
                        default:
                            Debug.Assert(index == 2);
                            _valueObj = new List<TValue>
                            {
                                array[0],
                                array[1],
                                item
                            };
                            break;
                    }

                    break;
                case StorageState.ValuesInList:
                    Debug.Assert(_valueObj is IList<TValue>);
                    var list = (IList<TValue>) _valueObj;
                    Debug.Assert(list.Count > 2);

                    Validate.ArgumentLessThanOrEqualTo(nameof(index), index, list.Count);

                    _ver++;
                    list.Insert(index, item);
                    Debug.Assert(list.Count > 2);

                    break;
                default:
                    Debug.Fail($"Invalid mash state detected: {_state}.");
                    break;
            }
        }

        /// <summary>
        /// Removes the <see cref="Mash{TKey,TValue}" />'s item at the specified <paramref name="index"/>.
        /// </summary>
        /// <param name="index">The zero-based index of the item to remove.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="index"/> is out of bounds.</exception>
        public void RemoveAt(int index)
        {
            Validate.ArgumentGreaterThanOrEqualToZero(nameof(index), index);

            // ReSharper disable once SwitchStatementMissingSomeCases
            switch (_state & StorageState.ValueMask)
            {
                case 0:
                    Debug.Assert(_valueObj == null);
                    Validate.ArgumentLessThan(nameof(index), index, 0);
                    break;
                case StorageState.ValueIsOneObject:
                    Debug.Assert(_valueObj == null || _valueObj is TValue);

                    Validate.ArgumentLessThan(nameof(index), index, 1);

                    _ver++;
                    _state = _state & StorageState.ChildrenMask;
                    _valueObj = null;

                    break;
                case StorageState.ValuesInTwoElementArray:
                    Debug.Assert(_valueObj is TValue[]);
                    var array = (TValue[]) _valueObj;
                    Debug.Assert(array.Length == 2);

                    Validate.ArgumentLessThan(nameof(index), index, 2);

                    _ver++;
                    _state = (_state & StorageState.ChildrenMask) | StorageState.ValueIsOneObject;

                    switch (index)
                    {
                        case 0:
                            _valueObj = array[1];
                            break;
                        default:
                            Debug.Assert(index == 1);
                            _valueObj = array[0];
                            break;
                    }

                    break;
                case StorageState.ValuesInList:
                    Debug.Assert(_valueObj is IList<TValue>);
                    var list = (IList<TValue>) _valueObj;
                    Debug.Assert(list.Count > 2);

                    Validate.ArgumentLessThan(nameof(index), index, list.Count);

                    _ver++;
                    list.RemoveAt(index);
                    Debug.Assert(list.Count >= 2);

                    if (list.Count == 2)
                    {
                        _state = (_state & StorageState.ChildrenMask) | StorageState.ValuesInTwoElementArray;
                        _valueObj = new[]
                        {
                            list[0],
                            list[1]
                        };
                    }

                    break;
                default:
                    Debug.Fail($"Invalid mash state detected: {_state}.");
                    break;
            }
        }

        /// <summary>
        /// Gets or sets the value in the <see cref="Mash{TKey,TValue}"/>'s value collection at the specified <paramref name="index"/>.
        /// </summary>
        /// <value>
        /// The value at the specified index.
        /// </value>
        /// <param name="index">The index.</param>
        /// <returns>The value stored in the <see cref="Mash{TKey,TValue}"/>'s value collection at the specified index.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="index"/> is out of bounds.</exception>
        public TValue this[int index]
        {
            get
            {
                Validate.ArgumentGreaterThanOrEqualToZero(nameof(index), index);

                // ReSharper disable once SwitchStatementMissingSomeCases
                switch (_state & StorageState.ValueMask)
                {
                    case 0:
                        Debug.Assert(_valueObj == null);
                        Validate.ArgumentLessThan(nameof(index), index, 0);

                        break;
                    case StorageState.ValueIsOneObject:
                        Debug.Assert(_valueObj == null || _valueObj is TValue);

                        Validate.ArgumentLessThan(nameof(index), index, 1);
                        Debug.Assert(index == 0);
                        return (TValue) _valueObj;
                    case StorageState.ValuesInTwoElementArray:
                        Debug.Assert(_valueObj is TValue[]);
                        var array = (TValue[]) _valueObj;
                        Debug.Assert(array.Length == 2);

                        Validate.ArgumentLessThan(nameof(index), index, 2);
                        Debug.Assert(index == 0 || index == 1);

                        return array[index];
                    case StorageState.ValuesInList:
                        Debug.Assert(_valueObj is IList<TValue>);
                        var list = (IList<TValue>) _valueObj;
                        Debug.Assert(list.Count > 2);

                        Validate.ArgumentLessThan(nameof(index), index, list.Count);
                        Debug.Assert(index >= 0 && index < list.Count);
                        return list[index];
                    default:
                        Debug.Fail($"Invalid mash state detected: {_state}.");
                        break;
                }

                return default(TValue);
            }
            set
            {
                Validate.ArgumentGreaterThanOrEqualToZero(nameof(index), index);

                // ReSharper disable once SwitchStatementMissingSomeCases
                switch (_state & StorageState.ValueMask)
                {
                    case 0:
                        Debug.Assert(_valueObj == null);
                        Validate.ArgumentLessThan(nameof(index), index, 0);

                        break;
                    case StorageState.ValueIsOneObject:
                        Debug.Assert(_valueObj == null || _valueObj is TValue);

                        Validate.ArgumentLessThan(nameof(index), index, 1);
                        Debug.Assert(index == 0);

                        _ver++;
                        _valueObj = value;
                        break;
                    case StorageState.ValuesInTwoElementArray:
                        Debug.Assert(_valueObj is TValue[]);
                        var array = (TValue[]) _valueObj;
                        Debug.Assert(array.Length == 2);

                        Validate.ArgumentLessThan(nameof(index), index, 2);
                        Debug.Assert(index == 0 || index == 1);

                        _ver++;
                        array[index] = value;
                        break;
                    case StorageState.ValuesInList:
                        Debug.Assert(_valueObj is IList<TValue>);
                        var list = (IList<TValue>) _valueObj;
                        Debug.Assert(list.Count > 2);

                        Validate.ArgumentLessThan(nameof(index), index, list.Count);
                        Debug.Assert(index >= 0 && index < list.Count);

                        _ver++;
                        list[index] = value;
                        break;
                    default:
                        Debug.Fail($"Invalid mash state detected: {_state}.");
                        break;
                }
            }
        }

        /// <summary>
        /// Gets or sets the value stored by this <see cref="Mash{TKey,TValue}"/>.
        /// </summary>
        /// <value>
        /// The value stored by this mash.
        /// </value>
        /// <remarks>
        /// The <see cref="Mash{TKey,TValue}"/> allows for storage of collection of items. The <see cref="Value"/> property
        /// acts as a simplified way to access the element with zero-index in this collection. If the collection is empty and a setter
        /// is accessed, the value is added into collection. Given how the <see cref="Mash{TKey,TValue}"/> class stores the value collection,
        /// use the <see cref="Value"/> property if you only need to store one element in the <see cref="Mash{TKey,TValue}"/>.
        /// </remarks>
        public TValue Value
        {
            get
            {
                // ReSharper disable once SwitchStatementMissingSomeCases
                switch (_state & StorageState.ValueMask)
                {
                    case 0:
                        Debug.Assert(_valueObj == null);
                        break;
                    case StorageState.ValueIsOneObject:
                        Debug.Assert(_valueObj == null || _valueObj is TValue);
                        return (TValue) _valueObj;
                    case StorageState.ValuesInTwoElementArray:
                        Debug.Assert(_valueObj is TValue[]);
                        var array = (TValue[]) _valueObj;
                        Debug.Assert(array.Length == 2);
                        return array[0];
                    case StorageState.ValuesInList:
                        Debug.Assert(_valueObj is IList<TValue>);
                        var list = (IList<TValue>) _valueObj;
                        Debug.Assert(list.Count > 2);
                        return list[0];
                    default:
                        Debug.Fail($"Invalid mash state detected: {_state}.");
                        break;
                }

                return default(TValue);
            }
            set
            {
                // ReSharper disable once SwitchStatementMissingSomeCases
                switch (_state & StorageState.ValueMask)
                {
                    case 0:
                        Debug.Assert(_valueObj == null);
                        _ver++;
                        _state = (_state & StorageState.ChildrenMask) | StorageState.ValueIsOneObject;
                        _valueObj = value;
                        break;
                    case StorageState.ValueIsOneObject:
                        Debug.Assert(_valueObj == null || _valueObj is TValue);
                        _ver++;
                        _valueObj = value;
                        break;
                    case StorageState.ValuesInTwoElementArray:
                        Debug.Assert(_valueObj is TValue[]);
                        var array = (TValue[]) _valueObj;
                        Debug.Assert(array.Length == 2);
                        _ver++;
                        array[0] = value;
                        break;
                    case StorageState.ValuesInList:
                        Debug.Assert(_valueObj is IList<TValue>);
                        var list = (IList<TValue>) _valueObj;
                        Debug.Assert(list.Count > 2);
                        _ver++;
                        list[0] = value;
                        break;
                    default:
                        Debug.Fail($"Invalid mash state detected: {_state}.");
                        break;
                }
            }
        }

        /// <summary>
        /// Gets the linked <see cref="Mash{TKey, TValue}"/> with the specified key.
        /// </summary>
        /// <remarks>
        /// This method serves as an alternative to the indexer in cases when <typeparamref name="TKey"/> and <typeparamref name="TValue"/>
        /// are of the same type (and method overloading fails).
        /// </remarks>
        /// <param name="key">The key.</param>
        /// <returns>The linked <see cref="Mash{TKey,TValue}"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="key"/> is <c>null</c>.</exception>
        [NotNull]
        public Mash<TKey, TValue> GetLinked([NotNull] TKey key)
        {
            Validate.ArgumentNotNull(nameof(key), key);

            // ReSharper disable once SwitchStatementMissingSomeCases
            switch (_state & StorageState.ChildrenMask)
            {
                case 0:
                    Debug.Assert(_childrenObj == null);
                    _state = (_state & StorageState.ValueMask) | StorageState.HasOneChildInATuple;
                    var subMash = new Mash<TKey, TValue>(_equalityComparer);
                    _childrenObj = Tuple.Create(key, subMash);
                    return subMash;

                case StorageState.HasOneChildInATuple:
                    Debug.Assert(_childrenObj is Tuple<TKey, Mash<TKey, TValue>>);
                    var tuple = (Tuple<TKey, Mash<TKey, TValue>>) _childrenObj;
                    if (_equalityComparer.Equals(tuple.Item1, key))
                    {
                        return tuple.Item2;
                    }

                    _state = (_state & StorageState.ValueMask) | StorageState.HasTwoChildrenInKeyValuePairArray;
                    subMash = new Mash<TKey, TValue>(_equalityComparer);
                    _childrenObj = new[]
                    {
                        new KeyValuePair<TKey, Mash<TKey, TValue>>(key, subMash),
                        new KeyValuePair<TKey, Mash<TKey, TValue>>(tuple.Item1, tuple.Item2),
                    };

                    return subMash;
                case StorageState.HasTwoChildrenInKeyValuePairArray:
                    Debug.Assert(_childrenObj is KeyValuePair<TKey, Mash<TKey, TValue>>[]);
                    var array2 = (KeyValuePair<TKey, Mash<TKey, TValue>>[]) _childrenObj;
                    Debug.Assert(array2.Length == 2);

                    if (_equalityComparer.Equals(array2[0].Key, key))
                    {
                        return array2[0].Value;
                    }
                    if (_equalityComparer.Equals(array2[1].Key, key))
                    {
                        return array2[1].Value;
                    }

                    _state = (_state & StorageState.ValueMask) | StorageState.HasThreeChildrenInKeyValuePairArray;
                    subMash = new Mash<TKey, TValue>(_equalityComparer);
                    _childrenObj = new[]
                    {
                        new KeyValuePair<TKey, Mash<TKey, TValue>>(key, subMash),
                        array2[0],
                        array2[1]
                    };

                    return subMash;
                case StorageState.HasThreeChildrenInKeyValuePairArray:
                    Debug.Assert(_childrenObj is KeyValuePair<TKey, Mash<TKey, TValue>>[]);
                    var array3 = (KeyValuePair<TKey, Mash<TKey, TValue>>[]) _childrenObj;
                    Debug.Assert(array3.Length == 3);

                    if (_equalityComparer.Equals(array3[0].Key, key))
                    {
                        return array3[0].Value;
                    }
                    if (_equalityComparer.Equals(array3[1].Key, key))
                    {
                        return array3[1].Value;
                    }
                    if (_equalityComparer.Equals(array3[2].Key, key))
                    {
                        return array3[2].Value;
                    }

                    _state = (_state & StorageState.ValueMask) | StorageState.HasFourChildrenInKeyValuePairArray;
                    subMash = new Mash<TKey, TValue>(_equalityComparer);
                    _childrenObj = new[]
                    {
                        new KeyValuePair<TKey, Mash<TKey, TValue>>(key, subMash),
                        array3[0],
                        array3[1],
                        array3[2]
                    };

                    return subMash;
                case StorageState.HasFourChildrenInKeyValuePairArray:
                    Debug.Assert(_childrenObj is KeyValuePair<TKey, Mash<TKey, TValue>>[]);
                    var array4 = (KeyValuePair<TKey, Mash<TKey, TValue>>[]) _childrenObj;
                    Debug.Assert(array4.Length == 4);

                    if (_equalityComparer.Equals(array4[0].Key, key))
                    {
                        return array4[0].Value;
                    }
                    if (_equalityComparer.Equals(array4[1].Key, key))
                    {
                        return array4[1].Value;
                    }
                    if (_equalityComparer.Equals(array4[2].Key, key))
                    {
                        return array4[2].Value;
                    }
                    if (_equalityComparer.Equals(array4[3].Key, key))
                    {
                        return array4[3].Value;
                    }

                    _state = (_state & StorageState.ValueMask) | StorageState.HasFiveChildrenInKeyValuePairArray;
                    subMash = new Mash<TKey, TValue>(_equalityComparer);
                    _childrenObj = new[]
                    {
                        new KeyValuePair<TKey, Mash<TKey, TValue>>(key, subMash),
                        array4[0],
                        array4[1],
                        array4[2],
                        array4[3]
                    };

                    return subMash;
                case StorageState.HasFiveChildrenInKeyValuePairArray:
                    Debug.Assert(_childrenObj is KeyValuePair<TKey, Mash<TKey, TValue>>[]);
                    var array5 = (KeyValuePair<TKey, Mash<TKey, TValue>>[]) _childrenObj;
                    Debug.Assert(array5.Length == 5);

                    if (_equalityComparer.Equals(array5[0].Key, key))
                    {
                        return array5[0].Value;
                    }
                    if (_equalityComparer.Equals(array5[1].Key, key))
                    {
                        return array5[1].Value;
                    }
                    if (_equalityComparer.Equals(array5[2].Key, key))
                    {
                        return array5[2].Value;
                    }
                    if (_equalityComparer.Equals(array5[3].Key, key))
                    {
                        return array5[3].Value;
                    }
                    if (_equalityComparer.Equals(array5[4].Key, key))
                    {
                        return array5[4].Value;
                    }

                    _state = (_state & StorageState.ValueMask) | StorageState.HasManyChildrenInAHashTable;
                    subMash = new Mash<TKey, TValue>(_equalityComparer);
                    _childrenObj = new Dictionary<TKey, Mash<TKey, TValue>>
                    {
                        {array5[0].Key, array5[0].Value},
                        {array5[1].Key, array5[1].Value},
                        {array5[2].Key, array5[2].Value},
                        {array5[3].Key, array5[3].Value},
                        {array5[4].Key, array5[4].Value},
                        {key, subMash}
                    };

                    return subMash;
                case StorageState.HasManyChildrenInAHashTable:
                    Debug.Assert(_childrenObj is IDictionary<TKey, Mash<TKey, TValue>>);
                    var dict = (IDictionary<TKey, Mash<TKey, TValue>>) _childrenObj;

                    if (!dict.TryGetValue(key, out subMash))
                    {
                        subMash = new Mash<TKey, TValue>(_equalityComparer);
                        dict.Add(key, subMash);
                    }

                    return subMash;
            }

            Debug.Fail($"Invalid mash state detected: {_state}.");
            return null;
        }

        /// <summary>
        /// Links a given <paramref name="mash"/> with this <see cref="Mash{TKey,TValue}"/> using the supplied <paramref name="key"/>.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="mash">The mash to link.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="key"/> or <paramref name="mash"/> is <c>null</c>.</exception>
        public void Link([NotNull] TKey key, [NotNull] Mash<TKey, TValue> mash)
        {
            Validate.ArgumentNotNull(nameof(mash), mash);
            Validate.ArgumentNotNull(nameof(key), key);

            // ReSharper disable once SwitchStatementMissingSomeCases
            switch (_state & StorageState.ChildrenMask)
            {
                case 0:
                    Debug.Assert(_childrenObj == null);
                    _state = (_state & StorageState.ValueMask) | StorageState.HasOneChildInATuple;
                    _childrenObj = Tuple.Create(key, mash);
                    break;

                case StorageState.HasOneChildInATuple:
                    Debug.Assert(_childrenObj is Tuple<TKey, Mash<TKey, TValue>>);
                    var tuple = (Tuple<TKey, Mash<TKey, TValue>>) _childrenObj;
                    if (_equalityComparer.Equals(tuple.Item1, key))
                    {
                        _childrenObj = Tuple.Create(key, mash);
                    }
                    else
                    {
                        _state = (_state & StorageState.ValueMask) | StorageState.HasTwoChildrenInKeyValuePairArray;
                        _childrenObj = new[]
                        {
                            new KeyValuePair<TKey, Mash<TKey, TValue>>(key, mash),
                            new KeyValuePair<TKey, Mash<TKey, TValue>>(tuple.Item1, tuple.Item2),
                        };
                    }
                    break;

                case StorageState.HasTwoChildrenInKeyValuePairArray:
                    Debug.Assert(_childrenObj is KeyValuePair<TKey, Mash<TKey, TValue>>[]);
                    var array2 = (KeyValuePair<TKey, Mash<TKey, TValue>>[]) _childrenObj;
                    Debug.Assert(array2.Length == 2);

                    var mashKvp = new KeyValuePair<TKey, Mash<TKey, TValue>>(key, mash);
                    if (_equalityComparer.Equals(array2[0].Key, key))
                    {
                        array2[0] = mashKvp;
                    }
                    else if (_equalityComparer.Equals(array2[1].Key, key))
                    {
                        array2[1] = mashKvp;
                    }
                    else
                    {
                        _state = (_state & StorageState.ValueMask) | StorageState.HasThreeChildrenInKeyValuePairArray;
                        _childrenObj = new[]
                        {
                            mashKvp,
                            array2[0],
                            array2[1]
                        };
                    }

                    break;
                case StorageState.HasThreeChildrenInKeyValuePairArray:
                    Debug.Assert(_childrenObj is KeyValuePair<TKey, Mash<TKey, TValue>>[]);
                    var array3 = (KeyValuePair<TKey, Mash<TKey, TValue>>[]) _childrenObj;
                    Debug.Assert(array3.Length == 3);

                    mashKvp = new KeyValuePair<TKey, Mash<TKey, TValue>>(key, mash);
                    if (_equalityComparer.Equals(array3[0].Key, key))
                    {
                        array3[0] = mashKvp;
                    }
                    else if (_equalityComparer.Equals(array3[1].Key, key))
                    {
                        array3[1] = mashKvp;
                    }
                    else if (_equalityComparer.Equals(array3[2].Key, key))
                    {
                        array3[2] = mashKvp;
                    }
                    else
                    {
                        _state = (_state & StorageState.ValueMask) | StorageState.HasFourChildrenInKeyValuePairArray;
                        _childrenObj = new[]
                        {
                            mashKvp,
                            array3[0],
                            array3[1],
                            array3[2]
                        };
                    }

                    break;
                case StorageState.HasFourChildrenInKeyValuePairArray:
                    Debug.Assert(_childrenObj is KeyValuePair<TKey, Mash<TKey, TValue>>[]);
                    var array4 = (KeyValuePair<TKey, Mash<TKey, TValue>>[]) _childrenObj;
                    Debug.Assert(array4.Length == 4);

                    mashKvp = new KeyValuePair<TKey, Mash<TKey, TValue>>(key, mash);
                    if (_equalityComparer.Equals(array4[0].Key, key))
                    {
                        array4[0] = mashKvp;
                    }
                    else if (_equalityComparer.Equals(array4[1].Key, key))
                    {
                        array4[1] = mashKvp;
                    }
                    else if (_equalityComparer.Equals(array4[2].Key, key))
                    {
                        array4[2] = mashKvp;
                    }
                    else if (_equalityComparer.Equals(array4[3].Key, key))
                    {
                        array4[3] = mashKvp;
                    }
                    else
                    {
                        _state = (_state & StorageState.ValueMask) | StorageState.HasFiveChildrenInKeyValuePairArray;
                        _childrenObj = new[]
                        {
                            mashKvp,
                            array4[0],
                            array4[1],
                            array4[2],
                            array4[3]
                        };
                    }
                    break;
                case StorageState.HasFiveChildrenInKeyValuePairArray:
                    Debug.Assert(_childrenObj is KeyValuePair<TKey, Mash<TKey, TValue>>[]);
                    var array5 = (KeyValuePair<TKey, Mash<TKey, TValue>>[]) _childrenObj;
                    Debug.Assert(array5.Length == 5);

                    mashKvp = new KeyValuePair<TKey, Mash<TKey, TValue>>(key, mash);
                    if (_equalityComparer.Equals(array5[0].Key, key))
                    {
                        array5[0] = mashKvp;
                    }
                    else if (_equalityComparer.Equals(array5[1].Key, key))
                    {
                        array5[1] = mashKvp;
                    }
                    else if (_equalityComparer.Equals(array5[2].Key, key))
                    {
                        array5[2] = mashKvp;
                    }
                    else if (_equalityComparer.Equals(array5[3].Key, key))
                    {
                        array5[3] = mashKvp;
                    }
                    else if (_equalityComparer.Equals(array5[4].Key, key))
                    {
                        array5[4] = mashKvp;
                    }
                    else
                    {
                        _state = (_state & StorageState.ValueMask) | StorageState.HasManyChildrenInAHashTable;
                        _childrenObj = new Dictionary<TKey, Mash<TKey, TValue>>
                        {
                            {array5[0].Key, array5[0].Value},
                            {array5[1].Key, array5[1].Value},
                            {array5[2].Key, array5[2].Value},
                            {array5[3].Key, array5[3].Value},
                            {array5[4].Key, array5[4].Value},
                            {key, mash}
                        };
                    }
                    break;
                case StorageState.HasManyChildrenInAHashTable:
                    Debug.Assert(_childrenObj is IDictionary<TKey, Mash<TKey, TValue>>);
                    var dict = (IDictionary<TKey, Mash<TKey, TValue>>) _childrenObj;

                    dict[key] = mash;
                    break;
                default:
                    Debug.Fail($"Invalid mash state detected: {_state}.");
                    break;
            }
        }

        /// <summary>
        /// Un-links the <see cref="Mash{TKey,TValue}"/> with the specified key from this <see cref="Mash{TKey,TValue}"/>.
        /// </summary>
        /// <param name="key">The key of the <see cref="Mash{TKey,TValue}"/> to un-link.</param>
        /// <returns><c>true</c> if the mash was un-linked; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="key"/> is <c>null</c>.</exception>
        public bool Unlink([NotNull] TKey key)
        {
            Validate.ArgumentNotNull(nameof(key), key);

            // ReSharper disable once SwitchStatementMissingSomeCases
            switch (_state & StorageState.ChildrenMask)
            {
                case 0:
                    Debug.Assert(_childrenObj == null);
                    break;

                case StorageState.HasOneChildInATuple:
                    Debug.Assert(_childrenObj is Tuple<TKey, Mash<TKey, TValue>>);
                    var tuple = (Tuple<TKey, Mash<TKey, TValue>>) _childrenObj;
                    if (_equalityComparer.Equals(tuple.Item1, key))
                    {
                        _state = _state & StorageState.ValueMask;
                        _childrenObj = null;

                        return true;
                    }

                    return false;

                case StorageState.HasTwoChildrenInKeyValuePairArray:
                    Debug.Assert(_childrenObj is KeyValuePair<TKey, Mash<TKey, TValue>>[]);
                    var array2 = (KeyValuePair<TKey, Mash<TKey, TValue>>[]) _childrenObj;
                    Debug.Assert(array2.Length == 2);

                    if (_equalityComparer.Equals(array2[0].Key, key))
                    {
                        _state = (_state & StorageState.ValueMask) | StorageState.HasOneChildInATuple;
                        _childrenObj = Tuple.Create(array2[1].Key, array2[1].Value);
                        return true;
                    }
                    else if (_equalityComparer.Equals(array2[1].Key, key))
                    {
                        _state = (_state & StorageState.ValueMask) | StorageState.HasOneChildInATuple;
                        _childrenObj = Tuple.Create(array2[0].Key, array2[0].Value);
                        return true;
                    }

                    return false;

                case StorageState.HasThreeChildrenInKeyValuePairArray:
                    Debug.Assert(_childrenObj is KeyValuePair<TKey, Mash<TKey, TValue>>[]);
                    var array3 = (KeyValuePair<TKey, Mash<TKey, TValue>>[]) _childrenObj;
                    Debug.Assert(array3.Length == 3);

                    if (_equalityComparer.Equals(array3[0].Key, key))
                    {
                        _state = (_state & StorageState.ValueMask) | StorageState.HasTwoChildrenInKeyValuePairArray;
                        _childrenObj = new[]
                        {
                            array3[1],
                            array3[2]
                        };

                        return true;
                    }
                    else if (_equalityComparer.Equals(array3[1].Key, key))
                    {
                        _state = (_state & StorageState.ValueMask) | StorageState.HasTwoChildrenInKeyValuePairArray;
                        _childrenObj = new[]
                        {
                            array3[0],
                            array3[2]
                        };

                        return true;
                    }
                    else if (_equalityComparer.Equals(array3[2].Key, key))
                    {
                        _state = (_state & StorageState.ValueMask) | StorageState.HasTwoChildrenInKeyValuePairArray;
                        _childrenObj = new[]
                        {
                            array3[0],
                            array3[1]
                        };

                        return true;
                    }

                    return false;
                case StorageState.HasFourChildrenInKeyValuePairArray:
                    Debug.Assert(_childrenObj is KeyValuePair<TKey, Mash<TKey, TValue>>[]);
                    var array4 = (KeyValuePair<TKey, Mash<TKey, TValue>>[]) _childrenObj;
                    Debug.Assert(array4.Length == 4);

                    if (_equalityComparer.Equals(array4[0].Key, key))
                    {
                        _state = (_state & StorageState.ValueMask) | StorageState.HasThreeChildrenInKeyValuePairArray;
                        _childrenObj = new[]
                        {
                            array4[1],
                            array4[2],
                            array4[3],
                        };

                        return true;
                    }
                    else if (_equalityComparer.Equals(array4[1].Key, key))
                    {
                        _state = (_state & StorageState.ValueMask) | StorageState.HasThreeChildrenInKeyValuePairArray;
                        _childrenObj = new[]
                        {
                            array4[0],
                            array4[2],
                            array4[3],
                        };

                        return true;
                    }
                    else if (_equalityComparer.Equals(array4[2].Key, key))
                    {
                        _state = (_state & StorageState.ValueMask) | StorageState.HasThreeChildrenInKeyValuePairArray;
                        _childrenObj = new[]
                        {
                            array4[0],
                            array4[1],
                            array4[3],
                        };

                        return true;
                    }
                    else if (_equalityComparer.Equals(array4[3].Key, key))
                    {
                        _state = (_state & StorageState.ValueMask) | StorageState.HasThreeChildrenInKeyValuePairArray;
                        _childrenObj = new[]
                        {
                            array4[0],
                            array4[1],
                            array4[2],
                        };

                        return true;
                    }

                    return false;
                case StorageState.HasFiveChildrenInKeyValuePairArray:
                    Debug.Assert(_childrenObj is KeyValuePair<TKey, Mash<TKey, TValue>>[]);
                    var array5 = (KeyValuePair<TKey, Mash<TKey, TValue>>[]) _childrenObj;
                    Debug.Assert(array5.Length == 5);

                    if (_equalityComparer.Equals(array5[0].Key, key))
                    {
                        _state = (_state & StorageState.ValueMask) | StorageState.HasFourChildrenInKeyValuePairArray;
                        _childrenObj = new[]
                        {
                            array5[1],
                            array5[2],
                            array5[3],
                            array5[4]
                        };

                        return true;
                    }
                    else if (_equalityComparer.Equals(array5[1].Key, key))
                    {
                        _state = (_state & StorageState.ValueMask) | StorageState.HasFourChildrenInKeyValuePairArray;
                        _childrenObj = new[]
                        {
                            array5[0],
                            array5[2],
                            array5[3],
                            array5[4]
                        };

                        return true;
                    }
                    else if (_equalityComparer.Equals(array5[2].Key, key))
                    {
                        _state = (_state & StorageState.ValueMask) | StorageState.HasFourChildrenInKeyValuePairArray;
                        _childrenObj = new[]
                        {
                            array5[0],
                            array5[1],
                            array5[3],
                            array5[4]
                        };

                        return true;
                    }
                    else if (_equalityComparer.Equals(array5[3].Key, key))
                    {
                        _state = (_state & StorageState.ValueMask) | StorageState.HasFourChildrenInKeyValuePairArray;
                        _childrenObj = new[]
                        {
                            array5[0],
                            array5[1],
                            array5[2],
                            array5[4]
                        };

                        return true;
                    }
                    else if (_equalityComparer.Equals(array5[4].Key, key))
                    {
                        _state = (_state & StorageState.ValueMask) | StorageState.HasFourChildrenInKeyValuePairArray;
                        _childrenObj = new[]
                        {
                            array5[0],
                            array5[1],
                            array5[2],
                            array5[3]
                        };

                        return true;
                    }

                    return false;
                case StorageState.HasManyChildrenInAHashTable:
                    Debug.Assert(_childrenObj is IDictionary<TKey, Mash<TKey, TValue>>);
                    var dict = (IDictionary<TKey, Mash<TKey, TValue>>) _childrenObj;

                    return dict.Remove(key);
            }

            Debug.Fail($"Invalid mash state detected: {_state}.");
            return false;
        }

        /// <summary>
        /// Gets the linked <see cref="Mash{TKey, TValue}"/> with the specified key.
        /// </summary>
        /// <value>
        /// The linked <see cref="Mash{TKey, TValue}"/> associated with the given key.
        /// </value>
        /// <remarks>
        /// This indexer is equivalent to <see cref="GetLinked"/> method.
        /// </remarks>
        /// <param name="key">The key.</param>
        /// <returns>The child <see cref="Mash{TKey,TValue}"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="key"/> is <c>null</c>.</exception>
        [NotNull]
        public Mash<TKey, TValue> this[[NotNull] TKey key] => GetLinked(key);

        /// <summary>
        /// Gets the count of linked <see cref="Mash{TKey,TValue}"/>s referenced by this instance.
        /// </summary>
        /// <value>
        /// The count of linked <see cref="Mash{TKey,TValue}"/>s.
        /// </value>
        public int LinkedCount
        {
            get
            {
                // ReSharper disable once SwitchStatementMissingSomeCases
                switch (_state & StorageState.ChildrenMask)
                {
                    case 0:
                        Debug.Assert(_childrenObj == null);
                        return 0;
                    case StorageState.HasOneChildInATuple:
                        Debug.Assert(_childrenObj is Tuple<TKey, Mash<TKey, TValue>>);
                        return 1;
                    case StorageState.HasTwoChildrenInKeyValuePairArray:
                        Debug.Assert(_childrenObj is KeyValuePair<TKey, Mash<TKey, TValue>>[]);
                        return 2;
                    case StorageState.HasThreeChildrenInKeyValuePairArray:
                        Debug.Assert(_childrenObj is KeyValuePair<TKey, Mash<TKey, TValue>>[]);
                        return 3;
                    case StorageState.HasFourChildrenInKeyValuePairArray:
                        Debug.Assert(_childrenObj is KeyValuePair<TKey, Mash<TKey, TValue>>[]);
                        return 4;
                    case StorageState.HasFiveChildrenInKeyValuePairArray:
                        Debug.Assert(_childrenObj is KeyValuePair<TKey, Mash<TKey, TValue>>[]);
                        return 5;
                    case StorageState.HasManyChildrenInAHashTable:
                        Debug.Assert(_childrenObj is IDictionary<TKey, Mash<TKey, TValue>>);
                        var dict = (IDictionary<TKey, Mash<TKey, TValue>>)_childrenObj;
                        Debug.Assert(dict.Count > 5);
                        return dict.Count;
                    default:
                        Debug.Fail($"Invalid mash state detected: {_state}.");
                        break;
                }

                return 0;
            }
        }
    }
}