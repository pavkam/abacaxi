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
    using System.Linq;

    /// <summary>
    /// Class implements the algorithm to efficiently reverse an array of elements. For a any sequence of length N, reversing is a simple
    /// process of swapping each element i with the element N - i.
    /// </summary>
    public static class ReverseSequence
    {
        /// <summary>
        /// Reverses a given <paramref name="array"/> in place (mutating the original).
        /// </summary>
        /// <typeparam name="T">The type of elements in the array.</typeparam>
        /// <param name="array">The array to reverse.</param>
        /// <param name="startIndex">The start index in the array.</param>
        /// <param name="length">The length of sequence to reverse.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="array"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the combination of <paramref name="startIndex"/> and <paramref name="length"/> is out of bounds.</exception>
        public static void Reverse<T>(T[] array, int startIndex, int length)
        {
            Validate.ArgumentNotNull(nameof(array), array);
            Validate.ArgumentGreaterThanOrEqualToZero(nameof(startIndex), startIndex);
            Validate.ArgumentGreaterThanOrEqualToZero(nameof(length), length);
            Validate.ArgumentLessThanOrEqual($"{nameof(startIndex)} + {nameof(length)}", startIndex + length, array.Length);

            for (var i = 0; i < length / 2; i++)
            {
                var swap = array[i + startIndex];
                array[i + startIndex] = array[length - i + startIndex - 1];
                array[length - i + startIndex - 1] = swap;
            }
        }
    }
}
