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
    using NUnit.Framework;

    [TestFixture]
    public sealed class SplitIntoLinesTests
    {
        [Test]
        public void SplitIntoLines_GeneratesTwoEmptyLinesOnASingleLineBreak()
        {
            const string line = "\n";
            TestHelper.AssertSequence(line.SplitIntoLines(), "", "");
        }

        [Test]
        public void SplitIntoLines_IgnoresCR_IfNotFollowedByLF()
        {
            const string line = "The\rbird\ris\r\rthe\r\r\rword.";
            TestHelper.AssertSequence(line.SplitIntoLines(), line);
        }

        [Test]
        public void SplitIntoLines_ReturnsOriginalStringAsLine_IfNoLineBreaksFound()
        {
            const string line = "This is a single line";
            TestHelper.AssertSequence(line.SplitIntoLines(), line);
        }

        [Test]
        public void SplitIntoLines_ReturnsString_IfEmpty()
        {
            TestHelper.AssertSequence(string.Empty.SplitIntoLines(), "");
        }

        [Test, SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void SplitIntoLines_ThrowsException_IfStringIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => ((string) null).SplitIntoLines());
        }

        [Test]
        public void SplitIntoLines_UnderstandsCRAndLFInMixture()
        {
            const string line = "The\n\rcut\r\nis\nhere.";
            TestHelper.AssertSequence(line.SplitIntoLines(), "The", "\rcut", "is", "here.");
        }

        [Test]
        public void SplitIntoLines_UnderstandsCRLFCharacters()
        {
            const string line = "The\r\ncut.";
            TestHelper.AssertSequence(line.SplitIntoLines(), "The", "cut.");
        }

        [Test]
        public void SplitIntoLines_UnderstandsLineBreakPrefix()
        {
            const string line = "\na";
            TestHelper.AssertSequence(line.SplitIntoLines(), "", "a");
        }

        [Test]
        public void SplitIntoLines_UnderstandsLineBreakSuffix()
        {
            const string line = "a\n";
            TestHelper.AssertSequence(line.SplitIntoLines(), "a", "");
        }

        [Test]
        public void SplitIntoLines_UnderstandsSingleLFCharacter()
        {
            const string line = "The\ncut.";
            TestHelper.AssertSequence(line.SplitIntoLines(), "The", "cut.");
        }
    }
}