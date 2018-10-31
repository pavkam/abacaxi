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
    using System.Diagnostics.CodeAnalysis;
    using NUnit.Framework;
    using Pairing = Abacaxi.Pairing;

    [TestFixture]
    public sealed class GetPairWithIncreasingOrderMaximumDifferenceTests
    {
        private static double Subtract(int left, int right)
        {
            return left - right;
        }

        [Test]
        public void GetPairWithIncreasingOrderMaximumDifference_ReturnsNull_ForEmptySequence()
        {
            var result = Pairing.GetPairWithIncreasingOrderMaximumDifference(new int[] { }, Subtract);

            Assert.IsNull(result);
        }

        [Test]
        public void GetPairWithIncreasingOrderMaximumDifference_ReturnsNull_ForOneElement()
        {
            var result = Pairing.GetPairWithIncreasingOrderMaximumDifference(new[] {1}, Subtract);

            Assert.IsNull(result);
        }

        [Test]
        public void GetPairWithIncreasingOrderMaximumDifference_ReturnsNull_ForEqualElements()
        {
            var result = Pairing.GetPairWithIncreasingOrderMaximumDifference(new[] {1, 1, 1, 1}, Subtract);

            Assert.IsNull(result);
        }

        [Test]
        public void GetPairWithIncreasingOrderMaximumDifference_ReturnsThePair_ForThreeElements()
        {
            var result = Pairing.GetPairWithIncreasingOrderMaximumDifference(new[] {1, 2, 3}, Subtract);

            Assert.AreEqual((1, 3), result);
        }

        [Test]
        public void GetPairWithIncreasingOrderMaximumDifference_ReturnsThePair_ForTwoIncreasingElements()
        {
            var result = Pairing.GetPairWithIncreasingOrderMaximumDifference(new[] {1, 2}, Subtract);

            Assert.AreEqual((1, 2), result);
        }

        [Test]
        public void GetPairWithIncreasingOrderMaximumDifference_ReturnsNull_ForTwoDecreasingElements()
        {
            var result = Pairing.GetPairWithIncreasingOrderMaximumDifference(new[] {2, 1}, Subtract);

            Assert.IsNull(result);
        }

        [Test]
        public void GetPairWithIncreasingOrderMaximumDifference_TakesFuncIntoAccount()
        {
            var result = Pairing.GetPairWithIncreasingOrderMaximumDifference(new[] {0, -10, 10, -11}, (l, r) => Math.Abs(l - r));

            Assert.AreEqual((0, -11), result);
        }

        [Test, SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void GetPairWithIncreasingOrderMaximumDifference_ThrowsException_ForNullFunc()
        {
            Assert.Throws<ArgumentNullException>(
                () => Pairing.GetPairWithIncreasingOrderMaximumDifference(new int[] { }, null));
        }

        [Test, SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void GetPairWithIncreasingOrderMaximumDifference_ThrowsException_ForNullSequence()
        {
            Assert.Throws<ArgumentNullException>(
                () => Pairing.GetPairWithIncreasingOrderMaximumDifference((int[]) null, Subtract));
        }

        [Test]
        public void GetPairWithIncreasingOrderMaximumDifference_WorksForNegative()
        {
            var result = Pairing.GetPairWithIncreasingOrderMaximumDifference(new[] {0, -100, 100}, Subtract);

            Assert.AreEqual((0, 100), result);
        }

        [Test]
        public void GetPairWithIncreasingOrderMaximumDifference_WorksForPerfectSequenceOfThree()
        {
            var result = Pairing.GetPairWithIncreasingOrderMaximumDifference(new[] {0, 1, 2}, Subtract);

            Assert.AreEqual((0, 2), result);
        }

        [Test]
        public void GetPairWithIncreasingOrderMaximumDifference_WorksInterruptedSequence_1()
        {
            var result = Pairing.GetPairWithIncreasingOrderMaximumDifference(new[] {10, 19, 20, 0, 11}, Subtract);

            Assert.AreEqual((0, 11), result);
        }

        [Test]
        public void GetPairWithIncreasingOrderMaximumDifference_WorksInterruptedSequence_2()
        {
            var result = Pairing.GetPairWithIncreasingOrderMaximumDifference(new[] {10, 20, 9, 19}, Subtract);

            Assert.AreEqual((10, 20), result);
        }

        [Test]
        public void GetPairWithIncreasingOrderMaximumDifference_WorksInterruptedSequence_3()
        {
            var result = Pairing.GetPairWithIncreasingOrderMaximumDifference(new[] {10, 20, 8, 19, 7, 19, 6}, Subtract);

            Assert.AreEqual((7, 19), result);
        }
    }
}