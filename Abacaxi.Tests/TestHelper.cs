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

namespace Abacaxi.Tests
{
    using NUnit.Framework;
    using System.Collections.Generic;
    using System.Linq;
    using JetBrains.Annotations;

    internal static class TestHelper
    {
        public static void AssertSequence<T>([NotNull] IEnumerable<T> sequence, [NotNull] params T[] expected)
        {
            Assert.NotNull(sequence, "The sequence is null.");

            var array = sequence.ToArray();
            Assert.AreEqual(expected.Length, array.Length, $"The length of the sequence [{array.Length}] does not match the expected length.");

            for (var i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(expected[i], array[i], $"Element [{i}] of the sequence does not match the expected value.");
            }
        }

        public static void AssertSequence<T>([NotNull] IEnumerable<IEnumerable<T>> sequence, [NotNull] params T[][] expected)
        {
            Assert.NotNull(sequence);

            var array = sequence.ToArray();
            Assert.AreEqual(expected.Length, array.Length);

            for (var i = 0; i < expected.Length; i++)
            {
                AssertSequence(array[i], expected[i]);
            }
        }

        public static void AssertSequence<T>([NotNull] IEnumerable<T[][]> sequence, [NotNull] params T[][][] expected)
        {
            Assert.NotNull(sequence);

            var array = sequence.ToArray();
            Assert.AreEqual(expected.Length, array.Length);

            for (var i = 0; i < expected.Length; i++)
            {
                AssertSequence(array[i], expected[i]);
            }
        }
    }
}
