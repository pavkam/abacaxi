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
    using NUnit.Framework;

    [TestFixture]
    public sealed class FindUniquesTests
    {
        [Test]
        public void GenericFindUniques_ReturnsDuplicates_ForDistinctElements()
        {
            TestHelper.AssertSequence(
                "0121312".FindUniques(EqualityComparer<char>.Default), '0', '3');
        }

        [Test]
        public void GenericFindUniques_ReturnsNothing_ForDuplicateElements()
        {
            TestHelper.AssertSequence(
                "1234554321".FindUniques(EqualityComparer<char>.Default));
        }

        [Test, SuppressMessage("ReSharper", "IteratorMethodResultIsIgnored"),
         SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void GenericFindUniques_ThrowsException_ForNullEqualityComparer()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new[] {1}.FindUniques(null));
        }

        [Test, SuppressMessage("ReSharper", "IteratorMethodResultIsIgnored"),
         SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void GenericFindUniques_ThrowsException_ForNullSequence()
        {
            Assert.Throws<ArgumentNullException>(() =>
                SequenceAlgorithms.FindUniques(null, EqualityComparer<int>.Default));
        }

        [Test]
        public void GenericFindUniques_UsesTheComparer()
        {
            TestHelper.AssertSequence(
                new[] {"a", "A"}.FindUniques(StringComparer.InvariantCultureIgnoreCase));
        }
    }
}