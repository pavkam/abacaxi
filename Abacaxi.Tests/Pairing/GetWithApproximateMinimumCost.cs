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
    using System.Linq;
    using NUnit.Framework;
    using Pairing = Abacaxi.Pairing;

    [TestFixture]
    public sealed class GetWithApproximateMinimumCost
    {
        private static double DistanceCostOfPairsEvaluator(int l, int r) => Math.Abs(l - r);

        [TestCase(10), TestCase(20), TestCase(100), TestCase(1000), Parallelizable]
        public void GetWithApproximateMinimumCost_OperatesAsExpected_AtLargeInputs(int length)
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

            var result = Pairing.GetWithApproximateMinimumCost(sequence, DistanceCostOfPairsEvaluator);
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

        [TestCase(4, 100, 10, 0.01), TestCase(32, 100, 1000, 0.025), TestCase(128, 100, 7000, 0.015),
         TestCase(1024, 50, 90000, 0.01), TestCase(4096, 25, 400000, 0.01), TestCase(4, 100, 10, 0.10),
         TestCase(32, 100, 350, 0.11), TestCase(128, 100, 1500, 0.10), TestCase(1024, 50, 9000, 0.10),
         TestCase(4096, 25, 35000, 0.10), Parallelizable]
        public void GetWithApproximateMinimumCost_ApproximatesAtExpectedError(int length, int samples, int iterations,
            double expectedMaxError)
        {
            var totalError = .0;
            for (var it = 0; it < samples; it++)
            {
                var random = new Random(Environment.TickCount);
                var sequence = new List<int>();
                for (var i = 0; i < length; i++)
                {
                    var item = random.Next(length);
                    sequence.Add(item);
                }

                var approxResult =
                    Pairing.GetWithApproximateMinimumCost(sequence, DistanceCostOfPairsEvaluator, iterations);
                var approxCost = approxResult.Sum(s => DistanceCostOfPairsEvaluator(s.Item1, s.Item2));

                sequence.Sort();
                var minCost = .0;
                var maxCost = .0;
                for (var i = 0; i < sequence.Count; i += 2)
                {
                    minCost += DistanceCostOfPairsEvaluator(sequence[i], sequence[i + 1]);
                    maxCost += DistanceCostOfPairsEvaluator(sequence[i], sequence[sequence.Count - i - 1]);
                }

                var error = 1 - (maxCost - approxCost) / (maxCost - minCost);
                if (double.IsNaN(error))
                {
                    error = 0;
                }

                totalError += error;
            }

            totalError /= samples;

            Assert.IsTrue(totalError <= expectedMaxError,
                $"Total error {totalError * 100:N}% should be less than or equal to {expectedMaxError * 100:N}%");
        }

        [Test]
        public void GetWithApproximateMinimumCost_ReturnsEmptyArray_ForEmptySequence()
        {
            var result = Pairing.GetWithApproximateMinimumCost(new int[] { }, DistanceCostOfPairsEvaluator);
            TestHelper.AssertSequence(result);
        }

        [Test]
        public void GetWithApproximateMinimumCost_ReturnsOnePair_ForTwoElementSequence()
        {
            var result = Pairing.GetWithApproximateMinimumCost(new[] {1, 2}, DistanceCostOfPairsEvaluator);

            Assert.AreEqual(1, result.Length);
            Assert.IsTrue(result[0].Equals(Tuple.Create(1, 2)) || result[0].Equals(Tuple.Create(2, 1)));
        }

        [Test, SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void GetWithApproximateMinimumCost_ThrowsException_ForNullEvaluateCostOfPairFunc()
        {
            Assert.Throws<ArgumentNullException>(
                () => Pairing.GetWithApproximateMinimumCost(new[] {1, 2}, null));
        }

        [Test, SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void GetWithApproximateMinimumCost_ThrowsException_ForNullSequence()
        {
            Assert.Throws<ArgumentNullException>(
                () => Pairing.GetWithApproximateMinimumCost<int>(null, DistanceCostOfPairsEvaluator));
        }

        [Test]
        public void GetWithApproximateMinimumCost_ThrowsException_ForSequenceWithOddNumberOfElements()
        {
            Assert.Throws<ArgumentException>(
                () => Pairing.GetWithApproximateMinimumCost(new[] {1, 2, 3}, DistanceCostOfPairsEvaluator));
        }

        [Test]
        public void GetWithApproximateMinimumCost_ThrowsException_ForZeroOrLessIterationCount()
        {
            Assert.Throws<ArgumentOutOfRangeException>(
                () => Pairing.GetWithApproximateMinimumCost(new[] {1, 2}, DistanceCostOfPairsEvaluator, 0));
        }
    }
}