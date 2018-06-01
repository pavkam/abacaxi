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

namespace Abacaxi.Tests.StringExtensions
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using JetBrains.Annotations;
    using NUnit.Framework;

    [TestFixture]
    public sealed class WordWrapTests
    {
        [TestCase("a", 1), TestCase("abc", 10), TestCase("abc def", 50)]
        public void WordWrap_ReturnsWholeString_IfLineLengthEqualOrLongerThanStringLength([NotNull] string s, int l)
        {
            TestHelper.AssertSequence(s.WordWrap(l), s);
        }

        [TestCase('.'), TestCase(','), TestCase(';'), TestCase('!'), TestCase('?'), TestCase('-'), TestCase('+'),
         TestCase('/'), TestCase('\\'), TestCase('*'), TestCase('^')]
        public void WordWrap_WillFunctionOverKnownSpecialCharacters(char c)
        {
            var result = $"ab{c}cd".WordWrap(3);
            TestHelper.AssertSequence(result,
                $"ab{c}", "cd");
        }

        [Test]
        public void WordWrap_ConsidersCrAsNothing()
        {
            var result = "a\rbb".WordWrap(2);
            TestHelper.AssertSequence(result,
                "a\r", "bb");
        }

        [Test]
        public void WordWrap_ConsidersCrlfAsSeparator()
        {
            var result = "a\r\nbb".WordWrap(2);
            TestHelper.AssertSequence(result,
                "a", "bb");
        }

        [Test]
        public void WordWrap_ConsidersLfAsSeparator()
        {
            var result = "a\nbb".WordWrap(2);
            TestHelper.AssertSequence(result,
                "a", "bb");
        }

        [Test]
        public void WordWrap_GoesConsecutive_ForSpecials()
        {
            var result = "aaaabbb.cc.cd.d.dd...e".WordWrap(4);
            TestHelper.AssertSequence(result,
                "aaaa", "bbb.", "cc.", "cd.", "d.", "dd..", ".e");
        }

        [Test]
        public void WordWrap_GoesConsecutive_ForWhiteSpaces()
        {
            var result = "aaaabbb cc cd d dd   e".WordWrap(4);
            TestHelper.AssertSequence(result,
                "aaaa", "bbb", "cc", "cd d", "dd  ", "e");
        }

        [Test]
        public void WordWrap_ReturnsNothing_ForEmptyString()
        {
            Assert.IsEmpty(string.Empty.WordWrap(1));
        }

        [Test]
        public void WordWrap_SeparatesOnWhiteSpaceAndEatsIt()
        {
            var result = "a bb".WordWrap(2);
            TestHelper.AssertSequence(result,
                "a", "bb");
        }

        [Test]
        public void WordWrap_SeparatesOnWhiteSpaceAndEatsIt_ButKeepsTheOthers()
        {
            var result = "\t\t \t".WordWrap(2);
            TestHelper.AssertSequence(result,
                "\t\t", "\t");
        }

        [Test]
        public void WordWrap_SplitsStringIntoIndividualCharacters_AtLengthOfOne()
        {
            var result = "a b c".WordWrap(1);
            TestHelper.AssertSequence(result,
                "a", "", "b", "", "c");
        }

        [Test]
        public void WordWrap_ThrowsException_IfLineLengthIsLessThanOne()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => "test".WordWrap(0));
        }

        [Test, SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void WordWrap_ThrowsException_IfStringIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => ((string) null).WordWrap(1));
        }

        [Test]
        public void WordWrap_WillRetainWhiteSpace_ExcludingTheSeparator()
        {
            var result = "\t\t \t\t".WordWrap(2);
            TestHelper.AssertSequence(result,
                "\t\t", "\t\t");
        }

        [Test]
        public void WordWrap_WillSeparateOnSpecial_ButRetainTheCharacter()
        {
            var result = "ab.cd".WordWrap(3);
            TestHelper.AssertSequence(result,
                "ab.", "cd");
        }
    }
}