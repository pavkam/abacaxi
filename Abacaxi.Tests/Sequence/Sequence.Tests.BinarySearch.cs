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

namespace Abacaxi.Tests
{
    using System;
    using System.Collections.Generic;
    using NUnit.Framework;

    [TestFixture]
    public class SequenceBinarySearchTests
    {
        [Test]
        public void Search_ThrowsException_ForNullArray()
        {
            Assert.Throws<ArgumentNullException>(() =>
                Abacaxi.Sequence.BinarySearch((int[])null, 1, 1, 0, Comparer<int>.Default));
        }
        
        [Test]
        public void Search_ThrowsException_ForNegativeStartIndex()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                Abacaxi.Sequence.BinarySearch(new[] { 1 }, -1, 1, 0, Comparer<int>.Default));
        }

        [Test]
        public void Search_ThrowsException_ForNegativeLength()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                Abacaxi.Sequence.BinarySearch(new[] { 1 }, 0, -1, 0, Comparer<int>.Default));
        }

        [Test]
        public void Search_ThrowsException_ForOutOfBounds1()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                Abacaxi.Sequence.BinarySearch(new[] { 1 }, 0, 2, 0, Comparer<int>.Default));
        }

        [Test]
        public void Search_ThrowsException_ForOutOfBounds2()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                Abacaxi.Sequence.BinarySearch(new[] { 1 }, 1, 1, 0, Comparer<int>.Default));
        }

        [Test]
        public void Search_ThrowsException_ForNullComparer()
        {
            Assert.Throws<ArgumentNullException>(() =>
                Abacaxi.Sequence.BinarySearch(new[] { 1 }, 0, 1, 0, null));
        }

        [Test]
        public void Search_ReturnsMinusOne_ForEmptyArray()
        {
            var result = Abacaxi.Sequence.BinarySearch(new int[] { }, 0, 0, 0, Comparer<int>.Default);
            Assert.AreEqual(-1, result);
        }

        [Test]
        public void Search_ReturnsMinusOne_ForZeroLength()
        {
            var result = Abacaxi.Sequence.BinarySearch(new int[] { 1 }, 0, 0, 0, Comparer<int>.Default);
            Assert.AreEqual(-1, result);
        }

        [Test]
        public void Search_ReturnsMinusOne_WhenNotFound()
        {
            var result = Abacaxi.Sequence.BinarySearch(new int[] { 1, 2, 3 }, 0, 3, 0, Comparer<int>.Default);
            Assert.AreEqual(-1, result);
        }

        [Test]
        public void Search_ReturnsIndex_ForOneElementArray()
        {
            var result = Abacaxi.Sequence.BinarySearch(new int[] { 1 }, 0, 1, 1, Comparer<int>.Default);
            Assert.AreEqual(0, result);
        }

        [Test]
        public void Search_ReturnsZeroIndex_ForTwoElementArray()
        {
            var result = Abacaxi.Sequence.BinarySearch(new int[] { 1, 2 }, 0, 2, 1, Comparer<int>.Default);
            Assert.AreEqual(0, result);
        }

        [Test]
        public void Search_ReturnsOneIndex_ForTwoElementArray()
        {
            var result = Abacaxi.Sequence.BinarySearch(new int[] { 1, 2 }, 0, 2, 2, Comparer<int>.Default);
            Assert.AreEqual(1, result);
        }

        [TestCase(0, -1)]
        [TestCase(1, 0)]
        [TestCase(2, 1)]
        [TestCase(3, 2)]
        [TestCase(4, 3)]
        [TestCase(5, 4)]
        [TestCase(6, -1)]
        public void Search_ReturnsValidIndex_ForAscSortedArray(int element, int expectedIndex)
        {
            var result = Abacaxi.Sequence.BinarySearch(new int[] { 1, 2, 3, 4, 5 }, 0, 5, element, Comparer<int>.Default);
            Assert.AreEqual(expectedIndex, result);
        }

        [TestCase(0, -1)]
        [TestCase(1, 4)]
        [TestCase(2, 3)]
        [TestCase(3, 2)]
        [TestCase(4, 1)]
        [TestCase(5, 0)]
        [TestCase(6, -1)]
        public void Search_ReturnsValidIndex_ForDescSortedArray(int element, int expectedIndex)
        {
            var result = Abacaxi.Sequence.BinarySearch(new int[] { 5, 4, 3, 2, 1 }, 0, 5, element, Comparer<int>.Default, false);
            Assert.AreEqual(expectedIndex, result);
        }

        [Test]
        public void Search_ReturnsMinusOne_ForIncorrectlySortedArray()
        {
            var result = Abacaxi.Sequence.BinarySearch(new int[] { 5, 4, 3, 2, 1 }, 0, 5, 5, Comparer<int>.Default, true);
            Assert.AreEqual(-1, result);
        }

        [Test]
        public void Search_UsesTheComparer()
        {
            var result = Abacaxi.Sequence.BinarySearch(new[] { "A" }, 0, 1, "a", StringComparer.OrdinalIgnoreCase, true);
            Assert.AreEqual(0, result);
        }
    }
}
