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

namespace Abacaxi.Tests.Graphs
{
    using System;
    using System.Linq;
    using Abacaxi.Graphs;
    using NUnit.Framework;

    [TestFixture]
    public class ChessHorseShortestPathTests
    {
        [Test]
        public void Find_ReturnsStartingNode_ForEndEqualsToStart()
        {
            TestHelper.AssertSequence(
                ChessHorseShortestPath.Find(0, 0, 0, 0),
                new Move(0, 0)
                );
        }

        [Test]
        public void Find_ReturnsValidMoves_ForJoinedHorizontal()
        {
            TestHelper.AssertSequence(
                ChessHorseShortestPath.Find(0, 0, 1, 0),
                new Move(0, 0),
                new Move(-2, -1),
                new Move(0, -2),
                new Move(1, 0)
                );
        }

        [Test]
        public void Find_ReturnsValidMoves_ForJoinedVertical()
        {
            TestHelper.AssertSequence(
                ChessHorseShortestPath.Find(0, 0, 0, 1),
                new Move(0, 0),
                new Move(-1, -2),
                new Move(-2, 0),
                new Move(0, 1)
                );
        }

        [Test]
        public void Find_ReturnsValidMoves_ForDiagonallyJoined()
        {
            TestHelper.AssertSequence(
                ChessHorseShortestPath.Find(0, 0, 1, 1),
                new Move(0, 0),
                new Move(-1, 2),
                new Move(1, 1)
                );
        }

        [Test]
        public void Find_ReturnsValidMoves_ForMinusAndPlusDisjoined()
        {
            TestHelper.AssertSequence(
                ChessHorseShortestPath.Find(-2, -2, 1, 1),
                new Move(-2, -2),
                new Move(-1, 0),
                new Move(1, 1)
                );
        }

        [Test]
        public void Find_ReturnsValidMoves_ForFullBoard()
        {
            TestHelper.AssertSequence(
                ChessHorseShortestPath.Find(0, 0, 8, 8),
                new Move(0, 0),
                new Move(-1, 2),
                new Move(0, 4),
                new Move(2, 5),
                new Move(4, 6),
                new Move(6, 7),
                new Move(8, 8)
                );
        }
    }
}
