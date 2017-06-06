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
    using System;
    using System.Collections.Generic;

    [TestFixture]
    public class CombinatoricsFindSubsetPairingWithLowestCostTests
    {
        private static double BanalCostOfPairsEvaluator(int l, int r)
        {
            return l + r;
        }

        private static double DistanceCostOfPairsEvaluator(int l, int r)
        {
            return Math.Abs(l - r);
        }

        [Test]
        public void FindSubsetPairingWithLowestCost_ThrowsException_ForNullSequence()
        {
            Assert.Throws<ArgumentNullException>(
                () => ((int[])null).FindSubsetPairingWithLowestCost(BanalCostOfPairsEvaluator));
        }

        [Test]
        public void FindSubsetPairingWithLowestCost_ThrowsException_ForSequenceWithOddNumberOfElements()
        {
            Assert.Throws<ArgumentException>(
                () => new[] {1, 2, 3}.FindSubsetPairingWithLowestCost(BanalCostOfPairsEvaluator));
        }

        [Test]
        public void FindSubsetPairingWithLowestCost_ThrowsException_ForNullEvaluateCostOfPairFunc()
        {
            Assert.Throws<ArgumentNullException>(
                () => new[] {1, 2}.FindSubsetPairingWithLowestCost(null));
        }

        [Test]
        public void FindSubsetPairingWithLowestCost_ReturnsEmptyArray_ForEmptySequence()
        {
            var result = new int[] { }.FindSubsetPairingWithLowestCost(BanalCostOfPairsEvaluator);
            TestHelper.AssertSequence(result);
        }

        [Test]
        public void FindSubsetPairingWithLowestCost_ReturnsOnePair_ForTwoElementSequence()
        {
            var result = new[] { 1, 2 }.FindSubsetPairingWithLowestCost(BanalCostOfPairsEvaluator);
            
            TestHelper.AssertSequence(result, Tuple.Create(1, 2));
        }

        [Test]
        public void FindSubsetPairingWithLowestCost_ReturnsTwoPairs_ForFourElementSequence()
        {
            var result = new[] { 4, 10, 2, 8 }.FindSubsetPairingWithLowestCost(BanalCostOfPairsEvaluator);

            TestHelper.AssertSequence(result, 
                Tuple.Create(4, 10), 
                Tuple.Create(2, 8));
        }


        [Test]
        public void FindSubsetPairingWithLowestCost_CreatesSets_UsingTheActualCost()
        {
            var result = new[] { 1, 2, 3, 8, 9, 12, 4, 6 }.FindSubsetPairingWithLowestCost(DistanceCostOfPairsEvaluator);

            TestHelper.AssertSequence(result,
                Tuple.Create(1, 2),
                Tuple.Create(3, 4),
                Tuple.Create(8, 6),
                Tuple.Create(9, 12));
        }

        [TestCase(10)]
        [TestCase(20)]
        public void FindSubsetPairingWithLowestCost_OperatesAsExpected_AtLargeInputs(int length)
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

            var result = sequence.FindSubsetPairingWithLowestCost(DistanceCostOfPairsEvaluator);
            foreach (var r in result)
            {
                var x = new[] {r.Item1, r.Item2};
                foreach (var item in x)
                {
                    Assert.IsTrue(expected.TryGetValue(item, out int appearances));
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
    }
}
