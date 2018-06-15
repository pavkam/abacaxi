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
    using Integer = Abacaxi.Integer;

    [TestFixture]
    public class ZipTests
    {
        [TestCase(0, 0, 0), TestCase(0, 1, 10), TestCase(1, 0, 1), TestCase(12, 3, 132), TestCase(1, 23, 231),
         TestCase(12, 34, 3142)]
        public void Zip_ReturnsCorrectResult_IntBase10(int x, int y, int expected)
        {
            var result = Integer.Zip(x, y);
            Assert.AreEqual(expected, result);
        }

        [TestCase(0xA, 0xBC, 0xBCA), TestCase(0xAB, 0xC, 0xACB), TestCase(0xAB, 0xCD, 0xCADB)]
        public void Zip_ReturnsCorrectResult_IntBase16(int x, int y, int expected)
        {
            var result = Integer.Zip(x, y, 16);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Zip_ThrowsException_ForBaseOne()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                Integer.Zip(1, 1, 1));
        }

        [Test]
        public void Zip_ThrowsException_ForNegativeX()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                Integer.Zip(-1, 1, 2));
        }

        [Test]
        public void Zip_ThrowsException_ForNegativeY()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                Integer.Zip(1, -1, 2));
        }
    }
}