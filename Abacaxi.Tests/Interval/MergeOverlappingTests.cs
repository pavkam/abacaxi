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

namespace Abacaxi.Tests.Interval
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using NUnit.Framework;
    using Interval = Abacaxi.Interval;

    [TestFixture]
    public sealed class MergeOverlappingTests
    {
        [TestCase(1, 5, 1, 9, 1, 9), TestCase(1, 5, 3, 4, 1, 5), TestCase(1, 5, 5, 6, 1, 6), TestCase(5, 9, 2, 6, 2, 9)]
        public void MergeOverlapping_ReturnsTheOverlappedInterval(int s1, int e1, int s2, int e2, int sm, int em)
        {
            var result = Interval.MergeOverlapping(new[] {(s1, e1), (s2, e2)}, Comparer<int>.Default);

            TestHelper.AssertSequence(result, (sm, em));
        }

        [Test]
        public void MergeOverlapping_MergesIntermediateOnes()
        {
            var result = Interval.MergeOverlapping(new[] {(1, 10), (11, 20), (5, 15)}, Comparer<int>.Default);

            TestHelper.AssertSequence(result, (1, 20));
        }

        [Test]
        public void MergeOverlapping_MergesSome_LeavesOthers()
        {
            var result = Interval.MergeOverlapping(new[] {(6, 9), (9, 11), (1, 5), (9, 9)}, Comparer<int>.Default);

            TestHelper.AssertSequence(result, (1, 5), (6, 11));
        }

        [Test]
        public void MergeOverlapping_ReturnsNothing_ForEmptyInput()
        {
            var result = Interval.MergeOverlapping(new (int, int)[] { }, Comparer<int>.Default);

            TestHelper.AssertSequence(result);
        }

        [Test]
        public void MergeOverlapping_ReturnsTheSameInterval_IfOnlyOne()
        {
            var result = Interval.MergeOverlapping(new[] {(1, 2)}, Comparer<int>.Default);

            TestHelper.AssertSequence(result, (1, 2));
        }

        [Test]
        public void MergeOverlapping_ReturnsTheSameIntervals_IfNothingOverlaps()
        {
            var result = Interval.MergeOverlapping(new[] {(3, 4), (1, 2), (5, 50)}, Comparer<int>.Default);

            TestHelper.AssertSequence(result, (1, 2), (3, 4), (5, 50));
        }

        [Test, SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void MergeOverlapping_ThrowsException_ForNullEqualityComparer()
        {
            Assert.Throws<ArgumentNullException>(() => Interval.MergeOverlapping(null, Comparer<int>.Default));
        }

        [Test, SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void MergeOverlapping_ThrowsException_ForNullSequence()
        {
            Assert.Throws<ArgumentNullException>(() => Interval.MergeOverlapping(new[] {(1, 2)}, null));
        }

        [Test]
        public void MergeOverlapping_ThrowsException_IfAnyIntervalIsInvalid()
        {
            Assert.Throws<InvalidOperationException>(() => Interval.MergeOverlapping(new[] {(2, 1)}, Comparer<int>.Default));
        }
    }
}