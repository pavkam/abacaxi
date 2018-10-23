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
    using Containers;
    using Internal;
    using JetBrains.Annotations;

    /// <summary>
    ///     Class provides a large number of algorithms to use on sequences.
    /// </summary>
    [PublicAPI]
    public static class SequenceAlgorithms
    {
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
            Assert.NotNull(sequence);
            Assert.NotNull(resultSequence);
            Assert.NotNull(evalMatchCostFunc);
            Assert.NotNull(evalInsertCostFunc);
            Assert.NotNull(evalDeleteCostFunc);

            var matrix = new EditChoice[sequence.Count + 1, resultSequence.Count + 1];
            for (var i = 0; i < Math.Max(sequence.Count, resultSequence.Count) + 1; i++)
            {
                if (i <= sequence.Count)
                {
                    matrix[i, 0] = initColumnCellFunc(i);
                }

                if (i <= resultSequence.Count)
                {
                    matrix[0, i] = initRowCellFunc(i);
                }
            }

            var opr = new double[3];
            for (var i1 = 1; i1 <= sequence.Count; i1++)
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
                    if (minCostOperation != -1 &&
                        !(opr[op] < minCost))
                    {
                        continue;
                    }

                    minCost = opr[op];
                    minCostOperation = op;
                }

                matrix[i1, i2] = new EditChoice
                {
                    Cost = minCost,
                    Operation = minCostOperation
                };
            }

            var path = new List<Edit<T>>();
            var pi1 = sequence.Count;
            var pi2 = resultSequence.Count;
            for (;;)
            {
                var choice = matrix[pi1, pi2];
                if (choice.Operation == EditChoice.Cancel)
                {
                    break;
                }

                Edit<T> edit;
                switch (choice.Operation)
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
        ///     Finds the longest increasing sequence in a given <paramref name="sequence" />.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="sequence">The sequence to verify.</param>
        /// <param name="comparer">The comparer used to compare the elements in the sequence.</param>
        /// <returns>The longest increasing sequence.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the <paramref name="sequence" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">Thrown if the <paramref name="comparer" /> is <c>null</c>.</exception>
        [NotNull]
        public static IEnumerable<T> FindLongestIncreasingSequence<T>(
            [NotNull] this IEnumerable<T> sequence,
            [NotNull] IComparer<T> comparer)
        {
            Validate.ArgumentNotNull(nameof(sequence), sequence);
            Validate.ArgumentNotNull(nameof(comparer), comparer);

            // ReSharper disable once IdentifierTypo
            var dyna = new List<(T item, int start, int end)>();
            var li = -1;
            var lm = 0;
            foreach (var e in sequence)
            {
                var pi = -1;
                var pm = 0;

                for (var i = dyna.Count - 1; i >= 0; i--)
                {
                    if (comparer.Compare(dyna[i].item, e) >= 0 ||
                        pi != -1 && pm >= dyna[i].start)
                    {
                        continue;
                    }

                    pi = i;
                    pm = dyna[i].start;
                }

                var nm = pm + 1;
                dyna.Add((e, nm, pi));

                if (lm >= nm)
                {
                    continue;
                }

                lm = nm;
                li = dyna.Count - 1;
            }

            var result = new T[lm];
            while (li > -1)
            {
                result[--lm] = dyna[li].item;
                li = dyna[li].end;
            }

            return result;
        }

        /// <summary>
        ///     Determines whether the sequence contains two elements that aggregate to a given <paramref name="target" /> value.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="sequence">The sequence to check.</param>
        /// <param name="target">The target value to search for.</param>
        /// <param name="aggregator">The function that aggregates two values.</param>
        /// <param name="comparer">The comparer.</param>
        /// <returns>
        ///     <c>true</c> if the <paramref name="sequence" /> contains two elements that aggregate; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     Thrown if <paramref name="sequence" />, <paramref name="aggregator" /> or
        ///     <paramref name="comparer" /> are null.
        /// </exception>
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

            Sorting.QuickSort(array, 0, array.Length, comparer);
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
        ///     Finds all duplicate items in a given <paramref name="sequence" />.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
        /// <param name="sequence">The sequence to inspect.</param>
        /// <param name="equalityComparer">The comparer used to verify the elements in the sequence.</param>
        /// <returns>A sequence of element-frequency pairs of the detected duplicates.</returns>
        /// <exception cref="ArgumentNullException">
        ///     Thrown if either the <paramref name="sequence" /> or the
        ///     <paramref name="equalityComparer" /> are <c>null</c>.
        /// </exception>
        [NotNull]
        public static Frequency<T>[] FindDuplicates<T>(
            [NotNull] this IEnumerable<T> sequence, [NotNull] IEqualityComparer<T> equalityComparer)
        {
            var appearances = GetItemFrequencies(sequence, equalityComparer);
            var result = new List<Frequency<T>>();

            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var kvp in appearances)
            {
                if (kvp.Value > 1)
                {
                    result.Add(new Frequency<T>(kvp.Key, kvp.Value));
                }
            }

            return result.ToArray();
        }

        /// <summary>
        ///     Finds all duplicate integers in a given <paramref name="sequence" />.
        /// </summary>
        /// <param name="sequence">The sequence to inspect.</param>
        /// <param name="minInSequence">The minimum possible value of an element part of the <paramref name="sequence" />.</param>
        /// <param name="maxInSequence">The maximum possible value of an element part of the <paramref name="sequence" />.</param>
        /// <returns>A sequence of element-frequency pairs of the detected duplicates.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="sequence" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Thrown if <paramref name="maxInSequence" /> is less than
        ///     <paramref name="minInSequence" />.
        /// </exception>
        [NotNull]
        public static Frequency<int>[] FindDuplicates(
            [NotNull] this IEnumerable<int> sequence, int minInSequence, int maxInSequence)
        {
            Validate.ArgumentNotNull(nameof(sequence), sequence);
            Validate.ArgumentGreaterThanOrEqualTo(nameof(maxInSequence), maxInSequence, minInSequence);

            var appearances = new int[maxInSequence - minInSequence + 1];
            foreach (var item in sequence)
            {
                if (item < minInSequence ||
                    item > maxInSequence)
                {
                    throw new InvalidOperationException(
                        $"The sequence of integers contains element {item} which is outside of the given {minInSequence}..{maxInSequence} range.");
                }

                appearances[item - minInSequence]++;
            }

            var result = new List<Frequency<int>>();
            for (var i = 0; i < appearances.Length; i++)
            {
                if (appearances[i] > 1)
                {
                    result.Add(new Frequency<int>(i + minInSequence, appearances[i]));
                }
            }

            return result.ToArray();
        }

        /// <summary>
        ///     Finds all unique items in a given <paramref name="sequence" />.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
        /// <param name="sequence">The sequence to inspect.</param>
        /// <param name="equalityComparer">The comparer used to verify the elements in the sequence.</param>
        /// <returns>A sequence of detected uniques.</returns>
        /// <exception cref="ArgumentNullException">
        ///     Thrown if either the <paramref name="sequence" /> or the
        ///     <paramref name="equalityComparer" /> are <c>null</c>.
        /// </exception>
        [NotNull]
        public static T[] FindUniques<T>(
            [NotNull] this IEnumerable<T> sequence, [NotNull] IEqualityComparer<T> equalityComparer)
        {
            var appearances = GetItemFrequencies(sequence, equalityComparer);
            var result = new List<T>();

            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var kvp in appearances)
            {
                if (kvp.Value == 1)
                {
                    result.Add(kvp.Key);
                }
            }

            return result.ToArray();
        }

        /// <summary>
        ///     Finds all unique items in a given <paramref name="sequence" /> and returns them in order of appearance.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
        /// <param name="sequence">The sequence to inspect.</param>
        /// <param name="equalityComparer">The comparer used to verify the elements in the sequence.</param>
        /// <returns>A sequence of detected uniques in order of appearance.</returns>
        /// <exception cref="ArgumentNullException">
        ///     Thrown if either the <paramref name="sequence" /> or the
        ///     <paramref name="equalityComparer" /> are <c>null</c>.
        /// </exception>
        [NotNull]
        public static T[] FindUniquesInOrder<T>(
            [NotNull] this IEnumerable<T> sequence,
            [NotNull] IEqualityComparer<T> equalityComparer)
        {
            Validate.ArgumentNotNull(nameof(sequence), sequence);
            Validate.ArgumentNotNull(nameof(equalityComparer), equalityComparer);

            var appearanceMap = new Dictionary<T, DllNode<T>>(equalityComparer);
            DllNode<T> head = null, tail = null;

            foreach (var item in sequence)
            {
                if (!appearanceMap.TryGetValue(item, out var node))
                {
                    node = new DllNode<T>
                    {
                        Prev = tail,
                        Value = item
                    };

                    if (head == null)
                    {
                        Assert.Null(tail);
                        head = node;
                    }

                    if (tail != null)
                    {
                        tail.Next = node;
                    }

                    tail = node;

                    appearanceMap.Add(item, node);
                }
                else if (node != null)
                {
                    if (node.Prev != null)
                    {
                        node.Prev.Next = node.Next;
                    }
                    else
                    {
                        head = node.Next;
                    }

                    if (node.Next != null)
                    {
                        node.Next.Prev = node.Prev;
                    }
                    else
                    {
                        tail = node.Prev;
                    }

                    appearanceMap[item] = null;
                }
            }

            var result = new List<T>();
            while (head != null)
            {
                result.Add(head.Value);
                head = head.Next;
            }

            return result.ToArray();
        }

        [NotNull, ItemNotNull]
        private static IEnumerable<T[]> ExtractNestedBlocksIterate<T>(
            [NotNull] this IEnumerable<T> sequence,
            T openBracket,
            T closeBracket,
            [NotNull] IEqualityComparer<T> comparer)
        {
            Assert.NotNull(sequence);
            Assert.NotNull(comparer);

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
                throw new InvalidOperationException(
                    $"There are {stack.Count} number of blocks that have not been closed.");
            }

            if (currentList.Count > 0)
            {
                yield return currentList.ToArray();
            }
        }

        /// <summary>
        ///     Extracts all nested groups from sequence. The method returns a sequence of sequences.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="sequence" />.</typeparam>
        /// <param name="sequence">The sequence.</param>
        /// <param name="openBracket">The element that signifies the start of a group.</param>
        /// <param name="closeBracket">The element that signifies the end of a group.</param>
        /// <param name="comparer">The equality comparer for the elements of the <paramref name="sequence" />.</param>
        /// <returns>The sequence of extracted groups, starting with the inner most ones.</returns>
        /// <exception cref="ArgumentNullException">
        ///     Thrown if <paramref name="sequence" /> or <paramref name="comparer" /> are
        ///     <c>null</c>.
        /// </exception>
        /// <exception cref="InvalidOperationException">Throws if the number of open and close brackets do not match.</exception>
        [NotNull, ItemNotNull]
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

        [ItemNotNull, NotNull]
        private static IEnumerable<T[]> GetSubsequencesOfAggregateValueIterate<T>(
            [NotNull] this IList<T> sequence,
            [NotNull] Aggregator<T> aggregator,
            [NotNull] Aggregator<T> disaggregator,
            [NotNull] IComparer<T> comparer,
            T target)
        {
            Assert.NotNull(sequence);
            Assert.NotNull(aggregator);
            Assert.NotNull(disaggregator);
            Assert.NotNull(comparer);


            var aggregate = default(T);

            var mem = new Dictionary<T, List<int>> {{aggregate, new List<int> {-1}}};
            for (var i = 0; i < sequence.Count; i++)
            {
                /* Calculate the current aggregate. */
                aggregate = aggregator(aggregate, sequence[i]);

                /* Record the current "aggregate" x "index" */
                if (!mem.TryGetValue(aggregate, out var list))
                {
                    list = new List<int>();
                    mem.Add(aggregate, list);
                }

                list.Add(i);

                /* Check if target achieved by previous pre-calculations. */
                if (mem.TryGetValue(disaggregator(aggregate, target), out var prevStarts))
                {
                    foreach (var start in prevStarts)
                    {
                        yield return sequence.Copy(start + 1, i - start);
                    }
                }
            }
        }

        /// <summary>
        ///     Finds the sub-sequences whose aggregated values are equal to a given <paramref name="target" /> value.
        /// </summary>
        /// <param name="sequence">The sequence to check.</param>
        /// <param name="aggregator">The value aggregator.</param>
        /// <param name="disaggregator">The value dis-aggregator.</param>
        /// <param name="comparer">The comparer.</param>
        /// <param name="target">The target aggregated value.</param>
        /// <returns>A sequence of found integers.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the <paramref name="sequence" /> is <c>null</c>.</exception>
        [NotNull]
        public static IEnumerable<T[]> GetSubsequencesOfAggregateValue<T>(
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

            return GetSubsequencesOfAggregateValueIterate(sequence, aggregator, disaggregator, comparer,
                target);
        }

        [ItemCanBeNull, NotNull]
        private static IEnumerable<T> InterleaveIterate<T>(
            [NotNull] IComparer<T> comparer,
            [NotNull] IEnumerable<T> sequence,
            [NotNull, ItemNotNull] params IEnumerable<T>[] sequences)
        {
            Assert.NotNull(comparer);
            Assert.NotNull(sequence);
            Assert.NotNull(sequences);

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

                if (!top.MoveNext())
                {
                    continue;
                }

                if (comparer.Compare(top.Current, c) > 0)
                {
                    throw new InvalidOperationException("One or more sequence contains unsorted items.");
                }

                heap.Add(top);
            }
        }

        /// <summary>
        ///     Interleaves multiple sequences into one output sequence.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequences.</typeparam>
        /// <param name="comparer">The comparer.</param>
        /// <param name="sequence">The first sequence to interleave.</param>
        /// <param name="sequences">The next sequences to interleave.</param>
        /// <returns>A new interleaved stream.</returns>
        /// <exception cref="ArgumentNullException">
        ///     Thrown if the <paramref name="comparer" /> or <paramref name="sequences" /> is
        ///     <c>null</c>.
        /// </exception>
        /// <exception cref="ArgumentException">Thrown if the <paramref name="sequences" /> is empty.</exception>
        /// <exception cref="InvalidOperationException">Thrown if one or more sequences contain unsorted items.</exception>
        [NotNull]
        public static IEnumerable<T> Interleave<T>(
            [NotNull] IComparer<T> comparer,
            [NotNull] IEnumerable<T> sequence,
            [NotNull, ItemNotNull] params IEnumerable<T>[] sequences)
        {
            Validate.ArgumentNotNull(nameof(comparer), comparer);
            Validate.ArgumentNotNull(nameof(sequence), sequence);
            Validate.ArgumentNotNull(nameof(sequences), sequences);

            return InterleaveIterate(comparer, sequence, sequences);
        }

        /// <summary>
        ///     Reverses a given <paramref name="sequence" /> in place (mutating the original).
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="sequence">The sequence to reverse.</param>
        /// <param name="startIndex">The start index in the sequence.</param>
        /// <param name="length">The length of sequence to reverse.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="sequence" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Thrown when the combination of <paramref name="startIndex" /> and
        ///     <paramref name="length" /> is out of bounds.
        /// </exception>
        public static void Reverse<T>([NotNull] this IList<T> sequence, int startIndex, int length)
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
        ///     Creates an array whose contents are the elements of the <paramref name="input" /> repeated
        ///     <paramref name="repetitions" /> times.
        /// </summary>
        /// <typeparam name="T">The type of the sequence's elements</typeparam>
        /// <param name="input">The input sequence.</param>
        /// <param name="repetitions">Number of times to repeat the sequence.</param>
        /// <returns>A new array.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the <paramref name="input" /> sequence is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Thrown if the value of <paramref name="repetitions" /> argument is less
        ///     than <c>1</c>.
        /// </exception>
        [NotNull]
        public static T[] Repeat<T>([NotNull] this IEnumerable<T> input, int repetitions)
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
                if (repetitions <= 0)
                {
                    continue;
                }

                var currentLength = arrayInput.Length;
                Array.Resize(ref arrayInput, currentLength * 2);
                Array.Copy(arrayInput, 0, arrayInput, currentLength, currentLength);
            }

            return arrayOutput;
        }

        /// <summary>
        ///     Finds the location of <paramref name="item" /> in the given <paramref name="sequence" />.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="sequence">The sequence to search.</param>
        /// <param name="startIndex">The start index in the sequence.</param>
        /// <param name="length">The length of sequence to search..</param>
        /// <param name="item">The item to search for.</param>
        /// <param name="comparer">Comparer used in the search.</param>
        /// <param name="ascending">Specifies whether the sequence is sorted in ascending or descending order.</param>
        /// <returns>The index in the sequence where the <paramref name="item" /> was found; <c>-1</c> otherwise.</returns>
        /// <exception cref="ArgumentNullException">
        ///     Thrown if either <paramref name="sequence" /> or <paramref name="comparer" />
        ///     are <c>null</c>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Thrown when the combination of <paramref name="startIndex" /> and
        ///     <paramref name="length" /> is out of bounds.
        /// </exception>
        public static int BinarySearch<T>(
            [NotNull] this IList<T> sequence,
            int startIndex,
            int length,
            T item,
            [NotNull] IComparer<T> comparer,
            bool ascending = true)
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

                if (compareResult < 0)
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
        ///     Finds the location of <paramref name="item" /> in the given <paramref name="sequence" />. If the item is repeated a
        ///     number of times,
        ///     this method returns their index range. Otherwise, this method returns the item immediately smaller than the
        ///     searched value.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="sequence">The sequence to search.</param>
        /// <param name="startIndex">The start index in the sequence.</param>
        /// <param name="length">The length of sequence to search..</param>
        /// <param name="item">The item to search for.</param>
        /// <param name="comparer">Comparer used in the search.</param>
        /// <param name="ascending">Specifies whether the sequence is sorted in ascending or descending order.</param>
        /// <returns>An index pair in the sequence where the <paramref name="item" />(s) were found; <c>-1</c> is .</returns>
        /// <exception cref="ArgumentNullException">
        ///     Thrown if either <paramref name="sequence" /> or <paramref name="comparer" />
        ///     are <c>null</c>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Thrown when the combination of <paramref name="startIndex" /> and
        ///     <paramref name="length" /> is out of bounds.
        /// </exception>
        public static (int first, int last) BinaryLookup<T>(
            [NotNull] this IList<T> sequence,
            int startIndex,
            int length,
            T item,
            [NotNull] IComparer<T> comparer,
            bool ascending = true)
        {
            Validate.CollectionArgumentsInBounds(nameof(sequence), sequence, startIndex, length);
            Validate.ArgumentNotNull(nameof(comparer), comparer);

            var direction = ascending ? 1 : -1;

            /* Search the first appearance. */
            var left = startIndex;
            var right = startIndex + length - 1;
            while (left < right)
            {
                var mid = (left + right) / 2;
                if (direction * comparer.Compare(sequence[mid], item) < 0)
                {
                    left = mid + 1;
                }
                else
                {
                    right = mid;
                }
            }

            var comp = right < 0 ? -1 : direction * comparer.Compare(sequence[left], item);

            if (comp > 0)
            {
                return comp > 0 ? (left - 1, left - 1) : (left, left);
            }

            if (comp < 0)
            {
                return (right, right);
            }

            var first = left;

            /* Search the last appearance. */
            left = startIndex;
            right = startIndex + length - 1;
            while (left < right)
            {
                var dia = left + right;
                var mid = dia / 2 + dia % 2;

                if (direction * comparer.Compare(sequence[mid], item) > 0)
                {
                    right = mid - 1;
                }
                else
                {
                    left = mid;
                }
            }

            return (first, left);
        }

        /// <summary>
        ///     Evaluates the edit distance between two given sequences <paramref name="sequence" /> and
        ///     <paramref name="resultSequence" />.
        /// </summary>
        /// <typeparam name="T">The type of elements in both sequences.</typeparam>
        /// <param name="sequence">The sequence to compare to.</param>
        /// <param name="resultSequence">The sequence to compare with.</param>
        /// <returns>
        ///     A sequence of "edits" applied to the original <paramref name="sequence" /> to obtain the
        ///     <paramref name="resultSequence" />.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     Thrown if either <paramref name="sequence" /> or
        ///     <paramref name="resultSequence" /> are <c>null</c>.
        /// </exception>
        [NotNull]
        public static Edit<T>[] Diff<T>([NotNull] this IList<T> sequence, [NotNull] IList<T> resultSequence)
        {
            Validate.ArgumentNotNull(nameof(sequence), sequence);
            Validate.ArgumentNotNull(nameof(resultSequence), resultSequence);

            return GetEditDistance(sequence, resultSequence,
                row =>
                {
                    Assert.Condition(row >= 0 && row <= resultSequence.Count);
                    return new EditChoice
                    {
                        Operation = row > 0 ? EditChoice.Insert : EditChoice.Cancel,
                        Cost = row
                    };
                },
                column =>
                {
                    Assert.Condition(column >= 0 && column <= sequence.Count);
                    return new EditChoice
                    {
                        Operation = column > 0 ? EditChoice.Delete : EditChoice.Cancel,
                        Cost = column
                    };
                },
                (l, r) => Equals(l, r) ? 0 : 1,
                i => 1,
                d => 1
            );
        }

        /// <summary>
        ///     Gets the longest common sub-sequence shared by <paramref name="sequence" /> and <paramref name="otherSequence" />.
        /// </summary>
        /// <typeparam name="T">The type of elements in both sequences.</typeparam>
        /// <param name="sequence">The sequence to compare to.</param>
        /// <param name="otherSequence">The sequence to compare with.</param>
        /// <returns>The longest common sub-sequence shared by both sequences.</returns>
        /// <exception cref="ArgumentNullException">
        ///     Thrown if either <paramref name="sequence" /> or
        ///     <paramref name="otherSequence" /> are <c>null</c>.
        /// </exception>
        [NotNull]
        public static T[] GetLongestCommonSubsequence<T>(
            [NotNull] this IList<T> sequence,
            [NotNull] IList<T> otherSequence)
        {
            Validate.ArgumentNotNull(nameof(sequence), sequence);
            Validate.ArgumentNotNull(nameof(otherSequence), otherSequence);

            return GetEditDistance(sequence, otherSequence,
                    row =>
                    {
                        Assert.Condition(row >= 0 && row <= otherSequence.Count);
                        return new EditChoice
                        {
                            Operation = row > 0 ? EditChoice.Insert : EditChoice.Cancel,
                            Cost = row
                        };
                    },
                    column =>
                    {
                        Assert.Condition(column >= 0 && column <= sequence.Count);
                        return new EditChoice
                        {
                            Operation = column > 0 ? EditChoice.Delete : EditChoice.Cancel,
                            Cost = column
                        };
                    },
                    (l, r) => Equals(l, r) ? 0 : double.PositiveInfinity,
                    i => 1,
                    d => 1
                )
                .Where(p => p.Operation == EditOperation.Match)
                .Select(s => s.Item)
                .ToArray();
        }

        /// <summary>
        ///     Evaluates the appearance frequency for each item in a <paramref name="sequence" />.
        /// </summary>
        /// <typeparam name="T">The type of items in the sequence </typeparam>
        /// <param name="sequence">The sequence.</param>
        /// <param name="comparer">The comparer.</param>
        /// <returns>
        ///     A new dictionary where each key is an item form the <paramref name="sequence" /> and associated values are the
        ///     frequencies.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     Thrown if either of <paramref name="sequence" /> or
        ///     <paramref name="comparer" /> are <c>null</c>.
        /// </exception>
        [NotNull]
        public static IDictionary<T, int> GetItemFrequencies<T>([NotNull] this IEnumerable<T> sequence,
            [NotNull] IEqualityComparer<T> comparer)
        {
            Validate.ArgumentNotNull(nameof(sequence), sequence);
            Validate.ArgumentNotNull(nameof(comparer), comparer);

            var result = new Dictionary<T, int>(comparer);
            foreach (var item in sequence)
            {
                if (!result.TryGetValue(item, out var frequency))
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
        ///     Deconstructs a give n<paramref name="sequence" /> into subsequences known as "terms". Each term is validated/scored
        ///     by
        ///     <paramref name="scoreTermFunc" /> function.
        /// </summary>
        /// <typeparam name="T">The type of items in the sequence.</typeparam>
        /// <param name="sequence">The sequence.</param>
        /// <param name="scoreTermFunc">The scoring function.</param>
        /// <returns>A sequence of terms that the original sequence was split into.</returns>
        /// <exception cref="ArgumentNullException">
        ///     Thrown if <paramref name="sequence" /> or <paramref name="scoreTermFunc" /> are <c>null</c>.
        /// </exception>
        [NotNull, ItemNotNull]
        public static T[][] DeconstructIntoTerms<T>(
            [NotNull] this IList<T> sequence,
            [NotNull] Func<IList<T>, int, int, double> scoreTermFunc)
        {
            Validate.ArgumentNotNull(nameof(sequence), sequence);
            Validate.ArgumentNotNull(nameof(scoreTermFunc), scoreTermFunc);

            var mem = new (double score, int prev)[sequence.Count + 1, sequence.Count + 1];

            mem[0, 0] = (0, -1);

            for (var i = 1; i <= sequence.Count; i++)
            {
                mem[i, 0] = (double.NaN, -1);
                mem[0, i] = (double.NaN, -1);
            }

            for (var m = 0; m < sequence.Count; m++)
            {
                for (var e = m; e < sequence.Count; e++)
                {
                    var wordScore = scoreTermFunc(sequence, m, e - m + 1);
                    var left = mem[m, m];
                    var other = mem[m, e + 1];

                    if (!double.IsNaN(wordScore) &&
                        !double.IsNaN(left.score) &&
                        (double.IsNaN(other.score) ||
                         left.score + wordScore > other.score))
                    {
                        mem[m + 1, e + 1] = (left.score + wordScore, m);
                    }
                    else
                    {
                        mem[m + 1, e + 1] = mem[m, e + 1];
                    }
                }
            }

            var last = mem[sequence.Count, sequence.Count];
            var path = new List<T[]>();

            if (!double.IsNaN(last.score))
            {
                var end = sequence.Count - 1;
                while (last.prev >= 0)
                {
                    var piece = sequence.Copy(last.prev, end - last.prev + 1);

                    path.Add(piece);

                    end = last.prev - 1;
                    last = mem[last.prev, last.prev];
                }

                path.Reverse();
            }

            return path.ToArray();
        }

        /// <summary>
        ///     Determines whether the specified <paramref name="sequence" /> is a palindrome.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="sequence">The sequence.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="length">The length to check.</param>
        /// <param name="comparer">The element comparer.</param>
        /// <returns>
        ///     <c>true</c> if the specified sequence is a palindrome; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     Thrown if <paramref name="sequence" /> or <paramref name="comparer" /> are <c>null</c>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Thrown when the combination of <paramref name="startIndex" /> and
        ///     <paramref name="length" /> is out of bounds.
        /// </exception>
        public static bool IsPalindrome<T>(
            [NotNull] this IList<T> sequence, int startIndex, int length, [NotNull] IEqualityComparer<T> comparer)
        {
            Validate.CollectionArgumentsInBounds(nameof(sequence), sequence, startIndex, length);
            Validate.ArgumentNotNull(nameof(comparer), comparer);

            for (var x = 0; x < length; x++)
            {
                if (!comparer.Equals(sequence[x + startIndex], sequence[length + startIndex - x - 1]))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Determines whether the given <paramref name="sequence"/> is a permutation of a palindrome.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="sequence">The sequence.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="length">The length to check.</param>
        /// <param name="comparer">The element comparer.</param>
        /// <returns>
        ///     <c>true</c> if the specified sequence is a permutation of a palindrome; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     Thrown if <paramref name="sequence" /> or <paramref name="comparer" /> are <c>null</c>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Thrown when the combination of <paramref name="startIndex" /> and
        ///     <paramref name="length" /> is out of bounds.
        /// </exception>
        public static bool IsPermutationOfPalindrome<T>(
            [NotNull] this IList<T> sequence, int startIndex, int length, [NotNull] IEqualityComparer<T> comparer)
        {
            Validate.CollectionArgumentsInBounds(nameof(sequence), sequence, startIndex, length);
            Validate.ArgumentNotNull(nameof(comparer), comparer);

            var set = new HashSet<T>(comparer);
            for (var x = 0; x < length; x++)
            {
                var item = sequence[x + startIndex];
                if (!set.Add(item))
                {
                    set.Remove(item);
                }
            }

            return set.Count <= 1;
        }

        /// <summary>
        /// Returns the index of a permutation substring.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="sequence">The sequence.</param>
        /// <param name="subSequence">The sub-sequence to check for.</param>
        /// <param name="comparer">The comparer.</param>
        /// <returns>A zero-based index in the <paramref name="sequence"/> where a permutation of <paramref name="subSequence"/> was found. <c>-1</c> if not found.</returns>
        /// <exception cref="ArgumentNullException">
        ///     Thrown if <paramref name="sequence" /> or <paramref name="comparer" /> are <c>null</c>.
        /// </exception>
        public static int IndexOfPermutationOf<T>(
            [NotNull] this IList<T> sequence,
            [NotNull] IList<T> subSequence,
            [NotNull] IEqualityComparer<T> comparer)
        {
            Validate.ArgumentNotNull(nameof(sequence), sequence);
            Validate.ArgumentNotNull(nameof(subSequence), subSequence);
            Validate.ArgumentNotNull(nameof(comparer), comparer);

            if (subSequence.Count > sequence.Count)
            {
                return -1;
            }

            /* Build patterns (sub-sequence and the first part of the sequence) */
            var acc = new Dictionary<T, int>(comparer);
            var pattern = new Dictionary<T, int>(comparer);

            for (var i = 0; i < subSequence.Count; i++)
            {
                pattern.AddOrUpdate(subSequence[i], 1, v => v + 1);
                acc.AddOrUpdate(sequence[i], 1, v => v + 1);
            }

            /* The pattern matching function. */
            bool PatternsMatch()
            {
                if (acc.Count != pattern.Count)
                {
                    return false;
                }

                foreach (var kvp in acc)
                {
                    if (!pattern.TryGetValue(kvp.Key, out var freq) || freq != kvp.Value)
                    {
                        return false;
                    }
                }

                return true;
            }

            /* Check current pattern matching. */
            if (PatternsMatch())
            {
                return 0;
            }

            /* Check the rest of the sequence. */
            for (var i = 1; i <= sequence.Count - subSequence.Count; i++)
            {
                var prev = sequence[i - 1];

                var c = acc[prev] - 1;
                if (c == 0)
                {
                    acc.Remove(prev);
                }
                else
                {
                    acc[prev] = c;
                }

                var next = sequence[i + subSequence.Count - 1];
                acc.AddOrUpdate(next, 1, v => v + 1);

                if (PatternsMatch())
                {
                    return i;
                }
            }

            return -1;
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

        private sealed class DllNode<T>
        {
            [CanBeNull] public DllNode<T> Next;
            [CanBeNull] public DllNode<T> Prev;
            [CanBeNull] public T Value;
        }
    }
}