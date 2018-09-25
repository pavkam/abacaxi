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

namespace Abacaxi.Trees
{
    using System.Collections.Generic;
    using Internal;
    using JetBrains.Annotations;

    /// <summary>
    ///     Class implements the left-leaning red-black balanced search tree.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    [PublicAPI]
    public sealed class LeftLeaningRedBlackTree<TKey, TValue> : BinarySearchTree<TKey, TValue>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="LeftLeaningRedBlackTree{TKey, TValue}" /> class.
        /// </summary>
        /// <param name="comparer">The key comparer used.</param>
        /// <exception cref="System.ArgumentNullException">Thrown if <paramref name="comparer" /> is <c>null</c>.</exception>
        public LeftLeaningRedBlackTree([NotNull] IComparer<TKey> comparer) : base(comparer)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="LeftLeaningRedBlackTree{TKey, TValue}" /> class using the default
        ///     <typeparamref name="TKey" /> comparer.
        /// </summary>
        public LeftLeaningRedBlackTree()
        {
        }

        /// <summary>
        ///     Gets or sets the root node of the AVL tree.
        /// </summary>
        /// <value>
        ///     The root.
        /// </value>
        [CanBeNull]
        public new RedBlackTreeNode<TKey, TValue> Root
        {
            get
            {
                Assert.Condition(base.Root == null || base.Root is RedBlackTreeNode<TKey, TValue>);
                return (RedBlackTreeNode<TKey, TValue>) base.Root;
            }
            set => base.Root = value;
        }

        private static bool IsRed([CanBeNull] RedBlackTreeNode<TKey, TValue> node)
        {
            return node?.Color == RedBlackTreeNodeColor.Red;
        }

        private static void FlipColor([NotNull] RedBlackTreeNode<TKey, TValue> node)
        {
            Assert.NotNull(node);
            Assert.NotNull(node.LeftChild);
            Assert.NotNull(node.RightChild);

            node.Color =
                node.Color == RedBlackTreeNodeColor.Black
                    ? RedBlackTreeNodeColor.Red
                    : RedBlackTreeNodeColor.Black;
            node.LeftChild.Color =
                node.LeftChild.Color == RedBlackTreeNodeColor.Black
                    ? RedBlackTreeNodeColor.Red
                    : RedBlackTreeNodeColor.Black;
            node.RightChild.Color =
                node.RightChild.Color == RedBlackTreeNodeColor.Black
                    ? RedBlackTreeNodeColor.Red
                    : RedBlackTreeNodeColor.Black;
        }

        [NotNull]
        private static RedBlackTreeNode<TKey, TValue> RotateLeft([NotNull] RedBlackTreeNode<TKey, TValue> node)
        {
            Assert.NotNull(node);

            var right = node.RightChild;
            Assert.NotNull(right);

            node.RightChild = right.LeftChild;
            right.LeftChild = node;
            right.Color = node.Color;
            node.Color = RedBlackTreeNodeColor.Red;

            return right;
        }

        [NotNull]
        private static RedBlackTreeNode<TKey, TValue> RotateRight([NotNull] RedBlackTreeNode<TKey, TValue> node)
        {
            Assert.NotNull(node);

            var left = node.LeftChild;
            Assert.NotNull(left);

            node.LeftChild = left.RightChild;
            left.RightChild = node;
            left.Color = node.Color;
            node.Color = RedBlackTreeNodeColor.Red;

            return left;
        }

        [NotNull]
        private static RedBlackTreeNode<TKey, TValue> MoveRedLeft([NotNull] RedBlackTreeNode<TKey, TValue> node)
        {
            Assert.NotNull(node);
            Assert.NotNull(node.RightChild);

            FlipColor(node);
            if (!IsRed(node.RightChild.LeftChild))
            {
                return node;
            }

            node.RightChild = RotateRight(node.RightChild);
            node = RotateLeft(node);

            FlipColor(node);

            Assert.NotNull(node.RightChild);
            if (IsRed(node.RightChild.RightChild))
            {
                node.RightChild = RotateLeft(node.RightChild);
            }

            return node;
        }

        [NotNull]
        private static RedBlackTreeNode<TKey, TValue> MoveRedRight([NotNull] RedBlackTreeNode<TKey, TValue> node)
        {
            Assert.NotNull(node);
            Assert.NotNull(node.LeftChild);

            FlipColor(node);
            if (!IsRed(node.LeftChild.LeftChild))
            {
                return node;
            }

            node = RotateRight(node);
            FlipColor(node);

            return node;
        }

        [CanBeNull]
        private RedBlackTreeNode<TKey, TValue> DeleteMinimumRecursive([NotNull] RedBlackTreeNode<TKey, TValue> node)
        {
            Assert.NotNull(node);

            if (node.LeftChild == null)
            {
                return null;
            }

            if (!IsRed(node.LeftChild) &&
                !IsRed(node.LeftChild.LeftChild))
            {
                node = MoveRedLeft(node);
            }

            Assert.NotNull(node.LeftChild);
            node.LeftChild = DeleteMinimumRecursive(node.LeftChild);

            return FixUp(node);
        }

        [NotNull]
        private static RedBlackTreeNode<TKey, TValue> FixUp([NotNull] RedBlackTreeNode<TKey, TValue> node)
        {
            Assert.NotNull(node);

            if (IsRed(node.RightChild))
            {
                node = RotateLeft(node);
            }

            if (IsRed(node.LeftChild))
            {
                Assert.NotNull(node.LeftChild);
                if (IsRed(node.LeftChild.LeftChild))
                {
                    node = RotateRight(node);
                }
            }

            if (IsRed(node.LeftChild) &&
                IsRed(node.RightChild))
            {
                FlipColor(node);
            }

            if (node.LeftChild == null ||
                !IsRed(node.LeftChild.RightChild) ||
                IsRed(node.LeftChild.LeftChild))
            {
                return node;
            }

            node.LeftChild = RotateLeft(node.LeftChild);
            if (IsRed(node.LeftChild))
            {
                node = RotateRight(node);
            }

            return node;
        }

