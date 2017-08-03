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

namespace Abacaxi.Tests.Integer
{
    using NUnit.Framework;
    using System.Linq;

    [TestFixture]
    public class DeconstructIntoPowersOfTwoTests
    {
        [Test]
        public void DeconstructIntoPowersOfTwo_Zero_ReturnsNothing()
        {
            TestHelper.AssertSequence(
                Abacaxi.Integer.DeconstructIntoPowersOfTwo(0));
        }

        [Test]
        public void DeconstructIntoPowersOfTwo_One_ReturnsOne()
        {
            TestHelper.AssertSequence(
                Abacaxi.Integer.DeconstructIntoPowersOfTwo(1),
                1);
        }

        [Test]
        public void DeconstructIntoPowersOfTwo_Two_ReturnsTwo()
        {
            TestHelper.AssertSequence(
                Abacaxi.Integer.DeconstructIntoPowersOfTwo(2),
                2);
        }

        [Test]
        public void DeconstructIntoPowersOfTwo_Three_ReturnsOneThenTwo()
        {
            TestHelper.AssertSequence(
                Abacaxi.Integer.DeconstructIntoPowersOfTwo(3),
                1, 2);
        }

        [Test]
        public void DeconstructIntoPowersOfTwo_Four_ReturnsFour()
        {
            TestHelper.AssertSequence(
                Abacaxi.Integer.DeconstructIntoPowersOfTwo(4),
                4);
        }


        [Test]
        public void DeconstructIntoPowersOfTwo_MinusOne_ReturnsMinusOne()
        {
            TestHelper.AssertSequence(
                Abacaxi.Integer.DeconstructIntoPowersOfTwo(-1),
                -1);
        }

        [Test]
        public void DeconstructIntoPowersOfTwo_MinusTwo_ReturnsMinusTwo()
        {
            TestHelper.AssertSequence(
                Abacaxi.Integer.DeconstructIntoPowersOfTwo(-2),
                -2);
        }

        [Test]
        public void DeconstructIntoPowersOfTwo_MinusThree_ReturnsMinusOneThenMinusTwo()
        {
            TestHelper.AssertSequence(
                Abacaxi.Integer.DeconstructIntoPowersOfTwo(-3),
                -1, -2);
        }

        [Test]
        public void DeconstructIntoPowersOfTwo_Four_ReturnsMinusFour()
        {
            TestHelper.AssertSequence(
                Abacaxi.Integer.DeconstructIntoPowersOfTwo(-4),
                -4);
        }

        [TestCase(int.MaxValue)]
        [TestCase(int.MinValue)]
        public void DeconstructIntoPowersOfTwo__SumsToOriginal(int number)
        {
            var backSum = Abacaxi.Integer.DeconstructIntoPowersOfTwo(number).Sum();

            Assert.AreEqual(number, backSum);
        }
    }
}
