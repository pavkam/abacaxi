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

    /// <summary>
    /// Class implements the classic "shell sort" algorithm.
    /// </summary>
    public static class ShellSort
    {
        private const double ShrinkFactor = 2.2;

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

            var gap = array.Length;
            var finished = false;
            while (!finished)
            {
                gap = (int)Math.Floor(gap / ShrinkFactor);
                if (gap < 1)
                {
                    gap = 1;
                    finished = true;
                }

                for (var i = startIndex; i < startIndex + length - gap; i++)
                {
                    var temp = array[i + gap];
                    var j = i;
                    while (j >= startIndex + gap - 1 && comparer.Compare(temp, array[j]) < 0)
                    {
                        array[j + gap] = array[j];
                        j -= gap;
                    }

                    array[j + gap] = temp;
                }
            }
        }
    }
}
