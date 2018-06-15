/* Copyright 2017-2018 by Alexandru Ciobanu (alex+git@ciobanu.org)
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
    using System.Diagnostics.CodeAnalysis;
    using Abacaxi.Graphs;
    using JetBrains.Annotations;
    using NUnit.Framework;

    [TestFixture]
    public sealed class SubGraphTests
    {
        [TestCase("A", 'A', ""), TestCase("AB", 'A', "A >=1=> B"), TestCase("ABC", 'B', "B >=1=> A, B >=2=> C")]
        public void GetEdges_ReturnsOnlySuppliedVertices([NotNull] string vertices, char vertex, string expected)
        {
            var graph = new LiteralGraph("A-1-B,B-2-C,C-3-D,D-4-A,D<5<B", true);
            var sub = new SubGraph<char>(graph, vertices);
            var actual = string.Join(", ", sub.GetEdges(vertex));

            Assert.AreEqual(expected, actual);
        }

        [TestCase("A", "A"), TestCase("AB", "A,B"), TestCase("ABC", "A,B,C")]
        public void Enumeration_ReturnsOnlyIncludedVertices([NotNull] string vertices, string expected)
        {
            var graph = new LiteralGraph("A-1-B,B-1-C,C-1-D,D-1-A,D<1<B", true);
            var sub = new SubGraph<char>(graph, vertices);
            var actual = string.Join(",", sub);

            Assert.AreEqual(expected, actual);
        }

        [Test, SuppressMessage("ReSharper", "ObjectCreationAsStatement"),
         SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void Ctor_ThrowsException_ForNullGraph()
        {
            Assert.Throws<ArgumentNullException>(() => new SubGraph<int>(null, new[] {1}));
        }

        [Test, SuppressMessage("ReSharper", "ObjectCreationAsStatement"),
         SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void Ctor_ThrowsException_ForNullVerticesSequence()
        {
            Assert.Throws<ArgumentNullException>(() => new SubGraph<char>(new LiteralGraph("A-1-B", false), null));
        }

        [Test, SuppressMessage("ReSharper", "IteratorMethodResultIsIgnored")]
        public void GetEdges_ThrowsException_IfVertexNotPartOfSubGraph()
        {
            var graph = new LiteralGraph("A-1-B,B-1-C,C-1-D,D-1-A,D<1<B", true);
            var sub = new SubGraph<char>(graph, "AB");

            Assert.Throws<ArgumentException>(() => sub.GetEdges('C'));
        }

        [Test]
        public void GetPotentialWeight_ReturnsExpectedWeight_IfBackingGraphSupportsIt()
        {
            var graph = new MazeGraph(new[,] {{true, true}, {true, true}});
            var sub = new SubGraph<Cell>(graph, graph);

            Assert.AreEqual(2, sub.GetPotentialWeight(new Cell(0, 0), new Cell(1, 1)));
        }

        [Test]
        public void GetPotentialWeight_ThrowsException_IfBackingGraphDoesNotSupportIt()
        {
            var graph = new LiteralGraph("A>1>B", true);
            var sub = new SubGraph<char>(graph, new[] {'A', 'B'});

            Assert.Throws<NotSupportedException>(() => sub.GetPotentialWeight('A', 'B'));
        }

        [Test, SuppressMessage("ReSharper", "IteratorMethodResultIsIgnored")]
        public void GetPotentialWeight_ThrowsException_IfVerticesNotPartOfSubGraph()
        {
            var graph = new LiteralGraph("A-1-B,B-1-C,C-1-D,D-1-A,D<1<B", true);
            var sub = new SubGraph<char>(graph, "AB");

            Assert.Throws<ArgumentException>(() => sub.GetPotentialWeight('C', 'A'));
            Assert.Throws<ArgumentException>(() => sub.GetPotentialWeight('A', 'C'));
        }

        [Test]
        public void IsDirected_ReturnsFalse_IfBackingGraphIsUndirected()
        {
            var graph = new LiteralGraph("A", false);
            var sub = new SubGraph<char>(graph, "A");

            Assert.IsFalse(sub.IsDirected);
        }

        [Test]
        public void IsDirected_ReturnsTrue_IfBackingGraphIsDirected()
        {
            var graph = new LiteralGraph("A", true);
            var sub = new SubGraph<char>(graph, "A");

            Assert.IsTrue(sub.IsDirected);
        }

        [Test]
        public void IsReadOnly_ReturnsFalse_IfBackingGraphIsNotReaOnly()
        {
            var graph = new MazeGraph(new[,] {{true}});
            var sub = new SubGraph<Cell>(graph, new[] {new Cell(0, 0)});

            Assert.IsFalse(sub.IsReadOnly);
        }

        [Test]
        public void IsReadOnly_ReturnsTrue_IfBackingGraphIsReaOnly()
        {
            var graph = new LiteralGraph("A", true);
            var sub = new SubGraph<char>(graph, new[] {'A'});

            Assert.IsTrue(sub.IsReadOnly);
        }

        [Test]
        public void SupportsPotentialWeightEvaluation_ReturnsFalse_IfBackingGraphDoesNotSupportIt()
        {
            var graph = new LiteralGraph("A", true);
            var sub = new SubGraph<char>(graph, new[] {'A'});

            Assert.IsFalse(sub.SupportsPotentialWeightEvaluation);
        }

        [Test]
        public void SupportsPotentialWeightEvaluation_ReturnsTrue_IfBackingGraphSupportsIt()
        {
            var graph = new MazeGraph(new[,] {{true}});
            var sub = new SubGraph<Cell>(graph, new[] {new Cell(0, 0)});

            Assert.IsTrue(sub.SupportsPotentialWeightEvaluation);
        }
    }
}