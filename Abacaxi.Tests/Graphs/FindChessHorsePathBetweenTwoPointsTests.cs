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

namespace Abacaxi.Tests.Graphs
{
    using Abacaxi.Graphs;
    using NUnit.Framework;
    using System.Linq;
    using Practice.Graphs;

    [TestFixture]
    public class FindChessHorsePathBetweenTwoPointsTests
    {
        [TestCase(0, 0, 0, 0, "0,0"),TestCase(0, 0, 1, 0, "0,0;-2,-1;0,-2;1,0"),TestCase(0, 0, 0, 1, "0,0;-1,-2;-2,0;0,1"),TestCase(0, 0, 1, 1, "0,0;2,-1;1,1"),TestCase(-5, -5, 10, 10, "-5,-5;-3,-4;-1,-3;1,-2;3,-1;5,0;6,2;7,4;8,6;9,8;10,10")]
        public void FindChessHorsePathBetweenTwoPoints_FindsShortestPathBetweenTwoPoints(int sx, int sy, int ex, int ey, string expected)
        {
            var actual = string.Join(";", ChessHorsePathGraph.FindChessHorsePathBetweenTwoPoints(new Cell(sx, sy), new Cell(ex, ey))
                .Select(s => $"{s.X},{s.Y}"));

            Assert.AreEqual(expected, actual);
        }
    }
}
