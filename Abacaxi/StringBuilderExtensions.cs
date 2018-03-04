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
    using Internal;
    using JetBrains.Annotations;
    using System.Text;

    /// <summary>
    /// Extension method for the <see cref="StringBuilder"/> class.
    /// </summary>
    [PublicAPI]
    public static class StringBuilderExtensions
    {
        /// <summary>
        /// Appends a line to the string builder if the value is not empty.
        /// </summary>
        /// <param name="builder">The string builder.</param>
        /// <param name="value">The value to append.</param>
        /// <returns>A sequence of lines containing the word wrapped string.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown if <paramref name="builder"/> is <c>null</c>.</exception>
        [NotNull]
        public static StringBuilder AppendNotEmptyLine([NotNull] this StringBuilder builder, [CanBeNull] string value)
        {
            Validate.ArgumentNotNull(nameof(builder), builder);

            if (!string.IsNullOrEmpty(value))
            {
                builder.AppendLine(value);
            }

            return builder;
        }
    }
}