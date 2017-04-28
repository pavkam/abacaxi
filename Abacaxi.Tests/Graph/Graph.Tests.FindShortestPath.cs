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
    public class GraphFindShortestPathTests
    {
        [Test]
        public void FindShortestPath_ThrowsException_ForInvalidStartVertex()
        {
            var graph = new LiteralGraph("A>B");
            Assert.Throws<InvalidOperationException>(() => graph.FindShortestPath('Z', 'A').ToArray());
        }

        [TestCase("A-B,A-C", 'A', 'B', "A,B")]
        [TestCase("A-B,B-C,C-D,D-A,B>D", 'D', 'B', "D,C,B")]
        [TestCase("A-B,B-C,C-D,D-A,B>D", 'B', 'D', "B,D")]
        [TestCase("A>B,A>C,C<F,F-E,E-D,D>B,D>C", 'A', 'E', "")]
        [TestCase("A>B,A>C,C<F,F-E,E-D,D>B,D>C", 'E', 'Z', "")]
        public void FindShortestPath_FillsExpectedVertices(
            string relationships, char startVertex, char endVertex, string expected)
        {
            var graph = new LiteralGraph(relationships);
            var seq = graph.FindShortestPath(startVertex, endVertex);

            Assert.AreEqual(expected, string.Join(",", seq));
        }
    }
}
