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

namespace Abacaxi.Matrix
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    /// <summary>
    /// The "flood fill" algorithm. For any given matrix the algorithm will start at a given (X, Y) position and will fill in the
    /// matrix with a given "color" using a "path function".
    /// This class contains two implementations, one recursive and the other one - iterative.
    /// </summary>
    public static class FloodFill
    {
        private static void ApplyRecursiveNoChecks<T>(
            T[,] matrix,
            CellCoordinates rootCellCoordinates,
            PathFunction<T> pathFunc,
            Predicate<T> cellCanBeColoredFunc,
            T color)
        {
            Debug.Assert(matrix != null);
            Debug.Assert(pathFunc != null);
            Debug.Assert(cellCanBeColoredFunc != null);
            Debug.Assert(rootCellCoordinates.X < matrix.GetLength(0));
            Debug.Assert(rootCellCoordinates.Y < matrix.GetLength(1));

            if (cellCanBeColoredFunc(matrix[rootCellCoordinates.X, rootCellCoordinates.Y]))
            {
                matrix[rootCellCoordinates.X, rootCellCoordinates.Y] = color;
                Debug.Assert(!cellCanBeColoredFunc(matrix[rootCellCoordinates.X, rootCellCoordinates.Y]), "Coloring of cell failed. The predicate is invalid.");

                foreach (var neighborCellCoordinates in pathFunc(matrix, rootCellCoordinates))
                {
                    ApplyRecursiveNoChecks(matrix, neighborCellCoordinates, pathFunc, cellCanBeColoredFunc, color);
                }
            }
        }

        /// <summary>
        /// Performs the "flood fill" using the recursive method.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the matrix (color).</typeparam>
        /// <param name="matrix">The two-dimensional matrix.</param>
        /// <param name="rootCellCoordinates">The starting position.</param>
        /// <param name="pathFunc">The path function that dictates the neighbor selection.</param>
        /// <param name="cellCanBeColoredFunc">The predicate that tests whether a specific cell is coloreable.</param>
        /// <param name="color">The color to fill the matrix with.</param>
        /// <exception cref="ArgumentNullException">Thrown if either of <paramref name="marix"/>, <paramref name="pathFunc"/> or <paramref name="cellCanBeColoredFunc"/> are null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the <paramref name="rootCellCoordinates"/> is pointing to a cell outside the <paramref name="matrix"/>.</exception>
        public static void ApplyRecursive<T>(
            T[,] matrix,
            CellCoordinates rootCellCoordinates,
            PathFunction<T> pathFunc,
            Predicate<T> cellCanBeColoredFunc,
            T color)
        {
            Validate.ArgumentNotNull(nameof(matrix), matrix);
            Validate.ArgumentNotNull(nameof(pathFunc), pathFunc);
            Validate.ArgumentNotNull(nameof(cellCanBeColoredFunc), cellCanBeColoredFunc);
            Validate.ArgumentLessThan(nameof(rootCellCoordinates.X), rootCellCoordinates.X, matrix.GetLength(0));
            Validate.ArgumentLessThan(nameof(rootCellCoordinates.X), rootCellCoordinates.Y, matrix.GetLength(1));

            ApplyRecursiveNoChecks(matrix, rootCellCoordinates, pathFunc, cellCanBeColoredFunc, color);
        }


        /// <summary>
        /// Performs the "flood fill" using the iterative method.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the matrix (color).</typeparam>
        /// <param name="matrix">The two-dimensional matrix.</param>
        /// <param name="rootCellCoordinates">The starting position.</param>
        /// <param name="pathFunc">The path function that dictates the neighbor selection.</param>
        /// <param name="cellCanBeColoredFunc">The predicate that tests whether a specific cell is coloreable.</param>
        /// <param name="color">The color to fill the matrix with.</param>
        /// <exception cref="ArgumentNullException">Thrown if either of <paramref name="marix"/>, <paramref name="pathFunc"/> or <paramref name="cellCanBeColoredFunc"/> are null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the <paramref name="rootCellCoordinates"/> is pointing to a cell outside the <paramref name="matrix"/>.</exception>
        public static void ApplyIterative<T>(
            T[,] matrix,
            CellCoordinates rootCellCoordinates,
            PathFunction<T> pathFunc,
            Predicate<T> cellCanBeColoredFunc,
            T color)
        {
            Validate.ArgumentNotNull(nameof(matrix), matrix);
            Validate.ArgumentNotNull(nameof(pathFunc), pathFunc);
            Validate.ArgumentNotNull(nameof(cellCanBeColoredFunc), cellCanBeColoredFunc);
            Validate.ArgumentLessThan(nameof(rootCellCoordinates.X), rootCellCoordinates.X, matrix.GetLength(0));
            Validate.ArgumentLessThan(nameof(rootCellCoordinates.X), rootCellCoordinates.Y, matrix.GetLength(1));

            var cellsToInspectNext = new Queue<CellCoordinates>();
            cellsToInspectNext.Enqueue(rootCellCoordinates);

            while(cellsToInspectNext.Count > 0)
            {
                var nextCellToInspect = cellsToInspectNext.Dequeue();
                if (cellCanBeColoredFunc(matrix[nextCellToInspect.X, nextCellToInspect.Y]))
                {
                    matrix[nextCellToInspect.X, nextCellToInspect.Y] = color;
                    foreach (var neighbor in pathFunc(matrix, nextCellToInspect))
                    {
                        cellsToInspectNext.Enqueue(neighbor);
                    }
                }
            }
        }
    }
}
