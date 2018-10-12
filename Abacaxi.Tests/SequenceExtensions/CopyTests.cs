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
    public sealed class CopyTests
    {
        [TestCase(1, 0, 2), TestCase(1, 2, 0), TestCase(1, -1, 0), TestCase(1, 0, -1), TestCase(2, 1, 2)]
        public void Copy_ThrowsException_ForInvalidIndexes(int actualCount, int startIndex, int count)
        {
            var list = Enumerable.Range(0, actualCount).ToList();

            Assert.Throws<ArgumentOutOfRangeException>(() =>
                list.Copy(startIndex, count));
        }

        [TestCase(0, 0, 0), TestCase(1, 0, 0), TestCase(1, 1, 0)]
        public void Copy_ReturnsNothing_ForSpecificIndexes(int actualCount, int startIndex, int count)
        {
            var list = Enumerable.Range(0, actualCount).ToList();
            TestHelper.AssertSequence(list.Copy(startIndex, count));
        }

        [Test]
        public void Copy_CopiesFromTheEnd()
        {
            var a = new[] {1, 2, 3, 4, 5};

            TestHelper.AssertSequence(a.Skip(2).Take(3), a.Copy(2, 3));
        }

        [Test]
        public void Copy_CopiesFromTheMiddle()
        {
            var a = new[] {1, 2, 3, 4, 5};

            TestHelper.AssertSequence(a.Skip(1).Take(3), a.Copy(1, 3));
        }

        [Test]
        public void Copy_CopiesFromTheStart()
        {
            var a = new[] {1, 2, 3, 4, 5};

            TestHelper.AssertSequence(a.Take(3), a.Copy(0, 3));
        }

        [Test]
        public void Copy_CopiesFullSequence()
        {
            var a = new[] {1, 2, 3};

            TestHelper.AssertSequence(a, a.Copy(0, 3));
        }

        [Test, SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void Copy_ThrowsException_ForNullArray()
        {
            Assert.Throws<ArgumentNullException>(() =>
                ((int[]) null).Copy(0, 1));
        }

        [Test]
        public void Copy_WorksAsExpected_OnArray()
        {
            var a = new[] {1, 2, 3, 4, 5};

            TestHelper.AssertSequence(a.Skip(1).Take(3), a.Copy(1, a.Length - 2));
        }

        [Test]
        public void Copy_WorksAsExpected_OnIList()
        {
            var a = new[] {1, 2, 3, 4, 5}.Segment(0, 5);

            TestHelper.AssertSequence(a.Skip(1).Take(3), a.Copy(1, a.Count - 2));
        }

        [Test]
        public void Copy_WorksAsExpected_OnLists()
        {
            var a = new List<int> {1, 2, 3, 4, 5};

            TestHelper.AssertSequence(a.Skip(1).Take(3), a.Copy(1, a.Count - 2));
        }
    }
}