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
    public sealed class ToSetTests
    {
        [Test]
        public void ToSet_ReturnsAValidSet1()
        {
            var set = new[] { 1, 1, 2, 3 }.ToSet();

            TestHelper.AssertSequence(set, 1, 2, 3);
        }

        [Test]
        public void ToSet_ReturnsAValidSet2()
        {
            var set = new[] { 1, 1, 2, 3 }.ToSet(EqualityComparer<int>.Default);

            TestHelper.AssertSequence(set, 1, 2, 3);
        }

        [Test]
        public void ToSet_ReturnsAValidSet3()
        {
            var set = new[] { 1, 1, 2, 3 }.ToSet(a => a.ToString());

            TestHelper.AssertSequence(set, "1", "2", "3");
        }

        [Test, SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void ToSet_ThrowsException_IfEqualityComparerIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new[] { 1 }.ToSet(null));
        }

        [Test, SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void ToSet_ThrowsException_IfSelectorIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new[] { 1 }.ToSet((Func<int, int>) null));
        }

        [Test, SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void ToSet_ThrowsException_IfSequenceIsNull1()
        {
            Assert.Throws<ArgumentNullException>(() => ((int[]) null).ToSet(EqualityComparer<int>.Default));
        }

        [Test, SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void ToSet_ThrowsException_IfSequenceIsNull2()
        {
            Assert.Throws<ArgumentNullException>(() => ((int[]) null).ToSet());
        }

        [Test, SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void ToSet_ThrowsException_IfSequenceIsNull3()
        {
            Assert.Throws<ArgumentNullException>(() => ((int[]) null).ToSet(a => a));
        }

        [Test]
        public void ToSet_UsesTheEqualityComparer()
        {
            var set = new[] { "a", "A", "b", "c" }.ToSet(StringComparer.OrdinalIgnoreCase);

            TestHelper.AssertSequence(set, "a", "b", "c");
        }
    }
}