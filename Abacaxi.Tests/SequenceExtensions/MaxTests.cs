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
    using SequenceExtensions = Abacaxi.SequenceExtensions;

    [TestFixture]
    public class MaxTests
    {
        [Test]
        public void Max1_ReturnsNull_ForEmptyCollectionOfObjects()
        {
            var result = new string[] { }.Max(i => i, StringComparer.Ordinal);
            Assert.IsNull(result);
        }

        [Test]
        public void Max1_ReturnsTheFirstFoundItem_BasedOnHighestKey()
        {
            var result = new[] {"bb", "ccc", "a", "z"}.Max(i => i.Length, Comparer<int>.Default);
            Assert.AreEqual("ccc", result);
        }

        [Test]
        public void Max1_ReturnsTheOnlyElement_EvenIfTheKeyIsNull()
        {
            var result = new[] {1}.Max(i => null, StringComparer.Ordinal);
            Assert.AreEqual(1, result);
        }

        [Test]
        public void Max1_SkipsElements_ThatHaveNullKeys()
        {
            var result =
                new[] {1, 2, 3, 4}.Max(i => i % 2 == 0 ? null as string : i.ToString(), StringComparer.Ordinal);
            Assert.AreEqual(3, result);
        }

        [Test]
        public void Max1_ThrowsException_ForEmptyCollectionOfValueTypes()
        {
            Assert.Throws<InvalidOperationException>(() =>
                new int[] { }.Max(i => i, Comparer<int>.Default));
        }

        [Test, SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void Max1_ThrowsException_ForNullComparer()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new string[] { }.Max(i => i, null));
        }

        [Test, SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void Max1_ThrowsException_ForNullSelector()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new string[] { }.Max(null, StringComparer.Ordinal));
        }

        [Test, SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void Max1_ThrowsException_ForNullSequence()
        {
            Assert.Throws<ArgumentNullException>(() =>
                SequenceExtensions.Max<string, string>(null, i => i, StringComparer.Ordinal));
        }

        [Test]
        public void Max2_ReturnsNull_ForEmptyCollectionOfObjects()
        {
            var result = new string[] { }.Max(i => i);
            Assert.IsNull(result);
        }

        [Test]
        public void Max2_ReturnsTheFirstFoundItem_BasedOnHighestKey()
        {
            var result = new[] {"bb", "ccc", "a", "z"}.Max(i => i.Length);
            Assert.AreEqual("ccc", result);
        }

        [Test]
        public void Max2_ReturnsTheOnlyElement_EvenIfTheKeyIsNull()
        {
            var result = new[] {1}.Max<int, string>(i => null);
            Assert.AreEqual(1, result);
        }

        [Test]
        public void Max2_SkipsElements_ThatHaveNullKeys()
        {
            var result = new[] {1, 2, 3, 4}.Max(i => i % 2 == 0 ? null as string : i.ToString());
            Assert.AreEqual(3, result);
        }

        [Test]
        public void Max2_ThrowsException_ForEmptyCollectionOfValueTypes()
        {
            Assert.Throws<InvalidOperationException>(() =>
                new int[] { }.Max(i => i));
        }

        [Test, SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void Max2_ThrowsException_ForNullSelector()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new string[] { }.Max<string, string>(null));
        }

        [Test, SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void Max2_ThrowsException_ForNullSequence()
        {
            Assert.Throws<ArgumentNullException>(() =>
                SequenceExtensions.Max<string, string>(null, i => i));
        }
    }
}