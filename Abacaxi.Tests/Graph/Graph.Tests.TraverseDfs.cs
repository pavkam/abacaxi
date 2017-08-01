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
    public class GraphTraverseDfsTests
    {
        private static bool True<T>(Graph<T>.IDfsNode node)
        {
            return true;
        }

        [Test]
        public void TraverseDfs_ThrowsException_ForNullVisitationHandler()
        {
            var graph = new LiteralGraph("A>B", true);

            Assert.Throws<ArgumentNullException>(() => graph.TraverseDfs('A', null, True,  (node, dfsNode) => true));
        }

        [Test]
        public void TraverseDfs_ThrowsException_ForNullCompletionHandler()
        {
            var graph = new LiteralGraph("A>B", true);

            Assert.Throws<ArgumentNullException>(() => graph.TraverseDfs('A', True, null, (node, dfsNode) => true));
        }

        [Test]
        public void TraverseDfs_ThrowsException_ForNullCycleHandler()
        {
            var graph = new LiteralGraph("A>B", true);

            Assert.Throws<ArgumentNullException>(() => graph.TraverseDfs('A', True, True, null));
        }

        [Test]
        public void TraverseDfs_ThrowsException_ForInvalidVertex()
        {
            var graph = new LiteralGraph("A>B", true);

            Assert.Throws<InvalidOperationException>(() => graph.TraverseDfs('Z', True, True, (node, dfsNode) => true));
        }

        [TestCase("A-B,A-C", "A>B,A>C,>A")]
        [TestCase("A-B,C-D", "A>B,>A")]
        [TestCase("A-B,A-D,B-C,C-D,C-E", "C>D,C>E,B>C,A>B,>A")]
        [TestCase("A-B,A-C,A-D,B-E,B-F,E-G", "E>G,B>E,B>F,A>B,A>C,A>D,>A")]
        public void TraverseDfs_ReturnsProperSequence_ForUndirectedGraph(string relationships, string expected)
        {
            var graph = new LiteralGraph(relationships, true);
            var result = new List<string>();

            graph.TraverseDfs('A', True, node =>
            {
                Assert.IsNotNull(node);
                result.Add($"{node.Parent?.Vertex}>{node.Vertex}");
                return true;
            }, (from, to) => true);

            Assert.AreEqual(expected, string.Join(",", result));
        }

        [TestCase("A>A", ">A")]
        [TestCase("A>B,B>C,C>A", "B>C,A>B,>A")]
        [TestCase("A>B,C>A,C>B", "A>B,>A")]
        [TestCase("A>B,B>C,C>A,D>B,C>D", "C>D,B>C,A>B,>A")]
        public void TraverseDfs_ReturnsProperSequence_ForDirectedGraph(string relationships, string expected)
        {
            var graph = new LiteralGraph(relationships, true);
            var result = new List<string>();

            graph.TraverseDfs('A', True, node =>
            {
                Assert.IsNotNull(node);
                result.Add($"{node.Parent?.Vertex}>{node.Vertex}");
                return true;
            }, (from, to) => true);

            Assert.AreEqual(expected, string.Join(",", result));
        }

        [TestCase("A-B,A-C,A-D,B-E,B-F,E-G", 'A', "E>G,B>E,B>F,A>B,A>C,A>D,>A")]
        [TestCase("A-B,A-C,A-D,B-E,B-F,E-G", 'D', "E>G,B>E,B>F,A>B,A>C,A>D,>A")]
        [TestCase("A-B,A-C,A-D,B-E,B-F,E-G", 'E', "E>G,B>E,A>B,>A")]
        public void TraverseDfs_ReturnsProperSequence_IfInterrupted(string relationships, char killVertex, string expected)
        {
            var graph = new LiteralGraph(relationships, true);
            var result = new List<string>();

            graph.TraverseDfs('A', True, node =>
            {
                Assert.IsNotNull(node);
                result.Add($"{node.Parent?.Vertex}>{node.Vertex}");
                return node.Vertex != killVertex;
            }, (from, to) => true);

            Assert.AreEqual(expected, string.Join(",", result));
        }

        [TestCase("A-B,A-C", "B~A,C~A")]
        [TestCase("A-B,C-D", "B~A")]
        [TestCase("A-B,A-D,B-C,C-D,C-E", "B~A,C~B,D~A,D~C,E~C")]
        [TestCase("A-B,A-C,A-D,B-E,B-F,E-G", "B~A,E~B,G~E,F~B,C~A,D~A")]
        [TestCase("A>A", "A~A")]
        [TestCase("A>B,B>C,C>A", "C~A")]
        [TestCase("A>B,C>A,C>B", "")]
        [TestCase("A>B,B>C,C>A,D>B,C>D", "C~A,D~B")]
        [TestCase("A-B,B-C,C-D,D-A,B>D", "B~A,C~B,D~C,D~A")]
        public void TraverseDfs_ReportsTheCycles_InDirectedGraphs(string relationships, string expected)
        {
            var graph = new LiteralGraph(relationships, true);
            var result = new List<string>();

            graph.TraverseDfs('A', True, True, (from, to) =>
            {
                result.Add($"{from.Vertex}~{to.Vertex}");
                return true;
            });

            Assert.AreEqual(expected, string.Join(",", result));
        }

        [TestCase("A", "")]
        [TestCase("A,B,C", "")]
        [TestCase("A-B,A-C", "")]
        [TestCase("A-B,C-D", "")]
        [TestCase("A-B,A-D,B-C,C-D,C-E", "D~A")]
        [TestCase("A-B,A-C,A-D,B-E,B-F,E-G", "")]
        public void TraverseDfs_ReportsTheCycles_InUndirectedGraphs(string relationships, string expected)
        {
            var graph = new LiteralGraph(relationships, false);
            var result = new List<string>();

            graph.TraverseDfs('A', True, True, (from, to) =>
            {
                result.Add($"{from.Vertex}~{to.Vertex}");
                return true;
            });

            Assert.AreEqual(expected, string.Join(",", result));
        }

        [TestCase("A>A", 'A', ">A")]
        [TestCase("A>B,B>C,C>A", 'A', "B>C,A>B,>A")]
        [TestCase("A>B,B>C,C>A,D>B,C>D", 'A', "B>C,A>B,>A")]
        [TestCase("A-B,B-C,C-D,D-A,B>D", 'B', "B>C,A>B,>A")]
        public void TraverseDfs_InterruptsOnCycle(string relationships, char killVertex, string expected)
        {
            var graph = new LiteralGraph(relationships, true);
            var result = new List<string>();

            graph.TraverseDfs('A', True, node =>
            {
                Assert.IsNotNull(node);
                result.Add($"{node.Parent?.Vertex}>{node.Vertex}");
                return true;
            }, 
            (from, to) => to.Vertex != killVertex);

            Assert.AreEqual(expected, string.Join(",", result));
        }


        [TestCase("A-B,B-D,D-F,F-A,F-Z,A-C,C-E,E-A,E-G,G-H,H-E", "Z4,F3,D2,B1,H12,G11,E10,C9,A0")]
        public void TraverseDfs_MarksNodesWithCorrectEntryTimes(string relationships, string expected)
        {
            var graph = new LiteralGraph(relationships, true);
            var result = new List<string>();

            graph.TraverseDfs('A', True, node =>
            {
                result.Add($"{node.Vertex}{node.EntryTime}");
                return true;
            },
            (from, to) => true);

            Assert.AreEqual(expected, string.Join(",", result));
        }

        [TestCase("A-B,B-D,D-F,F-A,F-Z,A-C,C-E,E-A,E-G,G-H,H-E", "Z5,F6,D7,B8,H13,G14,E15,C16,A17")]
        public void TraverseDfs_MarksNodesWithCorrectExitTimes(string relationships, string expected)
        {
            var graph = new LiteralGraph(relationships, true);
            var result = new List<string>();

            graph.TraverseDfs('A', True, node =>
            {
                result.Add($"{node.Vertex}{node.ExitTime}");
                return true;
            },
            (from, to) => true);

            Assert.AreEqual(expected, string.Join(",", result));
        }
    }
}
