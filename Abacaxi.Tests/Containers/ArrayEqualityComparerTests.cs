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

namespace Abacaxi.Tests.Containers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Abacaxi.Containers;
    using NUnit.Framework;

    [TestFixture]
    public sealed class ArrayEqualityComparerTests
    {
        private readonly ArrayEqualityComparer<int> _comparer =
            new ArrayEqualityComparer<int>(EqualityComparer<int>.Default);

        [Test]
        public void Comparer_TakesIntoAccount_TheElementComparer()
        {
            var comparer = new ArrayEqualityComparer<string>(StringComparer.OrdinalIgnoreCase);
            var result = comparer.Equals(new[] {"a"}, new[] {"A"});

            Assert.IsTrue(result);
        }

        [Test, SuppressMessage("ReSharper", "ObjectCreationAsStatement"),
         SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void Ctor_ThrowsException_IfCompareIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new ArrayEqualityComparer<int>(null));
        }

        [Test]
        public void Default_ReturnsTheDefaultComparer()
        {
            var comparer = ArrayEqualityComparer<string>.Default;
            var result = comparer.Equals(new[] {"a"}, new[] {"A"});

            Assert.IsFalse(result);
        }

        [Test]
        public void Equals_ReturnsFalse_IfElementsDiffer()
        {
            var result = _comparer.Equals(new[] {1, 2}, new[] {1, 3});

            Assert.IsFalse(result);
        }

        [Test]
        public void Equals_ReturnsFalse_IfLeftIsNull()
        {
            var result = _comparer.Equals(null, new int[] { });

            Assert.IsFalse(result);
        }

        [Test]
        public void Equals_ReturnsFalse_IfLengthsDiffer()
        {
            var result = _comparer.Equals(new[] {1, 2}, new[] {1});

            Assert.IsFalse(result);
        }

        [Test]
        public void Equals_ReturnsFalse_IfRightIsNull()
        {
            var result = _comparer.Equals(new int[] { }, null);

            Assert.IsFalse(result);
        }

        [Test]
        public void Equals_ReturnsTrue_IfBothArrayAreEmpty()
        {
            var result = _comparer.Equals(new int[] { }, new int[] { });

            Assert.IsTrue(result);
        }

        [Test]
        public void Equals_ReturnsTrue_IfBothArrayAreNull()
        {
            var result = _comparer.Equals(null, null);

            Assert.IsTrue(result);
        }

        [Test]
        public void Equals_ReturnsTrue_IfSameArray()
        {
            var a = new int[] { };
            var result = _comparer.Equals(a, a);

            Assert.IsTrue(result);
        }

        [Test]
        public void GetHashCode_ReturnsDifferentHashCodes_ForDifferentArrays()
        {
            var r1 = _comparer.GetHashCode(new[] {1, 2, 3});
            var r2 = _comparer.GetHashCode(new[] {1, 2, 4});

            Assert.AreNotEqual(r1, r2);
        }

        [Test]
        public void GetHashCode_ReturnsSameHashCode_ForEqualArrays()
        {
            var r1 = _comparer.GetHashCode(new[] {1, 2, 3});
            var r2 = _comparer.GetHashCode(new[] {1, 2, 3});

            Assert.AreEqual(r1, r2);
        }

        [Test, SuppressMessage("ReSharper", "AssignNullToNotNullAttribute"),
         SuppressMessage("ReSharper", "ReturnValueOfPureMethodIsNotUsed")]
        public void GetHashCode_ThrowsException_ForNullArray()
        {
            Assert.Throws<ArgumentNullException>(() => _comparer.GetHashCode(null));
        }
    }
}