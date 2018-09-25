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
    using System.Linq;
    using Internal;
    using JetBrains.Annotations;

    /// <inheritdoc />
    /// <summary>
    ///     Class implements the heap data structure, most commonly known as the "priority queue".
    /// </summary>
    [PublicAPI]
    public sealed class Heap<T> : ICollection<T>
    {
        private const int DefaultArraySize = 32;

        [NotNull] private readonly IComparer<T> _comparer;

        [NotNull] private T[] _array;

        private int _ver;

        /// <summary>
        ///     Initializes a new instance of the <see cref="Heap{T}" /> class.
        /// </summary>
        /// <param name="comparer">The comparer.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="comparer" /> is <c>null</c>.</exception>
        public Heap([NotNull] IComparer<T> comparer)
        {
            Validate.ArgumentNotNull(nameof(comparer), comparer);

            _comparer = comparer;
            _array = new T[DefaultArraySize];
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Heap{T}" /> class.
        /// </summary>
        /// <param name="collection">The collection to add to the container..</param>
        /// <param name="comparer">The comparer.</param>
        /// <exception cref="ArgumentNullException">
        ///     Thrown if <paramref name="comparer" /> or <paramref name="collection" /> is
        ///     <c>null</c>.
        /// </exception>
        public Heap([NotNull] IEnumerable<T> collection, [NotNull] IComparer<T> comparer)
        {
            Validate.ArgumentNotNull(nameof(collection), collection);
            Validate.ArgumentNotNull(nameof(comparer), comparer);

            _comparer = comparer;
            var local = collection.ToArray();
            _array = new T[local.Length < DefaultArraySize ? DefaultArraySize : local.Length];

            Array.Copy(local, _array, local.Length);
            Count = local.Length;

            BuildHeap(_array, Count);
        }

        /// <summary>
        ///     Gets the top of the heap.
        /// </summary>
        /// <value>
        ///     The top element.
        /// </value>
        /// <exception cref="InvalidOperationException">The heap is empty.</exception>
        public T Top
        {
            get
            {
                if (Count == 0)
                {
                    throw new InvalidOperationException("The heap is empty.");
                }

                return _array[0];
            }
        }

        /// <inheritdoc />
        /// <summary>
        ///     Gets the number of elements contained in the <see cref="T:Abacaxi.Containers.Heap`1" />.
        /// </summary>
        public int Count { get; private set; }

        /// <inheritdoc />
        /// <summary>
        ///     Gets a value indicating whether the <see cref="T:Abacaxi.Containers.Heap`1" /> is read-only.
        /// </summary>
        public bool IsReadOnly => false;

        /// <inheritdoc />
        /// <summary>
        ///     Adds an item to the <see cref="T:Abacaxi.Containers.Heap`1" />.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="T:Abacaxi.Containers.Heap`1" />.</param>
        public void Add(T item)
        {
            if (Count == _array.Length)
            {
                Array.Resize(ref _array, Count * 2);
            }

            _array[Count] = item;
            SiftUp(_array, Count);

            _ver++;
            Count++;
        }

        /// <inheritdoc />
        /// <summary>
        ///     Removes all items from the <see cref="T:Abacaxi.Containers.Heap`1" />.
        /// </summary>
        public void Clear()
        {
            Count = 0;
            _ver++;
        }

        /// <inheritdoc />
        /// <summary>
        ///     Determines whether the <see cref="T:Abacaxi.Containers.Heap`1" /> contains a specific value.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="T:Abacaxi.Containers.Heap`1" />.</param>
        /// <returns>
        ///     true if <paramref name="item" /> is found in the <see cref="T:Abacaxi.Containers.Heap`1" />; otherwise, false.
        /// </returns>
        public bool Contains(T item)
        {
            for (var i = 0; i < Count; i++)
            {
                if (_comparer.Compare(_array[i], item) == 0)
                {
                    return true;
                }
            }

            return false;
        }

        /// <inheritdoc />
        /// <summary>
        ///     Copies the elements of the <see cref="T:Abacaxi.Containers.Heap{T}" /> to an <see cref="T:System.Array" />, starting at a particular
        ///     <see cref="T:System.Array" /> index.
        /// </summary>
        /// <param name="array">
        ///     The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied
        ///     from <see cref="T:Abacaxi.Containers.Heap{T}" />. The <see cref="T:System.Array" /> must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in <paramref name="array" /> at which copying begins.</param>
        /// <exception cref="T:System.ArgumentNullException">Thrown if the <paramref name="array" /> is <c>null</c>.</exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">Thrown if there is not enough space in the <paramref name="array" />.</exception>
        public void CopyTo(T[] array, int arrayIndex)
        {
            Validate.ArgumentNotNull(nameof(array), array);
            Validate.ArgumentGreaterThanOrEqualToZero(nameof(arrayIndex), arrayIndex);
            Validate.ArgumentLessThanOrEqualTo(nameof(arrayIndex), Count, array.Length - arrayIndex);

            if (Count == 0)
            {
                return;
            }

            var count = Count;
            var local = new T[count];
            Array.Copy(_array, local, count);

            while (count > 0)
            {
                array[arrayIndex++] = local[0];
                if (count > 1)
                {
                    local[0] = local[count - 1];
                    SiftDown(local, count - 1, 0);
                }

                count--;
            }
        }

        /// <inheritdoc />
        /// <summary>
        ///     Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        ///     A <see cref="T:System.Collections.Generic.IEnumerator{T}" /> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<T> GetEnumerator()
        {
            if (Count == 0)
            {
                yield break;
            }

            var startVer = _ver;
            var count = Count;
            var local = new T[count];
            Array.Copy(_array, local, count);

            while (count > 0)
            {
                if (_ver != startVer)
                {
                    throw new InvalidOperationException("The collection has been modified while being enumerated.");
                }

                yield return local[0];

                if (count > 1)
                {
                    local[0] = local[count - 1];
                    SiftDown(local, count - 1, 0);
                }

                count--;
            }
        }

        /// <inheritdoc />
        /// <summary>
        ///     Removes the first occurrence of a specific object from the <see cref="T:Abacaxi.Containers.Heap`1" />.
        /// </summary>
        /// <param name="item">The object to remove from the <see cref="T:Abacaxi.Containers.Heap{T}" />.</param>
        /// <returns>
        ///     true if <paramref name="item" /> was successfully removed from the <see cref="T:Abacaxi.Containers.Heap{T}" />; otherwise, false. This
        ///     method also returns false if <paramref name="item" /> is not found in the original <see cref="T:Abacaxi.Containers.Heap{T}" />.
        /// </returns>
        public bool Remove(T item)
        {
            for (var i = 0; i < Count; i++)
            {
                if (_comparer.Compare(_array[i], item) != 0)
                {
                    continue;
                }

                if (i < Count - 1)
                {
                    _array[i] = _array[Count - 1];

                    var pi = i % 2 == 1 ? (i - 1) / 2 : (i - 2) / 2;
                    if (pi >= 0 &&
                        _comparer.Compare(_array[pi], _array[i]) < 0)
                    {
                        SiftUp(_array, i);
                    }
                    else
                    {
                        SiftDown(_array, Count - 1, i);
                    }
                }

                Count--;
                _ver++;
                return true;
            }

            return false;
        }

        /// <inheritdoc />
        /// <summary>
        ///     Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        ///     An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private void SiftDown([NotNull] IList<T> array, int length, int pi)
        {
            Assert.Condition(length <= array.Count);
            Assert.Condition(pi >= 0 && pi < length);

            while (pi < length)
            {
                var gi = pi;

                for (var i = 1; i <= 2; i++)
                {
                    var ci = pi * 2 + i;
                    if (ci < length &&
                        _comparer.Compare(array[gi], array[ci]) < 0)
                    {
                        gi = ci;
                    }
                }

                if (gi != pi)
                {
                    var temp = array[gi];
                    array[gi] = array[pi];
                    array[pi] = temp;

                    pi = gi;
                }
                else
                {
                    break;
                }
            }
        }

        private void SiftUp([NotNull] IList<T> array, int ci)
        {
            while (ci > 0)
            {
                var pi = ci % 2 == 1 ? (ci - 1) / 2 : (ci - 2) / 2;
                if (_comparer.Compare(array[pi], array[ci]) < 0)
                {
                    var temp = array[ci];
                    array[ci] = array[pi];
                    array[pi] = temp;

                    ci = pi;
                }
                else
                {
                    break;
                }
            }
        }

        private void BuildHeap([NotNull] IList<T> array, int length)
        {
            Assert.Condition(length <= array.Count);

            for (var ci = 1; ci < length; ci++)
            {
                SiftUp(array, ci);
            }
        }

        /// <summary>
        ///     Removes the top element in the <see cref="Heap{T}" />.
        /// </summary>
        /// <returns>The top element.</returns>
        /// <exception cref="InvalidOperationException">The heap is empty.</exception>
        public T RemoveTop()
        {
            if (Count == 0)
            {
                throw new InvalidOperationException("The heap is empty.");
            }

            var result = _array[0];
            if (Count > 1)
            {
                _array[0] = _array[Count - 1];
                SiftDown(_array, Count - 1, 0);
            }

            Count--;
            _ver++;

            return result;
        }
    }
}