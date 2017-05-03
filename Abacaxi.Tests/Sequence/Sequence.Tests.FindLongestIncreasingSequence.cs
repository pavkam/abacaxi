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

// ReSharper disable ReturnValueOfPureMethodIsNotUsed

namespace Abacaxi.Tests.Sequence
{
    using System;
    using NUnit.Framework;
    using System.Collections.Generic;

    [TestFixture]
    public class FindLongestIncreasingSequence
    {
        [Test]
        public void FindLongestIncreasingSequence_ThrowsException_ForNullSequence()
        {
            Assert.Throws<ArgumentNullException>(() =>
                ((int[])null).FindLongestIncreasingSequence(Comparer<int>.Default));
        }

        [Test]
        public void FindLongestIncreasingSequence_ThrowsException_ForNullComparer()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new[] { 1 }.FindLongestIncreasingSequence(null));
        }

        [Test]
        public void FindLongestIncreasingSequence_ReturnsNothing_ForEmptySequence()
        {
            TestHelper.AssertSequence(
                new int[] {}.FindLongestIncreasingSequence(Comparer<int>.Default));
        }

        [Test]
        public void FindLongestIncreasingSequence_ReturnsSingleElement()
        {
            TestHelper.AssertSequence(
                new[] { 1 }.FindLongestIncreasingSequence(Comparer<int>.Default),
                1);
        }

        [Test]
        public void FindLongestIncreasingSequence_ReturnsAllElements_ForPerfectSequence()
        {
            TestHelper.AssertSequence(
                new[] { 1, 2, 3, 4, 5 }.FindLongestIncreasingSequence(Comparer<int>.Default),
                1, 2, 3, 4, 5);
        }

        [Test]
        public void FindLongestIncreasingSequence_SkipsAllTheJunk_ForANiceLongSequence()
        {
            TestHelper.AssertSequence(
                new[] { 1, 10, 2, 3, 0, 4, 5 }.FindLongestIncreasingSequence(Comparer<int>.Default),
                1, 2, 3, 4, 5);
        }

        [Test]
        public void FindLongestIncreasingSequence_ReturnsTheLastClosedSequence_IfTwoAvailable()
        {
            TestHelper.AssertSequence(
                new[] { 1, 4, 2, 5, 3 }.FindLongestIncreasingSequence(Comparer<int>.Default),
                1, 2, 5);
        }
    }
}
