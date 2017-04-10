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

namespace Abacaxi.Numerics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Implements the algorithm which finds a sequence of numbers in an integer array which, summed, return the biggest achievable sum.
    /// This algoritmh is a basically a 0/1 knapsack usinf dynamic programming.
    /// </summary>
    public static class LargestPossibleNaturalSummedSequence
    {
        /// <summary>
        /// Finds the sequence of integers, which summed, return the closest sum to a given <paramref name="targetSum"/>.
        /// </summary>
        /// <param name="sequence">The sequence of natural integers.</param>
        /// <param name="targetSum">The target sum to aim for.</param>
        /// <returns>A sequence of found integers.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the <paramref name="sequence"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="targetSum"/> is less than <c>1</c> or the <paramref name="sequence"/> contains negative number.</exception>
        public static IEnumerable<int> Find(IEnumerable<int> sequence, int targetSum)
        {
            Validate.ArgumentNotNull(nameof(sequence), sequence);
            Validate.ArgumentGreaterThanZero(nameof(targetSum), targetSum);

            var elements = sequence.ToArray();
            Array.Sort(elements);

            if (elements.Length > 0)
            {
                Validate.ArgumentGreaterThanOrEqualToZero(nameof(sequence), elements[0]);
            }

            var solutions = new int[targetSum + 1, elements.Length + 1];
            for (var si = 1; si <= elements.Length; si++)
            {
                for (var wi = 0; wi <= targetSum; wi ++)
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

            var rwi = targetSum;
            var rsi = elements.Length;
            while (rsi > 0 && rwi > 0)
            {
                if (solutions[rwi, rsi] > solutions[rwi, rsi - 1])
                {
                    rwi -= elements[rsi - 1];
                    yield return elements[rsi - 1];
                }

                rsi--;
            }
        }
    }
}
