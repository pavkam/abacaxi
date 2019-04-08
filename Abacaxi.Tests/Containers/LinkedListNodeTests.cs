/* Copyright 2017-2019 by Alexandru Ciobanu (alex+git@ciobanu.org)
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
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using Abacaxi.Containers;
    using NUnit.Framework;

    [TestFixture]
    public sealed class LinkedListNodeTests
    {
        [Test]
        public void Create_ReturnsNull_ForEmptySequence()
        {
            var head = LinkedListNode<int>.Create(new int[] { });

            Assert.IsNull(head);
        }

        [Test]
        public void Create_ReturnsOneValidNode_ForSequenceOfOne()
        {
            var head = LinkedListNode<int>.Create(new[] {1});

            Assert.NotNull(head);
            Assert.AreEqual(1, head.Value);
            Assert.IsNull(head.Next);
        }

        [Test]
        public void Create_ReturnsTwoValidNodes_ForSequenceOfTwo()
        {
            var head = LinkedListNode<int>.Create(new[] {1, 2});

            Assert.NotNull(head);
            Assert.AreEqual(1, head.Value);
            Assert.IsNotNull(head.Next);
            Assert.AreEqual(2, head.Next.Value);
        }

        [Test]
        public void Ctor_StoresTheValue()
        {
            var node = new LinkedListNode<int>(99);

            Assert.AreEqual(99, node.Value);
        }

        [Test]
        public void Enumeration_ReturnsSelf()
        {
            var node = LinkedListNode<char>.Create("A");
            Debug.Assert(node != null);

            TestHelper.AssertSequence(node,
                node);
        }

        [Test]
        public void Enumeration_ReturnsSequence()
        {
            var head = LinkedListNode<char>.Create("ALEX");
            Debug.Assert(head != null);
            Debug.Assert(head.Next != null);
            Debug.Assert(head.Next.Next != null);

            TestHelper.AssertSequence(head,
                head,
                head.Next,
                head.Next.Next,
                head.Next.Next.Next);
        }


        [Test]
        public void GetIntersectionNode_ReturnsFirstNode_IfSelfChecking_1()
        {
            var head = LinkedListNode<int>.Create(new[] {1});
            Debug.Assert(head != null);

            Assert.AreSame(head, head.GetIntersectionNode(head));
        }

        [Test]
        public void GetIntersectionNode_ReturnsFirstNode_IfSelfChecking_2()
        {
            var head = LinkedListNode<int>.Create(new[] {1, 2});
            Debug.Assert(head != null);

            Assert.AreSame(head, head.GetIntersectionNode(head));
        }

        [Test]
        public void GetIntersectionNode_ReturnsFirstNode_IfSelfChecking_3()
        {
            var head = LinkedListNode<int>.Create(new[] {1, 2, 3});
            Debug.Assert(head != null);

            Assert.AreSame(head, head.GetIntersectionNode(head));
        }

        [Test]
        public void GetIntersectionNode_ReturnsMiddleNode_IfSelfChecking_ByMiddle()
        {
            var head = LinkedListNode<int>.Create(new[] {1, 2, 3});
            Debug.Assert(head != null);
            Debug.Assert(head.Next != null);

            Assert.AreSame(head.Next, head.GetIntersectionNode(head.Next));
        }

        [Test]
        public void GetIntersectionNode_ReturnsTheProperNode_1()
        {
            var list1 = LinkedListNode<int>.Create(new[] {1, 2, 3, 4, 5});
            Debug.Assert(list1 != null);

            var list2 = LinkedListNode<int>.Create(new[] {12});
            Debug.Assert(list2 != null);

            list2.Next = list1.Next;

            Assert.AreSame(list1.Next, list2.GetIntersectionNode(list1));
        }

        [Test]
        public void GetIntersectionNode_ReturnsTheProperNode_2()
        {
            var list1 = LinkedListNode<int>.Create(new[] {1, 2, 3, 4, 5});
            Debug.Assert(list1 != null);

            var list2 = LinkedListNode<int>.Create(new[] {11, 12, 13, 14, 15, 16});
            Debug.Assert(list2 != null);

            list2.GetTailNode().Next = list1.GetMiddleNode();

            Assert.AreSame(list1.GetMiddleNode(), list2.GetIntersectionNode(list1));
        }

        [Test]
        public void GetIntersectionNode_ReturnsTheProperNode_3()
        {
            var list1 = LinkedListNode<int>.Create(new[] {1, 2, 3, 4, 5});
            Debug.Assert(list1 != null);

            var list2 = LinkedListNode<int>.Create(new[] {6});
            Debug.Assert(list2 != null);

            list2.Next = list1.GetTailNode();

            Assert.IsTrue(list1.GetIntersectionNode(list2) == list2.Next);
        }


        [Test]
        public void GetIntersectionNode_ThrowsError_IfListIsKnotted()
        {
            var list = new LinkedListNode<int>(1);
            list.Next = list;
            var other = new LinkedListNode<int>(2);

            Assert.Throws<InvalidOperationException>(() => list.GetIntersectionNode(other));
        }

        [Test]
        public void GetIntersectionNode_ThrowsError_IfOtherListIsKnotted()
        {
            var list = new LinkedListNode<int>(1);
            var other = new LinkedListNode<int>(2);
            other.Next = other;

            Assert.Throws<InvalidOperationException>(() => list.GetIntersectionNode(other));
        }

        [Test, SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void GetIntersectionNode_ThrowsError_IfOtherListIsNull()
        {
            var list = new LinkedListNode<int>(1);

            Assert.Throws<ArgumentNullException>(() => list.GetIntersectionNode(null));
        }

        [Test]
        public void GetKnotNode_ReturnsHead_IfKnot()
        {
            var head = LinkedListNode<int>.Create(new[] {1});
            Debug.Assert(head != null);
            head.Next = head;

            Assert.AreSame(head, head.GetKnotNode());
        }


        [Test]
        public void GetKnotNode_ReturnsNull_IfTheListIsNotKnotted_1()
        {
            var head = LinkedListNode<int>.Create(new[] {1});
            Debug.Assert(head != null);

            Assert.IsNull(head.GetKnotNode());
        }

        [Test]
        public void GetKnotNode_ReturnsNull_IfTheListIsNotKnotted_2()
        {
            var head = LinkedListNode<int>.Create(new[] {1, 2});
            Debug.Assert(head != null);

            Assert.IsNull(head.GetKnotNode());
        }

        [Test]
        public void GetKnotNode_ReturnsNull_IfTheListIsNotKnotted_3()
        {
            var head = LinkedListNode<int>.Create(new[] {1, 2, 3});
            Debug.Assert(head != null);

            Assert.IsNull(head.GetKnotNode());
        }

        [Test]
        public void GetKnotNode_ReturnsSecond_IfKnot()
        {
            var head = LinkedListNode<int>.Create(new[] {1, 2});
            Debug.Assert(head != null);
            Debug.Assert(head.Next != null);

            head.Next.Next = head;

            Assert.That(head.Next == head.GetKnotNode());
        }

        [Test]
        public void GetKnotNode_ReturnsThird_IfKnot_1()
        {
            var head = LinkedListNode<int>.Create(new[] {1, 2, 3});
            Debug.Assert(head != null);
            Debug.Assert(head.Next != null);
            Debug.Assert(head.Next.Next != null);

            head.Next.Next.Next = head;

            Assert.That(head.Next.Next == head.GetKnotNode());
        }

        [Test]
        public void GetKnotNode_ReturnsThird_IfKnot_2()
        {
            var head = LinkedListNode<int>.Create(new[] {1, 2, 3});
            Debug.Assert(head != null);
            Debug.Assert(head.Next != null);
            Debug.Assert(head.Next.Next != null);

            head.Next.Next.Next = head.Next;

            Assert.That(head.Next.Next == head.GetKnotNode());
        }

        [Test]
        public void GetKnotNode_ReturnsThird_IfKnot_3()
        {
            var head = LinkedListNode<int>.Create(new[] {1, 2, 3});
            Debug.Assert(head != null);
            Debug.Assert(head.Next != null);
            Debug.Assert(head.Next.Next != null);

            head.Next.Next.Next = head.Next.Next;

            Assert.That(head.Next.Next == head.GetKnotNode());
        }


        [Test]
        public void GetLength_ReturnsOne_ForSingleNodeList()
        {
            var head = LinkedListNode<int>.Create(new[] {1});
            Debug.Assert(head != null);

            var result = head.GetLength();
            Assert.AreEqual(1, result);
        }

        [Test]
        public void GetLength_ReturnsThree_ForThreeNodeList()
        {
            var head = LinkedListNode<int>.Create(new[] {1, 2, 3});
            Debug.Assert(head != null);

            var result = head.GetLength();

            Assert.AreEqual(3, result);
        }

        [Test]
        public void GetLength_ReturnsTwo_ForTwoNodeList()
        {
            var head = LinkedListNode<int>.Create(new[] {1, 2});
            Debug.Assert(head != null);

            var result = head.GetLength();
            Assert.AreEqual(2, result);
        }

        [Test]
        public void GetLength_ThrowsError_ForKnottedList()
        {
            var head = new LinkedListNode<int>(1);
            head.Next = head;

            Assert.Throws<InvalidOperationException>(() => head.GetLength());
        }

        [Test]
        public void GetMiddleNode_ReturnsFirstNode_ForSingleNodeList()
        {
            var head = LinkedListNode<int>.Create(new[] {1});
            Debug.Assert(head != null);

            var result = head.GetMiddleNode();
            Assert.AreSame(head, result);
        }

        [Test]
        public void GetMiddleNode_ReturnsFirstNode_ForTwoNodeList()
        {
            var head = LinkedListNode<int>.Create(new[] {1, 2});
            Debug.Assert(head != null);

            var result = head.GetMiddleNode();
            Assert.AreSame(head, result);
        }

        [Test]
        public void GetMiddleNode_ReturnsSecondNode_ForThreeNodeList()
        {
            var head = LinkedListNode<int>.Create(new[] {1, 2, 3});
            Debug.Assert(head != null);

            var result = head.GetMiddleNode();
            Assert.AreSame(head.Next, result);
        }

        [Test]
        public void GetMiddleNode_ThrowsError_ForKnottedList()
        {
            var head = new LinkedListNode<int>(1);
            head.Next = head;

            Assert.Throws<InvalidOperationException>(() => head.GetMiddleNode());
        }

        [Test]
        public void GetTailNode_ReturnsFirstNode_ForSingleNodeList()
        {
            var head = LinkedListNode<int>.Create(new[] {1});
            Debug.Assert(head != null);

            var tail = head.GetTailNode();

            Assert.AreSame(head, tail);
        }


        [Test]
        public void GetTailNode_ReturnsSecondNode_ForTwoNodeList()
        {
            var head = LinkedListNode<int>.Create(new[] {1, 2});
            Debug.Assert(head != null);

            var tail = head.GetTailNode();
            Assert.AreSame(head.Next, tail);
        }

        [Test]
        public void GetTailNode_ReturnsThirdNode_ForThreeNodeList()
        {
            var head = LinkedListNode<int>.Create(new[] {1, 2, 3});
            Debug.Assert(head != null);

            var tail = head.GetTailNode();

            Debug.Assert(head.Next != null);
            Assert.AreSame(head.Next.Next, tail);
        }

        [Test]
        public void GetTailNode_ThrowsError_ForKnottedList()
        {
            var head = new LinkedListNode<int>(1);
            head.Next = head;

            Assert.Throws<InvalidOperationException>(() => head.GetTailNode());
        }


        [Test]
        public void Next_CanBeAssigned()
        {
            var node = new LinkedListNode<int>(0);

            node.Next = node;
            Assert.AreSame(node, node.Next);
        }

        [Test]
        public void Next_CanBeSetToNull()
        {
            var node = new LinkedListNode<int>(0);
            node.Next = node;
            node.Next = null;

            Assert.IsNull(node.Next);
        }

        [Test]
        public void Reverse_DoesNothing_ForSingleNode()
        {
            var head = LinkedListNode<int>.Create(new[] {1});
            Debug.Assert(head != null);

            var newHead = head.Reverse();

            Assert.AreSame(head, newHead);
            Assert.IsNull(newHead.Next);
        }

        [Test]
        public void Reverse_Reverses_AListOfThree()
        {
            var e1 = LinkedListNode<int>.Create(new[] {1, 2, 3});
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
        public void Reverse_Reverses_AListOfTwo()
        {
            var e1 = LinkedListNode<int>.Create(new[] {1, 2});
            Debug.Assert(e1 != null);
            var e2 = e1.Next;
            Debug.Assert(e2 != null);

            var newHead = e1.Reverse();

            Assert.AreSame(e2, newHead);
            Assert.AreSame(e2.Next, e1);
            Assert.IsNull(e1.Next);
        }

        [Test]
        public void ToString_Returns_ProperValue_1()
        {
            var node = new LinkedListNode<string>("this");

            Assert.AreEqual("this => null", node.ToString());
        }

        [Test]
        public void ToString_Returns_ProperValue_2()
        {
            var node = new LinkedListNode<string>("this")
            {
                Next = new LinkedListNode<string>("next")
            };

            Assert.AreEqual("this => next", node.ToString());
        }


        [Test]
        public void TryGetMiddleAndTailNodes_ReturnsFirstNode_AsMiddle_ForSingleNodeList()
        {
            var head = LinkedListNode<int>.Create(new[] {1});
            Debug.Assert(head != null);

            head.TryGetMiddleAndTailNodes(out var middle, out _);

            Assert.AreSame(head, middle);
        }

        [Test]
        public void TryGetMiddleAndTailNodes_ReturnsFirstNode_AsMiddle_ForTwoNodeList()
        {
            var head = LinkedListNode<int>.Create(new[] {1, 2});
            Debug.Assert(head != null);

            head.TryGetMiddleAndTailNodes(out var middle, out _);

            Assert.AreSame(head, middle);
        }

        [Test]
        public void TryGetMiddleAndTailNodes_ReturnsFirstNode_AsTail_ForSingleNodeList()
        {
            var head = LinkedListNode<int>.Create(new[] {1});
            Debug.Assert(head != null);

            head.TryGetMiddleAndTailNodes(out _, out var tail);

            Assert.AreSame(head, tail);
        }

        [Test]
        public void TryGetMiddleAndTailNodes_ReturnsMinusOne_ForDoubleKnottedNode()
        {
            var head = LinkedListNode<int>.Create(new[] {1, 2});
            Debug.Assert(head != null);
            Debug.Assert(head.Next != null);

            head.Next.Next = head;

            var len = head.TryGetMiddleAndTailNodes(out _, out _);
            Assert.AreEqual(-1, len);
        }

        [Test]
        public void TryGetMiddleAndTailNodes_ReturnsMinusOne_ForSingleKnottedNode()
        {
            var head = LinkedListNode<int>.Create(new[] {1});
            Debug.Assert(head != null);

            head.Next = head;

            var len = head.TryGetMiddleAndTailNodes(out _, out _);
            Assert.AreEqual(-1, len);
        }

        [Test]
        public void TryGetMiddleAndTailNodes_ReturnsMinusOne_ForTripleKnottedNode()
        {
            var head = LinkedListNode<int>.Create(new[] {1, 2, 3});
            Debug.Assert(head != null);
            Debug.Assert(head.Next != null);
            Debug.Assert(head.Next.Next != null);

            head.Next.Next.Next = head.Next;

            var len = head.TryGetMiddleAndTailNodes(out _, out _);
            Assert.AreEqual(-1, len);
        }

        [Test]
        public void TryGetMiddleAndTailNodes_ReturnsOne_ForSingleNodeList()
        {
            var head = LinkedListNode<int>.Create(new[] {1});
            Debug.Assert(head != null);

            var result = head.TryGetMiddleAndTailNodes(out _, out _);
            Assert.AreEqual(1, result);
        }

        [Test]
        public void TryGetMiddleAndTailNodes_ReturnsSecondNode_AsMiddle_ForThreeNodeList()
        {
            var head = LinkedListNode<int>.Create(new[] {1, 2, 3});
            Debug.Assert(head != null);

            head.TryGetMiddleAndTailNodes(out var middle, out _);

            Assert.AreSame(head.Next, middle);
        }

        [Test]
        public void TryGetMiddleAndTailNodes_ReturnsSecondNode_AsTail_ForTwoNodeList()
        {
            var head = LinkedListNode<int>.Create(new[] {1, 2});
            Debug.Assert(head != null);

            head.TryGetMiddleAndTailNodes(out _, out var tail);

            Assert.AreSame(head.Next, tail);
        }

        [Test]
        public void TryGetMiddleAndTailNodes_ReturnsThirdNode_AsTail_ForThreeNodeList()
        {
            var head = LinkedListNode<int>.Create(new[] {1, 2, 3});
            Debug.Assert(head != null);

            head.TryGetMiddleAndTailNodes(out _, out var tail);

            Debug.Assert(head.Next != null);
            Assert.AreSame(head.Next.Next, tail);
        }

        [Test]
        public void TryGetMiddleAndTailNodes_ReturnsThree_ForThreeNodeList()
        {
            var head = LinkedListNode<int>.Create(new[] {1, 2, 3});
            Debug.Assert(head != null);

            var result = head.TryGetMiddleAndTailNodes(out _, out _);

            Assert.AreEqual(3, result);
        }

        [Test]
        public void TryGetMiddleAndTailNodes_ReturnsTwo_ForTwoNodeList()
        {
            var head = LinkedListNode<int>.Create(new[] {1, 2});
            Debug.Assert(head != null);

            var result = head.TryGetMiddleAndTailNodes(out _, out _);
            Assert.AreEqual(2, result);
        }
    }
}