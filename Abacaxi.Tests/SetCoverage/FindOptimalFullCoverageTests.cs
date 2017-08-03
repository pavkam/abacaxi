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

namespace Abacaxi.Tests.SetCoverage
{
    using System;
    using System.Collections.Generic;
    using NUnit.Framework;
    using System.Diagnostics.CodeAnalysis;

    [TestFixture]
    public class FindOptimalFullCoverageTests
    {
        [Test]
        [SuppressMessage("ReSharper", "IteratorMethodResultIsIgnored")]
        [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void FindOptimalFullCoverage_ThrowsException_ForNullSets()
        {
            Assert.Throws<ArgumentNullException>(() => 
                Abacaxi.SetCoverage.FindOptimalFullCoverage(null, EqualityComparer<int>.Default));
        }

        [Test]
        [SuppressMessage("ReSharper", "IteratorMethodResultIsIgnored")]
        [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void FindOptimalFullCoverage_ThrowsException_ForEqualityComparer()
        {
            Assert.Throws<ArgumentNullException>(() =>
                Abacaxi.SetCoverage.FindOptimalFullCoverage(new ISet<int>[] { }, null));
        }

        [Test]
        public void FindOptimalFullCoverage_ReturnsNothing_ForEmptySets()
        {
            var coverage = Abacaxi.SetCoverage.FindOptimalFullCoverage(new ISet<int>[] {}, EqualityComparer<int>.Default);

            TestHelper.AssertSequence(coverage);
        }

        [Test]
        public void FindOptimalFullCoverage_ReturnsSingleSet()
        {
            var set = new HashSet<int> {1, 2, 3, 4};
            var coverage = Abacaxi.SetCoverage.FindOptimalFullCoverage(new[] { set }, EqualityComparer<int>.Default);

            TestHelper.AssertSequence(
                coverage, set);
        }

        [Test]
        public void FindOptimalFullCoverage_ReturnsIndividualSets_IfNoIntersectionFound()
        {
            var set1 = new HashSet<int> {1};
            var set2 = new HashSet<int> {2};
            var set3 = new HashSet<int> {3};

            var coverage = Abacaxi.SetCoverage.FindOptimalFullCoverage(new[] { set1, set2, set3 }, EqualityComparer<int>.Default);

            TestHelper.AssertSequence(
                coverage, set1, set2, set3);
        }

        [Test]
        public void FindOptimalFullCoverage_ReturnsBestChoiceOnly()
        {
            var set1 = new HashSet<int> { 1, 2 };
            var set2 = new HashSet<int> { 2 };

            var coverage = Abacaxi.SetCoverage.FindOptimalFullCoverage(new[] { set1, set2 }, EqualityComparer<int>.Default);

            TestHelper.AssertSequence(
                coverage, set1);
        }

        [Test]
        public void FindOptimalFullCoverage_ReturnsGreatestOneFirst()
        {
            var set1 = new HashSet<int> { 1, 4 };
            var set2 = new HashSet<int> { 2, 3, 1 };

            var coverage = Abacaxi.SetCoverage.FindOptimalFullCoverage(new[] { set1, set2 }, EqualityComparer<int>.Default);

            TestHelper.AssertSequence(coverage, set2, set1);
        }

        [Test]
        public void FindOptimalFullCoverage_ReturnsTwoSets()
        {
            var set1 = new HashSet<int> { 1, 2 };
            var set2 = new HashSet<int> { 2, 3 };
            var set3 = new HashSet<int> { 1, 3 };

            var coverage = Abacaxi.SetCoverage.FindOptimalFullCoverage(new[] { set1, set2, set3 }, EqualityComparer<int>.Default);

            TestHelper.AssertSequence(coverage, set1, set2);
        }
    }
}
