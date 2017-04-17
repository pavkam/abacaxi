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
    /// Class implements an algorithm to do division without using the * or / operators.
    /// </summary>
    public static class StupidDivision
    {
        /// <summary>
        /// Divides <paramref name="number"/> by <paramref name="divisor"/>.
        /// </summary>
        /// <param name="number">The number to divide.</param>
        /// <param name="divisor">The divisor.</param>
        /// <returns>The result of division.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="divisor"/> is <c>0</c>.</exception>
        public static int Divide(int number, int divisor)
        {
            Validate.ArgumentDifferentThanZero(nameof(divisor), divisor);

            var neg = 
                number < 0 && divisor > 0 ||
                number > 0 && divisor < 0;

            number = Math.Abs(number);
            divisor = Math.Abs(divisor);

            var multipliers = new List<int>();
            var kappa = divisor;
            while (kappa <= number)
            {
                multipliers.Add(kappa);
                kappa += kappa;
            }

            var result = 0;
            var i = multipliers.Count - 1;
            while (number >= divisor)
            {
                if (multipliers[i] <= number)
                {
                    number -= multipliers[i];
                    result += (1 << i);
                }

                i--;
            }

            if (neg)
                result = -result;

            return result;
        }
    }
}
