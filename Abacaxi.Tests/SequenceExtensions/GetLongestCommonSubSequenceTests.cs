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

namespace Abacaxi.Tests.SequenceExtensions
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using JetBrains.Annotations;
    using NUnit.Framework;
    using SequenceExtensions = Abacaxi.SequenceExtensions;

    [TestFixture]
    public sealed class GetLongestCommonSubSequenceTests
    {
        [TestCase("", "", ""), TestCase("a", "a", "a"), TestCase("a", "b", ""), TestCase("", "a", ""),
         TestCase("a", "", ""), TestCase("ab", "a", "a"), TestCase("a", "ab", "a"), TestCase("ab", "ba", "b"),
         TestCase("hello my dear friend", "Hello you fiend!", "ello y fiend"),
         SuppressMessage("ReSharper", "StringLiteralTypo")]
        public void GetLongestCommonSubSequence_ReturnsExpectedSequence([NotNull] string s1, [NotNull] string s2,
            string expected)
        {
            var actual = new string(s1.ToCharArray().GetLongestCommonSubSequence(s2.ToCharArray()));
            Assert.AreEqual(expected, actual);
        }

        [Test, SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void GetLongestCommonSubSequence_ThrowsException_ForNullOtherSequence()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new char[] { }.GetLongestCommonSubSequence(null));
        }

        [Test, SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void GetLongestCommonSubSequence_ThrowsException_ForNullSequence()
        {
            Assert.Throws<ArgumentNullException>(() =>
                SequenceExtensions.GetLongestCommonSubSequence(null, new char[] { }));
        }
    }
}