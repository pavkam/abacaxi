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
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Internal;
    using JetBrains.Annotations;

    /// <summary>
    /// Class handles dependencies and conflicts across tags. Allows maintaining a valid state when toggling on/off tags.
    /// </summary>
    /// <typeparam name="TTag">The type of the tag (item managed by the squid).</typeparam>
    [PublicAPI]
    public sealed class DependencySquid<TTag>
    {
        [NotNull] private readonly IDictionary<TTag, Node> _all;
        [NotNull] private readonly HashSet<Node> _selected;

        private sealed class Node
        {
            public TTag Tag;
            public ISet<Node> Conflicts;
            public ISet<Node> Dependencies;
            public ISet<Node> Dependents;
        }

        [NotNull]
        private Node GetOrAddNode([NotNull] TTag tag)
        {
            Assert.NotNull(tag);

            if (!_all.TryGetValue(tag, out var node))
            {
                node = new Node
                {
                    Conflicts = new HashSet<Node>(),
                    Dependencies = new HashSet<Node>(),
                    Dependents = new HashSet<Node>(),
                    Tag = tag
                };

                _all.Add(tag, node);
            }

            return node;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DependencySquid{TTag}"/> class.
        /// </summary>
        /// <param name="equalityComparer">The equality comparer.</param>
        /// <exception cref="ArgumentNullException">
        ///     Thrown if <paramref name="equalityComparer" /> is <c>null</c>.
        /// </exception>
        public DependencySquid([NotNull] IEqualityComparer<TTag> equalityComparer)
        {
            Validate.ArgumentNotNull(nameof(equalityComparer), equalityComparer);

            _all = new Dictionary<TTag, Node>(equalityComparer);
            _selected = new HashSet<Node>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DependencySquid{TTag}"/> class using the default equality comparer for <typeparamref name="TTag"/>.
        /// </summary>
        public DependencySquid() : this(EqualityComparer<TTag>.Default)
        {
        }

        /// <summary>
        /// Adds a number of dependencies for a given <paramref name="dependent"/>.
        /// </summary>
        /// <param name="dependent">The dependent.</param>
        /// <param name="dependencies">The dependencies to add.</param>
        /// <exception cref="ArgumentNullException">
        ///     Thrown if <paramref name="dependent" /> or <paramref name="dependencies"/> are null <c>null</c>.
        /// </exception>
        public void AddDependencies([NotNull] TTag dependent, [NotNull, ItemNotNull] params TTag[] dependencies)
        {
            Validate.ArgumentNotNull(nameof(dependent), dependent);

            var dependentNode = GetOrAddNode(dependent);
            foreach (var dependency in dependencies)
            {
                var dependencyNode = GetOrAddNode(dependency);

                dependentNode.Dependencies.Add(dependencyNode);
                dependencyNode.Dependents.Add(dependentNode);
            }
        }

        /// <summary>
        /// Adds a number of conflicts for a given <paramref name="dependent"/>.
        /// </summary>
        /// <param name="dependent">The dependent.</param>
        /// <param name="conflicts">The conflicts to add.</param>
        /// <exception cref="ArgumentNullException">
        ///     Thrown if <paramref name="dependent" /> or <paramref name="conflicts"/> are null <c>null</c>.
        /// </exception>
        public void AddConflicts([NotNull] TTag dependent, [NotNull, ItemNotNull] params TTag[] conflicts)
        {
            Validate.ArgumentNotNull(nameof(dependent), dependent);

            var dependentNode = GetOrAddNode(dependent);
            foreach (var conflict in conflicts)
            {
                var conflictNode = GetOrAddNode(conflict);
                dependentNode.Conflicts.Add(conflictNode);
            }
        }

        /// <summary>
        /// Removes a number of dependencies from a given <paramref name="dependent"/>.
        /// </summary>
        /// <param name="dependent">The dependent.</param>
        /// <param name="dependencies">The dependencies to remove.</param>
        /// <exception cref="ArgumentNullException">
        ///     Thrown if <paramref name="dependent" /> or <paramref name="dependencies"/> are null <c>null</c>.
        /// </exception>
        public void RemoveDependencies([NotNull] TTag dependent, [NotNull, ItemNotNull] params TTag[] dependencies)
        {
            Validate.ArgumentNotNull(nameof(dependent), dependent);

            if (!_all.TryGetValue(dependent, out var dependentNode))
            {
                return;
            }

            foreach (var dependency in dependencies)
            {
                if (!_all.TryGetValue(dependency, out var dependencyNode))
                {
                    continue;
                }

                dependentNode.Dependencies.Add(dependencyNode);
                dependencyNode.Dependents.Add(dependentNode);
            }
        }

        /// <summary>
        /// Removes a number of conflicts from a given <paramref name="dependent"/>.
        /// </summary>
        /// <param name="dependent">The dependent.</param>
        /// <param name="conflicts">The conflicts to remove.</param>
        /// <exception cref="ArgumentNullException">
        ///     Thrown if <paramref name="dependent" /> or <paramref name="conflicts"/> are null <c>null</c>.
        /// </exception>
        public void RemoveConflicts([NotNull] TTag dependent, [NotNull, ItemNotNull] params TTag[] conflicts)
        {
            Validate.ArgumentNotNull(nameof(dependent), dependent);

            if (!_all.TryGetValue(dependent, out var dependentNode))
            {
                return;
            }

            foreach (var conflict in conflicts)
            {
                if (!_all.TryGetValue(conflict, out var conflictNode))
                {
                    continue;
                }

                dependentNode.Conflicts.Add(conflictNode);
            }
        }

        /// <summary>
        /// Toggles the specified dependent.
        /// </summary>
        /// <remarks>
        /// Selected tags and all dependencies are selected. All conflicts are removed.
        /// </remarks>
        /// <param name="tag">The tag to toggle.</param>
        /// <param name="selected">If set to <c>true</c>, the tag is selected. Otherwise it is removed.</param>
        public void Toggle([NotNull] TTag tag, bool selected)
        {
            var node = GetOrAddNode(tag);
        }

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public void Clear()
        {
            _selected.Clear();
        }

        /// <summary>
        /// Gets the current tag selection.
        /// </summary>
        /// <value>
        /// The selection of tags.
        /// </value>
        [NotNull, ItemNotNull]
        public TTag[] Selection => _selected.Select(node => node.Tag).ToArray();
    }
}