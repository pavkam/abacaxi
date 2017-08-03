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

namespace Abacaxi.Tests.Set
{
    using System;
    using NUnit.Framework;
    using System.Diagnostics.CodeAnalysis;

    [TestFixture]
    public class EnumerateSubsetCombinationsTests
    {
        [Test]
        [SuppressMessage("ReSharper", "IteratorMethodResultIsIgnored")]
        public void EnumerateSubsetCombinations_ThrowsException_ForNullSequence()
        {
            Assert.Throws<ArgumentNullException>(() =>
                ((int[]) null).EnumerateSubsetCombinations(1));
        }

        [Test]
        [SuppressMessage("ReSharper", "IteratorMethodResultIsIgnored")]
        public void EnumerateSubsetCombinations_ThrowsException_ForSubsetsLessThanOne()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                new int[] {}.EnumerateSubsetCombinations(0));
        }

        [Test]
        public void EnumerateSubsetCombinations_ReturnsNothing_ForEmptySequence()
        {
            TestHelper.AssertSequence(
                new int[] { }.EnumerateSubsetCombinations(1)
                );
        }

        [Test]
        public void EnumerateSubsetCombinations_ReturnsOneCombination_ForOneElementAndOneSubset()
        {
            TestHelper.AssertSequence(
                new[] { 10 }.EnumerateSubsetCombinations(1),
                new[] { new[] { 10 } });
        }

        [Test]
        public void EnumerateSubsetCombinations_ReturnsTwoCombinations_ForOneElementAndTwoSubsets()
        {
            TestHelper.AssertSequence(
                new[] { 10 }.EnumerateSubsetCombinations(2),
                new[] { new[] { 10 }, new int[] {} },
                new[] { new int[] { }, new[] { 10 } });
        }

        [Test]
        public void EnumerateSubsetCombinations_ReturnsFourCombinations_ForTwoElementAndTwoSubsets()
        {
            TestHelper.AssertSequence(
                new[] {11, 19}.EnumerateSubsetCombinations(2),
                new[]
                {
                    new[] {11, 19},
                    new int[] {}
                },
                new[]
                {
                    new[] {11},
                    new[] {19}
                },
                new[]
                {
                    new[] {19},
                    new[] {11}
                },
                new[]
                {
                    new int[] {},
                    new[] {11, 19}
                });
        }

        [Test]
        public void EnumerateSubsetCombinations_DoesNotCareIfDuplicates()
        {
            TestHelper.AssertSequence(
                new[] { 1, 1 }.EnumerateSubsetCombinations(2),
                new[]
                {
                    new[] {1, 1},
                    new int[] {}
                },
                new[]
                {
                    new[] {1},
                    new[] {1}
                },
                new[]
                {
                    new[] {1},
                    new[] {1}
                },
                new[]
                {
                    new int[] {},
                    new[] {1, 1}
                });
        }
    }
}
