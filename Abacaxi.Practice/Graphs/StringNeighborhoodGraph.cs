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

namespace Abacaxi.Practice.Graphs
{
    using System;
    using System.Collections.Generic;
    using Abacaxi.Graphs;
    using Containers;
    using Internal;

    /// <summary>
    /// A graph composed by a number of strings (representing vertices) and connected by edges signifying potential one-letter transformations.
    /// </summary>
    public sealed class StringNeighborhoodGraph : Graph<string>
    {
        private readonly Trie<char, ISet<string>> _neighborhoods;
        private readonly ISet<string> _vertices;

        private static IEnumerable<char[]> GetAllStringPatterns(string s)
        {
            var ca = (s ?? string.Empty).ToCharArray();
            var result = new char[ca.Length][];
            for (var i = 0; i < ca.Length; i++)
            {
                result[i] = new char[ca.Length];
                Array.Copy(ca, result[i], ca.Length);
                result[i][i] = '\0';
            }

            return result;
        }

        /// <summary>
        /// Gets a value indicating whether this graph's edges are directed.
        /// </summary>
        /// <value>
        /// Always returns <c>false</c>.
        /// </value>
        public override bool IsDirected => false;

        /// <summary>
        /// Gets a value indicating whether this instance is read only.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is read only; otherwise, <c>false</c>.
        /// </value>
        public override bool IsReadOnly => true;

        /// <summary>
        /// Initializes a new instance of the <see cref="StringNeighborhoodGraph"/> class.
        /// </summary>
        /// <param name="sequence">The sequence of strings to build the graph upon.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="sequence"/> is <c>null</c>.</exception>
        public StringNeighborhoodGraph(IEnumerable<string> sequence)
        {
            Validate.ArgumentNotNull(nameof(sequence), sequence);

            _neighborhoods = new Trie<char, ISet<string>>();
            _vertices = new HashSet<string>();
            foreach (var item in sequence)
            {
                _vertices.Add(item);

                foreach (var pattern in GetAllStringPatterns(item))
                {
                    if (!_neighborhoods.TryGetValue(pattern, out ISet<string> neighbors))
                    {
                        neighbors = new HashSet<string>();
                        _neighborhoods.Add(pattern, neighbors);
                    }

                    neighbors.Add(item);
                }
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates all vertices in the graph.
        /// </summary>
        /// <returns>
        /// A <see cref="IEnumerator{T}" /> that can be used to iterate through the collection.
        /// </returns>
        public override IEnumerator<string> GetEnumerator()
        {
            return _vertices.GetEnumerator();
        }

        /// <summary>
        /// Gets the edges for a given <param name="vertex" />.
        /// </summary>
        /// <param name="vertex"></param>
        /// <returns>
        /// A sequence of edges connected to the given <param name="vertex" />
        /// </returns>
        /// <exception cref="ArgumentException">Thrown if the <paramref name="vertex"/> is not part of the graph.</exception>
        public override IEnumerable<Edge<string>> GetEdges(string vertex)
        {
            if (vertex == null || !_vertices.Contains(vertex))
            {
                throw new ArgumentException($"Vertex '{vertex}' is not part of this graph.", nameof(vertex));
            }

            foreach (var pattern in GetAllStringPatterns(vertex))
            {
                // ReSharper disable once CollectionNeverUpdated.Local
                if (_neighborhoods.TryGetValue(pattern, out ISet<string> neighbors))
                {
                    foreach (var neighbor in neighbors)
                    {
                        if (neighbor != vertex)
                        {
                            yield return new Edge<string>(vertex, neighbor);
                        }
                    }
                }
            }
        }
    }
}
