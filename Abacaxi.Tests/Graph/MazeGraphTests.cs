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
    using System.Diagnostics.CodeAnalysis;

    [TestFixture]
    public class MazeGraphTests
    {
        private readonly bool[,] _m3X3 = {
            { true, true, true },
            { true, true, true },
            { true, true, true },
        };

        private readonly bool[,] _m2X2 = {
            { true, false },
            { true, true },
        };

        private static Cell Parse(string v)
        {
            Assert.NotNull(v);
            Assert.AreEqual(2, v.Length);

            int x = v[0] - '0', y = v[1] - '0';
            Assert.IsTrue(x >= 0 && x <= 9);
            Assert.IsTrue(y >= 0 && y <= 9);

            return new Cell(x, y);
        }

        private static IEnumerable<Cell> ParseList(string v)
        {
            Assert.NotNull(v);
            var split = v.Split(',');
            foreach (var s in split)
            {
                yield return Parse(s.Trim());
            }
        }

        [Test]
        [SuppressMessage("ReSharper", "ObjectCreationAsStatement")]
        public void Ctor_ThrowsException_ForNullMatrix()
        {
            Assert.Throws<ArgumentNullException>(() => new MazeGraph(null));
        }

        [Test]
        public void IsDirected_ReturnsFalse()
        {
            var graph = new MazeGraph(new[,]
            {
                { true, false },
                { true, true },
            });

            Assert.IsFalse(graph.IsDirected);
        }

        [Test]
        public void IsReadOnly_ReturnsFalse()
        {
            var graph = new MazeGraph(new[,] { { true } });

            Assert.IsFalse(graph.IsReadOnly);
        }

        [TestCase(-1, 0)]
        [TestCase(0, -1)]
        [TestCase(1, 0)]
        [TestCase(0, 1)]
        [SuppressMessage("ReSharper", "IteratorMethodResultIsIgnored")]
        public void GetEdges_ThrowsException_ForInvalidCell(int x, int y)
        {
            var graph = new MazeGraph(new[,] { { true } });

            Assert.Throws<ArgumentException>(() => graph.GetEdges(new Cell(x, y)));
        }

        [Test]
        [SuppressMessage("ReSharper", "IteratorMethodResultIsIgnored")]
        public void GetEdges_ThrowsException_ForFalseCell()
        {
            var graph = new MazeGraph(new[,] { { false } });

            Assert.Throws<ArgumentException>(() => graph.GetEdges(new Cell(0, 0)));
        }

        [TestCase("00", "10,01")]
        [TestCase("10", "00,11,20")]
        [TestCase("20", "10,21")]
        [TestCase("01", "00,11,02")]
        [TestCase("11", "10,01,21,12")]
        [TestCase("21", "20,11,22")]
        [TestCase("02", "01,12")]
        [TestCase("12", "11,02,22")]
        [TestCase("22", "21,12")]
        public void GetEdges_ReturnsAllValidEdgesIn3x3_ForVertex(string from, string to)
        {
            var fromVertex = Parse(from);
            var expectedEdges = new HashSet<Edge<Cell>>();
            foreach (var vertex in ParseList(to))
            {
                expectedEdges.Add(new Edge<Cell>(fromVertex, vertex));
            }

            var graph = new MazeGraph(_m3X3);
            var actualEdges = new HashSet<Edge<Cell>>();
            foreach (var edge in graph.GetEdges(fromVertex))
            {
                actualEdges.Add(edge);
            }

            Assert.IsTrue(expectedEdges.SetEquals(actualEdges));
        }

        [TestCase("00", "10")]
        [TestCase("10", "00,11")]
        [TestCase("11", "10")]
        public void GetEdges_ReturnsAllValidEdgesIn2x2_ForVertex(string from, string to)
        {
            var fromVertex = Parse(from);
            var expectedEdges = new HashSet<Edge<Cell>>();
            foreach (var vertex in ParseList(to))
            {
                expectedEdges.Add(new Edge<Cell>(fromVertex, vertex));
            }

            var graph = new MazeGraph(_m2X2);
            var actualEdges = new HashSet<Edge<Cell>>();
            foreach (var edge in graph.GetEdges(fromVertex))
            {
                actualEdges.Add(edge);
            }

            Assert.IsTrue(expectedEdges.SetEquals(actualEdges));
        }

        [Test]
        public void Enumeration_ReturnsNothing_ForEmptyMatrix()
        {
            var m = new bool[0, 0];
            var graph = new MazeGraph(m);

            TestHelper.AssertSequence(graph);
        }

        [TestCase("00,10,11")]
        public void Enumeration_ReturnsOnlyTrueCells(string cells)
        {
            var expectedCells = new HashSet<Cell>();
            foreach (var vertex in ParseList(cells))
            {
                expectedCells.Add(vertex);
            }

            var graph = new MazeGraph(_m2X2);
            var actualCells = new HashSet<Cell>();
            foreach (var vertex in graph)
            {
                actualCells.Add(vertex);
            }

            Assert.IsTrue(expectedCells.SetEquals(actualCells));
        }
    }
}
