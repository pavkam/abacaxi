﻿/* Copyright 2017 by Alexandru Ciobanu (alex+git@ciobanu.org)
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
    public class IntegerDeconstructIntoPrimeFactorsTests
    {
        [Test]
        public void DeconstructIntoPrimeFactors_Zero_ReturnsZero()
        {
            TestHelper.AssertSequence(
                0.DeconstructIntoPrimeFactors(),
                0);
        }

        [Test]
        public void DeconstructIntoPrimeFactors_One_ReturnsOne()
        {
            TestHelper.AssertSequence(
                1.DeconstructIntoPrimeFactors(),
                1);
        }

        [Test]
        public void DeconstructIntoPrimeFactors_Two_ReturnsTwo()
        {
            TestHelper.AssertSequence(
                2.DeconstructIntoPrimeFactors(),
                2);
        }

        [Test]
        public void DeconstructIntoPrimeFactors_Three_ReturnsThree()
        {
            TestHelper.AssertSequence(
                3.DeconstructIntoPrimeFactors(),
                3);
        }

        [Test]
        public void DeconstructIntoPrimeFactors_Four_ReturnsTwoTwo()
        {
            TestHelper.AssertSequence(
                4.DeconstructIntoPrimeFactors(),
                2, 2);
        }

        [Test]
        public void DeconstructIntoPrimeFactors_Twenty_ReturnsTwoTwoFive()
        {
            TestHelper.AssertSequence(
                20.DeconstructIntoPrimeFactors(),
                2, 2, 5);
        }


        [Test]
        public void DeconstructIntoPrimeFactors_MinusOne_ReturnsMinusOne()
        {
            TestHelper.AssertSequence(
                (-1).DeconstructIntoPrimeFactors(),
                -1);
        }

        [Test]
        public void DeconstructIntoPrimeFactors_MinusTwo_ReturnsMinusTwo()
        {
            TestHelper.AssertSequence(
                (-2).DeconstructIntoPrimeFactors(),
                -2);
        }

        [Test]
        public void DeconstructIntoPrimeFactors_MinusThree_ReturnsMinusThree()
        {
            TestHelper.AssertSequence(
                (-3).DeconstructIntoPrimeFactors(),
                -3);
        }

        [Test]
        public void DeconstructIntoPrimeFactors_MinusFour_ReturnsMinusOneMinusTwoMinusTwo()
        {
            TestHelper.AssertSequence(
                (-4).DeconstructIntoPrimeFactors(),
                -2, -2, -1);
        }

        [Test]
        public void DeconstructIntoPrimeFactors_MinusEight_ReturnsMinusTwoMinusTwoMinusTwo()
        {
            TestHelper.AssertSequence(
                (-8).DeconstructIntoPrimeFactors(),
                -2, -2, -2);
        }

        [TestCase(int.MaxValue)]
        [TestCase(int.MinValue)]
        public void DeconstructIntoPrimeFactors__MultipliesToOriginal(int number)
        {
            var mul = number.DeconstructIntoPrimeFactors().Aggregate((x, y) => x * y);

            Assert.AreEqual(number, mul);
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(13)]
        [TestCase(239)]
        [TestCase(-1)]
        [TestCase(-2)]
        [TestCase(-3)]
        [TestCase(-13)]
        [TestCase(-239)]
        public void IsPrime_ReturnsTrue(int number)
        {
            var result = number.IsPrime();
            Assert.IsTrue(result);
        }

        [TestCase(4)]
        [TestCase(8)]
        [TestCase(144)]
        [TestCase(-4)]
        [TestCase(-8)]
        [TestCase(-144)]
        public void IsPrime_ReturnsFalse(int number)
        {
            var result = number.IsPrime();
            Assert.IsFalse(result);
        }
    }
}
