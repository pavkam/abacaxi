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

namespace Abacaxi.Tests.Set
{
    using System;
    using System.Collections.Generic;
    using NUnit.Framework;
    using System.Diagnostics.CodeAnalysis;

    [TestFixture]
    public class SplitIntoSubsetsOfEqualValueTests
    {
        private static int IntegerAggregator(int a, int b)
        {
            return a + b;
        }

        [Test]
        [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void SplitIntoSubsetsOfEqualValue_ThrowsException_IfSequenceIsNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
                Abacaxi.Set.SplitIntoSubsetsOfEqualValue(null, IntegerAggregator, Comparer<int>.Default, 1));
        }

        [Test]
        [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void SplitIntoSubsetsOfEqualValue_ThrowsException_IfAggregatorIsNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
                Abacaxi.Set.SplitIntoSubsetsOfEqualValue(new[] {1}, null, Comparer<int>.Default, 1));
        }

        [Test]
        [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void SplitIntoSubsetsOfEqualValue_ThrowsException_IfComparerIsNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
                Abacaxi.Set.SplitIntoSubsetsOfEqualValue(new[] {1}, IntegerAggregator, null, 1));
        }

        [Test]
        public void SplitIntoSubsetsOfEqualValue_ThrowsException_IsCountOfPartitionsLessThanOne()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                Abacaxi.Set.SplitIntoSubsetsOfEqualValue(new[] {1}, IntegerAggregator, Comparer<int>.Default, 0));
        }

        [Test]
        public void SplitIntoSubsetsOfEqualValue_ReturnsNothing_ForEmptySequence()
        {
            var array = new int[] { };
            TestHelper.AssertSequence(
                Abacaxi.Set.SplitIntoSubsetsOfEqualValue(array, IntegerAggregator, Comparer<int>.Default, 1)
            );
        }

        [Test]
        public void SplitIntoSubsetsOfEqualValue_ReturnsEverything_IfOnlyOnePartition()
        {
            var array = new[] {1, 2, 3, 4, 5};
            TestHelper.AssertSequence(
                Abacaxi.Set.SplitIntoSubsetsOfEqualValue(array, IntegerAggregator, Comparer<int>.Default, 1),
                new[] {1, 2, 3, 4, 5});
        }

        [Test]
        public void SplitIntoSubsetsOfEqualValue_ReturnsTwoPartitions_IfPossible()
        {
            var array = new[] {2, 1, 3, 2};
            TestHelper.AssertSequence(
                Abacaxi.Set.SplitIntoSubsetsOfEqualValue(array, IntegerAggregator, Comparer<int>.Default, 2),
                new[] {2, 2},
                new[] {1, 3}
            );
        }

        [Test]
        public void SplitIntoSubsetsOfEqualValue_ReturnsThreePartitions_IfPossible()
        {
            var array = new[] {2, 1, 3, 4, 5};
            TestHelper.AssertSequence(
                Abacaxi.Set.SplitIntoSubsetsOfEqualValue(array, IntegerAggregator, Comparer<int>.Default, 3),
                new[] {2, 3},
                new[] {1, 4},
                new[] {5}
            );
        }

        [Test]
        public void SplitIntoSubsetsOfEqualValue_ReturnsNothing_IfThreePartitionsNotPossible()
        {
            var array = new[] {2, 1, 3, 2, 5};
            TestHelper.AssertSequence(
                Abacaxi.Set.SplitIntoSubsetsOfEqualValue(array, IntegerAggregator, Comparer<int>.Default, 3)
            );
        }

        [Test]
        public void SplitIntoSubsetsOfEqualValue_ReturnsEmptyArrayAndAll_ForNegativeZeroing()
        {
            var array = new[] {-1, 1};
            TestHelper.AssertSequence(
                Abacaxi.Set.SplitIntoSubsetsOfEqualValue(array, IntegerAggregator, Comparer<int>.Default, 2),
                new[] {-1, 1},
                new int[] { }
            );
        }
    }
}