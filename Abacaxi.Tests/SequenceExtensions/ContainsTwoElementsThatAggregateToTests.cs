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

namespace Abacaxi.Tests.SequenceExtensions
{
    using System;
    using System.Collections.Generic;
    using NUnit.Framework;

    [TestFixture]
    public class ContainsTwoElementsThatAggregateToTests
    {
        private static int IntegerAggregator(int a, int b)
        {
            return a + b;
        }

        [Test]
        public void ContainsTwoElementsThatAggregateTo_ThrowsException_IfSequenceIsNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
                ((int[])null).ContainsTwoElementsThatAggregateTo(IntegerAggregator, Comparer<int>.Default, 1));
        }

        [Test]
        public void ContainsTwoElementsThatAggregateTo_ThrowsException_IfAggregatorIsNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new[] { 1 }.ContainsTwoElementsThatAggregateTo(null, Comparer<int>.Default, 1));
        }

        [Test]
        public void ContainsTwoElementsThatAggregateTo_ThrowsException_IfComparerIsNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new[] { 1 }.ContainsTwoElementsThatAggregateTo(IntegerAggregator, null, 1));
        }

        [Test]
        public void ContainsTwoElementsThatAggregateTo_ReturnsFalse_ForEmptySequence()
        {
            var result = new int[] { }.ContainsTwoElementsThatAggregateTo(IntegerAggregator, Comparer<int>.Default, 3);

            Assert.IsFalse(result);
        }

        [Test]
        public void ContainsTwoElementsThatAggregateTo_ReturnsTrue_ForTwoElements()
        {
            var result = new[] { 1, 0 }.ContainsTwoElementsThatAggregateTo(IntegerAggregator, Comparer<int>.Default, 1);

            Assert.IsTrue(result);
        }

        [Test]
        public void ContainsTwoElementsThatAggregateTo_ReturnsFalse_IsSumImpossible_ForFiveElements()
        {
            var result = new[] { 1, 2, 3, 4, 5 }.ContainsTwoElementsThatAggregateTo(IntegerAggregator, Comparer<int>.Default, 10);

            Assert.IsFalse(result);
        }

        [Test]
        public void ContainsTwoElementsThatAggregateTo_ReturnsTrue_ForLongSequence()
        {
            var result = new[] { 1, 10, 2, 8, 2, 2, 3, 4, 19, 6 }.ContainsTwoElementsThatAggregateTo(IntegerAggregator, Comparer<int>.Default, 29);

            Assert.IsTrue(result);
        }
    }
}
