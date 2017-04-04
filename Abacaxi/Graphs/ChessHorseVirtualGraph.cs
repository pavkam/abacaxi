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
    /// Virtual graph in which each cell represents a cell on a chess board (travelled by the a horse).
    /// </summary>
    /// <typeparam name="TValue">The type of the value stored in the graph's nodes.</typeparam>
    internal sealed class ChessHorseVirtualGraph : Graph<int, CellCoordinates>
    {
        private int _columns;
        private int _rows;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChessHorseVirtualGraph{TValue}"/> class.
        /// </summary>
        /// <param name="columns">Number of columns on the board.</param>
        /// <param name="rows">Number of rows on the board.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if either <paramref name="columns"/> or <paramref name="rows"/> is less than <c>1</c>.</exception>
        public ChessHorseVirtualGraph(int columns, int rows)
        {
            Validate.ArgumentGreaterThanZero(nameof(columns), columns);
            Validate.ArgumentGreaterThanZero(nameof(rows), rows);

            _columns = columns;
            _rows = rows;
        }

        /// <summary>
        /// Operation not supported.
        /// </summary>
        /// <param name="cellCoordinates">The cell coordinates.</param>
        /// <returns>The value of the node.</returns>
        /// <exception cref="NotSupportedException">Always thrown.</exception>
        public override int GetNodeValue(CellCoordinates cellCoordinates)
        {
            throw new NotSupportedException("Operation not supported in this graph type.");
        }

        /// <summary>
        /// Operation not supported.
        /// </summary>
        /// <param name="cellCoordinates">The cell coordinates.</param>
        /// <param name="value">The new value of the node.</param>
        /// <exception cref="NotSupportedException">Always thrown.</exception>
        public override void SetNodeValue(CellCoordinates cellCoordinates, int value)
        {
            throw new NotSupportedException("Operation not supported in this graph type.");
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

            for (var i = -1; i <= 1; i += 2)
            {
                for (var j = -2; j <= 2; j += 4)
                {
                    var x = cellCoordinates.X + i;
                    var y = cellCoordinates.Y + j;
                    if (x >= 0 && x < _columns && y >= 0 && y < _rows)
                        yield return new CellCoordinates(x, y);

                    x = cellCoordinates.X + j;
                    y = cellCoordinates.Y + i;
                    if (x >= 0 && x < _columns && y >= 0 && y < _rows)
                        yield return new CellCoordinates(x, y);
                }
            }
        }
    }
}
