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
    ///     Class implements the heap data structure with additional "mean value" tracking.
    /// </summary>
    /// <seealso cref="Heap{T}" />
    [PublicAPI]
    public sealed class MeanHeap<T> : ICollection<T>
    {
        [NotNull] private readonly Func<T, T, T> _averageEvalFunc;
        [NotNull] private readonly IComparer<T> _comparer;
        [NotNull] private readonly Heap<T> _leftHeap;
        [NotNull] private readonly Heap<T> _rightHeap;

        /// <summary>
        ///     Initializes a new instance of the <see cref="MeanHeap{T}" /> class.
        /// </summary>
        /// <param name="averageEvalFunc">The average evaluation function (if there are odd number of elements).</param>
        /// <param name="comparer">The comparer.</param>
        /// <exception cref="ArgumentNullException">
        ///     Thrown if <paramref name="comparer" /> or <paramref name="averageEvalFunc" />
        ///     are <c>null</c>.
        /// </exception>
        public MeanHeap([NotNull] IComparer<T> comparer, [NotNull] Func<T, T, T> averageEvalFunc)
        {
            Validate.ArgumentNotNull(nameof(comparer), comparer);
            Validate.ArgumentNotNull(nameof(averageEvalFunc), averageEvalFunc);

            _averageEvalFunc = averageEvalFunc;
            _comparer = comparer;
            var invComparer = Comparer<T>.Create((l, r) => -comparer.Compare(l, r));

            _leftHeap = new Heap<T>(invComparer);
            _rightHeap = new Heap<T>(comparer);
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:Abacaxi.Containers.MeanHeap{T}" /> class.
        /// </summary>
        /// <param name="collection">The collection to add to the container.</param>
        /// <param name="comparer">The comparer.</param>
        /// <param name="averageEvalFunc">The average evaluation function (if there are odd number of elements).</param>
        /// <exception cref="T:System.ArgumentNullException">
        ///     Thrown if <paramref name="comparer" />, <paramref name="averageEvalFunc" /> or <paramref name="collection" /> are
        ///     <c>null</c>.
        /// </exception>
        public MeanHeap([NotNull] IEnumerable<T> collection, [NotNull] IComparer<T> comparer,
            [NotNull] Func<T, T, T> averageEvalFunc) : this(comparer, averageEvalFunc)
        {
            Validate.ArgumentNotNull(nameof(collection), collection);

            foreach (var item in collection)
            {
                Add(item);
            }
        }

        /// <summary>
        ///     Gets the mean value of the heap.
        /// </summary>
        /// <value>
        ///     The mean element.
        /// </value>
        /// <exception cref="InvalidOperationException">The heap is empty.</exception>
        public T Mean
        {
            get
            {
                if (_leftHeap.Count == 0 && _rightHeap.Count == 0)
                {
                    throw new InvalidOperationException("The heap is empty.");
                }

                Assert.Condition(_leftHeap.Count > 0);
                if (_leftHeap.Count > _rightHeap.Count)
                {
                    return _leftHeap.Top;
                }

                Assert.Condition(_rightHeap.Count == _leftHeap.Count);

                return _averageEvalFunc(_rightHeap.Top, _leftHeap.Top);
            }
        }

        /// <inheritdoc />
        /// <summary>
        ///     Gets the number of elements contained in the <see cref="T:Abacaxi.Containers.MeanHeap{T}" />.
        /// </summary>
        public int Count => _leftHeap.Count + _rightHeap.Count;

        /// <inheritdoc />
        /// <summary>
        ///     Gets a value indicating whether the <see cref="T:Abacaxi.Containers.MeanHeap{T}" /> is read-only.
        /// </summary>
        public bool IsReadOnly => false;

        /// <inheritdoc />
        /// <summary>
        ///     Adds an item to the <see cref="T:Abacaxi.Containers.MeanHeap{T}" />.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="T:Abacaxi.Containers.MeanHeap{T}" />.</param>
        public void Add(T item)
        {
            if (_leftHeap.Count <= _rightHeap.Count)
            {
                _leftHeap.Add(item);
            }
            else
            {
                _rightHeap.Add(item);
            }

            Assert.Condition(_leftHeap.Count >= _rightHeap.Count);

            if (_leftHeap.Count > 0 && _rightHeap.Count > 0)
            {
                while (_comparer.Compare(_rightHeap.Top, _leftHeap.Top) > 0)
                {
                    var l = _leftHeap.RemoveTop();
                    var r = _rightHeap.RemoveTop();
                    _leftHeap.Add(r);
                    _rightHeap.Add(l);
                }
            }
        }

        /// <inheritdoc />
        /// <summary>
        ///     Removes all items from the <see cref="T:Abacaxi.Containers.MeanHeap{T}" />.
        /// </summary>
        public void Clear()
        {
            _leftHeap.Clear();
            _rightHeap.Clear();
        }

        /// <inheritdoc />
        /// <summary>
        ///     Determines whether the <see cref="T:Abacaxi.Containers.MeanHeap{T}" /> contains a specific value.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="T:Abacaxi.Containers.MeanHeap{T}" />.</param>
        /// <returns>
        ///     true if <paramref name="item" /> is found in the <see cref="T:Abacaxi.Containers.MeanHeap{T}" />; otherwise, false.
        /// </returns>
        public bool Contains(T item)
        {
            if (_leftHeap.Count > 0 && _comparer.Compare(_leftHeap.Top, item) <= 0)
            {
                return _leftHeap.Contains(item);
            }

            if (_rightHeap.Count > 0 && _comparer.Compare(_rightHeap.Top, item) >= 0)
            {
                return _rightHeap.Contains(item);
            }

            return false;
        }

        /// <inheritdoc />
        /// <summary>
        ///     Copies the elements of the <see cref="T:Abacaxi.Containers.MeanHeap{T}" /> to an <see cref="T:System.Array" />,
        ///     starting at a particular
        ///     <see cref="T:System.Array" /> index.
        /// </summary>
        /// <param name="array">
        ///     The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied
        ///     from <see cref="T:Abacaxi.Containers.MeanHeap{T}" />. The <see cref="T:System.Array" /> must have zero-based
        ///     indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in <paramref name="array" /> at which copying begins.</param>
        /// <exception cref="T:System.ArgumentNullException">Thrown if the <paramref name="array" /> is <c>null</c>.</exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        ///     Thrown if there is not enough space in the
        ///     <paramref name="array" />.
        /// </exception>
        public void CopyTo(T[] array, int arrayIndex)
        {
            Validate.ArgumentNotNull(nameof(array), array);
            Validate.ArgumentGreaterThanOrEqualToZero(nameof(arrayIndex), arrayIndex);
            Validate.ArgumentLessThanOrEqualTo(nameof(arrayIndex), Count, array.Length - arrayIndex);

            _leftHeap.CopyTo(array, arrayIndex);
            array.Reverse(0, _leftHeap.Count);

            _rightHeap.CopyTo(array, arrayIndex + _leftHeap.Count);
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
            foreach (var item in _leftHeap.Reverse())
            {
                yield return item;
            }

            foreach (var item in _rightHeap)
            {
                yield return item;
            }
        }

        /// <inheritdoc />
        /// <summary>
        ///     Removes the first occurrence of a specific object from the <see cref="T:Abacaxi.Containers.MeanHeap{T}" />.
        /// </summary>
        /// <param name="item">The object to remove from the <see cref="T:Abacaxi.Containers.MeanHeap{T}" />.</param>
        /// <returns>
        ///     true if <paramref name="item" /> was successfully removed from the <see cref="T:Abacaxi.Containers.MeanHeap{T}" />;
        ///     otherwise, false. This
        ///     method also returns false if <paramref name="item" /> is not found in the original
        ///     <see cref="T:Abacaxi.Containers.Heap{T}" />.
        /// </returns>
        public bool Remove(T item)
        {
            if (_leftHeap.Count > 0 && _comparer.Compare(_leftHeap.Top, item) <= 0)
            {
                var removed = _leftHeap.Remove(item);
                if (removed && _leftHeap.Count < _rightHeap.Count)
                {
                    _leftHeap.Add(_rightHeap.RemoveTop());
                }

                return removed;
            }

            if (_rightHeap.Count > 0 && _comparer.Compare(_rightHeap.Top, item) >= 0)
            {
                var removed = _rightHeap.Remove(item);

                Assert.Condition(_leftHeap.Count > 0);
                if (removed && _leftHeap.Count - _rightHeap.Count > 1)
                {
                    _rightHeap.Add(_leftHeap.RemoveTop());
                }

                return removed;
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
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}