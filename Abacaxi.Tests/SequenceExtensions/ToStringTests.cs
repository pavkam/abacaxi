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

namespace Abacaxi.Tests.SequenceExtensions
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using JetBrains.Annotations;
    using NUnit.Framework;

    [TestFixture]
    public class ToStringTests
    {
        [CanBeNull] private readonly int[] _nullArray = null;
        [NotNull] private readonly int[] _emptyArray = { };
        [NotNull] private readonly int[] _oneArray = {123};
        [NotNull] private readonly int[] _twoArray = {123, 456};

        [Test]
        public void ToString1_ActuallyCaresAboutSelector()
        {
            var result = _twoArray.ToString(s => $"[{s}]", "-");
            Assert.AreEqual("[123]-[456]", result);
        }

        [Test]
        public void ToString1_ReturnsCommaDelimitedString_ForSequenceOfTwo()
        {
            var result = _twoArray.ToString(s => s, ",");
            Assert.AreEqual("123,456", result);
        }

        [Test]
        public void ToString1_ReturnsEmptyString_ForEmptySequence()
        {
            var result = _emptyArray.ToString(s => s, ",");
            Assert.IsEmpty(result);
        }

        [Test]
        public void ToString1_ReturnsStringValue_ForSequenceOfOne()
        {
            var result = _oneArray.ToString(s => s, ",");
            Assert.AreEqual("123", result);
        }

        [Test, SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void ToString1_ThrowsException_ForNullSelector()
        {
            Assert.Throws<ArgumentNullException>(() => _emptyArray.ToString((Func<int, int>) null, ""));
        }

        [Test, SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void ToString1_ThrowsException_ForNullSeparator()
        {
            Assert.Throws<ArgumentNullException>(() => _emptyArray.ToString(i => i, null));
        }

        [Test, SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void ToString1_ThrowsException_ForNullSequence()
        {
            Assert.Throws<ArgumentNullException>(() => _nullArray.ToString(s => s, ""));
        }

        [Test]
        public void ToString2_ReturnsCommaDelimitedString_ForSequenceOfTwo()
        {
            var result = _twoArray.ToString(",");
            Assert.AreEqual("123,456", result);
        }

        [Test]
        public void ToString2_ReturnsEmptyString_ForEmptySequence()
        {
            var result = _emptyArray.ToString(",");
            Assert.IsEmpty(result);
        }

        [Test]
        public void ToString2_ReturnsStringValue_ForSequenceOfOne()
        {
            var result = _oneArray.ToString(",");
            Assert.AreEqual("123", result);
        }

        [Test, SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void ToString2_ThrowsException_ForNullSeparator()
        {
            Assert.Throws<ArgumentNullException>(() => _emptyArray.ToString(null));
        }

        [Test, SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void ToString2_ThrowsException_ForNullSequence()
        {
            Assert.Throws<ArgumentNullException>(() => _nullArray.ToString(""));
        }
    }
}