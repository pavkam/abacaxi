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
    public class CombinatoricsGetIntegerPartitionsTests
    {
        [Test]
        public void PartitionInteger_Zero_ReturnsNothing()
        {
            TestHelper.AssertSequence(
                0.GetIntegerPartitions()
                );
        }

        [Test]
        public void PartitionInteger_One_ReturnsOne()
        {
            TestHelper.AssertSequence(
                1.GetIntegerPartitions(),
                new[] { 1 });
        }

        [Test]
        public void PartitionInteger_Two_ReturnsTwo_ThenOneOne()
        {
            TestHelper.AssertSequence(
                2.GetIntegerPartitions(),
                new[] { 2 },
                new[] { 1, 1 });
        }

        [Test]
        public void PartitionInteger_Three_ReturnsThree_ThenTwoOne_ThenOneOneOne()
        {
            TestHelper.AssertSequence(
                3.GetIntegerPartitions(),
                new[] { 3 },
                new[] { 2, 1 },
                new[] { 1, 1, 1 });
        }

        [Test]
        public void PartitionInteger_Four_ReturnsFour_ThenThreeOne_ThenTwoTwo_ThenTwoOneOne_ThenOneOneOneOne()
        {
            TestHelper.AssertSequence(
                4.GetIntegerPartitions(),
                new[] { 4 },
                new[] { 3, 1 },
                new[] { 2, 1, 1 },
                new[] { 1, 1, 1, 1 },
                new[] { 2, 2 });
        }

        [Test]
        public void PartitionInteger_MinusOne_ReturnsMinusOne()
        {
            TestHelper.AssertSequence(
                (-1).GetIntegerPartitions(),
                new[] { -1 });
        }

        [Test]
        public void PartitionInteger_MinusTwo_ReturnsMinusTwo_ThenMinusOneMinusOne()
        {
            TestHelper.AssertSequence(
                (-2).GetIntegerPartitions(),
                new[] { -2 },
                new[] { -1, -1 });
        }

        [Test]
        public void PartitionInteger_MinusThree_ReturnsMinusThree_ThenMinusTwoMinusOne_ThenMinusOneMinusOneMinusOne()
        {
            TestHelper.AssertSequence(
                (-3).GetIntegerPartitions(),
                new[] { -3 },
                new[] { -2, -1 },
                new[] { -1, -1, -1 });
        }

        [Test]
        public void PartitionInteger_MinusFour_ReturnsMinusFour_ThenMinusThreeMinusOne_ThenMinusTwoMinusTwo_ThenMinusTwoMinusOneMinusOne_ThenMinusOneMinusOneMinusOneMinusOne()
        {
            TestHelper.AssertSequence(
                (-4).GetIntegerPartitions(),
                new[] { -4 },
                new[] { -3, -1 },
                new[] { -2, -1, -1 },
                new[] { -1, -1, -1, -1 },
                new[] { -2, -2 });
        }

        [TestCase(20)]
        [TestCase(-20)]
        public void PartitionInteger_SumsToOriginal(int number)
        {
            foreach (var combo in number.GetIntegerPartitions())
            {
                var sum = combo.Sum();
                Assert.AreEqual(number, sum);
            }
        }
    }
}
