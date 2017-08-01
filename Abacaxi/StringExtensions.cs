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
    using System.Collections.Generic;
    using Internal;
    using JetBrains.Annotations;
    using System.Globalization;
    using System.Text;

    /// <summary>
    /// Extension method for the <see cref="string"/> data type.
    /// </summary>
    [PublicAPI]
    public static class StringExtensions
    {
        /// <summary>
        /// Treats a given string as a list of characters.
        /// </summary>
        /// <param name="s">The string.</param>
        /// <returns>A wrapping list.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown if <paramref name="s"/> is <c>null</c>.</exception>
        public static IList<char> AsList(this string s)
        {
            Validate.ArgumentNotNull(nameof(s), s);
            return new StringListWrapper(s);
        }

        /// <summary>
        /// Reverses the specified string using "undivided" string chunks.
        /// </summary>
        /// <param name="s">The string to reverse.</param>
        /// <returns>The reserved string.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown if <paramref name="s"/> is <c>null</c>.</exception>
        public static string Reverse(this string s)
        {
            Validate.ArgumentNotNull(nameof(s), s);

            var result = new char[s.Length];
            var index = result.Length;
            var enumerator = StringInfo.GetTextElementEnumerator(s);
            while (enumerator.MoveNext())
            {
                var chunk = (string) enumerator.Current;
                index -= chunk.Length;
                chunk.CopyTo(0, result, index, chunk.Length);
            }

            return new string(result);
        }

        /// <summary>
        /// Shortens the specified string up to a maximum length.
        /// </summary>
        /// <param name="s">The string.</param>
        /// <param name="maxLength">The maximum length of the output string.</param>
        /// <param name="ellipsis">The optional ellipsis string.</param>
        /// <returns>A string of a maximum of <paramref name="maxLength"/> character.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown if <paramref name="s"/> is <c>null</c>.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown if <paramref name="maxLength"/> is less than one or the length of <paramref name="ellipsis"/>is greater than <paramref name="maxLength"/>.</exception>
        public static string Shorten([NotNull] this string s, int maxLength, [CanBeNull] string ellipsis = "...")
        {
            Validate.ArgumentNotNull(nameof(s), s);
            Validate.ArgumentGreaterThanZero(nameof(maxLength), maxLength);
            if (ellipsis != null)
            {
                Validate.ArgumentLessThanOrEqualTo(nameof(ellipsis), ellipsis.Length, maxLength);
            }

            var length = s.Length;
            if (length > maxLength)
            {
                length = maxLength;
                if (!string.IsNullOrEmpty(ellipsis))
                {
                    length -= ellipsis.Length;
                }

                return new StringBuilder(s, 0, length, maxLength).Append(ellipsis).ToString();
            }

            return s;
        }
    }
}
