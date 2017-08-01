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
    using Containers;
    using JetBrains.Annotations;
    using System.Text;

    /// <summary>
    /// Class provides a large number of algorithms to use on sequences.
    /// </summary>
    [PublicAPI]
    public static class SequenceExtensions
    {
        private const double CombSortShrinkFactor = 1.3;
        private const double ShellSortShrinkFactor = 2.2;

        private static void BuildHeap<T>(IList<T> sequence, int lo, int hi, IComparer<T> comparer)
        {
            Debug.Assert(sequence != null);
            Debug.Assert(lo >= 0);
            Debug.Assert(hi < sequence.Count);
            Debug.Assert(lo <= hi);
            Debug.Assert(comparer != null);

            for (var tailIndex = lo; tailIndex <= hi; tailIndex++)
            {
                var j = tailIndex;
                while (j > lo)
                {
                    var parentIndex = (j - (j % 2 == 0 ? 2 : 1)) / 2;
                    if (comparer.Compare(sequence[parentIndex], sequence[j]) < 0)
                    {
                        var temp = sequence[parentIndex];
                        sequence[parentIndex] = sequence[j];
                        sequence[j] = temp;

                        j = parentIndex;
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        private static void ReorderHeap<T>(IList<T> sequence, int lo, int hi, IComparer<T> comparer)
        {
            Debug.Assert(sequence != null);
            Debug.Assert(lo >= 0);
            Debug.Assert(hi < sequence.Count);
            Debug.Assert(lo <= hi);
            Debug.Assert(comparer != null);

            for (var tailIndex = hi; tailIndex > lo; tailIndex--)
            {
                var last = sequence[tailIndex];
                sequence[tailIndex] = sequence[lo];
                sequence[lo] = last;

                var j = lo;
                for (;;)
                {
                    var leftIndex = ((j - lo) << 1) + 1 + lo;
                    if (leftIndex >= tailIndex)
                    {
                        break;
                    }

                    var rightIndex = leftIndex + 1;
                    if (comparer.Compare(sequence[j], sequence[leftIndex]) < 0 &&
                        (rightIndex >= tailIndex || comparer.Compare(sequence[leftIndex], sequence[rightIndex]) > 0))
                    {
                        var temp = sequence[leftIndex];
                        sequence[leftIndex] = sequence[j];
                        sequence[j] = temp;

                        j = leftIndex;
                    }
                    else if (rightIndex < tailIndex && comparer.Compare(sequence[j], sequence[rightIndex]) < 0)
                    {
                        var temp = sequence[rightIndex];
                        sequence[rightIndex] = sequence[j];
                        sequence[j] = temp;

                        j = rightIndex;
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        private static int PartitionSegment<T>(IList<T> sequence, int lo, int hi, IComparer<T> comparer)
        {
            Debug.Assert(sequence != null);
            Debug.Assert(lo >= 0);
            Debug.Assert(hi < sequence.Count);
            Debug.Assert(lo <= hi);
            Debug.Assert(comparer != null);

            var pi = hi;
            var li = lo;
            while (li < pi)
            {
                if (comparer.Compare(sequence[pi - 1], sequence[pi]) > 0)
                {
                    var temp = sequence[pi];
                    sequence[pi] = sequence[pi - 1];
                    sequence[pi - 1] = temp;
                    pi--;
                    li = lo;
                }
                else
                {
                    if (comparer.Compare(sequence[li], sequence[pi - 1]) > 0)
                    {
                        var temp = sequence[li];
                        sequence[li] = sequence[pi - 1];
                        sequence[pi - 1] = temp;
                    }
                    li++;
                }
            }

            return pi;
        }

        private static void QuickSortRecursive<T>(IList<T> sequence, int lo, int hi, IComparer<T> comparer)
        {
            Debug.Assert(sequence != null);
            Debug.Assert(lo >= 0);
            Debug.Assert(hi < sequence.Count);
            Debug.Assert(lo <= hi);
            Debug.Assert(comparer != null);

            if (hi - lo == 1)
            {
                if (comparer.Compare(sequence[lo], sequence[hi]) > 0)
                {
                    var temp = sequence[lo];
                    sequence[lo] = sequence[hi];
                    sequence[hi] = temp;
                }
            }
            else
            {
                var pivotIndex = PartitionSegment(sequence, lo, hi, comparer);
                if (pivotIndex - lo > 1)
                {
                    QuickSortRecursive(sequence, lo, pivotIndex - 1, comparer);
                }
                if (hi - pivotIndex > 1)
                {
                    QuickSortRecursive(sequence, pivotIndex + 1, hi, comparer);
                }
            }
        }

        private static void MergeSegments<T>(IList<T> sequence, int llo, int lhi, int rlo, int rhi,
            IComparer<T> comparer)
        {
            Debug.Assert(sequence != null);
            Debug.Assert(comparer != null);
            Debug.Assert(llo >= 0);
            Debug.Assert(lhi < sequence.Count);
            Debug.Assert(llo <= lhi);
            Debug.Assert(rlo >= 0);
            Debug.Assert(rhi < sequence.Count);
            Debug.Assert(rlo <= rhi);
            Debug.Assert(rlo > lhi);

            var mergeLength = (lhi - llo) + (rhi - rlo) + 2;
            var mergeArray = new T[mergeLength];

            var li = llo;
            var ri = rlo;
            var mi = 0;

            while (mi < mergeLength)
            {
                T next;
                if (li <= lhi && ri <= rhi)
                {
                    next = comparer.Compare(sequence[li], sequence[ri]) < 0 ? sequence[li++] : sequence[ri++];
                }
                else if (li > lhi)
                {
                    next = sequence[ri++];
                }
                else
                {
                    next = sequence[li++];
                }

                mergeArray[mi++] = next;
            }

            for (var x = 0; x < mergeLength; x++)
            {
                sequence[x + llo] = mergeArray[x];
            }
        }

        private static void MergeSortSegments<T>(IList<T> sequence, int lo, int hi, IComparer<T> comparer)
        {
            Debug.Assert(sequence != null);
            Debug.Assert(comparer != null);
            Debug.Assert(lo >= 0);
            Debug.Assert(hi < sequence.Count);
            Debug.Assert(lo <= hi);

            if (lo == hi)
            {
                return;
            }

            var middle = (lo + hi) / 2;
            MergeSortSegments(sequence, lo, middle, comparer);
            MergeSortSegments(sequence, middle + 1, hi, comparer);

            MergeSegments(sequence, lo, middle, middle + 1, hi, comparer);
        }

        private struct EditChoice
        {
            public const int Cancel = -1;
            public const int Match = 0;
            public const int Insert = 1;
            public const int Delete = 2;

            public int Operation;
            public double Cost;
        }

        [NotNull]
        private static Edit<T>[] GetEditDistance<T>(
            [NotNull] this IList<T> sequence,
            [NotNull] IList<T> resultSequence,
            [NotNull] Func<int, EditChoice> initRowCellFunc,
            [NotNull] Func<int, EditChoice> initColumnCellFunc,
            [NotNull] Func<T, T, double> evalMatchCostFunc,
            [NotNull] Func<T, double> evalInsertCostFunc,
            [NotNull] Func<T, double> evalDeleteCostFunc)
        {
            Debug.Assert(sequence != null);
            Debug.Assert(resultSequence != null);
            Debug.Assert(evalMatchCostFunc != null);
            Debug.Assert(evalInsertCostFunc != null);
            Debug.Assert(evalDeleteCostFunc != null);

            var matrix = new EditChoice[sequence.Count + 1, resultSequence.Count + 1];
            for (var i = 0; i < Math.Max(sequence.Count, resultSequence.Count) + 1; i++)
            {
                if (i <= sequence.Count)
                    matrix[i, 0] = initColumnCellFunc(i);
                if (i <= resultSequence.Count)
                    matrix[0, i] = initRowCellFunc(i);
            }

            var opr = new double[3];
            for (var i1 = 1; i1 <= sequence.Count; i1++)
            {
                for (var i2 = 1; i2 <= resultSequence.Count; i2++)
                {
                    opr[EditChoice.Match] = matrix[i1 - 1, i2 - 1].Cost +
                                            evalMatchCostFunc(sequence[i1 - 1], resultSequence[i2 - 1]);
                    opr[EditChoice.Delete] = matrix[i1 - 1, i2].Cost + evalDeleteCostFunc(sequence[i1 - 1]);
                    opr[EditChoice.Insert] = matrix[i1, i2 - 1].Cost + evalInsertCostFunc(resultSequence[i2 - 1]);

                    var minCostOperation = -1;
                    var minCost = opr[EditChoice.Match];

                    for (var op = EditChoice.Match; op <= EditChoice.Delete; op++)
                    {
                        if (minCostOperation == -1 || opr[op] < minCost)
                        {
                            minCost = opr[op];
                            minCostOperation = op;
                        }
                    }

                    matrix[i1, i2] = new EditChoice
                    {
                        Cost = minCost,
                        Operation = minCostOperation,
                    };
                }
            }

            var path = new List<Edit<T>>();
            var pi1 = sequence.Count;
            var pi2 = resultSequence.Count;
            for (;;)
            {
                // ReSharper disable once IdentifierTypo
                var ceds = matrix[pi1, pi2];
                if (ceds.Operation == EditChoice.Cancel)
                {
                    break;
                }

                Edit<T> edit;
                switch (ceds.Operation)
                {
                    case EditChoice.Match:
                        var actualEdit = Equals(sequence[pi1 - 1], resultSequence[pi2 - 1])
                            ? EditOperation.Match
                            : EditOperation.Substitute;
                        edit = new Edit<T>(actualEdit, resultSequence[pi2 - 1]);
                        pi1--;
                        pi2--;
                        break;
                    case EditChoice.Insert:
                        edit = new Edit<T>(EditOperation.Insert, resultSequence[pi2 - 1]);
                        pi2--;
                        break;
                    case EditChoice.Delete:
                        edit = new Edit<T>(EditOperation.Delete, sequence[pi1 - 1]);
                        pi1--;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                path.Add(edit);
            }

            path.Reverse();
            return path.ToArray();
        }


        /// <summary>
        /// Finds the longest increasing sequence in a given <paramref name="sequence"/>.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="sequence">The sequence to verify.</param>
        /// <param name="comparer">The comparer used to compare the elements in the sequence.</param>
        /// <returns>The longest increasing sequence.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the <paramref name="sequence"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">Thrown if the <paramref name="comparer"/> is <c>null</c>.</exception>
        [NotNull]
        public static IEnumerable<T> FindLongestIncreasingSequence<T>([NotNull] this IEnumerable<T> sequence,
            [NotNull] IComparer<T> comparer)
        {
            Validate.ArgumentNotNull(nameof(sequence), sequence);
            Validate.ArgumentNotNull(nameof(comparer), comparer);

            // ReSharper disable once IdentifierTypo
            var dyna = new List<Tuple<T, int, int>>();
            var li = -1;
            var lm = 0;
            foreach (var e in sequence)
            {
                var pi = -1;
                var pm = 0;

                for (var i = dyna.Count - 1; i >= 0; i--)
                {
                    if (comparer.Compare(dyna[i].Item1, e) < 0 && (pi == -1 || pm < dyna[i].Item2))
                    {
                        pi = i;
                        pm = dyna[i].Item2;
                    }
                }

                var nm = pm + 1;
                dyna.Add(Tuple.Create(e, nm, pi));

                if (lm < nm)
                {
                    lm = nm;
                    li = dyna.Count - 1;
                }
            }

            var result = new T[lm];
            while (li > -1)
            {
                result[--lm] = dyna[li].Item1;
                li = dyna[li].Item3;
            }

            return result;
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
        public static IEnumerable<int> FindSubsetWithClosestAggregatedValue([NotNull] this IEnumerable<int> sequence, int target)
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
            {
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
        public static bool ContainsSubsetWithAggregatedValue([NotNull] this IEnumerable<int> sequence, int target)
        {
            return FindSubsetWithClosestAggregatedValue(sequence, target).Sum() == target;
        }

        /// <summary>
        /// Determines whether the sequence contains two elements that target to a given <paramref name="target"/> value.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="sequence">The sequence to check.</param>
        /// <param name="target">The target value to search for.</param>
        /// <param name="aggregator">The function that aggregates two values.</param>
        /// <param name="comparer">The comparer.</param>
        /// <returns>
        ///   <c>true</c> if the <paramref name="sequence"/> contains two elements that target; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="sequence"/>, <paramref name="aggregator"/> or <paramref name="comparer"/> are null.</exception>
        public static bool ContainsTwoElementsThatAggregateTo<T>(
            [NotNull] this IEnumerable<T> sequence, 
            [NotNull] Aggregator<T> aggregator, 
            [NotNull] IComparer<T> comparer, 
            T target)
        {
            Validate.ArgumentNotNull(nameof(sequence), sequence);
            Validate.ArgumentNotNull(nameof(aggregator), aggregator);
            Validate.ArgumentNotNull(nameof(comparer), comparer);

            var array = sequence.ToArray();
            if (array.Length == 0)
            {
                return false;
            }

            array.QuickSort(0, array.Length, comparer);
            var i = 0;
            var j = array.Length - 1;
            while (i < j)
            {
                var a = aggregator(array[i], array[j]);
                var cr = comparer.Compare(a, target);

                if (cr == 0)
                {
                    return true;
                }
                if (cr > 0)
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
        /// Finds the elements, which summed, yield the biggest sum.
        /// </summary>
        /// <typeparam name="T">The type of elements in the <paramref name="sequence"/>.</typeparam>
        /// <param name="sequence">The sequence of elements.</param>
        /// <param name="count">The count of elements to consider.</param>
        /// <param name="aggregator">The aggregator function which sums elements.</param>
        /// <param name="comparer">The comparer.</param>
        /// <returns>An array of elements with the highest sum.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="sequence"/>, <paramref name="aggregator"/> or <paramref name="comparer"/> are null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the <paramref name="count"/> is greater than the number of elements in <paramref name="sequence"/>.</exception>
        [NotNull]
        public static T[] FindSubsetWithGreatestAggregatedValue<T>(
            [NotNull] this IEnumerable<T> sequence, int count, 
            [NotNull] Aggregator<T> aggregator, [NotNull] IComparer<T> comparer)
        {
            Validate.ArgumentNotNull(nameof(sequence), sequence);
            Validate.ArgumentNotNull(nameof(aggregator), aggregator);
            Validate.ArgumentNotNull(nameof(comparer), comparer);
            Validate.ArgumentGreaterThanOrEqualTo(nameof(count), count, 1);

            var array = sequence.ToArray();
            Validate.ArgumentLessThanOrEqualTo(nameof(count), count, array.Length);

            array.QuickSort(0, array.Length, comparer);

            var result = new T[count];
            Array.Copy(array, array.Length - count, result, 0, count);

            return result;
        }

        /// <summary>
        /// Finds all duplicate items in a given <paramref name="sequence"/>.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
        /// <param name="sequence">The sequence to inspect.</param>
        /// <param name="equalityComparer">The comparer used to verify the elements in the sequence.</param>
        /// <returns>A sequence of element-appearances pairs of the detected duplicates.</returns>
        /// <exception cref="ArgumentNullException">Thrown if either the <paramref name="sequence"/> or the <paramref name="equalityComparer"/> are <c>null</c>.</exception>
        [NotNull]
        public static KeyValuePair<T, int>[] FindDuplicates<T>(
            [NotNull] this IEnumerable<T> sequence, [NotNull] IEqualityComparer<T> equalityComparer)
        {
            Validate.ArgumentNotNull(nameof(sequence), sequence);
            Validate.ArgumentNotNull(nameof(equalityComparer), equalityComparer);

            var appearances = new Dictionary<T, int>(equalityComparer);
            foreach (var item in sequence)
            {
                if (!appearances.TryGetValue(item, out int count))
                {
                    appearances.Add(item, 1);
                }
                else
                {
                    appearances[item] = count + 1;
                }
            }

            return appearances.Where(kvp => kvp.Value > 1).ToArray();
        }

        /// <summary>
        /// Finds all duplicate integers in a given <paramref name="sequence"/>.
        /// </summary>
        /// <param name="sequence">The sequence to inspect.</param>
        /// <param name="minInSequence">The minimum possible value of an element part of the <paramref name="sequence"/>.</param>
        /// <param name="maxInSequence">The maximum possible value of an element part of the <paramref name="sequence"/>.</param>
        /// <returns>A sequence of element-appearances pairs of the detected duplicates.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="sequence"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="maxInSequence"/> is less than <paramref name="minInSequence"/>.</exception>
        [NotNull]
        public static KeyValuePair<int, int>[] FindDuplicates(
            [NotNull] this IEnumerable<int> sequence, int minInSequence, int maxInSequence)
        {
            Validate.ArgumentNotNull(nameof(sequence), sequence);
            Validate.ArgumentGreaterThanOrEqualTo(nameof(maxInSequence), maxInSequence, minInSequence);

            var appearances = new int[maxInSequence - minInSequence + 1];
            foreach (var item in sequence)
            {
                if (item < minInSequence || item > maxInSequence)
                    throw new InvalidOperationException($"The sequence of integers contains element {item} which is outside of the given {minInSequence}..{maxInSequence} range.");

                appearances[item - minInSequence]++;
            }

            return appearances.Select((a, i) => new KeyValuePair<int, int>(i + minInSequence, a)).Where(kvp => kvp.Value > 1).ToArray();
        }

        /// <summary>
        /// Finds all duplicate characters in a given <paramref name="sequence"/>.
        /// </summary>
        /// <param name="sequence">The sequence to inspect.</param>
        /// <returns>A sequence of element-appearances pairs of the detected duplicates.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="sequence"/> is <c>null</c>.</exception>
        [NotNull]
        public static KeyValuePair<char, int>[] FindDuplicates([NotNull] this string sequence)
        {
            Validate.ArgumentNotNull(nameof(sequence), sequence);

            var asciiAppearances = new int[byte.MaxValue + 1];
            var appearances = new Dictionary<char, int>();

            foreach (var item in sequence)
            {
                if (item <= byte.MaxValue)
                {
                    asciiAppearances[item]++;
                }
                else
                {
                    if (!appearances.TryGetValue(item, out int count))
                    {
                        appearances.Add(item, 1);
                    }
                    else
                    {
                        appearances[item] = count + 1;
                    }
                }
            }

            return
                asciiAppearances
                .Select((a, i) => new KeyValuePair<char, int>((char)i, a))
                .Concat(appearances)
                .Where(kvp => kvp.Value > 1).ToArray();
        }

        [NotNull]
        [ItemNotNull]
        private static IEnumerable<T[]> ExtractNestedBlocksIterate<T>(
            [NotNull] this IEnumerable<T> sequence,
            T openBracket,
            T closeBracket,
            [NotNull] IEqualityComparer<T> comparer)
        {
            Debug.Assert(sequence != null);
            Debug.Assert(comparer != null);

            var stack = new Stack<List<T>>();
            var currentList = new List<T>();
            foreach (var item in sequence)
            {
                if (comparer.Equals(item, openBracket))
                {
                    currentList.Add(item);

                    stack.Push(currentList);
                    currentList = new List<T>();
                }
                else if (comparer.Equals(item, closeBracket))
                {
                    yield return currentList.ToArray();
                    if (stack.Count == 0)
                    {
                        throw new InvalidOperationException("There are no blocks open to be closed.");
                    }

                    var previousList = stack.Pop();
                    previousList.AddRange(currentList);
                    currentList = previousList;
                    currentList.Add(item);
                }
                else
                {
                    currentList.Add(item);
                }
            }

            if (stack.Count > 0)
            {
                throw new InvalidOperationException($"There are {stack.Count} number of blocks that have not been closed.");
            }

            if (currentList.Count > 0)
            {
                yield return currentList.ToArray();
            }
        }

        /// <summary>
        /// Extracts all nested groups from sequence. The method returns a sequence of sequences.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="sequence"/>.</typeparam>
        /// <param name="sequence">The sequence.</param>
        /// <param name="openBracket">The element that signifies the start of a group.</param>
        /// <param name="closeBracket">The element that signifies the end of a group.</param>
        /// <param name="comparer">The equality comparer for the elements of the <paramref name="sequence"/>.</param>
        /// <returns>The sequence of extracted groups, starting with the inner most ones.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="sequence"/> or <paramref name="comparer"/> are <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">Throws if the number of open and close brackets do not match.</exception>
        [NotNull]
        [ItemNotNull]
        public static IEnumerable<T[]> ExtractNestedBlocks<T>(
            [NotNull] this IEnumerable<T> sequence, 
            T openBracket, 
            T closeBracket,
            [NotNull] IEqualityComparer<T> comparer)
        {
            Validate.ArgumentNotNull(nameof(sequence), sequence);
            Validate.ArgumentNotNull(nameof(comparer), comparer);

            return ExtractNestedBlocksIterate(sequence, openBracket, closeBracket, comparer);
        }

        [NotNull]
        private static IEnumerable<T[]> FindSubsequencesWithGivenAggregatedValueIterate<T>(
            [NotNull] this IList<T> sequence,
            [NotNull] Aggregator<T> aggregator,
            [NotNull] Aggregator<T> disaggregator,
            [NotNull] IComparer<T> comparer,
            T target)
        {
            Debug.Assert(sequence != null);
            Debug.Assert(aggregator != null);
            Debug.Assert(disaggregator != null);
            Debug.Assert(comparer != null);

            if (sequence.Count == 0)
            {
                yield break;
            }

            var s = 0;
            var e = 0;
            var sum = sequence[s];

            while (e < sequence.Count)
            {
                var cmp = comparer.Compare(sum, target);
                if (cmp < 0)
                {
                    e++;
                    if (e < sequence.Count)
                    {
                        sum = aggregator(sum, sequence[e]);
                    }
                }
                else if (cmp > 0)
                {
                    sum = disaggregator(sum, sequence[s]);
                    s++;
                }
                else
                {
                    var l = e - s + 1;
                    var t = new T[l];
                    for (var i = 0; i < l; i++)
                    {
                        t[i] = sequence[i + s];
                    }

                    yield return t;

                    e++;
                    if (e < sequence.Count)
                    {
                        sum = aggregator(sum, sequence[e]);
                    }
                }
            }
        }

        /// <summary>
        /// Finds the sub-sequences whose aggregated values are equal to a given <paramref name="target"/> value.
        /// </summary>
        /// <param name="sequence">The sequence to check.</param>
        /// <param name="aggregator">The value aggregator.</param>
        /// <param name="disaggregator">The value dis-aggregator.</param>
        /// <param name="comparer">The comparer.</param>
        /// <param name="target">The target aggregated value.</param>
        /// <returns>A sequence of found integers.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the <paramref name="sequence"/> is <c>null</c>.</exception>
        [NotNull]
        public static IEnumerable<T[]> FindSubsequencesWithGivenAggregatedValue<T>(
            [NotNull] this IList<T> sequence,
            [NotNull] Aggregator<T> aggregator,
            [NotNull] Aggregator<T> disaggregator,
            [NotNull] IComparer<T> comparer, 
            T target)
        {
            Validate.ArgumentNotNull(nameof(sequence), sequence);
            Validate.ArgumentNotNull(nameof(aggregator), aggregator);
            Validate.ArgumentNotNull(nameof(disaggregator), disaggregator);
            Validate.ArgumentNotNull(nameof(comparer), comparer);

            return FindSubsequencesWithGivenAggregatedValueIterate(sequence, aggregator, disaggregator, comparer, target);
        }

        [NotNull]
        private static IEnumerable<T> InterleaveIterate<T>(
            [NotNull] IComparer<T> comparer,
            [NotNull] IEnumerable<T> sequence,
            [NotNull] [ItemNotNull] params IEnumerable<T>[] sequences)
        {
            Debug.Assert(comparer != null);
            Debug.Assert(sequence != null);
            Debug.Assert(sequences != null);

            var innerComparer = Comparer<IEnumerator<T>>.Create((a, b) => comparer.Compare(a.Current, b.Current));

            var heap = new Heap<IEnumerator<T>>(innerComparer);
            for (var i = -1; i < sequences.Length; i++)
            {
                // ReSharper disable once PossibleMultipleEnumeration
                var enumerator = i < 0 ? sequence.GetEnumerator() : sequences[i].GetEnumerator();
                if (enumerator.MoveNext())
                {
                    heap.Add(enumerator);
                }
            }

            while (heap.Count > 0)
            {
                var top = heap.RemoveTop();
                var c = top.Current;
                yield return c;

                if (top.MoveNext())
                {
                    if (comparer.Compare(top.Current, c) > 0)
                    {
                        throw new InvalidOperationException("One or more sequence contains unsorted items.");
                    }

                    heap.Add(top);
                }
            }
        }

        /// <summary>
        /// Interleaves multiple sequences into one output sequence.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequences.</typeparam>
        /// <param name="comparer">The comparer.</param>
        /// <param name="sequence">The first sequence to interleave.</param>
        /// <param name="sequences">The next sequences to interleave.</param>
        /// <returns>A new interleaved stream.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the <paramref name="comparer"/> or <paramref name="sequences"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown if the <paramref name="sequences"/> is empty.</exception>
        /// <exception cref="InvalidOperationException">Thrown if one or more sequences contain unsorted items.</exception>
        [NotNull]
        public static IEnumerable<T> Interleave<T>(
            [NotNull] IComparer<T> comparer, 
            [NotNull] IEnumerable<T> sequence, 
            [NotNull] [ItemNotNull] params IEnumerable<T>[] sequences)
        {
            Validate.ArgumentNotNull(nameof(comparer), comparer);
            Validate.ArgumentNotNull(nameof(sequence), sequence);
            Validate.ArgumentNotNull(nameof(sequences), sequences);

            return InterleaveIterate(comparer, sequence, sequences);
        }

        /// <summary>
        /// Reverses a given <paramref name="sequence"/> in place (mutating the original).
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="sequence">The sequence to reverse.</param>
        /// <param name="startIndex">The start index in the sequence.</param>
        /// <param name="length">The length of sequence to reverse.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="sequence"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the combination of <paramref name="startIndex"/> and <paramref name="length"/> is out of bounds.</exception>
        public static void Reverse<T>(this IList<T> sequence, int startIndex, int length)
        {
            Validate.CollectionArgumentsInBounds(nameof(sequence), sequence, startIndex, length);

            for (var i = 0; i < length / 2; i++)
            {
                var swap = sequence[i + startIndex];
                sequence[i + startIndex] = sequence[length - i + startIndex - 1];
                sequence[length - i + startIndex - 1] = swap;
            }
        }

        /// <summary>
        /// Creates an array whose contents are the elements of the <paramref name="input"/> repeated <paramref name="repetitions"/> times.
        /// </summary>
        /// <typeparam name="T">The type of the sequence's elements</typeparam>
        /// <param name="input">The input sequence.</param>
        /// <param name="repetitions">Number of times to repeat the sequence.</param>
        /// <returns>A new array.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the <paramref name="input"/> sequence is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the value of <paramref name="repetitions"/> argument is less than <c>1</c>.</exception>
        public static T[] Repeat<T>(this IEnumerable<T> input, int repetitions)
        {
            Validate.ArgumentNotNull(nameof(input), input);
            Validate.ArgumentGreaterThanZero(nameof(repetitions), repetitions);

            var arrayInput = input.ToArray();
            var arrayOutput = new T[arrayInput.Length * repetitions];
            var outputIndex = 0;

            while (repetitions > 0)
            {
                if (repetitions % 2 == 1)
                {
                    Array.Copy(arrayInput, 0, arrayOutput, outputIndex, arrayInput.Length);
                    outputIndex += arrayInput.Length;
                }

                repetitions >>= 1;
                if (repetitions > 0)
                {
                    var currentLength = arrayInput.Length;
                    Array.Resize(ref arrayInput, currentLength * 2);
                    Array.Copy(arrayInput, 0, arrayInput, currentLength, currentLength);
                }
            }

            return arrayOutput;
        }

        /// <summary>
        /// Finds the location of <paramref name="item"/> in the given <paramref name="sequence"/>.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="sequence">The sequence to search.</param>
        /// <param name="startIndex">The start index in the sequence.</param>
        /// <param name="length">The length of sequence to search..</param>
        /// <param name="item">The item to search for.</param>
        /// <param name="comparer">Comparer used in the search.</param>
        /// <param name="ascending">Specifies whether the sequence is sorted in ascending or descending order.</param>
        /// <returns>The index in the sequence where the <paramref name="item"/> was found; <c>-1</c> otherwise.</returns>
        /// <exception cref="ArgumentNullException">Thrown if either <paramref name="sequence"/> or <paramref name="comparer"/> are <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the combination of <paramref name="startIndex"/> and <paramref name="length"/> is out of bounds.</exception>
        public static int BinarySearch<T>(this IList<T> sequence, int startIndex, int length, T item, IComparer<T> comparer, bool ascending = true)
        {
            Validate.CollectionArgumentsInBounds(nameof(sequence), sequence, startIndex, length);
            Validate.ArgumentNotNull(nameof(comparer), comparer);

            var start = startIndex;
            var end = startIndex + length - 1;
            var direction = ascending ? 1 : -1;

            while (start <= end)
            {
                var mid = (start + end) / 2;
                var compareResult = direction * comparer.Compare(sequence[mid], item);

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

        /// <summary>
        /// Bubble-Sorts the <paramref name="sequence"/> using the provided <paramref name="comparer"/>.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="sequence">The sequence to sort.</param>
        /// <param name="startIndex">The start index in the sequence.</param>
        /// <param name="length">The length of sequence to sort.</param>
        /// <param name="comparer">Comparer used in the search.</param>
        /// <exception cref="ArgumentNullException">Thrown if either <paramref name="sequence"/> or <paramref name="comparer"/> are <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the combination of <paramref name="startIndex"/> and <paramref name="length"/> is out of bounds.</exception>
        public static void BubbleSort<T>(this IList<T> sequence, int startIndex, int length, IComparer<T> comparer)
        {
            Validate.CollectionArgumentsInBounds(nameof(sequence), sequence, startIndex, length);
            Validate.ArgumentNotNull(nameof(comparer), comparer);

            var swapped = true;
            while (swapped)
            {
                swapped = false;
                for (var i = startIndex; i < startIndex + length - 1; i++)
                {
                    if (comparer.Compare(sequence[i], sequence[i + 1]) > 0)
                    {
                        var temp = sequence[i];
                        sequence[i] = sequence[i + 1];
                        sequence[i + 1] = temp;

                        swapped = true;
                    }
                }
            }
        }

        /// <summary>
        /// Cocktail-Shaker-Sorts the <paramref name="sequence"/> using the provided <paramref name="comparer"/>.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="sequence">The sequence to sort.</param>
        /// <param name="startIndex">The start index in the sequence.</param>
        /// <param name="length">The length of sequence to sort.</param>
        /// <param name="comparer">Comparer used in the search.</param>
        /// <exception cref="ArgumentNullException">Thrown if either <paramref name="sequence"/> or <paramref name="comparer"/> are <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the combination of <paramref name="startIndex"/> and <paramref name="length"/> is out of bounds.</exception>
        public static void CocktailShakerSort<T>(this IList<T> sequence, int startIndex, int length, IComparer<T> comparer)
        {
            Validate.CollectionArgumentsInBounds(nameof(sequence), sequence, startIndex, length);
            Validate.ArgumentNotNull(nameof(comparer), comparer);

            var swapped = true;
            var upperBound = startIndex + length - 1;
            var lowerBound = startIndex;
            while (swapped)
            {
                swapped = false;
                for (var i = lowerBound; i < upperBound; i++)
                {
                    if (comparer.Compare(sequence[i], sequence[i + 1]) > 0)
                    {
                        var temp = sequence[i];
                        sequence[i] = sequence[i + 1];
                        sequence[i + 1] = temp;

                        swapped = true;
                    }
                }

                upperBound--;

                if (swapped)
                {
                    swapped = false;
                    for (var i = upperBound; i > lowerBound; i--)
                    {
                        if (comparer.Compare(sequence[i - 1], sequence[i]) > 0)
                        {
                            var temp = sequence[i];
                            sequence[i] = sequence[i - 1];
                            sequence[i - 1] = temp;

                            swapped = true;
                        }
                    }
                }

                lowerBound++;
            }
        }

        /// <summary>
        /// Comb-Sorts the <paramref name="sequence"/> using the provided <paramref name="comparer"/>.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="sequence">The sequence to sort.</param>
        /// <param name="startIndex">The start index in the sequence.</param>
        /// <param name="length">The length of sequence to sort.</param>
        /// <param name="comparer">Comparer used in the search.</param>
        /// <exception cref="ArgumentNullException">Thrown if either <paramref name="sequence"/> or <paramref name="comparer"/> are <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the combination of <paramref name="startIndex"/> and <paramref name="length"/> is out of bounds.</exception>
        public static void CombSort<T>(this IList<T> sequence, int startIndex, int length, IComparer<T> comparer)
        {
            Validate.CollectionArgumentsInBounds(nameof(sequence), sequence, startIndex, length);
            Validate.ArgumentNotNull(nameof(comparer), comparer);

            var gap = sequence.Count;
            var finished = false;
            while (!finished)
            {
                gap = (int)Math.Floor(gap / CombSortShrinkFactor);
                gap = gap < 1 ? 1 : gap;

                finished = !(gap > 1);

                for (var i = startIndex; i < startIndex + length - gap; i++)
                {
                    if (comparer.Compare(sequence[i], sequence[i + gap]) > 0)
                    {
                        var temp = sequence[i];
                        sequence[i] = sequence[i + gap];
                        sequence[i + gap] = temp;

                        finished = false;
                    }
                }
            }
        }

        /// <summary>
        /// Gnome-Sorts the <paramref name="sequence"/> using the provided <paramref name="comparer"/>.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="sequence">The sequence to sort.</param>
        /// <param name="startIndex">The start index in the sequence.</param>
        /// <param name="length">The length of sequence to sort.</param>
        /// <param name="comparer">Comparer used in the search.</param>
        /// <exception cref="ArgumentNullException">Thrown if either <paramref name="sequence"/> or <paramref name="comparer"/> are <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the combination of <paramref name="startIndex"/> and <paramref name="length"/> is out of bounds.</exception>
        public static void GnomeSort<T>(this IList<T> sequence, int startIndex, int length, IComparer<T> comparer)
        {
            Validate.CollectionArgumentsInBounds(nameof(sequence), sequence, startIndex, length);
            Validate.ArgumentNotNull(nameof(comparer), comparer);

            var i = startIndex;
            while (i < startIndex + length - 1)
            {
                if (comparer.Compare(sequence[i], sequence[i + 1]) <= 0)
                {
                    i++;
                }
                else
                {
                    var temp = sequence[i];
                    sequence[i] = sequence[i + 1];
                    sequence[i + 1] = temp;

                    if (i > startIndex)
                    {
                        i--;
                    }
                }
            }
        }

        /// <summary>
        /// Heap-Sorts the <paramref name="sequence"/> using the provided <paramref name="comparer"/>.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="sequence">The sequence to sort.</param>
        /// <param name="startIndex">The start index in the sequence.</param>
        /// <param name="length">The length of sequence to sort.</param>
        /// <param name="comparer">Comparer used in the search.</param>
        /// <exception cref="ArgumentNullException">Thrown if either <paramref name="sequence"/> or <paramref name="comparer"/> are <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the combination of <paramref name="startIndex"/> and <paramref name="length"/> is out of bounds.</exception>
        public static void HeapSort<T>(this IList<T> sequence, int startIndex, int length, IComparer<T> comparer)
        {
            Validate.CollectionArgumentsInBounds(nameof(sequence), sequence, startIndex, length);
            Validate.ArgumentNotNull(nameof(comparer), comparer);

            if (length < 2)
            {
                return;
            }

            BuildHeap(sequence, startIndex, startIndex + length - 1, comparer);
            ReorderHeap(sequence, startIndex, startIndex + length - 1, comparer);
        }

        /// <summary>
        /// Insertion-Sorts the <paramref name="sequence"/> using the provided <paramref name="comparer"/>.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="sequence">The sequence to sort.</param>
        /// <param name="startIndex">The start index in the sequence.</param>
        /// <param name="length">The length of sequence to sort.</param>
        /// <param name="comparer">Comparer used in the search.</param>
        /// <exception cref="ArgumentNullException">Thrown if either <paramref name="sequence"/> or <paramref name="comparer"/> are <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the combination of <paramref name="startIndex"/> and <paramref name="length"/> is out of bounds.</exception>
        public static void InsertionSort<T>(this IList<T> sequence, int startIndex, int length, IComparer<T> comparer)
        {
            Validate.CollectionArgumentsInBounds(nameof(sequence), sequence, startIndex, length);
            Validate.ArgumentNotNull(nameof(comparer), comparer);

            for (var i = startIndex; i < startIndex + length - 1; i++)
            {
                var temp = sequence[i + 1];
                var j = i;
                while (j >= startIndex && comparer.Compare(temp, sequence[j]) < 0)
                {
                    sequence[j + 1] = sequence[j];
                    j--;
                }

                sequence[j + 1] = temp;
            }
        }

        /// <summary>
        /// Merge-Sorts the <paramref name="sequence"/> using the provided <paramref name="comparer"/>.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="sequence">The sequence to sort.</param>
        /// <param name="startIndex">The start index in the sequence.</param>
        /// <param name="length">The length of sequence to sort.</param>
        /// <param name="comparer">Comparer used in the search.</param>
        /// <exception cref="ArgumentNullException">Thrown if either <paramref name="sequence"/> or <paramref name="comparer"/> are <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the combination of <paramref name="startIndex"/> and <paramref name="length"/> is out of bounds.</exception>
        public static void MergeSort<T>(this IList<T> sequence, int startIndex, int length, IComparer<T> comparer)
        {
            Validate.CollectionArgumentsInBounds(nameof(sequence), sequence, startIndex, length);
            Validate.ArgumentNotNull(nameof(comparer), comparer);

            if (length > 0)
            {
                MergeSortSegments(sequence, startIndex, startIndex + length - 1, comparer);
            }
        }

        /// <summary>
        /// Odd-Even-Sorts the <paramref name="sequence"/> using the provided <paramref name="comparer"/>.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="sequence">The sequence to sort.</param>
        /// <param name="startIndex">The start index in the sequence.</param>
        /// <param name="length">The length of sequence to sort.</param>
        /// <param name="comparer">Comparer used in the search.</param>
        /// <exception cref="ArgumentNullException">Thrown if either <paramref name="sequence"/> or <paramref name="comparer"/> are <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the combination of <paramref name="startIndex"/> and <paramref name="length"/> is out of bounds.</exception>
        public static void OddEvenSort<T>(this IList<T> sequence, int startIndex, int length, IComparer<T> comparer)
        {
            Validate.CollectionArgumentsInBounds(nameof(sequence), sequence, startIndex, length);
            Validate.ArgumentNotNull(nameof(comparer), comparer);

            var swapped = true;
            var upperBound = startIndex + length - 1;
            var lowerBound = startIndex;
            while (swapped)
            {
                swapped = false;
                for (var i = lowerBound; i < upperBound; i += 2)
                {
                    if (comparer.Compare(sequence[i], sequence[i + 1]) > 0)
                    {
                        var temp = sequence[i];
                        sequence[i] = sequence[i + 1];
                        sequence[i + 1] = temp;

                        swapped = true;
                    }
                }

                for (var i = lowerBound + 1; i < upperBound; i += 2)
                {
                    if (comparer.Compare(sequence[i], sequence[i + 1]) > 0)
                    {
                        var temp = sequence[i];
                        sequence[i] = sequence[i + 1];
                        sequence[i + 1] = temp;

                        swapped = true;
                    }
                }
            }
        }

        /// <summary>
        /// Quick-Sorts the <paramref name="sequence"/> using the provided <paramref name="comparer"/>.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="sequence">The sequence to sort.</param>
        /// <param name="startIndex">The start index in the sequence.</param>
        /// <param name="length">The length of sequence to sort.</param>
        /// <param name="comparer">Comparer used in the search.</param>
        /// <exception cref="ArgumentNullException">Thrown if either <paramref name="sequence"/> or <paramref name="comparer"/> are <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the combination of <paramref name="startIndex"/> and <paramref name="length"/> is out of bounds.</exception>
        public static void QuickSort<T>(this IList<T> sequence, int startIndex, int length, IComparer<T> comparer)
        {
            Validate.CollectionArgumentsInBounds(nameof(sequence), sequence, startIndex, length);
            Validate.ArgumentNotNull(nameof(comparer), comparer);

            if (length >= 2)
            {
                QuickSortRecursive(sequence, startIndex, startIndex + length - 1, comparer);
            }
        }

        /// <summary>
        /// Shell-Sorts the <paramref name="sequence"/> using the provided <paramref name="comparer"/>.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="sequence">The sequence to sort.</param>
        /// <param name="startIndex">The start index in the sequence.</param>
        /// <param name="length">The length of sequence to sort.</param>
        /// <param name="comparer">Comparer used in the search.</param>
        /// <exception cref="ArgumentNullException">Thrown if either <paramref name="sequence"/> or <paramref name="comparer"/> are <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the combination of <paramref name="startIndex"/> and <paramref name="length"/> is out of bounds.</exception>
        public static void ShellSort<T>(this IList<T> sequence, int startIndex, int length, IComparer<T> comparer)
        {
            Validate.CollectionArgumentsInBounds(nameof(sequence), sequence, startIndex, length);
            Validate.ArgumentNotNull(nameof(comparer), comparer);

            var gap = sequence.Count;
            var finished = false;
            while (!finished)
            {
                gap = (int)Math.Floor(gap / ShellSortShrinkFactor);
                if (gap < 1)
                {
                    gap = 1;
                    finished = true;
                }

                for (var i = startIndex; i < startIndex + length - gap; i++)
                {
                    var temp = sequence[i + gap];
                    var j = i;
                    while (j >= startIndex + gap - 1 && comparer.Compare(temp, sequence[j]) < 0)
                    {
                        sequence[j + gap] = sequence[j];
                        j -= gap;
                    }

                    sequence[j + gap] = temp;
                }
            }
        }

        /// <summary>
        /// Evaluates the edit distance between two given sequences <paramref name="sequence"/> and <paramref name="resultSequence"/>.
        /// </summary>
        /// <typeparam name="T">The type of elements in both sequences.</typeparam>
        /// <param name="sequence">The sequence to compare to.</param>
        /// <param name="resultSequence">The sequence to compare with.</param>
        /// <returns>A sequence of "edits" applied to the original <paramref name="sequence"/> to obtain the <paramref name="resultSequence"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown if either <paramref name="sequence"/> or <paramref name="resultSequence"/> are <c>null</c>.</exception>
        public static Edit<T>[] Diff<T>(this IList<T> sequence, IList<T> resultSequence)
        {
            Validate.ArgumentNotNull(nameof(sequence), sequence);
            Validate.ArgumentNotNull(nameof(resultSequence), resultSequence);

            return GetEditDistance(sequence, resultSequence,
                row =>
                {
                    Debug.Assert(row >= 0 && row <= resultSequence.Count);
                    return new EditChoice
                    {
                        Operation = row > 0 ? EditChoice.Insert : EditChoice.Cancel,
                        Cost = row,
                    };
                },
                column =>
                {
                    Debug.Assert(column >= 0 && column <= sequence.Count);
                    return new EditChoice
                    {
                        Operation = column > 0 ? EditChoice.Delete : EditChoice.Cancel,
                        Cost = column,
                    };
                },
                (l, r) => Equals(l, r) ? 0 : 1,
                i => 1,
                d => 1
            );
        }

        /// <summary>
        /// Gets the longest common sub-sequence shared by <paramref name="sequence"/> and <paramref name="otherSequence"/>.
        /// </summary>
        /// <typeparam name="T">The type of elements in both sequences.</typeparam>
        /// <param name="sequence">The sequence to compare to.</param>
        /// <param name="otherSequence">The sequence to compare with.</param>
        /// <returns>The longest common sub-sequence shared by both sequences.</returns>
        /// <exception cref="ArgumentNullException">Thrown if either <paramref name="sequence"/> or <paramref name="otherSequence"/> are <c>null</c>.</exception>
        public static T[] GetLongestCommonSubSequence<T>(this IList<T> sequence, IList<T> otherSequence)
        {
            Validate.ArgumentNotNull(nameof(sequence), sequence);
            Validate.ArgumentNotNull(nameof(otherSequence), otherSequence);

            return GetEditDistance(sequence, otherSequence,
                row =>
                {
                    Debug.Assert(row >= 0 && row <= otherSequence.Count);
                    return new EditChoice
                    {
                        Operation = row > 0 ? EditChoice.Insert : EditChoice.Cancel,
                        Cost = row,
                    };
                },
                column =>
                {
                    Debug.Assert(column >= 0 && column <= sequence.Count);
                    return new EditChoice
                    {
                        Operation = column > 0 ? EditChoice.Delete : EditChoice.Cancel,
                        Cost = column,
                    };
                },
                (l, r) => Equals(l, r) ? 0 : double.PositiveInfinity,
                i => 1,
                d => 1
            ).Where(p => p.Operation == EditOperation.Match).Select(s => s.Item).ToArray();
        }

        /// <summary>
        /// Converts a sequence to a set.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="sequence">The sequence.</param>
        /// <param name="comparer">An equality comparer.</param>
        /// <returns>A new set containing the elements in <paramref name="sequence"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown if either of <paramref name="sequence"/> or <paramref name="comparer"/> are <c>null</c>.</exception>
        public static ISet<T> ToSet<T>(this IEnumerable<T> sequence, IEqualityComparer<T> comparer)
        {
            Validate.ArgumentNotNull(nameof(sequence), sequence);
            Validate.ArgumentNotNull(nameof(comparer), comparer);

            return new HashSet<T>(sequence, comparer);
        }

        /// <summary>
        /// Converts a sequence to a set.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="sequence">The sequence.</param>
        /// <returns>A new set containing the elements in <paramref name="sequence"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="sequence"/> is <c>null</c>.</exception>
        public static ISet<T> ToSet<T>(this IEnumerable<T> sequence)
        {
            Validate.ArgumentNotNull(nameof(sequence), sequence);

            return new HashSet<T>(sequence);
        }

        /// <summary>
        /// Interprets a given <paramref name="sequence"/> as a list. The returned list can either be the same object or a new
        /// object.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="sequence">The sequence.</param>
        /// <returns>A list representing the original sequence.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="sequence"/> is <c>null</c>.</exception>
        [NotNull]
        public static IList<T> AsList<T>([NotNull] this IEnumerable<T> sequence)
        {
            Validate.ArgumentNotNull(nameof(sequence), sequence);

            if (sequence is IList<T> asList)
            {
                return asList;
            }
            if (sequence is ICollection<T> asCollection)
            {
                var result = new T[asCollection.Count];
                asCollection.CopyTo(result, 0);

                return result;
            }
           
            return sequence.ToArray();
        }

        /// <summary>
        /// Evaluates the appearance frequency for each item in a <paramref name="sequence"/>.
        /// </summary>
        /// <typeparam name="T">The type of items in the sequence </typeparam>
        /// <param name="sequence">The sequence.</param>
        /// <param name="comparer">The comparer.</param>
        /// <returns>A new dictionary where each key is an item form the <paramref name="sequence"/> and associated values are the frequencies.</returns>
        /// <exception cref="ArgumentNullException">Thrown if either of <paramref name="sequence"/> or <paramref name="comparer"/> are <c>null</c>.</exception>
        [NotNull]
        public static IDictionary<T, int> GetItemFrequencies<T>([NotNull] this IEnumerable<T> sequence, [NotNull] IEqualityComparer<T> comparer)
        {
            Validate.ArgumentNotNull(nameof(sequence), sequence);
            Validate.ArgumentNotNull(nameof(comparer), comparer);

            var result = new Dictionary<T, int>(comparer);
            foreach (var item in sequence)
            {
                if (!result.TryGetValue(item, out int frequency))
                {
                    result.Add(item, 1);
                }
                else
                {
                    result[item] = frequency + 1;
                }
            }

            return result;
        }

        /// <summary>
        /// Adds a new key/valuer pair or updates an existing one.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="dict">The dictionary.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="updateFunc">The value update function.</param>
        /// <returns><c>true</c> if the a new key/value pair was added; otherwise <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">Thrown if either of <paramref name="dict"/> or <paramref name="updateFunc"/> are <c>null</c>.</exception>
        public static bool AddOrUpdate<TKey, TValue>(
            [NotNull] this IDictionary<TKey, TValue> dict, 
            TKey key, TValue value,
            [NotNull] Func<TValue, TValue> updateFunc)
        {
            Validate.ArgumentNotNull(nameof(dict), dict);
            Validate.ArgumentNotNull(nameof(updateFunc), updateFunc);

            if (dict.TryGetValue(key, out TValue existing))
            {
                dict[key] = updateFunc(existing);
                return false;
            }

            dict.Add(key, value);
            return true;
        }

        /// <summary>
        /// Appends the specified <paramref name="item1"/> to an array <paramref name="array"/>.
        /// </summary>
        /// <typeparam name="T">The type of elements in <paramref name="array"/>.</typeparam>
        /// <param name="array">The array.</param>
        /// <param name="item1">The item to append to array.</param>
        /// <returns>A new array consisting <paramref name="array"/> and appended <paramref name="item1"/>.</returns>
        [NotNull]
        public static T[] Append<T>([CanBeNull] this T[] array, T item1)
        {
            var length = array?.Length + 1 ?? 1;
            Array.Resize(ref array, length);
            array[length - 1] = item1;

            return array;
        }

        /// <summary>
        /// Appends the specified items to an array <paramref name="array"/>.
        /// </summary>
        /// <typeparam name="T">The type of elements in <paramref name="array"/>.</typeparam>
        /// <param name="array">The array.</param>
        /// <param name="item1">The first item to append to array.</param>
        /// <param name="item2">The second item to append to array.</param>
        /// <returns>A new array consisting <paramref name="array"/> and appended items.</returns>
        [NotNull]
        public static T[] Append<T>([CanBeNull] this T[] array, T item1, T item2)
        {
            var length = array?.Length + 2 ?? 2;
            Array.Resize(ref array, length);
            array[length - 2] = item1;
            array[length - 1] = item2;

            return array;
        }

        /// <summary>
        /// Appends the specified items to an array <paramref name="array"/>.
        /// </summary>
        /// <typeparam name="T">The type of elements in <paramref name="array"/>.</typeparam>
        /// <param name="array">The array.</param>
        /// <param name="item1">The first item to append to array.</param>
        /// <param name="item2">The second item to append to array.</param>
        /// <param name="item3">The third item to append to array.</param>
        /// <returns>A new array consisting <paramref name="array"/> and appended items.</returns>
        [NotNull]
        public static T[] Append<T>([CanBeNull] this T[] array, T item1, T item2, T item3)
        {
            var length = array?.Length + 3 ?? 3;
            Array.Resize(ref array, length);
            array[length - 3] = item1;
            array[length - 2] = item2;
            array[length - 1] = item3;

            return array;
        }

        /// <summary>
        /// Appends the specified items to an array <paramref name="array"/>.
        /// </summary>
        /// <typeparam name="T">The type of elements in <paramref name="array"/>.</typeparam>
        /// <param name="array">The array.</param>
        /// <param name="item1">The first item to append to array.</param>
        /// <param name="item2">The second item to append to array.</param>
        /// <param name="item3">The third item to append to array.</param>
        /// <param name="item4">The fourth item to append to array.</param>
        /// <returns>A new array consisting <paramref name="array"/> and appended items.</returns>
        [NotNull]
        public static T[] Append<T>([CanBeNull] this T[] array, T item1, T item2, T item3, T item4)
        {
            var length = array?.Length + 4 ?? 4;
            Array.Resize(ref array, length);
            array[length - 4] = item1;
            array[length - 3] = item2;
            array[length - 2] = item3;
            array[length - 1] = item4;

            return array;
        }

        /// <summary>
        /// Appends the specified items to an array <paramref name="array"/>.
        /// </summary>
        /// <typeparam name="T">The type of elements in <paramref name="array"/>.</typeparam>
        /// <param name="array">The array.</param>
        /// <param name="item1">The first item to append to array.</param>
        /// <param name="item2">The second item to append to array.</param>
        /// <param name="item3">The third item to append to array.</param>
        /// <param name="item4">The fourth item to append to array.</param>
        /// <param name="item5">The fifth item to append to array.</param>
        /// <returns>A new array consisting <paramref name="array"/> and appended items.</returns>
        [NotNull]
        public static T[] Append<T>([CanBeNull] this T[] array, T item1, T item2, T item3, T item4, T item5)
        {
            var length = array?.Length + 5 ?? 5;
            Array.Resize(ref array, length);
            array[length - 5] = item1;
            array[length - 4] = item2;
            array[length - 3] = item3;
            array[length - 2] = item4;
            array[length - 1] = item5;

            return array;
        }

        /// <summary>
        /// Appends the specified <paramref name="items"/> to an array <paramref name="array"/>.
        /// </summary>
        /// <typeparam name="T">The type of elements in <paramref name="array"/>.</typeparam>
        /// <param name="array">The array.</param>
        /// <param name="items">The items to append to the array.</param>
        /// <returns>A new array consisting <paramref name="array"/> and appended items.</returns>
        [NotNull]
        public static T[] Append<T>([CanBeNull] this T[] array, [NotNull] params T[] items)
        {
            var il = array?.Length ?? 0;
            var al = items.Length;

            Array.Resize(ref array, il + al);
            for (var i = 0; i < al; i++)
            {
                array[il + i] = items[i];
            }

            return array;
        }

        [NotNull]
        public static IEnumerable<KeyValuePair<int, T>> AsIndexedEnumerableIterate<T>([NotNull] this IEnumerable<T> sequence)
        {
            Debug.Assert(sequence != null );

            if (sequence is IList<T> list)
            {
                for (var i = 0; i < list.Count; i++)
                {
                    yield return new KeyValuePair<int, T>(i, list[i]);
                }
            }
            else
            {
                var index = 0;
                foreach (var item in sequence)
                {
                    yield return new KeyValuePair<int, T>(index++, item);
                }
            }
        }

        /// <summary>
        /// Interprets a list as an index-value pair sequence.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="sequence">The sequence to convert to an index-value sequence.</param>
        /// <returns>The resulting sequence.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="sequence"/> is <c>null</c>.</exception>
        [NotNull]
        public static IEnumerable<KeyValuePair<int, T>> AsIndexedEnumerable<T>([NotNull] this IEnumerable<T> sequence)
        {
            Validate.ArgumentNotNull(nameof(sequence), sequence);
            return AsIndexedEnumerableIterate(sequence);
        }

        /// <summary>
        /// Converts a given sequence to a list by applying a <paramref name="selector"/> to each element of the <paramref name="sequence"/>.
        /// </summary>
        /// <typeparam name="T">/The type of elements in the sequence.</typeparam>
        /// <typeparam name="TResult">The type of the resulting elements.</typeparam>
        /// <param name="sequence">The input sequence.</param>
        /// <param name="selector">The selector function.</param>
        /// <returns>A new list which contains the selected values.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="sequence"/> or <paramref name="sequence"/> are <c>null</c>.</exception>
        [NotNull]
        public static List<TResult> ToList<T, TResult>([NotNull] this IEnumerable<T> sequence, [NotNull] Func<T, TResult> selector)
        {
            Validate.ArgumentNotNull(nameof(sequence), sequence);
            Validate.ArgumentNotNull(nameof(selector), selector);

            List<TResult> result;
            if (sequence is IList<T> list)
            {
                result = new List<TResult>(list.Count);
                // ReSharper disable once LoopCanBeConvertedToQuery
                // ReSharper disable once ForCanBeConvertedToForeach
                for (var i = 0; i < list.Count; i++)
                {
                    result.Add(selector(list[i]));
                }
            }
            else
            {
                result = new List<TResult>();
                // ReSharper disable once LoopCanBeConvertedToQuery
                foreach (var item in sequence)
                {
                    result.Add(selector(item));
                }
            }

            return result;
        }

        /// <summary>
        /// Converts a given sequence to a list by applying a <paramref name="selector"/> to each element of the <paramref name="sequence"/>.
        /// </summary>
        /// <typeparam name="T">/The type of elements in the sequence.</typeparam>
        /// <typeparam name="TResult">The type of the resulting elements.</typeparam>
        /// <param name="sequence">The input sequence.</param>
        /// <param name="selector">The selector function.</param>
        /// <remarks>The second argument to <paramref name="selector"/> is the index of the element in the original <paramref name="sequence"/>.</remarks>
        /// <returns>A new list which contains the selected values.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="sequence"/> or <paramref name="sequence"/> are <c>null</c>.</exception>
        [NotNull]
        public static List<TResult> ToList<T, TResult>([NotNull] this IEnumerable<T> sequence, [NotNull] Func<T, int, TResult> selector)
        {
            Validate.ArgumentNotNull(nameof(sequence), sequence);
            Validate.ArgumentNotNull(nameof(selector), selector);

            List<TResult> result;
            if (sequence is IList<T> list)
            {
                result = new List<TResult>(list.Count);
                // ReSharper disable once LoopCanBeConvertedToQuery
                // ReSharper disable once ForCanBeConvertedToForeach
                for (var i = 0; i < list.Count; i++)
                {
                    result.Add(selector(list[i], i));
                }
            }
            else
            {
                result = new List<TResult>();
                var index = 0;

                // ReSharper disable once LoopCanBeConvertedToQuery
                foreach (var item in sequence)
                {
                    result.Add(selector(item, index++));
                }
            }

            return result;
        }

        [NotNull]
        [ItemNotNull]
        private static IEnumerable<T[]> PartitionIterate<T>([NotNull] this IEnumerable<T> sequence, int size)
        {
            Debug.Assert(sequence != null);
            Debug.Assert(size > 0);
            Validate.ArgumentGreaterThanZero(nameof(size), size);

            var temp = new T[size];
            var count = 0;
            foreach (var item in sequence)
            {
                temp[count++] = item;
                if (count == size)
                {
                    count = 0;
                    var res = new T[size];
                    Array.Copy(temp, res, size);

                    yield return res;
                }
            }

            if (count > 0)
            {
                var res = new T[count];
                Array.Copy(temp, res, count);

                yield return res;
            }
        }

        /// <summary>
        /// Partitions a specified <paramref name="sequence"/> into chunks of given <paramref name="size"/>.
        /// </summary>
        /// <typeparam name="T">The type of elements in the input sequence.</typeparam>
        /// <param name="sequence">The sequence to partition.</param>
        /// <param name="size">The size of each partition.</param>
        /// <returns>A sequence of partitioned items. Each partition is of the specified <paramref name="size"/> (or less, if no elements are left).</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="sequence"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="size"/> is less than one.</exception>
        [NotNull]
        [ItemNotNull]
        public static IEnumerable<T[]> Partition<T>([NotNull] this IEnumerable<T> sequence, int size)
        {
            Validate.ArgumentNotNull(nameof(sequence), sequence);
            Validate.ArgumentGreaterThanZero(nameof(size), size);

            return PartitionIterate(sequence, size);
        }

        /// <summary>
        /// Returns either the given <paramref name="sequence"/> or an empty one if <paramref name="sequence"/> is <c>null</c>.
        /// </summary>
        /// <typeparam name="T">The type of elements in the given sequence.</typeparam>
        /// <param name="sequence">The sequence.</param>
        /// <returns>The original sequence or an empty one.</returns>
        [NotNull]
        public static IEnumerable<T> EmptyIfNull<T>([CanBeNull] this IEnumerable<T> sequence)
        {
            return sequence ?? Enumerable.Empty<T>();
        }

        /// <summary>
        /// Determines whether the given <paramref name="sequence"/> is null or empty.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="sequence">The sequence.</param>
        /// <returns>
        ///   <c>true</c> if the sequence is null or empty; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNullOrEmpty<T>([CanBeNull] this IEnumerable<T> sequence)
        {
            return
                sequence == null ||
                !sequence.Any();
        }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this sequence of elements.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <typeparam name="TResult">The type of the result of the selector.</typeparam>
        /// <param name="sequence">The sequence.</param>
        /// <param name="selector">The result selector.</param>
        /// <param name="separator">The separator used between selected items.</param>
        /// <returns>
        /// A <see cref="string"/> that contains all the elements of the <paramref name="sequence"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="sequence"/> or <paramref name="selector"/> or <paramref name="separator"/> are <c>null</c>.</exception>
        public static string ToString<T, TResult>(
            [NotNull] this IEnumerable<T> sequence, 
            [NotNull] Func<T, TResult> selector,
            [NotNull] string separator)
        {
            Validate.ArgumentNotNull(nameof(sequence), sequence);
            Validate.ArgumentNotNull(nameof(selector), selector);
            Validate.ArgumentNotNull(nameof(separator), separator);

            var sb = new StringBuilder();
            foreach (var item in sequence)
            {
                var selected = selector(item);
                if (sb.Length > 0)
                {
                    sb.Append(separator);
                }

                sb.Append(selected);
            }

            return sb.ToString();
        }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this sequence of elements.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="sequence">The sequence.</param>
        /// <param name="separator">The separator used between selected items.</param>
        /// <returns>
        /// A <see cref="string"/> that contains all the elements of the <paramref name="sequence"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="sequence"/> or <paramref name="separator"/> are <c>null</c>.</exception>
        public static string ToString<T>(
            [NotNull] this IEnumerable<T> sequence,
            [NotNull] string separator)
        {
            Validate.ArgumentNotNull(nameof(sequence), sequence);
            Validate.ArgumentNotNull(nameof(separator), separator);

            var sb = new StringBuilder();
            foreach (var item in sequence)
            {
                if (sb.Length > 0)
                {
                    sb.Append(separator);
                }

                sb.Append(item);
            }

            return sb.ToString();
        }

        /// <summary>
        /// Finds the object that has a given minimum <typeparamref name="TKey"/>.
        /// </summary>
        /// <typeparam name="T">The type of elements in the <paramref name="sequence"/>.</typeparam>
        /// <typeparam name="TKey">The type of the key selected by <paramref name="selector"/>.</typeparam>
        /// <param name="sequence">The input sequence.</param>
        /// <param name="selector">The selector that return the key to compare.</param>
        /// <param name="comparer">The comparer used to compare the keys.</param>
        /// <returns>The item that has the minimum key.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="sequence"/> or <paramref name="selector"/> or <paramref name="comparer"/> are <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">Thrown if the <paramref name="sequence"/> is empty and <typeparamref name="T"/> is a value type.</exception>
        [CanBeNull]
        public static T Min<T, TKey>(
            [NotNull] this IEnumerable<T> sequence, 
            [NotNull] Func<T, TKey> selector,
            [NotNull] IComparer<TKey> comparer)
        {
            Validate.ArgumentNotNull(nameof(sequence), sequence);
            Validate.ArgumentNotNull(nameof(selector), selector);
            Validate.ArgumentNotNull(nameof(comparer), comparer);

            var item = default(T);
            using(var enumerator = sequence.GetEnumerator())
            {
                TKey min;
                if (enumerator.MoveNext())
                {
                    item = enumerator.Current;
                    min = selector(item);
                }
                else
                {
                    if (item == null)
                    {
                        // ReSharper disable once ExpressionIsAlwaysNull
                        return item;
                    }
                    throw new InvalidOperationException($"The {nameof(sequence)} does not contain any elements.");
                }

                while (enumerator.MoveNext())
                {
                    var nextItem = enumerator.Current;
                    var nextMin = selector(nextItem);

                    if (nextMin != null && (min == null || comparer.Compare(min, nextMin) > 0))
                    {
                        min = nextMin;
                        item = nextItem;
                    }
                }
            }

            return item;
        }

        /// <summary>
        /// Finds the object that has a given minimum <typeparamref name="TKey"/>.
        /// </summary>
        /// <typeparam name="T">The type of elements in the <paramref name="sequence"/>.</typeparam>
        /// <typeparam name="TKey">The type of the key selected by <paramref name="selector"/>.</typeparam>
        /// <param name="sequence">The input sequence.</param>
        /// <param name="selector">The selector that return the key to compare.</param>
        /// <remarks>The default comparer is used to compare values of type <typeparamref name="TKey"/>.</remarks>
        /// <returns>The item that has the minimum key.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="sequence"/> or <paramref name="selector"/> are <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">Thrown if the <paramref name="sequence"/> is empty and <typeparamref name="T"/> is a value type.</exception>
        [CanBeNull]
        public static T Min<T, TKey>(
            [NotNull] this IEnumerable<T> sequence,
            [NotNull] Func<T, TKey> selector)
        {
            return Min(sequence, selector, Comparer<TKey>.Default);
        }

        /// <summary>
        /// Finds the object that has a given maximum <typeparamref name="TKey"/>.
        /// </summary>
        /// <typeparam name="T">The type of elements in the <paramref name="sequence"/>.</typeparam>
        /// <typeparam name="TKey">The type of the key selected by <paramref name="selector"/>.</typeparam>
        /// <param name="sequence">The input sequence.</param>
        /// <param name="selector">The selector that return the key to compare.</param>
        /// <param name="comparer">The comparer used to compare the keys.</param>
        /// <returns>The item that has the maximum key.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="sequence"/> or <paramref name="selector"/> or <paramref name="comparer"/> are <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">Thrown if the <paramref name="sequence"/> is empty and <typeparamref name="T"/> is a value type.</exception>
        [CanBeNull]
        public static T Max<T, TKey>(
            [NotNull] this IEnumerable<T> sequence,
            [NotNull] Func<T, TKey> selector,
            [NotNull] IComparer<TKey> comparer)
        {
            Validate.ArgumentNotNull(nameof(sequence), sequence);
            Validate.ArgumentNotNull(nameof(selector), selector);
            Validate.ArgumentNotNull(nameof(comparer), comparer);

            var item = default(T);
            using (var enumerator = sequence.GetEnumerator())
            {
                TKey min;
                if (enumerator.MoveNext())
                {
                    item = enumerator.Current;
                    min = selector(item);
                }
                else
                {
                    if (item == null)
                    {
                        // ReSharper disable once ExpressionIsAlwaysNull
                        return item;
                    }
                    throw new InvalidOperationException($"The {nameof(sequence)} does not contain any elements.");
                }

                while (enumerator.MoveNext())
                {
                    var nextItem = enumerator.Current;
                    var nextMin = selector(nextItem);

                    if (nextMin != null && (min == null || comparer.Compare(min, nextMin) < 0))
                    {
                        min = nextMin;
                        item = nextItem;
                    }
                }
            }

            return item;
        }

        /// <summary>
        /// Finds the object that has a given maximum <typeparamref name="TKey"/>.
        /// </summary>
        /// <typeparam name="T">The type of elements in the <paramref name="sequence"/>.</typeparam>
        /// <typeparam name="TKey">The type of the key selected by <paramref name="selector"/>.</typeparam>
        /// <param name="sequence">The input sequence.</param>
        /// <param name="selector">The selector that return the key to compare.</param>
        /// <remarks>The default comparer is used to compare values of type <typeparamref name="TKey"/>.</remarks>
        /// <returns>The item that has the maximum key.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="sequence"/> or <paramref name="selector"/> are <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">Thrown if the <paramref name="sequence"/> is empty and <typeparamref name="T"/> is a value type.</exception>
        [CanBeNull]
        public static T Max<T, TKey>(
            [NotNull] this IEnumerable<T> sequence,
            [NotNull] Func<T, TKey> selector)
        {
            return Max(sequence, selector, Comparer<TKey>.Default);
        }
    }
}
