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
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using LinkedLists;

    /// <summary>
    /// Class implements the algorithm to find "a good" way to cycle through all theh elements in a sequence. The algorithm uses a heuristic
    /// approach at deciding on the best movement path.
    /// </summary>
    public static class OptimalSequenceCycle
    {
        private static Node<T> TailOf<T>(Node<T> head)
        {
            Debug.Assert(head != null);
            while (head.Next != null)
            {
                head = head.Next;
            }

            return head;
        }

        /// <summary>
        /// Finds "a good" sequence which cycles through all the elements in a given <paramref name="sequence"/>.
        /// </summary>
        /// <typeparam name="T">The type of the values in the input <paramref name="sequence"/>.</typeparam>
        /// <param name="sequence">The input sequence.</param>
        /// <param name="movementCostFunc">The cost function (for two distinct items in the sequence).</param>
        /// <returns>The input sequence arranged in a "good enough" cycle.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="sequence"/> or <paramref name="movementCostFunc"/> are <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">Thrown if the <paramref name="movementCostFunc"/> returns a negative value.</exception>
        public static IEnumerable<T> Find<T>(IEnumerable<T> sequence, Func<T, T, int> movementCostFunc)
        {
            Validate.ArgumentNotNull(nameof(sequence), sequence);
            Validate.ArgumentNotNull(nameof(movementCostFunc), movementCostFunc);

            var heads = new HashSet<Node<T>>();
            var tails = new HashSet<Node<T>>();

            foreach (var node in sequence.Select(s => new Node<T>(s)))
            {
                heads.Add(node);
                tails.Add(node);
            }

            if (heads.Count == 0)
            {
                yield break;
            }

            while (tails.Count > 1 && heads.Count > 1)
            {
                Tuple<int, Node<T>, Node<T>> bestResult = null;
                foreach (var tail in tails)
                {
                    foreach (var head in heads)
                    {
                        if (TailOf(head) != tail)
                        {
                            var cost = movementCostFunc(tail.Value, head.Value);
                            if (cost < 0)
                            {
                                throw new InvalidOperationException($"Invalid cost ({cost}) has been provided for items {head.Value} and {tail.Value}.");
                            }

                            if (bestResult == null || cost < bestResult.Item1)
                            {
                                bestResult = Tuple.Create(cost, head, tail);
                            }
                        }
                    }
                }

                Debug.Assert(bestResult != null);

                tails.Remove(bestResult.Item3);
                heads.Remove(bestResult.Item2);

                bestResult.Item3.Next = bestResult.Item2;
            }

            Debug.Assert(heads.Count == 1);
            Debug.Assert(tails.Count == 1);

            var current = heads.Single();
            while (current != null)
            {
                yield return current.Value;
                current = current.Next;
            }
        }
    }
}
