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
    using Abacaxi.Containers;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Class implements a simple algorithm to interleve multiple "streams" of data.
    /// </summary>
    public static class InterleaveSequences
    {
        /// <summary>
        /// Interleaves multiple streams into one output stream.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequences.</typeparam>
        /// <param name="comparer">The comparer.</param>
        /// <param name="sequences">The sequences to interleave.</param>
        /// <returns>A new interleaved stream.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the <paramref name="comparer"/> or <paramref name="sequences"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown if the <paramref name="sequences"/> is empty.</exception>
        /// <exception cref="InvalidOperationException">Throwsn if one or more enumerables return unsorted items.</exception>
        public static IEnumerable<T> Interleave<T>(IComparer<T> comparer, params IEnumerable<T>[] sequences)
        {
            Validate.ArgumentNotNull(nameof(comparer), comparer);
            Validate.ArgumentNotEmpty(nameof(sequences), sequences);

            var innerComparer = Comparer<IEnumerator<T>>.Create((a, b) =>
            {
                return comparer.Compare(a.Current, b.Current);
            });

            var heap = new Heap<IEnumerator<T>>(innerComparer);
            for (var i = 0; i < sequences.Length; i++)
            {
                var enumerator = sequences[i].GetEnumerator();
                if (enumerator.MoveNext())
                {
                    heap.Add(enumerator);
                }
            }

            while (heap.Count > 0)
            {
                var top = heap.RemoveTop();
                var c = top.Current;
                yield return c;

                if (top.MoveNext())
                {
                    if (comparer.Compare(top.Current, c) > 0)
                    {
                        throw new InvalidOperationException("One or more enumerables return unsorted items.");
                    }

                    heap.Add(top);
                }
            }
        }
    }
}
