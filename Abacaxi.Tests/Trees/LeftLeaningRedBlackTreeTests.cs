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

namespace Abacaxi.Tests.Trees
{
    using NUnit.Framework;
    using Abacaxi.Trees;
    using System.Collections.Generic;

    [TestFixture]
    public class LeftLeaningRedBlackTreeTests : BinarySearchTreeTests
    {
        protected override BinarySearchTree<TKey, TValue> Create<TKey, TValue>(IComparer<TKey> comparer) => new LeftLeaningRedBlackTree<TKey, TValue>(comparer);

        protected override BinarySearchTree<TKey, TValue> Create<TKey, TValue>() => new LeftLeaningRedBlackTree<TKey, TValue>();

        [Test]
        public override void GetEnumerator_PostOrder_ReturnsElementsPostOrder()
        {
            var tree = CreateFilled();

            var result = new List<KeyValuePair<int, int>>();
            using (var enumerator = tree.GetEnumerator(TreeTraversalMode.PostOrder))
            {
                while (enumerator.MoveNext())
                {
                    result.Add(enumerator.Current);
                }
            }

            TestHelper.AssertSequence(result,
                new KeyValuePair<int, int>(1, 100),
                new KeyValuePair<int, int>(3, 300),
                new KeyValuePair<int, int>(4, 400),
                new KeyValuePair<int, int>(2, 200),
                new KeyValuePair<int, int>(6, 600),
                new KeyValuePair<int, int>(7, 700),
                new KeyValuePair<int, int>(9, 900),
                new KeyValuePair<int, int>(8, 800),
                new KeyValuePair<int, int>(5, 500));
        }

        [Test]
        public override void GetEnumerator_PreOrder_ReturnsElementsPreOrder()
        {
            var tree = CreateFilled();

            var result = new List<KeyValuePair<int, int>>();
            using (var enumerator = tree.GetEnumerator(TreeTraversalMode.PreOrder))
            {
                while (enumerator.MoveNext())
                {
                    result.Add(enumerator.Current);
                }
            }

            TestHelper.AssertSequence(result,
                new KeyValuePair<int, int>(5, 500),
                new KeyValuePair<int, int>(2, 200),
                new KeyValuePair<int, int>(1, 100),
                new KeyValuePair<int, int>(4, 400),
                new KeyValuePair<int, int>(3, 300),
                new KeyValuePair<int, int>(8, 800),
                new KeyValuePair<int, int>(7, 700),
                new KeyValuePair<int, int>(6, 600),
                new KeyValuePair<int, int>(9, 900));
        }

    }
}