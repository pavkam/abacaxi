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

    /// <summary>
    /// Defines a 2D matrix move.
    /// </summary>
    public struct Move
    {
        /// <summary>
        /// The X coordinate in a 2-D matrix.
        /// </summary>
        public int X { get; private set; }
        /// <summary>
        /// The Y coordinate in a 2-D matrix.
        /// </summary>
        public int Y { get; private set; }

        /// <summary>
        /// Creates a new instance of the <see cref="Move"/> struct.
        /// </summary>
        /// <param name="x">The X coordinate of the 2-D matrix.</param>
        /// <param name="y">The Y coordinate of the 2-D matrix.</param>
        public Move(int x, int y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Checks that current instance of <see cref="Move"/> is equal to the given object.
        /// </summary>
        /// <param name="obj">The object ot compare to.</param>
        /// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is Move))
                return false;

            var co = (Move)obj;
            return co.X == X && co.Y == Y; 
        }

        /// <summary>
        /// Returns the hashcode of this <see cref="Move"/> instance.
        /// </summary>
        /// <returns>The hashcode.</returns>
        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode();
        }

        /// <summary>
        /// Returns the string representation of this instance of <see cref="Move"/> struct.
        /// </summary>
        /// <returns>The string representation.</returns>
        public override string ToString()
        {
            return $"({X}, {Y})";
        }
    }
}
