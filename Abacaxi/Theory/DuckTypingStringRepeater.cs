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

namespace Abacaxi.Theory
{
    /// <summary>
    /// Class that allows repeating a string N times in an efficient manner.
    /// It uses the fact that any number N can be expressed as a sum of power of twos.
    /// </summary>
    /// <remarks>Note: This is only a theoretical algorithm and is not efficient.</remarks>
    public static class DuckTypingStringRepeater
    {
        public static string Repeat(string input, int repetitions)
        {
            Validate.StringNotEmpty(nameof(input), input);
            Validate.ArgumentGreaterThanZero(nameof(repetitions), repetitions);

            var output = string.Empty;
            while (repetitions > 0)
            {
                if (repetitions % 2 == 1)
                {
                    output += input;
                }

                repetitions >>= 1;
                if (repetitions > 0)
                    input += input;
            }

            return output;
        }
    }
}
