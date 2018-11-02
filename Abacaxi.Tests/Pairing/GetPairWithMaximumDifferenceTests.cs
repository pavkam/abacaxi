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

namespace Abacaxi.Tests.Pairing
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using NUnit.Framework;
    using Pairing = Abacaxi.Pairing;

    [TestFixture]
    public sealed class GetPairWithMaximumDifferenceTests
    {
        [Test]
        public void GetPairWithMaximumDifference_ReturnsNull_ForEmptySequence()
        {
            var result = Pairing.GetPairWithMaximumDifference(new int[] { }, Comparer<int>.Default);

            Assert.IsNull(result);
        }

        [Test]
        public void GetPairWithMaximumDifference_ReturnsNull_ForOneElement()
        {
            var result = Pairing.GetPairWithMaximumDifference(new[] {1}, Comparer<int>.Default);

            Assert.IsNull(result);
        }

        [Test]
        public void GetPairWithMaximumDifference_ReturnsThePair_ForEqualElements()
        {
            var result = Pairing.GetPairWithMaximumDifference(new[] {1, 1, 1, 1}, Comparer<int>.Default);

            Assert.AreEqual((1, 1), result);
        }

        [Test]
        public void GetPairWithMaximumDifference_ReturnsThePair_ForThreeElements()
        {
            var result = Pairing.GetPairWithMaximumDifference(new[] {1, 2, 3}, Comparer<int>.Default);

            Assert.AreEqual((1, 3), result);
        }

        [Test]
        public void GetPairWithMaximumDifference_ReturnsThePair_ForTwoElements_1()
        {
            var result = Pairing.GetPairWithMaximumDifference(new[] {1, 2}, Comparer<int>.Default);

            Assert.AreEqual((1, 2), result);
        }

        [Test]
        public void GetPairWithMaximumDifference_ReturnsThePair_ForTwoElements_2()
        {
            var result = Pairing.GetPairWithMaximumDifference(new[] {2, 1}, Comparer<int>.Default);

            Assert.AreEqual((1, 2), result);
        }

        [Test]
        public void GetPairWithMaximumDifference_TakesComparerIntoAccount()
        {
            var result = Pairing.GetPairWithMaximumDifference(new[] {-1, 0, 1}, Comparer<int>.Create((l, r) => r - l));

            Assert.AreEqual((1, -1), result);
        }

        [Test, SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void GetPairWithMaximumDifference_ThrowsException_ForNullComparer()
        {
            Assert.Throws<ArgumentNullException>(
                () => Pairing.GetPairWithMaximumDifference(new int[] { }, null));
        }

        [Test, SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void GetPairWithMaximumDifference_ThrowsException_ForNullSequence()
        {
            Assert.Throws<ArgumentNullException>(
                () => Pairing.GetPairWithMaximumDifference(null, Comparer<int>.Default));
        }

        [Test]
        public void GetPairWithMaximumDifference_WorksForNegative()
        {
            var result = Pairing.GetPairWithMaximumDifference(new[] {0, -100, 100}, Comparer<int>.Default);

            Assert.AreEqual((-100, 100), result);
        }
    }
}