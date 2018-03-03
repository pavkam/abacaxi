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

using System;

namespace Abacaxi.Graphs
{
    using JetBrains.Annotations;
    using Internal;

    /// <summary>
    /// Defines a weighted edge connecting two graph vertices.
    /// <typeparam name="TVertex">The type of the vertex.</typeparam>
    /// </summary>
    [PublicAPI]
    public sealed class Edge<TVertex>
    {
        /// <summary>
        /// Gets the starting vertex.
        /// </summary>
        /// <value>
        /// From starting vertex.
        /// </value>
        [NotNull]
        public TVertex FromVertex { get; }

        /// <summary>
        /// Gets the connected vertex.
        /// </summary>
        /// <value>
        /// The connected vertex.
        /// </value>
        [NotNull]
        public TVertex ToVertex { get; }

        /// <summary>
        /// Gets the edge's weight.
        /// </summary>
        /// <value>
        /// The edge's weight.
        /// </value>
        public double Weight { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Edge{TVertex}" /> struct.
        /// </summary>
        /// <param name="fromVertex">The first vertex.</param>
        /// <param name="toVertex">The second vertex.</param>
        /// <param name="weight">The weight of the edge.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="weight"/> is less than zero.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="fromVertex"/> or <paramref name="toVertex"/> is <c>null</c>.</exception>
        public Edge([NotNull] TVertex fromVertex, [NotNull] TVertex toVertex, double weight = 1.0)
        {
            Validate.ArgumentNotNull(nameof(fromVertex), fromVertex);
            Validate.ArgumentNotNull(nameof(toVertex), toVertex);
            Validate.ArgumentGreaterThanOrEqualToZero(nameof(weight), weight);

            FromVertex = fromVertex;
            ToVertex = toVertex;
            Weight = weight;
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
            if (obj == null || obj.GetType() != GetType())
            {
                return false;
            }

            var co = (Edge<TVertex>)obj;
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
            var hashCode = 17;
            hashCode = hashCode * 23 + ToVertex.GetHashCode();
            hashCode = hashCode * 23 + FromVertex.GetHashCode();
            hashCode = hashCode * 23 + Weight.GetHashCode();

            return hashCode;
        }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return $"{FromVertex} >={Weight}=> {ToVertex}";
        }
    }
}
