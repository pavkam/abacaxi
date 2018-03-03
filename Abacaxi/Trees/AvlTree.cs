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

using System;

namespace Abacaxi.Trees
{
    using System.Diagnostics;
    using System.Collections.Generic;
    using JetBrains.Annotations;

    /// <summary>
    /// Class implements the AVL balanced search tree.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    [PublicAPI]
    public class AvlTree<TKey, TValue> : BinarySearchTree<TKey, TValue>
    {
        [NotNull]
        private AvlTreeNode<TKey, TValue> RotateLeft([NotNull] AvlTreeNode<TKey, TValue> node)
        {
            Debug.Assert(node != null);

            var right = node.RightChild;
            Debug.Assert(right != null);
            var rightLeft = right.LeftChild;
            var parent = node.Parent;

            right.Parent = parent;
            right.LeftChild = node;
            node.RightChild = rightLeft;
            node.Parent = right;

            if (rightLeft != null)
            {
                rightLeft.Parent = node;
            }

            if (node == Root)
            {
                Root = right;
            }
            else
            {
                Debug.Assert(parent != null);

                if (parent.RightChild == node)
                {
                    parent.RightChild = right;
                }
                else
                {
                    parent.LeftChild = right;
                }
            }

            right.Balance++;
            node.Balance = -right.Balance;

            return right;
        }

        [NotNull]
        private AvlTreeNode<TKey, TValue> RotateRight([NotNull] AvlTreeNode<TKey, TValue> node)
        {
            Debug.Assert(node != null);

            var left = node.LeftChild;
            Debug.Assert(left != null);

            var leftRight = left.RightChild;
            var parent = node.Parent;

            left.Parent = parent;
            left.RightChild = node;
            node.LeftChild = leftRight;
            node.Parent = left;

            if (leftRight != null)
            {
                leftRight.Parent = node;
            }

            if (node == Root)
            {
                Root = left;
            }
            else
            {
                Debug.Assert(parent != null);
                if (parent.LeftChild == node)
                {
                    parent.LeftChild = left;
                }
                else
                {
                    parent.RightChild = left;
                }
            }

            left.Balance--;
            node.Balance = -left.Balance;

            return left;
        }

        [NotNull]
        private AvlTreeNode<TKey, TValue> RotateLeftRight([NotNull] AvlTreeNode<TKey, TValue> node)
        {
            Debug.Assert(node != null);

            var left = node.LeftChild;
            Debug.Assert(left != null);
            Debug.Assert(left.RightChild != null);

            var leftRight = left.RightChild;
            var parent = node.Parent;
            var leftRightRight = leftRight.RightChild;
            var leftRightLeft = leftRight.LeftChild;

            leftRight.Parent = parent;
            node.LeftChild = leftRightRight;
            left.RightChild = leftRightLeft;
            leftRight.LeftChild = left;
            leftRight.RightChild = node;
            left.Parent = leftRight;
            node.Parent = leftRight;

            if (leftRightRight != null)
            {
                leftRightRight.Parent = node;
            }

            if (leftRightLeft != null)
            {
                leftRightLeft.Parent = left;
            }

            if (node == Root)
            {
                Root = leftRight;
            }
            else
            {
                Debug.Assert(parent != null);
                if (parent.LeftChild == node)
                {
                    parent.LeftChild = leftRight;
                }
                else
                {
                    parent.RightChild = leftRight;
                }
            }

            switch (leftRight.Balance)
            {
                case -1:
                    node.Balance = 0;
                    left.Balance = 1;
                    break;
                case 0:
                    node.Balance = 0;
                    left.Balance = 0;
                    break;
                default:
                    node.Balance = -1;
                    left.Balance = 0;
                    break;
            }

            leftRight.Balance = 0;

            return leftRight;
        }

        [NotNull]
        private AvlTreeNode<TKey, TValue> RotateRightLeft([NotNull] AvlTreeNode<TKey, TValue> node)
        {
            Debug.Assert(node != null);

            var right = node.RightChild;
            Debug.Assert(right != null);

            var rightLeft = right.LeftChild;
            Debug.Assert(rightLeft != null);

            var parent = node.Parent;
            var rightLeftLeft = rightLeft.LeftChild;
            var rightLeftRight = rightLeft.RightChild;

            rightLeft.Parent = parent;
            node.RightChild = rightLeftLeft;
            right.LeftChild = rightLeftRight;
            rightLeft.RightChild = right;
            rightLeft.LeftChild = node;
            right.Parent = rightLeft;
            node.Parent = rightLeft;

            if (rightLeftLeft != null)
            {
                rightLeftLeft.Parent = node;
            }

            if (rightLeftRight != null)
            {
                rightLeftRight.Parent = right;
            }

            if (node == Root)
            {
                Root = rightLeft;
            }
            else
            {
                Debug.Assert(parent != null);
                if (parent.RightChild == node)
                {
                    parent.RightChild = rightLeft;
                }
                else
                {
                    parent.LeftChild = rightLeft;
                }
            }

            switch (rightLeft.Balance)
            {
                case 1:
                    node.Balance = 0;
                    right.Balance = -1;
                    break;
                case 0:
                    node.Balance = 0;
                    right.Balance = 0;
                    break;
                default:
                    node.Balance = 1;
                    right.Balance = 0;
                    break;
            }

            rightLeft.Balance = 0;

            return rightLeft;
        }

