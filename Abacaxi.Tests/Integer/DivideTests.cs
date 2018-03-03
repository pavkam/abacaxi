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
    public class DivideTests
    {
        [Test]
        public void Divide_ThrowsException_ForZeroDivisor()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                Integer.Divide(1, 0));
        }

        [TestCase(0, 1, 0),TestCase(1, 1, 1),TestCase(2, 1, 2),TestCase(10, 2, 5),TestCase(100, 3, 33),TestCase(-10, 2, -5),TestCase(-10, -2, 5),TestCase(10, -2, -5)]
        public void Divide_ReturnsCorrectResult(int x, int y, int expected)
        {
            var result = Integer.Divide(x, y);
            Assert.AreEqual(expected, result);
        }
    }
}
