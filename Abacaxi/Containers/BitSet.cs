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

    /// <summary>
    /// Class implements an integer set internally represented by a bit array. Provides O(1) access times.
    /// </summary>
    [PublicAPI]
    public sealed class BitSet : ISet<int>
    {
        [NotNull] private const int BitsPerChunk = sizeof(int) * 8;
        [NotNull] private int[] _chunks;
        private readonly int _max;
        private readonly int _min;
        private int _ver;

        private void LocateItem(int item, out int chunk, out int mask)
        {
            Assert.Condition(item >= _min && item <= _max);

            item -= _min;
            chunk = item / BitsPerChunk;
            mask = 1 << (item % BitsPerChunk);
        }

        /// <summary>
        /// Initializes a new instance of <see cref="BitSet"/> class.
        /// </summary>
        /// <param name="min">The minimum value stored in the set.</param>
        /// <param name="max">The maximum value stored in the set.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="min"/> is greater then <paramref name="max"/>.</exception>
        public BitSet(int min, int max)
        {
            Validate.ArgumentGreaterThanOrEqualTo(nameof(max), max, min);
            _min = min;
            _max = max;

            var bitCount = Math.Abs(max - min) + 1;
            var chunkCount = bitCount / BitsPerChunk + (bitCount % BitsPerChunk > 0 ? 1 : 0);
            _chunks = new int[chunkCount];
        }

        /// <summary>
        /// Initializes a new instance of <see cref="BitSet"/> class.
        /// </summary>
        /// <param name="count">The number of integers this set can hold (<c>0</c>...<paramref name="count"/><c>-1</c>)</param>
        public BitSet(int count) : this(0, count - 1)
        {
        }

        /// <summary>
        /// The count of integers currently stored in the set.
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        /// Always returns <c>false</c>. The <see cref="BitSet"/> is not read-only.
        /// </summary>
        public bool IsReadOnly => false;

        /// <summary>
        /// Adds an item to the set.
        /// </summary>
        /// <param name="item">The item to add.</param>
        /// <returns><c>true</c> if the item was added; <c>false</c> if the item was already in the set.</returns>
        public bool Add(int item)
        {
            Validate.ArgumentGreaterThanOrEqualTo(nameof(item), item, _min);
            Validate.ArgumentLessThanOrEqualTo(nameof(item), item, _max);

            LocateItem(item, out var chunk, out var mask);

            if ((_chunks[chunk] & mask) != 0)
            {
                return false;
            }

            _chunks[chunk] |= mask;
            Count++;
            _ver++;
            return true;
        }

        /// <summary>
        /// Removes all elements in the specified collection from the current set.
        /// </summary>
        /// <param name="other">The collection of items to remove from the set.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="other"/> is <c>null</c>.</exception>
        public void ExceptWith([NotNull] IEnumerable<int> other)
        {
            Validate.ArgumentNotNull(nameof(other), other);

            foreach (var item in other)
            {
                Remove(item);
            }
        }


        /// <summary>
        /// Modifies the current set so that it contains only elements that are also in a specified collection.
        /// </summary>
        /// <param name="other">The collection to compare to the current set.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="other"/> is <c>null</c>.</exception>
        public void IntersectWith([NotNull] IEnumerable<int> other)
        {
            Validate.ArgumentNotNull(nameof(other), other);

            var inner = new int[_chunks.Length];
            var count = 0;
            foreach (var item in other)
            {
                if (!Contains(item))
                {
                    continue;
                }

                LocateItem(item, out var chunk, out var mask);

                inner[chunk] |= mask;
                count++;
            }

            _ver++;
            _chunks = inner;
            Count = count;
        }

        /// <summary>
        /// Determines whether the current set is a proper (strict) subset of a specified collection.
        /// </summary>
        /// <param name="other">The collection to compare to the current set.</param>
        /// <returns>
        /// true if the current set is a proper subset of <paramref name="other" />; otherwise, false.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="other"/> is <c>null</c>.</exception>
        public bool IsProperSubsetOf([NotNull] IEnumerable<int> other)
        {
            Validate.ArgumentNotNull(nameof(other), other);

            var inner = new BitSet(_min, _max);
            var unmatched = 0;
            foreach (var item in other)
            {
                if (Contains(item))
                {
                    inner.Add(item);
                }
                else
                {
                    unmatched++;
                }
            }

            // ReSharper disable once LoopCanBeConvertedToQuery
            for (var i = 0; i < _chunks.Length; i++)
            {
                if (_chunks[i] != inner._chunks[i])
                {
                    return false;
                }
            }

            return unmatched > 0;
        }

        /// <summary>
        /// Determines whether the current set is a proper (strict) superset of a specified collection.
        /// </summary>
        /// <param name="other">The collection to compare to the current set.</param>
        /// <returns>
        /// true if the current set is a proper superset of <paramref name="other" />; otherwise, false.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="other"/> is <c>null</c>.</exception>
        public bool IsProperSupersetOf([NotNull] IEnumerable<int> other)
        {
            Validate.ArgumentNotNull(nameof(other), other);

            var inner = new BitSet(_min, _max);
            foreach (var item in other)
            {
                if (!Contains(item))
                {
                    return false;
                }

                inner.Add(item);
            }

            return inner.Count < Count;
        }

        /// <summary>
        /// Determines whether the current set is a subset of a specified collection.
        /// </summary>
        /// <param name="other">The collection to compare to the current set.</param>
        /// <returns>
        /// true if the current set is a subset of <paramref name="other" />; otherwise, false.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="other"/> is <c>null</c>.</exception>
        public bool IsSubsetOf([NotNull] IEnumerable<int> other)
        {
            Validate.ArgumentNotNull(nameof(other), other);

            var inner = new BitSet(_min, _max);
            foreach (var item in other)
            {
                if (Contains(item))
                {
                    inner.Add(item);
                }
            }

            // ReSharper disable once LoopCanBeConvertedToQuery
            for (var i = 0; i < _chunks.Length; i++)
            {
                if (_chunks[i] != inner._chunks[i])
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Determines whether the current set is a superset of a specified collection.
        /// </summary>
        /// <param name="other">The collection to compare to the current set.</param>
        /// <returns>
        /// true if the current set is a superset of <paramref name="other" />; otherwise, false.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="other"/> is <c>null</c>.</exception>
        public bool IsSupersetOf([NotNull] IEnumerable<int> other)
        {
            Validate.ArgumentNotNull(nameof(other), other);

            return other.All(Contains);
        }

        /// <summary>
        /// Determines whether the current set overlaps with the specified collection.
        /// </summary>
        /// <param name="other">The collection to compare to the current set.</param>
        /// <returns>
        /// true if the current set and <paramref name="other" /> share at least one common element; otherwise, false.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="other"/> is <c>null</c>.</exception>
        public bool Overlaps([NotNull] IEnumerable<int> other)
        {
            Validate.ArgumentNotNull(nameof(other), other);

            var otherHasItems = false;
            foreach (var item in other)
            {
                if (Contains(item))
                {
                    return true;
                }

                otherHasItems = true;
            }

            return !otherHasItems;
        }

        /// <summary>
        /// Determines whether the current set and the specified collection contain the same elements.
        /// </summary>
        /// <param name="other">The collection to compare to the current set.</param>
        /// <returns>
        /// true if the current set is equal to <paramref name="other" />; otherwise, false.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="other"/> is <c>null</c>.</exception>
        public bool SetEquals([NotNull] IEnumerable<int> other)
        {
            Validate.ArgumentNotNull(nameof(other), other);

            var copy = new int[_chunks.Length];
            Array.Copy(_chunks, copy, copy.Length);

            foreach (var item in other)
            {
                if (Contains(item))
                {
                    LocateItem(item, out var chunk, out var mask);

                    copy[chunk] &= ~mask;
                }
                else
                {
                    return false;
                }
            }

            return copy.All(t => t == 0);
        }

        /// <summary>
        /// Modifies the current set so that it contains only elements that are present either in the current set or in the specified collection, but not both.
        /// </summary>
        /// <param name="other">The collection to compare to the current set.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="other"/> is <c>null</c>.</exception>
        public void SymmetricExceptWith([NotNull] IEnumerable<int> other)
        {
            Validate.ArgumentNotNull(nameof(other), other);

            var itemsToRemove = new List<int>();
            var itemsToAdd = new List<int>();
            foreach (var item in other)
            {
                if (Contains(item))
                {
                    itemsToRemove.Add(item);
                }
                else
                {
                    itemsToAdd.Add(item);
                }
            }

            foreach (var itemToRemove in itemsToRemove)
            {
                Remove(itemToRemove);
            }

            foreach (var itemToAdd in itemsToAdd)
            {
                Add(itemToAdd);
            }
        }

        /// <summary>
        /// Modifies the current set so that it contains all elements that are present in either the current set or the specified collection.
        /// </summary>
        /// <param name="other">The collection to compare to the current set.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="other"/> is <c>null</c>.</exception>
        public void UnionWith([NotNull] IEnumerable<int> other)
        {
            Validate.ArgumentNotNull(nameof(other), other);

            foreach (var item in other)
            {
                Add(item);
            }
        }

        /// <summary>
        /// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
        void ICollection<int>.Add(int item)
        {
            Add(item);
        }

        /// <summary>
        /// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        public void Clear()
        {
            for (var i = 0; i < _chunks.Length; i++)
            {
                _chunks[i] = 0;
            }

            _ver++;
            Count = 0;
        }

        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.Generic.ICollection`1" /> contains a specific value.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
        /// <returns>
        /// true if <paramref name="item" /> is found in the <see cref="T:System.Collections.Generic.ICollection`1" />; otherwise, false.
        /// </returns>
        public bool Contains(int item)
        {
            if (item < _min || item > _max)
            {
                return false;
            }

            LocateItem(item, out var chunk, out var mask);

            return (_chunks[chunk] & mask) != 0;

        }

        /// <summary>
        /// Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1" /> to an <see cref="T:System.Array" />, starting at a particular <see cref="T:System.Array" /> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.ICollection`1" />. The <see cref="T:System.Array" /> must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in <paramref name="array" /> at which copying begins.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="array"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the destination <paramref name="array"/> does not have enough space to hold the contents of the set.</exception>
        public void CopyTo([NotNull] int[] array, int arrayIndex)
        {
            Validate.ArgumentNotNull(nameof(array), array);
            Validate.ArgumentGreaterThanOrEqualToZero(nameof(arrayIndex), arrayIndex);
            Validate.ArgumentGreaterThanOrEqualTo(nameof(arrayIndex), array.Length - arrayIndex, Count);

            for (var ci = 0; ci < _chunks.Length; ci++)
            {
                var chunk = _chunks[ci];
                for (var bi = 0; bi < BitsPerChunk && chunk != 0; bi++)
                {
                    if (bi > 0)
                    {
                        chunk >>= 1;
                    }

                    if (chunk % 2 == 1)
                    {
                        array[arrayIndex++] = _min + (ci * BitsPerChunk + bi);
                    }
                }
            }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection{T}" />.
        /// </summary>
        /// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection{T}" />.</param>
        /// <returns>
        /// true if <paramref name="item" /> was successfully removed from the <see cref="T:System.Collections.Generic.ICollection{T}" />; otherwise, false. This method also returns false if <paramref name="item" /> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </returns>
        public bool Remove(int item)
        {
            if (item < _min || item > _max)
            {
                return false;
            }

            LocateItem(item, out var chunk, out var mask);

            if ((_chunks[chunk] & mask) == 0)
            {
                return false;
            }

            _chunks[chunk] &= ~mask;
            Count--;
            _ver++;
            return true;

        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator{T}" /> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<int> GetEnumerator()
        {
            var startVer = _ver;
            for (var ci = 0; ci < _chunks.Length; ci++)
            {
                var chunk = _chunks[ci];
                for (var bi = 0; bi < BitsPerChunk && chunk != 0; bi++)
                {
                    if (_ver != startVer)
                    {
                        throw new InvalidOperationException(
                            "The collection has been modified. Enumeration impossible.");
                    }

                    if (bi > 0)
                    {
                        chunk >>= 1;
                    }

                    if (chunk % 2 == 1)
                    {
                        yield return _min + (ci * BitsPerChunk + bi);
                    }
                }
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        [NotNull]
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}