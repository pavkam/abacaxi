using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abacaxi.LinkedLists
{
    /// <summary>
    /// Represents a linked list node.
    /// </summary>
    /// <typeparam name="T">The type of the value stored in the node.</typeparam>
    public sealed class Node<T>
    {
        /// <summary>
        /// The node's value.
        /// </summary>
        public T Value { get; private set; }

        /// <summary>
        /// Next element in the list.
        /// </summary>
        public Node<T> Next { get; set; }

        /// <summary>
        /// Initializes a new instance of <see cref="Node{T}"/> class with a given value.
        /// </summary>
        /// <param name="value">The node's value.</param>
        public Node(T value)
        {
            Value = value;
        }

        /// <summary>
        /// Creates a new linked list from a given <paramref name="sequence"/>.
        /// </summary>
        /// <param name="sequence">The sequence to convert into a linked list.</param>
        /// <returns>The first node in the list (head).</returns>
        public static Node<T> Create(IEnumerable<T> sequence)
        {
            Validate.ArgumentNotNull(nameof(sequence), sequence);

            Node<T> head = null;
            Node<T> current = null;
            foreach(var item in sequence)
            {
                if (current == null)
                {
                    current = new Node<T>(item);
                    head = current;
                }
                else
                {
                    current.Next = new Node<T>(item);
                    current = current.Next;
                }
            }

            return head;
        }
    }
}
