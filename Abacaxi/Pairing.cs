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
    /// Class implements algorithms used to pair items in sequences towards a given cost goal.
    /// </summary>
    [PublicAPI]
    public static class Pairing
    {
        private sealed class RecursiveFindSubsetPairingWithLowestCostContext
        {
            [NotNull]
            public readonly int[] Indices;
            [CanBeNull]
            public int[] BestCombination;
            public double? LowestCostSoFar;
            [NotNull]
            public readonly Func<int, int, double> CalculatePairCost;

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

        private static void RecursiveFindSubsetPairingWithLowestCost(
            [NotNull] RecursiveFindSubsetPairingWithLowestCostContext context,
            int i,
            int set,
            double currentCost)
        {
            Assert.NotNull(context);

            if (context.LowestCostSoFar.HasValue && currentCost >= context.LowestCostSoFar.Value)
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

        private sealed class RecursiveFindSubsetPairingWithLowestCostPair<T>
        {
            public T Item1;
            public T Item2;
        }

        /// <summary>
        /// Finds all pairs of items from a given <paramref name="sequence"/> whose total combination cost is minimum.
        /// </summary>
        /// <typeparam name="T">The type of items in the sequence.</typeparam>
        /// <param name="sequence">The input sequence.</param>
        /// <param name="evaluateCostOfPairFunc">The function used to evaluate costs of pairs.</param>
        /// <returns>A sequence of pairs which lowest overall cost.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="sequence"/> or <paramref name="evaluateCostOfPairFunc"/> are <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown if the number of elements in <paramref name="sequence"/> is not even.</exception>
        [NotNull,
         ItemNotNull]
        public static Tuple<T, T>[] GetWithMinimumCost<T>(
            [NotNull] IList<T> sequence,
            [NotNull] Func<T, T, double> evaluateCostOfPairFunc)
        {
            Validate.CollectionArgumentsHasEvenNumberOfElements(nameof(sequence), sequence);
            Validate.ArgumentNotNull(nameof(evaluateCostOfPairFunc), evaluateCostOfPairFunc);

            if (sequence.Count == 0)
            {
                return new Tuple<T, T>[0];
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
                    sets[setIndex] = new RecursiveFindSubsetPairingWithLowestCostPair<T> { Item1 = sequence[i] };
                }
                else
                {
                    sets[setIndex].Item2 = sequence[i];
                }
            }

            return sets.Select(s => Tuple.Create(s.Item1, s.Item2)).ToArray();
        }

        /// <summary>
        /// Finds all pairs of items from a given <paramref name="sequence" /> whose total combination cost is minimum.
        /// </summary>
        /// <typeparam name="T">The type of items in the sequence.</typeparam>
        /// <param name="sequence">The input sequence.</param>
        /// <param name="evaluateCostOfPairFunc">The function used to evaluate costs of pairs.</param>
        /// <param name="iterations">The heuristics iteration (the higher the value, the better the results, but slower execution).</param>
        /// <returns>
        /// A sequence of pairs which lowest overall cost.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="sequence" /> or <paramref name="evaluateCostOfPairFunc" /> are <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown if the number of elements in <paramref name="sequence" /> is not even.</exception>
        [NotNull,ItemNotNull]
        public static Tuple<T, T>[] GetWithApproximateMinimumCost<T>(
            [NotNull] IList<T> sequence,
            [NotNull] Func<T, T, double> evaluateCostOfPairFunc,
            int iterations = 100000)
        {
            Validate.CollectionArgumentsHasEvenNumberOfElements(nameof(sequence), sequence);
            Validate.ArgumentNotNull(nameof(evaluateCostOfPairFunc), evaluateCostOfPairFunc);
            Validate.ArgumentGreaterThanZero(nameof(iterations), iterations);

            if (sequence.Count == 0)
            {
                return new Tuple<T, T>[0];
            }

            var steps = (int)Math.Sqrt(iterations);
            var result = SimulatedAnnealing.Evaluate(sequence, 2, pair =>
                {
                    Assert.NotNull(pair);
                    Assert.Condition(pair.Length == 2);

                    return evaluateCostOfPairFunc(pair[0], pair[1]);
                },
                new SimulatedAnnealing.AlgorithmParameters(coolingSteps: steps, iterationsPerCoolingStep: steps));

            return result.Select(pair =>
            {
                Assert.NotNull(pair);
                Assert.Condition(pair.Length == 2);

                return Tuple.Create(pair[0], pair[1]);
            }).ToArray();
        }
    }
}