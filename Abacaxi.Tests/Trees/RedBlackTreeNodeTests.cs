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

namespace Abacaxi.Tests.Trees
{
    using Abacaxi.Trees;
    using NUnit.Framework;

    [TestFixture]
    public class RedBlackTreeNodeTests
    {
        [Test]
        public void Ctor_CreatesInstance_WithDefaultKey()
        {
            var instance = new RedBlackTreeNode<int, string>();

            Assert.AreEqual(0, instance.Key);
        }

        [Test]
        public void Ctor_CreatesInstance_WithDefaultValue()
        {
            var instance = new RedBlackTreeNode<int, string>();

            Assert.IsNull(instance.Value);
        }

        [Test]
        public void Ctor_CreatesInstance_WithRedColor()
        {
            var instance = new RedBlackTreeNode<int, string>();

            Assert.AreEqual(RedBlackTreeNodeColor.Red, instance.Color);
        }

        [Test]
        public void Ctor_CreatesInstance_WithNullRightChild()
        {
            var instance = new RedBlackTreeNode<int, string>();

            Assert.IsNull(instance.RightChild);
        }

        [Test]
        public void Ctor_CreatesInstance_WithNullLeftChild()
        {
            var instance = new RedBlackTreeNode<int, string>();

            Assert.IsNull(instance.LeftChild);
        }

        [Test]
        public void Key_Setter_StoresTheValue()
        {
            var instance = new RedBlackTreeNode<int, string> {Key = 100};
            Assert.AreEqual(100, instance.Key);
        }

        [Test]
        public void Value_Setter_StoresTheValue()
        {
            var instance = new RedBlackTreeNode<int, string> {Value = "Hello"};
            Assert.AreEqual("Hello", instance.Value);
        }

        [Test]
        public void Color_Setter_StoresTheValue()
        {
            var instance = new RedBlackTreeNode<int, string> { Color = RedBlackTreeNodeColor.Black };
            Assert.AreEqual(RedBlackTreeNodeColor.Black, instance.Color);
        }

        [Test]
        public void RightChild_Setter_StoresTheValue()
        {
            var r = new RedBlackTreeNode<int, string>();
            var instance = new RedBlackTreeNode<int, string> {RightChild = r};
            Assert.AreSame(r, instance.RightChild);
        }

        [Test]
        public void LeftChild_Setter_StoresTheValue()
        {
            var r = new RedBlackTreeNode<int, string>();
            var instance = new RedBlackTreeNode<int, string> {LeftChild = r};
            Assert.AreSame(r, instance.LeftChild);
        }
    }
}