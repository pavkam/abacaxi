/* Copyright 2017-2019 by Alexandru Ciobanu (alex+git@ciobanu.org)
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
    using System.Linq;
    using NUnit.Framework;
    using Pairing = Abacaxi.Pairing;

    [TestFixture]
    public sealed class GetEqualizationPairsTests
    {
        [Test]
        public void GetEqualizationPairs_ReturnsAll_ForReversedSame()
        {
            var s = new[] {1, 2, 3};
            var result = Pairing.GetEqualizationPairs(s, s.Reverse().ToArray());

            TestHelper.AssertSequence(result, (3, 3), (2, 2), (1, 1));
        }

        [Test]
        public void GetEqualizationPairs_ReturnsAll_ForSameSequence_1()
        {
            var s = new[] {1};
            var result = Pairing.GetEqualizationPairs(s, s);

            TestHelper.AssertSequence(result, (1, 1));
        }

        [Test]
        public void GetEqualizationPairs_ReturnsAll_ForSameSequence_2()
        {
            var s = new[] {1, 2};
            var result = Pairing.GetEqualizationPairs(s, s);

            TestHelper.AssertSequence(result, (1, 1), (2, 2));
        }

        [Test]
        public void GetEqualizationPairs_ReturnsAll_ForSameSequence_3()
        {
            var s = new[] {1, 2, 3};
            var result = Pairing.GetEqualizationPairs(s, s);

            TestHelper.AssertSequence(result, (1, 1), (2, 2), (3, 3));
        }

        [Test]
        public void GetEqualizationPairs_ReturnsNothing_IfCannotBeEqualized()
        {
            var result = Pairing.GetEqualizationPairs(new[] {7, 3}, new[] {5, 6});

            TestHelper.AssertSequence(result);
        }

        [Test]
        public void GetEqualizationPairs_ReturnsNothing_IfSequence1IsEmpty()
        {
            var result = Pairing.GetEqualizationPairs(new int[] { }, new[] {1, 2, 3});
            TestHelper.AssertSequence(result);
        }

        [Test]
        public void GetEqualizationPairs_ReturnsNothing_IfSequence2IsEmpty()
        {
            var result = Pairing.GetEqualizationPairs(new[] {1, 2, 3}, new int[] { });
            TestHelper.AssertSequence(result);
        }

        [Test]
        public void GetEqualizationPairs_ReturnsOnePair_IfCanBeEqualized()
        {
            var result = Pairing.GetEqualizationPairs(new[] {8, 5, 5, 0, 1, -18, 2, 4, 3}, new[] {6, 6});

            TestHelper.AssertSequence(result, (5, 6));
        }

        [Test, SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void GetEqualizationPairs_ThrowsException_ForNullSequence1()
        {
            Assert.Throws<ArgumentNullException>(
                () => Pairing.GetEqualizationPairs(null, new int[] { }));
        }

        [Test, SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void GetEqualizationPairs_ThrowsException_ForNullSequence2()
        {
            Assert.Throws<ArgumentNullException>(
                () => Pairing.GetEqualizationPairs(new int[] { }, null));
        }
    }
}