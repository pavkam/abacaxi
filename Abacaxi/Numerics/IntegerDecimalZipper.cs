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
    /// <summary>
    /// Class that allows zipping of digits of two numbers into a new number. For example, <code>12 and 34</code>, zipped toghether would result in
    /// <code>3142</code>. The class also takes care of the special case of <code>0</code> and allows zipping digits in multiple bases.
    /// </summary>
    public static class IntegerDecimalZipper
    {
        private static void AppendLastDigit(ref int number, ref int result, ref int power, int @base)
        {
            if (number >= 0)
            {
                var digit = number % @base;
                number /= @base;
                if (number == 0)
                {
                    number = -1;
                }

                result = result >= 0 ? (digit * power) + result : digit;
                power *= @base;
            }
        }

        /// <summary>
        /// Zips the digits of two integer numbers to form a new integer number.
        /// </summary>
        /// <param name="x">The first number to zip.</param>
        /// <param name="y">The second number to zip.</param>
        /// <param name="base">The base of the digits.</param>
        /// <returns>A number whose digits are taken from both <paramref name="x"/> and <paramref name="y"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="x"/> or <paramref name="y"/> are less than zero; or <paramref name="@base"/> is less than two.</exception>
        public static int Zip(int x, int y, int @base = 10)
        {
            Validate.ArgumentGreaterThanOrEqualToZero(nameof(x), x);
            Validate.ArgumentGreaterThanOrEqualToZero(nameof(y), y);
            Validate.ArgumentGreaterThanOne(nameof(@base), @base);

            var result = -1;
            var power = 1;
            while (x >= 0 || y >= 0)
            {
                AppendLastDigit(ref x, ref result, ref power, @base);
                AppendLastDigit(ref y, ref result, ref power, @base);
            }

            return result;
        }
    }
}
