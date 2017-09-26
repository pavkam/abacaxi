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

namespace Abacaxi.Trees
{
    using JetBrains.Annotations;

    /// <summary>
    /// Class represents a node in a binary search tree (or any derivative balanced search tree).
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    [PublicAPI]
    internal sealed class BinaryTreeNode<TKey, TValue>
    {
        /// <summary>
        /// Gets the key of the node.
        /// </summary>
        /// <value>
        /// The key.
        /// </value>
        [CanBeNull]
        public TKey Key { get; set; }

        /// <summary>
        /// Gets the value of the node.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        [CanBeNull]
        public TValue Value { get; set; }

        /// <summary>
        /// Gets the right child node.
        /// </summary>
        /// <value>
        /// The right child node.
        /// </value>
        [CanBeNull]
        public BinaryTreeNode<TKey, TValue> RightChild { get; set; }

        /// <summary>
        /// Gets the left child node.
        /// </summary>
        /// <value>
        /// The left child node.
        /// </value>
        [CanBeNull]
        public BinaryTreeNode<TKey, TValue> LeftChild { get; set; }
    }
}