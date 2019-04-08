/* Copyright 2017-2019 by Alexandru Ciobanu (alex+git@ciobanu.org)
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
    using JetBrains.Annotations;

    /// <summary>
    ///     Class implements integer partitioning algorithms.
    /// </summary>
    [PublicAPI]
    public static class IntegerPartitions
    {
        /// <summary>
        ///     Partitions a given integer into all possible combinations of smaller integers.
        /// </summary>
        /// <param name="number">The input number.</param>
        /// <returns>A sequence of combinations.</returns>
        [ItemNotNull, NotNull]
        public static IEnumerable<int[]> Enumerate(int number)
        {
            if (number == 0)
            {
                yield break;
            }

            var selection = new Stack<int>();
            var numbers = new Stack<int>();
            var i = 0;
            var doNotBreak = false;
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
                else if (i * sign > number / 2 * sign || doNotBreak)
                {
                    if (selection.Count == 0)
                    {
                        break;
                    }

                    number = numbers.Pop();
                    i = selection.Pop() + sign;

                    doNotBreak = false;
                }
                else
                {
                    selection.Push(i);
                    numbers.Push(number);

                    doNotBreak = i * 2 == number;

                    number = number - i;
                    i = 0;
                }
            }
        }

        /// <summary>
        ///     Calculates the count of partitions that a <paramref name="number" /> can be split into.
        /// </summary>
        /// <param name="number">The number to split.</param>
        /// <returns>The partition count.</returns>
        public static int GetCount(int number)
        {
            number = Math.Abs(number);

            var solutions = new int[number + 1, number + 1];

            for (var m = 0; m <= number; m++)
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

            return solutions[number, number];
        }
    }
}