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
    using System.Diagnostics;
    using System.Collections.Generic;
    using Graphs;

    internal static class Validate
    {
        public static void ArgumentDifferentThan(string argumentName, int value, int bound)
        {
            Debug.Assert(!string.IsNullOrEmpty(argumentName), $"Argument {nameof(argumentName)} cannot be null or empty.");

            if (value == bound)
                throw new ArgumentOutOfRangeException(argumentName, $"Argument {argumentName} must be different from {bound}.");
        }

        public static void ArgumentGreaterThan(string argumentName, int value, int bound)
        {
            Debug.Assert(!string.IsNullOrEmpty(argumentName), $"Argument {nameof(argumentName)} cannot be null or empty.");

            if (value <= bound)
                throw new ArgumentOutOfRangeException(argumentName, $"Argument {argumentName} must be greater than {bound}.");
        }

        public static void ArgumentLessThan(string argumentName, int value, int bound)
        {
            Debug.Assert(!string.IsNullOrEmpty(argumentName), $"Argument {nameof(argumentName)} cannot be null or empty.");

            if (value >= bound)
                throw new ArgumentOutOfRangeException(argumentName, $"Argument {argumentName} must be less than {bound}.");
        }

        public static void ArgumentLessThanOrEqualTo(string argumentName, int value, int bound)
        {
            Debug.Assert(!string.IsNullOrEmpty(argumentName), $"Argument {nameof(argumentName)} cannot be null or empty.");

            if (value > bound)
                throw new ArgumentOutOfRangeException(argumentName, $"Argument {argumentName} must be less than or equal to {bound}.");
        }

        public static void ArgumentGreaterThanOrEqualTo(string argumentName, int value, int bound)
        {
            Debug.Assert(!string.IsNullOrEmpty(argumentName), $"Argument {nameof(argumentName)} cannot be null or empty.");

            if (value < bound)
                throw new ArgumentOutOfRangeException(argumentName, $"Argument {argumentName} must be greater than or equal to {bound}.");
        }



        public static void ArgumentDifferentThanZero(string argumentName, int value)
        {
            ArgumentDifferentThan(argumentName, value, 0);
        }

        public static void ArgumentGreaterThanOrEqualToZero(string argumentName, int value)
        {
            ArgumentGreaterThanOrEqualTo(argumentName, value, 0);
        }

        public static void ArgumentGreaterThanZero(string argumentName, int value)
        {
            ArgumentGreaterThan(argumentName, value, 0);
        }

        public static void ArgumentNotNull(string argumentName, object value)
        {
            Debug.Assert(!string.IsNullOrEmpty(argumentName), $"Argument {nameof(argumentName)} cannot be null or empty.");

            if (value == null)
                throw new ArgumentNullException(argumentName, $"Argument {argumentName} must not be null.");
        }

        public static void SequenceArgumentNotEmpty<T>(string argumentName, IEnumerable<T> sequence)
        {
            Debug.Assert(!string.IsNullOrEmpty(argumentName), $"Argument {nameof(argumentName)} cannot be null or empty.");

            ArgumentNotNull(argumentName, sequence);
            var collection = sequence as ICollection<T>;
            if (collection != null)
            {
                if (collection.Count == 0)
                    throw new ArgumentException($"Argument {argumentName} must not be empty.", argumentName);

                return;
            }

            var @string = sequence as string;
            if (@string != null)
            {
                if (@string.Length == 0)
                    throw new ArgumentException($"Argument {argumentName} must not be empty.", argumentName);

                return;
            }

            var enumerator = sequence.GetEnumerator();
            if (!enumerator.MoveNext())
                throw new ArgumentException($"Argument {argumentName} must not be empty.", argumentName);
        }

        public static void CollectionArgumentsInBounds<T>(string sequenceArgName, ICollection<T> sequence, int startIndex, int length)
        {
            Debug.Assert(!string.IsNullOrEmpty(sequenceArgName), $"Argument {nameof(sequenceArgName)} cannot be null or empty.");

            ArgumentNotNull(nameof(sequence), sequence);
            if (startIndex < 0 || length < 0 || startIndex + length > sequence.Count)
            {
                throw new ArgumentOutOfRangeException($"The combination of start index ({startIndex}) and length ({length}) must be less of equal to {sequence.Count}");
            }
        }

        public static void CollectionArgumentsHasEvenNumberOfElements<T>(string sequenceArgName, ICollection<T> sequence)
        {
            Debug.Assert(!string.IsNullOrEmpty(sequenceArgName), $"Argument {nameof(sequenceArgName)} cannot be null or empty.");

            ArgumentNotNull(nameof(sequence), sequence);
            if (sequence.Count % 2 != 0)
            {
                throw new ArgumentException($"The sequence {sequenceArgName} is expected to have an even number of elements.", sequenceArgName);
            }
        }
    }
}
