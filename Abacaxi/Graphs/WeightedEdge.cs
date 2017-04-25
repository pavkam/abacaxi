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
    /// Defines a weighted edge connecting two graph vertices.
    /// <typeparam name="TVertex">The type of the vertex.</typeparam>
    /// <typeparam name="TWeight">The type of the weight.</typeparam>
    /// </summary>
    public struct WeightedEdge<TVertex, TWeight>
    {
        /// <summary>
        /// Gets the starting vertex.
        /// </summary>
        /// <value>
        /// From starting vertex.
        /// </value>
        public TVertex FromVertex { get; private set; }

        /// <summary>
        /// Gets the connected vertex.
        /// </summary>
        /// <value>
        /// The connected vertex.
        /// </value>
        public TVertex ToVertex { get; private set; }

        /// <summary>
        /// Gets the edge's weight.
        /// </summary>
        /// <value>
        /// The edge's weight.
        /// </value>
        public TWeight Weight { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="WeightedEdge{TVertex, TWeight}"/> struct.
        /// </summary>
        /// <param name="fromVertex">The first vertex.</param>
        /// <param name="toVertex">The second vertex.</param>
        /// <param name="weight">The weight of the edge.</param>
        public WeightedEdge(TVertex fromVertex, TVertex toVertex, TWeight weight)
        {
            FromVertex = fromVertex;
            ToVertex = toVertex;
            Weight = weight;
        }

        /// <summary>
        /// Determines whether the specified <see cref="Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (!(obj is WeightedEdge<TVertex, TWeight>))
                return false;

            var co = (WeightedEdge<TVertex, TWeight>)obj;
            return
                Equals(co.Weight, Weight) &&
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
                (Weight != null ? Weight.GetHashCode() : 0) ^
                (FromVertex != null ? FromVertex.GetHashCode() : 0) ^
                (ToVertex != null ? ToVertex.GetHashCode() : 0);
        }

        /// <summary>
        /// Returns a <see cref="String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return $"{FromVertex} >={Weight}=> {ToVertex}";
        }
    }
}
