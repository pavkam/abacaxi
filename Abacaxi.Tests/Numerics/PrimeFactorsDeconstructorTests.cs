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
    using Abacaxi.Numerics;
    using NUnit.Framework;
    using System.Linq;

    [TestFixture]
    public class PrimeFactorsDeconstructorTests
    {
        private PrimeFactorsDeconstructor _deconstructor;

        protected virtual PrimeFactorsDeconstructor CreateDeconstructor()
        {
            return new PrimeFactorsDeconstructor();
        }

        [SetUp]
        public void SetUp()
        {
            _deconstructor = CreateDeconstructor();
        }

        [Test]
        public void DeconstructZero_ReturnsZero()
        {
            TestHelper.AssertSequence(
                _deconstructor.Deconstruct(0),
                0);
        }

        [Test]
        public void DeconstructOne_ReturnsOne()
        {
            TestHelper.AssertSequence(
                _deconstructor.Deconstruct(1),
                1);
        }

        [Test]
        public void DeconstructTwo_ReturnsTwo()
        {
            TestHelper.AssertSequence(
                _deconstructor.Deconstruct(2),
                2);
        }

        [Test]
        public void DeconstructThree_ReturnsThree()
        {
            TestHelper.AssertSequence(
                _deconstructor.Deconstruct(3),
                3);
        }

        [Test]
        public void DeconstructFour_ReturnsTwoTwo()
        {
            TestHelper.AssertSequence(
                _deconstructor.Deconstruct(4),
                2, 2);
        }

        [Test]
        public void DeconstructTwenty_ReturnsTwoTwoFive()
        {
            TestHelper.AssertSequence(
                _deconstructor.Deconstruct(20),
                2, 2, 5);
        }


        [Test]
        public void DeconstructMinusOne_ReturnsMinusOne()
        {
            TestHelper.AssertSequence(
                _deconstructor.Deconstruct(-1),
                -1);
        }

        [Test]
        public void DeconstructMinusTwo_ReturnsMinusTwo()
        {
            TestHelper.AssertSequence(
                _deconstructor.Deconstruct(-2),
                -2);
        }

        [Test]
        public void DeconstructMinusThree_ReturnsMinusThree()
        {
            TestHelper.AssertSequence(
                _deconstructor.Deconstruct(-3),
                -3);
        }

        [Test]
        public void DeconstructMinusFour_ReturnsMinusOneMinusTwoMinusTwo()
        {
            TestHelper.AssertSequence(
                _deconstructor.Deconstruct(-4),
                -2, -2, -1);
        }

        [Test]
        public void DeconstructMinusEight_ReturnsMinusTwoMinusTwoMinusTwo()
        {
            TestHelper.AssertSequence(
                _deconstructor.Deconstruct(-8),
                -2, -2, -2);
        }

        [TestCase(int.MaxValue)]
        [TestCase(int.MinValue)]
        public void Deconstruct_MultipliesToOriginal(int number)
        {
            var mul = _deconstructor.Deconstruct(number).Aggregate((x, y) => x * y);

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
        public void IsPrimeNumber_ReturnsTrue(int number)
        {
            var result = _deconstructor.IsPrimeNumber(number);
            Assert.IsTrue(result);
        }

        [TestCase(4)]
        [TestCase(8)]
        [TestCase(144)]
        [TestCase(-4)]
        [TestCase(-8)]
        [TestCase(-144)]
        public void IsPrimeNumber_ReturnsFalse(int number)
        {
            var result = _deconstructor.IsPrimeNumber(number);
            Assert.IsFalse(result);
        }
    }
}
