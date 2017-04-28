﻿/* Copyright 2017 by Alexandru Ciobanu (alex+git@ciobanu.org)
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

using System.Diagnostics;
using System.Linq;

namespace Abacaxi.Graphs
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// A maze-structured graph.
    /// </summary>
    public sealed class LiteralGraph : Graph<char>
    {
        private readonly Dictionary<char, ISet<char>> _vertices;

        private void AddRelation(char from, char to)
        {
            Debug.Assert(char.IsLetterOrDigit(from));
            Debug.Assert(char.IsLetterOrDigit(to));
            Debug.Assert(_vertices != null);

            if (!_vertices.TryGetValue(from, out ISet<char> fromToSet))
            {
                fromToSet = new HashSet<char>();
                _vertices.Add(from, fromToSet);
            }
            if (!_vertices.TryGetValue(to, out ISet<char> toFromSet))
            {
                toFromSet = new HashSet<char>();
                _vertices.Add(to, toFromSet);
            }

            if (!fromToSet.Add(to))
            {
                throw new InvalidOperationException($"Vertex '{from}' already contains an edge to vertex '{to}'.");
            }
        }

        private bool ParseRelations(string relationships)
        {
            Debug.Assert(relationships != null);

            var ci = 0;
            var stage = -1;

            var vertexChars = new char[2];
            var fromToEdge = false;
            var toFromEdge = false;

            while (ci < relationships.Length)
            {
                var c = relationships[ci];
                if (!char.IsWhiteSpace(c))
                {
                    if (stage == -1)
                    {
                        stage = 0;
                    }

                    switch (stage)
                    {
                        case 0:
                        case 2:
                            if (!char.IsLetterOrDigit(c))
                            {
                                throw new FormatException(
                                    $"Invalid character \"{c}\" found at position {ci} in \"{relationships}\".");
                            }

                            vertexChars[stage / 2] = c;

                            stage += 1;
                            break;
                        case 1:
                            switch (c)
                            {
                                case '-':
                                    fromToEdge = true;
                                    toFromEdge = true;
                                    break;
                                case '>':
                                    fromToEdge = true;
                                    toFromEdge = false;
                                    break;
                                case '<':
                                    fromToEdge = false;
                                    toFromEdge = true;
                                    break;
                                default:
                                    throw new FormatException(
                                        $"Invalid character \"{c}\" found at position {ci} in \"{relationships}\".");
                            }

                            stage = 2;
                            break;
                        case 3:
                            if (c != ',')
                            {
                                throw new FormatException(
                                    $"Invalid character \"{c}\" found at position {ci} in \"{relationships}\".");
                            }

                            if (fromToEdge)
                            {
                                AddRelation(vertexChars[0], vertexChars[1]);
                            }
                            if (toFromEdge)
                            {
                                AddRelation(vertexChars[1], vertexChars[0]);
                            }

                            stage = 0;
                            break;
                    }
                }

                ci++;
            }

            if (stage == 3)
            {
                if (fromToEdge)
                {
                    AddRelation(vertexChars[0], vertexChars[1]);
                }
                if (toFromEdge)
                {
                    AddRelation(vertexChars[1], vertexChars[0]);
                }
            }
            else if (stage != 0)
            {
                throw new FormatException($"Unexpected end in the relationship string: \"{relationships}\".");
            }

            return _vertices.All(f => f.Value.All(t => _vertices[t].Contains(f.Key)));
        }

        /// <summary>
        /// Gets a value indicating whether this graph's edges are directed.
        /// </summary>
        /// <value>
        ///     <c>true</c> if the graph is directed; <c>false</c> otherwise.
        /// </value>
        public override bool IsDirected { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LiteralGraph"/> class.
        /// </summary>
        /// <param name="relationships">The vertex relationship definitions.</param>
        public LiteralGraph(string relationships)
        {
            Validate.ArgumentNotNull(nameof(relationships), relationships);

            _vertices = new Dictionary<char, ISet<char>>();
            IsDirected = ParseRelations(relationships);
        }

        /// <summary>
        /// Gets all vertices in the graph.
        /// </summary>
        /// <returns>
        /// The sequence of all vertices in the graph.
        /// </returns>
        public override IEnumerable<char> GetVertices()
        {
            return _vertices.Keys;
        }

        /// <summary>
        /// Gets the edges for a given <param name="vertex" />.
        /// </summary>
        /// <param name="vertex"></param>
        /// <returns>
        /// A sequence of edges connected to the given <param name="vertex" />
        /// </returns>
        /// <exception cref="InvalidOperationException">Thrown if the <paramref name="vertex"/> is not part of the graph.</exception>
        public override IEnumerable<Edge<char>> GetEdges(char vertex)
        {
            // ReSharper disable once CollectionNeverUpdated.Local
            if (!_vertices.TryGetValue(vertex, out ISet<char> list))
            {
                throw new InvalidOperationException($"Vertex {vertex} is not part of this graph.");
            }

            return list.Select(s => new Edge<char>(vertex, s));
        }
    }
}