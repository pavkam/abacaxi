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

namespace Abacaxi
{
    using System;
    using System.Collections.Generic;
    using Internal;
    using JetBrains.Annotations;

    /// <summary>
    /// Implements a number of random-related helper methods useable across the library (and beyond!).
    /// </summary>
    [PublicAPI]
    public static class RandomExtensions
    {
        /// <summary>
        /// Returns a random sample of a given sequence of elements.
        /// </summary>
        /// <typeparam name="T">Thetype of elements in the <paramref name="sequence" />.</typeparam>
        /// <param name="random">The random instance to use for sampling.</param>
        /// <param name="sequence">The sequence of elements.</param>
        /// <param name="sampleSize">Length of the sample to be selected.</param>
        /// <returns>
        /// A random sequence of elements from <paramref name="sequence" />.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown if either <paramref name="sequence" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="sampleSize" /> is less than one.</exception>
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
        /// Returns a random <see cref="bool"/> value.
        /// </summary>
        /// <param name="random">The random class instance.</param>
        /// <returns>The random boolean value.</returns>
        public static bool NextBool(this Random random)
        {
            Validate.ArgumentNotNull(nameof(random), random);

            return
                random.Next(2) == 1;
        }

        public static T NextItem<T>(this Random random, IList<T> sequence)
        {
            Validate.ArgumentNotNull(nameof(random), random);
            Validate.SequenceArgumentNotEmpty(nameof(sequence), sequence);

            return sequence[random.Next(sequence.Count)];
        }

        public static T NextItem<T>(this Random random, T item1, T item2, params T[] others)
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