        private static void Replace(
            [NotNull] AvlTreeNode<TKey, TValue> target,
            [NotNull] AvlTreeNode<TKey, TValue> source)
        {
            Debug.Assert(target != null);
            Debug.Assert(source != null);

            var left = source.LeftChild;
            var right = source.RightChild;

            target.Balance = source.Balance;
            target.Key = source.Key;
            target.Value = source.Value;
            target.LeftChild = left;
            target.RightChild = right;

            if (left != null)
            {
                left.Parent = target;
            }

            if (right != null)
            {
                right.Parent = target;
            }
        }

        private void Insert(TKey key, TValue value, bool allowUpdate)
        {
            if (Root == null)
            {
                NotifyTreeChanged(1);
                Root = new AvlTreeNode<TKey, TValue>
                {
                    Key = key,
                    Value = value
                };
            }
            else
            {
                var node = Root;
                while (node != null)
                {
                    var comparisonResult = Comparer.Compare(key, node.Key);
                    if (comparisonResult < 0)
                    {
                        var left = node.LeftChild;

                        if (left == null)
                        {
                            NotifyTreeChanged(1);
                            node.LeftChild = new AvlTreeNode<TKey, TValue>
                            {
                                Key = key,
                                Value = value,
                                Parent = node
                            };

                            ReBalanceTreeAfterInsert(node, 1);
                            return;
                        }
                        node = left;
                    }
                    else if (comparisonResult > 0)
                    {
                        var right = node.RightChild;

                        if (right == null)
                        {
                            NotifyTreeChanged(1);
                            node.RightChild = new AvlTreeNode<TKey, TValue>
                            {
                                Key = key,
                                Value = value,
                                Parent = node
                            };

                            ReBalanceTreeAfterInsert(node, -1);
                            return;
                        }

                        node = right;
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

                        return;
                    }
                }
            }
        }

        private void ReBalanceTreeAfterInsert([CanBeNull] AvlTreeNode<TKey, TValue> node, int balance)
        {
            while (node != null)
            {
                balance = node.Balance += balance;

                // ReSharper disable once SwitchStatementMissingSomeCases
                switch (balance)
                {
                    case 0:
                        return;
                    case 2:
                        Debug.Assert(node.LeftChild != null);

                        if (node.LeftChild.Balance == 1)
                        {
                            RotateRight(node);
                        }
                        else
                        {
                            RotateLeftRight(node);
                        }

                        return;
                    case -2:
                        Debug.Assert(node.RightChild != null);

                        if (node.RightChild.Balance == -1)
                        {
                            RotateLeft(node);
                        }
                        else
                        {
                            RotateRightLeft(node);
                        }

                        return;
                }

                var parent = node.Parent;
                if (parent != null)
                {
                    balance = parent.LeftChild == node ? 1 : -1;
                }

                node = parent;
            }
        }

        private void ReBalanceTreeAfterDelete([CanBeNull] AvlTreeNode<TKey, TValue> node, int balance)
        {
            while (node != null)
            {
                balance = node.Balance += balance;

                switch (balance)
                {
                    case 2:
                        Debug.Assert(node.LeftChild != null);

                        if (node.LeftChild.Balance >= 0)
                        {
                            node = RotateRight(node);

                            if (node.Balance == -1)
                            {
                                return;
                            }
                        }
                        else
                        {
                            node = RotateLeftRight(node);
                        }
                        break;
                    case -2:
                        Debug.Assert(node.RightChild != null);

                        if (node.RightChild.Balance <= 0)
                        {
                            node = RotateLeft(node);

                            if (node.Balance == 1)
                            {
                                return;
                            }
                        }
                        else
                        {
                            node = RotateRightLeft(node);
                        }
                        break;
                    default:
                        if (balance != 0)
                        {
                            return;
                        }
                        break;
                }

                var parent = node.Parent;

                if (parent != null)
                {
                    balance = parent.LeftChild == node ? -1 : 1;
                }

                node = parent;
            }
        }


        /// <summary>
        /// Looks up the node ky the given <paramref name="key"/>.
        /// </summary>
        /// <param name="key">The key of the node.</param>
        /// <returns>The node, if found; otherwise, <c>null</c>.</returns>
        [CanBeNull]
        public new AvlTreeNode<TKey, TValue> LookupNode(TKey key)
        {
            var node = base.LookupNode(key);
            Debug.Assert(node == null || node is AvlTreeNode<TKey, TValue>);

            return (AvlTreeNode<TKey, TValue>) node;
        }

