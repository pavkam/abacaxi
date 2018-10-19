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
    using System.Linq;
    using NUnit.Framework;

    [TestFixture]
    public sealed class IsPermutationOfPalindromeTests
    {
        [TestCase(1, 0, 2), TestCase(1, 2, 0), TestCase(1, -1, 0), TestCase(1, 0, -1), TestCase(2, 1, 2)]
        public void IsPermutationOfPalindrome_ThrowsException_ForInvalidIndexes(int actualCount, int startIndex, int count)
        {
            var list = Enumerable.Range(0, actualCount).ToList();

            Assert.Throws<ArgumentOutOfRangeException>(() =>
                list.IsPermutationOfPalindrome(startIndex, count, EqualityComparer<int>.Default));
        }

        [TestCase(0, 0, 0), TestCase(1, 0, 0), TestCase(1, 1, 0)]
        public void IsPermutationOfPalindrome_ReturnsTrue_ForEmptySequenceCombos(int actualCount, int startIndex, int count)
        {
            var list = Enumerable.Range(0, actualCount).ToList();
            var result = list.IsPermutationOfPalindrome(startIndex, count, EqualityComparer<int>.Default);

            Assert.IsTrue(result);
        }

        [TestCase(1, true), TestCase(2, false), TestCase(3, false), TestCase(4, false), TestCase(5, true), TestCase(6, true), TestCase(7, true)]
        public void IsPermutationOfPalindrome_ReturnsTrue_ForLongExample(int length, bool expected)
        {
            var list = new[] {'1', '2', '3', '1', '2', '3', '4'};
            var result = list.IsPermutationOfPalindrome(0, length, EqualityComparer<char>.Default);

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void IsPermutationOfPalindrome_ReturnsFalse_ForThreeElementSequence()
        {
            var list = new[] {'a', 'b', 'c'};
            var result = list.IsPermutationOfPalindrome(0, 3, EqualityComparer<char>.Default);

            Assert.IsFalse(result);
        }

        [Test]
        public void IsPermutationOfPalindrome_ReturnsFalse_ForTwoElementSequence()
        {
            var list = new[] {'a', 'b', 'c'};
            var result = list.IsPermutationOfPalindrome(0, 2, EqualityComparer<char>.Default);

            Assert.IsFalse(result);
        }

        [Test]
        public void IsPermutationOfPalindrome_ReturnsTrue_ForNormalPalindrome()
        {
            var list = new[] {'a', 'b', 'c', 'z', 'c', 'b', 'a'};
            var result = list.IsPermutationOfPalindrome(0, list.Length, EqualityComparer<char>.Default);

            Assert.IsTrue(result);
        }

        [Test]
        public void IsPermutationOfPalindrome_ReturnsTrue_ForOneElement()
        {
            var list = new[] {'a', 'b', 'c'};
            var result = list.IsPermutationOfPalindrome(1, 1, EqualityComparer<char>.Default);

            Assert.IsTrue(result);
        }

        [Test]
        public void IsPermutationOfPalindrome_ReturnsTrue_ForThreeElementPalindrome()
        {
            var list = new[] {'a', 'b', 'a'};
            var result = list.IsPermutationOfPalindrome(0, 3, EqualityComparer<char>.Default);

            Assert.IsTrue(result);
        }

        [Test]
        public void IsPermutationOfPalindrome_ReturnsTrue_ForTwoEqualElements()
        {
            var list = new[] {'a', 'b', 'b'};
            var result = list.IsPermutationOfPalindrome(1, 2, EqualityComparer<char>.Default);

            Assert.IsTrue(result);
        }

        [Test]
        public void IsPermutationOfPalindrome_TakesComparerIntoConsideration()
        {
            var list = new[] {"a", "A"};
            var result = list.IsPermutationOfPalindrome(0, list.Length, StringComparer.OrdinalIgnoreCase);

            Assert.IsTrue(result);
        }

        [Test, SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void IsPermutationOfPalindrome_ThrowsException_ForNullComparer()
        {
            Assert.Throws<ArgumentNullException>(() =>
                ((int[]) null).IsPermutationOfPalindrome(0, 1, EqualityComparer<int>.Default));
        }

        [Test, SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void IsPermutationOfPalindrome_ThrowsException_ForNullSequence()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new[] {'a'}.IsPermutationOfPalindrome(0, 1, null));
        }
    }
}