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
    using System.Collections.Generic;

    /// <summary>
    /// Class used to treat a 2-dimensional array as a graph.
    /// </summary>
    /// <typeparam name="TValue">The type of the value stored in the graph's nodes.</typeparam>
    public class MatrixGraph<TValue> : Graph<TValue, CellCoordinates>
    {
        private TValue[,] _matrix;

        /// <summary>
        /// Number of columns in the backing matrix.
        /// </summary>
        protected int ColumnCount { get; private set; }

        /// <summary>
        /// Number of rows in the backing matrix.
        /// </summary>
        protected int RowCount { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MatrixGraph{TValue}"/> class.
        /// </summary>
        /// <param name="matrix">The backing two-dimensional array.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="matrix"/> is <c>null</c>.</exception>
        public MatrixGraph(TValue[,] matrix)
        {
            Validate.ArgumentNotNull(nameof(matrix), matrix);

            _matrix = matrix;
            ColumnCount = matrix.GetLength(0);
            RowCount = matrix.GetLength(1); 
        }

        /// <summary>
        /// Gets the value of the node identified by the <paramref name="cellCoordinates"/> parameter.
        /// </summary>
        /// <param name="cellCoordinates">The cell coordinates.</param>
        /// <returns>The value of the node.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the value of <paramref name="cellCoordinates"/> is outside the bounds of the array.</exception>
        public override TValue GetNodeValue(CellCoordinates cellCoordinates)
        {
            Validate.ArgumentLessThan(nameof(cellCoordinates.X), cellCoordinates.X, ColumnCount);
            Validate.ArgumentLessThan(nameof(cellCoordinates.Y), cellCoordinates.Y, RowCount);

            return _matrix[cellCoordinates.X, cellCoordinates.Y];
        }

        /// <summary>
        /// Sets the value of the node identified by the <paramref name="cellCoordinates"/> parameter.
        /// </summary>
        /// <param name="cellCoordinates">The cell coordinates.</param>
        /// <param name="value">The new value of the node.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the value of <paramref name="cellCoordinates"/> is outside the bounds of the array.</exception>
        public override void SetNodeValue(CellCoordinates cellCoordinates, TValue value)
        {
            Validate.ArgumentLessThan(nameof(cellCoordinates.X), cellCoordinates.X, ColumnCount);
            Validate.ArgumentLessThan(nameof(cellCoordinates.Y), cellCoordinates.Y, RowCount);

            _matrix[cellCoordinates.X, cellCoordinates.Y] = value;
        }

        /// <summary>
        /// Returns a list of all nodes connected to the node identified by the <paramref name="cellCoordinates"/> parameter.
        /// </summary>
        /// <param name="cellCoordinates">The cell coordinates.</param>
        /// <returns>The list of connected nodes and the associated connection cost.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the value of <paramref name="cellCoordinates"/> is outside the bounds of the array.</exception>
        public override IEnumerable<CellCoordinates> GetNodeConnections(CellCoordinates cellCoordinates)
        {
            Validate.ArgumentLessThan(nameof(cellCoordinates.X), cellCoordinates.X, ColumnCount);
            Validate.ArgumentLessThan(nameof(cellCoordinates.Y), cellCoordinates.Y, RowCount);

            if (cellCoordinates.Y > 0)
                yield return new CellCoordinates(cellCoordinates.X, cellCoordinates.Y - 1);
            if (cellCoordinates.X < ColumnCount - 1)
                yield return new CellCoordinates(cellCoordinates.X + 1, cellCoordinates.Y);
            if (cellCoordinates.Y < RowCount - 1)
                yield return new CellCoordinates(cellCoordinates.X, cellCoordinates.Y + 1);
            if (cellCoordinates.X > 0)
                yield return new CellCoordinates(cellCoordinates.X - 1, cellCoordinates.Y);
        }
    }
}
