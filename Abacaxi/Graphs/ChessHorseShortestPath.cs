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

        private static bool TruePredicate(Graph<int, CellCoordinates> graph, CellCoordinates cellCoordinates)
        {
            Debug.Assert(graph != null);

            return true;
        }

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
