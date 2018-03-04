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

namespace Abacaxi.Containers
{
    using System.Collections.Generic;
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

        [NotNull] private readonly IEqualityComparer<T> _comparer;
        [NotNull] private readonly IDictionary<T, Node> _nodes;

        [NotNull]
        private Node GetRootNodeRecursive([NotNull] T @object)
        {
            Assert.NotNull(@object);
            Assert.Condition(_nodes.ContainsKey(@object));

            var node = _nodes[@object];
            if (_comparer.Equals(@object, node.Parent))
            {
                return node;
            }

            var parentNode = GetRootNodeRecursive(node.Parent);
            node.Parent = parentNode.Parent;

            return parentNode;

        }

        [NotNull]
        private Node GetRootNode([NotNull] T @object)
        {
            Assert.NotNull(@object);

            if (_nodes.TryGetValue(@object, out var node))
            {
                return GetRootNodeRecursive(@object);
            }

            node = new Node
            {
                Parent = @object,
                Rank = 1
            };

            _nodes.Add(@object, node);
            return node;

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
        public T Merge([NotNull] T @object, [NotNull] params T[] otherObjects)
        {
            Validate.ArgumentNotNull(nameof(@object), @object);
            Validate.ArgumentNotNull(nameof(otherObjects), otherObjects);

            var roots = new Node[otherObjects.Length + 1];
            roots[0] = GetRootNode(@object);
            Assert.NotNull(roots[0]);
            var heaviest = roots[0];

            if (otherObjects.Length <= 0)
            {
                return heaviest.Parent;
            }

            for (var i = 0; i < otherObjects.Length; i++)
            {
                Validate.ArgumentNotNull(nameof(otherObjects), otherObjects[i]);

                var root = GetRootNode(otherObjects[i]);
                roots[i + 1] = root;

                Assert.NotNull(heaviest);
                if (root.Rank > heaviest.Rank)
                {
                    heaviest = root;
                }
            }

            foreach (var r in roots)
            {
                if (_comparer.Equals(r.Parent, heaviest.Parent))
                {
                    continue;
                }

                heaviest.Rank += r.Rank;
                r.Parent = heaviest.Parent;
            }

            return heaviest.Parent;
        }

        /// <summary>
        /// Checks whether two objects are in the same sub-set. If the objects are in different sub-sets, they are merged.
        /// </summary>
        /// <param name="object1">The first object.</param>
        /// <param name="object2">The second object.</param>
        /// <returns><c>true</c> if the objects are in the same set already; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown if <paramref name="object1"/> or <paramref name="object2"/> is <c>null</c>.</exception>
        public bool CheckAndMerge([NotNull] T object1, [NotNull] T object2)
        {
            Validate.ArgumentNotNull(nameof(object1), object1);
            Validate.ArgumentNotNull(nameof(object2), object2);

            var root1 = GetRootNode(object1);
            var root2 = GetRootNode(object2);

            if (_comparer.Equals(root1.Parent, root2.Parent))
            {
                return true;
            }

            if (root1.Rank > root2.Rank)
            {
                root1.Rank += root2.Rank;
                root2.Parent = root1.Parent;
            }
            else
            {
                root2.Rank += root1.Rank;
                root1.Parent = root2.Parent;
            }

            return false;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the set.
        /// </summary>
        /// <returns>
        /// An enumerator that can be used to iterate through the set.
        /// </returns>
        public IEnumerator<T> GetEnumerator() => _nodes.Keys.GetEnumerator();

        /// <summary>
        /// Returns an enumerator that iterates through the set.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the set.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}