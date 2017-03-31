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

namespace Abacaxi.Sequences
{
    using System.Collections.Generic;

    /// <summary>
    /// Class implements the simplest way to find a linked list's middle. The idea is to navigate the list with two pointers
    /// at the same time. One moves from node to node, while the other skips a node. When the second reaches the end, the first one will be pointing
    /// to the middle of the list.
    /// </summary>
    public static class LinkedListMiddle
    {
        /// <summary>
        /// Find the middle node of a linked list.
        /// </summary>
        /// <typeparam name="T">The type of linked list node.</typeparam>
        /// <param name="list">The linked list.</param>
        /// <returns>The middle node; <c>null</c> if the list is empty.</returns>
        public static LinkedListNode<T> Find<T>(LinkedList<T> list)
        {
            Validate.ArgumentNotNull(nameof(list), list);
           
            if (list.First == null)
            {
                return null;
            }

            var one = list.First;
            var two = list.First;

            while (two?.Next != null)
            {
                one = one.Next;
                two = two.Next.Next;
            }

            return one;
        }
    }
}
