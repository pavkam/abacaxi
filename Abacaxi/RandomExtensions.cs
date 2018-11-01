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

namespace Abacaxi
{
    using System;
    using System.Collections.Generic;
    using Internal;
    using JetBrains.Annotations;

    /// <summary>
    ///     Implements a number of random-related helper methods useable across the library (and beyond!).
    /// </summary>
    [PublicAPI]
    public static class RandomExtensions
    {
        /// <summary>
        ///     Returns a random sample of a given sequence of elements.
        /// </summary>
        /// <typeparam name="T">The type of elements in the <paramref name="sequence" />.</typeparam>
        /// <param name="random">The random instance to use for sampling.</param>
        /// <param name="sequence">The sequence of elements.</param>
        /// <param name="sampleSize">Length of the sample to be selected.</param>
        /// <returns>
        ///     A random sequence of elements from <paramref name="sequence" />.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     Thrown if <paramref name="sequence" /> or <paramref name="random" /> are
        ///     <c>null</c>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="sampleSize" /> is less than one.</exception>
        [NotNull]
        public static T[] Sample<T>([NotNull] this Random random, [NotNull] IEnumerable<T> sequence, int sampleSize)
        {
            Validate.ArgumentNotNull(nameof(random), random);
            Validate.ArgumentNotNull(nameof(sequence), sequence);
            Validate.ArgumentGreaterThanZero(nameof(sampleSize), sampleSize);

            var sample = new T[sampleSize];
            var i = 0;

            using (var enumerator = sequence.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    if (i < sampleSize)
                    {
                        sample[i++] = enumerator.Current;
                    }
                    else
                    {
                        var j = random.Next(i - 1);
                        if (j < sampleSize)
                        {
                            sample[j] = enumerator.Current;
                        }
                    }
                }
            }

            if (i < sampleSize)
            {
                Array.Resize(ref sample, i);
            }

            return sample;
        }

        /// <summary>
        ///     Returns a random <see cref="bool" /> value.
        /// </summary>
        /// <param name="random">The random class instance.</param>
        /// <returns>The random boolean value.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="random" /> is <c>null</c>.</exception>
        public static bool NextBool([NotNull] this Random random)
        {
            Validate.ArgumentNotNull(nameof(random), random);

            return
                random.Next(2) == 1;
        }

        /// <summary>
        ///     Returns a random item from a given <paramref name="sequence" />.
        /// </summary>
        /// <typeparam name="T">The type of items in the sequence.</typeparam>
        /// <param name="random">The random class instance.</param>
        /// <param name="sequence">The sequence.</param>
        /// <returns>A random element from the given <paramref name="sequence" /></returns>
        /// <exception cref="ArgumentNullException">
        ///     Thrown if <paramref name="sequence" /> or <paramref name="random" /> are
        ///     <c>null</c>.
        /// </exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="sequence" /> is empty.</exception>
        public static T NextItem<T>([NotNull] this Random random, [NotNull] IList<T> sequence)
        {
            Validate.ArgumentNotNull(nameof(random), random);
            Validate.ArgumentNotEmpty(nameof(sequence), sequence);

            return sequence[random.Next(sequence.Count)];
        }

        /// <summary>
        ///     Returns a random item from the given items.
        /// </summary>
        /// <typeparam name="T">The type of items in the given list.</typeparam>
        /// <param name="random">The random class instance.</param>
        /// <param name="item1">The first item to consider.</param>
        /// <param name="item2">The second item to consider.</param>
        /// <param name="others">The others (third and after).</param>
        /// <returns>The randomly selected item.</returns>
        /// <exception cref="ArgumentNullException">
        ///     Thrown if <paramref name="others" /> or <paramref name="random" /> are
        ///     <c>null</c>.
        /// </exception>
        public static T NextItem<T>([NotNull] this Random random, T item1, T item2, [NotNull] params T[] others)
        {
            Validate.ArgumentNotNull(nameof(random), random);

            var next = random.Next(others.Length + 2);
            switch (next)
            {
                case 0:
                    return item1;
                case 1:
                    return item2;
                default:
                    return others[next - 2];
            }
        }
    }
}