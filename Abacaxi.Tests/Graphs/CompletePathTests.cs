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

namespace Abacaxi.Tests.Graphs
{
    using System;
    using System.Linq;
    using Abacaxi.Graphs;
    using NUnit.Framework;

    [TestFixture]
    public class CompletePathTests
    {
        [Test]
        public void FindHeuristical_ThrowsException_ForNullSequence()
        {
            Assert.Throws<ArgumentNullException>(() =>
                CompletePath.FindHeuristical((IntegerSequenceGraph)null).ToArray());
        }
        
        [Test]
        public void FindHeuristical_ReturnsNothing_ForAnEmptyGraph()
        {
            var graph = new IntegerSequenceGraph(new int[] { });
            TestHelper.AssertSequence(
                CompletePath.FindHeuristical(graph));
        }

        [Test]
        public void FindHeuristical_ReturnsOne_ForASequenceOfOne()
        {
            var graph = new IntegerSequenceGraph(new int[] { 1 });
            TestHelper.AssertSequence(
                CompletePath.FindHeuristical(graph),
                0);
        }

        [Test]
        public void FindHeuristical_BehavesAsExpected_ForSegment()
        {
            var graph = new IntegerSequenceGraph(new int[] { 1, 2 });
            TestHelper.AssertSequence(
                CompletePath.FindHeuristical(graph),
                0, 1);
        }

        [Test]
        public void FindHeuristical_BehavesAsExpected_ForTriangle1()
        {
            var graph = new IntegerSequenceGraph(new int[] { 1, 2, 5 });
            TestHelper.AssertSequence(
                CompletePath.FindHeuristical(graph),
                0, 1, 2);
        }

        [Test]
        public void FindHeuristical_BehavesAsExpected_ForTriangle2()
        {
            var graph = new IntegerSequenceGraph(new int[] { 6, 1, 10, 2, 11 });
            TestHelper.AssertSequence(
                CompletePath.FindHeuristical(graph),
                0, 2, 4, 1, 3);
        }
    }
}
