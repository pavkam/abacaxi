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
    using System.Linq;
    using NUnit.Framework;

    [TestFixture]
    public sealed class ToListTests
    {
        [Test]
        public void ToList1_SelectsExpectedItems_ForAList()
        {
            var actual = new List<string> {"1", "2", "3"}.ToList(int.Parse);

            TestHelper.AssertSequence(actual, 1, 2, 3);
        }

        [Test]
        public void ToList1_SelectsExpectedItems_ForEnumerable()
        {
            var actual = new List<string> {"1", "2", "3"}.Where(p => true).ToList(int.Parse);

            TestHelper.AssertSequence(actual, 1, 2, 3);
        }

        [Test, SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void ToList1_ThrowsException_ForNullSelector()
        {
            Assert.Throws<ArgumentNullException>(() => new int[] { }.ToList((Func<int, int>) null));
        }

        [Test, SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void ToList1_ThrowsException_ForNullSequence()
        {
            Assert.Throws<ArgumentNullException>(() => ((int[]) null).ToList(i => i));
        }

        [Test]
        public void ToList2_SelectsExpectedItems_ForAList()
        {
            var actual = new List<string> {"a", "b", "c"}.ToList((s, i) => $"{i}:{s}");

            TestHelper.AssertSequence(actual, "0:a", "1:b", "2:c");
        }

        [Test]
        public void ToList2_SelectsExpectedItems_ForEnumerable()
        {
            var actual = new List<string> {"a", "b", "c"}.Where(p => true).ToList((s, i) => $"{i}:{s}");

            TestHelper.AssertSequence(actual, "0:a", "1:b", "2:c");
        }

        [Test, SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void ToList2_ThrowsException_ForNullSelector()
        {
            Assert.Throws<ArgumentNullException>(() => new int[] { }.ToList((Func<int, int, int>) null));
        }

        [Test, SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void ToList2_ThrowsException_ForNullSequence()
        {
            Assert.Throws<ArgumentNullException>(() => ((int[]) null).ToList((n, i) => i));
        }
    }
}