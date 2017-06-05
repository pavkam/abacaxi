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

namespace Abacaxi.Tests.Costs
{
    using NUnit.Framework;
    using Abacaxi.Costs;

    [TestFixture]
    public class IntegerAccountantTests
    {
        private readonly IntegerAccountant _accountant = new IntegerAccountant();

        [Test]
        public void IntegerAccountant_Zero_ReturnsZero()
        {
            Assert.AreEqual(0, _accountant.Zero);
        }

        [TestCase(0, 0, 0)]
        [TestCase(-1, -1, -2)]
        [TestCase(1, 1, 2)]
        [TestCase(1, -1, 0)]
        public void IntegerAccountant_Add_ReturnsTheSumOfTwoCosts(int left, int right, int expected)
        {
            Assert.AreEqual(expected, _accountant.Add(left, right));
        }

        [TestCase(0, 0, 0)]
        [TestCase(-1, -1, 0)]
        [TestCase(1, 1, 0)]
        [TestCase(1, -1, 2)]
        public void IntegerAccountant_Subtract_ReturnsTheDifferenceOfTwoCosts(int left, int right, int expected)
        {
            Assert.AreEqual(expected, _accountant.Subtract(left, right));
        }

        [TestCase(0, 0, 0)]
        [TestCase(-1, -1, 1)]
        [TestCase(5, 1, 5)]
        [TestCase(1, -5, -5)]
        public void IntegerAccountant_Multiply_ReturnsTheProductOfCost(int cost, int multiplier, int expected)
        {
            Assert.AreEqual(expected, _accountant.Multiply(cost, multiplier));
        }

        [TestCase(0, 0, 0)]
        [TestCase(-1, -1, 0)]
        [TestCase(5, 1, 4)]
        [TestCase(1, -5, 6)]
        public void IntegerAccountant_Compare_ReturnsCorrectComparisonResult(int left, int right, int expected)
        {
            Assert.AreEqual(expected, _accountant.Compare(left, right));
        }
    }
}
