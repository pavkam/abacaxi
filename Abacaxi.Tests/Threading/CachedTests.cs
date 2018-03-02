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

namespace Abacaxi.Tests.Threading
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using Abacaxi.Threading;
    using NUnit.Framework;

    [TestFixture]
    public class CachedTests
    {
        [Test]
        [SuppressMessage("ReSharper", "ObjectCreationAsStatement")]
        [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void Ctor1_ThrowsException_WhenRefreshValueFuncIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new Cached<int>(null, 1));
        }

        [Test]
        [SuppressMessage("ReSharper", "ObjectCreationAsStatement")]
        public void Ctor1_ThrowsException_WhenValueLifespanMillisIsLessThanZero()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Cached<int>(-1));
        }

        [Test]
        [SuppressMessage("ReSharper", "ObjectCreationAsStatement")]
        public void Ctor1_DoesNotInitializeTheValueImmediately()
        {
            var called = false;
            new Cached<int>(() =>
            {
                called = true;
                return -1;
            }, 1);

            Assert.IsFalse(called);
        }

        [Test]
        [SuppressMessage("ReSharper", "ObjectCreationAsStatement")]
        public void Ctor2_ThrowsException_WhenValueLifespanMillisIsLessThanZero()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Cached<int>(-1));
        }

        [Test]
        [SuppressMessage("ReSharper", "ObjectCreationAsStatement")]
        public void Value_ThrowsExceptionIfRefreshValueFuncWasNotSpecified()
        {
            var cached = new Cached<int>(100);
            Assert.Throws<InvalidOperationException>(() => Assert.IsTrue(cached.Value != 0));
        }

        [Test]
        [SuppressMessage("ReSharper", "UnusedVariable")]
        public void Value_RefreshesTheValueOnFirstRead()
        {
            var called = false;
            var temp = new Cached<int>(() =>
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
        public void Value_RefreshesTheValueIfExpired()
        {
            var called = 0;
            var temp = new Cached<int>(() =>
            {
                called++;
                return -1;
            }, 1);

            var x = temp.Value;
            Thread.Sleep(10);

            x = temp.Value;
            Assert.IsTrue(called > 1);
        }

        [Test]
        [SuppressMessage("ReSharper", "UnusedVariable")]
        [SuppressMessage("ReSharper", "RedundantAssignment")]
        [SuppressMessage("ReSharper", "NotAccessedVariable")]
        public void Value_RefreshesTheValueAtEveryOneMsReadIfTtlWasSuppliedAsZero()
        {
            var called = 0;
            var temp = new Cached<int>(() =>
            {
                called++;
                return -1;
            }, 0);

            var x = temp.Value;
            Thread.Sleep(1);
            x = temp.Value;
            Thread.Sleep(1);
            x = temp.Value;

            Assert.AreEqual(3, called);
        }

        [Test]
        [SuppressMessage("ReSharper", "NotAccessedVariable")]
        [SuppressMessage("ReSharper", "RedundantAssignment")]
        [SuppressMessage("ReSharper", "UnusedVariable")]
        public void Expire_MarksTheValueAsExpired()
        {
            var called = 0;
            var temp = new Cached<int>(() =>
            {
                called++;
                return -1;
            }, 1000);

            var x = temp.Value;
            temp.Expire();
            Assert.IsTrue(called == 1);
        }

        [Test]
        [SuppressMessage("ReSharper", "NotAccessedVariable")]
        [SuppressMessage("ReSharper", "RedundantAssignment")]
        public void Expire_WillForceTheValueToBeRefreshed()
        {
            var called = 0;
            var temp = new Cached<int>(() =>
            {
                called++;
                return -1;
            }, 1000);

            var x = temp.Value;
            temp.Expire();

            x = temp.Value;

            Assert.IsTrue(called == 2);
        }

        [Test]
        [SuppressMessage("ReSharper", "UnusedVariable")]
        public void Get_RefreshesTheValueOnFirstRead()
        {
            var called = false;
            var temp = new Cached<int>(1);

            var x = temp.Get(() =>
            {
                called = true;
                return -1;
            });

            Assert.IsTrue(called);
        }

        [Test]
        [SuppressMessage("ReSharper", "UnusedVariable")]
        [SuppressMessage("ReSharper", "RedundantAssignment")]
        [SuppressMessage("ReSharper", "NotAccessedVariable")]
        public void Get_RefreshesTheValueIfExpired()
        {
            var called = 0;
            var temp = new Cached<int>(1);

            var x = temp.Get(() =>
            {
                called++;
                return -1;
            });
            Thread.Sleep(10);

            x = temp.Get(() =>
            {
                called++;
                return -1;
            });
            Assert.IsTrue(called == 2);
        }

        [Test]
        [SuppressMessage("ReSharper", "UnusedVariable")]
        [SuppressMessage("ReSharper", "RedundantAssignment")]
        [SuppressMessage("ReSharper", "NotAccessedVariable")]
        public void Get_UsesTheSuppliedFunctionInsteadOfTheDefaultOne()
        {
            var called = 0;
            var temp = new Cached<int>(() =>
            {
                called+=10;
                return -1;
            }, 1);

            var x = temp.Get(() =>
            {
                called++;
                return -1;
            });

            Assert.IsTrue(called == 1);
        }
    }
}