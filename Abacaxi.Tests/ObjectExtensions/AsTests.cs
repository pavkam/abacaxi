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

namespace Abacaxi.Tests.ObjectExtensions
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using NUnit.Framework;

    [TestFixture]
    public sealed class AsTests
    {
        [Test]
        public void As_ActuallyPerformsValidation1()
        {
            var result = string.Empty;
            100.As<string>(v =>
            {
                result = v;
                return true;
            });

            Assert.AreEqual("100", result);
        }

        [Test]
        public void As_ActuallyPerformsValidation2()
        {
            var result = string.Empty;
            100.As<string>(CultureInfo.InvariantCulture, v =>
            {
                result = v;
                return true;
            });

            Assert.AreEqual("100", result);
        }

        [Test]
        public void As_ReturnsValidValue_WhenConversionSucceeds()
        {
            Assert.AreEqual(EditOperation.Match, "Match".As<EditOperation>());
        }

        [Test]
        public void As_Takes_CultureIntoAccount()
        {
            Assert.AreEqual(1.1, "1,1".As<double>(CultureInfo.GetCultureInfo("ro-RO")));
            Assert.AreEqual(11, "1,1".As<double>(CultureInfo.InvariantCulture));
        }

        [Test]
        public void As_ThrowException_WhenConversionFails()
        {
            Assert.Throws<FormatException>(() => "a".As<int>());
            Assert.Throws<FormatException>(() => "a".As<int>(CultureInfo.InvariantCulture));
        }

        [Test]
        public void As_ThrowException_WhenValidationFails()
        {
            Assert.Throws<InvalidOperationException>(() => "100".As<int>(v => false));
            Assert.Throws<InvalidOperationException>(() => "100".As<int>(CultureInfo.InvariantCulture, v => false));
        }

        [Test, SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void As_ThrowsException_ForNullFormatProvider()
        {
            Assert.Throws<ArgumentNullException>(() => "a".As<string>((IFormatProvider) null));
        }

        [Test, SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void As_ThrowsException_ForNullValidateFunc1()
        {
            Assert.Throws<ArgumentNullException>(() => "a".As((Func<string, bool>) null));
        }

        [Test, SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void As_ThrowsException_ForNullValidateFunc2()
        {
            Assert.Throws<ArgumentNullException>(() => "a".As(CultureInfo.InvariantCulture, (Func<string, bool>) null));
        }

        [Test]
        public void As_UsesInvariantCulture_ByDefault()
        {
            Assert.AreEqual(11, "1,1".As<double>());
        }
    }
}