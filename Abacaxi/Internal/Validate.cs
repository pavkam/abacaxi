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

namespace Abacaxi.Internal
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using JetBrains.Annotations;

    [SuppressMessage("ReSharper", "UnusedParameter.Global"),
     SuppressMessage("ReSharper", "MemberCanBePrivate.Global"),
     SuppressMessage("ReSharper", "UnusedMember.Global"),
     SuppressMessage("ReSharper", "ParameterOnlyUsedForPreconditionCheck.Global")]
    internal static class Validate
    {
        public static void ArgumentDifferentThan(
            [NotNull, InvokerParameterName] string argumentName,
            int value,
            int bound)
        {
            Assert.Condition(!string.IsNullOrEmpty(argumentName));

            if (value == bound)
            {
                throw new ArgumentOutOfRangeException(argumentName,
                    $"Argument {argumentName} must be different from {bound}.");
            }
        }

        public static void ArgumentGreaterThan(
            [NotNull, InvokerParameterName] string argumentName,
            int value,
            int bound)
        {
            Assert.Condition(!string.IsNullOrEmpty(argumentName));

            if (value <= bound)
            {
                throw new ArgumentOutOfRangeException(argumentName,
                    $"Argument {argumentName} must be greater than {bound}.");
            }
        }

        public static void ArgumentGreaterThan(
            [NotNull, InvokerParameterName] string argumentName,
            double value,
            double bound)
        {
            Assert.Condition(!string.IsNullOrEmpty(argumentName));

            if (value <= bound)
            {
                throw new ArgumentOutOfRangeException(argumentName,
                    $"Argument {argumentName} must be greater than {bound}.");
            }
        }

        public static void ArgumentLessThan(
            [NotNull, InvokerParameterName] string argumentName,
            int value,
            int bound)
        {
            Assert.Condition(!string.IsNullOrEmpty(argumentName));

            if (value >= bound)
            {
                throw new ArgumentOutOfRangeException(argumentName,
                    $"Argument {argumentName} must be less than {bound}.");
            }
        }

        public static void ArgumentLessThan(
            [NotNull, InvokerParameterName] string argumentName,
            double value,
            double bound)
        {
            Assert.Condition(!string.IsNullOrEmpty(argumentName));

            if (value >= bound)
            {
                throw new ArgumentOutOfRangeException(argumentName,
                    $"Argument {argumentName} must be less than {bound}.");
            }
        }

        public static void ArgumentLessThanOrEqualTo(
            [NotNull, InvokerParameterName] string argumentName,
            int value,
            int bound)
        {
            Assert.Condition(!string.IsNullOrEmpty(argumentName));

            if (value > bound)
            {
                throw new ArgumentOutOfRangeException(argumentName,
                    $"Argument {argumentName} must be less than or equal to {bound}.");
            }
        }

        public static void ArgumentLessThanOrEqualTo(
            [NotNull, InvokerParameterName] string argumentName,
            double value,
            double bound)
        {
            Assert.Condition(!string.IsNullOrEmpty(argumentName));

            if (value > bound)
            {
                throw new ArgumentOutOfRangeException(argumentName,
                    $"Argument {argumentName} must be less than or equal to {bound}.");
            }
        }

        public static void ArgumentGreaterThanOrEqualTo(
            [NotNull, InvokerParameterName] string argumentName,
            int value,
            int bound)
        {
            Assert.Condition(!string.IsNullOrEmpty(argumentName));

            if (value < bound)
            {
                throw new ArgumentOutOfRangeException(argumentName,
                    $"Argument {argumentName} must be greater than or equal to {bound}.");
            }
        }

        public static void ArgumentGreaterThanOrEqualTo(
            [NotNull, InvokerParameterName] string argumentName,
            double value,
            double bound)
        {
            Assert.Condition(!string.IsNullOrEmpty(argumentName));

            if (value < bound)
            {
                throw new ArgumentOutOfRangeException(argumentName,
                    $"Argument {argumentName} must be greater than or equal to {bound}.");
            }
        }

        public static void ArgumentDifferentThanZero([NotNull, InvokerParameterName] string argumentName, int value)
        {
            ArgumentDifferentThan(argumentName, value, 0);
        }

        public static void ArgumentGreaterThanOrEqualToZero([NotNull, InvokerParameterName] string argumentName,
            int value)
        {
            ArgumentGreaterThanOrEqualTo(argumentName, value, 0);
        }

        public static void ArgumentGreaterThanOrEqualToZero([NotNull, InvokerParameterName] string argumentName,
            double value)
        {
            ArgumentGreaterThanOrEqualTo(argumentName, value, 0);
        }

        public static void ArgumentGreaterThanZero([NotNull, InvokerParameterName] string argumentName, int value)
        {
            ArgumentGreaterThan(argumentName, value, 0);
        }

        public static void ArgumentGreaterThanZero([NotNull, InvokerParameterName] string argumentName, double value)
        {
            ArgumentGreaterThan(argumentName, value, 0);
        }

        [ContractAnnotation("value:null => halt")]
        public static void ArgumentNotNull([NotNull, InvokerParameterName] string argumentName,
            [CanBeNull] object value)
        {
            Assert.Condition(!string.IsNullOrEmpty(argumentName));

            if (value == null)
            {
                throw new ArgumentNullException(argumentName, $"Argument {argumentName} must not be null.");
            }
        }

        [ContractAnnotation("sequence:null => halt")]
        public static void SequenceArgumentNotEmpty<T>([NotNull, InvokerParameterName] string argumentName,
            [CanBeNull] IEnumerable<T> sequence)
        {
            Assert.Condition(!string.IsNullOrEmpty(argumentName));

            ArgumentNotNull(argumentName, sequence);
            if (sequence is ICollection<T> collection)
            {
                if (collection.Count == 0)
                {
                    throw new ArgumentException($"Argument {argumentName} must not be empty.", argumentName);
                }

                return;
            }

            using (var enumerator = sequence.GetEnumerator())
            {
                if (!enumerator.MoveNext())
                {
                    throw new ArgumentException($"Argument {argumentName} must not be empty.", argumentName);
                }
            }
        }

        [ContractAnnotation("sequence:null => halt")]
        public static void CollectionArgumentsInBounds<T>([NotNull, InvokerParameterName] string sequenceArgName,
            [NotNull] ICollection<T> sequence, int startIndex, int length)
        {
            Assert.Condition(!string.IsNullOrEmpty(sequenceArgName));

            ArgumentNotNull(nameof(sequence), sequence);
            if (startIndex < 0 ||
                length < 0 ||
                startIndex + length > sequence.Count)
            {
                throw new ArgumentOutOfRangeException(
                    $"The combination of start index ({startIndex}) and length ({length}) must be less of equal to {sequence.Count}");
            }
        }

        [ContractAnnotation("sequence:null => halt")]
        public static void CollectionArgumentsHasEvenNumberOfElements<T>(
            [NotNull, InvokerParameterName] string sequenceArgName, [NotNull] ICollection<T> sequence)
        {
            Assert.Condition(!string.IsNullOrEmpty(sequenceArgName));

            ArgumentNotNull(nameof(sequence), sequence);
            if (sequence.Count % 2 != 0)
            {
                throw new ArgumentException(
                    $"The sequence {sequenceArgName} is expected to have an even number of elements.", sequenceArgName);
            }
        }
    }
}