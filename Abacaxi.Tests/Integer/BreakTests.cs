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

namespace Abacaxi.Tests.Integer
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using NUnit.Framework;
    using Integer = Abacaxi.Integer;

    [TestFixture]
    public sealed class BreakTests
    {
        [Test]
        public void Break_ReturnsNothingIfComponentCannotBeBroken()
        {
            var result = Integer.Break(9, 5);

            TestHelper.AssertSequence(result);
        }

        [Test]
        public void Break_ReturnsNothingIfComponentsIsEmpty()
        {
            var result = Integer.Break(1);

            TestHelper.AssertSequence(result);
        }

        [Test]
        public void Break_ReturnsNothingIfNumberIsZero()
        {
            var result = Integer.Break(0, 1);

            TestHelper.AssertSequence(result);
        }

        [Test]
        public void Break_ReturnsProperResult_ForSimpleCase_1()
        {
            var result = Integer.Break(1, 1);

            TestHelper.AssertSequence(result, (1, 1));
        }

        [Test]
        public void Break_ReturnsProperResult_ForSimpleCase_2()
        {
            var result = Integer.Break(1, 1, 2);

            TestHelper.AssertSequence(result, (1, 1));
        }

        [Test]
        public void Break_ReturnsProperResult_ForSimpleCase_3()
        {
            var result = Integer.Break(2, 1);

            TestHelper.AssertSequence(result, (1, 2));
        }

        [Test]
        public void Break_ReturnsProperResult_ForSimpleCase_4()
        {
            var result = Integer.Break(2, 2, 1);

            TestHelper.AssertSequence(result, (2, 1));
        }

        [Test]
        public void Break_ReturnsProperResult_ForSimpleCase_5()
        {
            var result = Integer.Break(10, 4, 1);

            TestHelper.AssertSequence(result, (4, 2), (1, 2));
        }

        [Test]
        public void Break_ReturnsProperResult_ForSimpleCase_6()
        {
            var result = Integer.Break(20, 7, 8, 6);

            TestHelper.AssertSequence(result, (7, 2), (6, 1));
        }

        [Test]
        public void Break_ReturnsProperResult_ForSimpleCase_7()
        {
            var result = Integer.Break(1000, 7, 5, 3, 2, 1);

            TestHelper.AssertSequence(result, (7, 142), (5, 1), (1, 1));
        }

        [Test, SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void Break_ThrowsException_IfComponentArrayIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => Integer.Break(1, null));
        }

        [Test]
        public void Break_ThrowsException_IfNumberIsNegative()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Integer.Break(-1, 1));
        }

        [Test]
        public void Break_ThrowsException_IfOneOfComponentsIsLessThanOne()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Integer.Break(10, 0));
        }
    }
}