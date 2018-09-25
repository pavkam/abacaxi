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
    public sealed class GetWithMinimumCost
    {
        private static double BanalCostOfPairsEvaluator(int l, int r) => l + r;

        private static double DistanceCostOfPairsEvaluator(int l, int r) => Math.Abs(l - r);

        [TestCase(10), TestCase(20)]
        public void GetWithMinimumCost_OperatesAsExpected_AtLargeInputs(int length)
        {
            var random = new Random();
            var sequence = new List<int>();
            var expected = new Dictionary<int, int>();
            for (var i = 0; i < length; i++)
            {
                var item = random.Next(length);
                sequence.Add(item);
                expected.AddOrUpdate(item, 1, e => e + 1);
            }

            var result = Pairing.GetWithMinimumCost(sequence, DistanceCostOfPairsEvaluator);
            foreach (var r in result)
            {
                var x = new[] {r.Item1, r.Item2};
                foreach (var item in x)
                {
                    Assert.IsTrue(expected.TryGetValue(item, out var appearances));
                    if (appearances == 1)
                    {
                        expected.Remove(item);
                    }
                    else
                    {
                        expected[item] = appearances - 1;
                    }
                }
            }

            Assert.AreEqual(0, expected.Count);
        }


        [Test]
        public void GetWithMinimumCost_CreatesSets_UsingTheActualCost()
        {
            var result = Pairing.GetWithMinimumCost(new[] {1, 2, 3, 8, 9, 12, 4, 6}, DistanceCostOfPairsEvaluator);

            TestHelper.AssertSequence(result,
                Tuple.Create(1, 2),
                Tuple.Create(3, 4),
                Tuple.Create(8, 6),
                Tuple.Create(9, 12));
        }

        [Test]
        public void GetWithMinimumCost_ReturnsEmptyArray_ForEmptySequence()
        {
            var result = Pairing.GetWithMinimumCost(new int[] { }, BanalCostOfPairsEvaluator);
            TestHelper.AssertSequence(result);
        }

        [Test]
        public void GetWithMinimumCost_ReturnsOnePair_ForTwoElementSequence()
        {
            var result = Pairing.GetWithMinimumCost(new[] {1, 2}, BanalCostOfPairsEvaluator);

            TestHelper.AssertSequence(result, Tuple.Create(1, 2));
        }

        [Test]
        public void GetWithMinimumCost_ReturnsTwoPairs_ForFourElementSequence()
        {
            var result = Pairing.GetWithMinimumCost(new[] {4, 10, 2, 8}, BanalCostOfPairsEvaluator);

            TestHelper.AssertSequence(result,
                Tuple.Create(4, 10),
                Tuple.Create(2, 8));
        }

        [Test, SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void GetWithMinimumCost_ThrowsException_ForNullEvaluateCostOfPairFunc()
        {
            Assert.Throws<ArgumentNullException>(
                () => Pairing.GetWithMinimumCost(new[] {1, 2}, null));
        }

        [Test, SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void GetWithMinimumCost_ThrowsException_ForNullSequence()
        {
            Assert.Throws<ArgumentNullException>(
                () => Pairing.GetWithMinimumCost<int>(null, BanalCostOfPairsEvaluator));
        }

        [Test]
        public void GetWithMinimumCost_ThrowsException_ForSequenceWithOddNumberOfElements()
        {
            Assert.Throws<ArgumentException>(
                () => Pairing.GetWithMinimumCost(new[] {1, 2, 3}, BanalCostOfPairsEvaluator));
        }
    }
}