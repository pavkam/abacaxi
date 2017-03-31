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
    using Abacaxi.LinkedLists;
    using NUnit.Framework;

    [TestFixture]
    public class NodeTests
    {
        [Test]
        public void Ctor_StoresTheValue()
        {
            var node = new Node<int>(99);

            Assert.AreEqual(99, node.Value);
        }

        [Test]
        public void Next_CanBeAssigned()
        {
            var node = new Node<int>(0);

            node.Next = node;
            Assert.AreSame(node, node.Next);
        }

        [Test]
        public void Next_CanBeSetToNull()
        {
            var node = new Node<int>(0);
            node.Next = node;
            node.Next = null;

            Assert.IsNull(node.Next);
        }

        [Test]
        public void Create_ReturnsNull_ForEmptySequence()
        {
            var head = Node<int>.Create(new int[] { });

            Assert.IsNull(head);
        }
        
        [Test]
        public void Create_ReturnsOneValidNode_ForSequenceOfOne()
        {
            var head = Node<int>.Create(new int[] { 1 });

            Assert.NotNull(head);
            Assert.AreEqual(1, head.Value);
            Assert.IsNull(head.Next);
        }

        [Test]
        public void Create_ReturnsTwoValidNodes_ForSequenceOfTwo()
        {
            var head = Node<int>.Create(new int[] { 1, 2 });

            Assert.NotNull(head);
            Assert.AreEqual(1, head.Value);
            Assert.IsNotNull(head.Next);
            Assert.AreEqual(2, head.Next.Value);
        }
    }
}
