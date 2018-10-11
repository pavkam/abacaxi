﻿/* Copyright 2017-2018 by Alexandru Ciobanu (alex+git@ciobanu.org)
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
    public sealed class IsOrderedTests
    {
        [Test]
        public void IsOrdered1_ReturnsFalse_IfThreeElementsAreNotOrderedAscending()
        {
            var result = new[] {2, 3, 1}.IsOrdered(Comparer<int>.Default);

            Assert.IsFalse(result);
        }

        [Test]
        public void IsOrdered1_ReturnsFalse_IfTwoElementsAreNotOrderedAscending()
        {
            var result = new[] {2, 1}.IsOrdered(Comparer<int>.Default);

            Assert.IsFalse(result);
        }


        [Test]
        public void IsOrdered1_ReturnsTrue_ForEmptyCollection()
        {
            var isOrdered = new string[] { }.IsOrdered(StringComparer.Ordinal);
            Assert.IsTrue(isOrdered);
        }

        [Test]
        public void IsOrdered1_ReturnsTrue_ForEqualElements()
        {
            var result = new[] {1, 1}.IsOrdered(Comparer<int>.Default);

            Assert.IsTrue(result);
        }


        [Test]
        public void IsOrdered1_ReturnsTrue_ForOneElement()
        {
            var isOrdered = new[] {"A"}.IsOrdered(StringComparer.Ordinal);
            Assert.IsTrue(isOrdered);
        }

        [Test]
        public void IsOrdered1_ReturnsTrue_IfThreeElementsAreOrderedAscending()
        {
            var result = new[] {3, 5, 8}.IsOrdered(Comparer<int>.Default);

            Assert.IsTrue(result);
        }

        [Test]
        public void IsOrdered1_ReturnsTrue_IfTwoElementsAreOrderedAscending()
        {
            var result = new[] {1, 2}.IsOrdered(Comparer<int>.Default);

            Assert.IsTrue(result);
        }


        [Test]
        public void IsOrdered1_TakesComparer_IntoAccount()
        {
            var result = new[] {"A", "a"}.IsOrdered(StringComparer.OrdinalIgnoreCase);
            Assert.IsTrue(result);
        }

        [Test, SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void IsOrdered1_ThrowsException_ForNullComparer()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new[] {"bb", "ccc", "a", "z"}.IsOrdered(null));
        }

        [Test, SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void IsOrdered1_ThrowsException_ForNullSequence()
        {
            Assert.Throws<ArgumentNullException>(() =>
                ((string[]) null).IsOrdered(StringComparer.Ordinal));
        }

        [Test]
        public void IsOrdered2_ReturnsFalse_IfThreeElementsAreNotOrderedAscending()
        {
            var result = new[] {2, 3, 1}.IsOrdered();

            Assert.IsFalse(result);
        }

        [Test]
        public void IsOrdered2_ReturnsFalse_IfTwoElementsAreNotOrderedAscending()
        {
            var result = new[] {2, 1}.IsOrdered();

            Assert.IsFalse(result);
        }

        [Test]
        public void IsOrdered2_ReturnsTrue_ForEmptyCollection()
        {
            var isOrdered = new string[] { }.IsOrdered();
            Assert.IsTrue(isOrdered);
        }

        [Test]
        public void IsOrdered2_ReturnsTrue_ForEqualElements()
        {
            var result = new[] {1, 1}.IsOrdered();

            Assert.IsTrue(result);
        }

        [Test]
        public void IsOrdered2_ReturnsTrue_ForOneElement()
        {
            var isOrdered = new[] {"A"}.IsOrdered();
            Assert.IsTrue(isOrdered);
        }

        [Test]
        public void IsOrdered2_ReturnsTrue_IfThreeElementsAreOrderedAscending()
        {
            var result = new[] {3, 5, 8}.IsOrdered();

            Assert.IsTrue(result);
        }

        [Test]
        public void IsOrdered2_ReturnsTrue_IfTwoElementsAreOrderedAscending()
        {
            var result = new[] {1, 2}.IsOrdered();

            Assert.IsTrue(result);
        }

        [Test, SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void IsOrdered2_ThrowsException_ForNullSequence()
        {
            Assert.Throws<ArgumentNullException>(() =>
                ((string[]) null).IsOrdered());
        }
    }
}