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
    /// Class implements a number of algorithms that all use sorting to prepare the sequence before working.
    /// </summary>
    public static class SequenceElements
    {
        /// <summary>
        /// Finds the elements, which summed, yield the biggest sum.
        /// </summary>
        /// <typeparam name="T">The type of elements in the <paramref name="sequence"/>.</typeparam>
        /// <param name="sequence">The sequence of elements.</param>
        /// <param name="count">The count of elements to consider.</param>
        /// <param name="aggregateFunc">The aggregate function which sums elements.</param>
        /// <param name="comparer">The comparer.</param>
        /// <returns>An array of elements with the highest sum.</returns>
        public static T[] FindBiggestSumOfNumberOfElements<T>(T[] sequence, int count, Func<T, T, T> aggregateFunc, IComparer<T> comparer)
        {
            Validate.ArgumentNotNull(nameof(sequence), sequence);
            Validate.ArgumentNotNull(nameof(aggregateFunc), aggregateFunc);
            Validate.ArgumentNotNull(nameof(comparer), comparer);
            Validate.ArgumentGreaterThanOrEqualTo(nameof(count), count, 1);
            Validate.ArgumentLessThanOrEqualTo(nameof(count), count, sequence.Length);

            var array = new T[sequence.Length];
            Array.Copy(sequence, array, array.Length);
            QuickSort.Sort(array, 0, array.Length, comparer);

            var result = new T[count];
            Array.Copy(array, array.Length - count, result, 0, count);

            return result; 
        }
    }
}
