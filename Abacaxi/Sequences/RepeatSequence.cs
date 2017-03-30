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

namespace Abacaxi.Sequences
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Class that allows repeating an enumerable N times in an efficient manner.
    /// It uses the fact that any number N can be expressed as a sum of power of twos.
    /// </summary>
    public static class RepeatSequence
    {
        /// <summary>
        /// Creates an array whose contents are the elements of the <paramref name="input"/> repeated <paramref name="repetitions"/> times.
        /// </summary>
        /// <typeparam name="T">The type of the sequence's elements</typeparam>
        /// <param name="input">The input sequence.</param>
        /// <param name="repetitions">Number of times to repeat the sequence.</param>
        /// <returns>A new array.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the <paramref name="input"/> sequence is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the value of <paramref name="repetitions"/> argument is less than <c>1</c>.</exception>
        public static T[] Repeat<T>(IEnumerable<T> input, int repetitions)
        {
            Validate.ArgumentNotNull(nameof(input), input);
            Validate.ArgumentGreaterThanZero(nameof(repetitions), repetitions);

            var arrayInput = input.ToArray();
            var arrayOutput = new T[arrayInput.Length * repetitions];
            var outputIndex = 0;

            while (repetitions > 0)
            {
                if (repetitions % 2 == 1)
                {
                    Array.Copy(arrayInput, 0, arrayOutput, outputIndex, arrayInput.Length);
                    outputIndex += arrayInput.Length;
                }

                repetitions >>= 1;
                if (repetitions > 0)
                {
                    var currentLength = arrayInput.Length;
                    Array.Resize(ref arrayInput, currentLength * 2);
                    Array.Copy(arrayInput, 0, arrayInput, currentLength, currentLength);
                }
            }

            return arrayOutput;
        }
    }
}
