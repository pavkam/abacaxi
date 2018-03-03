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
    using System.Diagnostics;

    /// <summary>
    /// Extension method for the <see cref="string"/> data type.
    /// </summary>
    [PublicAPI]
    public static class StringExtensions
    {
        [NotNull]
        private static readonly IDictionary<char, char> SpecialDiacriticMappings = new
            Dictionary<char, char>
            {
                {'ł', 'l'},
                {'Ł', 'L'}
            };

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
        public static string Reverse([NotNull] this string s)
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
        [NotNull]
        public static string Shorten([NotNull] this string s, int maxLength, [CanBeNull] string ellipsis = null)
        {
            Validate.ArgumentNotNull(nameof(s), s);
            Validate.ArgumentGreaterThanZero(nameof(maxLength), maxLength);
            if (ellipsis != null)
            {
                Validate.ArgumentLessThanOrEqualTo(nameof(ellipsis), ellipsis.Length, maxLength);
            }

            if (s.Length <= maxLength)
            {
                return s;
            }

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
            {
                sb.Append(ellipsis);
            }

            s = sb.ToString();

            return s;
        }

        /// <summary>
        /// Escapes the specified string.
        /// </summary>
        /// <remarks>
        /// This method escapes the special characters and unicode escape characters.</remarks>
        /// <param name="s">The string to escape.</param>
        /// <returns>The escaped string.</returns>
        [NotNull]
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
        /// Finds all duplicate characters in a given <paramref name="s"/>.
        /// </summary>
        /// <param name="s">The string to inspect.</param>
        /// <returns>A s of element-appearances pairs of the detected duplicates.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown if <paramref name="s"/> is <c>null</c>.</exception>
        [NotNull]
        public static Frequency<char>[] FindDuplicates([NotNull] this string s)
        {
            Validate.ArgumentNotNull(nameof(s), s);

            var asciiAppearances = new int[byte.MaxValue + 1];
            var appearances = new Dictionary<char, int>();

            foreach (var item in s)
            {
                if (item <= byte.MaxValue)
                {
                    asciiAppearances[item]++;
                }
                else
                {
                    if (!appearances.TryGetValue(item, out var count))
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
                    result.Add(new Frequency<char>((char) i, asciiAppearances[i]));
                }
            }

            foreach (var kvp in appearances)
            {
                if (kvp.Value > 1)
                {
                    result.Add(new Frequency<char>(kvp.Key, kvp.Value));
                }
            }

            return result.ToArray();
        }

        [NotNull]
        [ItemNotNull]
        private static IEnumerable<string> SplitIntoLinesIterate([NotNull] this string s)
        {
            Debug.Assert(s != null);

            var si = 0;
            var ci = 0;
            while (ci < s.Length)
            {
                Debug.Assert(ci >= si);

                if (s[ci] == '\n')
                {
                    if (ci == si)
                    {
                        yield return string.Empty;
                    }
                    else
                    {
                        if (ci > si && s[ci - 1] == '\r')
                        {
                            yield return s.Substring(si, ci - si - 1);
                        }
                        else
                        {
                            yield return s.Substring(si, ci - si);
                        }
                    }

                    si = ++ci;
                }
                else
                {
                    ci++;
                }
            }

            Debug.Assert(ci == s.Length);
            if (si < ci)
            {
                yield return s.Substring(si, ci - si);
            }
            else
            {
                yield return string.Empty;
            }
        }

        /// <summary>
        /// Splits a given string into separate lines (based on the presence of CRLF or LF sequences).
        /// </summary>
        /// <param name="s">The string to split.</param>
        /// <returns>A sequence of strings, each representing an individual line in the string.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown if <paramref name="s"/> is <c>null</c>.</exception>
        [NotNull]
        [ItemNotNull]
        public static IEnumerable<string> SplitIntoLines([NotNull] this string s)
        {
            Validate.ArgumentNotNull(nameof(s), s);

            return SplitIntoLinesIterate(s);
        }

        [NotNull]
        private static IEnumerable<string> WordWrapIterate([NotNull] this string s, int lineLength)
        {
            Debug.Assert(s != null);
            Debug.Assert(lineLength > 0);

            foreach (var line in SplitIntoLinesIterate(s))
            {
                var si = 0;
                while (si < line.Length)
                {
                    var lxi = -1;
                    var i = si;
                    for (; i - si < lineLength && i < line.Length; i++)
                    {
                        if (
                            line[i] != '\r' &&
                            (char.IsWhiteSpace(line, i) || char.IsPunctuation(line, i)))
                        {
                            lxi = i;
                        }
                    }

                    if (i == line.Length)
                    {
                        yield return line.Substring(si, i - si);

                        si = i;
                    }
                    else if (char.IsWhiteSpace(line, i) && lineLength > 1)
                    {
                        yield return line.Substring(si, lineLength);

                        si += lineLength + 1;
                    }
                    else if (lxi > -1)
                    {
                        if (char.IsPunctuation(line, lxi))
                        {
                            yield return line.Substring(si, lxi - si + 1);
                        }
                        else
                        {
                            yield return line.Substring(si, lxi - si);
                        }

                        si = lxi + 1;
                    }
                    else
                    {
                        yield return line.Substring(si, lineLength);

                        si += lineLength;
                    }
                }
            }
        }

        /// <summary>
        /// Wraps the specified string according to a given line length.
        /// </summary>
        /// <param name="s">The string to word wrap.</param>
        /// <param name="lineLength">Length of the line.</param>
        /// <returns>A sequence of lines containing the word wrapped string.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown if <paramref name="s"/> is <c>null</c>.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown if <paramref name="lineLength"/> is less than one.</exception>
        [NotNull]
        public static IEnumerable<string> WordWrap([NotNull] this string s, int lineLength)
        {
            Validate.ArgumentNotNull(nameof(s), s);
            Validate.ArgumentGreaterThanZero(nameof(lineLength), lineLength);

            return WordWrapIterate(s, lineLength);
        }

        /// <summary>
        /// Strips the diacritics from a given string, replacing the characters in question with equivalent non-diacritic ones.
        /// </summary>
        /// <param name="s">The string.</param>
        /// <returns>A string with stripped diacritics.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown if <paramref name="s"/> is <c>null</c>.</exception>
        [NotNull]
        public static string StripDiacritics([NotNull] this string s)
        {
            Validate.ArgumentNotNull(nameof(s), s);

            var normalizedForm = s.Normalize(NormalizationForm.FormD);
            var result = new StringBuilder();
            foreach (var c in normalizedForm)
            {
                switch (CharUnicodeInfo.GetUnicodeCategory(c))
                {
                    case UnicodeCategory.NonSpacingMark:
                    case UnicodeCategory.SpacingCombiningMark:
                    case UnicodeCategory.EnclosingMark:
                        break;
                    default:
                        result.Append(SpecialDiacriticMappings.TryGetValue(c, out var folded) ? folded : c);
                        break;
                }
            }

            return result.ToString().Normalize(NormalizationForm.FormC);
        }
    }
}