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

namespace Abacaxi.Tests.SequenceExtensions
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using NUnit.Framework;

    [TestFixture]
    public sealed class IsValidAdjacencyTests
    {
        [Test]
        public void IsValidAdjacency_ReturnsFalse_IfNotAllElementsSatisfyRelation()
        {
            var result = new[] {3, 2, 2}.IsValidAdjacency((a, b) => a > b);

            Assert.IsFalse(result);
        }

        [Test]
        public void IsValidAdjacency_ReturnsFalse_IfTwoElementsSatisfyRelation()
        {
            var result = new[] {1, 1}.IsValidAdjacency((a, b) => a > b);

            Assert.IsFalse(result);
        }

        [Test]
        public void IsValidAdjacency_ReturnsTrue_ForEmptySequence()
        {
            var result = new int[] { }.IsValidAdjacency((a, b) => true);
            Assert.IsTrue(result);
        }

        [Test]
        public void IsValidAdjacency_ReturnsTrue_ForOneElement()
        {
            var result = new[] {1}.IsValidAdjacency((a, b) => true);
            Assert.IsTrue(result);
        }

        [Test]
        public void IsValidAdjacency_ReturnsTrue_IfAllElementsSatisfyRelation()
        {
            var result = new[] {3, 2, 1}.IsValidAdjacency((a, b) => a > b);

            Assert.IsTrue(result);
        }

        [Test]
        public void IsValidAdjacency_ReturnsTrue_IfTwoElementsSatisfyRelation()
        {
            var result = new[] {2, 1}.IsValidAdjacency((a, b) => a > b);

            Assert.IsTrue(result);
        }

        [Test, SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void IsValidAdjacency_ThrowsException_ForNullPredicate()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new[] {"bb", "ccc", "a", "z"}.IsValidAdjacency(null));
        }

        [Test, SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void IsValidAdjacency_ThrowsException_ForNullSequence()
        {
            Assert.Throws<ArgumentNullException>(() =>
                ((string[]) null).IsValidAdjacency((a, b) => true));
        }
    }
}