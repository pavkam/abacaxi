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

namespace Abacaxi.Tests.SequenceExtensions
{
    using System;
    using NUnit.Framework;

    [TestFixture]
    public class GetLongestCommonSubSequenceTests
    {
        [Test]
        public void GetLongestCommonSubSequence_ThowsException_ForNullSequence()
        {
            Assert.Throws<ArgumentNullException>(() =>
                Abacaxi.SequenceExtensions.GetLongestCommonSubSequence(null, new char[] {}));
        }

        [Test]
        public void GetLongestCommonSubSequence_ThowsException_ForNullOtherSequence()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new char[] { }.GetLongestCommonSubSequence(null));
        }

        [TestCase("", "", "")]
        [TestCase("a", "a", "a")]
        [TestCase("a", "b", "")]
        [TestCase("", "a", "")]
        [TestCase("a", "", "")]
        [TestCase("ab", "a", "a")]
        [TestCase("a", "ab", "a")]
        [TestCase("ab", "ba", "b")]
        [TestCase("hello my dear friend", "Hello you fiend!", "ello y fiend")]
        public void GetLongestCommonSubSequence_ReturnsExpectedSequence(string s1, string s2, string expected)
        {
            var actual = new string(s1.ToCharArray().GetLongestCommonSubSequence(s2.ToCharArray()));
            Assert.AreEqual(expected, actual);
        }
    }
}
