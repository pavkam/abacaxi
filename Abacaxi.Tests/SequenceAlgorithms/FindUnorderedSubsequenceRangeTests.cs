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
    using NUnit.Framework;

    [TestFixture]
    public sealed class FindUnorderedSubsequenceRangeTests
    {
        [Test]
        public void FindUnorderedSubsequence_ReturnsZeroTuple_ForEmptySequence()
        {
            var result = new int[] { }.FindUnorderedSubsequenceRange(Comparer<int>.Default);

            Assert.AreEqual((0, 0), result);
        }

        [Test]
        public void FindUnorderedSubsequence_ReturnsZeroTuple_ForOneElementSequence()
        {
            var result = new[] {1}.FindUnorderedSubsequenceRange(Comparer<int>.Default);

            Assert.AreEqual((0, 0), result);
        }

        [Test]
        public void FindUnorderedSubsequence_ReturnsZeroTuple_ForOrderedSequence_1()
        {
            var result = new[] {1, 2}.FindUnorderedSubsequenceRange(Comparer<int>.Default);

            Assert.AreEqual((0, 0), result);
        }

        [Test]
        public void FindUnorderedSubsequence_ReturnsZeroTuple_ForOrderedSequence_2()
        {
            var result = new[] {1, 2, 3}.FindUnorderedSubsequenceRange(Comparer<int>.Default);

            Assert.AreEqual((0, 0), result);
        }

        [Test]
        public void FindUnorderedSubsequence_ReturnsZeroTuple_ForOrderedSequence_3()
        {
            var result = new[] {1, 2, 3, 10, 20, 30}.FindUnorderedSubsequenceRange(Comparer<int>.Default);

            Assert.AreEqual((0, 0), result);
        }

        [Test]
        public void FindUnorderedSubsequence_ReturnsWholeSequence_1()
        {
            var result = new[] {10, 1}.FindUnorderedSubsequenceRange(
                Comparer<int>.Default);

            Assert.AreEqual((0, 2), result);
        }

        [Test]
        public void FindUnorderedSubsequence_ReturnsWholeSequence_2()
        {
            var result = new[] {4, 3, 2}.FindUnorderedSubsequenceRange(
                Comparer<int>.Default);

            Assert.AreEqual((0, 3), result);
        }

        [Test]
        public void FindUnorderedSubsequence_ReturnsWholeSequence_3()
        {
            var result = new[] {1, 4, 3, 2}.FindUnorderedSubsequenceRange(
                Comparer<int>.Default);

            Assert.AreEqual((1, 3), result);
        }

        [Test]
        public void FindUnorderedSubsequence_ReturnsWholeSequence_4()
        {
            var result = new[] {1, 2, 4, 3}.FindUnorderedSubsequenceRange(
                Comparer<int>.Default);

            Assert.AreEqual((2, 2), result);
        }

        [Test]
        public void FindUnorderedSubsequence_ReturnsWholeSequence_5()
        {
            var result = new[] {1, 3, 2, 4}.FindUnorderedSubsequenceRange(
                Comparer<int>.Default);

            Assert.AreEqual((1, 2), result);
        }

        [Test]
        public void FindUnorderedSubsequence_ReturnsWholeSequence_6()
        {
            var result = new[] {1, 6, 3, 4, 5, 2, 7}.FindUnorderedSubsequenceRange(
                Comparer<int>.Default);

            Assert.AreEqual((1, 5), result);
        }

        [Test]
        public void FindUnorderedSubsequence_ReturnsWholeSequence_7()
        {
            var result = new[] {1, 2, 3, 4, 5, 0, 0}.FindUnorderedSubsequenceRange(
                Comparer<int>.Default);

            Assert.AreEqual((0, 7), result);
        }

        [Test]
        public void FindUnorderedSubsequence_TakesComparerIntoAccount()
        {
            var result = new[] {1, 2, 3, 4, 5}.FindUnorderedSubsequenceRange(
                Comparer<int>.Create((l, r) => r - l));

            Assert.AreEqual((0, 5), result);
        }

        [Test, SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void FindUnorderedSubsequence_ThrowsException_ForNullComparer()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new[] {1}.FindUnorderedSubsequenceRange(null));
        }

        [Test, SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void FindUnorderedSubsequence_ThrowsException_ForNullSequence()
        {
            Assert.Throws<ArgumentNullException>(() =>
                ((int[]) null).FindUnorderedSubsequenceRange(Comparer<int>.Default));
        }
    }
}