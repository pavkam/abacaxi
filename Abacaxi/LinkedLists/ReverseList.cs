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

namespace Abacaxi.LinkedLists
{
    using System;

    /// <summary>
    /// Class implements two version of the linked-list reversal algorithms. One is iterative and the second one is recursive.
    /// </summary>
    public static class ReverseList
    {
        public static Node<T> ReverseRecursive<T>(Node<T> head, Node<T> current)
        {
            var next = current.Next;
            current.Next = head;

            if (next != null)
            {
                return ReverseRecursive(current, next);
            }
            else
            {
                return current;
            }
        }

        /// <summary>
        /// Reverses a given linked list using the iterative method.
        /// </summary>
        /// <remarks>This method does not check for knotted lists. A knowtted list will force this method to execute indefinitely.</remarks>
        /// <typeparam name="T">The type of values stored in the linked list.</typeparam>
        /// <param name="head">The head of the linked list.</param>
        /// <returns>The new head of the linked list.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="head"/> is <c>null</c>.</exception>
        public static Node<T> ReverseIterative<T>(Node<T> head)
        {
            Validate.ArgumentNotNull(nameof(head), head);

            var current = head.Next;
            head.Next = null;

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
        /// Reverses a given linked list using the recursive method.
        /// </summary>
        /// <remarks>This method does not check for knotted lists. A knowtted list will force this method to fail.</remarks>
        /// <typeparam name="T">The type of values stored in the linked list.</typeparam>
        /// <param name="head">The head of the linked list.</param>
        /// <returns>The new head of the linked list.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="head"/> is <c>null</c>.</exception>
        public static Node<T> ReverseRecursive<T>(Node<T> head)
        {
            Validate.ArgumentNotNull(nameof(head), head);

            return ReverseRecursive(null, head);
        }
    }
}
