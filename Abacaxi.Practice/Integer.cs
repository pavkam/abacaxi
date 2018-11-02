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

namespace Abacaxi.Practice
{
    using System;
    using System.Collections.Generic;
    using Internal;

    /// <summary>
    ///     Class that only contains practice algorithms related to integers.
    /// </summary>
    public static class Integer
    {
        /// <summary>
        ///     Divides <paramref name="number" /> by <paramref name="divisor" /> using only addition.
        /// </summary>
        /// <param name="number">The number to divide.</param>
        /// <param name="divisor">The divisor.</param>
        /// <returns>The result of division.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="divisor" /> is <c>0</c>.</exception>
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
                    result += 1 << i;
                }

                i--;
            }

            if (neg)
            {
                result = -result;
            }

            return result;
        }

        /// <summary>
        /// Sums the two integers using only bitwise operations.
        /// </summary>
        /// <param name="a">The first value.</param>
        /// <param name="b">The second value.</param>
        /// <returns>The sum of <paramref name="a"/> and <paramref name="b"/>.</returns>
        public static int Sum(int a, int b)
        {
            while (b != 0)
            {
                var z = a ^ b;

                b = (a & b) << 1;
                a = z;
            }

            return a;
        }

        /// <summary>
        /// Swaps the values of two variables without using an intermediary variable (using XORs).
        /// </summary>
        /// <param name="a">The first variable.</param>
        /// <param name="b">The second variable.</param>
        public static void Swap(ref int a, ref int b)
        {
            a = a ^ b;
            b = a ^ b;
            a = a ^ b;
        }

        /// <summary>
        /// Gets the count of trailing zeroes in factorial.
        /// </summary>
        /// <param name="n">The number to factor.</param>
        /// <returns>The number of trailing zeroes in the factorial.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="n" /> is negative.</exception>
        public static int GetCountOfTrailingZeroesInFactorial(int n)
        {
            Validate.ArgumentGreaterThanOrEqualToZero(nameof(n), n);

            var count = 0;
            for (var i = 2; i <= n; i++)
            {
                var k = i;
                while (k % 5 == 0)
                {
                    k = k / 5;
                    count++;
                }
            }

            return count;
        }

        /// <summary>
        /// Returns the maximum of two numbers without using comparison operators.
        /// </summary>
        /// <param name="a">The first value.</param>
        /// <param name="b">The second value.</param>
        /// <returns>The maximum of two values.</returns>
        public static int Max(int a, int b)
        {
            /* Identify if a, b and their difference are positive (cha, chb and chd will be 1 if that is the case and 0 otherwise). */
            var cha = ((a >> 31) & 1) ^ 1;
            var chb = ((b >> 31) & 1) ^ 1;
            var chd = (((a - b) >> 31) & 1) ^ 1;

            /* Calculate the "max" using two cases - when the sign differs (z1)
             and when the sign is the same (z2). One of the terms will be equal to either a or b. Otherwise, zero. */
            var z1 = (cha ^ chb) * (cha * a + chb * b);
            var z2 = ((cha ^ chb) ^ 1) * (chd * a + (chd ^ 1) * b);

            /* Select either z1 or z2 (they are exclusive) */
            return z1 + z2;
        }
    }
}