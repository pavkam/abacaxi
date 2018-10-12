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

namespace Abacaxi.Tests.SequenceExtensions
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using NUnit.Framework;

    [TestFixture]
    public sealed class BinarySearchTests
    {
        [TestCase(0, -1), TestCase(1, 0), TestCase(2, 1), TestCase(3, 2), TestCase(4, 3), TestCase(5, 4),
         TestCase(6, -1)]
        public void BinarySearch_ReturnsValidIndex_ForAscSortedArray(int element, int expectedIndex)
        {
            var result = new[] {1, 2, 3, 4, 5}.BinarySearch(0, 5, element, Comparer<int>.Default);
            Assert.AreEqual(expectedIndex, result);
        }

        [TestCase(0, -1), TestCase(1, 4), TestCase(2, 3), TestCase(3, 2), TestCase(4, 1), TestCase(5, 0),
         TestCase(6, -1)]
        public void BinarySearch_ReturnsValidIndex_ForDescSortedArray(int element, int expectedIndex)
        {
            var result = new[] {5, 4, 3, 2, 1}.BinarySearch(0, 5, element, Comparer<int>.Default, false);
            Assert.AreEqual(expectedIndex, result);
        }

        [Test]
        public void BinarySearch_ReturnsIndex_ForOneElementArray()
        {
            var result = new[] {1}.BinarySearch(0, 1, 1, Comparer<int>.Default);
            Assert.AreEqual(0, result);
        }

        [Test]
        public void BinarySearch_ReturnsMinusOne_ForEmptyArray()
        {
            var result = new int[] { }.BinarySearch(0, 0, 0, Comparer<int>.Default);
            Assert.AreEqual(-1, result);
        }

        [Test]
        public void BinarySearch_ReturnsMinusOne_ForIncorrectlySortedArray()
        {
            var result = new[] {5, 4, 3, 2, 1}.BinarySearch(0, 5, 5, Comparer<int>.Default);
            Assert.AreEqual(-1, result);
        }

        [Test]
        public void BinarySearch_ReturnsMinusOne_ForZeroLength()
        {
            var result = new[] {1}.BinarySearch(0, 0, 0, Comparer<int>.Default);
            Assert.AreEqual(-1, result);
        }

        [Test]
        public void BinarySearch_ReturnsMinusOne_WhenNotFound()
        {
            var result = new[] {1, 2, 3}.BinarySearch(0, 3, 0, Comparer<int>.Default);
            Assert.AreEqual(-1, result);
        }

        [Test]
        public void BinarySearch_ReturnsOneIndex_ForTwoElementArray()
        {
            var result = new[] {1, 2}.BinarySearch(0, 2, 2, Comparer<int>.Default);
            Assert.AreEqual(1, result);
        }

        [Test]
        public void BinarySearch_ReturnsZeroIndex_ForTwoElementArray()
        {
            var result = new[] {1, 2}.BinarySearch(0, 2, 1, Comparer<int>.Default);
            Assert.AreEqual(0, result);
        }

        [Test]
        public void BinarySearch_ThrowsException_ForNegativeLength()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                new[] {1}.BinarySearch(0, -1, 0, Comparer<int>.Default));
        }

        [Test]
        public void BinarySearch_ThrowsException_ForNegativeStartIndex()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                new[] {1}.BinarySearch(-1, 1, 0, Comparer<int>.Default));
        }

        [Test, SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void BinarySearch_ThrowsException_ForNullArray()
        {
            Assert.Throws<ArgumentNullException>(() =>
                ((int[]) null).BinarySearch(1, 1, 0, Comparer<int>.Default));
        }

        [Test, SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void BinarySearch_ThrowsException_ForNullComparer()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new[] {1}.BinarySearch(0, 1, 0, null));
        }

        [Test]
        public void BinarySearch_ThrowsException_ForOutOfBounds1()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                new[] {1}.BinarySearch(0, 2, 0, Comparer<int>.Default));
        }

        [Test]
        public void BinarySearch_ThrowsException_ForOutOfBounds2()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                new[] {1}.BinarySearch(1, 1, 0, Comparer<int>.Default));
        }

        [Test]
        public void BinarySearch_UsesTheComparer()
        {
            var result = new[] {"A"}.BinarySearch(0, 1, "a", StringComparer.OrdinalIgnoreCase);
            Assert.AreEqual(0, result);
        }
    }
}