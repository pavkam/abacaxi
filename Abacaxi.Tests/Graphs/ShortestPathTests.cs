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

    [TestFixture]
    public class ShortestPathTests
    {
        private MatrixGraph<int> M1 = new MatrixGraph<int>(new int[,]
        {
            { 1 }
        });

        private MatrixGraph<int> M2 = new MatrixGraph<int>(new int[,]
        {
            { 0, 1, 0 }
        });

        private MatrixGraph<int> M3 = new MatrixGraph<int>(new int[,]
        {
            { 0, 0, 0 }
        });

        private MatrixGraph<int> M4 = new MatrixGraph<int>(new int[,]
        {
            { 0, 1, 0 },
            { 0, 1, 0 },
            { 0, 0, 0 },
        });

        private MatrixGraph<int> M5 = new MatrixGraph<int>(new int[,]
        {
            { 0, 1, 0, 0, 0 },
            { 0, 1, 0, 1, 0 },
            { 0, 1, 0, 1, 0 },
            { 0, 1, 0, 1, 0 },
            { 0, 0, 0, 1, 0 }
        });

        private MatrixGraph<int> M6 = new MatrixGraph<int>(new int[,]
        {
            { 0, 0, 0, 1, 0 },
            { 0, 1, 0, 1, 0 },
            { 0, 1, 0, 0, 0 },
            { 0, 1, 1, 0, 1 },
            { 0, 0, 0, 0, 1 }
        });

        private static bool AcceptZeros(Graph<int, CellCoordinates> graph, CellCoordinates cellCoordinates)
        {
            Assert.IsNotNull(graph);
            return graph.GetNodeValue(cellCoordinates) == 0;
        }

        private static bool AcceptOnes(Graph<int, CellCoordinates> graph, CellCoordinates cellCoordinates)
        {
            Assert.IsNotNull(graph);
            return graph.GetNodeValue(cellCoordinates) == 1;
        }

        [Test]
        public void Find_ThrowsException_ForNullGraph()
        {
            Assert.Throws<ArgumentNullException>(() =>
                ShortestPath.Find((MatrixGraph<int>)null, AcceptZeros, new CellCoordinates(0, 0), new CellCoordinates(0, 0)).ToArray());
        }


        [Test]
        public void Find_ThrowsException_ForNullNodePredicate()
        {
            var graph = new MatrixGraph<int>(new[,] { { 1 } });
            Assert.Throws<ArgumentNullException>(() =>
                ShortestPath.Find(graph, null, new CellCoordinates(0, 0), new CellCoordinates(0, 0)).ToArray());
        }

        [Test]
        public void Find_ReturnsNothing_ForSameStartAndEnd_AndStartIsNotAccepted()
        {
            TestHelper.AssertSequence(
                ShortestPath.Find(M1, AcceptZeros,
                new CellCoordinates(0, 0),
                new CellCoordinates(0, 0)));
        }

        [Test]
        public void Find_ReturnsSingleCell_ForSameStartAndEnd_AndStartIsAccepted()
        {
            TestHelper.AssertSequence(
                ShortestPath.Find(M1, AcceptOnes,
                new CellCoordinates(0, 0),
                new CellCoordinates(0, 0)),
                new CellCoordinates(0, 0));
        }

        [Test]
        public void Find_ReturnsNothing_ForBlockedPath()
        {
            TestHelper.AssertSequence(
                ShortestPath.Find(M2, AcceptZeros,
                new CellCoordinates(0, 0),
                new CellCoordinates(0, 2)));
        }

        [Test]
        public void Find_ReturnsThreeNodes_ForSinglePath()
        {
            TestHelper.AssertSequence(
                ShortestPath.Find(M3, AcceptZeros,
                new CellCoordinates(0, 0),
                new CellCoordinates(0, 2)),
                new CellCoordinates(0, 0),
                new CellCoordinates(0, 1),
                new CellCoordinates(0, 2)
                );
        }

        [Test]
        public void Find_ReturnsFullPath_InSmallZPattern()
        {
            TestHelper.AssertSequence(
                ShortestPath.Find(M4, AcceptZeros,
                new CellCoordinates(0, 0),
                new CellCoordinates(0, 2)),
                new CellCoordinates(0, 0),
                new CellCoordinates(1, 0),
                new CellCoordinates(2, 0),
                new CellCoordinates(2, 1),
                new CellCoordinates(2, 2),
                new CellCoordinates(1, 2),
                new CellCoordinates(0, 2)
                );
        }

        [Test]
        public void Find_ReturnsFullPath_InZBigPattern()
        {
            TestHelper.AssertSequence(
                ShortestPath.Find(M5, AcceptZeros,
                new CellCoordinates(0, 0),
                new CellCoordinates(4, 4)),

                new CellCoordinates(0, 0),
                new CellCoordinates(1, 0),
                new CellCoordinates(2, 0),
                new CellCoordinates(3, 0),
                new CellCoordinates(4, 0),
                new CellCoordinates(4, 1),
                new CellCoordinates(4, 2),
                new CellCoordinates(3, 2),
                new CellCoordinates(2, 2),
                new CellCoordinates(1, 2),
                new CellCoordinates(0, 2),
                new CellCoordinates(0, 3),
                new CellCoordinates(0, 4),
                new CellCoordinates(1, 4),
                new CellCoordinates(2, 4),
                new CellCoordinates(3, 4),
                new CellCoordinates(4, 4)
                );
        }


        [Test]
        public void Find_ReturnsShortestPath_InMergingPattern()
        {
            TestHelper.AssertSequence(
                ShortestPath.Find(M6, AcceptZeros,
                new CellCoordinates(3, 0),
                new CellCoordinates(0, 4)),

                new CellCoordinates(3, 0),
                new CellCoordinates(4, 0),
                new CellCoordinates(4, 1),
                new CellCoordinates(4, 2),
                new CellCoordinates(4, 3),
                new CellCoordinates(3, 3),
                new CellCoordinates(2, 3),
                new CellCoordinates(2, 4),
                new CellCoordinates(1, 4),
                new CellCoordinates(0, 4)
                );
        }
    }
}
