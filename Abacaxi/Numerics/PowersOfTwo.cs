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

    public static class PowersOfTwo
    {
        /// <summary>
        /// Returns a sequence of numbers (powers of two), which summed, result in the original number <paramref name="number"/>.
        /// </summary>
        /// <remarks>
        /// If number is 
        /// </remarks>
        /// <param name="number">The number to be decomposed.</param>
        /// <returns>A sequence of numbers.</returns>
        public static IEnumerable<int> Decompose(int number)
        {
            var sign = Math.Sign(number);

            var power = 1;
            while (number != 0)
            {
                if (number % 2 != 0)
                    yield return sign * power;

                power *= 2;
                number /= 2;
            }
        }
    }
}
