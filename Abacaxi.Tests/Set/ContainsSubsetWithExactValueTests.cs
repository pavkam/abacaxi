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
    using NUnit.Framework;
    using Set = Abacaxi.Set;

    [TestFixture]
    public class ContainsSubsetWithExactValueTests
    {
        [TestCase(1), TestCase(2), TestCase(3), TestCase(4), TestCase(5), TestCase(6), TestCase(7), TestCase(8),
         TestCase(9), TestCase(10), TestCase(11), TestCase(12), TestCase(13), TestCase(14), TestCase(15)]
        public void ContainsSubsetWithExactValue_ReturnsTrue_IfSumFound(int target)
        {
            var result = Set.ContainsSubsetWithExactValue(new[] { 1, 2, 3, 4, 5 }, target);
            Assert.IsTrue(result);
        }

        [TestCase(2), TestCase(14), TestCase(17)]
        public void ContainsSubsetWithExactValue_ReturnsFalse_IfSumNotFound(int target)
        {
            var result = Set.ContainsSubsetWithExactValue(new[] { 1, 3, 5, 7 }, target);
            Assert.IsFalse(result);
        }

        [Test]
        public void ContainsSubsetWithExactValue_ReturnsFalse_ForEmptyArray()
        {
            var result = Set.ContainsSubsetWithExactValue(new int[] { }, 1);
            Assert.IsFalse(result);
        }

        [Test]
        public void ContainsSubsetWithExactValue_ReturnsFalse_WhenSumCannotBeCompleted()
        {
            var result = Set.ContainsSubsetWithExactValue(new[] { 2 }, 1);
            Assert.IsFalse(result);
        }

        [Test]
        public void ContainsSubsetWithExactValue_ThrowsException_ForNegativeNumberInSequence()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                Set.ContainsSubsetWithExactValue(new[] { -1 }, 1));
        }

        [Test, SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void ContainsSubsetWithExactValue_ThrowsException_ForNullSequence()
        {
            Assert.Throws<ArgumentNullException>(() =>
                Set.ContainsSubsetWithExactValue(null, 1));
        }

        [Test]
        public void ContainsSubsetWithExactValue_ThrowsException_ForZeroTargetSum()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                Set.ContainsSubsetWithExactValue(new int[] { }, 0));
        }
    }
}