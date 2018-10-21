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

namespace Abacaxi.Tests.Set
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using NUnit.Framework;
    using Set = Abacaxi.Set;

    [TestFixture]
    public class GetPermutationsTests
    {
        [Test, SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void GetPermutations_ThrowsException_ForNullSequence()
        {
            Assert.Throws<ArgumentNullException>(() =>
                Set.GetPermutations<int>(null));
        }

        [Test]
        public void GetPermutations_ReturnsEmptySequence_ForEmptySet()
        {
            var result = Set.GetPermutations(new int[0]);
            TestHelper.AssertSequence(result);
        }

        [Test]
        public void GetPermutations_ReturnsOneElementSequence_ForOneElementSet()
        {
            var result = Set.GetPermutations("a".AsList()).Select(s => s.ToString(string.Empty)).ToArray();

            TestHelper.AssertSequence(result, "a");
        }

        [Test]
        public void GetPermutations_ReturnsPermutationsOfTwo()
        {
            var result = Set.GetPermutations("ab".AsList()).Select(s => s.ToString(string.Empty)).ToArray();

            TestHelper.AssertSequence(result, "ba", "ab");
        }

        [Test]
        public void GetPermutations_ReturnsPermutationsOfThree()
        {
            var result = Set.GetPermutations("abc".AsList()).Select(s => s.ToString(string.Empty)).ToArray();

            TestHelper.AssertSequence(result, "cba", "bca", "bac", "cab", "acb", "abc");
        }
    }
}