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

namespace Abacaxi.Graphs
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    /// <summary>
    /// Class implements the "shortest path for chess horse" algorithm. Works for any number of rows/columns.
    /// </summary>
    public static class ChessHorseShortestPath
    {
        private const int PaddingSize = 2;

        private sealed class ChessHorseVirtualGraph : IGraph<int, CellCoordinates, int>
        {
            private int _columns;
            private int _rows;

            public ChessHorseVirtualGraph(int columns, int rows)
            {
                Validate.ArgumentGreaterThanZero(nameof(columns), columns);
                Validate.ArgumentGreaterThanZero(nameof(rows), rows);

                _columns = columns;
                _rows = rows;
            }

            public int GetValue(CellCoordinates cellCoordinates)
            {
                throw new NotSupportedException();
            }

            public void SetValue(CellCoordinates cellCoordinates, int value)
            {
                throw new NotSupportedException();
            }

            public IEnumerable<Connection<CellCoordinates, int>> GetConnections(CellCoordinates cellCoordinates)
            {
                Validate.ArgumentLessThan(nameof(cellCoordinates.X), cellCoordinates.X, _columns);
                Validate.ArgumentLessThan(nameof(cellCoordinates.Y), cellCoordinates.Y, _rows);

                for (var i = -1; i <= 1; i += 2)
                {
                    for (var j = -2; j <= 2; j += 4)
                    {
                        var x = cellCoordinates.X + i;
                        var y = cellCoordinates.Y + j;
                        if (x >= 0 && x < _columns && y >= 0 && y < _rows)
                            yield return new Connection<CellCoordinates, int>(cellCoordinates, new CellCoordinates(x, y), 1);

                        x = cellCoordinates.X + j;
                        y = cellCoordinates.Y + i;
                        if (x >= 0 && x < _columns && y >= 0 && y < _rows)
                            yield return new Connection<CellCoordinates, int>(cellCoordinates, new CellCoordinates(x, y), 1);
                    }
                }
            }

            public int AddConnectionCosts(int a, int b)
            {
                throw new NotImplementedException();
            }

            public int CompareConnectionCosts(int a, int b)
            {
                throw new NotImplementedException();
            }

            public int EvaluatePotentialConnectionCost(CellCoordinates from, CellCoordinates to)
            {
                throw new NotImplementedException();
            }
        }

        private static bool TruePredicate(IGraph<int, CellCoordinates, int> graph, CellCoordinates cellCoordinates)
        {
            Debug.Assert(graph != null);

            return true;
        }

        /// <summary>
        /// Finds the shortest path between a starting and an ending cell on a chess .board
        /// </summary>
        /// <param name="startX">The starting cell X.</param>
        /// <param name="startY">The starting cell Y.</param>
        /// <param name="endX">The ending cell X.</param>
        /// <param name="endY">The ending cell Y.</param>
        /// <returns>A sequence of moves required to get from starting cell to the ending cell.</returns>
        public static IEnumerable<Move> Find(int startX, int startY, int endX, int endY)
        {
            var minX = Math.Min(startX, endX);
            var minY = Math.Min(startY, endY);
            var maxX = Math.Max(startX, endX);
            var maxY = Math.Max(startY, endY);

            var columns = maxX - minX + 1 + PaddingSize * 2;
            var rows = maxY - minY + 1 + PaddingSize * 2;

            var graph = new ChessHorseVirtualGraph(columns, rows);
            var preResult = ShortestPath.Find(
                graph, 
                TruePredicate, 
                new CellCoordinates(startX - minX + PaddingSize, startY - minY + PaddingSize), 
                new CellCoordinates(endX - minX + PaddingSize, endY - minY + PaddingSize));

            foreach(var r in preResult)
            {
                yield return new Move(r.X + minX - PaddingSize, r.Y + minY - PaddingSize);
            }
        }
    }
}
