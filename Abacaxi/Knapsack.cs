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
    using Internal;
    using JetBrains.Annotations;

    /// <summary>
    /// Class implements a set of  knapsack algorithms.
    /// </summary>
    [PublicAPI]
    public static class Knapsack
    {
        /// <summary>
        /// Finds the best combination of items to be placed in a knapsack of given <paramref name="knapsackWeight" /> weight.
        /// </summary>
        /// <typeparam name="T">The type of item.</typeparam>
        /// <param name="sequence">The sequence of item/value/weight elements.</param>
        /// <param name="knapsackWeight">The total knapsack weight.</param>
        /// <returns>
        /// The best selection of items filling the knapsack and maximizing total value.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="sequence" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="knapsackWeight"/> is less than one.</exception>
        [NotNull]
        public static T[] Fill<T>([NotNull] IEnumerable<KnapsackItem<T>> sequence, int knapsackWeight)
        {
            Validate.ArgumentNotNull(nameof(sequence), sequence);
            Validate.ArgumentGreaterThanZero(nameof(knapsackWeight), knapsackWeight);

            var elements = sequence.OrderBy(s => s.Weight).ToArray();

            var m = new double[elements.Length + 1, knapsackWeight + 1];
            for (var i = 0; i <= elements.Length; i++)
            {
                for (var w = 0; w <= knapsackWeight; w++)
                {
                    if (i == 0 || w == 0)
                        m[i, w] = 0;
                    else
                    {
                        var ei1 = elements[i - 1];
                        if (ei1.Weight <= w)
                            m[i, w] = Math.Max(ei1.Value + m[i - 1, w - ei1.Weight], m[i - 1, w]);
                        else
                            m[i, w] = m[i - 1, w];
                    }
                }
            }

            var rwi = knapsackWeight;
            var rsi = elements.Length;
            var result = new List<T>();
            while (rsi > 0 && rwi > 0)
            {
                if (m[rsi, rwi] > m[rsi - 1, rwi])
                {
                    rwi -= elements[rsi - 1].Weight;
                    result.Add(elements[rsi - 1].Item);
                }

                rsi--;
            }

            return result.ToArray();
        }
    }
}