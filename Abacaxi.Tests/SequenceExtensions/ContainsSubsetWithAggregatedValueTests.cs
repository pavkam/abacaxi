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

// ReSharper disable ReturnValueOfPureMethodIsNotUsed

namespace Abacaxi.Tests.SequenceExtensions
{
    using System;
    using NUnit.Framework;

    [TestFixture]
    public class ContainsSubsetWithAggregatedValueTests
    {
        [Test]
        public void ContainsSubsequenceWithAggregatedValue_ThowsException_ForNullSequence()
        {
            Assert.Throws<ArgumentNullException>(() =>
                ((int[])null).ContainsSubsetWithAggregatedValue(1));
        }

        [Test]
        public void ContainsSubsequenceWithAggregatedValue_ThowsException_ForZeroTargedSum()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                new int[] { }.ContainsSubsetWithAggregatedValue(0));
        }

        [Test]
        public void ContainsSubsequenceWithAggregatedValue_ThowsException_ForNegativeNumberInSequence()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                new[] { -1 }.ContainsSubsetWithAggregatedValue(1));
        }

        [Test]
        public void ContainsSubsequenceWithAggregatedValue_ReturnsFalse_WhenSumCannotBeCompleted()
        {
            var result = new[] { 2 }.ContainsSubsetWithAggregatedValue(1);
            Assert.IsFalse(result);
        }

        [Test]
        public void ContainsSubsequenceWithAggregatedValue_ReturnsFalse_ForEmptyArray()
        {
            var result = new int[] { }.ContainsSubsetWithAggregatedValue(1);
            Assert.IsFalse(result);
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        [TestCase(6)]
        [TestCase(7)]
        [TestCase(8)]
        [TestCase(9)]
        [TestCase(10)]
        [TestCase(11)]
        [TestCase(12)]
        [TestCase(13)]
        [TestCase(14)]
        [TestCase(15)]
        public void ContainsSubsequenceWithAggregatedValue_ReturnsTrue_IfSumFound(int target)
        {
            var result = new[] { 1, 2, 3, 4, 5 }.ContainsSubsetWithAggregatedValue(target);
            Assert.IsTrue(result);
        }

        [TestCase(2)]
        [TestCase(14)]
        [TestCase(17)]
        public void ContainsSubsequenceWithAggregatedValue_ReturnsFalse_IfSumNotFound(int target)
        {
            var result = new[] { 1, 3, 5, 7 }.ContainsSubsetWithAggregatedValue(target);
            Assert.IsFalse(result);
        }
    }
}
