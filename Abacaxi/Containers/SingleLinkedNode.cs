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
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// Represents a linked list node.
    /// </summary>
    /// <typeparam name="T">The type of the value stored in the node.</typeparam>
    public sealed class SingleLinkedNode<T> : IEnumerable<SingleLinkedNode<T>>
    {
        /// <summary>
        /// The node's value.
        /// </summary>
        public T Value { get; private set; }

        /// <summary>
        /// Next element in the list.
        /// </summary>
        public SingleLinkedNode<T> Next { get; set; }

        /// <summary>
        /// Initializes a new instance of <see cref="SingleLinkedNode{T}"/> class with a given value.
        /// </summary>
        /// <param name="value">The node's value.</param>
        public SingleLinkedNode(T value)
        {
            Value = value;
        }

        /// <summary>
        /// Creates a new linked list from a given <paramref name="sequence"/>.
        /// </summary>
        /// <param name="sequence">The sequence to convert into a linked list.</param>
        /// <returns>The first node in the list (head).</returns>
        public static SingleLinkedNode<T> Create(IEnumerable<T> sequence)
        {
            Validate.ArgumentNotNull(nameof(sequence), sequence);

            SingleLinkedNode<T> head = null;
            SingleLinkedNode<T> current = null;
            foreach(var item in sequence)
            {
                if (current == null)
                {
                    current = new SingleLinkedNode<T>(item);
                    head = current;
                }
                else
                {
                    current.Next = new SingleLinkedNode<T>(item);
                    current = current.Next;
                }
            }

            return head;
        }

        /// <summary>
        /// Find the middle node of a linked list.
        /// </summary>
        /// <remarks>This method does not check for knotted lists. A knotted list will force this method to execute indefinitely.</remarks>
        /// <returns>The middle node; <c>null</c> if the list is empty.</returns>
        public SingleLinkedNode<T> FindMiddle()
        {
            var one = this;
            var two = Next?.Next;

            while (two != null)
            {
                one = one.Next;
                two = two.Next?.Next;
            }

            return one;
        }

        /// <summary>
        /// Finds whether the list is knotted.
        /// </summary>
        /// <returns><c>true</c> if the list is knotted; <c>false</c> otherwise.</returns>
        public bool VerifyIfKnotted()
        {
            var one = this;
            var two = Next?.Next;

            while (two != null)
            {
                one = one.Next;
                two = two.Next.Next;

                if (two == one)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Reverses a given linked list using the iterative method.
        /// </summary>
        /// <remarks>This method does not check for knotted lists. A knotted list will force this method to execute indefinitely.</remarks>
        /// <returns>The new head of the linked list.</returns>
        public SingleLinkedNode<T> Reverse()
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
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// An enumerator that can be used to iterate through the collection.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public IEnumerator<SingleLinkedNode<T>> GetEnumerator()
        {
            var current = this;
            while (current != null)
            {
                yield return current;
                current = current.Next;
            }
        }
    }
}
