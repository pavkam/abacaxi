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
    using NUnit.Framework;
    using Practice;

    [TestFixture]
    public sealed class MaxTests
    {
        [TestCase(0, 0, 0),
         TestCase(int.MaxValue, 1, int.MaxValue),
         TestCase(int.MinValue, -1, -1),
         TestCase(int.MinValue, int.MaxValue, int.MaxValue),
         TestCase(1, int.MaxValue, int.MaxValue),
         TestCase(-1, int.MinValue, -1),
         TestCase(int.MaxValue, int.MinValue, int.MaxValue)
        ]
        public void Max_ReturnsProperValue(int a, int b, int expected)
        {
            var result = Integer.Max(a, b);

            Assert.AreEqual(expected, result);
        }
    }
}