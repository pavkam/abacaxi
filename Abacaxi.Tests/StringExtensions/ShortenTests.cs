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

namespace Abacaxi.Tests.StringExtensions
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using JetBrains.Annotations;
    using NUnit.Framework;

    [TestFixture]
    public sealed class ShortenTests
    {
        [TestCase("", 1, null, ""), TestCase("1", 1, null, "1"), TestCase("12", 1, null, "1"),
         TestCase("12345", 5, null, "12345"), TestCase("1234567890", 5, "?", "1234?"),
         TestCase("1234567890", 5, "...", "12..."), TestCase("12345", 3, "...", "..."), TestCase("1", 1, "?", "1"),
         TestCase("12", 1, "?", "?")]
        public void Shorten_ReturnsExpectedString([NotNull] string s, int l, string e, string expected)
        {
            var actual = s.Shorten(l, e);

            Assert.AreEqual(expected, actual);
        }

        [TestCase("Les Misérables", 7, "Les Mis"), TestCase("Les Misérables", 8, "Les Mis"),
         TestCase("Les Misérables", 9, "Les Misé"), TestCase("क्षि", 4, "क्षि"), TestCase("क्षि", 3, "क्"),
         TestCase("क्षि", 2, "क्"), TestCase("क्षि", 1, ""), SuppressMessage("ReSharper", "StringLiteralTypo")]
        public void Shorten_TakesIntoAccount_MultiCharSequences([NotNull] string s, int l, string expected)
        {
            var actual = s.Shorten(l);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Shorten_ThrowsException_IfEllipsisLengthGreaterThanMaxLength()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => "".Shorten(2, "..."));
        }

        [Test]
        public void Shorten_ThrowsException_IfMaxLengthLessThanOne()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => "".Shorten(0));
        }

        [Test, SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void Shorten_ThrowsException_IfStringIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => ((string) null).Shorten(100));
        }
    }
}