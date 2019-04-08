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
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using NUnit.Framework;

    [TestFixture]
    public sealed class AddOrUpdateTests
    {
        [Test]
        public void AddOrUpdate_AddsTheKeyValue_IfTheKeyIsNotFound()
        {
            var dict = new Dictionary<string, int>();
            dict.AddOrUpdate("key", 1, i => i);

            Assert.IsTrue(dict.TryGetValue("key", out var value));
            Assert.AreEqual(1, value);
        }

        [Test]
        public void AddOrUpdate_ReturnsFalse_IfKeyValueIsUpdated()
        {
            var dict = new Dictionary<string, int> {{"key", 1}};
            Assert.IsFalse(dict.AddOrUpdate("key", 1, i => i));
        }

        [Test]
        public void AddOrUpdate_ReturnsTrue_IfKeyValueIsAdded()
        {
            var dict = new Dictionary<string, int>();
            Assert.IsTrue(dict.AddOrUpdate("key", 1, i => i));
        }

        [Test, SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void AddOrUpdate_ThrowsException_IfDictIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => ((Dictionary<int, int>) null).AddOrUpdate(1, 1, i => i));
        }

        [Test, SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void AddOrUpdate_ThrowsException_IfUpdateFuncIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new Dictionary<int, int>().AddOrUpdate(1, 1, null));
        }

        [Test]
        public void AddOrUpdate_UpdatesTheValue_IfTheKeyIsFound()
        {
            var dict = new Dictionary<string, int> {{"key", 1}};
            dict.AddOrUpdate("key", 2, i => -1);

            Assert.IsTrue(dict.TryGetValue("key", out var value));
            Assert.AreEqual(-1, value);
        }
    }
}