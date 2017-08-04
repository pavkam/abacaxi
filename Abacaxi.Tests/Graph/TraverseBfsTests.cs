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

namespace Abacaxi.Tests.Graph
{
    using System;
    using System.Collections.Generic;
    using Graphs;
    using NUnit.Framework;

    [TestFixture]
    public class TraverseBfsTests
    {
        [Test]
        public void TraverseBfs_ThrowsException_ForNullCompletionHandler()
        {
            var graph = new LiteralGraph("A>1>B", true);

            Assert.Throws<ArgumentNullException>(() => graph.TraverseBfs('A', null));
        }

        [Test]
        public void TraverseBfs_ThrowsException_ForInvalidVertex()
        {
            var graph = new LiteralGraph("A>1>B", true);

            Assert.Throws<ArgumentException>(() => graph.TraverseBfs('Z', node => true));
        }

        [TestCase("A-1-B,A-1-C",">A,A>B,A>C")]
        [TestCase("A-1-B,C-1-D", ">A,A>B")]
        [TestCase("A-1-B,A-1-D,B-1-C,C-1-D,C-1-E", ">A,A>B,A>D,B>C,C>E")]
        [TestCase("A-1-B,A-1-C,A-1-D,B-1-E,B-1-F,E-1-G", ">A,A>B,A>C,A>D,B>E,B>F,E>G")]
        public void TraverseBfs_ReturnsProperSequence_ForUndirectedGraph(string relationships, string expected)
        {
            var graph = new LiteralGraph(relationships, true);
            var result = new List<string>();

            graph.TraverseBfs('A', node =>
            {
                Assert.IsNotNull(node);
                result.Add($"{node.Parent?.Vertex}>{node.Vertex}");
                return true;
            });

            Assert.AreEqual(expected, string.Join(",", result));
        }

        [TestCase("A>1>A", ">A")]
        [TestCase("A>1>B,B>1>C,C>1>A", ">A,A>B,B>C")]
        [TestCase("A>1>B,C>1>A,C>1>B", ">A,A>B")]
        [TestCase("A>1>B,B>1>C,C>1>A,D>1>B,C>1>D", ">A,A>B,B>C,C>D")]
        public void TraverseBfs_ReturnsProperSequence_ForDirectedGraph(string relationships, string expected)
        {
            var graph = new LiteralGraph(relationships, true);
            var result = new List<string>();

            graph.TraverseBfs('A', node =>
            {
                Assert.IsNotNull(node);
                result.Add($"{node.Parent?.Vertex}>{node.Vertex}");
                return true;
            });

            Assert.AreEqual(expected, string.Join(",", result));
        }

        [TestCase("A-1-B,A-1-C,A-1-D,B-1-E,B-1-F,E-1-G", 'A', ">A")]
        [TestCase("A-1-B,A-1-C,A-1-D,B-1-E,B-1-F,E-1-G", 'D', ">A,A>B,A>C,A>D")]
        [TestCase("A-1-B,A-1-C,A-1-D,B-1-E,B-1-F,E-1-G", 'G', ">A,A>B,A>C,A>D,B>E,B>F,E>G")]
        public void TraverseBfs_ReturnsProperSequence_IfInterrupted(string relationships, char killVertex, string expected)
        {
            var graph = new LiteralGraph(relationships, true);
            var result = new List<string>();

            graph.TraverseBfs('A', node =>
            {
                Assert.IsNotNull(node);
                result.Add($"{node.Parent?.Vertex}>{node.Vertex}");
                return node.Vertex != killVertex;
            });

            Assert.AreEqual(expected, string.Join(",", result));
        }
    }
}
