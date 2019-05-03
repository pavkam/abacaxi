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
    using JetBrains.Annotations;
    using NUnit.Framework;
    using SequenceAlgorithms = Abacaxi.SequenceAlgorithms;

    [TestFixture]
    public sealed class GetRangeWithGreatestAggregateValueTests
    {
        private static int IntegerAggregator(int a, int b)
        {
            return a + b;
        }

        [CanBeNull]
        private static (int index, int length)? Do([NotNull] params int[] s)
        {
            return s.GetRangeWithGreatestAggregateValue(IntegerAggregator, Comparer<int>.Default);
        }

        [Test]
        public void GetRangeWithGreatestAggregateValue_AvoidsStartingNegative()
        {
            var r = Do(-1, 2, 3, 4);
            Assert.AreEqual((1, 3), r);
        }

        [Test]
        public void GetRangeWithGreatestAggregateValue_ReturnsFullSequenceOfPositives()
        {
            var r = Do(1, 2, 3, 4);
            Assert.AreEqual((0, 4), r);
        }

        [Test]
        public void GetRangeWithGreatestAggregateValue_ReturnsNullIfSequenceIsEmpty()
        {
            var r = Do();
            Assert.IsNull(r);
        }

        [Test]
        public void GetRangeWithGreatestAggregateValue_ReturnsSingleElement_IfNegative()
        {
            var r = Do(-10);
            Assert.AreEqual((0, 1), r);
        }

        [Test]
        public void GetRangeWithGreatestAggregateValue_ReturnsSingleElement_IfPositive()
        {
            var r = Do(10);
            Assert.AreEqual((0, 1), r);
        }

        [Test]
        public void GetRangeWithGreatestAggregateValue_ReturnsSmallestOfNegatives()
        {
            var r = Do(-10, -9, -6, -5, -1, -1, -2);
            Assert.AreEqual((4, 1), r);
        }

        [Test]
        public void GetRangeWithGreatestAggregateValue_SelectsMiddleBecauseItsBetter()
        {
            var r = Do(2, -8, 3, -1, 2, -1, 2, -5, 4);
            Assert.AreEqual((2, 5), r);
        }

        [Test]
        public void GetRangeWithGreatestAggregateValue_SelectsTheOnlyPositiveInAllNegatives()
        {
            var r = Do(-1, -1, -1, 1, -1, -1);
            Assert.AreEqual((3, 1), r);
        }

        [Test]
        public void GetRangeWithGreatestAggregateValue_TakesInNegativesToMakeALongerSequence()
        {
            var r = Do(5, -2, -3, 10);
            Assert.AreEqual((0, 4), r);
        }

        [Test, SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void GetRangeWithGreatestAggregateValue_ThrowsException_IfAggregatorIsNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new int[] { }.GetRangeWithGreatestAggregateValue(null, Comparer<int>.Default));
        }

        [Test, SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void GetRangeWithGreatestAggregateValue_ThrowsException_IfComparerIsNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new int[] { }.GetRangeWithGreatestAggregateValue(IntegerAggregator, null));
        }

        [Test, SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void GetRangeWithGreatestAggregateValue_ThrowsException_IfSequenceIsNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
                SequenceAlgorithms.GetRangeWithGreatestAggregateValue(null, IntegerAggregator, Comparer<int>.Default));
        }
    }
}