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

    /// <summary>
    /// Class implements a simple algorithm to extract all nested groups from a sequence. The algorithm uses a stack to 
    /// simulate the recursive behaviour.
    /// </summary>
    public static class NestedGroups
    {
        /// <summary>
        /// Extracts all nested groups from sequence. The method returns a sequence of sequences.
        /// </summary>
        /// <typeparam name="T">The type of the elemnets of <paramref name="sequence"/>.</typeparam>
        /// <param name="sequence">The sequence.</param>
        /// <param name="openBracket">The element that signifies the start of a group.</param>
        /// <param name="closeBracket">The element that signifies the end of a group.</param>
        /// <param name="comparer">The equality comparere for the elements of the <paramref name="sequence"/>.</param>
        /// <returns>The sequence of extracted groups, starting with the inner most ones.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="sequence"/> or <paramref name="comparer"/> are <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">Throws if the number of open and close brackets do not match.</exception>
        public static IEnumerable<T[]> Extract<T>(IEnumerable<T> sequence, T openBracket, T closeBracket, IEqualityComparer<T> comparer)
        {
            Validate.ArgumentNotNull(nameof(sequence), sequence);
            Validate.ArgumentNotNull(nameof(comparer), comparer);

            Stack<List<T>> stack = new Stack<List<T>>();
            var currentList = new List<T>();
            foreach (var item in sequence)
            {
                if (comparer.Equals(item, openBracket))
                {
                    currentList.Add(item);

                    stack.Push(currentList);
                    currentList = new List<T>();
                }
                else if (comparer.Equals(item, closeBracket))
                {
                    yield return currentList.ToArray();
                    if (stack.Count == 0)
                    {
                        throw new InvalidOperationException($"There are no blocks open to be closed.");
                    }

                    var previousList = stack.Pop();
                    previousList.AddRange(currentList);
                    currentList = previousList;
                    currentList.Add(item);
                }
                else
                {
                    currentList.Add(item);
                }
            }

            if (stack.Count > 0)
            {
                throw new InvalidOperationException($"There are {stack.Count} number of blocks that have not been closed.");
            }

            if (currentList.Count > 0)
            {
                yield return currentList.ToArray();
            }
        }
    }
}
