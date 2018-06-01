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
    using System.Linq;
    using NUnit.Framework;

    [TestFixture]
    public sealed class NextItemTests
    {
        private readonly Random _random = new Random();

        [Test]
        public void NextItem1_ReturnsARandomSample()
        {
            var all = Enumerable.Range(0, 100).AsList();
            var set = all.ToSet();

            for (var i = 0; i < 10000 && set.Count > 0; i++)
            {
                var r = _random.NextItem(all);
                Assert.IsTrue(all.Contains(r));
                set.Remove(r);
            }

            Assert.AreEqual(0, set.Count);
        }

        [Test, SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void NextItem1_ThrowsException_IfRandomIsNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
                ((Random) null).NextItem(new[] { 1 }));
        }

        [Test]
        public void NextItem1_ThrowsException_IfSequenceIsEmpty()
        {
            Assert.Throws<ArgumentException>(() =>
                _random.NextItem(new int[0]));
        }

        [Test, SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void NextItem1_ThrowsException_IfSequenceIsNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
                _random.NextItem<int>(null));
        }

        [Test]
        public void NextItem2_ReturnsARandomSample()
        {
            var all = Enumerable.Range(0, 100).AsList();
            var items = all.Skip(2).ToArray();
            var set = all.ToSet();

            for (var i = 0; i < 10000 && set.Count > 0; i++)
            {
                var r = _random.NextItem(all[0], all[1], items);
                Assert.IsTrue(all.Contains(r));
                set.Remove(r);
            }

            Assert.AreEqual(0, set.Count);
        }

        [Test]
        public void NextItem2_ReturnsARandomSample_ForTwoItems()
        {
            var all = new[] { 100, 200 };
            var set = all.ToSet();

            for (var i = 0; i < 100; i++)
            {
                var r = _random.NextItem(all[0], all[1]);
                Assert.IsTrue(all.Contains(r));
                set.Remove(r);
            }

            Assert.AreEqual(0, set.Count);
        }

        [Test, SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void NextItem2_ThrowsException_IfRandomIsNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
                ((Random) null).NextItem(1, 2));
        }
    }
}