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
    using System.Collections.Generic;

    /// <summary>
    /// Class implements the classic binary search algorithm. The algorithm will select half of a sorted array at each iteration, until it finds
    /// the element it was searching for.
    /// </summary>
    public static class BinarySearch
    {
        /// <summary>
        /// Finds the location of <paramref name="item"/> in the given <paramref name="array"/>.
        /// </summary>
        /// <typeparam name="T">The type of elements in the array.</typeparam>
        /// <param name="array">The array to search.</param>
        /// <param name="startIndex">The start index in the array.</param>
        /// <param name="length">The length of sequence to search..</param>
        /// <param name="item">The item to search for.</param>
        /// <param name="comparer">Comparer used in the search.</param>
        /// <param name="ascending">Specifies whether the array iss sorted in ascending or descending order.</param>
        /// <returns>The index in the array where the <paramref name="item"/> was found; <c>-1</c> otherwise.</returns>
        /// <exception cref="ArgumentNullException">Thrown if either <paramref name="array"/> or <paramref name="comparer"/> are <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the combination of <paramref name="startIndex"/> and <paramref name="length"/> is out of bounds.</exception>
        public static int Search<T>(T[] array, int startIndex, int length, T item, IComparer<T> comparer, bool ascending = true)
        {
            Validate.ArgumentNotNull(nameof(array), array);
            Validate.ArgumentNotNull(nameof(comparer), comparer);
            Validate.ArgumentGreaterThanOrEqualToZero(nameof(startIndex), startIndex);
            Validate.ArgumentGreaterThanOrEqualToZero(nameof(length), length);
            Validate.ArgumentLessThanOrEqualTo($"{nameof(startIndex)} + {nameof(length)}", startIndex + length, array.Length);

            var start = startIndex;
            var end = startIndex + length - 1;
            var direction = ascending ? 1 : -1;

            while (start <= end)
            {
                var mid = (start + end) / 2;
                var compareResult = direction * comparer.Compare(array[mid], item);

                if (compareResult == 0)
                {
                    return mid;
                }
                else if (compareResult < 0)
                {
                    start = mid + 1;
                }
                else
                {
                    end = mid - 1;
                }
            }

            return -1;
        }
    }
}
