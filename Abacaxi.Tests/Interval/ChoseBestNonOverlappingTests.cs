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
    public sealed class ChoseBestNonOverlappingTests
    {
        [Test]
        public void ChoseBestNonOverlapping_ReturnsNothing_ForEmptyInput()
        {
            var result = Interval.ChoseBestNonOverlapping(new (int, int, double)[] { }, Comparer<int>.Default);

            TestHelper.AssertSequence(result);
        }

        [Test]
        public void ChoseBestNonOverlapping_ReturnsTheSameInterval_IfOnlyOne()
        {
            var result = Interval.ChoseBestNonOverlapping(new[] {(1, 2, .0)}, Comparer<int>.Default);

            TestHelper.AssertSequence(result, (1, 2));
        }

        [Test]
        public void ChoseBestNonOverlapping_ReturnsTheSameIntervals_IfNothingOverlaps()
        {
            var result = Interval.ChoseBestNonOverlapping(new[] {(3, 4, 1.0), (1, 2, 1.0), (5, 50, 1.0)},
                Comparer<int>.Default);

            TestHelper.AssertSequence(result, (1, 2), (3, 4), (5, 50));
        }

        [Test]
        public void ChoseBestNonOverlapping_SelectsTheBestScoredInterval_1()
        {
            var result = Interval.ChoseBestNonOverlapping(new[] {(1, 2, 1.0), (1, 3, 2.0)}, Comparer<int>.Default);

            TestHelper.AssertSequence(result, (1, 3));
        }

        [Test]
        public void ChoseBestNonOverlapping_SelectsTheBestScoredInterval_2()
        {
            var result = Interval.ChoseBestNonOverlapping(new[] {(1, 2, 2.0), (1, 3, 1.0)}, Comparer<int>.Default);

            TestHelper.AssertSequence(result, (1, 2));
        }

        [Test]
        public void ChoseBestNonOverlapping_SelectsTheBestScoredInterval_3()
        {
            var result =
                Interval.ChoseBestNonOverlapping(new[] {(1, 2, 2.0), (1, 3, 1.0), (2, 3, 3.0)}, Comparer<int>.Default);

            TestHelper.AssertSequence(result, (2, 3));
        }

        [Test]
        public void ChoseBestNonOverlapping_SelectsTheBestScoredInterval_4()
        {
            var result = Interval.ChoseBestNonOverlapping(new[] {(1, 2, 1.0), (3, 4, 1.0), (5, 6, 1.0), (3, 5, 1.0)},
                Comparer<int>.Default);

            TestHelper.AssertSequence(result, (1, 2), (3, 4), (5, 6));
        }

        [Test]
        public void ChoseBestNonOverlapping_SelectsTheBestScoredInterval_5()
        {
            var result = Interval.ChoseBestNonOverlapping(
                new[] {(1, 1, 2.0), (1, 2, 1.0), (3, 3, 1.0), (3, 5, 5.0), (5, 5, 5.0)}, Comparer<int>.Default);

            TestHelper.AssertSequence(result, (1, 1), (3, 3), (5, 5));
        }

        [Test, SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void ChoseBestNonOverlapping_ThrowsException_ForNullEqualityComparer()
        {
            Assert.Throws<ArgumentNullException>(() => Interval.ChoseBestNonOverlapping(null, Comparer<int>.Default));
        }

        [Test, SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void ChoseBestNonOverlapping_ThrowsException_ForNullSequence()
        {
            Assert.Throws<ArgumentNullException>(() => Interval.ChoseBestNonOverlapping(new[] {(1, 2, .0)}, null));
        }

        [Test]
        public void ChoseBestNonOverlapping_ThrowsException_IfAnyIntervalIsInvalid()
        {
            Assert.Throws<InvalidOperationException>(() =>
                Interval.ChoseBestNonOverlapping(new[] {(2, 1, .0)}, Comparer<int>.Default));
        }
    }
}