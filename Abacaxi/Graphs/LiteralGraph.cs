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
    using Internal;
    using JetBrains.Annotations;

    /// <summary>
    /// A graph used mostly for designing algorithms. Each vertex is a digit or letter and each vertex can be connected
    /// in undirected or directed fashion to other vertices.
    /// </summary>
    [PublicAPI]
    public sealed class LiteralGraph : Graph<char>
    {
        private readonly Dictionary<char, IList<char>> _vertices;

        private void AddVertex(char vertex)
        {
            Debug.Assert(char.IsLetterOrDigit(vertex));
            Debug.Assert(_vertices != null);

            if (!_vertices.TryGetValue(vertex, out IList<char> set))
            {
                set = new List<char>();
                _vertices.Add(vertex, set);
            }
            else
            {
                throw new InvalidOperationException($"Vertex '{vertex}' has already been defined.");
            }
        }

        private void AddVertices(char from, char to)
        {
            Debug.Assert(char.IsLetterOrDigit(from));
            Debug.Assert(char.IsLetterOrDigit(to));
            Debug.Assert(_vertices != null);

            if (!_vertices.TryGetValue(from, out IList<char> fromToSet))
            {
                fromToSet = new List<char>();
                _vertices.Add(from, fromToSet);
            }
            if (!_vertices.TryGetValue(to, out IList<char> toFromSet))
            {
                toFromSet = new List<char>();
                _vertices.Add(to, toFromSet);
            }

            fromToSet.Add(to);
        }

        private void AddVertices(char from, char to, char relation)
        {
            switch (relation)
            {
                case '>':
                    AddVertices(from, to);
                    break;
                case '<':
                    AddVertices(to, from);
                    break;
                case '-':
                    AddVertices(from, to);

                    if (to != from)
                    {
                        AddVertices(to, from);
                    }
                    break;
                default:
                    Debug.Fail("Unexpected relation character.");
                    break;
            }
        }


        private void Parse(string relationships)
        {
            Debug.Assert(relationships != null);

            // ReSharper disable once IdentifierTypo
            var rels = new HashSet<char> { '-' };
            if (IsDirected)
            {
                rels.Add('>');
                rels.Add('<');
            }

            var vertices = new char[2];
            var relation = '\0';
            var stage = 0;
            for (var i = 0; i < relationships.Length; i++)
            {
                var c = relationships[i];

                switch (stage)
                {
                    case 0: /* Expect char or whitespace. */
                        if (char.IsWhiteSpace(c))
                        {
                            continue;
                        }
                        if (!char.IsLetterOrDigit(c))
                        {
                            throw new FormatException($"Invalid character \"{c}\" found at position {i}. Expected a letter or digit.");
                        }

                        vertices[0] = c;
                        stage = 1;
                        break;
                    case 1: /* Expect relationship char or whitespace or comma */
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
                    case 2: /* Expect char or whitespace. */
                        if (char.IsWhiteSpace(c))
                        {
                            continue;
                        }
                        if (!char.IsLetterOrDigit(c))
                        {
                            throw new FormatException($"Invalid character \"{c}\" found at position {i}. Expected a letter or digit.");
                        }

                        vertices[1] = c;
                        stage = 3;
                        break;
                    case 3: /* Expect comma or whitespace */
                        if (char.IsWhiteSpace(c))
                        {
                            continue;
                        }
                        if (c != ',')
                        {
                            throw new FormatException($"Invalid character \"{c}\" found at position {i}. Expected comma.");
                        }

                        AddVertices(vertices[0], vertices[1], relation);

                        stage = 0;
                        break;
                }
            }

            switch (stage)
            {
                case 3:
                    AddVertices(vertices[0], vertices[1], relation);
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
        /// Initializes a new instance of the <see cref="LiteralGraph"/> class.
        /// </summary>
        /// <param name="relationships">The vertex relationship definitions.</param>
        /// <param name="isDirected">Specifies whether the graph is directed.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="relationships"/> is <c>null</c>.</exception>
        public LiteralGraph(string relationships, bool isDirected)
        {
            Validate.ArgumentNotNull(nameof(relationships), relationships);

            _vertices = new Dictionary<char, IList<char>>();

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
            if (!_vertices.TryGetValue(vertex, out IList<char> list))
            {
                throw new InvalidOperationException($"Vertex {vertex} is not part of this graph.");
            }

            return list.Select(s => new Edge<char>(vertex, s));
        }
    }
}
