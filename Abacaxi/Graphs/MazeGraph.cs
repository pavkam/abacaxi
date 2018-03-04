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

namespace Abacaxi.Graphs
{
    using System;
    using System.Collections.Generic;
    using Internal;
    using JetBrains.Annotations;

    /// <summary>
    /// A maze-structured graph.
    /// </summary>
    [PublicAPI]
    public sealed class MazeGraph : Graph<Cell>
    {
        [NotNull]
        private readonly bool[,] _matrix;
        private readonly int _lengthX;
        private readonly int _lengthY;

        private bool VertexExists(int x, int y) => x >= 0 && x < _lengthX && y >= 0 && y < _lengthY && _matrix[x, y];

        private void ValidateVertex([InvokerParameterName, NotNull]  string argumentName, Cell vertex)
        {
            if (!VertexExists(vertex.X, vertex.Y))
            {
                throw new ArgumentException($"Vertex '{vertex}' is not part of this graph.", argumentName);
            }
        }

        [NotNull, ItemNotNull]
        private IEnumerable<Edge<Cell>> GetEdgesIterate(Cell vertex)
        {
            Assert.Condition(VertexExists(vertex.X, vertex.Y));

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

        /// <summary>
        /// Gets a value indicating whether this graph's edges are directed.
        /// </summary>
        /// <value>
        /// Always returns <c>true</c>.
        /// </value>
        public override bool IsDirected => false;

        /// <summary>
        /// Gets a value indicating whether this instance is read only.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is read only; otherwise, <c>false</c>.
        /// </value>
        public override bool IsReadOnly => false;

        /// <summary>
        /// Gets a value indicating whether this graph supports potential weight evaluation (heuristics).
        /// </summary>
        /// <remarks>
        /// This graph implementation supports this potential weight evaluation based on cell proximity.
        /// </remarks>
        /// <value>
        /// <c>true</c> if graph supports potential weight evaluation; otherwise, <c>false</c>.
        /// </value>
        public override bool SupportsPotentialWeightEvaluation => true;

        /// <summary>
        /// Initializes a new instance of the <see cref="MazeGraph"/> class.
        /// </summary>
        /// <param name="matrix">The backing two-dimensional array.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="matrix"/> is <c>null</c>.</exception>
        public MazeGraph([NotNull] bool[,] matrix)
        {
            Validate.ArgumentNotNull(nameof(matrix), matrix);

            _matrix = matrix;
            _lengthX = matrix.GetLength(0);
            _lengthY = matrix.GetLength(1);
        }

        /// <summary>
        /// Returns an enumerator that iterates all vertices in the graph.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator{T}" /> that can be used to iterate through the collection.
        /// </returns>
        public override IEnumerator<Cell> GetEnumerator()
        {
            for (var x = 0; x < _lengthX; x++)
            for (var y = 0; y < _lengthY; y++)
            {
                if (_matrix[x, y])
                {
                    yield return new Cell(x, y);
                }
            }
        }

        /// <summary>
        /// Gets the potential total weight connecting <paramref name="fromVertex" /> and <paramref name="toVertex" /> vertices.
        /// </summary>
        /// <param name="fromVertex">The first vertex.</param>
        /// <param name="toVertex">The destination vertex.</param>
        /// <returns>
        /// The potential total cost (calculated as the number of cell hops from <paramref name="fromVertex"/> to <paramref name="toVertex"/>).
        /// </returns>
        public override double GetPotentialWeight(Cell fromVertex, Cell toVertex)
        {
            ValidateVertex(nameof(fromVertex), fromVertex);
            ValidateVertex(nameof(toVertex), toVertex);

            return
                Math.Abs(toVertex.X - fromVertex.X) +
                Math.Abs(toVertex.Y - fromVertex.Y);
        }

        /// <summary>
        /// Gets the edges for a given <paramref name="vertex" />.
        /// </summary>
        /// <param name="vertex"></param>
        /// <returns>
        /// A sequence of edges connected to the given <paramref name="vertex" />
        /// </returns>
        /// <exception cref="ArgumentException">Thrown if the <paramref name="vertex"/> is not part of the graph.</exception>
        public override IEnumerable<Edge<Cell>> GetEdges(Cell vertex)
        {
            ValidateVertex(nameof(vertex), vertex);

            return GetEdgesIterate(vertex);
        }
    }
}
