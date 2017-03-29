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
    using System.Diagnostics;

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
    }
}
