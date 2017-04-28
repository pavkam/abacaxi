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

    /// <summary>
    /// A chess-horse virtual graph. Each cell is connected to the cells that are reachable by a chess horse (L-shaped movements).
    /// </summary>
    public class ChessHorsePathGraph : Graph<Cell>
    {
        private readonly int _lenghtX;
        private readonly int _lengthY;

        private bool VertexExists(int x, int y)
        {
            return x >= 0 && x < _lenghtX && y >= 0 && y < _lengthY;
        }

        /// <summary>
        /// Gets a value indicating whether this graph's edges are directed.
        /// </summary>
        /// <value>
        /// Always returns <c>true</c>.
        /// </value>
        public override bool IsDirected => false;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChessHorsePathGraph"/> class.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="boardWidth"/> or <paramref name="boardHeight"/> are less than <c>1</c>.</exception>
        public ChessHorsePathGraph(int boardWidth, int boardHeight)
        {
            Validate.ArgumentGreaterThanZero(nameof(boardWidth), boardWidth);
            Validate.ArgumentGreaterThanZero(nameof(boardHeight), boardHeight);

            _lenghtX = boardWidth;
            _lengthY = boardHeight; 
        }

        /// <summary>
        /// Returns an enumerator that iterates all vertices in the graph.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.
        /// </returns>
        public override IEnumerator<Cell> GetEnumerator()
        {
            for (var x = 0; x < _lenghtX; x++)
            {
                for (var y = 0; y < _lengthY; y++)
                {
                    yield return new Cell(x, y);
                }
            }
        }

        /// <summary>
        /// Gets the edges for a given <param name="vertex" />.
        /// </summary>
        /// <param name="vertex"></param>
        /// <returns>
        /// A sequence of edges connected to the given <param name="vertex" />
        /// </returns>
        /// <exception cref="InvalidOperationException">Thrown if the <paramref name="vertex"/> is not part of the graph.</exception>
        public override IEnumerable<Edge<Cell>> GetEdges(Cell vertex)
        {
            if (!VertexExists(vertex.X, vertex.Y))
                throw new InvalidOperationException($"Vertex {vertex} is not part of this graph.");

            for (var i = -2; i <= 2; i += 4)
            {
                for (var j = -1; j <= 1; j += 2)
                {
                    if (VertexExists(vertex.X + i, vertex.Y + j))
                    {
                        yield return new Edge<Cell>(vertex, new Cell(vertex.X + i, vertex.Y + j));
                    }
                    if (VertexExists(vertex.X + j, vertex.Y + i))
                    {
                        yield return new Edge<Cell>(vertex, new Cell(vertex.X + j, vertex.Y + i));
                    }
                }
            }
        }

        /// <summary>
        /// Finds the shortest path between any two arbitrary cells on an infinite chess board.
        /// </summary>
        /// <param name="startCell">The start cell.</param>
        /// <param name="endCell">The end cell.</param>
        /// <returns>The shortest path between any two arbitrary cells in space.</returns>
        public static IEnumerable<Cell> FindShortestPathInInfiniteBoard(Cell startCell, Cell endCell)
        {
            const int padding = 2;

            var boardWidth = Math.Abs(endCell.X - startCell.X) + padding * 2;
            var boardHeight = Math.Abs(endCell.Y - startCell.Y) + padding * 2;

            var deltaX = Math.Min(startCell.X, endCell.X) - padding;
            var deltaY = Math.Min(startCell.Y, endCell.Y) - padding;

            var board = new ChessHorsePathGraph(boardWidth, boardHeight);
            foreach (var cell in board.FindShortestPath(new Cell(startCell.X - deltaX, startCell.Y - deltaY), new Cell(endCell.X - deltaX, endCell.Y - deltaY)))
            {
                yield return new Cell(cell.X + deltaX, cell.Y + deltaY);
            }
        }
    }
}
