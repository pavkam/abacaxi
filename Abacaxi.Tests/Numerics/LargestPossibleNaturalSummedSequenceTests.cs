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

namespace Abacaxi.Tests.Numerics
{
    using System;
    using System.Linq;
    using Abacaxi.Numerics;
    using NUnit.Framework;

    [TestFixture]
    public class LargestPossibleNaturalSummedSequenceTests
    {
        [Test]
        public void Find_ThowsException_ForNullSequence()
        {
            Assert.Throws<ArgumentNullException>(() =>
                LargestPossibleNaturalSummedSequence.Find(null, 1).ToArray());
        }

        [Test]
        public void Find_ThowsException_ForZeroTargedSum()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                LargestPossibleNaturalSummedSequence.Find(new int[] { }, 0).ToArray());
        }

        [Test]
        public void Find_ThowsException_ForNegativeNumberInSequence()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                LargestPossibleNaturalSummedSequence.Find(new int[] { -1 }, 1).ToArray());
        }

        [Test]
        public void Find_ReturnsNothing_WhenSumCannotBeCompleted()
        {
            var array = new[] { 2, 3, 4 };
            TestHelper.AssertSequence(
                LargestPossibleNaturalSummedSequence.Find(array, 1));
        }

        [Test]
        public void Find_ReturnsNothing_ForEmptyArray()
        {
            var array = new int[] { };
            TestHelper.AssertSequence(
                LargestPossibleNaturalSummedSequence.Find(array, 1));
        }

        [Test]
        public void Find_ReturnsOne_ForPerfectMatchingSumOfOne()
        {
            var array = new[] { 1 };
            TestHelper.AssertSequence(
                LargestPossibleNaturalSummedSequence.Find(array, 1),
                1);
        }

        [Test]
        public void Find_ReturnsOne_WhenSkippingZeroes()
        {
            var array = new[] { 0, 0, 1 };
            TestHelper.AssertSequence(
                LargestPossibleNaturalSummedSequence.Find(array, 1),
                1);
        }

        [Test]
        public void Find_ReturnsTwo_ForPerfectFullyCompletedSumOfTwo()
        {
            var array = new[] { 1, 2 };
            TestHelper.AssertSequence(
                LargestPossibleNaturalSummedSequence.Find(array, 2),
                2);
        }

        [Test]
        public void Find_ReturnsOneTwoThree_ForASumOfTen()
        {
            var array = new[] { 1, 2, 3 };
            TestHelper.AssertSequence(
                LargestPossibleNaturalSummedSequence.Find(array, 10),
                3, 2, 1);
        }

        [Test]
        public void Find_ReturnsOneAndTwo_ForASumOfThree()
        {
            var array = new[] { 1, 2, 3, 4, 5 };
            TestHelper.AssertSequence(
                LargestPossibleNaturalSummedSequence.Find(array, 3),
                2, 1);
        }

        [Test]
        public void Find_ReturnsOneTwoThree_ForASumOfSeven_ExcludingTen()
        {
            var array = new[] { 1, 2, 3, 10 };
            TestHelper.AssertSequence(
                LargestPossibleNaturalSummedSequence.Find(array, 7),
                3, 2, 1);
        }

        [Test]
        public void Find_ReturnsTen_ForASumOfEleven_ExcludingFive()
        {
            var array = new[] { 5, 10 };
            TestHelper.AssertSequence(
                LargestPossibleNaturalSummedSequence.Find(array, 11),
                10);
        }

        [Test]
        public void Find_ReturnsTenOne_ForASumOfEleven_ExcludingOneTwoThree()
        {
            var array = new[] { 1, 2, 3, 10 };
            TestHelper.AssertSequence(
                LargestPossibleNaturalSummedSequence.Find(array, 11),
                10, 1);
        }
    }
}
