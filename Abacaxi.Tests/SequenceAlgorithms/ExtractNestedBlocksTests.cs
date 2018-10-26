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

namespace Abacaxi.Tests.SequenceAlgorithms
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using NUnit.Framework;

    [TestFixture]
    public sealed class ExtractNestedBlocksTests
    {
        [Test]
        public void ExtractNestedBlocks_ReturnsAllSequences_InMultiBlock()
        {
            TestHelper.AssertSequence(
                "a(b(c))d(e)".ExtractNestedBlocks('(', ')', EqualityComparer<char>.Default),
                "c".ToCharArray(),
                "b(c)".ToCharArray(),
                "e".ToCharArray(),
                "a(b(c))d(e)".ToCharArray());
        }

        [Test]
        public void ExtractNestedBlocks_ReturnsFullSequence_IfNoBlocksDefined()
        {
            TestHelper.AssertSequence(
                "Hello World".ExtractNestedBlocks('(', ')', EqualityComparer<char>.Default),
                "Hello World".ToCharArray());
        }

        [Test]
        public void ExtractNestedBlocks_ReturnsNothing_ForEmptySequence()
        {
            TestHelper.AssertSequence(
                new int[] { }.ExtractNestedBlocks(1, 1, EqualityComparer<int>.Default));
        }

        [Test]
        public void ExtractNestedBlocks_ReturnsTwoSequences_IfOneBlockPresent()
        {
            TestHelper.AssertSequence(
                "(Hello World)".ExtractNestedBlocks('(', ')', EqualityComparer<char>.Default),
                "Hello World".ToCharArray(),
                "(Hello World)".ToCharArray());
        }

        [Test, SuppressMessage("ReSharper", "IteratorMethodResultIsIgnored"),
         SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void ExtractNestedBlocks_ThrowsException_ForComparer()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new int[] { }.ExtractNestedBlocks(1, 1, null));
        }

        [Test, SuppressMessage("ReSharper", "IteratorMethodResultIsIgnored"),
         SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void ExtractNestedBlocks_ThrowsException_ForNullSequence()
        {
            Assert.Throws<ArgumentNullException>(() =>
                ((int[]) null).ExtractNestedBlocks(1, 1, EqualityComparer<int>.Default));
        }

        [Test, SuppressMessage("ReSharper", "ReturnValueOfPureMethodIsNotUsed")]
        public void ExtractNestedBlocks_ThrowsException_ForOrphanCloseBracket()
        {
            Assert.Throws<InvalidOperationException>(() =>
                ")".ExtractNestedBlocks('(', ')', EqualityComparer<char>.Default).ToArray());
        }

        [Test, SuppressMessage("ReSharper", "ReturnValueOfPureMethodIsNotUsed")]
        public void ExtractNestedBlocks_ThrowsException_ForOrphanOpenBracket()
        {
            Assert.Throws<InvalidOperationException>(() =>
                "(".ExtractNestedBlocks('(', ')', EqualityComparer<char>.Default).ToArray());
        }
    }
}