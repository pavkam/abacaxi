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

using System.Diagnostics;

namespace Abacaxi.Tests.Containers
{
    using NUnit.Framework;
    using Abacaxi.Containers;

    [TestFixture]
    public class SingleLinkedNodeTests
    {
        [Test]
        public void Ctor_StoresTheValue()
        {
            var node = new SingleLinkedNode<int>(99);

            Assert.AreEqual(99, node.Value);
        }

        [Test]
        public void Next_CanBeAssigned()
        {
            var node = new SingleLinkedNode<int>(0);

            node.Next = node;
            Assert.AreSame(node, node.Next);
        }

        [Test]
        public void Next_CanBeSetToNull()
        {
            var node = new SingleLinkedNode<int>(0);
            node.Next = node;
            node.Next = null;

            Assert.IsNull(node.Next);
        }

        [Test]
        public void Create_ReturnsNull_ForEmptySequence()
        {
            var head = SingleLinkedNode<int>.Create(new int[] { });

            Assert.IsNull(head);
        }

        [Test]
        public void Create_ReturnsOneValidNode_ForSequenceOfOne()
        {
            var head = SingleLinkedNode<int>.Create(new[] { 1 });

            Assert.NotNull(head);
            Assert.AreEqual(1, head.Value);
            Assert.IsNull(head.Next);
        }

        [Test]
        public void Create_ReturnsTwoValidNodes_ForSequenceOfTwo()
        {
            var head = SingleLinkedNode<int>.Create(new[] { 1, 2 });

            Assert.NotNull(head);
            Assert.AreEqual(1, head.Value);
            Assert.IsNotNull(head.Next);
            Assert.AreEqual(2, head.Next.Value);
        }

        [Test]
        public void VerifyIfKnotted_ReturnsFalse_ForSingleUnknottedNode()
        {
            var head = SingleLinkedNode<int>.Create(new[] { 1 });
            Debug.Assert(head != null);

            var check = head.VerifyIfKnotted();
            Assert.IsFalse(check);
        }

        [Test]
        public void VerifyIfKnotted_ReturnsFalse_ForDoubleUnknottedNode()
        {
            var head = SingleLinkedNode<int>.Create(new[] { 1, 2 });
            Debug.Assert(head != null);

            var check = head.VerifyIfKnotted();
            Assert.IsFalse(check);
        }

        [Test]
        public void VerifyIfKnotted_ReturnsFalse_ForTripleUnknottedNode()
        {
            var head = SingleLinkedNode<int>.Create(new[] { 1, 2, 3 });
            Debug.Assert(head != null);

            var check = head.VerifyIfKnotted();
            Assert.IsFalse(check);
        }

        [Test]
        public void VerifyIfKnotted_ReturnsTrue_ForSingleKnottedNode()
        {
            var head = SingleLinkedNode<int>.Create(new[] { 1 });
            Debug.Assert(head != null);

            head.Next = head;

            var check = head.VerifyIfKnotted();
            Assert.IsTrue(check);
        }

        [Test]
        public void VerifyIfKnotted_ReturnsTrue_ForDoubleKnottedNode()
        {
            var head = SingleLinkedNode<int>.Create(new[] { 1, 2 });
            Debug.Assert(head != null);
            Debug.Assert(head.Next != null);

            head.Next.Next = head;

            var check = head.VerifyIfKnotted();
            Assert.IsTrue(check);
        }

        [Test]
        public void VerifyIfKnotted_ReturnsTrue_ForTripleKnottedNode()
        {
            var head = SingleLinkedNode<int>.Create(new[] { 1, 2, 3 });
            Debug.Assert(head != null);
            Debug.Assert(head.Next != null);
            Debug.Assert(head.Next.Next != null);

            head.Next.Next.Next = head.Next;

            var check = head.VerifyIfKnotted();
            Assert.IsTrue(check);
        }

        [Test]
        public void FindMiddle_ReturnsFirst_ForSingleNodeList()
        {
            var head = SingleLinkedNode<int>.Create(new[] { 1 });
            Debug.Assert(head != null);

            var node = head.FindMiddle();
            Assert.AreSame(head, node);
        }

        [Test]
        public void FindMiddle_ReturnsFirst_ForTwoNodeList()
        {
            var head = SingleLinkedNode<int>.Create(new[] { 1, 2 });
            Debug.Assert(head != null);

            var node = head.FindMiddle();
            Assert.AreSame(head, node);
        }

        [Test]
        public void FindMiddle_ReturnsSecond_ForThreeNodeList()
        {
            var head = SingleLinkedNode<int>.Create(new[] { 1, 2, 3 });
            Debug.Assert(head != null);

            var node = head.FindMiddle();
            Assert.AreSame(head.Next, node);
        }

        [Test]
        public void Reverse_DoesNothing_ForSingleNode()
        {
            var head = SingleLinkedNode<int>.Create(new[] { 1 });
            Debug.Assert(head != null);

            var newHead = head.Reverse();

            Assert.AreSame(head, newHead);
            Assert.IsNull(newHead.Next);
        }

        [Test]
        public void Reverse_Reverses_AListOfTwo()
        {
            var e1 = SingleLinkedNode<int>.Create(new[] { 1, 2 });
            Debug.Assert(e1 != null);
            var e2 = e1.Next;
            Debug.Assert(e2 != null);

            var newHead = e1.Reverse();

            Assert.AreSame(e2, newHead);
            Assert.AreSame(e2.Next, e1);
            Assert.IsNull(e1.Next);
        }


        [Test]
        public void Reverse_Reverses_AListOfThree()
        {
            var e1 = SingleLinkedNode<int>.Create(new[] { 1, 2, 3 });
            Debug.Assert(e1 != null);
            var e2 = e1.Next;
            Debug.Assert(e2 != null);
            var e3 = e2.Next;
            Debug.Assert(e3 != null);

            var newHead = e1.Reverse();

            Assert.AreSame(e3, newHead);
            Assert.AreSame(e3.Next, e2);
            Assert.AreSame(e2.Next, e1);
            Assert.IsNull(e1.Next);
        }

        [Test]
        public void Enumeration_ReturnsSelf()
        {
            var node = SingleLinkedNode<char>.Create("A");

            TestHelper.AssertSequence(node,
                node);
        }

        [Test]
        public void Enumeration_ReturnsSequence()
        {
            var head = SingleLinkedNode<char>.Create("ALEX");
            Debug.Assert(head != null);
            Debug.Assert(head.Next != null);
            Debug.Assert(head.Next.Next != null);

            TestHelper.AssertSequence(head,
                head,
                head.Next,
                head.Next.Next,
                head.Next.Next.Next);
        }
    }
}
