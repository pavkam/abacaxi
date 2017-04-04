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
    /// Defines a connection between two graph nodes.
    /// </summary>
    public struct Connection<TIdentifier, TCost>
    {
        public TIdentifier From { get; private set; }
        public TIdentifier To { get; private set; }
        public TCost Cost { get; private set; }

        /// <summary>
        /// Creates a new instance of the <see cref="Connection{TIdentifier, TCost}"/> struct.
        /// </summary>
        /// <param name="cost">The cost of the connection.</param>
        /// <param name="from">The first connected node.</param>
        /// <param name="to">The second connected node.</param>
        public Connection(TIdentifier from, TIdentifier to, TCost cost)
        {
            From = from;
            To = to;
            Cost = cost;
        }

        /// <summary>
        /// Checks that current instance of <see cref="Connection{TIdentifier, TCost}"/> is equal to the given object.
        /// </summary>
        /// <param name="obj">The object ot compare to.</param>
        /// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is Connection<TIdentifier, TCost>))
                return false;

            var co = (Connection<TIdentifier, TCost>)obj;
            return
                Equals(co.Cost, Cost) &&
                Equals(co.From, From) &&
                Equals(co.To, To); 
        }

        /// <summary>
        /// Returns the hashcode of this <see cref="Connection{TIdentifier, TCost}"/> instance.
        /// </summary>
        /// <returns>The hashcode.</returns>
        public override int GetHashCode()
        {
            return
                (Cost != null ? Cost.GetHashCode() : 0) ^
                (From != null ? From.GetHashCode() : 0) ^
                (To != null ? To.GetHashCode() : 0);
        }

        /// <summary>
        /// Returns the string representation of this instance of <see cref="Connection{TIdentifier, TCost}"/> struct.
        /// </summary>
        /// <returns>The string representation.</returns>
        public override string ToString()
        {
            return $"{From} >={Cost}=> {To}";
        }
    }
}
