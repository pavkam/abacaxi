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
    /// <summary>
    /// Defines an edge connecting two graph vertices.
    /// <typeparam name="TVertex">The type of the vertex.</typeparam>
    /// </summary>
    public class Edge<TVertex>
    {
        /// <summary>
        /// Gets the starting vertex.
        /// </summary>
        /// <value>
        /// From starting vertex.
        /// </value>
        public TVertex FromVertex { get; }

        /// <summary>
        /// Gets the connected vertex.
        /// </summary>
        /// <value>
        /// The connected vertex.
        /// </value>
        public TVertex ToVertex { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Edge{TVertex}"/> structure.
        /// </summary>
        /// <param name="fromVertex">The first vertex.</param>
        /// <param name="toVertex">The second vertex.</param>
        public Edge(TVertex fromVertex, TVertex toVertex)
        {
            FromVertex = fromVertex;
            ToVertex = toVertex;
        }

        /// <summary>
        /// Determines whether the specified <see cref="object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (!(obj is Edge<TVertex>))
                return false;

            var co = (Edge<TVertex>)obj;
            return
                Equals(co.FromVertex, FromVertex) &&
                Equals(co.ToVertex, ToVertex);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return
                (FromVertex != null ? FromVertex.GetHashCode() : 0) ^
                (ToVertex != null ? ToVertex.GetHashCode() : 0);
        }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return $"{FromVertex} >==> {ToVertex}";
        }
    }
}
