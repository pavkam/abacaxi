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
    using Internal;
    using JetBrains.Annotations;

    /// <summary>
    ///     Helper class that defines a number of methods useful in integer/algorithmic manipulations.
    /// </summary>
    [PublicAPI]
    public static class Integer
    {
        private static int GetIterationLimit(int number)
        {
            double squareRoot;
            if (number > 0)
            {
                squareRoot = Math.Sqrt(number);
            }
            else if (number > int.MinValue)
            {
                squareRoot = Math.Sqrt(Math.Abs(number));
            }
            else
            {
                squareRoot = Math.Sqrt(Math.Abs(number + 1));
            }

            return (int) squareRoot;
        }

        private static void AppendLastDigit(ref int number, ref int result, ref int power, int @base)
        {
            if (number < 0)
            {
                return;
            }

            var digit = number % @base;
            number /= @base;
            if (number == 0)
            {
                number = -1;
            }

            result = result >= 0 ? digit * power + result : digit;
            power *= @base;
        }

        /// <summary>
        ///     Returns a sequence of numbers (powers of two), which summed, result in the original number
        ///     <paramref name="number" />.
        /// </summary>
        /// <param name="number">The number to be decomposed.</param>
        /// <returns>A sequence of numbers.</returns>
        [NotNull]
        public static IEnumerable<int> DeconstructIntoPowersOfTwo(int number)
        {
            var sign = Math.Sign(number);

            var power = 1;
            while (number != 0)
            {
                if (number % 2 != 0)
                {
                    yield return sign * power;
                }

                power *= 2;
                number /= 2;
            }
        }

        /// <summary>
        ///     Returns a sequence of numbers, which, when multiplied produce the value of <paramref name="number" />.
        /// </summary>
        /// <param name="number">The number to be dis-constructed into its prime factors.</param>
        /// <returns>A sequence of primes.</returns>
        [NotNull]
        public static IEnumerable<int> DeconstructIntoPrimeFactors(int number)
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
                        i++;
                    }
                }

                if (number != sign)
                {
                    yield return number;

                    factors++;
                }

                if (sign == -1 &&
                    factors % 2 == 0)
                {
                    yield return sign;
                }
            }
        }

        /// <summary>
        ///     Checks whether a given number is prime.
        /// </summary>
        /// <param name="number">The number to check.</param>
        /// <returns><c>true</c> if the number is prime; <c>false</c> otherwise.</returns>
        public static bool IsPrime(int number)
        {
            // ReSharper disable once GenericEnumeratorNotDisposed
            var enumerator = DeconstructIntoPrimeFactors(number).GetEnumerator();
            return enumerator.MoveNext() && !enumerator.MoveNext();
        }

        /// <summary>
        ///     Zips the digits of two integer numbers to form a new integer number.
        /// </summary>
        /// <param name="x">The first number to zip.</param>
        /// <param name="y">The second number to zip.</param>
        /// <param name="base">The base of the digits.</param>
        /// <returns>A number whose digits are taken from both <paramref name="x" /> and <paramref name="y" />.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Thrown if <paramref name="x" /> or <paramref name="y" /> are less than
        ///     zero; or <paramref name="base" /> is less than two.
        /// </exception>
        public static int Zip(int x, int y, int @base = 10)
        {
            Validate.ArgumentGreaterThanOrEqualToZero(nameof(x), x);
            Validate.ArgumentGreaterThanOrEqualToZero(nameof(y), y);
            Validate.ArgumentGreaterThanOrEqualTo(nameof(@base), @base, 2);

            var result = -1;
            var power = 1;
            while (x >= 0 ||
                   y >= 0)
            {
                AppendLastDigit(ref x, ref result, ref power, @base);
                AppendLastDigit(ref y, ref result, ref power, @base);
            }

            return result;
        }

        /// <summary>
        ///     Breaks the specified natural number into smaller components minimizing the number of time those components are
        ///     used.
        /// </summary>
        /// <param name="number">The number to break.</param>
        /// <param name="components">The components that can be used.</param>
        /// <returns>
        ///     A list of components and the number of times they are used. An empty array is returned if the number cannot be
        ///     broken into specified components.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Thrown if <paramref name="number" /> is less than zero or items in <paramref name="components" /> are less than
        ///     one.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///     Thrown if <paramref name="components" /> is <c>null</c>.
        /// </exception>
        [NotNull]
        public static (int component, int count)[] Break(int number, [NotNull] params int[] components)
        {
            Validate.ArgumentGreaterThanOrEqualToZero(nameof(number), number);
            Validate.ArgumentNotNull(nameof(components), components);

            foreach (var c in components)
            {
                Validate.ArgumentGreaterThanZero(nameof(components), c);
            }

            /* Do the dynamic dance */
            var mem = new (int score, int component)[number + 1];
            mem[0] = (0, 0);

            for (var i = 1; i <= number; i++)
            {
                var min = int.MaxValue;
                var component = -1;

                for (var j = 0; j < components.Length; j++)
                {
                    if (components[j] <= i)
                    {
                        var p = mem[i - components[j]];
                        if (p.component > -1 && p.score < min)
                        {
                            min = p.score;
                            component = j;
                        }
                    }
                }

                mem[i] = (min + 1, component);
            }

            /* Extract the results */
            var z = number;
            var mx = new int[components.Length];
            while (z > 0 && mem[z].component >= 0)
            {
                mx[mem[z].component]++;
                z = z - components[mem[z].component];
            }

            /* Convert the results into output form. */
            var output = new List<(int component, int count)>(components.Length);
            for (var x = 0; x < components.Length; x++)
            {
                if (mx[x] > 0)
                {
                    output.Add((components[x], mx[x]));
                }
            }

            return output.ToArray();
        }
    }
}