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

namespace Abacaxi.Tests.LinkedLists
{
    using System;
    using System.Collections.Generic;
    using Abacaxi.LinkedLists;
    using NUnit.Framework;

    [TestFixture]
    public class ListMiddleTests
    {
        [Test]
        public void Find_ThrowsException_ForNullHead()
        {
            Assert.Throws<ArgumentNullException>(() =>
                ListMiddle.Find<int>(null));
        }

        [Test]
        public void Find_ReturnsFirst_ForSingleNodeList()
        {
            var head = Node<int>.Create(new[] { 1 });

            var node = ListMiddle.Find(head);
            Assert.AreSame(head, node);
        }

        [Test]
        public void Find_ReturnsFirst_ForTwoNodeList()
        {
            var head = Node<int>.Create(new[] { 1, 2 });

            var node = ListMiddle.Find(head);
            Assert.AreSame(head, node);
        }

        [Test]
        public void Find_ReturnsSecond_ForThreeNodeList()
        {
            var head = Node<int>.Create(new[] { 1, 2, 3 });

            var node = ListMiddle.Find(head);
            Assert.AreSame(head.Next, node);
        }
    }
}
