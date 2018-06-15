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
    public sealed class GetItemFrequenciesTests
    {
        [Test]
        public void GetItemFrequencies_ReturnsEmptyDictionary_ForEmptySequence()
        {
            var list = new int[] { };
            TestHelper.AssertSequence(list.GetItemFrequencies(EqualityComparer<int>.Default));
        }

        [Test]
        public void GetItemFrequencies_ReturnsValidItems()
        {
            var list = new[] {10, 1, 10, 10, 2, 2};
            var freq = list.GetItemFrequencies(EqualityComparer<int>.Default);

            Assert.AreEqual(3, freq.Count);
            Assert.AreEqual(3, freq[10]);
            Assert.AreEqual(2, freq[2]);
            Assert.AreEqual(1, freq[1]);
        }

        [Test, SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void GetItemFrequencies_ThrowsException_IfEqualityComparerIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new[] {1}.GetItemFrequencies(null));
        }

        [Test, SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void GetItemFrequencies_ThrowsException_IfSequenceIsNull()
        {
            Assert.Throws<ArgumentNullException>(
                () => ((int[]) null).GetItemFrequencies(EqualityComparer<int>.Default));
        }

        [Test]
        public void GetItemFrequencies_UsesTheEqualityComparer()
        {
            var list = new[] {"a", "A"};
            var freq = list.GetItemFrequencies(StringComparer.OrdinalIgnoreCase);

            Assert.AreEqual(2, freq["a"]);
        }
    }
}