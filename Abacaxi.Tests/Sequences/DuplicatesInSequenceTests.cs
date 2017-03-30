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

namespace Abacaxi.Tests.Sequences
{
    using System;
    using System.Linq;
    using Abacaxi.Sequences;
    using NUnit.Framework;
    using System.Collections.Generic;

    [TestFixture]
    public class DuplicatesInSequenceTests
    {
        [Test]
        public void GenericFind_ThowsException_ForNullSequence()
        {
            Assert.Throws<ArgumentNullException>(() =>
                DuplicatesInSequence.Find(null, EqualityComparer<int>.Default).ToArray());
        }

        [Test]
        public void GenericFind_ThowsException_ForNullEqualityComparer()
        {
            Assert.Throws<ArgumentNullException>(() =>
                DuplicatesInSequence.Find(new[] { 1 }, null).ToArray());
        }

        [Test]
        public void IntegerFind_ThowsException_ForNullSequence()
        {
            Assert.Throws<ArgumentNullException>(() =>
                DuplicatesInSequence.Find(null, 1, 1).ToArray());
        }

        [Test]
        public void IntegerFind_ThowsException_ForMaxLessThanMin()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                DuplicatesInSequence.Find(new[] { 1 }, 1, 0).ToArray());
        }

        [Test]
        public void StringFind_ThowsException_ForNullSequence()
        {
            Assert.Throws<ArgumentNullException>(() =>
                DuplicatesInSequence.Find((string)null).ToArray());
        }

        [Test]
        public void GenericFind_ReturnsNothing_ForDistinctElements()
        {
            TestHelper.AssertSequence(
                DuplicatesInSequence.Find("123456789", EqualityComparer<char>.Default));
        }

        [Test]
        public void GenericFind_ReturnsDuplicates_ForDistinctElements()
        {
            TestHelper.AssertSequence(
                DuplicatesInSequence.Find("121312", EqualityComparer<char>.Default),
                new KeyValuePair<char, int>('1', 3),
                new KeyValuePair<char, int>('2', 2));
        }

        [Test]
        public void GenericFind_UsesTheComparer()
        {
            TestHelper.AssertSequence(
                DuplicatesInSequence.Find(new[] { "a", "A" }, StringComparer.InvariantCultureIgnoreCase),
                new KeyValuePair<string, int>("a", 2));
        }

        [Test]
        public void IntegerFind_ReturnsNothing_ForDistinctElements()
        {
            TestHelper.AssertSequence(
                DuplicatesInSequence.Find(new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 }, 1, 9));
        }

        [Test]
        public void IntegerFind_ReturnsDuplicates_ForDistinctElements()
        {
            TestHelper.AssertSequence(
                DuplicatesInSequence.Find(new[] { 1, 2, 1, 3, 1, 2 }, 1, 3),
                new KeyValuePair<int, int>(1, 3),
                new KeyValuePair<int, int>(2, 2));
        }

        [Test]
        public void IntegerFind_ThowsException_IfSequenceContainsElementsOutOfMinAndMax()
        {
            Assert.Throws<InvalidOperationException>(() =>
                DuplicatesInSequence.Find(new[] { 0, 1, -1 }, 0, 1).ToArray());
        }

        [Test]
        public void StringFind_ReturnsNothing_ForDistinctElements()
        {
            TestHelper.AssertSequence(
                DuplicatesInSequence.Find("123456789\u5000\u5001"));
        }

        [Test]
        public void StringFind_ReturnsDuplicates_ForDistinctElements()
        {
            TestHelper.AssertSequence(
                DuplicatesInSequence.Find("\u5000121312\u5000"),
                new KeyValuePair<char, int>('1', 3),
                new KeyValuePair<char, int>('2', 2),
                new KeyValuePair<char, int>('\u5000', 2));
        }
    }
}
