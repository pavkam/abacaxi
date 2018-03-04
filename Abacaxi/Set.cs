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

namespace Abacaxi
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Diagnostics;
    using Internal;
    using JetBrains.Annotations;

    /// <summary>
    /// Class implements set coverage algorithms.
    /// </summary>
    [PublicAPI]
    public static class Set
    {
        [NotNull]
        [ItemNotNull]
        private static IEnumerable<ISet<T>> FindMinimumNumberOfSetsWithFullCoverageIterate<T>(
            [NotNull] [ItemNotNull] IEnumerable<ISet<T>> sets,
            [NotNull] IEqualityComparer<T> comparer)
        {
            Assert.NotNull(sets != null);
            Assert.NotNull(comparer != null);

            var copies = sets.ToSet();
            var superSet = new HashSet<T>(comparer);
            foreach (var set in copies)
            {
                superSet.UnionWith(set);
            }

            while (superSet.Count > 0)
            {
                ISet<T> bestSet = null;

                foreach (var set in copies)
                {
                    var itemsInSuperSet = set.Count(p => superSet.Contains(p));
                    if (bestSet == null || itemsInSuperSet > bestSet.Count)
                    {
                        bestSet = set;
                    }
                }

                if (bestSet != null)
                {
                    superSet.ExceptWith(bestSet);
                    copies.Remove(bestSet);
                    yield return bestSet;
                }
            }
        }

        /// <summary>
        /// Finds the minimum number of sets that cover the full set of elements.
        /// </summary>
        /// <typeparam name="T">The type of elements in the set.</typeparam>
        /// <param name="sets">The sets.</param>
        /// <param name="comparer">The comparer.</param>
        /// <returns>A sequence of selected sets whose union results in the full coverage.</returns>
        /// <exception cref="ArgumentNullException">Thrown if either <paramref name="sets"/> or <paramref name="comparer"/> is <c>null</c>.</exception>
        [NotNull]
        [ItemNotNull]
        public static IEnumerable<ISet<T>> GetOptimalFullCoverage<T>(
            [NotNull] [ItemNotNull] IEnumerable<ISet<T>> sets,
            [NotNull] IEqualityComparer<T> comparer)
        {
            Validate.ArgumentNotNull(nameof(sets), sets);
            Validate.ArgumentNotNull(nameof(comparer), comparer);

            return FindMinimumNumberOfSetsWithFullCoverageIterate(sets, comparer);
        }

        private struct EvaluateAllSubsetCombinationsStep
        {
            public int ItemIndex { get; }
            public int SetIndex { get; }

            public EvaluateAllSubsetCombinationsStep(int itemIndex, int setIndex)
            {
                ItemIndex = itemIndex;
                SetIndex = setIndex;
            }
        }

        [NotNull]
        [ItemNotNull]
        private static IEnumerable<T[][]> EvaluateAllSubsetCombinationsIterate<T>([NotNull] IList<T> sequence,
            int subsets)
        {
            Assert.NotNull(sequence != null);
            Assert.NotNull(subsets > 0);

            if (sequence.Count == 0)
            {
                yield break;
            }

            var resultSets = new List<T>[subsets];
            for (var i = 0; i < resultSets.Length; i++)
            {
                resultSets[i] = new List<T>();
            }

            var stack = new Stack<EvaluateAllSubsetCombinationsStep>();
            stack.Push(new EvaluateAllSubsetCombinationsStep(0, 0));

            while (stack.Count > 0)
            {
                var step = stack.Pop();

                if (step.SetIndex > 0)
                {
                    resultSets[step.SetIndex - 1].RemoveAt(resultSets[step.SetIndex - 1].Count - 1);
                }
                if (step.SetIndex < subsets)
                {
                    resultSets[step.SetIndex].Add(sequence[step.ItemIndex]);

                    stack.Push(new EvaluateAllSubsetCombinationsStep(step.ItemIndex, step.SetIndex + 1));

                    if (step.ItemIndex == sequence.Count - 1)
                    {
                        yield return resultSets.Select(s => s.ToArray()).ToArray();
                    }
                    else
                    {
                        stack.Push(new EvaluateAllSubsetCombinationsStep(step.ItemIndex + 1, 0));
                    }
                }
            }
        }

        /// <summary>
        /// Evaluates all combinations of items in <paramref name="sequence"/> divided into <paramref name="subsets"/>.
        /// </summary>
        /// <typeparam name="T">The type of items in the <paramref name="sequence"/></typeparam>
        /// <param name="sequence">The sequence of elements.</param>
        /// <param name="subsets">Number of sub-sets.</param>
        /// <returns>All the combinations of subsets.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="sequence"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="subsets"/> is less than one.</exception>
        [NotNull]
        [ItemNotNull]
        public static IEnumerable<T[][]> EnumerateSubsetCombinations<T>([NotNull] IList<T> sequence, int subsets)
        {
            Validate.ArgumentNotNull(nameof(sequence), sequence);
            Validate.ArgumentGreaterThanZero(nameof(subsets), subsets);

            return EvaluateAllSubsetCombinationsIterate(sequence, subsets);
        }

        /// <summary>
        /// Finds the subsets with equal aggregate value.
        /// </summary>
        /// <typeparam name="T">The type of elements in <paramref name="sequence"/>.</typeparam>
        /// <param name="sequence">The sequence.</param>
        /// <param name="aggregator">The aggregator function.</param>
        /// <param name="comparer">The comparer.</param>
        /// <param name="subsets">The number of subsets to split into.</param>
        /// <returns>The first sequence of subsets that have the same aggregated value.</returns>
        /// <exception cref="ArgumentNullException">Thrown if either <paramref name="sequence"/> or <paramref name="aggregator"/> or <paramref name="comparer"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="subsets"/> is less than one.</exception>
        [NotNull]
        [ItemNotNull]
        public static T[][] SplitIntoSubsetsOfEqualValue<T>(
            [NotNull] IList<T> sequence,
            [NotNull] Aggregator<T> aggregator,
            [NotNull] IComparer<T> comparer,
            int subsets)
        {
            Validate.ArgumentNotNull(nameof(sequence), sequence);
            Validate.ArgumentNotNull(nameof(aggregator), aggregator);
            Validate.ArgumentNotNull(nameof(comparer), comparer);
            Validate.ArgumentGreaterThanZero(nameof(subsets), subsets);

            foreach (var combo in EnumerateSubsetCombinations(sequence, subsets))
            {
                var firstSum = default(T);
                var allEqual = true;
                for (var i = 0; i < subsets; i++)
                {
                    var subsetSum = combo[i].Length == 0
                        ? default(T)
                        : combo[i].Aggregate((current, item) => aggregator(current, item));
                    if (i == 0)
                    {
                        firstSum = subsetSum;
                    }
                    else
                    {
                        if (comparer.Compare(firstSum, subsetSum) != 0)
                        {
                            allEqual = false;
                            break;
                        }
                    }
                }

                if (allEqual)
                {
                    return combo;
                }
            }

            return new T[][] { };
        }

        /// <summary>
        /// Finds the <paramref name="sequence"/> of integers, which summed, return the closest sum to a given <paramref name="target"/>.
        /// </summary>
        /// <param name="sequence">The sequence of natural integers.</param>
        /// <param name="target">The target sum to aim for.</param>
        /// <returns>A sequence of found integers.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the <paramref name="sequence"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="target"/> is less than <c>1</c> or the <paramref name="sequence"/> contains negative number.</exception>
        [NotNull]
        public static IEnumerable<int> GetSubsetWithNearValue(
            [NotNull] IEnumerable<int> sequence,
            int target)
        {
            Validate.ArgumentNotNull(nameof(sequence), sequence);
            Validate.ArgumentGreaterThanZero(nameof(target), target);

            var elements = sequence.ToArray();
            Array.Sort(elements);

            if (elements.Length > 0)
            {
                Validate.ArgumentGreaterThanOrEqualToZero(nameof(sequence), elements[0]);
            }

            var solutions = new int[target + 1, elements.Length + 1];
            for (var si = 1; si <= elements.Length; si++)
            for (var wi = 0; wi <= target; wi++)
            {
                var currentElement = elements[si - 1];
                if (currentElement > wi)
                {
                    solutions[wi, si] = solutions[wi, si - 1];
                }
                else
                {
                    solutions[wi, si] = Math.Max(
                        currentElement + solutions[wi - currentElement, si - 1],
                        solutions[wi, si - 1]);
                }
            }

            var rwi = target;
            var rsi = elements.Length;
            var result = new List<int>();
            while (rsi > 0 && rwi > 0)
            {
                if (solutions[rwi, rsi] > solutions[rwi, rsi - 1])
                {
                    rwi -= elements[rsi - 1];
                    result.Add(elements[rsi - 1]);
                }

                rsi--;
            }

            return result;
        }

        /// <summary>
        /// Checks if the <paramref name="sequence"/> contains elements, which, summed, yield a given target <paramref name="target"/>.
        /// </summary>
        /// <param name="sequence">The sequence of natural integers.</param>
        /// <param name="target">The sum to target for.</param>
        /// <returns><c>true</c> if the condition is satisfied; <c>false</c> otherwise.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the <paramref name="sequence"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="target"/> is less than <c>1</c> or the <paramref name="sequence"/> contains negative number.</exception>
        public static bool ContainsSubsetWithExactValue(
            [NotNull] IEnumerable<int> sequence, 
            int target)
        {
            return GetSubsetWithNearValue(sequence, target).Sum() == target;
        }

        /// <summary>
        /// Finds the elements, which summed, yield the biggest sum.
        /// </summary>
        /// <typeparam name="T">The type of elements in the <paramref name="sequence"/>.</typeparam>
        /// <param name="sequence">The sequence of elements.</param>
        /// <param name="size">The size of elements to consider.</param>
        /// <param name="aggregator">The aggregator function which sums elements.</param>
        /// <param name="comparer">The comparer.</param>
        /// <returns>An array of elements with the highest sum.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="sequence"/>, <paramref name="aggregator"/> or <paramref name="comparer"/> are null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the <paramref name="size"/> is greater than the number of elements in <paramref name="sequence"/>.</exception>
        [NotNull]
        public static T[] GetSubsetWithGreatestValue<T>(
            [NotNull] IEnumerable<T> sequence,
            int size,
            [NotNull] Aggregator<T> aggregator, 
            [NotNull] IComparer<T> comparer)
        {
            Validate.ArgumentNotNull(nameof(sequence), sequence);
            Validate.ArgumentNotNull(nameof(aggregator), aggregator);
            Validate.ArgumentNotNull(nameof(comparer), comparer);
            Validate.ArgumentGreaterThanOrEqualTo(nameof(size), size, 1);

            var array = sequence.ToArray();
            Validate.ArgumentLessThanOrEqualTo(nameof(size), size, array.Length);

            Sorting.QuickSort(array, 0, array.Length, comparer);

            var result = new T[size];
            Array.Copy(array, array.Length - size, result, 0, size);

            return result;
        }
    }
}