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
    using Internal;
    using JetBrains.Annotations;

    /// <summary>
    /// A graph class used primarily for designing algorithms. Each vertex is a digit or letter and can be connected with other
    /// vertices in directed or undirected fashion.
    /// </summary>
    [PublicAPI]
    public sealed class LiteralGraph : Graph<char>
    {
        [NotNull]
        private readonly Dictionary<char, ISet<Edge<char>>> _vertices;

        private void AddVertex(char vertex)
        {
            Debug.Assert(char.IsLetterOrDigit(vertex));
            Debug.Assert(_vertices != null);

            if (!_vertices.TryGetValue(vertex, out var set))
            {
                set = new HashSet<Edge<char>> ();
                _vertices.Add(vertex, set);
            }
        }

        private void AddVertices(char from, char to, int weight)
        {
            Debug.Assert(char.IsLetterOrDigit(from));
            Debug.Assert(char.IsLetterOrDigit(to));
            Debug.Assert(_vertices != null);

            if (!_vertices.TryGetValue(from, out var fromToSet))
            {
                fromToSet = new HashSet<Edge<char>>();
                _vertices.Add(from, fromToSet);
            }
            if (!_vertices.TryGetValue(to, out var toFromSet))
            {
                toFromSet = new HashSet<Edge<char>>();
                _vertices.Add(to, toFromSet);
            }

            fromToSet.Add(new Edge<char>(from, to, weight));
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

                    if (to != from)
                    {
                        AddVertices(to, @from, weight);
                    }
                    break;
                default:
                    Debug.Assert(false, "Unexpected relation character.");
                    break;
            }
        }

        private void Parse([NotNull] string relationships)
        {
            Debug.Assert(relationships != null);

            // ReSharper disable once IdentifierTypo
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

        private void ValidateVertex([InvokerParameterName] [NotNull] string argumentName, char vertex)
        {
            if (!_vertices.ContainsKey(vertex))
            {
                throw new ArgumentException($"Vertex '{vertex}' is not part of this graph.", argumentName);
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
        public override bool IsReadOnly { get; } = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="LiteralGraph"/> class.
        /// </summary>
        /// <param name="relationships">The vertex relationship definitions.</param>
        /// <param name="isDirected">Specifies whether the graph is directed.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="relationships"/> is <c>null</c>.</exception>
        public LiteralGraph([NotNull] string relationships, bool isDirected)
        {
            Validate.ArgumentNotNull(nameof(relationships), relationships);

            _vertices = new Dictionary<char, ISet<Edge<char>>>();

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
        public override IEnumerable<Edge<char>> GetEdges(char vertex)
        {
            ValidateVertex(nameof(vertex), vertex);

            return _vertices[vertex];
        }

        /// <summary>
        /// Gets the potential weight.
        /// </summary>
        /// <param name="fromVertex">From vertex.</param>
        /// <param name="toVertex">To vertex.</param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException">Always thrown.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if either of <paramref name="fromVertex"/> or <paramref name="fromVertex"/> are not part of this graph.</exception>
        public override double GetPotentialWeight(char fromVertex, char toVertex)
        {
            ValidateVertex(nameof(fromVertex), fromVertex);
            ValidateVertex(nameof(toVertex), toVertex);

            throw new NotSupportedException("This graph does not support potential weight calculation.");
        }

        /// <summary>
        /// Gets a value indicating whether this graph supports potential weight evaluation (heuristics). This
        /// implementation always returns <c>false</c>.
        /// </summary>
        /// <value>
        /// <c>true</c> if graph supports potential weight evaluation; otherwise, <c>false</c>.
        /// </value>
        public override bool SupportsPotentialWeightEvaluation => false;
    }
}
