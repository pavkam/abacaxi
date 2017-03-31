using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abacaxi.Graphs
{
    /// <summary>
    /// Class used to treat a 2-dimensional array as a graph.
    /// </summary>
    /// <typeparam name="TValue">The type of the value stored in the graph's nodes.</typeparam>
    public sealed class MatrixGraph<TValue> : Graph<TValue, CellCoordinates>
    {
        private TValue[,] _matrix;
        private int _columns, _rows;

        /// <summary>
        /// Initializes a new instance of the <see cref="MatrixGraph{TValue}"/> class.
        /// </summary>
        /// <param name="matrix">The backing two-dimensional array.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="matrix"/> is <c>null</c>.</exception>
        public MatrixGraph(TValue[,] matrix)
        {
            Validate.ArgumentNotNull(nameof(matrix), matrix);

            _matrix = matrix;
            _columns = matrix.GetLength(0);
            _rows = matrix.GetLength(1); 
        }

        /// <summary>
        /// Gets the value of the node identified by the <paramref name="cellCoordinates"/> parameter.
        /// </summary>
        /// <param name="cellCoordinates">The cell coordinates.</param>
        /// <returns>The value of the node.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the value of <paramref name="cellCoordinates"/> is outside the bounds of the array.</exception>
        public override TValue GetNodeValue(CellCoordinates cellCoordinates)
        {
            Validate.ArgumentLessThan(nameof(cellCoordinates.X), cellCoordinates.X, _columns);
            Validate.ArgumentLessThan(nameof(cellCoordinates.Y), cellCoordinates.Y, _rows);

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
            Validate.ArgumentLessThan(nameof(cellCoordinates.X), cellCoordinates.X, _columns);
            Validate.ArgumentLessThan(nameof(cellCoordinates.Y), cellCoordinates.Y, _rows);

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
            Validate.ArgumentLessThan(nameof(cellCoordinates.X), cellCoordinates.X, _columns);
            Validate.ArgumentLessThan(nameof(cellCoordinates.Y), cellCoordinates.Y, _rows);

            if (cellCoordinates.Y > 0)
                yield return new CellCoordinates(cellCoordinates.X, cellCoordinates.Y - 1);
            if (cellCoordinates.X < _columns - 1)
                yield return new CellCoordinates(cellCoordinates.X + 1, cellCoordinates.Y);
            if (cellCoordinates.Y < _rows - 1)
                yield return new CellCoordinates(cellCoordinates.X, cellCoordinates.Y + 1);
            if (cellCoordinates.X > 0)
                yield return new CellCoordinates(cellCoordinates.X - 1, cellCoordinates.Y);
        }
    }
}
