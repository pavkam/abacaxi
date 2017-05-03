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

// ReSharper disable SuspiciousTypeConversion.Global
// ReSharper disable ReturnValueOfPureMethodIsNotUsed

namespace Abacaxi.Tests.Graph
{
    using System;
    using System.Linq;
    using Graphs;
    using NUnit.Framework;

    [TestFixture]
    public class GraphTopologicalSortTests
    {
        [Test]
        public void TopologicalSort_ThrowsException_ForUndirectedGraph()
        {
            var graph = new LiteralGraph("A-B", false);
            Assert.Throws<InvalidOperationException>(() => graph.TopologicalSort().ToArray());
        }

        [TestCase("A-B")]
        [TestCase("A>B,B>A")]
        [TestCase("A>B,B>C,C>A")]
        public void TopologicalSort_ThrowsException_ForDirectedGraphWith(string relationships)
        {
            var graph = new LiteralGraph(relationships, true);
            Assert.Throws<InvalidOperationException>(() => graph.TopologicalSort().ToArray());
        }

        [TestCase("A", "A")]
        [TestCase("A>B", "A,B")]
        [TestCase("A>B,B>C", "A,B,C")]
        [TestCase("A>B,B>C,A>C", "A,B,C")]
        [TestCase("A,B,C,C>D", "A,B,C,D")]
        [TestCase("A>B,C>B,B>D,B>E,E>D,F", "A,C,F,B,E,D")]
        public void TopologicalSort_AlignsTheVerticesAsExpected(string relationships, string expected)
        {
            var graph = new LiteralGraph(relationships, true);
            var result = graph.TopologicalSort().ToList();
            var actual = string.Join(",", result);

            Assert.AreEqual(expected, actual);
        }
    }
}
