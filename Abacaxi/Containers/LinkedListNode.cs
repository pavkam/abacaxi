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
    using Internal;
    using JetBrains.Annotations;

    /// <inheritdoc />
    /// <summary>
    ///     Represents a single-linked list node.
    /// </summary>
    /// <typeparam name="T">The type of the value stored in the node.</typeparam>
    [PublicAPI]
    public sealed class LinkedListNode<T> : IEnumerable<LinkedListNode<T>>
    {
        /// <summary>
        ///     Initializes a new instance of <see cref="LinkedListNode{T}" /> class with a given value.
        /// </summary>
        /// <param name="value">The node's value.</param>
        public LinkedListNode(T value)
        {
            Value = value;
        }

        /// <summary>
        ///     The node's value.
        /// </summary>
        public T Value { get; }

        /// <summary>
        ///     Next element in the list.
        /// </summary>
        [CanBeNull]
        public LinkedListNode<T> Next { get; set; }

        /// <inheritdoc />
        /// <summary>
        ///     Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        ///     An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <inheritdoc />
        /// <summary>
        ///     Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        ///     An enumerator that can be used to iterate through the collection.
        /// </returns>
        /// <exception cref="T:System.NotImplementedException"></exception>
        public IEnumerator<LinkedListNode<T>> GetEnumerator()
        {
            var current = this;
            while (current != null)
            {
                yield return current;

                current = current.Next;
            }
        }

        private static void RaiseListKnottedError()
        {
            throw new InvalidOperationException(
                "The linked list is knotted (circular) and its length cannot be evaluated.");
        }

        /// <summary>
        ///     Creates a new linked list from a given <paramref name="sequence" />.
        /// </summary>
        /// <param name="sequence">The sequence to convert into a linked list.</param>
        /// <returns>The first node in the list (head).</returns>
        [CanBeNull]
        public static LinkedListNode<T> Create([NotNull] IEnumerable<T> sequence)
        {
            Validate.ArgumentNotNull(nameof(sequence), sequence);

            LinkedListNode<T> head = null;
            LinkedListNode<T> current = null;
            foreach (var item in sequence)
            {
                if (current == null)
                {
                    current = new LinkedListNode<T>(item);
                    head = current;
                }
                else
                {
                    current.Next = new LinkedListNode<T>(item);
                    current = current.Next;
                }
            }

            return head;
        }

        /// <summary>
        ///     Tries the get the middle and tail nodes in one iteration and returns the length of the list in the process.
        /// </summary>
        /// <param name="middleNode">The middle node, if the list is not knotted (circular).</param>
        /// <param name="tailNode">The tail node, if the list is not knotted (circular).</param>
        /// <returns>The length of the linked list if it's not knotted (circular); <c>-1</c> otherwise.</returns>
        public int TryGetMiddleAndTailNodes(
            [CanBeNull] out LinkedListNode<T> middleNode,
            [CanBeNull] out LinkedListNode<T> tailNode)
        {
            var current = this;
            var skip = this;
            middleNode = null;
            tailNode = null;

            var count = 1;
            for (;;)
            {
                /* Find middle condition. */
                if (skip != null)
                {
                    if (skip.Next?.Next == null)
                    {
                        middleNode = current;
                    }

                    skip = skip.Next?.Next;
                }

                /* Find tail condition. */
                if (current.Next == null)
                {
                    tailNode = current;
                    break;
                }

                /* Knotted condition. */
                if (current == skip)
                {
                    return -1;
                }

                count++;
                current = current.Next;
            }

            return count;
        }

        /// <summary>
        ///     Evaluates the length of the linked list that starts with this node.
        /// </summary>
        /// <returns>The length of the linked list.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the list is knotted (circular).</exception>
        public int GetLength()
        {
            var result = TryGetMiddleAndTailNodes(out _, out _);
            if (result == -1)
            {
                RaiseListKnottedError();
            }

            return result;
        }

        /// <summary>
        ///     Gets the linked list's middle node.
        /// </summary>
        /// <returns>The middle node.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the list is knotted (circular).</exception>
        [NotNull]
        public LinkedListNode<T> GetMiddleNode()
        {
            if (TryGetMiddleAndTailNodes(out var result, out _) == -1)
            {
                RaiseListKnottedError();
            }

            Assert.NotNull(result);
            return result;
        }

        /// <summary>
        ///     Gets the linked list's tail node.
        /// </summary>
        /// <returns>The tail node.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the list is knotted (circular).</exception>
        [NotNull]
        public LinkedListNode<T> GetTailNode()
        {
            if (TryGetMiddleAndTailNodes(out _, out var result) == -1)
            {
                RaiseListKnottedError();
            }

            Assert.NotNull(result);

            return result;
        }

        /// <summary>
        ///     Reverses a given linked list using the iterative method.
        /// </summary>
        /// <remarks>
        ///     This method does not check for knotted lists. A knotted (circular) list will force this method to execute
        ///     indefinitely.
        /// </remarks>
        /// <returns>The new head of the linked list.</returns>
        [NotNull]
        public LinkedListNode<T> Reverse()
        {
            var current = Next;
            Next = null;

            var head = this;
            while (current != null)
            {
                var attach = current;
                current = current.Next;
                attach.Next = head;
                head = attach;
            }

            return head;
        }

        /// <summary>
        ///     Gets the node that is the intersection of two linked lists.
        /// </summary>
        /// <param name="head">The head of the other linked list.</param>
        /// <returns>The node that is the intersection of the two lists (or <c>null</c> if the lists do not intersect.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the list is knotted (circular).</exception>
        [CanBeNull]
        public LinkedListNode<T> GetIntersectionNode([NotNull] LinkedListNode<T> head)
        {
            Validate.ArgumentNotNull(nameof(head), head);

            var head1 = this;
            var head2 = head;

            var len1 = head1.GetLength();
            var len2 = head2.GetLength();

            while (len1 > len2)
            {
                Assert.NotNull(head1);

                len1--;
                head1 = head1.Next;
            }

            while (len2 > len1)
            {
                Assert.NotNull(head2);

                len2--;
                head2 = head2.Next;
            }

            while (head1 != head2)
            {
                Assert.NotNull(head1);
                Assert.NotNull(head2);

                head1 = head1.Next;
                head2 = head2.Next;
            }

            return head1;
        }

        /// <summary>
        /// Gets the knot node (the would-be tail).
        /// </summary>
        /// <returns>The knot node. If the list is not knotted, <c>null</c>.</returns>
        [CanBeNull]
        public LinkedListNode<T> GetKnotNode()
        {
            /* Rotate until we detect the knotting */
            var current = this;
            var visitedSet = new HashSet<LinkedListNode<T>> {current};

            while (current.Next != null)
            {
                if (!visitedSet.Add(current.Next))
                {
                    return current;
                }

                current = current.Next;
            }

            return null;
        }
    }
}