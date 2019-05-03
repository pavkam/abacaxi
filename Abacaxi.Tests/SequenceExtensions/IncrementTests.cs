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

namespace Abacaxi.Tests.SequenceExtensions
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using NUnit.Framework;

    [TestFixture]
    public sealed class IncrementTests
    {
        [Test]
        public void Increment_AddsNewKeyIntoDictionaryWithValueOfDelta()
        {
            var dict = new Dictionary<string, int>();
            dict.Increment("test");

            Assert.AreEqual(1, dict["test"]);
        }

        [Test]
        public void Increment_IncrementsTheExistingValueByDelta()
        {
            var dict = new Dictionary<string, int> {{"test", 10}};
            dict.Increment("test", -1);

            Assert.AreEqual(9, dict["test"]);
        }

        [Test]
        public void Increment_ReturnsTheNewValueOfKeyAsResult_IfKeyDoesNotExist()
        {
            var dict = new Dictionary<string, int>();
            var r = dict.Increment("test", -99);

            Assert.AreEqual(-99, r);
        }

        [Test]
        public void Increment_ReturnsTheNewValueOfKeyAsResult_IfKeyExists()
        {
            var dict = new Dictionary<string, int> {{"test", 10}};

            var r = dict.Increment("test", 10);

            Assert.AreEqual(20, r);
        }

        [Test, SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void Increment_ThrowsExceptionIfDictionaryIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => ((Dictionary<string, int>) null).Increment("a"));
        }

        [Test, SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void Increment_ThrowsExceptionIfKeyIsNull()
        {
            var dict = new Dictionary<string, int>();
            Assert.Throws<ArgumentNullException>(() => dict.Increment(null));
        }
    }
}