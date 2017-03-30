﻿/* Copyright 2017 by Alexandru Ciobanu (alex+git@ciobanu.org)
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
    using System.Diagnostics;

    internal static class Validate
    {
        public static void ArgumentLessThan(string argumentName, int value, int bound)
        {
            Debug.Assert(!string.IsNullOrEmpty(argumentName), $"Argument {nameof(argumentName)} cannot be null or empty.");

            if (value >= bound)
                throw new ArgumentOutOfRangeException(argumentName, $"Argument {argumentName} must be less than {bound}.");
        }

        public static void ArgumentGreaterOrEqualTo(string argumentName, int value, int bound)
        {
            Debug.Assert(!string.IsNullOrEmpty(argumentName), $"Argument {nameof(argumentName)} cannot be null or empty.");

            if (value < bound)
                throw new ArgumentOutOfRangeException(argumentName, $"Argument {argumentName} must be greater than or equal to {bound}.");
        }

        public static void ArgumentGreaterThanOrEqualToZero(string argumentName, int value)
        {
            Debug.Assert(!string.IsNullOrEmpty(argumentName), $"Argument {nameof(argumentName)} cannot be null or empty.");

            if (value < 0)
                throw new ArgumentOutOfRangeException(argumentName, $"Argument {argumentName} must be greater or equal to zero.");
        }

        public static void ArgumentGreaterThanZero(string argumentName, int value)
        {
            Debug.Assert(!string.IsNullOrEmpty(argumentName), $"Argument {nameof(argumentName)} cannot be null or empty.");

            if (value < 1)
                throw new ArgumentOutOfRangeException(argumentName, $"Argument {argumentName} must be greater than zero.");
        }

        public static void ArgumentGreaterThanOne(string argumentName, int value)
        {
            Debug.Assert(!string.IsNullOrEmpty(argumentName), $"Argument {nameof(argumentName)} cannot be null or empty.");

            if (value < 2)
                throw new ArgumentOutOfRangeException(argumentName, $"Argument {argumentName} must be greater than one.");
        }

        public static void ArgumentNotNull(string argumentName, object value)
        {
            Debug.Assert(!string.IsNullOrEmpty(argumentName), $"Argument {nameof(argumentName)} cannot be null or empty.");

            if (value == null)
                throw new ArgumentNullException(argumentName, $"Argument {argumentName} must not be null.");
        }

        public static void ArrayNotEmpty<T>(string argumentName, T[] array)
        {
            Debug.Assert(!string.IsNullOrEmpty(argumentName), $"Argument {nameof(argumentName)} cannot be null or empty.");

            if (array == null)
                throw new ArgumentNullException(argumentName, $"Array argument {argumentName} must not be null.");
            if (array.Length == 0)
                throw new ArgumentException($"Array argument {argumentName} must not be empty.", argumentName);
        }

        public static void StringNotEmpty(string argumentName, string array)
        {
            Debug.Assert(!string.IsNullOrEmpty(argumentName), $"Argument {nameof(argumentName)} cannot be null or empty.");

            if (array == null)
                throw new ArgumentNullException(argumentName, $"String argument {argumentName} must not be null.");
            if (array.Length == 0)
                throw new ArgumentException($"String argument {argumentName} must not be empty.", argumentName);
        }
    }
}
