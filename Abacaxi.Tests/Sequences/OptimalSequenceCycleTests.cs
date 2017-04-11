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
    using System.Collections.Generic;
    using System.Linq;
    using Abacaxi.Sequences;
    using NUnit.Framework;

    [TestFixture]
    public class OptimalSequenceCycleTests
    {
        private int StdCost(int a, int b)
        {
            return Math.Abs(b - a);
        }

        private int BadCost(int a, int b)
        {
            return -1;
        }

        [Test]
        public void Find_ThrowsException_ForNullSequence()
        {
            Assert.Throws<ArgumentNullException>(() =>
                OptimalSequenceCycle.Find((int[])null, StdCost).ToArray());
        }

        [Test]
        public void Find_ThrowsException_ForNullCostFunction()
        {
            Assert.Throws<ArgumentNullException>(() =>
                OptimalSequenceCycle.Find(new int[] { }, null).ToArray());
        }

        [Test]
        public void Find_ThrowsException_ForInvalidCostFunction()
        {
            Assert.Throws<InvalidOperationException>(() =>
                OptimalSequenceCycle.Find(new int[] { 1, 2 }, BadCost).ToArray());
        }

        [Test]
        public void Find_ResturnsNothing_ForAnEmptySequence()
        {
            TestHelper.AssertSequence(
                OptimalSequenceCycle.Find(new int[] { }, StdCost));
        }

        [Test]
        public void Find_ResturnsOne_ForASequenceOfOne()
        {
            TestHelper.AssertSequence(
                OptimalSequenceCycle.Find(new[] { 1 }, StdCost),
                1);
        }

        [Test]
        public void Find_ResturnsOneTwo_ForASequenceOfOneTwo()
        {
            TestHelper.AssertSequence(
                OptimalSequenceCycle.Find(new[] { 1, 2 }, StdCost),
                1, 2);
        }

        [Test]
        public void Find_ReturnsZeroThreeFive_ForZeroThreeFive()
        {
            TestHelper.AssertSequence(
                OptimalSequenceCycle.Find(new[] { 0, 3, 5 }, StdCost),
                0, 3, 5);
        }

        [Test]
        public void Find_SortsAnIntegerSequence_UsingStdCost()
        {
            TestHelper.AssertSequence(
                OptimalSequenceCycle.Find(new[] { 5, 0, 2, 3, 1, 1, 4 }, StdCost),
                0, 1, 1, 2, 3, 5, 4);
        }
    }
}
