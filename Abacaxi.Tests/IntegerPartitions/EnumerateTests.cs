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

namespace Abacaxi.Tests.IntegerPartitions
{
    using System.Linq;
    using NUnit.Framework;
    using IntegerPartitions = Abacaxi.IntegerPartitions;

    [TestFixture]
    public class EnumerateTests
    {
        [TestCase(20), TestCase(-20)]
        public void Enumerate_SumsToOriginal(int number)
        {
            foreach (var combo in IntegerPartitions.Enumerate(number))
            {
                var sum = combo.Sum();
                Assert.AreEqual(number, sum);
            }
        }

        [Test]
        public void Enumerate_Four_ReturnsFour_ThenThreeOne_ThenTwoTwo_ThenTwoOneOne_ThenOneOneOneOne()
        {
            TestHelper.AssertSequence(
                IntegerPartitions.Enumerate(4),
                new[] {4},
                new[] {3, 1},
                new[] {2, 1, 1},
                new[] {1, 1, 1, 1},
                new[] {2, 2});
        }

        [Test]
        public void
            Enumerate_MinusFour_ReturnsMinusFour_ThenMinusThreeMinusOne_ThenMinusTwoMinusTwo_ThenMinusTwoMinusOneMinusOne_ThenMinusOneMinusOneMinusOneMinusOne()
        {
            TestHelper.AssertSequence(
                IntegerPartitions.Enumerate(-4),
                new[] {-4},
                new[] {-3, -1},
                new[] {-2, -1, -1},
                new[] {-1, -1, -1, -1},
                new[] {-2, -2});
        }

        [Test]
        public void Enumerate_MinusOne_ReturnsMinusOne()
        {
            TestHelper.AssertSequence(
                IntegerPartitions.Enumerate(-1),
                new[] {-1});
        }

        [Test]
        public void Enumerate_MinusThree_ReturnsMinusThree_ThenMinusTwoMinusOne_ThenMinusOneMinusOneMinusOne()
        {
            TestHelper.AssertSequence(
                IntegerPartitions.Enumerate(-3),
                new[] {-3},
                new[] {-2, -1},
                new[] {-1, -1, -1});
        }

        [Test]
        public void Enumerate_MinusTwo_ReturnsMinusTwo_ThenMinusOneMinusOne()
        {
            TestHelper.AssertSequence(
                IntegerPartitions.Enumerate(-2),
                new[] {-2},
                new[] {-1, -1});
        }

        [Test]
        public void Enumerate_One_ReturnsOne()
        {
            TestHelper.AssertSequence(
                IntegerPartitions.Enumerate(1),
                new[] {1});
        }

        [Test]
        public void Enumerate_Three_ReturnsThree_ThenTwoOne_ThenOneOneOne()
        {
            TestHelper.AssertSequence(
                IntegerPartitions.Enumerate(3),
                new[] {3},
                new[] {2, 1},
                new[] {1, 1, 1});
        }

        [Test]
        public void Enumerate_Two_ReturnsTwo_ThenOneOne()
        {
            TestHelper.AssertSequence(
                IntegerPartitions.Enumerate(2),
                new[] {2},
                new[] {1, 1});
        }

        [Test]
        public void Enumerate_Zero_ReturnsNothing()
        {
            TestHelper.AssertSequence(
                IntegerPartitions.Enumerate(0)
            );
        }
    }
}