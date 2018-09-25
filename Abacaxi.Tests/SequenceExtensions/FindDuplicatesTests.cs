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
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using NUnit.Framework;
    using SequenceExtensions = Abacaxi.SequenceExtensions;

    [TestFixture]
    public class FindDuplicatesTests
    {
        [Test]
        public void GenericFindDuplicates_ReturnsDuplicates_ForDistinctElements()
        {
            TestHelper.AssertSequence(
                "121312".FindDuplicates(EqualityComparer<char>.Default),
                new Frequency<char>('1', 3),
                new Frequency<char>('2', 2));
        }

        [Test]
        public void GenericFindDuplicates_ReturnsNothing_ForDistinctElements()
        {
            TestHelper.AssertSequence(
                "123456789".FindDuplicates(EqualityComparer<char>.Default));
        }

        [Test, SuppressMessage("ReSharper", "IteratorMethodResultIsIgnored"),
         SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void GenericFindDuplicates_ThrowsException_ForNullEqualityComparer()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new[] {1}.FindDuplicates(null));
        }

        [Test, SuppressMessage("ReSharper", "IteratorMethodResultIsIgnored"),
         SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void GenericFindDuplicates_ThrowsException_ForNullSequence()
        {
            Assert.Throws<ArgumentNullException>(() =>
                SequenceExtensions.FindDuplicates(null, EqualityComparer<int>.Default));
        }

        [Test]
        public void GenericFindDuplicates_UsesTheComparer()
        {
            TestHelper.AssertSequence(
                new[] {"a", "A"}.FindDuplicates(StringComparer.InvariantCultureIgnoreCase),
                new Frequency<string>("a", 2));
        }

        [Test]
        public void IntegerFindDuplicates_ReturnsDuplicates_ForDistinctElements()
        {
            TestHelper.AssertSequence(
                new[] {1, 2, 1, 3, 1, 2}.FindDuplicates(1, 3),
                new Frequency<int>(1, 3),
                new Frequency<int>(2, 2));
        }

        [Test]
        public void IntegerFindDuplicates_ReturnsNothing_ForDistinctElements()
        {
            TestHelper.AssertSequence(
                new[] {1, 2, 3, 4, 5, 6, 7, 8, 9}.FindDuplicates(1, 9));
        }

        [Test, SuppressMessage("ReSharper", "IteratorMethodResultIsIgnored")]
        public void IntegerFindDuplicates_ThrowsException_ForMaxLessThanMin()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                new[] {1}.FindDuplicates(1, 0));
        }

        [Test, SuppressMessage("ReSharper", "IteratorMethodResultIsIgnored"),
         SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void IntegerFindDuplicates_ThrowsException_ForNullSequence()
        {
            Assert.Throws<ArgumentNullException>(() =>
                SequenceExtensions.FindDuplicates(null, 1, 1));
        }

        [Test, SuppressMessage("ReSharper", "ReturnValueOfPureMethodIsNotUsed")]
        public void IntegerFindDuplicates_ThrowsException_IfSequenceContainsElementsOutOfMinAndMax()
        {
            Assert.Throws<InvalidOperationException>(() =>
                new[] {0, 1, -1}.FindDuplicates(0, 1).ToArray());
        }

        [Test, SuppressMessage("ReSharper", "IteratorMethodResultIsIgnored"),
         SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void StringFindDuplicates_ThrowsException_ForNullSequence()
        {
            Assert.Throws<ArgumentNullException>(() =>
                ((string) null).FindDuplicates());
        }
    }
}