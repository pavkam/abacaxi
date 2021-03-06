﻿/* Copyright 2017-2019 by Alexandru Ciobanu (alex+git@ciobanu.org)
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
    public sealed class SelectValuesTests
    {
        [Test]
        public void SelectValues_ReturnsElement_ForSequenceOfOneElement()
        {
            var sequence = new int?[] {1};
            var result = sequence.SelectValues();

            TestHelper.AssertSequence(result, 1);
        }

        [Test]
        public void SelectValues_ReturnsElements_ForSequenceOfTwoAndThree()
        {
            var sequence = new int?[] {null, 1, null, 2, 3};
            var result = sequence.SelectValues();

            TestHelper.AssertSequence(result, 1, 2, 3);
        }

        [Test]
        public void SelectValues_ReturnsNothing_ForEmptySequence()
        {
            var sequence = new int?[] { };
            var result = sequence.SelectValues();

            TestHelper.AssertSequence(result);
        }

        [Test]
        public void SelectValues_ReturnsNothing_ForSequenceOfNull()
        {
            var sequence = new int?[] {null};
            var result = sequence.SelectValues();

            TestHelper.AssertSequence(result);
        }

        [Test, SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void SelectValues_ThrowsException_ForNullSequence()
        {
            Assert.Throws<ArgumentNullException>(() => ((int?[]) null).SelectValues());
        }
    }
}