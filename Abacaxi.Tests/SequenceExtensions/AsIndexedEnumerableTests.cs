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

namespace Abacaxi.Tests.SequenceExtensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NUnit.Framework;
    using System.Diagnostics.CodeAnalysis;

    [TestFixture]
    public sealed class AsIndexedEnumerableTests
    {
        [Test]
        [SuppressMessage("ReSharper", "IteratorMethodResultIsIgnored")]
        [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void AsIndexedEnumerable_ThrowsException_ForNullSequence()
        {
            Assert.Throws<ArgumentNullException>(() =>
                ((int[]) null).AsIndexedEnumerable());
        }

        [Test]
        public void AsIndexedEnumerable_ReturnsNothing_ForEmptySequence()
        {
            var result = new int[] { }.AsIndexedEnumerable();

            TestHelper.AssertSequence(result);
        }

        [Test]
        public void AsIndexedEnumerable_ReturnsTheExpected_ForAList()
        {
            var result = new List<string> {"a", "b", "c"}.AsIndexedEnumerable();

            TestHelper.AssertSequence(result,
                new KeyValuePair<int, string>(0, "a"),
                new KeyValuePair<int, string>(1, "b"),
                new KeyValuePair<int, string>(2, "c"));
        }

        [Test]
        public void AsIndexedEnumerable_ReturnsTheExpected_ForAnEnumerable()
        {
            var result = new[] {"a", "b", "c"}.Where(p => true).AsIndexedEnumerable();

            TestHelper.AssertSequence(result,
                new KeyValuePair<int, string>(0, "a"),
                new KeyValuePair<int, string>(1, "b"),
                new KeyValuePair<int, string>(2, "c"));
        }
    }
}