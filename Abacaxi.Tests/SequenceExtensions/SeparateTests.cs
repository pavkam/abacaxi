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

namespace Abacaxi.Tests.SequenceExtensions
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using NUnit.Framework;

    [TestFixture]
    public sealed class SeparateTests
    {
        [Test]
        public void Separate_ReturnsEmptyArrays_IfEmptyInput()
        {
            var (m, n) = "".Separate(c => true);

            TestHelper.AssertSequence(m);
            TestHelper.AssertSequence(n);
        }

        [Test]
        public void Separate_ReturnsEmptyMatchingArray_IfNoneMatch()
        {
            var (m, _) = "test".Separate(c => false);

            TestHelper.AssertSequence(m);
        }

        [Test]
        public void Separate_ReturnsEmptyNotMatchingArray_IfAllMatch()
        {
            var (_, n) = "test".Separate(c => true);

            TestHelper.AssertSequence(n);
        }

        [Test]
        public void Separate_ReturnsFullMatchingArray_IfAllMatch()
        {
            var (m, _) = "test".Separate(c => true);

            TestHelper.AssertSequence(m, 't', 'e', 's', 't');
        }

        [Test]
        public void Separate_ReturnsFullNotMatchingArray_IfNoneMatch()
        {
            var (_, n) = "test".Separate(c => false);

            TestHelper.AssertSequence(n, 't', 'e', 's', 't');
        }

        [Test]
        public void Separate_SeparatesElements_Accordingly()
        {
            var (m, n) = "t35o".Separate(char.IsLetter);

            TestHelper.AssertSequence(m, 't', 'o');
            TestHelper.AssertSequence(n, '3', '5');
        }

        [Test, SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void Separate_ThrowsException_IfPredicateIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => { "alex".Separate(null); });
        }

        [Test, SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void Separate_ThrowsException_IfSequenceIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => { ((int[]) null).Separate(i => true); });
        }
    }
}