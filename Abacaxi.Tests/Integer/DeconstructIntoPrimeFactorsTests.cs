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

namespace Abacaxi.Tests.Integer
{
    using System.Linq;
    using NUnit.Framework;
    using Integer = Abacaxi.Integer;

    [TestFixture]
    public class DeconstructIntoPrimeFactorsTests
    {
        [TestCase(int.MaxValue), TestCase(int.MinValue)]
        public void DeconstructIntoPrimeFactors__MultipliesToOriginal(int number)
        {
            var mul = Integer.DeconstructIntoPrimeFactors(number).Aggregate((x, y) => x * y);

            Assert.AreEqual(number, mul);
        }

        [Test]
        public void DeconstructIntoPrimeFactors_Four_ReturnsTwoTwo()
        {
            TestHelper.AssertSequence(
                Integer.DeconstructIntoPrimeFactors(4),
                2, 2);
        }

        [Test]
        public void DeconstructIntoPrimeFactors_MinusEight_ReturnsMinusTwoMinusTwoMinusTwo()
        {
            TestHelper.AssertSequence(
                Integer.DeconstructIntoPrimeFactors(-8),
                -2, -2, -2);
        }

        [Test]
        public void DeconstructIntoPrimeFactors_MinusFour_ReturnsMinusOneMinusTwoMinusTwo()
        {
            TestHelper.AssertSequence(
                Integer.DeconstructIntoPrimeFactors(-4),
                -2, -2, -1);
        }


        [Test]
        public void DeconstructIntoPrimeFactors_MinusOne_ReturnsMinusOne()
        {
            TestHelper.AssertSequence(
                Integer.DeconstructIntoPrimeFactors(-1),
                -1);
        }

        [Test]
        public void DeconstructIntoPrimeFactors_MinusThree_ReturnsMinusThree()
        {
            TestHelper.AssertSequence(
                Integer.DeconstructIntoPrimeFactors(-3),
                -3);
        }

        [Test]
        public void DeconstructIntoPrimeFactors_MinusTwo_ReturnsMinusTwo()
        {
            TestHelper.AssertSequence(
                Integer.DeconstructIntoPrimeFactors(-2),
                -2);
        }

        [Test]
        public void DeconstructIntoPrimeFactors_One_ReturnsOne()
        {
            TestHelper.AssertSequence(
                Integer.DeconstructIntoPrimeFactors(1),
                1);
        }

        [Test]
        public void DeconstructIntoPrimeFactors_Three_ReturnsThree()
        {
            TestHelper.AssertSequence(
                Integer.DeconstructIntoPrimeFactors(3),
                3);
        }

        [Test]
        public void DeconstructIntoPrimeFactors_Twenty_ReturnsTwoTwoFive()
        {
            TestHelper.AssertSequence(
                Integer.DeconstructIntoPrimeFactors(20),
                2, 2, 5);
        }

        [Test]
        public void DeconstructIntoPrimeFactors_Two_ReturnsTwo()
        {
            TestHelper.AssertSequence(
                Integer.DeconstructIntoPrimeFactors(2),
                2);
        }

        [Test]
        public void DeconstructIntoPrimeFactors_Zero_ReturnsZero()
        {
            TestHelper.AssertSequence(
                Integer.DeconstructIntoPrimeFactors(0),
                0);
        }
    }
}