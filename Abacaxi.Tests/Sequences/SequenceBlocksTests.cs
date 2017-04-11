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

namespace Abacaxi.Tests.Sequences
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Abacaxi.Sequences;
    using NUnit.Framework;

    [TestFixture]
    public class SequenceBlocksTests
    {
        [Test]
        public void Extract_ThrowsException_ForNullSequence()
        {
            Assert.Throws<ArgumentNullException>(() =>
                NestedGroups.Extract((int[])null, 1, 1, EqualityComparer<int>.Default).ToArray());
        }

        [Test]
        public void Extract_ThrowsException_ForComparer()
        {
            Assert.Throws<ArgumentNullException>(() =>
                NestedGroups.Extract(new int[] { }, 1, 1, null).ToArray());
        }

        [Test]
        public void Extract_ThrowsException_ForOrphanOpenBracket()
        {
            Assert.Throws<InvalidOperationException>(() =>
                NestedGroups.Extract("(", '(', ')', EqualityComparer<char>.Default).ToArray());
        }

        [Test]
        public void Extract_ThrowsException_ForOrphanCloseBracket()
        {
            Assert.Throws<InvalidOperationException>(() =>
                NestedGroups.Extract(")", '(', ')', EqualityComparer<char>.Default).ToArray());
        }

        [Test]
        public void Extract_ReturnsNothing_ForEmptySequence()
        {
            TestHelper.AssertSequence(
                NestedGroups.Extract(new int[] { }, 1, 1, EqualityComparer<int>.Default));
        }

        [Test]
        public void Extract_ReturnsFullSequence_IfNoBlocksDefined()
        {
            TestHelper.AssertSequence(
                NestedGroups.Extract("Hello World", '(', ')', EqualityComparer<char>.Default),
                "Hello World".ToCharArray());
        }

        [Test]
        public void Extract_ReturnsTwoSequences_IfOneBlockPresent()
        {
            TestHelper.AssertSequence(
                NestedGroups.Extract("(Hello World)", '(', ')', EqualityComparer<char>.Default),
                "Hello World".ToCharArray(),
                "(Hello World)".ToCharArray());
        }

        [Test]
        public void Extract_ReturnsAllSequences_InMultiBlock()
        {
            TestHelper.AssertSequence(
                NestedGroups.Extract("a(b(c))d(e)", '(', ')', EqualityComparer<char>.Default),
                "c".ToCharArray(),
                "b(c)".ToCharArray(),
                "e".ToCharArray(),
                "a(b(c))d(e)".ToCharArray());
        }
    }
}
