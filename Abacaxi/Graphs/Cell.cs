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
    using JetBrains.Annotations;

    /// <summary>
    ///     Defines a 2-D coordinate to be used when manipulating matrices.
    /// </summary>
    [PublicAPI]
    public struct Cell
    {
        /// <summary>
        ///     Gets the X coordinate.
        /// </summary>
        /// <value>
        ///     The X coordinate.
        /// </value>
        public int X { get; }

        /// <summary>
        ///     Gets the Y coordinate.
        /// </summary>
        /// <value>
        ///     The Y coordinate.
        /// </value>
        public int Y { get; }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Cell" /> struct.
        /// </summary>
        /// <param name="x">The X coordinate.</param>
        /// <param name="y">The Y coordinate.</param>
        public Cell(int x, int y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        ///     Determines whether the specified <see cref="object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="object" /> to compare with this instance.</param>
        /// <returns>
        ///     <c>true</c> if the specified <see cref="object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj == null ||
                obj.GetType() != GetType())
            {
                return false;
            }

            var co = (Cell) obj;
            return co.X == X && co.Y == Y;
        }

        /// <summary>
        ///     Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        ///     A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode();
        }

        /// <summary>
        ///     Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>
        ///     A <see cref="string" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return $"({X}, {Y})";
        }
    }
}