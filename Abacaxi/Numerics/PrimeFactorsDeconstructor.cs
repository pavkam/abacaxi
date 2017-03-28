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
    /// Deconstructs any integer number into its prime factors. Positive numbers will be deconstructed into positive factors whie negative will be deconstructed
    /// into only negative numbers.
    /// </summary>
    public class PrimeFactorsDeconstructor: ISequentialDeconstructor<int, int>
    {
        private int GetIterationLimit(int number)
        {
            double sqrt;
            if (number > 0)
            {
                sqrt = Math.Sqrt(number);
            }
            else if (number > int.MinValue)
            {
                sqrt = Math.Sqrt(Math.Abs(number));
            }
            else
            {
                sqrt = Math.Sqrt(Math.Abs(number + 1));
            }

            return (int)sqrt;
        }

        /// <summary>
        /// Returns a sequence of numbers, which, when multiplied produce the value of <paramref name="number"/>.
        /// </summary>
        /// <param name="number">The number to be deconstructed into its prime factors.</param>
        /// <returns>A sequence of primes.</returns>
        public virtual IEnumerable<int> Deconstruct(int number)
        {
            var sign = Math.Sign(number);
            
            if (number == sign)
            {
                yield return number;
            }
            else
            {
                var limit = GetIterationLimit(number);
                var factors = 0;
                var i = 2;

                while (i <= limit)
                {
                    if (number % i == 0)
                    {
                        factors++;

                        yield return sign * i;
                        number = number / i;
                        limit = GetIterationLimit(number);
                    }
                    else
                    {
                        i ++;
                    }
                }

                if (number != sign)
                {
                    yield return number;
                    factors++;
                }

                if (sign == -1 && factors % 2 == 0)
                    yield return sign;
            }
        }
    }
}
