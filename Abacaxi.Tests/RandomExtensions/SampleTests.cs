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

namespace Abacaxi.Tests.RandomExtensions
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using NUnit.Framework;

    [TestFixture]
    public sealed class SampleTests
    {
        private readonly Random _random = new Random();

        [TestCase(1), TestCase(2)]
        public void Sample_ReturnsSingleElement_ForOneLengthSequence(int expectedLength)
        {
            var result = _random.Sample(new[] { 1 }, expectedLength);

            TestHelper.AssertSequence(result, 1);
        }

        [Test]
        public void Sample_ReturnsARandomSample()
        {
            var input = new[] { 1, 2, 3, 4, 5, 6 }.ToSet();
            var result = _random.Sample(input, 3).ToSet();

            Assert.IsTrue(input.IsProperSupersetOf(result));
        }

        [Test]
        public void Sample_ReturnsNothing_ForAnEmptyInputSequence()
        {
            var result = _random.Sample(new int[] { }, 1);

            TestHelper.AssertSequence(result);
        }

        [Test, SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void Sample_ThrowsException_IfRandomIsNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
                ((Random) null).Sample(new[] { 1 }, 1));
        }

        [Test]
        public void Sample_ThrowsException_IfSampleLengthIsLessThanOne()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                _random.Sample(new[] { 1 }, 0));
        }

        [Test, SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void Sample_ThrowsException_IfSequenceIsNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
                _random.Sample<int>(null, 1));
        }
    }
}