        /// <summary>
        /// Gets or sets the root node of the AVL tree.
        /// </summary>
        /// <value>
        /// The root.
        /// </value>
        [CanBeNull]
        public new AvlTreeNode<TKey, TValue> Root
        {
            get
            {
                Debug.Assert(base.Root == null || base.Root is AvlTreeNode<TKey, TValue>);
                return (AvlTreeNode<TKey, TValue>) base.Root;
            }
            set => base.Root = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AvlTree{TKey, TValue}"/> class.
        /// </summary>
        /// <param name="comparer">The key comparer used.</param>
        /// <exception cref="System.ArgumentNullException">Thrown if <paramref name="comparer"/> is <c>null</c>.</exception>
        public AvlTree([NotNull] IComparer<TKey> comparer) : base(comparer)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AvlTree{TKey, TValue}"/> class using the default <typeparamref name="TKey"/> comparer.
        /// </summary>
        public AvlTree()
        {
        }

        /// <summary>
        /// Adds the specified key/value node to the tree.
        /// </summary>
        /// <param name="key">The node's key.</param>
        /// <param name="value">The node's value.</param>
        /// <exception cref="System.ArgumentException">Thrown if a node with the same <paramref name="key"/> is already present in the tree.</exception>
        public override void Add(TKey key, TValue value)
        {
            Insert(key, value, false);
        }

        /// <summary>
        /// Adds or updates a tree node that has a given key and value.
        /// </summary>
        /// <param name="key">The node's key.</param>
        /// <param name="value">The node's new value.</param>
        public override void AddOrUpdate(TKey key, TValue value)
        {
            Insert(key, value, true);
        }

        /// <summary>
        /// Removes the node from the tree that has a specified key.
        /// </summary>
        /// <param name="key">The node's key.</param>
        /// <returns><c>true</c> if the node was removed; otherwise, <c>false</c>.</returns>
        public override bool Remove(TKey key)
        {
            var node = Root;

            while (node != null)
            {
                if (Comparer.Compare(key, node.Key) < 0)
                {
                    node = node.LeftChild;
                }
                else if (Comparer.Compare(key, node.Key) > 0)
                {
                    node = node.RightChild;
                }
                else
                {
                    NotifyTreeChanged(-1);

                    var left = node.LeftChild;
                    var right = node.RightChild;

                    if (left == null)
                    {
                        if (right == null)
                        {
                            if (node == Root)
                            {
                                Root = null;
                            }
                            else
                            {
                                var parent = node.Parent;
                                Debug.Assert(parent != null);

                                if (parent.LeftChild == node)
                                {
                                    parent.LeftChild = null;
                                    ReBalanceTreeAfterDelete(parent, -1);
                                }
                                else
                                {
                                    parent.RightChild = null;
                                    ReBalanceTreeAfterDelete(parent, 1);
                                }
                            }
                        }
                        else
                        {
                            Replace(node, right);
                            ReBalanceTreeAfterDelete(node, 0);
                        }
                    }
                    else if (right == null)
                    {
                        Replace(node, left);
                        ReBalanceTreeAfterDelete(node, 0);
                    }
                    else
                    {
                        var successor = right;

                        if (successor.LeftChild == null)
                        {
                            var parent = node.Parent;

                            successor.Parent = parent;
                            successor.LeftChild = left;
                            successor.Balance = node.Balance;

                            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
                            if (left != null)
                            {
                                left.Parent = successor;
                            }

                            if (node == Root)
                            {
                                Root = successor;
                            }
                            else
                            {
                                Debug.Assert(parent != null);

                                if (parent.LeftChild == node)
                                {
                                    parent.LeftChild = successor;
                                }
                                else
                                {
                                    parent.RightChild = successor;
                                }
                            }

                            ReBalanceTreeAfterDelete(successor, 1);
                        }
                        else
                        {
                            while (successor.LeftChild != null)
                            {
                                successor = successor.LeftChild;
                            }

                            var parent = node.Parent;

                            var successorParent = successor.Parent;
                            Debug.Assert(successorParent != null);

                            var successorRight = successor.RightChild;

                            if (successorParent.LeftChild == successor)
                            {
                                successorParent.LeftChild = successorRight;
                            }
                            else
                            {
                                successorParent.RightChild = successorRight;
                            }

                            if (successorRight != null)
                            {
                                successorRight.Parent = successorParent;
                            }

                            successor.Parent = parent;
                            successor.LeftChild = left;
                            successor.Balance = node.Balance;
                            successor.RightChild = right;
                            right.Parent = successor;

                            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
                            if (left != null)
                            {
                                left.Parent = successor;
                            }

                            if (node == Root)
                            {
                                Root = successor;
                            }
                            else
                            {
                                Debug.Assert(parent != null);

                                if (parent.LeftChild == node)
                                {
                                    parent.LeftChild = successor;
                                }
                                else
                                {
                                    parent.RightChild = successor;
                                }
                            }

                            ReBalanceTreeAfterDelete(successorParent, -1);
                        }
                    }

                    return true;
                }
            }

            return false;
        }
    }
}