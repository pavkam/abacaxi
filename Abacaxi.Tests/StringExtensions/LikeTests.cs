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

using System.Diagnostics.CodeAnalysis;

namespace Abacaxi.Tests.StringExtensions
{
    using System;
    using NUnit.Framework;

    [TestFixture]
    public sealed class LikeTests
    {
        [Test,SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void Like_ThrowsException_IfStringIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => ((string)null).Like(""));
        }

        [Test,SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void Like_ThrowsException_IfPatternIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => "".Like(null));
        }

        [TestCase("a", "A", true, true),TestCase("a", "a", false, true),TestCase("a", "A", false, false)]
        public void Like_TakesIntoAccountCasing(string s, string p, bool i, bool expected)
        {
            Assert.AreEqual(expected, s.Like(p, i));
        }

        [TestCase("a", "A"),TestCase("a*b*c", "abc"),TestCase("a?b?c", "aoboc")]
        public void Like_MatchesTheExpectedString(string p, string s)
        {
            Assert.IsTrue(s.Like(p));
        }
    }
}