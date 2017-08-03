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
    /// Class implements set coverage algorithms.
    /// </summary>
    [PublicAPI]
    public static class SetCoverage
    {
        [NotNull]
        [ItemNotNull]
        public static IEnumerable<ISet<T>> FindMinimumNumberOfSetsWithFullCoverageIterate<T>(
            [NotNull] [ItemNotNull] IEnumerable<ISet<T>> sets,
            [NotNull] IEqualityComparer<T> comparer)
        {
            Debug.Assert(sets != null);
            Debug.Assert(comparer != null);

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
        /// Finds the minimum number of sets that cover the full set of elements.
        /// </summary>
        /// <typeparam name="T">The type of elements in the set.</typeparam>
        /// <param name="sets">The sets.</param>
        /// <param name="comparer">The comparer.</param>
        /// <returns>A sequence of selected sets whose union results in the full coverage.</returns>
        /// <exception cref="ArgumentNullException">Thrown if either <paramref name="sets"/> or <paramref name="comparer"/> is <c>null</c>.</exception>
        [NotNull]
        [ItemNotNull]
        public static IEnumerable<ISet<T>> FindOptimalFullCoverage<T>(
            [NotNull] [ItemNotNull] IEnumerable<ISet<T>> sets,
            [NotNull] IEqualityComparer<T> comparer)
        {
            Validate.ArgumentNotNull(nameof(sets), sets);
            Validate.ArgumentNotNull(nameof(comparer), comparer);

            return FindMinimumNumberOfSetsWithFullCoverageIterate(sets, comparer);
        }
    }
}