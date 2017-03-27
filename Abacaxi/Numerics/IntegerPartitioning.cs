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

    /// <summary>
    /// Partitions an integer number into all possible combinations of smaller numbers. When summed, the original integer number is
    /// obtained. All combinations are unique. E.g. 1 1 2; 2 1 1 and 1 2 1 are considered equal and only one will be returned.
    /// </summary>
    public static class IntegerPartitioning
    {
        /// <summary>
        /// Partitions a given integer into all possible combinations of smaller integers.
        /// </summary>
        /// <param name="number">The input number.</param>
        /// <returns>A sequence of combinations.</returns>
        public static IEnumerable<IEnumerable<int>> Decompose(int number)
        {
            if (number == 0)
            {
                yield break;
            }
            else
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
                        yield return  selection.ToArray();
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
    }
}
