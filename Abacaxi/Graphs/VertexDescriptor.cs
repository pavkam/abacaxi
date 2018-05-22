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
    using System;
    using Internal;
    using JetBrains.Annotations;

    /// <summary>
    /// Class used to describe a vertex in the context of a graph.
    /// </summary>
    [PublicAPI]
    public sealed class VertexDescriptor<TVertex>
    {
        /// <summary>
        /// Gets the vertex.
        /// </summary>
        /// <value>
        /// The vertex.
        /// </value>
        public TVertex Vertex { get; }

        /// <summary>
        /// Gets the in-degree of the <see cref="Vertex"/>.
        /// </summary>
        /// <value>
        /// The in-degree.
        /// </value>
        public int InDegree { get; }

        /// <summary>
        /// Gets the out-degree of the <see cref="Vertex"/>.
        /// </summary>
        /// <value>
        /// The out degree.
        /// </value>
        public int OutDegree { get; }

        /// <summary>
        /// Gets the index of the component that contains the <see cref="Vertex"/>.
        /// </summary>
        /// <value>
        /// The component index.
        /// </value>
        public int ComponentIndex { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="VertexDescriptor{TVertex}" /> structure.
        /// </summary>
        /// <param name="vertex">The vertex.</param>
        /// <param name="componentIndex">Index of the component.</param>
        /// <param name="inDegree">The in-degree.</param>
        /// <param name="outDegree">The out-degree.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="componentIndex"/> or
        /// <paramref name="inDegree"/> or <paramref name="outDegree"/> are less than zero.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="vertex"/> is <c>null</c>.</exception>
        public VertexDescriptor([NotNull] TVertex vertex, int componentIndex, int inDegree, int outDegree)
        {
            Validate.ArgumentNotNull(nameof(vertex), vertex);
            Validate.ArgumentGreaterThanOrEqualToZero(nameof(componentIndex), componentIndex);
            Validate.ArgumentGreaterThanOrEqualToZero(nameof(inDegree), inDegree);
            Validate.ArgumentGreaterThanOrEqualToZero(nameof(outDegree), outDegree);

            Vertex = vertex;
            InDegree = inDegree;
            OutDegree = outDegree;
            ComponentIndex = componentIndex;
        }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> that represents this instance.
        /// </returns>
        public override string ToString() => $"{InDegree} => {Vertex} ({ComponentIndex}) => {OutDegree}";
    }
}
