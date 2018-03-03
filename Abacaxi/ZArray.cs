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
    /// Class exposes one static method used to compute a Z-Array of a sequence.
    /// </summary>
    [PublicAPI]
    public static class ZArray
    {
        /// <summary>
        /// Computes the Z-array for the given <paramref name="sequence" />.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="sequence">The sequence to compute the Z-array for.</param>
        /// <param name="startIndex">The start index in the sequence.</param>
        /// <param name="length">The length of the sequence.</param>
        /// <param name="comparer">The comparer.</param>
        /// <returns>A new, computed Z-array (of integers).</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="comparer"/> ic <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="sequence" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the combination of <paramref name="startIndex" /> and <paramref name="length" /> is out of bounds.</exception>
        [NotNull]
        public static int[] Construct<T>(
            [NotNull] IList<T> sequence,
            int startIndex,
            int length,
            [NotNull] IEqualityComparer<T> comparer)
        {
            Validate.CollectionArgumentsInBounds(nameof(sequence), sequence, startIndex, length);
            Validate.ArgumentNotNull(nameof(comparer), comparer);

            var zArray = new int[length];

            if (length > 0)
            {
                zArray[0] = length;

                var li = 0;
                var ri = 0;

                for (var i = 1; i < length; i++)
                {
                    if (i > ri)
                    {
                        li = ri = i;

                        while (ri < length &&
                               comparer.Equals(sequence[ri - li + startIndex], sequence[ri + startIndex]))
                        {
                            ri++;
                        }

                        zArray[i] = ri - li;
                        ri--;
                    }
                    else
                    {
                        var x = i - li;
                        if (zArray[x] < ri - i + 1)
                        {
                            zArray[i] = zArray[x];
                        }
                        else
                        {
                            li = i;
                            while (ri < length &&
                                   comparer.Equals(sequence[ri - li + startIndex], sequence[ri + startIndex]))
                            {
                                ri++;
                            }

                            zArray[i] = ri - li;
                            ri--;
                        }
                    }
                }
            }

            return zArray;
        }
    }
}