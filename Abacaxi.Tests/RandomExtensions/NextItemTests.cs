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

using System.Linq;

namespace Abacaxi.Tests.RandomExtensions
{
    using System;
    using NUnit.Framework;

    [TestFixture]
    public sealed class NextItemTests
    {
        private readonly Random _random = new Random();

        [Test]
        public void NextItem1_ThrowsException_IfRandomIsNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
                    ((Random)null).NextItem(new []{1}));
        }

        [Test]
        public void NextItem1_ThrowsException_IfSequenceIsNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
                _random.NextItem<int>(null));
        }


        [Test]
        public void NextItem1_ThrowsException_IfSequenceIsEmpty()
        {
            Assert.Throws<ArgumentException>(() =>
                _random.NextItem(new int[0]));
        }

        [Test]
        public void NextItem1_ReturnsARandomSample()
        {
            var t = 0;
            var f = 0;

            var items = Enumerable.Range(0, 100).AsList();

            for (var i = 0; i < 100; i++)
            {
                var r = _random.NextItem(items);
                Assert.IsTrue(items.Contains(r));

                if (r % 2 == 0)
                    t++;
                else
                {
                    f++;
                }
            }

            var ratio = t / (double) (t + f);
            Assert.IsTrue(ratio > .4 && ratio < .6);
        }



        [Test]
        public void NextItem2_ThrowsException_IfRandomIsNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
                ((Random)null).NextItem(1, 2));
        }

        [Test]
        public void NextItem2_ReturnsARandomSample_ForTwoItems()
        {
            var i1 = 0;
            var i2 = 0;
            for (var i = 0; i < 100; i++)
            {
                var r = _random.NextItem(100, 200);
                if (r == 100)
                    i1++;
                if (r== 200)
                    i2++;
            }

            var ratio = i1 / (double)(i1 + i2);
            Assert.IsTrue(ratio > .35 && ratio < .65);
        }

        [Test]
        public void NextItem2_ReturnsARandomSample()
        {
            var t = 0;
            var f = 0;

            var all = Enumerable.Range(0, 100).AsList();
            var items = all.Skip(2).ToArray();

            for (var i = 0; i < 100; i++)
            {
                var r = _random.NextItem(all[0], all[1], items);
                Assert.IsTrue(all.Contains(r));

                if (r % 2 == 0)
                    t++;
                else
                {
                    f++;
                }
            }

            var ratio = t / (double)(t + f);
            Assert.IsTrue(ratio > .35 && ratio < .65);
        }

    }
}
