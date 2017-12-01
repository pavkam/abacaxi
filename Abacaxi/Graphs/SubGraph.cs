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
    using Internal;
    using JetBrains.Annotations;
    using System.Diagnostics;

    /// <summary>
    /// Implements a "connected component" of a graph, basically a sub-graph. This implementation uses the original graph
    /// to obtain the edges but only reports the edges which stay within the given set of vertices.
    /// </summary>
    /// <typeparam name="TVertex">The type of the vertex.</typeparam>
    [PublicAPI]
    public class SubGraph<TVertex> : Graph<TVertex>
    {
        [NotNull]
        private readonly Graph<TVertex> _graph;
        [NotNull]
        private readonly HashSet<TVertex> _vertices;

        private void ValidateVertex([InvokerParameterName] [NotNull] string argumentName, TVertex vertex)
        {
            Validate.ArgumentNotNull(argumentName, vertex);
            if (!_vertices.Contains(vertex))
            {
                throw new ArgumentException($"Vertex '{vertex}' is not part of this sub-graph.", nameof(argumentName));
            }
        }

        [NotNull]
        [ItemNotNull]
        private IEnumerable<Edge<TVertex>> GetEdgesIterate([NotNull] TVertex vertex)
        {
            Debug.Assert(vertex != null);
            Debug.Assert(_vertices.Contains(vertex));

            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var edge in _graph.GetEdges(vertex))
            {
                if (_vertices.Contains(edge.ToVertex))
                {
                    yield return edge;
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SubGraph{TVertex}"/> class.
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="vertices">The vertices that part of this sub-graph.</param>
        /// <exception cref="ArgumentNullException">Thrown if either <paramref name="graph"/> or <paramref name="vertices"/> are <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">Thrown if <paramref name="vertices"/> contains at least one vertex that is not contained in <paramref name="graph"/>.</exception>
        public SubGraph([NotNull] Graph<TVertex> graph, [NotNull, ItemNotNull] IEnumerable<TVertex> vertices)
        {
            Validate.ArgumentNotNull(nameof(graph), graph);
            Validate.ArgumentNotNull(nameof(vertices), vertices);

            _vertices = new HashSet<TVertex>(vertices);
            _graph = graph;
        }

        /// <summary>
        /// Checks whether the sub-graph has directed edges.
        /// </summary>
        /// <remarks>
        /// This implementation uses the parent graph's <see cref="IsDirected"/> property.
        /// </remarks>
        /// <value>
        ///     <c>true</c> if the sub-graph is directed; otherwise, <c>false</c> .
        /// </value>
        public override bool IsDirected => _graph.IsDirected;

        /// <summary>
        /// Gets a value indicating whether this instance is read only.
        /// </summary>
        /// <remarks>
        /// This implementation uses the parent graph's <see cref="IsReadOnly"/> property.
        /// </remarks>
        /// <value>
        /// <c>true</c> if this instance is read only; otherwise, <c>false</c>.
        /// </value>
        public override bool IsReadOnly => _graph.IsReadOnly;

        /// <summary>
        /// Gets a value indicating whether this graph supports potential weight evaluation (heuristics).
        /// </summary>
        /// <remarks>
        /// This implementation uses the parent graph's <see cref="SupportsPotentialWeightEvaluation"/> property.
        /// </remarks>
        /// <value>
        /// <c>true</c> if graph supports potential weight evaluation; otherwise, <c>false</c>.
        /// </value>
        public override bool SupportsPotentialWeightEvaluation => _graph.SupportsPotentialWeightEvaluation;

        /// <summary>
        /// Returns an enumerator that iterates all vertices in the graph.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.
        /// </returns>
        public override IEnumerator<TVertex> GetEnumerator()
        {
            return _vertices.GetEnumerator();
        }

        /// <summary>
        /// Gets the potential total weight connecting <paramref name="fromVertex" /> and <paramref name="toVertex" /> vertices.
        /// </summary>
        /// <remarks>
        /// This implementation uses the parent graph's <see cref="GetPotentialWeight"/> method.
        /// </remarks>
        /// <param name="fromVertex">The first vertex.</param>
        /// <param name="toVertex">The destination vertex.</param>
        /// <returns>
        /// The potential total cost.
        /// </returns>
        /// <exception cref="NotImplementedException"></exception>
        public override double GetPotentialWeight(TVertex fromVertex, TVertex toVertex)
        {
            ValidateVertex(nameof(fromVertex), fromVertex);
            ValidateVertex(nameof(toVertex), toVertex);

            return _graph.GetPotentialWeight(fromVertex, toVertex);
        }

        /// <summary>
        /// Gets the edges for a given <paramref name="vertex" />.
        /// </summary>
        /// <param name="vertex">Then vertex to get edges for.</param>
        /// <returns>
        /// A sequence of edges connected to the given <paramref name="vertex"/>.
        /// </returns>
        /// <exception cref="ArgumentException">Thrown if the <paramref name="vertex"/> is not part of the graph.</exception>
        public override IEnumerable<Edge<TVertex>> GetEdges(TVertex vertex)
        {
            ValidateVertex(nameof(vertex), vertex);
            return GetEdgesIterate(vertex);
        }
    }
}
