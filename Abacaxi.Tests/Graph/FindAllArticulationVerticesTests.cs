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
    using Graphs;
    using NUnit.Framework;

    [TestFixture]
    public class FindAllArticulationVerticesTests
    {
        [TestCase("A", "")]
        [TestCase("A-1-B", "")]
        [TestCase("A,B,C", "")]
        [TestCase("A-1-B,A-1-C", "A")]
        [TestCase("A-1-B,A-1-C,A-1-D,B-1-E,B-1-F,E-1-G", "E,B,A")]
        [TestCase("A-1-B,B-1-D,D-1-F,F-1-A,F-1-Z,A-1-C,C-1-E,E-1-A,E-1-G,G-1-H,H-1-E", "F,E,A")]
        public void FindAllArticulationVertices_FindsArticulationsForUndirectedGraphs(string relationships, string expected)
        {
            var graph = new LiteralGraph(relationships, false);
            var actual = string.Join(",", graph.FindAllArticulationVertices());

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void FindAllArticulationVertices_ThrowsException_ForDirectedGraphs()
        {
            var graph = new LiteralGraph("A>1>B", true);
            Assert.Throws<InvalidOperationException>(() => graph.FindAllArticulationVertices());
        }
    }
}
