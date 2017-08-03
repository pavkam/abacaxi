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
    public sealed class EscapeTests
    {
        [Test]
        public void Escape_ThrowsException_IfStringIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => ((string)null).Escape());
        }

        [TestCase("", "")]
        [TestCase("this string should not be escaped!", "this string should not be escaped!")]
        [TestCase("'", "\\'")]
        [TestCase("\"", "\\\"")]
        [TestCase("\\", "\\\\")]
        [TestCase("\0", "\\0")]
        [TestCase("\a", "\\a")]
        [TestCase("\b", "\\b")]
        [TestCase("\f", "\\f")]
        [TestCase("\n", "\\n")]
        [TestCase("\r", "\\r")]
        [TestCase("\t", "\\t")]
        [TestCase("\u0001", "\\u0001")]
        [TestCase("\u009f", "\\u009f")]
        [TestCase("Les Misérables", "Les Misérables")]
        [TestCase("क्षि", "क्षि")]
        public void Escape_ReturnsTheExpectedString(string input, string expected)
        {
            var actual = input.Escape();
            Assert.AreEqual(expected, actual);
        }
    }
}