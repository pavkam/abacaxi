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

using System.Linq;
using Abacaxi.Containers;

namespace Abacaxi
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    public static class Sequence
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

        private static void QuickSortRecurse<T>(IList<T> sequence, int lo, int hi, IComparer<T> comparer)
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
                    QuickSortRecurse(sequence, lo, pivotIndex - 1, comparer);
                }
                if (hi - pivotIndex > 1)
                {
                    QuickSortRecurse(sequence, pivotIndex + 1, hi, comparer);
                }
            }
        }

        private static void MergeSegments<T>(IList<T> sequence, int llo, int lhi, int rlo, int rhi, IComparer<T> comparer)
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
        public static IEnumerable<T> Interleave<T>(IComparer<T> comparer, IEnumerable<T> sequence, params IEnumerable<T>[] sequences)
        {
            Validate.ArgumentNotNull(nameof(comparer), comparer);
            Validate.ArgumentNotNull(nameof(sequence), sequence);
            Validate.ArgumentNotNull(nameof(sequences), sequences);

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
                QuickSortRecurse(sequence, startIndex, startIndex + length - 1, comparer);
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
    }
}
