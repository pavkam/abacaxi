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
        /// Tries the get the middle and tail nodes in one iteration.
        /// </summary>
        /// <param name="middleNode">The middle node, if the list is not knotted.</param>
        /// <param name="tailNode">The tail node, if the list is not knotted.</param>
        /// <returns><c>true</c> if the list is not knotted and has middle and tail nodes; <c>false</c> otherwise.</returns>
        [ContractAnnotation(
            "=> true, middleNode: notnull, tailNode: notnull; => false, middleNode: null, tailNode: null")]
        public bool TryGetMiddleAndTailNodes([CanBeNull] out LinkedListNode<T> middleNode,
            [CanBeNull] out LinkedListNode<T> tailNode)
        {
            var current = this;
            var skip = this;
            middleNode = null;
            tailNode = null;

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
                    return false;
                }

                current = current.Next;
            }

            return true;
        }

        /// <summary>
        ///     Reverses a given linked list using the iterative method.
        /// </summary>
        /// <remarks>This method does not check for knotted lists. A knotted list will force this method to execute indefinitely.</remarks>
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
    }
}