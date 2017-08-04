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

namespace Abacaxi.Tests.Containers
{
    using System;
    using Abacaxi.Containers;
    using NUnit.Framework;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;

    [TestFixture]
    public class TemporaryTests
    {
        [Test]
        [SuppressMessage("ReSharper", "ObjectCreationAsStatement")]
        [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void Ctor_ThrowsException_WhenValueFuncIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new Temporary<int>(null, 1));
        }

        [Test]
        [SuppressMessage("ReSharper", "ObjectCreationAsStatement")]
        public void Ctor_ThrowsException_WhenValueLifespanMillisIsLessThanOne()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Temporary<int>(() => 1, 0));
        }

        [Test]
        [SuppressMessage("ReSharper", "ObjectCreationAsStatement")]
        public void Temporary_DoesNotInitializeTheValueImmediately()
        {
            var called = false;
            new Temporary<int>(() =>
            {
                called = true;
                return -1;
            }, 1);

            Assert.IsFalse(called);
        }

        [Test]
        [SuppressMessage("ReSharper", "UnusedVariable")]
        public void Temporary_InitializesTheValueOnFirstCall()
        {
            var called = false;
            var temp = new Temporary<int>(() =>
            {
                called = true;
                return -1;
            }, 1);

            var x = temp.Value;

            Assert.IsTrue(called);
        }

        [Test]
        [SuppressMessage("ReSharper", "UnusedVariable")]
        [SuppressMessage("ReSharper", "RedundantAssignment")]
        [SuppressMessage("ReSharper", "NotAccessedVariable")]
        public void Temporary_ResetsTheValueOfExpiredResource()
        {
            var called = 0;
            var temp = new Temporary<int>(() =>
            {
                called++;
                return -1;
            }, 1);

            var x = temp.Value;
            Thread.Sleep(10);

            x = temp.Value;
            Assert.IsTrue(called > 1);
        }
    }
}
