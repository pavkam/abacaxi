/* Copyright 2017-2018 by Alexandru Ciobanu (alex+git@ciobanu.org)
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
    using System;
    using NUnit.Framework;
    using Practice;

    [TestFixture]
    public sealed class GetCountOfTrailingZeroesInFactorialTests
    {
        [TestCase(0, 0), TestCase(1, 0), TestCase(2, 0), TestCase(3, 0), TestCase(4, 0), TestCase(5, 1), TestCase(6, 1),
         TestCase(7, 1), TestCase(8, 1), TestCase(9, 1), TestCase(10, 2), TestCase(11, 2),
         TestCase(12, 2), TestCase(13, 2), TestCase(14, 2), TestCase(15, 3), TestCase(25, 6)]
        public void GetCountOfTrailingZeroesInFactorial_ReturnsAppropriateValue(int n, int expected)
        {
            Assert.AreEqual(expected, Integer.GetCountOfTrailingZeroesInFactorial(n));
        }

        [Test]
        public void GetCountOfTrailingZeroesInFactorial_ThrowsException_IfNumberIsNegative()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Integer.GetCountOfTrailingZeroesInFactorial(-1));
        }

        [Test]
        public void Swap_SwapsNegativeAndPositiveValues()
        {
            var a = int.MaxValue;
            var b = int.MinValue;

            Integer.Swap(ref a, ref b);

            Assert.That(a == int.MinValue && b == int.MaxValue);
        }


        [Test]
        public void Swap_SwapsTwoNegativeValues()
        {
            var a = int.MinValue;
            var b = int.MinValue / 2;

            Integer.Swap(ref a, ref b);

            Assert.That(a == int.MinValue / 2 && b == int.MinValue);
        }
    }
}