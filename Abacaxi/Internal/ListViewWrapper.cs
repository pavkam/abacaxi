/* Copyright 2017 by Alexandru Ciobanu (alex+git@ciobanu.org)
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

namespace Abacaxi.Internal
{
    using System;
    using System.Collections;
    using System.Diagnostics;
    using System.Collections.Generic;
    using JetBrains.Annotations;

    internal sealed class ListViewWrapper<T> : IList<T>
    {
        [NotNull]
        private readonly IList<T> _sequence;
        private readonly int _startIndex;

        private void AssertBounds(int index, bool includeUpperBound = false)
        {
            Validate.ArgumentGreaterThanOrEqualToZero(nameof(index), index);

            if (includeUpperBound)
            {
                Validate.ArgumentLessThanOrEqualTo(nameof(index), index, Count);
            }
            else
            {
                Validate.ArgumentLessThan(nameof(index), index, Count);
            }
        }

        private void AssertSegmentStillValid()
        {
            if (_startIndex + Count > _sequence.Count)
            {
                throw new InvalidOperationException("The original list has been modified and the segment no longer fits into its bounds.");
            }
        }

        public ListViewWrapper([NotNull] IList<T> sequence, int startIndex, int length)
        {
            Debug.Assert(sequence != null);
            Debug.Assert(startIndex >= 0);
            Debug.Assert(length >= 0);
            Debug.Assert(length + startIndex <= sequence.Count);

            _startIndex = startIndex;
            Count = length;
            _sequence = sequence;
        }

        public IEnumerator<T> GetEnumerator()
        {
            AssertSegmentStillValid();
            for (var i = 0; i < Count; i++)
            {
                yield return _sequence[_startIndex + i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(T item)
        {
            AssertSegmentStillValid();

            _sequence.Insert(_startIndex + Count, item);
            Count++;
        }

        public void Clear()
        {
            AssertSegmentStillValid();

            for (var i = 0; i < Count; i++)
            {
                _sequence.RemoveAt(_startIndex);
            }

            Count = 0;
        }

        public bool Contains(T item)
        {
            AssertSegmentStillValid();

            return IndexOf(item) > -1;
        } 

        public void CopyTo(T[] array, int arrayIndex)
        {
            Validate.ArgumentNotNull(nameof(array), array);
            Validate.ArgumentGreaterThanOrEqualToZero(nameof(arrayIndex), arrayIndex);
            Validate.ArgumentLessThanOrEqualTo(nameof(arrayIndex), Count, array.Length - arrayIndex);

            AssertSegmentStillValid();

            for (var i = 0; i < Count; i++)
            {
                array[i] = _sequence[_startIndex + i];
            }
        }

        public bool Remove(T item)
        {
            AssertSegmentStillValid();

            for (var i = 0; i < Count; i++)
            {
                if (Equals(_sequence[_startIndex + i], item))
                {
                    _sequence.RemoveAt(_startIndex + i);
                    Count--;

                    return true;
                }
            }

            return false;
        }

        public int Count { get; private set; }

        public bool IsReadOnly => _sequence.IsReadOnly;

        public int IndexOf(T item)
        {
            AssertSegmentStillValid();

            for (var i = 0; i < Count; i++)
            {
                if (Equals(_sequence[_startIndex + i], item))
                {
                    return i;
                }
            }

            return -1;
        }

        public void Insert(int index, T item)
        {
            AssertBounds(index, true);
            AssertSegmentStillValid();

            _sequence.Insert(_startIndex + index, item);
            Count++;
        }

        public void RemoveAt(int index)
        {
            AssertBounds(index);
            AssertSegmentStillValid();

            _sequence.RemoveAt(_startIndex + index);
            Count--;
        }

        public T this[int index]
        {
            get
            {
                AssertBounds(index);
                AssertSegmentStillValid();

                return _sequence[_startIndex + index];
            }
            set
            {
                AssertBounds(index);
                AssertSegmentStillValid();

                _sequence[_startIndex + index] = value;
            }
        }
    }
}
