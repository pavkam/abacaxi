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
    using System.Diagnostics.CodeAnalysis;

    [TestFixture]
    public class TopologicalSortTests
    {
        [Test]
        [SuppressMessage("ReSharper", "IteratorMethodResultIsIgnored")]
        public void TopologicalSort_ThrowsException_ForUndirectedGraph()
        {
            var graph = new LiteralGraph("A-1-B", false);
            Assert.Throws<InvalidOperationException>(() => graph.TopologicalSort());
        }

        [TestCase("A-1-B")]
        [TestCase("A>1>B,B>1>A")]
        [TestCase("A>1>B,B>1>C,C>1>A")]
        [SuppressMessage("ReSharper", "IteratorMethodResultIsIgnored")]
        public void TopologicalSort_ThrowsException_ForDirectedGraphWith(string relationships)
        {
            var graph = new LiteralGraph(relationships, true);
            Assert.Throws<InvalidOperationException>(() => graph.TopologicalSort());
        }

        [TestCase("", "")]
        [TestCase("A", "A")]
        [TestCase("A>1>B", "A,B")]
        [TestCase("A>1>B,B>1>C", "A,B,C")]
        [TestCase("A>1>B,B>1>C,A>1>C", "A,B,C")]
        [TestCase("A,B,C,C>1>D", "A,B,C,D")]
        [TestCase("A>1>B,C>1>B,B>1>D,B>1>E,E>1>D,F", "A,C,F,B,E,D")]
        public void TopologicalSort_AlignsTheVerticesAsExpected(string relationships, string expected)
        {
            var graph = new LiteralGraph(relationships, true);
            var result = graph.TopologicalSort().ToList();
            var actual = string.Join(",", result);

            Assert.AreEqual(expected, actual);
        }
    }
}
