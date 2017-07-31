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
    using System;
    using System.Reflection;

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
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="s"/> is <c>null</c>.</exception>
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
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="s"/> is <c>null</c>.</exception>
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
        /// Tries to convert a given string to a given type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type to convert to.</typeparam>
        /// <param name="s">The string.</param>
        /// <param name="formatProvider">The format provider.</param>
        /// <param name="result">The converted value, if succeeded.</param>
        /// <returns><c>true</c> if the converts succeeded; <c>false</c> otherwise.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="formatProvider"/> is <c>null</c>.</exception>
        public static bool TryAs<T>([CanBeNull] this string s, [NotNull] IFormatProvider formatProvider, out T result) where T : struct
        {
            Validate.ArgumentNotNull(nameof(formatProvider), formatProvider);

            var typeInfo = typeof(T).GetTypeInfo();
            if (typeInfo.IsEnum)
            {
                return Enum.TryParse(s, true, out result);
            }

            try
            {
                result = (T)Convert.ChangeType(s, typeof(T), formatProvider);
            }
            catch
            {
                result = default(T);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Converts a given string to a given type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type to convert to.</typeparam>
        /// <param name="s">The string.</param>
        /// <param name="formatProvider">The format provider.</param>
        /// <returns>The converted value.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="formatProvider"/> is <c>null</c>.</exception>
        /// <exception cref="FormatException">Thrown if the conversion failed.</exception>
        public static T As<T>([CanBeNull] this string s, [NotNull] IFormatProvider formatProvider) where T : struct
        {
            if (!TryAs(s, formatProvider, out T result))
            {
                throw new FormatException($"Failed to convert the string \"{s}\" into a value of type {typeof(T)}.");
            }

            return result;
        }

        /// <summary>
        /// Converts a given string to a given type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type to convert to.</typeparam>
        /// <param name="s">The string.</param>
        /// <returns>The converted value.</returns>
        /// <remarks>This method uses <see cref="CultureInfo.InvariantCulture"/> as its format provider.</remarks>
        /// <exception cref="FormatException">Thrown if the conversion failed.</exception>
        public static T As<T>([CanBeNull] this string s) where T : struct
        {
            if (!TryAs(s, CultureInfo.InvariantCulture, out T result))
            {
                throw new FormatException($"Failed to convert the string \"{s}\" into a value of type {typeof(T)}.");
            }

            return result;
        }
    }
}
