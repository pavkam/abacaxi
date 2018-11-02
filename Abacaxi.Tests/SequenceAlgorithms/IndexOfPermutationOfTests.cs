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

namespace Abacaxi.Tests.SequenceAlgorithms
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using JetBrains.Annotations;
    using NUnit.Framework;

    [TestFixture]
    public sealed class IndexOfPermutationOfTests
    {
        [TestCase("a", "a", 0), TestCase("ab", "ab", 0), TestCase("abc", "b", 1), TestCase("abc", "c", 2),
         TestCase("abc", "bc", 1),
         TestCase("abcdefgh", "gef", 4), TestCase("abcdefgh", "hgfedcb", 1),
         SuppressMessage("ReSharper", "StringLiteralTypo")]
        public void IndexOfPermutationOf_ReturnsTheExpectedResult([NotNull] string seq, [NotNull] string sub,
            int expected)
        {
            var index = seq.AsList().IndexOfPermutationOf(sub.AsList(), EqualityComparer<char>.Default);

            Assert.AreEqual(expected, index);
        }

        [Test]
        public void IndexOfPermutationOf_ReturnsMinusOne_ForEmptySequence()
        {
            var index = "".AsList().IndexOfPermutationOf("a".AsList(), EqualityComparer<char>.Default);

            Assert.AreEqual(-1, index);
        }

        [Test]
        public void IndexOfPermutationOf_ReturnsMinusOne_IfNotFound_1()
        {
            var index = "abc".AsList().IndexOfPermutationOf("d".AsList(), EqualityComparer<char>.Default);

            Assert.AreEqual(-1, index);
        }

        [Test]
        public void IndexOfPermutationOf_ReturnsMinusOne_IfNotFound_2()
        {
            var index = "abc".AsList().IndexOfPermutationOf("abd".AsList(), EqualityComparer<char>.Default);

            Assert.AreEqual(-1, index);
        }

        [Test, SuppressMessage("ReSharper", "StringLiteralTypo")]
        public void IndexOfPermutationOf_ReturnsMinusOne_IsSubsequenceLongerThanSequence()
        {
            var index = "abc".AsList().IndexOfPermutationOf("abcd".AsList(), EqualityComparer<char>.Default);

            Assert.AreEqual(-1, index);
        }

        [Test]
        public void IndexOfPermutationOf_ReturnsZero_ForEmptySubsequence()
        {
            var index = "a".AsList().IndexOfPermutationOf("".AsList(), EqualityComparer<char>.Default);

            Assert.AreEqual(0, index);
        }

        [Test]
        public void IndexOfPermutationOf_TakesComparerIntoConsideration()
        {
            var index = new[] {"a", "b"}.IndexOfPermutationOf(new[] {"B"}, StringComparer.OrdinalIgnoreCase);

            Assert.AreEqual(1, index);
        }

        [Test, SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void IndexOfPermutationOf_ThrowsException_IfComparerIsNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
                "a".AsList().IndexOfPermutationOf("a".AsList(), null));
        }

        [Test, SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void IndexOfPermutationOf_ThrowsException_IfSequenceIsNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
                ((char[]) null).IndexOfPermutationOf(null, EqualityComparer<char>.Default));
        }

        [Test, SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void IndexOfPermutationOf_ThrowsException_IfSubsequenceIsNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
                "a".AsList().IndexOfPermutationOf(null, EqualityComparer<char>.Default));
        }
    }
}