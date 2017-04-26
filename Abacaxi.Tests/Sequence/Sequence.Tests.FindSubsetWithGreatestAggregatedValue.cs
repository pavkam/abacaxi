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

namespace Abacaxi.Tests.Sequence
{
    using System;
    using System.Collections.Generic;
    using NUnit.Framework;

    [TestFixture]
    public class SequenceFindSubsetWithGreatestAggregatedValueTests
    {
        private static int IntegerAggregator(int a, int b)
        {
            return a + b;
        }

        [Test]
        public void FindBiggestSumOfNumberOfElements_ThrowsException_IfSequenceIsNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
                ((int[])null).FindSubsetWithGreatestAggregatedValue(1, IntegerAggregator, Comparer<int>.Default));
        }

        [Test]
        public void FindBiggestSumOfNumberOfElements_ThrowsException_IfCountIsLessThanOne()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                new int[] { }.FindSubsetWithGreatestAggregatedValue(0, IntegerAggregator, Comparer<int>.Default));
        }

        [Test]
        public void FindBiggestSumOfNumberOfElements_ThrowsException_IfAggregatorIsNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new[] { 1 }.FindSubsetWithGreatestAggregatedValue(1, null, Comparer<int>.Default));
        }

        [Test]
        public void FindBiggestSumOfNumberOfElements_ThrowsException_IfComparerIsNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new[] { 1 }.FindSubsetWithGreatestAggregatedValue(1, IntegerAggregator, null));
        }

        [Test]
        public void FindBiggestSumOfNumberOfElements_ReturnsBiggestElement_ForCountOfOne()
        {
            TestHelper.AssertSequence(
                new[] { 1, 2, 3 }.FindSubsetWithGreatestAggregatedValue(1, IntegerAggregator, Comparer<int>.Default),
                3);
        }

        [Test]
        public void FindBiggestSumOfNumberOfElements_ReturnsAllElements_ForCountOfThree()
        {
            TestHelper.AssertSequence(
                new[] { 3, 2, 1 }.FindSubsetWithGreatestAggregatedValue(3, IntegerAggregator, Comparer<int>.Default),
                1, 2, 3);
        }

        [Test]
        public void FindBiggestSumOfNumberOfElements_ReturnsLastThreeElements()
        {
            TestHelper.AssertSequence(
                new[] { 100, 200, 1, 50, 70, 188 }.FindSubsetWithGreatestAggregatedValue(3, IntegerAggregator, Comparer<int>.Default),
                100, 188, 200);
        }
    }
}
