/* Copyright 2017-2019 by Alexandru Ciobanu (alex+git@ciobanu.org)
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

namespace Abacaxi.Tests.StringExtensions
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using JetBrains.Annotations;
    using NUnit.Framework;

    [TestFixture]
    public sealed class GlobPatternTests
    {
        [TestCase("*", ""), TestCase("*", "1"), TestCase("*", "1234")]
        public void Pattern_Asterisk_MatchesFromNothingToAll([NotNull] string pattern, [NotNull] string test)
        {
            var result = new GlobPattern(pattern, false).IsMatch(test);
            Assert.IsTrue(result);
        }

        [TestCase("", ""), TestCase("exact", "exact"), TestCase("1 2 3", "1 2 3")]
        public void Pattern_String_MatchesItself([NotNull] string pattern, [NotNull] string test)
        {
            var result = new GlobPattern(pattern, false).IsMatch(test);
            Assert.IsTrue(result);
        }

        [TestCase("", " "), TestCase("exact", "exact1"), TestCase("1", "2")]
        public void Pattern_String_DoeNotMatchOther([NotNull] string pattern, [NotNull] string test)
        {
            var result = new GlobPattern(pattern, false).IsMatch(test);
            Assert.IsFalse(result);
        }

        [TestCase("?", "1"), TestCase("??", "12"), TestCase("???", "123")]
        public void Pattern_QuestionMark_MatchesOneCharOnly([NotNull] string pattern, [NotNull] string test)
        {
            var result = new GlobPattern(pattern, false).IsMatch(test);
            Assert.IsTrue(result);
        }

        [TestCase("a?", "a1"), TestCase("?a", "1a"), TestCase("a?c", "abc"), TestCase("a??x", "alex")]
        public void Pattern_QuestionMarkInString_MatchesSimilarString([NotNull] string pattern, [NotNull] string test)
        {
            var result = new GlobPattern(pattern, false).IsMatch(test);
            Assert.IsTrue(result);
        }

        [TestCase("a?", "a"), TestCase("a?", "b1"), TestCase("?a", "1b"), TestCase("?a", "a"), TestCase("a?c", "abd"),
         TestCase("a??x", "alx")]
        public void Pattern_QuestionMarkInString_DoesNotMatchDifferentString([NotNull] string pattern,
            [NotNull] string test)
        {
            var result = new GlobPattern(pattern, false).IsMatch(test);
            Assert.IsFalse(result);
        }

        [TestCase("a*", "a1"), TestCase("a*", "a"), TestCase("a*", "a123"), TestCase("*a", "1a"), TestCase("*a", "a"),
         TestCase("*a", "123a"), TestCase("*a", "aaaa"), TestCase("a*c", "abc"), TestCase("a*x", "alex"),
         TestCase("a*x", "ax"), SuppressMessage("ReSharper", "StringLiteralTypo")]
        public void Pattern_AsteriskInString_MatchesSimilarString([NotNull] string pattern, [NotNull] string test)
        {
            var result = new GlobPattern(pattern, false).IsMatch(test);
            Assert.IsTrue(result);
        }

        [TestCase("a*", "b"), TestCase("a*", "ba"), TestCase("*a", "123ab"), TestCase("a*c", "abcd"),
         SuppressMessage("ReSharper", "StringLiteralTypo")]
        public void Pattern_AsteriskInString_DoesNotMatchDifferentString([NotNull] string pattern,
            [NotNull] string test)
        {
            var result = new GlobPattern(pattern, false).IsMatch(test);
            Assert.IsFalse(result);
        }

        [TestCase("a", "A"), TestCase("ABC", "ABc"), TestCase("*A", "AAAAAa"), TestCase("A??X", "AXXx"),
         SuppressMessage("ReSharper", "StringLiteralTypo")]
        public void Pattern_MatchingFails_IfCaseSensitive([NotNull] string pattern, [NotNull] string test)
        {
            var result = new GlobPattern(pattern, false).IsMatch(test);
            Assert.IsFalse(result);
        }

        [TestCase("a", "A"), TestCase("ABC", "ABc"), TestCase("*A", "AAAAAa"), TestCase("A??X", "AXXx"),
         SuppressMessage("ReSharper", "StringLiteralTypo")]
        public void Pattern_MatchingSucceeds_IfCaseInsensitive([NotNull] string pattern, [NotNull] string test)
        {
            var result = new GlobPattern(pattern, true).IsMatch(test);
            Assert.IsTrue(result);
        }

        [TestCase("1*", "A1"), TestCase("S*T", "S1234T0"), TestCase("-*-", " -1- ")]
        public void Pattern_TriesToMatchFullString([NotNull] string pattern, [NotNull] string test)
        {
            var result = new GlobPattern(pattern, false).IsMatch(test);
            Assert.IsFalse(result);
        }

        [Test, SuppressMessage("ReSharper", "ObjectCreationAsStatement"),
         SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void Ctor_ThrowsException_IfPatternIsNull()
        {
            Assert.Throws<ArgumentNullException>(
                () => new GlobPattern(null, true));
        }

        [Test, SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void IsMatch_ThrowsException_IfStringIsInvalid()
        {
            var glob = new GlobPattern("*", true);
            Assert.Throws<ArgumentNullException>(
                () => glob.IsMatch(null));
        }
    }
}