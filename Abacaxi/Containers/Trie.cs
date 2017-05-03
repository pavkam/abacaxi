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
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;

    /// <summary>
    /// Class implements a "trie" data structure, perfectly suited for fast string matching.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    public sealed class Trie<TValue> : ICollection<KeyValuePair<string, TValue>>, IReadOnlyCollection<KeyValuePair<string, TValue>>
    {
        private class Node
        {
            public char Char;
            public Node Parent;
            public TValue Value;
            public bool IsTerminal;
            public readonly Dictionary<char, Node> Children = new Dictionary<char, Node>();
        }

        private static readonly IEqualityComparer<TValue> ValueDefaultComparer =
            EqualityComparer<TValue>.Default;

        private int _ver;
        private Node _root;

        private bool FlowDown(string key, out Node node)
        {
            Debug.Assert(key != null);

            var i = 0;
            node = _root;
            while (i < key.Length && node.Children.TryGetValue(key[i], out Node child))
            {
                node = child;
                i++;
            }

            return i == key.Length;
        }

        private static void SwipeUselessBranch(Node node)
        {
            while (!node.IsTerminal && node.Children.Count == 0 && node.Parent != null)
            {
                node.Parent.Children.Remove(node.Char);
                node = node.Parent;
            }
        }

        /// <summary>
        /// Initializes a new instance of <see cref="Trie{TValue}"/> class.
        /// </summary>
        public Trie()
        {
            _root = new Node();
        }

        /// <summary>
        /// Initializes a new instance of <see cref="Trie{TValue}"/> class.
        /// </summary>
        /// <param name="sequence">Ane existing collection of key/value pairs to store in the trie.</param>
        /// <exception cref="ArgumentException">Thrown if the <paramref name="sequence"/> contains duplicate keys.</exception>
        /// <exception cref="ArgumentNullException">Thrown if the <paramref name="sequence"/> contains <c>null</c> keys.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="sequence"/> is <c>null</c>.</exception>
        public Trie(IEnumerable<KeyValuePair<string, TValue>> sequence) : this()
        {
            Validate.ArgumentNotNull(nameof(sequence), sequence);

            foreach (var kvp in sequence)
            {
                Add(kvp);
            }
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
        public IEnumerator<KeyValuePair<string, TValue>> GetEnumerator()
        {
            var stack = new Stack<KeyValuePair<string, Node>>();
            stack.Push(new KeyValuePair<string, Node>(string.Empty, _root));

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
                    yield return new KeyValuePair<string, TValue>(c.Key, c.Value.Value);
                }

                foreach (var n in c.Value.Children.Values)
                {
                    stack.Push(new KeyValuePair<string, Node>(c.Key + n.Char, n));
                }
            }
        }

        /// <summary>
        /// Queries this <see cref="Trie{TValue}"/> for all key/value pairs that start with a given <paramref name="prefix"/>.
        /// </summary>
        /// <param name="prefix">The prefix to query.</param>
        /// <returns>The sequence of all key/value pairs that share the given <paramref name="prefix"/>.</returns>
        /// <exception cref="System.InvalidOperationException">Collection has been modified while enumerating.</exception>
        public IEnumerable<KeyValuePair<string, TValue>> Query(string prefix)
        {
            Validate.ArgumentNotNull(nameof(prefix), prefix);

            if (FlowDown(prefix, out Node root))
            {
                var stack = new Stack<KeyValuePair<string, Node>>();
                stack.Push(new KeyValuePair<string, Node>(prefix, root));

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
                        yield return new KeyValuePair<string, TValue>(c.Key, c.Value.Value);
                    }

                    foreach (var n in c.Value.Children.Values)
                    {
                        stack.Push(new KeyValuePair<string, Node>(c.Key + n.Char, n));
                    }
                }
            }
        }

        /// <summary>
        /// Adds an item to the <see cref="Trie{TValue}" />.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="Trie{TValue}" />.</param>
        /// <exception cref="ArgumentException">The key component of <paramref name="item"/> is already present in the collection.</exception>
        /// <exception cref="ArgumentNullException">The key component of <paramref name="item"/> is <c>null</c>.</exception>
        public void Add(KeyValuePair<string, TValue> item)
        {
            Add(item.Key, item.Value);
        }

        /// <summary>
        /// Adds the specified key/value pair into the <see cref="Trie{TValue}"/>.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <exception cref="ArgumentException">The <paramref name="key"/> is already present in the collection.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="key"/> is <c>null</c>.</exception>
        public void Add(string key, TValue value)
        {
            Validate.ArgumentNotNull(nameof(key), key);

            var i = 0;
            var node = _root;
            while (i < key.Length && node.Children.TryGetValue(key[i], out Node child))
            {
                node = child;
                i++;
            }

            if (i == key.Length)
            {
                if (node.IsTerminal)
                {
                    throw new ArgumentException($"Key \"{key}\" has already been inserted into the collection.");
                }

                node.IsTerminal = true;
                node.Value = value;
                Count++;
                _ver++;
            }
            else
            {
                for (var r = i; r < key.Length; r++)
                {
                    var nn = new Node()
                    {
                        Parent = node,
                        Char = key[r],
                    };

                    node.Children.Add(key[r], nn);
                    node = nn;
                }

                node.IsTerminal = true;
                node.Value = value;
                Count++;
                _ver++;
            }
        }

        /// <summary>
        /// Removes all items from the <see cref="Trie{TValue}" />.
        /// </summary>
        public void Clear()
        {
            _root = new Node();
            Count = 0;
            _ver++;
        }

        /// <summary>
        /// Determines whether the <see cref="Trie{TValue}" /> contains a specific value.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="Trie{TValue}" />.</param>
        /// <returns>
        /// true if <paramref name="item" /> is found in the <see cref="Trie{TValue}" />; otherwise, false.
        /// </returns>
        /// <exception cref="ArgumentNullException">The key component of <paramref name="item"/> is <c>null</c>.</exception>
        public bool Contains(KeyValuePair<string, TValue> item)
        {
            Validate.ArgumentNotNull(nameof(item.Key), item.Key);

            return
                FlowDown(item.Key, out Node node) &&
                node.IsTerminal &&
                ValueDefaultComparer.Equals(node.Value, item.Value);
        }

        /// <summary>
        /// Determines whether this <see cref="Trie{TValue}"/> contains the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>
        ///   <c>true</c> if the <see cref="Trie{TValue}"/> contains the specified key; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="ArgumentNullException">The <paramref name="key"/> is <c>null</c>.</exception>
        public bool Contains(string key)
        {
            Validate.ArgumentNotNull(nameof(key), key);

            return
                FlowDown(key, out Node node) &&
                node.IsTerminal;
        }

        /// <summary>
        /// Copies the elements of the <see cref="Trie{TValue}" /> to an <see cref="T:System.Array" />, starting at a particular <see cref="T:System.Array" /> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied from <see cref="Trie{T}" />. The <see cref="T:System.Array" /> must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in <paramref name="array" /> at which copying begins.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="array"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="arrayIndex"/> is out of bounds or there is not enough space in the array.</exception>
        public void CopyTo(KeyValuePair<string, TValue>[] array, int arrayIndex)
        {
            Validate.CollectionArgumentsInBounds(nameof(array), array, arrayIndex, Count);

            foreach (var e in this)
            {
                array[arrayIndex++] = e;
            }
        }

        /// <summary>
        /// Removes the specified key/value pair from the <see cref="Trie{TValue}"/>.
        /// </summary>
        /// <param name="item">The key/value pair to remove.</param>
        /// <returns><c>true</c> if the key/value pair was found and removed; <c>false</c> otherwise.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the key component of <paramref name="item"/> is <c>null</c>.</exception>
        public bool Remove(KeyValuePair<string, TValue> item)
        {
            Validate.ArgumentNotNull(nameof(item.Key), item.Key);

            if (FlowDown(item.Key, out Node node) && 
                node.IsTerminal &&
                ValueDefaultComparer.Equals(node.Value, item.Value))
            {
                node.IsTerminal = false;
                SwipeUselessBranch(node);

                _ver++;
                Count--;

                return true;
            }

            return false;
        }

        /// <summary>
        /// Removes the specified key (and the associated value) from the <see cref="Trie{TValue}"/>.
        /// </summary>
        /// <param name="key">The key to remove.</param>
        /// <returns><c>true</c> if the key was found and removed; <c>false</c> otherwise.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the <paramref name="key"/> is <c>null</c>.</exception>
        public bool Remove(string key)
        {
            Validate.ArgumentNotNull(nameof(key), key);

            if (FlowDown(key, out Node node) && node.IsTerminal)
            {
                node.IsTerminal = false;
                SwipeUselessBranch(node);

                _ver++;
                Count--;

                return true;
            }

            return false;
        }

        /// <summary>
        /// Gets the number of elements contained in the <see cref="Trie{TValue}" />.
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the <see cref="Trie{TValue}" /> is read-only.
        /// </summary>
        public bool IsReadOnly => false;
    }
}
