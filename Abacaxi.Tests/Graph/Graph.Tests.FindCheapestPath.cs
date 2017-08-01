﻿/* Copyright 2017 by Alexandru Ciobanu (alex+git@ciobanu.org)
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

namespace Abacaxi.Tests.Graph
{
    using System;
    using Graphs;
    using NUnit.Framework;

    [TestFixture]
    public class GraphFindCheapestPathTests
    {
        [Test]
        public void FindCheapestPath_ThrowsException_IfFromVertexIsInvalid()
        {
            var graph = new LiteralWeightedGraph("A-1-B", false);
            Assert.Throws<ArgumentException>(() => graph.FindCheapestPath('Z', 'A'));
        }

        [TestCase("A", 'A', 'A', "A")]
        [TestCase("A-1-B", 'A', 'A', "A")]
        [TestCase("A,B", 'A', 'A', "A")]
        [TestCase("A,B", 'A', 'B', "")]
        [TestCase("A-1-B", 'A', 'B', "A,B")]
        [TestCase("A-3-B,A-1-C,C-1-B", 'A', 'B', "A,C,B")]
        public void FindCheapestPath_FindsTheProperPath_ForUndirectedGraphs(string relationships, char from, char to, string expected)
        {
            var graph = new LiteralWeightedGraph(relationships, false);
            var actual = string.Join(",", graph.FindCheapestPath(from, to));

            Assert.AreEqual(expected, actual);
        }

        [TestCase("A", 'A', 'A', "A")]
        [TestCase("A-1-B", 'A', 'A', "A")]
        [TestCase("A,B", 'A', 'A', "A")]
        [TestCase("A,B", 'A', 'B', "")]
        [TestCase("A-1-B", 'A', 'B', "A,B")]
        [TestCase("A-3-B,A-1-C,C-1-B", 'A', 'B', "A,C,B")]
        [TestCase("A<1<B,A>5>C,C>5>B", 'A', 'B', "A,C,B")]
        [TestCase("A>1>B", 'A', 'B', "A,B")]
        [TestCase("B>1>A", 'A', 'B', "")]
        public void FindCheapestPath_FindsTheProperPath_ForDirectedGraphs(string relationships, char from, char to, string expected)
        {
            var graph = new LiteralWeightedGraph(relationships, true);
            var actual = string.Join(",", graph.FindCheapestPath(from, to));

            Assert.AreEqual(expected, actual);
        }
    }
}