        [NotNull]
        private RedBlackTreeNode<TKey, TValue> InsertRecursive(
            [CanBeNull] RedBlackTreeNode<TKey, TValue> node,
            TKey key,
            TValue value,
            bool allowUpdate)
        {
            if (node == null)
            {
                NotifyTreeChanged(1);

                return new RedBlackTreeNode<TKey, TValue>
                {
                    Key = key,
                    Value = value,
                    Color = RedBlackTreeNodeColor.Red
                };
            }

            if (IsRed(node.LeftChild) &&
                IsRed(node.RightChild))
            {
                FlipColor(node);
            }

            var comparisonResult = Comparer.Compare(key, node.Key);
            if (comparisonResult < 0)
            {
                node.LeftChild = InsertRecursive(node.LeftChild, key, value, allowUpdate);
            }
            else if (comparisonResult > 0)
            {
                node.RightChild = InsertRecursive(node.RightChild, key, value, allowUpdate);
            }
            else
            {
                if (allowUpdate)
                {
                    NotifyTreeChanged(0);
                    node.Value = value;
                }
                else
                {
                    ThrowDuplicateKeyFound(nameof(key));
                }
            }

            if (IsRed(node.RightChild))
            {
                node = RotateLeft(node);
            }

            if (!IsRed(node.LeftChild))
            {
                return node;
            }

            Assert.NotNull(node.LeftChild);
            if (IsRed(node.LeftChild.LeftChild))
            {
                node = RotateRight(node);
            }

            return node;
        }

        [CanBeNull]
        private RedBlackTreeNode<TKey, TValue> RemoveRecursive([NotNull] RedBlackTreeNode<TKey, TValue> node, TKey key)
        {
            if (Comparer.Compare(key, node.Key) < 0)
            {
                if (node.LeftChild == null)
                {
                    return FixUp(node);
                }

                if (!IsRed(node.LeftChild) &&
                    !IsRed(node.LeftChild.LeftChild))
                {
                    node = MoveRedLeft(node);
                }

                Assert.NotNull(node.LeftChild);
                node.LeftChild = RemoveRecursive(node.LeftChild, key);
            }
            else
            {
                if (IsRed(node.LeftChild))
                {
                    node = RotateRight(node);
                }

                if (Comparer.Compare(key, node.Key) == 0 &&
                    node.RightChild == null)
                {
                    Assert.Condition(node.LeftChild == null);

                    NotifyTreeChanged(-1);
                    return null;
                }

                if (node.RightChild == null)
                {
                    return FixUp(node);
                }

                if (!IsRed(node.RightChild) &&
                    !IsRed(node.RightChild.LeftChild))
                {
                    node = MoveRedRight(node);
                }

                Assert.NotNull(node.RightChild);

                if (Comparer.Compare(key, node.Key) == 0)
                {
                    NotifyTreeChanged(-1);

                    var m = node.RightChild;
                    while (m.LeftChild != null)
                    {
                        m = m.LeftChild;
                    }

                    node.Key = m.Key;
                    node.Value = m.Value;
                    node.RightChild = DeleteMinimumRecursive(node.RightChild);
                }
                else
                {
                    node.RightChild = RemoveRecursive(node.RightChild, key);
                }
            }

            return FixUp(node);
        }

        /// <summary>
        ///     Looks up the node ky the given <paramref name="key" />.
        /// </summary>
        /// <param name="key">The key of the node.</param>
        /// <returns>The node, if found; otherwise, <c>null</c>.</returns>
        [CanBeNull]
        public new RedBlackTreeNode<TKey, TValue> LookupNode(TKey key)
        {
            var node = base.LookupNode(key);
            Assert.Condition(node == null || node is RedBlackTreeNode<TKey, TValue>);

            return (RedBlackTreeNode<TKey, TValue>) node;
        }

        /// <summary>
        ///     Adds the specified key/value node to the tree.
        /// </summary>
        /// <param name="key">The node's key.</param>
        /// <param name="value">The node's value.</param>
        /// <exception cref="System.ArgumentException">
        ///     Thrown if a node with the same <paramref name="key" /> is already present in
        ///     the tree.
        /// </exception>
        public override void Add(TKey key, TValue value)
        {
            Root = InsertRecursive(Root, key, value, false);
            Root.Color = RedBlackTreeNodeColor.Black;
        }

        /// <summary>
        ///     Adds or updates a tree node that has a given key and value.
        /// </summary>
        /// <param name="key">The node's key.</param>
        /// <param name="value">The node's new value.</param>
        public override void AddOrUpdate(TKey key, TValue value)
        {
            Root = InsertRecursive(Root, key, value, true);
            Root.Color = RedBlackTreeNodeColor.Black;
        }

        /// <summary>
        ///     Removes the node from the tree that has a specified key.
        /// </summary>
        /// <param name="key">The node's key.</param>
        /// <returns><c>true</c> if the node was removed; otherwise, <c>false</c>.</returns>
        public override bool Remove(TKey key)
        {
            var initialCount = Count;
            if (Root == null)
            {
                return initialCount != Count;
            }

            Root = RemoveRecursive(Root, key);
            if (Root != null)
            {
                Root.Color = RedBlackTreeNodeColor.Black;
            }

            return initialCount != Count;
        }
    }
}