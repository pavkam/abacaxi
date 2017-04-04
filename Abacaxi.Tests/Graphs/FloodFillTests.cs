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
    using Abacaxi.Graphs;
    using NUnit.Framework;

    [TestFixture]
    public class FloodFillTests
    {
        private int[,] M, O;

        [SetUp]
        public void SetUp()
        {
            M = new[,]
            {
                { 0, 1, 0, 1, 0 },
                { 0, 1, 0, 1, 0 },
                { 0, 1, 0, 1, 0 },
                { 0, 1, 1, 1, 0 },
                { 0, 0, 0, 0, 0 },
            };

            O = (int[,])M.Clone();
        }

        private static bool ZeroCanBeColored(Graph<int, CellCoordinates, int> graph, CellCoordinates cellCoordinates)
        {
            Assert.IsNotNull(graph);

            return graph.GetNodeValue(cellCoordinates) == 0;
        }

        private static bool OneCanBeColored(Graph<int, CellCoordinates, int> graph, CellCoordinates cellCoordinates)
        {
            Assert.IsNotNull(graph);

            return graph.GetNodeValue(cellCoordinates) == 1;
        }

        [Test]
        public void ApplyRecursive_ThrowsException_WhenMatrixIsNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
                FloodFill.ApplyRecursive(
                    (MatrixGraph<int>)null,
                    new CellCoordinates(0, 0),
                    ZeroCanBeColored,
                    2)
            );
        }

        [Test]
        public void ApplyRecursive_ThrowsException_ForNullNodePredicate()
        {
            Assert.Throws<ArgumentNullException>(() =>
                FloodFill.ApplyRecursive(
                    new MatrixGraph<int>(M),
                    new CellCoordinates(0, 0),
                    null,
                    2)
            );
        }

        [TestCase(5, 0)]
        [TestCase(0, 5)]
        public void ApplyRecursive_ThrowsException_WhenOutsideTheBounds(int x, int y)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                FloodFill.ApplyRecursive(
                    new MatrixGraph<int>(M),
                    new CellCoordinates(x, y),
                    ZeroCanBeColored,
                    2)
            );
        }

        [TestCase(1, 0)]
        public void ApplyRecursive_DoesNothing_WhenStartingOnNonColorable(int y, int x)
        {
            FloodFill.ApplyRecursive(
                new MatrixGraph<int>(M), 
                new CellCoordinates(x, y), 
                ZeroCanBeColored,
                2);

            Assert.AreEqual(O, M);
        }

        [Test]
        public void ApplyRecursive_ColorsZeroes()
        {
            FloodFill.ApplyRecursive(
                new MatrixGraph<int>(M),
                new CellCoordinates(0, 0),
                ZeroCanBeColored,
                2);


            var o = new MatrixGraph<int>(O);
            for (var x = 0; x < O.GetLength(0); x++)
            {
                for (var y = 0; y < O.GetLength(1); y++)
                {
                    if (ZeroCanBeColored(o, new CellCoordinates(x, y)))
                    {
                        O[x, y] = 2;
                    }
                }
            }

            O[0, 2] = 0;
            O[1, 2] = 0;
            O[2, 2] = 0;

            Assert.AreEqual(O, M);
        }


        [Test]
        public void ApplyIterative_ThrowsException_WhenMatrixIsNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
                FloodFill.ApplyIterative(
                    (MatrixGraph<int>)null,
                    new CellCoordinates(0, 0),
                    ZeroCanBeColored,
                    2)
            );
        }
        
        [Test]
        public void ApplyIterative_ThrowsException_WhenCellCanBeColoredFuncIsNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
                FloodFill.ApplyIterative(
                    new MatrixGraph<int>(M),
                    new CellCoordinates(0, 0),
                    null,
                    2)
            );
        }

        [TestCase(5, 0)]
        [TestCase(0, 5)]
        public void ApplyIterative_ThrowsException_WhenOutsideTheBounds(int x, int y)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                FloodFill.ApplyIterative(
                    new MatrixGraph<int>(M),
                    new CellCoordinates(x, y),
                    ZeroCanBeColored,
                    2)
            );
        }

        [TestCase(1, 0)]
        public void ApplyIterative_DoesNothing_WhenStartingOnNonColorable(int y, int x)
        {
            FloodFill.ApplyIterative(
                new MatrixGraph<int>(M),
                new CellCoordinates(x, y),
                ZeroCanBeColored,
                2);

            Assert.AreEqual(O, M);
        }

        [Test]
        public void ApplyIterative_ColorsZeroes()
        {
            FloodFill.ApplyIterative(
                new MatrixGraph<int>(M),
                new CellCoordinates(0, 0),
                ZeroCanBeColored,
                2);

            var o = new MatrixGraph<int>(O);
            for (var x = 0; x < O.GetLength(0); x++)
            {
                for (var y = 0; y < O.GetLength(1); y++)
                {
                    if (ZeroCanBeColored(o, new CellCoordinates(x, y)))
                    {
                        O[x, y] = 2;
                    }
                }
            }

            O[0, 2] = 0;
            O[1, 2] = 0;
            O[2, 2] = 0;

            Assert.AreEqual(O, M);
        }

    }
}
