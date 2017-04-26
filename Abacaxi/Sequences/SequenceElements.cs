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
    using System.Linq;
    using System.Collections.Generic;

    /// <summary>
    /// Class implements a number of algorithms that all use sorting to prepare the sequence before working.
    /// </summary>
    public static class SequenceElements
    {
        
        private static bool FindPartitionsEqualByAggregate<T>(
            T[] array, 
            int index, 
            T[] aggregates,
            Func<T, T, T> aggregateFunc,
            IComparer<T> comparer, 
            List<KeyValuePair<int, T>> result)
        {
            if (index == array.Length)
            {
                for (var ai = 0; ai < aggregates.Length - 1; ai++)
                {
                    if (comparer.Compare(aggregates[ai], aggregates[ai + 1]) != 0)
                    {
                        return false;
                    }
                }

                return true;
            }

            for (var ai = 0; ai < aggregates.Length; ai++)
            {
                var pagg = aggregates[ai];
                aggregates[ai] = aggregateFunc(pagg, array[index]);
                if (FindPartitionsEqualByAggregate(array, index + 1, aggregates, aggregateFunc, comparer, result))
                {
                    result.Add(new KeyValuePair<int, T>(ai, array[index]));
                    return true;
                }

                aggregates[ai] = pagg;
            }

            return false;
        }

        /// <summary>
        /// Finds all sub-sets of a give <paramref name="sequence"/> that add up to the same aggregate value. Returns nothing
        /// if no such partitioning exists.
        /// </summary>
        /// <typeparam name="T">The type of elements in the <paramref name="sequence"/>.</typeparam>
        /// <param name="sequence">The sequence of elements.</param>
        /// <param name="partitions">The count of partitions to consider.</param>
        /// <param name="aggregateFunc">The aggregate function which sums elements.</param>
        /// <param name="comparer">The comparer.</param>
        /// <returns>A sequence of partitions.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="sequence"/>, <paramref name="aggregateFunc"/> or <paramref name="comparer"/> are null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the <paramref name="partitions"/> is less than one.</exception>
        public static IEnumerable<T[]> FindPartitionsEqualByAggregate<T>(IEnumerable<T> sequence, int partitions, 
            Func<T, T, T> aggregateFunc, IComparer<T> comparer)
        {
            Validate.ArgumentNotNull(nameof(sequence), sequence);
            Validate.ArgumentNotNull(nameof(aggregateFunc), aggregateFunc);
            Validate.ArgumentNotNull(nameof(comparer), comparer);
            Validate.ArgumentGreaterThanOrEqualTo(nameof(partitions), partitions, 1);

            var array = sequence.ToArray();
            var aggregates = new T[partitions];
            var result = new List<KeyValuePair<int, T>>();
            if (FindPartitionsEqualByAggregate(array, 0, aggregates, aggregateFunc, comparer, result))
            {
                result.Reverse();
                foreach (var group in result.GroupBy(s => s.Key))
                {
                    yield return group.Select(s => s.Value).ToArray();
                }
            }
        }
    }
}
