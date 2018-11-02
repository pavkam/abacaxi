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
    using Internal;
    using JetBrains.Annotations;

    /// <summary>
    ///     Class implements algorithms used to pair items in sequences towards a given cost goal.
    /// </summary>
    [PublicAPI]
    public static class Pairing
    {
        private static void RecursiveFindSubsetPairingWithLowestCost(
            [NotNull] RecursiveFindSubsetPairingWithLowestCostContext context,
            int i,
            int set,
            double currentCost)
        {
            Assert.NotNull(context);

            if (context.LowestCostSoFar.HasValue &&
                currentCost >= context.LowestCostSoFar.Value)
            {
                return;
            }

            if (i == context.Indices.Length)
            {
                context.LowestCostSoFar = currentCost;
                context.BestCombination = new int[context.Indices.Length];
                Array.Copy(context.Indices, context.BestCombination, context.Indices.Length);

                return;
            }

            if (context.Indices[i] > 0)
            {
                // ReSharper disable once TailRecursiveCall
                RecursiveFindSubsetPairingWithLowestCost(context, i + 1, set, currentCost);
            }
            else
            {
                for (var ni = i + 1; ni < context.Indices.Length; ni++)
                {
                    if (context.Indices[ni] != 0)
                    {
                        continue;
                    }

                    context.Indices[i] = set;
                    context.Indices[ni] = set;

                    var pairCost = context.CalculatePairCost(i, ni);
                    var newCost = currentCost + pairCost;

                    RecursiveFindSubsetPairingWithLowestCost(context, i + 1, set + 1, newCost);

                    context.Indices[ni] = 0;
                }

                context.Indices[i] = 0;
            }
        }

        /// <summary>
        ///     Finds all pairs of items from a given <paramref name="sequence" /> whose total combination cost is minimum.
        /// </summary>
        /// <typeparam name="T">The type of items in the sequence.</typeparam>
        /// <param name="sequence">The input sequence.</param>
        /// <param name="evaluateCostOfPairFunc">The function used to evaluate costs of pairs.</param>
        /// <returns>A sequence of pairs which lowest overall cost.</returns>
        /// <exception cref="ArgumentNullException">
        ///     Thrown if <paramref name="sequence" /> or
        ///     <paramref name="evaluateCostOfPairFunc" /> are <c>null</c>.
        /// </exception>
        /// <exception cref="ArgumentException">Thrown if the number of elements in <paramref name="sequence" /> is not even.</exception>
        [NotNull]
        public static (T, T)[] GetPairsWithMinimumCost<T>(
            [NotNull] IList<T> sequence,
            [NotNull] Func<T, T, double> evaluateCostOfPairFunc)
        {
            Validate.CollectionArgumentsHasEvenNumberOfElements(nameof(sequence), sequence);
            Validate.ArgumentNotNull(nameof(evaluateCostOfPairFunc), evaluateCostOfPairFunc);

            if (sequence.Count == 0)
            {
                return new (T, T)[0];
            }

            var context = new RecursiveFindSubsetPairingWithLowestCostContext(
                new int[sequence.Count],
                (l, r) =>
                {
                    Assert.Condition(l >= 0 && l < sequence.Count);
                    Assert.Condition(r >= 0 && r < sequence.Count);
                    Assert.Condition(l != r);

                    return evaluateCostOfPairFunc(sequence[l], sequence[r]);
                }
            );

            RecursiveFindSubsetPairingWithLowestCost(context, 0, 1, 0);
            Assert.NotNull(context.BestCombination);

            var sets = new RecursiveFindSubsetPairingWithLowestCostPair<T>[sequence.Count / 2];
            for (var i = 0; i < context.BestCombination.Length; i++)
            {
                var setIndex = context.BestCombination[i] - 1;
                Assert.Condition(setIndex >= 0);

                if (sets[setIndex] == null)
                {
                    sets[setIndex] = new RecursiveFindSubsetPairingWithLowestCostPair<T> {Item1 = sequence[i]};
                }
                else
                {
                    sets[setIndex].Item2 = sequence[i];
                }
            }

            return sets.Select(s => (s.Item1, s.Item2)).ToArray();
        }

        /// <summary>
        ///     Finds all pairs of items from a given <paramref name="sequence" /> whose total combination cost is minimum.
        /// </summary>
        /// <typeparam name="T">The type of items in the sequence.</typeparam>
        /// <param name="sequence">The input sequence.</param>
        /// <param name="evaluateCostOfPairFunc">The function used to evaluate costs of pairs.</param>
        /// <param name="iterations">The heuristics iteration (the higher the value, the better the results, but slower execution).</param>
        /// <returns>
        ///     A sequence of pairs which lowest overall cost.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     Thrown if <paramref name="sequence" /> or
        ///     <paramref name="evaluateCostOfPairFunc" /> are <c>null</c>.
        /// </exception>
        /// <exception cref="ArgumentException">Thrown if the number of elements in <paramref name="sequence" /> is not even.</exception>
        [NotNull]
        public static (T, T)[] GetPairsWithApproximateMinimumCost<T>(
            [NotNull] IList<T> sequence,
            [NotNull] Func<T, T, double> evaluateCostOfPairFunc,
            int iterations = 100000)
        {
            Validate.CollectionArgumentsHasEvenNumberOfElements(nameof(sequence), sequence);
            Validate.ArgumentNotNull(nameof(evaluateCostOfPairFunc), evaluateCostOfPairFunc);
            Validate.ArgumentGreaterThanZero(nameof(iterations), iterations);

            if (sequence.Count == 0)
            {
                return new (T, T)[0];
            }

            var steps = (int) Math.Sqrt(iterations);
            var result = SimulatedAnnealing.Evaluate(sequence, 2, pair =>
                {
                    Assert.NotNull(pair);
                    Assert.Condition(pair.Length == 2);

                    return evaluateCostOfPairFunc(pair[0], pair[1]);
                },
                new SimulatedAnnealing.AlgorithmParameters(steps, steps));

            return result.Select(pair =>
                {
                    Assert.NotNull(pair);
                    Assert.Condition(pair.Length == 2);

                    return (pair[0], pair[1]);
                })
                .ToArray();
        }

        /// <summary>
        ///     Gets the first pair in <paramref name="sequence" /> that whose elements are at maximum distance from each other (in
        ///     terms of value).
        /// </summary>
        /// <typeparam name="T">The type of items in the sequence.</typeparam>
        /// <param name="sequence">The sequence.</param>
        /// <param name="comparer">The element comparer.</param>
        /// <returns>
        ///     The pair that has the maximum difference between its elements. If the <paramref name="sequence" /> contains
        ///     less than two elements, <c>null</c> is returned.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     Thrown if <paramref name="sequence" /> or
        ///     <paramref name="comparer" /> are <c>null</c>.
        /// </exception>
        public static (T first, T last)? GetPairWithMaximumDifference<T>(
            [NotNull] IEnumerable<T> sequence,
            [NotNull] IComparer<T> comparer)
        {
            Validate.ArgumentNotNull(nameof(sequence), sequence);
            Validate.ArgumentNotNull(nameof(comparer), comparer);

            var array = sequence.ToArray();
            if (array.Length < 2)
            {
                return null;
            }

            Sorting.QuickSort(array, 0, array.Length, comparer);

            return (array[0], array[array.Length - 1]);
        }

        /// <summary>
        ///     Gets the first pair in <paramref name="sequence" /> that whose elements are in are at maximum distance from each
        ///     other (in
        ///     terms of value) and in increasing order.
        /// </summary>
        /// <typeparam name="T">The type of items in the sequence.</typeparam>
        /// <param name="sequence">The sequence.</param>
        /// <param name="evaluateDiffOfPairFunc">The difference evaluation function.</param>
        /// <returns>
        ///     The pair that has the maximum difference between its elements. If the <paramref name="sequence" /> contains
        ///     less than two elements or all elements are in decreasing order, <c>null</c> is returned.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     Thrown if <paramref name="sequence" /> or
        ///     <paramref name="evaluateDiffOfPairFunc" /> are <c>null</c>.
        /// </exception>
        public static (T first, T last)? GetPairWithIncreasingOrderMaximumDifference<T>(
            [NotNull] IList<T> sequence,
            [NotNull] Func<T, T, double> evaluateDiffOfPairFunc)
        {
            Validate.ArgumentNotNull(nameof(sequence), sequence);
            Validate.ArgumentNotNull(nameof(evaluateDiffOfPairFunc), evaluateDiffOfPairFunc);

            if (sequence.Count < 2)
            {
                return null;
            }

            (T first, T last, double diff) current = (sequence[0], sequence[1],
                evaluateDiffOfPairFunc(sequence[1], sequence[0]));

            (T first, T last, double diff) best = (default(T), default(T), 0);

            for (var i = 2; i < sequence.Count; i++)
            {
                var diff = evaluateDiffOfPairFunc(sequence[i], current.first);
                if (diff > current.diff)
                {
                    current = (current.first, sequence[i], diff);
                }
                else if (diff < 0)
                {
                    if (best.diff < current.diff)
                    {
                        best = current;
                    }

                    current = (sequence[i], default(T), 0);
                }
            }

            if (best.diff < current.diff)
            {
                best = current;
            }

            return best.diff <= 0 ? ((T, T)?) null : (best.first, best.last);
        }

        /// <summary>
        ///     Gets all the pair of elements from <paramref name="sequence1" /> and <paramref name="sequence2" /> that can be
        ///     swapped in order to equalize the sums of given sequences.
        /// </summary>
        /// <param name="sequence1">The first sequence.</param>
        /// <param name="sequence2">The second sequence.</param>
        /// <returns>A list of pairs composed of elements from both sequences.</returns>
        /// <exception cref="ArgumentNullException">
        ///     Thrown if <paramref name="sequence1" /> or
        ///     <paramref name="sequence2" /> are <c>null</c>.
        /// </exception>
        [NotNull]
        public static (int element1, int element2)[] GetEqualizationPairs(
            [NotNull] IList<int> sequence1,
            [NotNull] IList<int> sequence2)
        {
            Validate.ArgumentNotNull(nameof(sequence1), sequence1);
            Validate.ArgumentNotNull(nameof(sequence2), sequence2);

            /* Calculate aggregates of both sequences. */
            var a1 = 0;
            var map1 = new HashSet<int>();
            foreach (var i in sequence1)
            {
                a1 += i;
                map1.Add(i);
            }

            var a2 = 0;
            var map2 = new HashSet<int>();
            foreach (var i in sequence2)
            {
                a2 += i;
                map2.Add(i);
            }

            /* Calculate the expected relationship and find the swap. */
            var delta = (a1 - a2) / 2;
            var result = new List<(int element1, int element2)>();
            foreach (var i in sequence2)
            {
                if (!map2.Remove(i))
                {
                    continue;
                }

                var opposite = delta + i;
                if (map1.Contains(opposite))
                {
                    result.Add((opposite, i));
                }
            }

            return result.ToArray();
        }

        private sealed class RecursiveFindSubsetPairingWithLowestCostContext
        {
            [NotNull] public readonly Func<int, int, double> CalculatePairCost;

            [NotNull] public readonly int[] Indices;

            [CanBeNull] public int[] BestCombination;

            public double? LowestCostSoFar;

            public RecursiveFindSubsetPairingWithLowestCostContext(
                [NotNull] int[] indices,
                [NotNull] Func<int, int, double> calculatePairCost)
            {
                Assert.NotNull(indices);
                Assert.NotNull(calculatePairCost);

                Indices = indices;
                CalculatePairCost = calculatePairCost;
            }
        }

        private sealed class RecursiveFindSubsetPairingWithLowestCostPair<T>
        {
            public T Item1;
            public T Item2;
        }
    }
}