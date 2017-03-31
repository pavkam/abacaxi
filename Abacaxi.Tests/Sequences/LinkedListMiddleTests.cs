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

namespace Abacaxi.Tests.Sequences
{
    using System;
    using System.Collections.Generic;
    using Abacaxi.Sequences;
    using NUnit.Framework;

    [TestFixture]
    public class LinkedListMiddleTests
    {
        [Test]
        public void Find_ThrowsException_ForNullList()
        {
            Assert.Throws<ArgumentNullException>(() =>
                LinkedListMiddle.Find<int>(null));
        }

        [Test]
        public void Find_ReturnsNull_ForEmptyList()
        {
            var list = new LinkedList<int>();
            var node = LinkedListMiddle.Find(list);

            Assert.IsNull(node);
        }

        [Test]
        public void Find_ReturnsFirst_ForSingleNodeList()
        {
            var list = new LinkedList<int>();
            list.AddLast(1);

            var node = LinkedListMiddle.Find(list);
            Assert.AreSame(list.First, node);
        }

        [Test]
        public void Find_ReturnsSecond_ForTwoNodeList()
        {
            var list = new LinkedList<int>();
            list.AddLast(1);
            list.AddLast(2);

            var node = LinkedListMiddle.Find(list);
            Assert.AreSame(list.Last, node);
        }

        [Test]
        public void Find_ReturnsSecond_ForThreeNodeList()
        {
            var list = new LinkedList<int>();
            list.AddLast(1);
            list.AddLast(2);
            list.AddLast(3);

            var node = LinkedListMiddle.Find(list);
            Assert.AreSame(list.First.Next, node);
        }
    }
}
