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
    using System.Globalization;
    using System.Diagnostics.CodeAnalysis;

    [TestFixture]
    public sealed class TryAsTests
    {
        [Test]
        [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void TryAs1_ThrowsException_IfFormatProviderIsNull()
        {
            Assert.Throws<ArgumentNullException>(
                () => "".TryAs(null, out int dummy));
        }

        [Test]
        public void TryAs1_ReturnsTrue_ForValidStructConversion()
        {
            Assert.IsTrue("1".TryAs(CultureInfo.CurrentCulture, out int dummy));
        }

        [Test]
        public void TryAs1_ReturnsExpectedResult_ForValidStructConversion()
        {
            "100".TryAs(CultureInfo.CurrentCulture, out int result);
            Assert.AreEqual(100, result);
        }

        [Test]
        public void TryAs1_ReturnsFalse_ForInvalidStructConversion()
        {
            Assert.IsFalse("a".TryAs(CultureInfo.CurrentCulture, out int dummy));
        }

        [Test]
        public void TryAs1_ReturnsTrue_ForValidEnumConversion()
        {
            Assert.IsTrue("INSERT".TryAs(CultureInfo.CurrentCulture, out EditOperation dummy));
        }

        [Test]
        public void TryAs1_ReturnsExpectedResult_ForValidEnumConversion()
        {
            "INSERT".TryAs(CultureInfo.CurrentCulture, out EditOperation result);
            Assert.AreEqual(EditOperation.Insert, result);
        }

        [Test]
        public void TryAs1_ReturnsFalse_IfStringIsNull_1()
        {
            Assert.IsFalse(((string)null).TryAs(CultureInfo.CurrentCulture, out EditOperation _));
        }

        [Test]
        public void TryAs1_ReturnsFalse_IfStringIsNull_2()
        {
            Assert.IsFalse(((string)null).TryAs(CultureInfo.CurrentCulture, out double _));
        }
    }
}