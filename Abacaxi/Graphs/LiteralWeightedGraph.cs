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
    using System.Diagnostics;
    using System.Linq;
    using Containers;

    /// <summary>
    /// A graph class used primarily for designing algorithms. Each vertex is a digit or letter and can be connected with other
    /// vertices in directed or undirected fashion.
    /// </summary>
    public sealed class LiteralWeightedGraph : WeightedGraph<char, int>
    {
        private readonly Dictionary<char, IDictionary<char, int>> _vertices;

        private void AddVertex(char vertex)
        {
            Debug.Assert(char.IsLetterOrDigit(vertex));
            Debug.Assert(_vertices != null);

            if (!_vertices.TryGetValue(vertex, out IDictionary<char, int> set))
            {
                set = new Dictionary<char, int>();
                _vertices.Add(vertex, set);
            }
            else
            {
                throw new InvalidOperationException($"Vertex '{vertex}' has already been defined.");
            }
        }

        private void AddVertices(char from, char to, int weight)
        {
            Debug.Assert(char.IsLetterOrDigit(from));
            Debug.Assert(char.IsLetterOrDigit(to));
            Debug.Assert(_vertices != null);

            if (!_vertices.TryGetValue(from, out IDictionary<char, int> fromToSet))
            {
                fromToSet = new Dictionary<char, int>();
                _vertices.Add(from, fromToSet);
            }
            if (!_vertices.TryGetValue(to, out IDictionary<char, int> toFromSet))
            {
                toFromSet = new Dictionary<char, int>();
                _vertices.Add(to, toFromSet);
            }
            
            if (fromToSet.ContainsKey(to))
            {
                throw new InvalidOperationException($"Vertex '{from}' already contains an edge to vertex '{to}'.");
            }

            fromToSet.Add(to, weight);
        }

        private void AddVertices(char from, char to, char relation, int weight)
        {
            Debug.Assert(weight >= 0);

            switch (relation)
            {
                case '>':
                    AddVertices(from, to, weight);
                    break;
                case '<':
                    AddVertices(to, from, weight);
                    break;
                case '-':
                    AddVertices(from, to, weight);
                    AddVertices(to, from, weight);
                    break;
                default:
                    Debug.Fail("Unexpected relation character.");
                    break;
            }
        }

        private void Parse(string relationships)
        {
            Debug.Assert(relationships != null);

            var rels = new HashSet<char> {'-'};
            if (IsDirected)
            {
                rels.Add('>');
                rels.Add('<');
            }

            var vertices = new char[2];
            var relation = '\0';
            var cost = 0;

            var stage = 0;
            for (var i = 0; i < relationships.Length; i++)
            {
                var c = relationships[i];
                switch (stage)
                {
                    case 0: /* Expect "from" vertex (or whitespace) */
                        if (char.IsWhiteSpace(c))
                        {
                            continue;
                        }
                        if (!char.IsLetterOrDigit(c))
                        {
                            throw new FormatException($"Invalid character '{c}' at position {i}. Expected letter or digit.");
                        }

                        vertices[0] = c;
                        stage = 1;
                        break;
                    case 1: /* Expect relationship character or comma (or whitespace) */
                        if (char.IsWhiteSpace(c))
                        {
                            continue;
                        }
                        if (c == ',')
                        {
                            AddVertex(vertices[0]);

                            stage = 0;
                            continue;
                        }
                        if (!rels.Contains(c))
                        {
                            throw new FormatException($"Invalid character '{c}' at position {i}. Expected a relationship character.");
                        }

                        relation = c;
                        stage = 2;
                        break;
                    case 2: /* Expect cost digit (or whitespace) */
                        if (char.IsWhiteSpace(c))
                        {
                            continue;
                        }
                        if (!char.IsDigit(c))
                        {
                            throw new FormatException($"Invalid character '{c}' at position {i}. Expected a digit.");
                        }

                        cost = c - '0';
                        stage = 3;
                        break;
                    case 3: /* Expect next cost digit or whitespace or relationship character */
                        if (char.IsWhiteSpace(c))
                        {
                            stage = 4;
                            continue;
                        }
                        if (rels.Contains(c))
                        {
                            if (c != relation)
                            {
                                throw new FormatException(
                                    $"Invalid character '{c}' at position {i}. Expected '{relation}' relationship character.");
                            }

                            stage = 5;
                            continue;
                        }
                        if (!char.IsDigit(c))
                        {
                            throw new FormatException($"Invalid character '{c}' at position {i}. Expected a digit.");
                        }

                        cost = cost * 10 + (c - '0');
                        break;
                    case 4: /* Expect relationship character or whitespace */
                        if (char.IsWhiteSpace(c))
                        {
                            continue;
                        }
                        if (c != relation)
                        {
                            throw new FormatException(
                                $"Invalid character '{c}' at position {i}. Expected '{relation}' relationship character.");
                        }

                        stage = 5;
                        break;
                    case 5: /* Expect "to" vertex (or whitespace) */
                        if (char.IsWhiteSpace(c))
                        {
                            continue;
                        }
                        if (!char.IsLetterOrDigit(c))
                        {
                            throw new FormatException($"Invalid character '{c}' at position {i}. Expected letter or digit.");
                        }

                        vertices[1] = c;
                        stage = 6;
                        break;
                    case 6: /* Expect "," (or whitespace) */
                        if (char.IsWhiteSpace(c))
                        {
                            continue;
                        }
                        if (c != ',')
                        {
                            throw new FormatException($"Invalid character '{c}' at position {i}. Expected comma.");
                        }

                        AddVertices(vertices[0], vertices[1], relation, cost);

                        stage = 0;
                        break;
                }
            }

            switch (stage)
            {
                case 6:
                    AddVertices(vertices[0], vertices[1], relation, cost);
                    break;
                case 1:
                    AddVertex(vertices[0]);
                    break;
                case 0:
                    break;
                default:
                    throw new FormatException("Unexpected end of relationships string.");
            }
        }

        private void ValidateVertex(string paramName, char vertex)
        {
            Debug.Assert(!string.IsNullOrEmpty(paramName));

            if (!_vertices.ContainsKey(vertex))
            {
                throw new ArgumentException($"Vertex '{vertex}' is not part of this graph.", nameof(paramName));
            }
        }

        /// <summary>
        /// Gets a value indicating whether this graph's edges are directed.
        /// </summary>
        /// <value>
        ///     <c>true</c> if the graph is directed; <c>false</c> otherwise.
        /// </value>
        public override bool IsDirected { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is read only.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is read only; otherwise, <c>false</c>.
        /// </value>
        public override bool IsReadOnly => true;

        /// <summary>
        /// Initializes a new instance of the <see cref="LiteralWeightedGraph"/> class.
        /// </summary>
        /// <param name="relationships">The vertex relationship definitions.</param>
        /// <param name="isDirected">Specifies whether the graph is directed.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="relationships"/> is <c>null</c>.</exception>
        public LiteralWeightedGraph(string relationships, bool isDirected)
        {
            Validate.ArgumentNotNull(nameof(relationships), relationships);

            _vertices = new Dictionary<char, IDictionary<char, int>>();

            IsDirected = isDirected;
            Parse(relationships);
        }

        /// <summary>
        /// Gets the enumerator.
        /// </summary>
        /// <returns></returns>
        public override IEnumerator<char> GetEnumerator()
        {
            return _vertices.Keys.GetEnumerator();
        }

        /// <summary>
        /// Gets the edges of a given <paramref name="vertex" />.
        /// </summary>
        /// <param name="vertex">The vertex.</param>
        /// <returns>
        /// A sequence of edges connecting the <paramref name="vertex" /> to other vertices.
        /// </returns>
        /// <exception cref="System.InvalidOperationException">Thrown if the given <paramref name="vertex"/> if not part of the graph.</exception>
        public override IEnumerable<WeightedEdge<char, int>> GetEdgesAndWeights(char vertex)
        {
            ValidateVertex(nameof(vertex), vertex);

            return _vertices[vertex].Select(s => new WeightedEdge<char, int>(vertex, s.Key, s.Value));
        }

        /// <summary>
        /// Aggregates two weights.
        /// </summary>
        /// <param name="left">The left weight.</param>
        /// <param name="right">The right weight.</param>
        /// <returns>
        /// The sum of two weights. If the sum is greater than <see cref="int.MaxValue"/> the method returns <see cref="int.MaxValue"/>.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if either of <paramref name="left"/> or <paramref name="right"/> are less than zero.</exception>
        public override int AddWeights(int left, int right)
        {
            Validate.ArgumentGreaterThanOrEqualToZero(nameof(left), left);
            Validate.ArgumentGreaterThanOrEqualToZero(nameof(right), right);

            if (int.MaxValue - left > right || int.MaxValue - right > left)
            {
                return int.MaxValue;
            }

            return left + right;
        }

        /// <summary>
        /// Compares two weights.
        /// </summary>
        /// <param name="left">The left weight.</param>
        /// <param name="right">The right weight.</param>
        /// <returns>
        /// The comparison result.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if either of <paramref name="left"/> or <paramref name="right"/> are less than zero.</exception>
        public override int CompareWeights(int left, int right)
        {
            Validate.ArgumentGreaterThanOrEqualToZero(nameof(left), left);
            Validate.ArgumentGreaterThanOrEqualToZero(nameof(right), right);

            return left - right;
        }

        /// <summary>
        /// Gets the potential weight.
        /// </summary>
        /// <param name="fromVertex">From vertex.</param>
        /// <param name="toVertex">To vertex.</param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public override int GetPotentialWeight(char fromVertex, char toVertex)
        {
            ValidateVertex(nameof(fromVertex), fromVertex);
            ValidateVertex(nameof(toVertex), toVertex);

            return int.MaxValue;
        }
    }
}
