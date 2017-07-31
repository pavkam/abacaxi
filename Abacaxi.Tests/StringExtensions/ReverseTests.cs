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

namespace Abacaxi.Tests.StringExtensions
{
    using System;
    using NUnit.Framework;

    [TestFixture]
    public sealed class ReverseTests
    {
        [Test]
        public void Reverse_ThrowsException_IfStringIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => ((string)null).Reverse());
        }

        [TestCase("", "")]
        [TestCase("1", "1")]
        [TestCase("Hello World", "dlroW olleH")]
        [TestCase("Les Misérables", "selbarésiM seL")]
        [TestCase("This is: 🀜", "🀜 :si sihT")]
        public void Reverse_ReversesTheString_AsExpected(string input, string expected)
        {
            var actual = input.Reverse();
            Assert.AreEqual(expected, actual);
        }
    }
}