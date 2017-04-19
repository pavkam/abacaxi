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
    using System.Collections;
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
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="sequence"/>, <paramref name="aggregateFunc"/> or <paramref name="comparer"/> are null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the <paramref name="count"/> is greater than the number of elements in <paramref name="sequence"/>.</exception>
        public static T[] FindBiggestSumOfNumberOfElements<T>(IEnumerable<T> sequence, int count, Func<T, T, T> aggregateFunc, IComparer<T> comparer)
        {
            Validate.ArgumentNotNull(nameof(sequence), sequence);
            Validate.ArgumentNotNull(nameof(aggregateFunc), aggregateFunc);
            Validate.ArgumentNotNull(nameof(comparer), comparer);
            Validate.ArgumentGreaterThanOrEqualTo(nameof(count), count, 1);

            var array = sequence.ToArray();
            Validate.ArgumentLessThanOrEqualTo(nameof(count), count, array.Length);

            QuickSort.Sort(array, 0, array.Length, comparer);

            var result = new T[count];
            Array.Copy(array, array.Length - count, result, 0, count);

            return result; 
        }

        /// <summary>
        /// Determines whether the sequence contains two elements that aggregate to a given <paramref name="aggregate"/> value.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="sequence">The sequence to check.</param>
        /// <param name="aggregate">The aggregate value to search for..</param>
        /// <param name="aggregateFunc">The function that aggregates two values.</param>
        /// <param name="comparer">The comparer.</param>
        /// <returns>
        ///   <c>true</c> if the <paramref name="sequence"/> contains two elements that aggregate; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="sequence"/>, <paramref name="aggregateFunc"/> or <paramref name="comparer"/> are null.</exception>
        public static bool ContainsTwoElementsThatAggregateTo<T>(IEnumerable<T> sequence, T aggregate, Func<T, T, T> aggregateFunc, IComparer<T> comparer)
        {
            Validate.ArgumentNotNull(nameof(sequence), sequence);
            Validate.ArgumentNotNull(nameof(aggregateFunc), aggregateFunc);
            Validate.ArgumentNotNull(nameof(comparer), comparer);

            var array = sequence.ToArray();
            if (array.Length == 0)
            {
                return false;
            }

            QuickSort.Sort(array, 0, array.Length, comparer);
            var i = 0;
            var j = array.Length - 1;
            while (i < j)
            {
                var a = aggregateFunc(array[i], array[j]);
                var cr = comparer.Compare(a, aggregate);

                if (cr == 0)
                {
                    return true;
                }
                else if (cr > 0)
                {
                    j--;
                }
                else
                {
                    i++;
                }
            }


            return false;
        }

        /// <summary>
        /// Finds all the sub-sequences of <paramref name="sequence">/ whose aggreagate is provided. 
        /// </summary>
        /// <typeparam name="T">The type of elements in the <paramref name="sequence"/>.</typeparam>
        /// <param name="sequence">The sequence of elements.</param>
        /// <param name="aggregate">The aggregate to search for.</param>
        /// <param name="aggregateFunc">The aggregate function which sums elements.</param>
        /// <param name="comparer">The comparer.</param>
        /// <returns>An array of elements with the highest sum.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="sequence"/>, <paramref name="aggregateFunc"/>, <paramref name="disaggregateFunc"/> or <paramref name="comparer"/> are null.</exception>
        public static IEnumerable<T[]> FindAllSequencesThatAggregateTo<T>(IEnumerable<T> sequence, T aggregate, 
            Func<T, T, T> aggregateFunc, Func<T, T, T> disaggregateFunc, IComparer<T> comparer)
        {
            Validate.ArgumentNotNull(nameof(sequence), sequence);
            Validate.ArgumentNotNull(nameof(aggregateFunc), aggregateFunc);
            Validate.ArgumentNotNull(nameof(disaggregateFunc), disaggregateFunc);
            Validate.ArgumentNotNull(nameof(comparer), comparer);

            var queue = new Queue<T>();
            T currentAgg = default(T);
            var isFirst = true;

            foreach (var i in sequence)
            {
                queue.Enqueue(i);
                if (isFirst)
                {
                    isFirst = false;
                    currentAgg = i;
                }
                else
                {
                    currentAgg = aggregateFunc(currentAgg, i);
                }

                for(;;)
                {
                    var comparison = comparer.Compare(currentAgg, aggregate);
                    if (comparison == 0)
                    {
                        yield return queue.ToArray();
                        break;
                    }
                    else if (comparison > 0)
                    {
                        currentAgg = disaggregateFunc(currentAgg, queue.Dequeue());
                    }
                    else
                    {
                        break;
                    }
                };
            }
        }
    }
}
