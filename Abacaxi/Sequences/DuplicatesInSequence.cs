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
    /// Provides simple algorithms to find duplicate elements in a sequence. The generic case uses a dictionary. The integer variant uses
    /// an in-memory array to strore the appearances (which is very efficient in terms of speed but very wasteful in terms of memory).
    /// The string variant uses a combination of an array (for ASCII characters) and a dictionary for Unicode.
    /// </summary>
    public static class DuplicatesInSequence
    {
        /// <summary>
        /// Finds all duplicate items in a given <paramref name="sequence"/>.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
        /// <param name="sequence">The sequence to inspect.</param>
        /// <param name="equalityComparer">The comparer used to verify the elements in the sequence.</param>
        /// <returns>A sequence of element-appearances pairs of the detected duplicates.</returns>
        /// <exception cref="ArgumentNullException">Thrown if either the <paramref name="sequence"/> or the <paramref name="equalityComparer"/> are <c>null</c>.</exception>
        public static IEnumerable<KeyValuePair<T, int>> Find<T>(IEnumerable<T> sequence, IEqualityComparer<T> equalityComparer)
        {
            Validate.ArgumentNotNull(nameof(sequence), sequence);
            Validate.ArgumentNotNull(nameof(equalityComparer), equalityComparer);

            var appearances = new Dictionary<T, int>(equalityComparer);
            foreach (var item in sequence)
            {
                int count;
                if (!appearances.TryGetValue(item, out count))
                {
                    appearances.Add(item, 1);
                }
                else
                {
                    appearances[item] = count + 1;
                }
            }

            foreach (var appearance in appearances)
            {
                if (appearance.Value > 1)
                {
                    yield return appearance;
                }
            }
        }

        /// <summary>
        /// Finds all duplicate integers in a given <paramref name="sequence"/>.
        /// </summary>
        /// <param name="sequence">The sequence to inspect.</param>
        /// <param name="minInSequence">The minimum possible value of an element part of the <paramref name="sequence"/>.</param>
        /// <param name="maxInSequence">The maximum possible value of an element part of the <paramref name="sequence"/>.</param>
        /// <returns>A sequence of element-appearances pairs of the detected duplicates.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="sequence"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="maxInSequence"/> is less than <paramref name="minInSequence"/>.</exception>
        public static IEnumerable<KeyValuePair<int, int>> Find(IEnumerable<int> sequence, int minInSequence, int maxInSequence)
        {
            Validate.ArgumentNotNull(nameof(sequence), sequence);
            Validate.ArgumentGreaterOrEqualTo(nameof(maxInSequence), maxInSequence, minInSequence);

            var appearances = new int[maxInSequence - minInSequence + 1];
            foreach(var item in sequence)
            {
                if (item < minInSequence || item > maxInSequence)
                    throw new InvalidOperationException($"The sequence of integers contains element {item} which is outside of the given {minInSequence}..{maxInSequence} range.");

                appearances[item - minInSequence]++;
            }

            for(var i = 0; i < appearances.Length; i++)
            {
                if (appearances[i] > 1)
                {
                    yield return new KeyValuePair<int, int>(i + minInSequence, appearances[i]);
                }
            }
        }

        /// <summary>
        /// Finds all duplicate characters in a given <paramref name="sequence"/>.
        /// </summary>
        /// <param name="sequence">The sequence to inspect.</param>
        /// <returns>A sequence of element-appearances pairs of the detected duplicates.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="sequence"/> is <c>null</c>.</exception>
        public static IEnumerable<KeyValuePair<char, int>> Find(string sequence)
        {
            Validate.ArgumentNotNull(nameof(sequence), sequence);

            var asciiAppearances = new int[byte.MaxValue + 1];
            var appearances = new Dictionary<char, int>();

            foreach (var item in sequence)
            {
                if (item <= byte.MaxValue)
                {
                    asciiAppearances[item]++;
                }
                else
                {
                    int count;
                    if (!appearances.TryGetValue(item, out count))
                    {
                        appearances.Add(item, 1);
                    }
                    else
                    {
                        appearances[item] = count + 1;
                    }
                }
            }

            for (var i = 0; i < asciiAppearances.Length; i++)
            {
                if (asciiAppearances[i] > 1)
                {
                    yield return new KeyValuePair<char, int>((char)i, asciiAppearances[i]);
                }
            }

            foreach(var appearance in appearances)
            {
                if (appearance.Value > 1)
                {
                    yield return appearance;
                }
            }
        }
    }
}
