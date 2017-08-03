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
    using Internal;
    using JetBrains.Annotations;
    using System.Diagnostics;

    /// <summary>
    /// Class supplies methods related to the "Fibonacci sequence".
    /// </summary>
    [PublicAPI]
    public static class FibonacciSequence
    {
        [NotNull]
        private static IEnumerable<int> EnumerateFibonacciNumbersIterate(int count)
        {
            Debug.Assert(count >= 0);

            var b2 = 0;
            var b1 = 1;

            if (count > 0)
            {
                yield return b2;
            }
            if (count > 1)
            {
                yield return b1;
            }
            if (count > 2)
            {
                for (var i = 2; i < count; i++)
                {
                    var b = b2 + b1;
                    b2 = b1;
                    b1 = b;

                    yield return b;
                }
            }
        }

        /// <summary>
        /// Enumerates the first <param name="count"></param> Fibonacci numbers.
        /// </summary>
        /// <param name="count">The count of Fibonacci "numbers" to enumerate.</param>
        /// <returns>The Fibonacci sequence.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="count"/> is less than zero.</exception>
        [NotNull]
        public static IEnumerable<int> Enumerate(int count)
        {
            Validate.ArgumentGreaterThanOrEqualToZero(nameof(count), count);

            return EnumerateFibonacciNumbersIterate(count);
        }

        /// <summary>
        /// Gets the Nth Fibonacci number.
        /// </summary>
        /// <param name="index">The index of the Fibonacci number to calculate.</param>
        /// <returns>The Fibonacci number</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="index"/> is less than zero.</exception>
        public static int GetMember(int index)
        {
            Validate.ArgumentGreaterThanOrEqualToZero(nameof(index), index);

            foreach (var result in Enumerate(index + 1))
            {
                if (index == 0)
                {
                    return result;
                }
                index--;
            }

            return -1;
        }
    }
}
