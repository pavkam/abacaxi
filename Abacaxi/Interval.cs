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
    using Containers;
    using Internal;
    using JetBrains.Annotations;

    /// <summary>
    ///     Helper class used for interval manipulation.
    /// </summary>
    [PublicAPI]
    public static class Interval
    {
        /// <summary>
        ///     Merges a sequence of overlapping intervals.
        /// </summary>
        /// <typeparam name="T">The type of interval's bound (e.g. DateTime or int).</typeparam>
        /// <param name="intervals">The intervals to merge.</param>
        /// <param name="comparer">The bound comparer.</param>
        /// <returns>An output sequence of all intervals (merged orr not).</returns>
        /// <exception cref="ArgumentNullException">
        ///     Thrown if either of <paramref name="intervals" /> or <paramref name="comparer" />
        ///     are <c>null</c>.
        /// </exception>
        [NotNull]
        public static (T start, T end)[] MergeOverlapping<T>(
            [NotNull] IEnumerable<(T start, T end)> intervals,
            [NotNull] IComparer<T> comparer)
        {
            Validate.ArgumentNotNull(nameof(intervals), intervals);
            Validate.ArgumentNotNull(nameof(comparer), comparer);

            /* Sort and merge intervals. */
            var inComparer = Comparer<(T start, T end)>.Create((l, r) => -comparer.Compare(l.start, r.start));
            var heap = new Heap<(T start, T end)>(inComparer);

            foreach (var interval in intervals)
            {
                if (comparer.Compare(interval.start, interval.end) > 0)
                {
                    throw new InvalidOperationException(
                        $"Invalid interval supplied: {interval.end} cannot be less than {interval.start}.");
                }

                heap.Add(interval);
            }

            var result = new List<(T start, T end)>();
            while (heap.Count > 0)
            {
                var interval = heap.RemoveTop();
                if (result.Count > 0)
                {
                    var last = result[result.Count - 1];
                    if (comparer.Compare(last.end, interval.start) >= 0)
                    {
                        if (comparer.Compare(last.end, interval.end) < 0)
                        {
                            result[result.Count - 1] = (last.start, interval.end);
                        }

                        continue;
                    }
                }

                result.Add(interval);
            }

            return result.ToArray();
        }
    }
}