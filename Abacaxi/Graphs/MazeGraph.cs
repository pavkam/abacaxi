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
    /// A maze-structured graph.
    /// </summary>
    public class MazeGraph : IGraph<Cell>
    {
        private bool[,] _matrix;
        private int _lenghtX;
        private int _lengthY;

        private bool VertexExists(int x, int y)
        {
            return x >= 0 && x < _lenghtX && y >= 0 && y < _lengthY && _matrix[x, y];
        }

        /// <summary>
        /// Gets a value indicating whether this graph's edges are directed.
        /// </summary>
        /// <value>
        /// Always returns <c>true</c>.
        /// </value>
        public bool IsDirected => false;

        /// <summary>
        /// Initializes a new instance of the <see cref="MatrixGraph{TValue}"/> class.
        /// </summary>
        /// <param name="matrix">The backing two-dimensional array.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="matrix"/> is <c>null</c>.</exception>
        public MazeGraph(bool[,] matrix)
        {
            Validate.ArgumentNotNull(nameof(matrix), matrix);

            _matrix = matrix;
            _lenghtX = matrix.GetLength(0);
            _lengthY = matrix.GetLength(1); 
        }

        /// <summary>
        /// Gets all vertices in the graph.
        /// </summary>
        /// <returns>
        /// The sequence of all vertices in the graph.
        /// </returns>
        public IEnumerable<Cell> GetVertices()
        {
            for (var x = 0; x < _lenghtX; x++)
            {
                for (var y = 0; y < _lengthY; y++)
                {
                    if (_matrix[x, y])
                    {
                        yield return new Cell(x, y);
                    }
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
        public IEnumerable<Edge<Cell>> GetEdges(Cell vertex)
        {
            if (!VertexExists(vertex.X, vertex.Y))
                throw new InvalidOperationException($"Vertex {vertex} is not part of this graph.");

            for (var i = -1; i < 2; i += 2)
            {
                if (VertexExists(vertex.X + i, vertex.Y))
                {
                    yield return new Edge<Cell>(vertex, new Cell(vertex.X + i, vertex.Y));
                }
                if (VertexExists(vertex.X, vertex.Y + i))
                {
                    yield return new Edge<Cell>(vertex, new Cell(vertex.X, vertex.Y + i));
                }
            }
        }
    }
}
