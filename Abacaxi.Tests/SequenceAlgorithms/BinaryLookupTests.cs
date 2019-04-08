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

namespace Abacaxi.Tests.SequenceAlgorithms
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using NUnit.Framework;

    [TestFixture]
    public sealed class BinaryLookupTests
    {
        [TestCase(0, -1, -1), TestCase(1, 0, 0), TestCase(2, 1, 1), TestCase(3, 2, 2), TestCase(4, 3, 3),
         TestCase(5, 4, 4),
         TestCase(6, 4, 4)]
        public void BinaryLookup_ReturnsValidIndexPairs_ForAscSortedArray(int element, int expectedLeft,
            int expectedRight)
        {
            var result = new[] {1, 2, 3, 4, 5}.BinaryLookup(0, 5, element, Comparer<int>.Default);

            Assert.AreEqual((expectedLeft, expectedRight), result);
        }

        [TestCase(-1, -1, -1), TestCase(2, 0, 0), TestCase(4, 0, 0), TestCase(6, 1, 1), TestCase(14, 2, 2),
         TestCase(15, 3, 3), TestCase(17, 3, 3),
         TestCase(21, 4, 4), TestCase(1000, 4, 4)]
        public void BinaryLookup_ReturnsValidIndexPairs_ForAscSortedArray_AndMissingElements(int element,
            int expectedLeft, int expectedRight)
        {
            var result = new[] {1, 5, 10, 15, 20}.BinaryLookup(0, 5, element, Comparer<int>.Default);

            Assert.AreEqual((expectedLeft, expectedRight), result);
        }

        [TestCase(0, 4, 4), TestCase(1, 4, 4), TestCase(2, 3, 3), TestCase(3, 2, 2), TestCase(4, 1, 1),
         TestCase(5, 0, 0),
         TestCase(6, -1, -1)]
        public void BinaryLookup_ReturnsValidIndexPairs_ForDescSortedArray(int element, int expectedLeft,
            int expectedRight)
        {
            var result = new[] {5, 4, 3, 2, 1}.BinaryLookup(0, 5, element, Comparer<int>.Default, false);
            Assert.AreEqual((expectedLeft, expectedRight), result);
        }

        [Test]
        public void BinaryLookup_ReturnsIndex_ForOneElementArray()
        {
            var result = new[] {1}.BinaryLookup(0, 1, 1, Comparer<int>.Default);
            Assert.AreEqual((0, 0), result);
        }

        [Test]
        public void BinaryLookup_ReturnsIndexes_ForTwoElementArray()
        {
            var result = new[] {1, 1}.BinaryLookup(0, 2, 1, Comparer<int>.Default);
            Assert.AreEqual((0, 1), result);
        }

        [Test]
        public void BinaryLookup_ReturnsIndexes_WhenInTheMiddle_Even()
        {
            var result = new[] {0, 1, 2, 2, 2, 4}.BinaryLookup(0, 6, 2, Comparer<int>.Default);
            Assert.AreEqual((2, 4), result);
        }

        [Test]
        public void BinaryLookup_ReturnsIndexes_WhenInTheMiddle_Odd()
        {
            var result = new[] {0, 1, 2, 2, 2, 3, 4}.BinaryLookup(0, 7, 2, Comparer<int>.Default);
            Assert.AreEqual((2, 4), result);
        }

        [Test]
        public void BinaryLookup_ReturnsMinusOnePair_ForEmptyArray()
        {
            var result = new int[] { }.BinaryLookup(0, 0, 0, Comparer<int>.Default);
            Assert.AreEqual((-1, -1), result);
        }

        [Test]
        public void BinaryLookup_ReturnsMinusOnePair_ForZeroLength()
        {
            var result = new[] {1}.BinaryLookup(0, 0, 0, Comparer<int>.Default);
            Assert.AreEqual((-1, -1), result);
        }

        [Test]
        public void BinaryLookup_ReturnsMinusOnePair_WhenNotFoundLess()
        {
            var result = new[] {1, 2, 3}.BinaryLookup(0, 3, 0, Comparer<int>.Default);
            Assert.AreEqual((-1, -1), result);
        }

        [Test]
        public void BinaryLookup_ReturnsOne_WhenNotFoundMid()
        {
            var result = new[] {1, 4, 6}.BinaryLookup(0, 3, 2, Comparer<int>.Default);
            Assert.AreEqual((0, 0), result);
        }

        [Test]
        public void BinaryLookup_ReturnsOneIndexPair_ForTwoElementArray()
        {
            var result = new[] {1, 2}.BinaryLookup(0, 2, 2, Comparer<int>.Default);
            Assert.AreEqual((1, 1), result);
        }

        [Test]
        public void BinaryLookup_ReturnsTwo_WhenNotFoundUpper()
        {
            var result = new[] {1, 4, 6}.BinaryLookup(0, 3, 7, Comparer<int>.Default);
            Assert.AreEqual((2, 2), result);
        }

        [Test]
        public void BinaryLookup_ReturnsZeroIndexPair_ForTwoElementArray()
        {
            var result = new[] {1, 2}.BinaryLookup(0, 2, 1, Comparer<int>.Default);

            Assert.AreEqual((0, 0), result);
        }

        [Test]
        public void BinaryLookup_ThrowsException_ForNegativeLength()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                new[] {1}.BinaryLookup(0, -1, 0, Comparer<int>.Default));
        }

        [Test]
        public void BinaryLookup_ThrowsException_ForNegativeStartIndex()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                new[] {1}.BinaryLookup(-1, 1, 0, Comparer<int>.Default));
        }

        [Test, SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void BinaryLookup_ThrowsException_ForNullArray()
        {
            Assert.Throws<ArgumentNullException>(() =>
                ((int[]) null).BinaryLookup(1, 1, 0, Comparer<int>.Default));
        }

        [Test, SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void BinaryLookup_ThrowsException_ForNullComparer()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new[] {1}.BinaryLookup(0, 1, 0, null));
        }

        [Test]
        public void BinaryLookup_ThrowsException_ForOutOfBounds1()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                new[] {1}.BinaryLookup(0, 2, 0, Comparer<int>.Default));
        }

        [Test]
        public void BinaryLookup_ThrowsException_ForOutOfBounds2()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                new[] {1}.BinaryLookup(1, 1, 0, Comparer<int>.Default));
        }

        [Test]
        public void BinaryLookup_UsesTheComparer()
        {
            var result = new[] {"A"}.BinaryLookup(0, 1, "a", StringComparer.OrdinalIgnoreCase);
            Assert.AreEqual((0, 0), result);
        }
    }
}