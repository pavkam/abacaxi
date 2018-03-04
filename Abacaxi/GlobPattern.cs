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
    using System.Text;
    using Internal;
    using JetBrains.Annotations;
    using System.Text.RegularExpressions;
    using System.Collections.Concurrent;

    /// <summary>
    /// Class implements the "glob-style" pattern matching.
    /// </summary>
    public sealed class GlobPattern
    {
        [NotNull]
        private static readonly ConcurrentDictionary<string, GlobPattern> CachedPatterns =
            new ConcurrentDictionary<string, GlobPattern>();
        [NotNull]
        private static readonly ConcurrentDictionary<string, GlobPattern> CachedIgnoreCasePatterns =
            new ConcurrentDictionary<string, GlobPattern>();

        [NotNull]
        internal static GlobPattern GetPattern([NotNull] string pattern, bool ignoreCase)
        {
            Assert.NotNull(pattern);
            return ignoreCase ?
                CachedIgnoreCasePatterns.GetOrAdd(pattern, key => new GlobPattern(key, true)) :
                CachedPatterns.GetOrAdd(pattern, key => new GlobPattern(key, false));
        }

        [NotNull]
        private readonly Regex _regex;

        /// <summary>
        /// Initializes a new instance of the <see cref="GlobPattern"/> class.
        /// </summary>
        /// <param name="pattern">The glob-style pattern.</param>
        /// <param name="ignoreCase">Supply a value of <c>true</c> to make the pattern case insensitive.</param>
        /// <exception cref="System.ArgumentNullException">Thrown if <paramref name="pattern"/> is <c>null</c>.</exception>
        public GlobPattern([NotNull] string pattern, bool ignoreCase)
        {
            Validate.ArgumentNotNull(nameof(pattern), pattern);

            var rePattern = new StringBuilder("^");
            var s = 0;
            for (var i = 0; i < pattern.Length; i++)
            {
                var pc = pattern[i];
                if (pc != '*' && pc != '?')
                {
                    continue;
                }

                if (s < i)
                {
                    var escapedChunk = Regex.Escape(pattern.Substring(s, i - s));
                    rePattern.Append(escapedChunk);
                }

                rePattern.Append(pc == '*' ? ".*" : ".");
                s = i + 1;
            }

            if (s < pattern.Length)
            {
                var escapedChunk = Regex.Escape(pattern.Substring(s, pattern.Length - s));
                rePattern.Append(escapedChunk);
            }


            rePattern.Append('$');

            _regex = new Regex(rePattern.ToString(), ignoreCase ? RegexOptions.IgnoreCase : RegexOptions.None);
        }

        /// <summary>
        /// Determines whether the specified string matches this pattern.
        /// </summary>
        /// <param name="s">The string to check.</param>
        /// <returns>
        ///   <c>true</c> if the specified string matches the pattern; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">Thrown if <paramref name="s"/> is <c>null</c>.</exception>
        public bool IsMatch([NotNull] string s) => _regex.IsMatch(s);
    }
}
