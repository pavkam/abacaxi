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

// ReSharper disable ReturnValueOfPureMethodIsNotUsed
// ReSharper disable ObjectCreationAsStatement



namespace Abacaxi.Tests.Graph
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Graphs;
    using NUnit.Framework;
    using Practice.Graphs;

    [TestFixture]
    public class ChessHorsePathGraphTests
    {
        [Test]
        public void Ctor_ThrowsException_ForBoardWidthLessThanOne()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new ChessHorsePathGraph(0, 1));
        }

        [Test]
        public void Ctor_ThrowsException_ForBoardHeightLessThanOne()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new ChessHorsePathGraph(1, 0));
        }

        [Test]
        public void IsDirected_ReturnsFalse()
        {
            var graph = new ChessHorsePathGraph(2, 3);
            Assert.IsFalse(graph.IsDirected);
        }

        [Test]
        public void IsReadOnly_ReturnsTrue()
        {
            var graph = new ChessHorsePathGraph(2, 3);
            Assert.IsTrue(graph.IsReadOnly);
        }

        [TestCase(-1, 0)]
        [TestCase(0, -1)]
        [TestCase(2, 0)]
        [TestCase(0, 3)]
        public void GetEdges_ThrowsException_ForInvalidCell(int x, int y)
        {
            var graph = new ChessHorsePathGraph(2, 3);
            Assert.Throws<InvalidOperationException>(() => graph.GetEdges(new Cell(x, y)).ToArray());
        }

        [TestCase(0, 0, "21,12")]
        [TestCase(1, 0, "02,31,22")]
        [TestCase(2, 0, "01,12,41,32")]
        [TestCase(3, 0, "11,22,42")]
        [TestCase(4, 0, "21,32")]
        [TestCase(0, 1, "20,22,13")]
        [TestCase(1, 1, "30,03,32,23")]
        [TestCase(2, 1, "00,02,40,13,42,33")]
        [TestCase(3, 1, "10,12,23,43")]
        [TestCase(4, 1, "20,22,33")]
        [TestCase(0, 2, "10,21,23,14")]
        [TestCase(1, 2, "00,20,31,04,33,24")]
        [TestCase(2, 2, "01,10,03,30,41,14,43,34")]
        [TestCase(3, 2, "11,20,13,40,24,44")]
        [TestCase(4, 2, "21,30,23,34")]
        [TestCase(0, 3, "11,22,24")]
        [TestCase(1, 3, "01,21,32,34")]
        [TestCase(2, 3, "02,11,04,31,42,44")]
        [TestCase(3, 3, "12,21,14,41")]
        [TestCase(4, 3, "22,31,24")]
        [TestCase(0, 4, "12,23")]
        [TestCase(1, 4, "02,22,33")]
        [TestCase(2, 4, "03,12,32,43")]
        [TestCase(3, 4, "13,22,42")]
        [TestCase(4, 4, "23,32")]
        public void GetEdges_ReturnsProperEdges(int x, int y, string expected)
        {
            var graph = new ChessHorsePathGraph(5, 5);
            var result = new List<string>();
            var cell = new Cell(x, y);
            foreach (var edge in graph.GetEdges(cell))
            {
                Assert.AreEqual(cell, edge.FromVertex);
                result.Add($"{edge.ToVertex.X}{edge.ToVertex.Y}");
            }
            var actual = string.Join(",", result);

            Assert.AreEqual(expected, actual);
        }

        [TestCase(1, 1, "00")]
        [TestCase(2, 3, "00,01,02,10,11,12")]
        public void Enumeration_ReturnsAllVertices(int w, int h, string expected)
        {
            var graph = new ChessHorsePathGraph(w, h);
            var result = string.Join(",", graph.Select(s => $"{s.X}{s.Y}"));

            Assert.AreEqual(expected, result);
        }
    }
}
