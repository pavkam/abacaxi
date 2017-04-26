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

namespace Abacaxi.Tests.Combinatorics
{
    using NUnit.Framework;
    using System.Linq;

    [TestFixture]
    public class CombinatoricsPartitionIntegerTests
    {
        [Test]
        public void DeconstructZero_ReturnsNothing()
        {
            TestHelper.AssertSequence(
                Abacaxi.Combinatorics.PartitionInteger(0)
                );
        }

        [Test]
        public void DeconstructOne_ReturnsOne()
        {
            TestHelper.AssertSequence(
                Abacaxi.Combinatorics.PartitionInteger(1),
                new[] { 1 });
        }

        [Test]
        public void DeconstructTwo_ReturnsTwo_ThenOneOne()
        {
            TestHelper.AssertSequence(
                Abacaxi.Combinatorics.PartitionInteger(2),
                new[] { 2 },
                new[] { 1, 1 });
        }

        [Test]
        public void DeconstructThree_ReturnsThree_ThenTwoOne_ThenOneOneOne()
        {
            TestHelper.AssertSequence(
                Abacaxi.Combinatorics.PartitionInteger(3),
                new[] { 3 },
                new[] { 2, 1 },
                new[] { 1, 1, 1 });
        }

        [Test]
        public void DeconstructFour_ReturnsFour_ThenThreeOne_ThenTwoTwo_ThenTwoOneOne_ThenOneOneOneOne()
        {
            TestHelper.AssertSequence(
                Abacaxi.Combinatorics.PartitionInteger(4),
                new[] { 4 },
                new[] { 3, 1 },
                new[] { 2, 1, 1 },
                new[] { 1, 1, 1, 1 },
                new[] { 2, 2 });
        }

        [Test]
        public void DeconstructMinusOne_ReturnsMinusOne()
        {
            TestHelper.AssertSequence(
                Abacaxi.Combinatorics.PartitionInteger(-1),
                new[] { -1 });
        }

        [Test]
        public void DeconstructMinusTwo_ReturnsMinusTwo_ThenMinusOneMinusOne()
        {
            TestHelper.AssertSequence(
                Abacaxi.Combinatorics.PartitionInteger(-2),
                new[] { -2 },
                new[] { -1, -1 });
        }

        [Test]
        public void DeconstructMinusThree_ReturnsMinusThree_ThenMinusTwoMinusOne_ThenMinusOneMinusOneMinusOne()
        {
            TestHelper.AssertSequence(
                Abacaxi.Combinatorics.PartitionInteger(-3),
                new[] { -3 },
                new[] { -2, -1 },
                new[] { -1, -1, -1 });
        }

        [Test]
        public void DeconstructMinusFour_ReturnsMinusFour_ThenMinusThreeMinusOne_ThenMinusTwoMinusTwo_ThenMinusTwoMinusOneMinusOne_ThenMinusOneMinusOneMinusOneMinusOne()
        {
            TestHelper.AssertSequence(
                Abacaxi.Combinatorics.PartitionInteger(-4),
                new[] { -4 },
                new[] { -3, -1 },
                new[] { -2, -1, -1 },
                new[] { -1, -1, -1, -1 },
                new[] { -2, -2 });
        }

        [TestCase(20)]
        [TestCase(-20)]
        public void Deconstruct_SumsToOriginal(int number)
        {
            foreach (var combo in Abacaxi.Combinatorics.PartitionInteger(number))
            {
                var sum = combo.Sum();
                Assert.AreEqual(number, sum);
            }
        }
    }
}
