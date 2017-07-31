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

namespace Abacaxi
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Diagnostics;
    using Internal;
    using JetBrains.Annotations;

    /// <summary>
    /// Class implements multiple algorithms that deals with combinatorial problems.
    /// </summary>
    [PublicAPI]
    public static class CombinatorialAlgorithms
    {
        private struct Step
        {
            public int ItemIndex { get; }
            public int SetIndex { get; }

            public Step(int itemIndex, int setIndex)
            {
                ItemIndex = itemIndex;
                SetIndex = setIndex;
            }
        }

        private sealed class RecursiveFindSubsetPairingWithLowestCostContext
        {
            public int[] Indices;
            public int[] BestCombination;
            public double? LowestCostSoFar;
            public Func<int, int, double> CalculatePairCost;
        }

        private sealed class Pair<T>
        {
            public T Item1;
            public T Item2;
        }

        private static void RecursiveFindSubsetPairingWithLowestCost(
            RecursiveFindSubsetPairingWithLowestCostContext context,
            int i,
            int set,
            double currentCost)
        {
            Debug.Assert(context != null);

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
                RecursiveFindSubsetPairingWithLowestCost(context, i + 1, set, currentCost);
            }
            else
            {
                for (var ni = i + 1; ni < context.Indices.Length; ni++)
                {
                    if (context.Indices[ni] == 0)
                    {
                        context.Indices[i] = set;
                        context.Indices[ni] = set;

                        var pairCost = context.CalculatePairCost(i, ni);
                        var newCost = currentCost + pairCost;

                        RecursiveFindSubsetPairingWithLowestCost(context, i + 1, set + 1, newCost);

                        context.Indices[ni] = 0;
                    }
                }

                context.Indices[i] = 0;
            }
        }

        /// <summary>
        /// Partitions a given integer into all possible combinations of smaller integers.
        /// </summary>
        /// <param name="number">The input number.</param>
        /// <returns>A sequence of combinations.</returns>
        public static IEnumerable<int[]> GetIntegerPartitions(this int number)
        {
            if (number != 0)
            {
                var selection = new Stack<int>();
                var numbers = new Stack<int>();
                var i = 0;
                var dontBreak = false;
                var sign = Math.Sign(number);

                for (;;)
                {
                    if (i == 0)
                    {
                        selection.Push(number);
                        yield return selection.ToArray();
                        selection.Pop();

                        i = sign;
                    }
                    else if (i * sign > number / 2 * sign || dontBreak)
                    {
                        if (selection.Count == 0)
                            break;

                        number = numbers.Pop();
                        i = selection.Pop() + sign;

                        dontBreak = false;
                    }
                    else
                    {
                        selection.Push(i);
                        numbers.Push(number);

                        dontBreak = i * 2 == number;

                        number = number - i;
                        i = 0;
                    }
                }
            }
        }

        /// <summary>
        /// Calculates the count of partitions that a <paramref name="number"/> can be split into.
        /// </summary>
        /// <param name="number">The number to split.</param>
        /// <returns>The partition count.</returns>
        public static int EvaluateIntegerPartitionCombinations(this int number)
        {
            number = Math.Abs(number);

            var solutions = new int[number + 1, number + 1];

            for (var m = 0; m <= number; m++)
            {
                for (var n = 0; n <= number; n++)
                {
                    if (m == 0)
                    {
                        solutions[n, m] = 0;
                    }
                    else if (n == 0)
                    {
                        solutions[n, m] = 1;
                    }
                    else if (n - m < 0)
                    {
                        solutions[n, m] = solutions[n, m - 1];
                    }
                    else
                    {
                        solutions[n, m] = solutions[n - m, m] + solutions[n, m - 1];
                    }
                }
            }

            return solutions[number, number];
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
        public static IEnumerable<T[][]> EvaluateAllSubsetCombinations<T>(this IList<T> sequence, int subsets)
        {
            Validate.ArgumentNotNull(nameof(sequence), sequence);
            Validate.ArgumentGreaterThanZero(nameof(subsets), subsets);

            if (sequence.Count == 0)
            {
                yield break;
            }

            var resultSets = new List<T>[subsets];
            for (var i = 0; i < resultSets.Length; i++)
            {
                resultSets[i] = new List<T>();
            }

            var stack = new Stack<Step>();
            stack.Push(new Step(0, 0));

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

                    stack.Push(new Step(step.ItemIndex, step.SetIndex + 1));

                    if (step.ItemIndex == sequence.Count - 1)
                    {
                        yield return resultSets.Select(s => s.ToArray()).ToArray();
                    }
                    else
                    {
                        stack.Push(new Step(step.ItemIndex + 1, 0));
                    }
                }
            }
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
        public static T[][] FindSubsetsWithEqualAggregateValue<T>(
            this IList<T> sequence, 
            Aggregator<T> aggregator, 
            IComparer<T> comparer, 
            int subsets)
        {
            Validate.ArgumentNotNull(nameof(sequence), sequence);
            Validate.ArgumentNotNull(nameof(aggregator), aggregator);
            Validate.ArgumentNotNull(nameof(comparer), comparer);
            Validate.ArgumentGreaterThanZero(nameof(subsets), subsets);

            foreach (var combo in EvaluateAllSubsetCombinations(sequence, subsets))
            {
                var firstSum = default(T);
                var allEqual = true;
                for (var i = 0; i < subsets; i++)
                {
                    var subsetSum = combo[i].Length == 0 ? default(T) : combo[i].Aggregate((current, item) => aggregator(current, item));
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

            return new T[][] {};
        }

        /// <summary>
        /// Finds the minimum number of sets that cover the full set of elements.
        /// </summary>
        /// <typeparam name="T">The type of elements in the set.</typeparam>
        /// <param name="sets">The sets.</param>
        /// <param name="comparer">The comparer.</param>
        /// <returns>A sequence of selected sets whose union results in the full coverage.</returns>
        /// <exception cref="ArgumentNullException">Thrown if either <paramref name="sets"/> or <paramref name="comparer"/> is <c>null</c>.</exception>
        public static IEnumerable<ISet<T>> FindMinimumNumberOfSetsWithFullCoverage<T>(IEnumerable<ISet<T>> sets, IEqualityComparer<T> comparer)
        {
            Validate.ArgumentNotNull(nameof(sets), sets);
            Validate.ArgumentNotNull(nameof(comparer), comparer);

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

                superSet.ExceptWith(bestSet);
                copies.Remove(bestSet);
                yield return bestSet;
            }
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
        public static Tuple<T, T>[] FindSubsetPairingWithLowestCost<T>(
            this IList<T> sequence,
            Func<T, T, double> evaluateCostOfPairFunc)
        {
            Validate.CollectionArgumentsHasEvenNumberOfElements(nameof(sequence), sequence);
            Validate.ArgumentNotNull(nameof(evaluateCostOfPairFunc), evaluateCostOfPairFunc);

            if (sequence.Count == 0)
            {
                return new Tuple<T, T>[0];
            }

            var context = new RecursiveFindSubsetPairingWithLowestCostContext
            {
                CalculatePairCost = (l, r) =>
                {
                    Debug.Assert(l >= 0 && l < sequence.Count);
                    Debug.Assert(r >= 0 && r < sequence.Count);
                    Debug.Assert(l != r);

                    return evaluateCostOfPairFunc(sequence[l], sequence[r]);
                },
                Indices = new int[sequence.Count]
            };

            RecursiveFindSubsetPairingWithLowestCost(context, 0, 1, 0);

            var sets = new Pair<T>[sequence.Count / 2];
            for (var i = 0; i < context.BestCombination.Length; i++)
            {
                var setIndex = context.BestCombination[i] - 1;
                Debug.Assert(setIndex >= 0);

                if (sets[setIndex] == null)
                {
                    sets[setIndex] = new Pair<T> {Item1 = sequence[i]};
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
        public static Tuple<T, T>[] ApproximateSubsetPairingWithLowestCost<T>(
            this IList<T> sequence,
            Func<T, T, double> evaluateCostOfPairFunc,
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
            var result = HeuristicAlgorithms.ApplySimulatedAnnealing(sequence, 2, pair =>
            {
                Debug.Assert(pair != null && pair.Length == 2);
                return evaluateCostOfPairFunc(pair[0], pair[1]);
            }, 
            new HeuristicAlgorithms.SimulatedAnnealingParams(coolingSteps: steps, iterationsPerCoolingStep: steps));

            return result.Select(pair =>
            {
                Debug.Assert(pair != null && pair.Length == 2);
                return Tuple.Create(pair[0], pair[1]);
            }).ToArray();
        }
    }
}