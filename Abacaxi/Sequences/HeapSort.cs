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

namespace Abacaxi.Sequences
{
    using System.Diagnostics;
    using System.Collections.Generic;

    /// <summary>
    /// Class implements the classic "heap sort" algorithm.
    /// </summary>
    public static class HeapSort
    {
        private static void Heapify<T>(T[] array, int lo, int hi, IComparer<T> comparer)
        {
            Debug.Assert(array != null);
            Debug.Assert(lo >= 0);
            Debug.Assert(hi < array.Length);
            Debug.Assert(lo <= hi);
            Debug.Assert(comparer != null);

            for (var tailIndex = lo; tailIndex <= hi; tailIndex++)
            {
                var j = tailIndex;
                while (j > lo)
                {
                    var parentIndex = (j - (j % 2 == 0 ? 2 : 1)) / 2;
                    if (comparer.Compare(array[parentIndex], array[j]) < 0)
                    {
                        var temp = array[parentIndex];
                        array[parentIndex] = array[j];
                        array[j] = temp;

                        j = parentIndex;
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        private static void Reorder<T>(T[] array, int lo, int hi, IComparer<T> comparer)
        {
            Debug.Assert(array != null);
            Debug.Assert(lo >= 0);
            Debug.Assert(hi < array.Length);
            Debug.Assert(lo <= hi);
            Debug.Assert(comparer != null);

            for (var tailIndex = hi; tailIndex > lo; tailIndex--)
            {
                var last = array[tailIndex];
                array[tailIndex] = array[lo];
                array[lo] = last;

                var j = lo;
                for (;;)
                {
                    var leftIndex = ((j - lo) << 1) + 1 + lo;
                    if (leftIndex >= tailIndex)
                    {
                        break;
                    }

                    var rightIndex = leftIndex + 1;
                    if (comparer.Compare(array[j], array[leftIndex]) < 0 &&
                        (rightIndex >= tailIndex || comparer.Compare(array[leftIndex], array[rightIndex]) > 0))
                    {
                        var temp = array[leftIndex];
                        array[leftIndex] = array[j];
                        array[j] = temp;

                        j = leftIndex;
                    }
                    else if (rightIndex < tailIndex && comparer.Compare(array[j], array[rightIndex]) < 0)
                    {
                        var temp = array[rightIndex];
                        array[rightIndex] = array[j];
                        array[j] = temp;

                        j = rightIndex;
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Sorts the <paramref name="array"/> using the provided <paramref name="comparer"/>.
        /// </summary>
        /// <typeparam name="T">The type of elements in the array.</typeparam>
        /// <param name="array">The array to sort.</param>
        /// <param name="startIndex">The start index in the array.</param>
        /// <param name="length">The length of sequence to sort.</param>
        /// <param name="comparer">Comparer used in the search.</param>
        /// <exception cref="ArgumentNullException">Thrown if either <paramref name="array"/> or <paramref name="comparer"/> are <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the combination of <paramref name="startIndex"/> and <paramref name="length"/> is out of bounds.</exception>
        public static void Sort<T>(T[] array, int startIndex, int length, IComparer<T> comparer)
        {
            Validate.ArgumentNotNull(nameof(array), array);
            Validate.ArgumentNotNull(nameof(comparer), comparer);
            Validate.ArgumentGreaterThanOrEqualToZero(nameof(startIndex), startIndex);
            Validate.ArgumentGreaterThanOrEqualToZero(nameof(length), length);
            Validate.ArgumentLessThanOrEqual($"{nameof(startIndex)} + {nameof(length)}", startIndex + length, array.Length);

            if (length < 2)
            {
                return;
            }

            Heapify(array, startIndex, startIndex + length - 1, comparer);
            Reorder(array, startIndex, startIndex + length - 1, comparer);
        }
    }
}
