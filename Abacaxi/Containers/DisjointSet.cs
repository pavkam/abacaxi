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

namespace Abacaxi.Containers
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using Internal;
    using JetBrains.Annotations;
    using System.Collections;

    /// <summary>
    /// Class implements the "union-find" data structure. This data structure is optimized for find and merge set operations.
    /// This class does not act as the standard set data structure. See https://en.wikipedia.org/wiki/Disjoint-set_data_structure for details.
    /// </summary>
    [PublicAPI]
    public sealed class DisjointSet<T> : IEnumerable<T>
    {
        private sealed class Node
        {
            public int Rank;
            public T Parent;
        }

        [NotNull]
        private readonly IEqualityComparer<T> _comparer;
        [NotNull]
        private readonly IDictionary<T, Node> _nodes;

        [NotNull]
        private Node GetRootNodeRecursive([NotNull] T @object)
        {
            Debug.Assert(@object != null);
            Debug.Assert(_nodes.ContainsKey(@object));

            var node = _nodes[@object];
            if (!_comparer.Equals(@object, node.Parent))
            {
                var parentNode = GetRootNodeRecursive(node.Parent);
                node.Parent = parentNode.Parent;

                return parentNode;
            }

            return node;
        }

        [NotNull]
        private Node GetRootNode([NotNull] T @object)
        {
            Debug.Assert(@object != null);

            if (!_nodes.TryGetValue(@object, out var node))
            {
                node = new Node
                {
                    Parent = @object,
                    Rank = 1
                };

                _nodes.Add(@object, node);
                return node;
            }

            return GetRootNodeRecursive(@object);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DisjointSet{T}"/> class.
        /// </summary>
        /// <param name="comparer">The comparer.</param>
        /// <exception cref="System.ArgumentNullException">Thrown if <paramref name="comparer"/> is <c>null</c>.</exception>
        public DisjointSet([NotNull] IEqualityComparer<T> comparer)
        {
            Validate.ArgumentNotNull(nameof(comparer), comparer);
            _comparer = comparer;
            _nodes = new Dictionary<T, Node>(comparer);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DisjointSet{T}"/> class using the default equality comparer for <typeparamref name="T"/>.
        /// </summary>
        public DisjointSet() : this(EqualityComparer<T>.Default)
        {
        }

        /// <summary>
        /// Gets the root object (called label) that identifies the sub-set that contains this <paramref name="object"/>.
        /// If <paramref name="object"/> is not stored in this <see cref="DisjointSet{T}"/>, it is added into its own set and 
        /// the return value is itself.
        /// </summary>
        /// <value>
        /// The "set label" object.
        /// </value>
        /// <param name="object">The object to check.</param>
        /// <returns>The object which serves as the "set label".</returns>
        /// <exception cref="System.ArgumentNullException">Thrown if <paramref name="object"/> is <c>null</c>.</exception>
        [NotNull]
        public T this[[NotNull] T @object]
        {
            get
            {
                Validate.ArgumentNotNull(nameof(@object), @object);
                return GetRootNode(@object).Parent;
            }
        }

        /// <summary>
        /// Merges the sets that contain the objects specified as input arguments and returns the "set label" object of the merged set.
        /// If only one argument is supplied, and it's not already in the set, a new set is created containing that single item.
        /// </summary>
        /// <param name="object">The first object.</param>
        /// <param name="otherObjects">The other objects.</param>
        /// <returns>The "set label" object that identifies the merged set.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown if <paramref name="object"/>, 
        /// <paramref name="otherObjects"/>, or its contents, are <c>null</c>.</exception>
        public T Merge([NotNull]T @object, [NotNull] params T[] otherObjects)
        {
            Validate.ArgumentNotNull(nameof(@object), @object);
            Validate.ArgumentNotNull(nameof(otherObjects), otherObjects);

            var roots = new Node[otherObjects.Length + 1];
            roots[0] = GetRootNode(@object);
            Debug.Assert(roots[0] != null);
            var heaviest = roots[0];

            if (otherObjects.Length > 0)
            {
                for (var i = 0; i < otherObjects.Length; i++)
                {
                    Validate.ArgumentNotNull(nameof(otherObjects), otherObjects[i]);

                    var root = GetRootNode(otherObjects[i]);
                    roots[i + 1] = root;

                    Debug.Assert(heaviest != null);
                    if (root.Rank > heaviest.Rank)
                    {
                        heaviest = root;
                    }
                }

                foreach (var r in roots)
                {
                    if (!_comparer.Equals(r.Parent, heaviest.Parent))
                    {
                        heaviest.Rank += r.Rank;
                        r.Parent = heaviest.Parent;
                    }
                }
            }

            return heaviest.Parent;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the set.
        /// </summary>
        /// <returns>
        /// An enumerator that can be used to iterate through the set.
        /// </returns>
        public IEnumerator<T> GetEnumerator()
        {
            return _nodes.Keys.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the set.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the set.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
