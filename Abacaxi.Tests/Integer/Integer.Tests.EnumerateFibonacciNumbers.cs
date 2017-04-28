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

namespace Abacaxi.Tests.Integer
{
    using System;
    using System.Linq;
    using NUnit.Framework;

    [TestFixture]
    public class IntegerEnumerateFibonacciNumbersTests
    {
        [Test]
        public void EnumerateFibonacciNumbers_ThrowsException_ForNegativeCount()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                Abacaxi.Integer.EnumerateFibonacciNumbers(-1).ToArray());
        }

        [Test]
        public void EnumerateFibonacciNumbers_ReturnsNothing_ForZeroCount()
        {
            TestHelper.AssertSequence(
                Abacaxi.Integer.EnumerateFibonacciNumbers(0));
        }

        [Test]
        public void EnumerateFibonacciNumbers_ReturnsCorrectSequence_ForCountOfOne()
        {
            TestHelper.AssertSequence(
                Abacaxi.Integer.EnumerateFibonacciNumbers(1),
                0);
        }

        [Test]
        public void EnumerateFibonacciNumbers_ReturnsCorrectSequence_ForCountOfTwo()
        {
            TestHelper.AssertSequence(
                Abacaxi.Integer.EnumerateFibonacciNumbers(2),
                0, 1);
        }

        [Test]
        public void EnumerateFibonacciNumbers_ReturnsCorrectSequence_ForCountOfThree()
        {
            TestHelper.AssertSequence(
                Abacaxi.Integer.EnumerateFibonacciNumbers(3),
                0, 1, 1);
        }

        [Test]
        public void EnumerateFibonacciNumbers_ReturnsCorrectSequenc_ForLongCount()
        {
            TestHelper.AssertSequence(
                Abacaxi.Integer.EnumerateFibonacciNumbers(21),
                0, 1, 1, 2, 3, 5, 8, 13, 21, 34, 55, 89, 144, 233, 377, 610, 987, 1597, 2584, 4181, 6765);
        }

        [Test]
        public void GetFibonacciNumber_ThrowsException_ForNegativeIndex()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                Abacaxi.Integer.GetFibonacciNumber(-1));
        }

        [Test]
        public void GetFibonacciNumber_ReturnsCorrectNumber_ForGivenIndex()
        {
            var expected = new[] { 0, 1, 1, 2, 3, 5, 8, 13, 21, 34, 55, 89, 144, 233, 377, 610, 987, 1597, 2584, 4181, 6765 };
            for (var i = 0; i < expected.Length; i++)
            {
                var result = Abacaxi.Integer.GetFibonacciNumber(i);
                Assert.AreEqual(expected[i], result);
            }
        }
    }
}
