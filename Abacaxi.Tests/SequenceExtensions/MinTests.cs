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
    using SequenceExtensions = Abacaxi.SequenceExtensions;

    [TestFixture]
    public class MinTests
    {
        [Test]
        public void Min1_ReturnsNull_ForEmptyCollectionOfObjects()
        {
            var result = new string[] { }.Min(i => i, StringComparer.Ordinal);
            Assert.IsNull(result);
        }

        [Test]
        public void Min1_ReturnsTheFirstFoundItem_BasedOnLowestKey()
        {
            var result = new[] {"bb", "ccc", "a", "z"}.Min(i => i.Length, Comparer<int>.Default);
            Assert.AreEqual("a", result);
        }

        [Test]
        public void Min1_ReturnsTheOnlyElement_EvenIfTheKeyIsNull()
        {
            var result = new[] {1}.Min(i => null, StringComparer.Ordinal);
            Assert.AreEqual(1, result);
        }

        [Test]
        public void Min1_SkipsElements_ThatHaveNullKeys()
        {
            var result =
                new[] {1, 2, 3, 4}.Min(i => i % 2 == 1 ? null as string : i.ToString(), StringComparer.Ordinal);
            Assert.AreEqual(2, result);
        }

        [Test]
        public void Min1_ThrowsException_ForEmptyCollectionOfValueTypes()
        {
            Assert.Throws<InvalidOperationException>(() =>
                new int[] { }.Min(i => i, Comparer<int>.Default));
        }

        [Test, SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void Min1_ThrowsException_ForNullComparer()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new string[] { }.Min(i => i, null));
        }

        [Test, SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void Min1_ThrowsException_ForNullSelector()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new string[] { }.Min(null, StringComparer.Ordinal));
        }

        [Test, SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void Min1_ThrowsException_ForNullSequence()
        {
            Assert.Throws<ArgumentNullException>(() =>
                SequenceExtensions.Min<string, string>(null, i => i, StringComparer.Ordinal));
        }

        [Test]
        public void Min2_ReturnsNull_ForEmptyCollectionOfObjects()
        {
            var result = new string[] { }.Min(i => i);
            Assert.IsNull(result);
        }

        [Test]
        public void Min2_ReturnsTheFirstFoundItem_BasedOnLowestKey()
        {
            var result = new[] {"bb", "ccc", "a", "z"}.Min(i => i.Length);
            Assert.AreEqual("a", result);
        }

        [Test]
        public void Min2_ReturnsTheOnlyElement_EvenIfTheKeyIsNull()
        {
            var result = new[] {1}.Min<int, string>(i => null);
            Assert.AreEqual(1, result);
        }

        [Test]
        public void Min2_SkipsElements_ThatHaveNullKeys()
        {
            var result = new[] {1, 2, 3, 4}.Min(i => i % 2 == 1 ? null as string : i.ToString());
            Assert.AreEqual(2, result);
        }

        [Test]
        public void Min2_ThrowsException_ForEmptyCollectionOfValueTypes()
        {
            Assert.Throws<InvalidOperationException>(() =>
                new int[] { }.Min(i => i));
        }

        [Test, SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void Min2_ThrowsException_ForNullSelector()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new string[] { }.Min<string, string>(null));
        }

        [Test, SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void Min2_ThrowsException_ForNullSequence()
        {
            Assert.Throws<ArgumentNullException>(() =>
                SequenceExtensions.Min<string, string>(null, i => i));
        }
    }
}