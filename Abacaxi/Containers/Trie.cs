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
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using Internal;
    using JetBrains.Annotations;

    /// <summary>
    /// Class implements a "trie" data structure, perfectly suited for fast string matching.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <typeparam name="TElement">The element of array keys.</typeparam>
    [PublicAPI]
    public sealed class Trie<TElement, TValue> : ICollection<KeyValuePair<TElement[], TValue>>,
        IReadOnlyCollection<KeyValuePair<TElement[], TValue>>
    {
        private class Node
        {
            public bool IsTerminal;
            public TValue Value;
            [NotNull] public readonly Dictionary<TElement, Node> Children;

            public Node([NotNull] IEqualityComparer<TElement> comparer)
            {
                Debug.Assert(comparer != null);
                Children = new Dictionary<TElement, Node>(comparer);
            }
        }

        [NotNull] private static readonly IEqualityComparer<TValue> ValueDefaultComparer =
            EqualityComparer<TValue>.Default;

        [NotNull] private static readonly TElement[] EmptyElementArray = { };

        [NotNull] private readonly IEqualityComparer<TElement> _comparer;
        private int _ver;
        [NotNull] private Node _root;

        private bool FlowDown([NotNull] IList<TElement> key, [NotNull] out Node node)
        {
            Debug.Assert(key != null);

            var i = 0;
            node = _root;
            while (i < key.Count && node.Children.TryGetValue(key[i], out var child))
            {
                node = child;
                i++;
            }

            return i == key.Count;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="Trie{TElement,TValue}"/> class.
        /// </summary>
        /// <param name="comparer">The element comparer.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="comparer"/> is <c>null</c>.</exception>
        public Trie([NotNull] IEqualityComparer<TElement> comparer)
        {
            Validate.ArgumentNotNull(nameof(comparer), comparer);

            _comparer = comparer;
            _root = new Node(_comparer);
        }

        /// <summary>
        /// Initializes a new instance of <see cref="Trie{TElement,TValue}"/> class.
        /// </summary>
        public Trie() : this(EqualityComparer<TElement>.Default)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="Trie{TElement,TValue}"/> class.
        /// </summary>
        /// <param name="sequence">Ane existing collection of key/value pairs to store in the trie.</param>
        /// <param name="comparer">The element comparer.</param>
        /// <exception cref="ArgumentException">Thrown if the <paramref name="sequence"/> contains duplicate keys.</exception>
        /// <exception cref="ArgumentNullException">Thrown if the <paramref name="sequence"/> contains <c>null</c> keys.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="sequence"/> is <c>null</c>.</exception>
        public Trie(
            [NotNull] IEnumerable<KeyValuePair<TElement[], TValue>> sequence,
            [NotNull] IEqualityComparer<TElement> comparer) : this(comparer)
        {
            Validate.ArgumentNotNull(nameof(sequence), sequence);

            foreach (var kvp in sequence)
            {
                Add(kvp.Key, kvp.Value);
            }
        }

        /// <summary>
        /// Initializes a new instance of <see cref="Trie{TElement,TValue}"/> class.
        /// </summary>
        /// <param name="sequence">Ane existing collection of key/value pairs to store in the trie.</param>
        /// <exception cref="ArgumentException">Thrown if the <paramref name="sequence"/> contains duplicate keys.</exception>
        /// <exception cref="ArgumentNullException">Thrown if the <paramref name="sequence"/> contains <c>null</c> keys.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="sequence"/> is <c>null</c>.</exception>
        public Trie(
            [NotNull] IEnumerable<KeyValuePair<TElement[], TValue>> sequence) : this(sequence,
            EqualityComparer<TElement>.Default)
        {
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// An enumerator that can be used to iterate through the collection.
        /// </returns>
        /// <exception cref="InvalidOperationException">Collection has been modified while enumerating.</exception>
        public IEnumerator<KeyValuePair<TElement[], TValue>> GetEnumerator()
        {
            return Query(EmptyElementArray).GetEnumerator();
        }

        /// <summary>
        /// Queries this <see cref="Trie{TElement,TValue}"/> for all key/value pairs that start with a given <paramref name="prefix"/>.
        /// </summary>
        /// <param name="prefix">The prefix to query.</param>
        /// <returns>The sequence of all key/value pairs that share the given <paramref name="prefix"/>.</returns>
        /// <exception cref="InvalidOperationException">Collection has been modified while enumerating.</exception>
        [NotNull]
        public IEnumerable<KeyValuePair<TElement[], TValue>> Query([NotNull] TElement[] prefix)
        {
            Validate.ArgumentNotNull(nameof(prefix), prefix);

            if (FlowDown(prefix, out var root))
            {
                var stack = new Stack<KeyValuePair<TElement[], Node>>();
                stack.Push(new KeyValuePair<TElement[], Node>(prefix, root));

                var ver = _ver;
                while (stack.Count > 0)
                {
                    if (ver != _ver)
                    {
                        throw new InvalidOperationException("Collection has been modified while enumerating.");
                    }

                    var c = stack.Pop();
                    if (c.Value.IsTerminal)
                    {
                        yield return new KeyValuePair<TElement[], TValue>(c.Key, c.Value.Value);
                    }

                    foreach (var n in c.Value.Children)
                    {
                        stack.Push(new KeyValuePair<TElement[], Node>(c.Key.Append(n.Key), n.Value));
                    }
                }
            }
        }

        /// <summary>
        /// Adds an item to the <see cref="Trie{TElement,TValue}" />.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="Trie{TElement,TValue}" />.</param>
        /// <exception cref="ArgumentException">The key component of <paramref name="item"/> is already present in the collection.</exception>
        /// <exception cref="ArgumentNullException">The key component of <paramref name="item"/> is <c>null</c>.</exception>
        void ICollection<KeyValuePair<TElement[], TValue>>.Add(KeyValuePair<TElement[], TValue> item)
        {
            Add(item.Key, item.Value);
        }

        /// <summary>
        /// Adds the specified key/value pair into the <see cref="Trie{TElement,TValue}"/>.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <exception cref="ArgumentException">The <paramref name="key"/> is already present in the collection.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="key"/> is <c>null</c>.</exception>
        public void Add([NotNull] TElement[] key, TValue value)
        {
            AddOrUpdate(key, value,
                i => throw new ArgumentException($"Key \"{key}\" has already been inserted into the collection."));
        }

        /// <summary>
        /// Adds a new, or updates and existing element.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="updateFunc">The update function.</param>
        /// <returns><c>true</c> if the key/value was added; otherwise <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="key"/> or <paramref name="updateFunc"/> are <c>null</c>.</exception>
        public bool AddOrUpdate([NotNull] TElement[] key, TValue value, [NotNull] Func<TValue, TValue> updateFunc)
        {
            Validate.ArgumentNotNull(nameof(key), key);
            Validate.ArgumentNotNull(nameof(updateFunc), updateFunc);

            var i = 0;
            var node = _root;
            while (i < key.Length && node.Children.TryGetValue(key[i], out var child))
            {
                node = child;
                i++;
            }

            if (i == key.Length)
            {
                if (node.IsTerminal)
                {
                    node.Value = updateFunc(value);
                    return false;
                }
                else
                {
                    node.IsTerminal = true;
                    node.Value = value;
                    Count++;
                }

                _ver++;
            }
            else
            {
                for (var r = i; r < key.Length; r++)
                {
                    var nn = new Node(_comparer);

                    node.Children.Add(key[r], nn);
                    node = nn;
                }

                node.IsTerminal = true;
                node.Value = value;
                Count++;
                _ver++;
            }

            return true;
        }

        /// <summary>
        /// Removes all items from the <see cref="Trie{TElement,TValue}" />.
        /// </summary>
        public void Clear()
        {
            _root = new Node(_comparer);
            Count = 0;
            _ver++;
        }

        /// <summary>
        /// Determines whether the <see cref="Trie{TElement,TValue}" /> contains a specific value.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="Trie{TElement,TValue}" />.</param>
        /// <returns>
        /// true if <paramref name="item" /> is found in the <see cref="Trie{TElement,TValue}" />; otherwise, false.
        /// </returns>
        /// <exception cref="ArgumentNullException">The key component of <paramref name="item"/> is <c>null</c>.</exception>
        bool ICollection<KeyValuePair<TElement[], TValue>>.Contains(KeyValuePair<TElement[], TValue> item)
        {
            Validate.ArgumentNotNull(nameof(item.Key), item.Key);

            return
                FlowDown(item.Key, out var node) &&
                node.IsTerminal &&
                ValueDefaultComparer.Equals(node.Value, item.Value);
        }

        /// <summary>
        /// Determines whether this <see cref="Trie{TElement,TValue}"/> contains the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>
        ///   <c>true</c> if the <see cref="Trie{TElement,TValue}"/> contains the specified key; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="ArgumentNullException">The <paramref name="key"/> is <c>null</c>.</exception>
        public bool Contains([NotNull] TElement[] key)
        {
            Validate.ArgumentNotNull(nameof(key), key);

            return
                FlowDown(key, out var node) &&
                node.IsTerminal;
        }

        /// <summary>
        /// Copies the elements of the <see cref="Trie{TElement,TValue}" /> to an <see cref="T:System.Array" />, starting at a particular <see cref="T:System.Array" /> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied from <see cref="Trie{TElement,TValue}" />. The <see cref="T:System.Array" /> must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in <paramref name="array" /> at which copying begins.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="array"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="arrayIndex"/> is out of bounds or there is not enough space in the array.</exception>
        public void CopyTo(KeyValuePair<TElement[], TValue>[] array, int arrayIndex)
        {
            Validate.CollectionArgumentsInBounds(nameof(array), array, arrayIndex, Count);

            foreach (var e in this)
            {
                array[arrayIndex++] = e;
            }
        }

        /// <summary>
        /// Removes the specified key/value pair from the <see cref="Trie{TElement,TValue}"/>.
        /// </summary>
        /// <param name="item">The key/value pair to remove.</param>
        /// <returns><c>true</c> if the key/value pair was found and removed; <c>false</c> otherwise.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the key component of <paramref name="item"/> is <c>null</c>.</exception>
        bool ICollection<KeyValuePair<TElement[], TValue>>.Remove(KeyValuePair<TElement[], TValue> item)
        {
            Validate.ArgumentNotNull(nameof(item.Key), item.Key);

            return ((ICollection<KeyValuePair<TElement[], TValue>>) this).Contains(item) && Remove(item.Key);
        }

        /// <summary>
        /// Removes the specified key (and the associated value) from the <see cref="Trie{TElement,TValue}"/>.
        /// </summary>
        /// <param name="key">The key to remove.</param>
        /// <returns><c>true</c> if the key was found and removed; <c>false</c> otherwise.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the <paramref name="key"/> is <c>null</c>.</exception>
        public bool Remove([NotNull] TElement[] key)
        {
            Validate.ArgumentNotNull(nameof(key), key);

            var i = 0;
            var node = _root;
            var path = new Stack<Node>();
            while (i < key.Length && node.Children.TryGetValue(key[i], out var child))
            {
                path.Push(node);

                node = child;
                i++;
            }

            if (i == key.Length && node.IsTerminal)
            {
                node.IsTerminal = false;

                while (
                    !node.IsTerminal &&
                    node.Children.Count == 0 &&
                    path.Count > 0)
                {
                    var parent = path.Pop();

                    parent.Children.Remove(key[--i]);
                    node = parent;
                }

                _ver++;
                Count--;

                return true;
            }

            return false;
        }

        /// <summary>
        /// Tries to get the value associated with the <paramref name="key"/>.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The output value.</param>
        /// <returns><c>true</c> if the key was found; otherwise, <c>false</c>.</returns>
        public bool TryGetValue([NotNull] TElement[] key, out TValue value)
        {
            Validate.ArgumentNotNull(nameof(key), key);

            var success =
                FlowDown(key, out var node) && node.IsTerminal;
            value = success ? node.Value : default(TValue);

            return success;
        }

        /// <summary>
        /// Gets the number of elements contained in the <see cref="Trie{TElement,TValue}" />.
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the <see cref="Trie{TElement,TValue}" /> is read-only.
        /// </summary>
        public bool IsReadOnly => false;
    }
}