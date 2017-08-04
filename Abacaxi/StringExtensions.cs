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
        [NotNull]
        public static IList<char> AsList([NotNull] this string s)
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
        [NotNull]
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
        public static string Shorten([NotNull] this string s, int maxLength, [CanBeNull] string ellipsis = null)
        {
            Validate.ArgumentNotNull(nameof(s), s);
            Validate.ArgumentGreaterThanZero(nameof(maxLength), maxLength);
            if (ellipsis != null)
            {
                Validate.ArgumentLessThanOrEqualTo(nameof(ellipsis), ellipsis.Length, maxLength);
            }

            if (s.Length > maxLength)
            {
                var elpLength = ellipsis?.Length ?? 0;
                var cutOffLen = 0; 
                var enumerator = StringInfo.GetTextElementEnumerator(s);
                while (enumerator.MoveNext())
                {
                    var expLength = enumerator.ElementIndex + elpLength;
                    if (expLength <= maxLength)
                    {
                        cutOffLen = enumerator.ElementIndex;
                    }
                    else
                    {
                        break;
                    }
                }

                var sb = new StringBuilder(s, 0, cutOffLen, maxLength);
                if (ellipsis != null)
                    sb.Append(ellipsis);

                s = sb.ToString();
            }

            return s;
        }

        /// <summary>
        /// Escapes the specified string.
        /// </summary>
        /// <remarks>
        /// This method escapes the special characters and unicode escape characters.</remarks>
        /// <param name="s">The string to escape.</param>
        /// <returns>The escaped string.</returns>
        public static string Escape([CanBeNull] this string s)
        {
            Validate.ArgumentNotNull(nameof(s), s);

            var result = new StringBuilder(s.Length * 2);
            var enumerator = StringInfo.GetTextElementEnumerator(s);
            while (enumerator.MoveNext())
            {
                var segment = (string) enumerator.Current;

                if (segment.Length == 1)
                {
                    var c = segment[0];
                    switch (c)
                    {
                        case '\'':
                            result.Append("\\'");
                            break;
                        case '"':
                            result.Append("\\\"");
                            break;
                        case '\\':
                            result.Append("\\\\");
                            break;
                        case '\0':
                            result.Append("\\0");
                            break;
                        case '\a':
                            result.Append("\\a");
                            break;
                        case '\b':
                            result.Append("\\b");
                            break;
                        case '\f':
                            result.Append("\\f");
                            break;
                        case '\n':
                            result.Append("\\n");
                            break;
                        case '\r':
                            result.Append("\\r");
                            break;
                        case '\t':
                            result.Append("\\t");
                            break;
                        case '\v':
                            result.Append("\\v");
                            break;
                        default:
                            if (char.IsControl(c))
                            {
                                result.Append($"\\u{(int) segment[0]:x4}");
                            }
                            else
                            {
                                result.Append(c);
                            }
                            break;
                    }
                }
                else
                {
                    result.Append(segment);
                }
            }

            return result.ToString();
        }

        /// <summary>
        /// Checks whether the given string matches the specified pattern.
        /// </summary>
        /// <param name="s">The string to check.</param>
        /// <param name="pattern">The pattern.</param>
        /// <param name="ignoreCase">If set to <c>true</c>, ignores the case.</param>
        /// <returns><c>true</c> if the string matches the pattern; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown if <paramref name="s"/> or <paramref name="pattern"/> are <c>null</c>.</exception>
        public static bool Like([NotNull] this string s, [NotNull] string pattern, bool ignoreCase = true)
        {
            Validate.ArgumentNotNull(nameof(pattern), pattern);
            return GlobPattern.GetPattern(pattern, ignoreCase).IsMatch(s);
        }

        /// <summary>
        /// Finds all duplicate characters in a given <paramref name="sequence"/>.
        /// </summary>
        /// <param name="sequence">The sequence to inspect.</param>
        /// <returns>A sequence of element-appearances pairs of the detected duplicates.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown if <paramref name="sequence"/> is <c>null</c>.</exception>
        [NotNull]
        public static Frequency<char>[] FindDuplicates([NotNull] this string sequence)
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
                    if (!appearances.TryGetValue(item, out int count))
                    {
                        appearances.Add(item, 1);
                    }
                    else
                    {
                        appearances[item] = count + 1;
                    }
                }
            }

            var result = new List<Frequency<char>>();
            for (var i = 0; i < asciiAppearances.Length; i++)
            {
                if (asciiAppearances[i] > 1)
                {
                    result.Add(new Frequency<char>((char)i, asciiAppearances[i]));
                }
            }

            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var kvp in appearances)
            {
                if (kvp.Value > 1)
                {
                    result.Add(new Frequency<char>(kvp.Key, kvp.Value));
                }
            }

            return result.ToArray();
        }

    }
}
