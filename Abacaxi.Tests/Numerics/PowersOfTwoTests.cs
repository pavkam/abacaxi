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

namespace Abacaxi.Tests.Numerics
{
    using Abacaxi.Numerics;
    using NUnit.Framework;
    using System.Collections.Generic;
    using System.Linq;

    [TestFixture]
    public class PowersOfTwoTests
    {
        private void AssertSequence(IEnumerable<int> sequence, params int[] expected)
        {
            Assert.NotNull(sequence);

            var array = sequence.ToArray();
            Assert.AreEqual(expected.Length, array.Length);

            for (var i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(expected[i], array[i]);
            }
        }

        [Test]
        public void DecomposeZero_ReturnsNothing()
        {
            AssertSequence(
                PowersOfTwo.Decompose(0));
        }

        [Test]
        public void DecomposeOne_ReturnsOne()
        {
            AssertSequence(
                PowersOfTwo.Decompose(1),
                1);
        }

        [Test]
        public void DecomposeTwo_ReturnsTwo()
        {
            AssertSequence(
                PowersOfTwo.Decompose(2),
                2);
        }

        [Test]
        public void DecomposeThree_ReturnsOneThenTwo()
        {
            AssertSequence(
                PowersOfTwo.Decompose(3),
                1, 2);
        }

        [Test]
        public void DecomposeFour_ReturnsFour()
        {
            AssertSequence(
                PowersOfTwo.Decompose(4),
                4);
        }


        [Test]
        public void DecomposeMinusOne_ReturnsMinusOne()
        {
            AssertSequence(
                PowersOfTwo.Decompose(-1),
                -1);
        }

        [Test]
        public void DecomposeMinusTwo_ReturnsMinusTwo()
        {
            AssertSequence(
                PowersOfTwo.Decompose(-2),
                -2);
        }

        [Test]
        public void DecomposeMinusThree_ReturnsMinusOneThenMinusTwo()
        {
            AssertSequence(
                PowersOfTwo.Decompose(-3),
                -1, -2);
        }

        [Test]
        public void DecomposeFour_ReturnsMinusFour()
        {
            AssertSequence(
                PowersOfTwo.Decompose(-4),
                -4);
        }

        [TestCase(int.MaxValue)]
        [TestCase(int.MinValue)]
        public void Decompose_SumsToOriginal(int number)
        {
            var backSum = PowersOfTwo.Decompose(number).Sum();

            Assert.AreEqual(number, backSum);
        }
    }
}
