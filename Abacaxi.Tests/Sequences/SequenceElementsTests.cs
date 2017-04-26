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
    using System.Collections.Generic;
    using Abacaxi.Sequences;
    using NUnit.Framework;

    [TestFixture]
    public class SequenceElementsTests
    {
        private int StdAgg(int a, int b)
        {
            return a + b;
        }

        [Test]
        public void FindPartitionsEqualByAggregate_ThrowsException_IfSequenceIsNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
                SequenceElements.FindPartitionsEqualByAggregate((int[])null, 1, StdAgg, Comparer<int>.Default).ToArray());
        }

        [Test]
        public void FindPartitionsEqualByAggregate_ThrowsException_IfAggregatorIsNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
                SequenceElements.FindPartitionsEqualByAggregate(new int[] { 1 }, 1, null, Comparer<int>.Default).ToArray());
        }

        [Test]
        public void FindPartitionsEqualByAggregate_ThrowsException_IfComparerIsNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
                SequenceElements.FindPartitionsEqualByAggregate(new int[] { 1 }, 1, StdAgg, null).ToArray());
        }

        [Test]
        public void FindPartitionsEqualByAggregate_ThrowsException_IsCountOfPartitionsLessThanOne()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                SequenceElements.FindPartitionsEqualByAggregate(new int[] { 1 }, 0, StdAgg, Comparer<int>.Default).ToArray());
        }

        [Test]
        public void FindPartitionsEqualByAggregate_ReturnsNothing_ForEmptySequence()
        {
            var array = new int[] { };
            TestHelper.AssertSequence(
                SequenceElements.FindPartitionsEqualByAggregate(array, 1, StdAgg, Comparer<int>.Default)
                );
        }

        [Test]
        public void FindPartitionsEqualByAggregate_ReturnsEverything_IfOnlyOnePartition()
        {
            var array = new int[] { 1, 2, 3, 4, 5 };
            TestHelper.AssertSequence(
                SequenceElements.FindPartitionsEqualByAggregate(array, 1, StdAgg, Comparer<int>.Default),
                new [] { 1, 2, 3, 4, 5 } );
        }

        [Test]
        public void FindPartitionsEqualByAggregate_ReturnsTwoPartitions_IfPossible()
        {
            var array = new int[] { 2, 1, 3, 2 };
            TestHelper.AssertSequence(
                SequenceElements.FindPartitionsEqualByAggregate(array, 2, StdAgg, Comparer<int>.Default),
                new[] { 2, 2 },
                new[] { 1, 3 });
        }

        [Test]
        public void FindPartitionsEqualByAggregate_ReturnsThreePartitions_IfPossible()
        {
            var array = new int[] { 2, 1, 3, 2, 4 };
            TestHelper.AssertSequence(
                SequenceElements.FindPartitionsEqualByAggregate(array, 3, StdAgg, Comparer<int>.Default),
                new[] { 2, 2 },
                new[] { 1, 3 },
                new[] { 4 });
        }

        [Test]
        public void FindPartitionsEqualByAggregate_ReturnsNothing_IfThreePartitionsNotPossible()
        {
            var array = new int[] { 2, 1, 3, 2, 5 };
            TestHelper.AssertSequence(
                SequenceElements.FindPartitionsEqualByAggregate(array, 3, StdAgg, Comparer<int>.Default)
                );
        }
    }
}
