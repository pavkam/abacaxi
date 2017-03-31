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
    using Abacaxi.LinkedLists;
    using NUnit.Framework;

    [TestFixture]
    public class ReverseListTests
    {
        [Test]
        public void ReverseIterative_ThrowsException_ForNullHead()
        {
            Assert.Throws<ArgumentNullException>(() =>
                ReverseList.ReverseIterative<int>(null));
        }

        [Test]
        public void ReverseIterative_DoesNothing_ForSingleNode()
        {
            var head = Node<int>.Create(new[] { 1 });

            var newHead = ReverseList.ReverseIterative(head);

            Assert.AreSame(head, newHead);
            Assert.IsNull(newHead.Next);
        }

        [Test]
        public void ReverseIterative_Reverses_AListOfTwo()
        {
            var e1 = Node<int>.Create(new[] { 1, 2 });
            var e2 = e1.Next;

            var newHead = ReverseList.ReverseIterative(e1);

            Assert.AreSame(e2, newHead);
            Assert.AreSame(e2.Next, e1);
            Assert.IsNull(e1.Next);
        }


        [Test]
        public void ReverseIterative_Reverses_AListOfThree()
        {
            var e1 = Node<int>.Create(new[] { 1, 2, 3 });
            var e2 = e1.Next;
            var e3 = e2.Next;

            var newHead = ReverseList.ReverseIterative(e1);

            Assert.AreSame(e3, newHead);
            Assert.AreSame(e3.Next, e2);
            Assert.AreSame(e2.Next, e1);
            Assert.IsNull(e1.Next);
        }

        [Test]
        public void ReverseRecursive_ThrowsException_ForNullHead()
        {
            Assert.Throws<ArgumentNullException>(() =>
                ReverseList.ReverseRecursive<int>(null));
        }

        [Test]
        public void ReverseRecursive_DoesNothing_ForSingleNode()
        {
            var head = Node<int>.Create(new[] { 1 });

            var newHead = ReverseList.ReverseRecursive(head);

            Assert.AreSame(head, newHead);
            Assert.IsNull(newHead.Next);
        }

        [Test]
        public void ReverseRecursive_Reverses_AListOfTwo()
        {
            var e1 = Node<int>.Create(new[] { 1, 2 });
            var e2 = e1.Next;

            var newHead = ReverseList.ReverseRecursive(e1);

            Assert.AreSame(e2, newHead);
            Assert.AreSame(e2.Next, e1);
            Assert.IsNull(e1.Next);
        }


        [Test]
        public void ReverseRecursive_Reverses_AListOfThree()
        {
            var e1 = Node<int>.Create(new[] { 1, 2, 3 });
            var e2 = e1.Next;
            var e3 = e2.Next;

            var newHead = ReverseList.ReverseRecursive(e1);

            Assert.AreSame(e3, newHead);
            Assert.AreSame(e3.Next, e2);
            Assert.AreSame(e2.Next, e1);
            Assert.IsNull(e1.Next);
        }
    }
}
