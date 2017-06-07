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
    using Graphs;
    using NUnit.Framework;

    [TestFixture]
    public class GraphDescribeVerticesTests
    {
        [TestCase("", "")]
        [TestCase("A", "0 => A (0) => 0")]
        [TestCase("A-A", "1 => A (0) => 1")]
        [TestCase("A-A,A-A", "2 => A (0) => 2")]
        [TestCase("A-B,B-C,C-D", "1 => A (0) => 1; 2 => B (0) => 2; 2 => C (0) => 2; 1 => D (0) => 1")]
        [TestCase("A,B,C", "0 => A (0) => 0; 0 => B (1) => 0; 0 => C (2) => 0")]
        public void Graph_DescribeVertices_ReturnsExpectedDescriptions_ForUndirectedGraphs(string relationships, string expected)
        {
            var graph = new LiteralGraph(relationships, false);
            var result = string.Join("; ", graph.DescribeVertices());

            Assert.AreEqual(expected, result);
        }

        [TestCase("", "")]
        [TestCase("A", "0 => A (0) => 0")]
        [TestCase("A>A", "1 => A (0) => 1")]
        [TestCase("A>A,A>A", "2 => A (0) => 2")]
        [TestCase("A,B,C", "0 => A (0) => 0; 0 => B (1) => 0; 0 => C (2) => 0")]
        [TestCase("A>B,B-C,C>D", "0 => A (0) => 1; 2 => B (0) => 1; 1 => C (0) => 2; 1 => D (0) => 0")]
        [TestCase("A>B,C>D", "0 => A (0) => 1; 1 => B (0) => 0; 0 => C (1) => 1; 1 => D (1) => 0")]
        public void Graph_DescribeVertices_ReturnsExpectedDescriptions_ForDirectedGraphs(string relationships, string expected)
        {
            var graph = new LiteralGraph(relationships, true);
            var result = string.Join("; ", graph.DescribeVertices());

            Assert.AreEqual(expected, result);
        }
    }
}
