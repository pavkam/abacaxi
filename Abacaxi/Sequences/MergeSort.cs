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
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    /// <summary>
    /// Class implements the "merge sort" algorithm.
    /// </summary>
    public static class MergeSort
    {

        private static void MergeSegments<T>(T[] array, int llo, int lhi, int rlo, int rhi, IComparer<T> comparer)
        {
            Debug.Assert(array != null);
            Debug.Assert(comparer != null);
            Debug.Assert(llo >= 0);
            Debug.Assert(lhi < array.Length);
            Debug.Assert(llo <= lhi);
            Debug.Assert(rlo >= 0);
            Debug.Assert(rhi < array.Length);
            Debug.Assert(rlo <= rhi);
            Debug.Assert(rlo > lhi);

            var mergeLength = (lhi - llo) + (rhi - rlo) + 2;
            var mergeArray = new T[mergeLength];

            var li = llo;
            var ri = rlo;
            var mi = 0;
            
            while (mi < mergeLength)
            {
                T next;
                if (li <= lhi && ri <= rhi)
                {
                    if (comparer.Compare(array[li], array[ri]) < 0)
                    {
                        next = array[li++];
                    }
                    else
                    {
                        next = array[ri++];
                    }
                }
                else if (li > lhi)
                {
                    next = array[ri++];
                }
                else
                {
                    next = array[li++];
                }

                mergeArray[mi++] = next;
            }

            Array.Copy(mergeArray, 0, array, llo, mergeLength);
        }

        private static void SortSegment<T>(T[] array, int lo, int hi, IComparer<T> comparer)
        {
            Debug.Assert(array != null);
            Debug.Assert(comparer != null);
            Debug.Assert(lo >= 0);
            Debug.Assert(hi < array.Length);
            Debug.Assert(lo <= hi);

            if (lo == hi)
            {
                return;
            }

            var middle = (lo + hi) / 2;
            SortSegment(array, lo, middle, comparer);
            SortSegment(array, middle + 1, hi, comparer);

            MergeSegments(array, lo, middle, middle + 1, hi, comparer);
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
            Validate.ArgumentLessThanOrEqualTo($"{nameof(startIndex)} + {nameof(length)}", startIndex + length, array.Length);

            if (length > 0)
            {
                SortSegment(array, startIndex, startIndex + length - 1, comparer);
            }
        }
    }
}
