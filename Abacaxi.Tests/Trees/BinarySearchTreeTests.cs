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
    using System;
    using NUnit.Framework;
    using Abacaxi.Trees;
    using System.Collections.Generic;
    using System.Collections;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    [TestFixture]
    public class BinarySearchTreeTests
    {
        protected virtual BinarySearchTree<TKey, TValue> Create<TKey, TValue>(IComparer<TKey> comparer)
        {
            return new BinarySearchTree<TKey, TValue>(comparer);
        }

        protected virtual BinarySearchTree<TKey, TValue> Create<TKey, TValue>()
        {
            return new BinarySearchTree<TKey, TValue>();
        }

        protected BinarySearchTree<int, int> CreateFilled()
        {
            var tree = Create<int, int>(Comparer<int>.Default);
            tree.Add(5, 500);
            tree.Add(8, 800);
            tree.Add(3, 300);
            tree.Add(1, 100);
            tree.Add(2, 200);
            tree.Add(4, 400);
            tree.Add(7, 700);
            tree.Add(9, 900);
            tree.Add(6, 600);

            return tree;
        }

        [Test]
        public void Ctor_ThrowsException_IfComparerIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => Create<int,int>(null));
        }

        [Test]
        public void Ctor_CreatesEmptyTree()
        {
            var tree = Create<int, int>();
            Assert.IsEmpty(tree);
        }

        [Test]
        public void Ctor_ProperlyConsiders_TheComparer()
        {
            var tree = Create<string, int>(StringComparer.OrdinalIgnoreCase);
            tree["test"] = 1;
            tree["TEST"] = 100;

            TestHelper.AssertSequence(tree, new KeyValuePair<string, int>("test", 100));
        }

        [Test]
        public void Ctor_UsesDefaultComparer_IfNotSupplied()
        {
            var tree = Create<string, int>();
            tree["test"] = 1;
            tree["TEST"] = 100;

            TestHelper.AssertSequence(tree, 
                new KeyValuePair<string, int>("test", 1),
                new KeyValuePair<string, int>("TEST", 100));
        }

        [Test]
        public void Add_AddsOneNode_ToTheTree()
        {
            var tree = Create<int, int>();
            tree.Add(1, 100);

            TestHelper.AssertSequence(tree, new KeyValuePair<int, int>(1, 100));
        }

        [Test]
        public void Add_WillKeepTheTreeBalanced1()
        {
            var tree = Create<int, int>(Comparer<int>.Default);
            tree.Add(1, 1);
            tree.Add(2, 2);
            tree.Add(3, 3);

            TestHelper.AssertSequence(tree,
                new KeyValuePair<int, int>(1, 1),
                new KeyValuePair<int, int>(2, 2),
                new KeyValuePair<int, int>(3, 3));
        }

        [Test]
        public void Add_WillKeepTheTreeBalanced2()
        {
            var tree = Create<int, int>(Comparer<int>.Default);
            tree.Add(3, 3);
            tree.Add(2, 2);
            tree.Add(1, 1);

            TestHelper.AssertSequence(tree,
                new KeyValuePair<int, int>(1, 1),
                new KeyValuePair<int, int>(2, 2),
                new KeyValuePair<int, int>(3, 3));
        }

        [Test]
        public void Add_WillKeepTheTreeBalanced3()
        {
            var tree = CreateFilled();

            TestHelper.AssertSequence(tree, 
                new KeyValuePair<int, int>(1, 100),
                new KeyValuePair<int, int>(2, 200),
                new KeyValuePair<int, int>(3, 300),
                new KeyValuePair<int, int>(4, 400),
                new KeyValuePair<int, int>(5, 500),
                new KeyValuePair<int, int>(6, 600),
                new KeyValuePair<int, int>(7, 700),
                new KeyValuePair<int, int>(8, 800),
                new KeyValuePair<int, int>(9, 900));
        }

        [Test]
        public void Add_ThrowsException_IfKeyDuplicate()
        {
            var tree = Create<int, int>();
            tree.Add(1, 100);
            Assert.Throws<ArgumentException>(() => tree.Add(1, 500));
        }


        [Test]
        public void ICollection_Add_AddsOneNode_ToTheTree()
        {
            var tree = Create<int, int>();
            tree.Add(new KeyValuePair<int, int>(1, 100));

            TestHelper.AssertSequence(tree, new KeyValuePair<int, int>(1, 100));
        }
        
        [Test]
        public void ICollection_Add_ThrowsException_IfKeyDuplicate()
        {
            var tree = Create<int, int>();
            tree.Add(new KeyValuePair<int, int>(1, 100));
            Assert.Throws<ArgumentException>(() => tree.Add(1, 500));
        }

        [Test]
        public void Update_UpdatesTheValue_OfExistingNode()
        {
            var tree = Create<int, int>();
            tree.Add(1, 100);
            tree.Update(1, 200);

            TestHelper.AssertSequence(tree, new KeyValuePair<int, int>(1, 200));
        }

        [Test]
        public void Update_UpdatesTheValue_OfExistingChildNode()
        {
            var tree = Create<int, int>();
            tree.Add(1, 100);
            tree.Add(2, 200);
            tree.Update(2, 222);

            TestHelper.AssertSequence(tree, 
                new KeyValuePair<int, int>(1, 100),
                new KeyValuePair<int, int>(2, 222));
        }

        [Test]
        public void Update_ThrowsException_IfKeyNotFound()
        {
            var tree = Create<int, int>();
            Assert.Throws<ArgumentException>(() => tree.Update(1, 500));
        }

        [Test]
        public void Update_ThrowsException_IfKeyNotFound_InOneNodeTree()
        {
            var tree = Create<int, int>();
            tree.Add(2, 200);
            Assert.Throws<ArgumentException>(() => tree.Update(1, 500));
        }

        [Test]
        public void AddOrUpdate_AddsTheNode_IfKeyDoesNotExist()
        {
            var tree = Create<int, int>();
            tree.AddOrUpdate(1, 100);

            TestHelper.AssertSequence(tree, new KeyValuePair<int, int>(1, 100));
        }

        [Test]
        public void AddOrUpdate_UpdatesTheNode_IfKeyDoesExist()
        {
            var tree = Create<int, int>();
            tree.Add(1, 100);
            tree.AddOrUpdate(1, 200);

            TestHelper.AssertSequence(tree, new KeyValuePair<int, int>(1, 200));
        }

        [Test]
        public void Remove_ReturnsFalse_IfKeyNotFound()
        {
            var tree = Create<int, int>();
            Assert.IsFalse(tree.Remove(1));
        }

        [Test]
        public void Remove_ReturnsFalse_IfKeyNotFound_InOneNodeTree()
        {
            var tree = Create<int, int>();
            tree.Add(1, 100);

            Assert.IsFalse(tree.Remove(2));
        }

        [Test]
        public void Remove_ReturnsTrue_IfKeyFound()
        {
            var tree = Create<int, int>();
            tree.Add(1, 100);

            Assert.IsTrue(tree.Remove(1));
        }

        [Test]
        public void Remove_ActuallyRemovesTheNode()
        {
            var tree = Create<int, int>();
            tree.Add(1, 100);
            tree.Remove(1);

            TestHelper.AssertSequence(tree);
        }

        [Test]
        public void Remove_KeepsTheTreeBalanced_Mid()
        {
            var tree = Create<int, int>();
            tree.Add(5, 500);
            tree.Add(9, 900);
            tree.Add(1, 100);

            tree.Remove(5);

            TestHelper.AssertSequence(tree,
                new KeyValuePair<int, int>(1, 100),
                new KeyValuePair<int, int>(9, 900));
        }

        [Test]
        public void Remove_KeepsTheTreeBalanced_Left()
        {
            var tree = Create<int, int>();
            tree.Add(5, 500);
            tree.Add(9, 900);
            tree.Add(1, 100);

            tree.Remove(1);

            TestHelper.AssertSequence(tree,
                new KeyValuePair<int, int>(5, 500),
                new KeyValuePair<int, int>(9, 900));
        }

        [Test]
        public void Remove_KeepsTheTreeBalanced_Right()
        {
            var tree = Create<int, int>();
            tree.Add(5, 500);
            tree.Add(9, 900);
            tree.Add(1, 100);

            tree.Remove(9);

            TestHelper.AssertSequence(tree,
                new KeyValuePair<int, int>(1, 100),
                new KeyValuePair<int, int>(5, 500));
        }

        [Test]
        public void ICollection_Remove_ReturnsFalse_IfKeyNotMatched()
        {
            var tree = Create<int, int>();
            tree.Add(5, 500);

            Assert.IsFalse(tree.Remove(new KeyValuePair<int, int>(1, 500)));
        }

        [Test]
        public void ICollection_Remove_ReturnsFalse_IfValueNotMatched()
        {
            var tree = Create<int, int>();
            tree.Add(5, 500);

            Assert.IsFalse(tree.Remove(new KeyValuePair<int, int>(5, 100)));
        }

        [Test]
        public void ICollection_Remove_ReturnsTrue_IfKeyAndValueAreMatched()
        {
            var tree = Create<int, int>();
            tree.Add(5, 500);

            Assert.IsTrue(tree.Remove(new KeyValuePair<int, int>(5, 500)));
        }

        [Test]
        public void ICollection_Remove_RemovesTheNode()
        {
            var tree = Create<int, int>();
            tree.Add(5, 500);

            tree.Remove(new KeyValuePair<int, int>(5, 500));

            Assert.IsEmpty(tree);
        }

        [Test]
        public void TryGetValue_ReturnsFalse_ForMissingKey()
        {
            var tree = Create<int, int>();
            Assert.IsFalse(tree.TryGetValue(1, out var dummy));
        }

        [Test]
        public void TryGetValue_ReturnsFalse_ForMissingKey_InOneNodeTree()
        {
            var tree = Create<int, int>();
            tree.Add(1, 100);

            Assert.IsFalse(tree.TryGetValue(2, out var dummy));
        }

        [Test]
        public void TryGetValue_ReturnsTrue_ForExistingKey()
        {
            var tree = Create<int, int>();
            tree.Add(1, 100);

            Assert.IsTrue(tree.TryGetValue(1, out var dummy));
        }

        [Test]
        public void TryGetValue_GetsTheValue_ForExistingKey()
        {
            var tree = Create<int, int>();
            tree.Add(1, 100);
            tree.TryGetValue(1, out var value);

            Assert.AreEqual(100, value);
        }

        [Test]
        public void ICollection_Contains_ReturnsFalse_IfKeyNotMatched()
        {
            var tree = Create<int, int>();
            tree.Add(5, 500);

            Assert.IsFalse(tree.Contains(new KeyValuePair<int, int>(1, 500)));
        }

        [Test]
        public void ICollection_Contains_ReturnsFalse_IfValueNotMatched()
        {
            var tree = Create<int, int>();
            tree.Add(5, 500);

            Assert.IsFalse(tree.Contains(new KeyValuePair<int, int>(5, 100)));
        }

        [Test]
        public void ICollection_Contains_ReturnsTrue_IfKeyAndValueAreMatched()
        {
            var tree = Create<int, int>();
            tree.Add(5, 500);

            Assert.IsTrue(tree.Contains(new KeyValuePair<int, int>(5, 500)));
        }

        [Test]
        public void Clear_RemovesAllNodes()
        {
            var tree = Create<int, int>();
            tree.Add(1, 100);
            tree.Clear();

            Assert.IsEmpty(tree);
        }

        [Test]
        public void Count_IsZero_WhenTreeIsCreated()
        {
            var tree = Create<int, int>();
            Assert.AreEqual(0, tree.Count);
        }

        [Test]
        public void Count_IsIncreased_WhenNodeIsAdded()
        {
            var tree = Create<int, int>();
            tree.Add(1, 1);
            Assert.AreEqual(1, tree.Count);
        }

        [Test]
        public void Count_IsDecreased_WhenNodeIsRemoved()
        {
            var tree = Create<int, int>();
            tree.Add(1, 1);
            tree.Remove(1);
            Assert.AreEqual(0, tree.Count);
        }

        [Test]
        public void Count_IsZero_AfterClear()
        {
            var tree = CreateFilled();
            tree.Clear();
            Assert.AreEqual(0, tree.Count);
        }

        [Test]
        public void Count_IsNotUpdated_WhenNodeIsUpdated()
        {
            var tree = Create<int, int>();
            tree[1] = 1;
            tree[1] = 2;
            Assert.AreEqual(1, tree.Count);
        }

        [Test]
        public void IsReadOnly_ReturnsFalse()
        {
            var tree = Create<int, int>();
            Assert.IsFalse(tree.IsReadOnly);
        }

        [Test]
        public void Indexer_ReturnsTheValueOfANode_IfKeyExists()
        {
            var tree = Create<int, int>();
            tree.Add(1, 100);

            Assert.AreEqual(100, tree[1]);
        }

        [Test]
        public void Indexer_ThrowsException_IfKeyDoesNotExist()
        {
            var tree = Create<int, int>();
            Assert.Throws<ArgumentException>(() =>
            {
                // ReSharper disable once UnusedVariable
                var a = tree[1];
            });
        }


        [Test]
        public void Indexer_AddsTheNode_IfKeyDoesNotExist()
        {
            var tree = Create<int, int>();
            tree[1] = 100;

            TestHelper.AssertSequence(tree, new KeyValuePair<int, int>(1, 100));
        }

        [Test]
        public void Indexer_UpdatesTheNode_IfKeyDoesExist()
        {
            var tree = Create<int, int>();
            tree.Add(1, 100);
            tree[1] = 200;

            TestHelper.AssertSequence(tree, new KeyValuePair<int, int>(1, 200));
        }

        [Test]
        public void Implicit_GetEnumerator_ReturnsElementsInOrder()
        {
            var tree = CreateFilled();

            var result = new List<KeyValuePair<int, int>>();
            foreach (var x in (IEnumerable) tree)
            {
                result.Add((KeyValuePair<int, int>)x);
            }

            TestHelper.AssertSequence(result,
                new KeyValuePair<int, int>(1, 100),
                new KeyValuePair<int, int>(2, 200),
                new KeyValuePair<int, int>(3, 300),
                new KeyValuePair<int, int>(4, 400),
                new KeyValuePair<int, int>(5, 500),
                new KeyValuePair<int, int>(6, 600),
                new KeyValuePair<int, int>(7, 700),
                new KeyValuePair<int, int>(8, 800),
                new KeyValuePair<int, int>(9, 900));
        }

        [Test]
        public void GetEnumerator_ReturnsElementsInOrder()
        {
            var tree = CreateFilled();

            var result = new List<KeyValuePair<int, int>>();
            foreach (var x in tree)
            {
                result.Add(x);
            }

            TestHelper.AssertSequence(result,
                new KeyValuePair<int, int>(1, 100),
                new KeyValuePair<int, int>(2, 200),
                new KeyValuePair<int, int>(3, 300),
                new KeyValuePair<int, int>(4, 400),
                new KeyValuePair<int, int>(5, 500),
                new KeyValuePair<int, int>(6, 600),
                new KeyValuePair<int, int>(7, 700),
                new KeyValuePair<int, int>(8, 800),
                new KeyValuePair<int, int>(9, 900));
        }

        [Test]
        public void GetEnumerator_InOrder_ReturnsElementsInOrder()
        {
            var tree = CreateFilled();

            var result = new List<KeyValuePair<int, int>>();
            using (var enumerator = tree.GetEnumerator(TreeTraversalMode.InOrder))
            {
                while (enumerator.MoveNext())
                {
                    result.Add(enumerator.Current);
                }
            }

            TestHelper.AssertSequence(result,
                new KeyValuePair<int, int>(1, 100),
                new KeyValuePair<int, int>(2, 200),
                new KeyValuePair<int, int>(3, 300),
                new KeyValuePair<int, int>(4, 400),
                new KeyValuePair<int, int>(5, 500),
                new KeyValuePair<int, int>(6, 600),
                new KeyValuePair<int, int>(7, 700),
                new KeyValuePair<int, int>(8, 800),
                new KeyValuePair<int, int>(9, 900));
        }

        [Test]
        public virtual void GetEnumerator_PostOrder_ReturnsElementsPostOrder()
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
                new KeyValuePair<int, int>(2, 200),
                new KeyValuePair<int, int>(1, 100),
                new KeyValuePair<int, int>(4, 400),
                new KeyValuePair<int, int>(3, 300),
                new KeyValuePair<int, int>(6, 600),
                new KeyValuePair<int, int>(7, 700),
                new KeyValuePair<int, int>(9, 900),
                new KeyValuePair<int, int>(8, 800),
                new KeyValuePair<int, int>(5, 500));
        }

        [Test]
        public virtual void GetEnumerator_PreOrder_ReturnsElementsPreOrder()
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
                new KeyValuePair<int, int>(3, 300),
                new KeyValuePair<int, int>(1, 100),
                new KeyValuePair<int, int>(2, 200),
                new KeyValuePair<int, int>(4, 400),
                new KeyValuePair<int, int>(8, 800),
                new KeyValuePair<int, int>(7, 700),
                new KeyValuePair<int, int>(6, 600),
                new KeyValuePair<int, int>(9, 900));
        }

        [TestCase(TreeTraversalMode.InOrder)]
        [TestCase(TreeTraversalMode.PostOrder)]
        [TestCase(TreeTraversalMode.PreOrder)]
        public void Enumeration_ThrowsExceptionOnAdd(TreeTraversalMode mode)
        {
            var tree = Create<int, int>();
            tree.Add(1, 1);
            using (var enumerator = tree.GetEnumerator(mode))
            {
                enumerator.MoveNext();
                tree.Add(2, 2);
                Assert.Throws<InvalidOperationException>(() => enumerator.MoveNext());
            }
        }

        [TestCase(TreeTraversalMode.InOrder)]
        [TestCase(TreeTraversalMode.PostOrder)]
        [TestCase(TreeTraversalMode.PreOrder)]
        public void Enumeration_ThrowsExceptionOnUpdate(TreeTraversalMode mode)
        {
            var tree = Create<int, int>();
            tree.Add(1, 1);
            using (var enumerator = tree.GetEnumerator(mode))
            {
                enumerator.MoveNext();
                tree.Update(1, 2);
                Assert.Throws<InvalidOperationException>(() => enumerator.MoveNext());
            }
        }

        [TestCase(TreeTraversalMode.InOrder)]
        [TestCase(TreeTraversalMode.PostOrder)]
        [TestCase(TreeTraversalMode.PreOrder)]
        public void Enumeration_ThrowsExceptionOnRemove(TreeTraversalMode mode)
        {
            var tree = Create<int, int>();
            tree.Add(1, 1);
            using (var enumerator = tree.GetEnumerator(mode))
            {
                enumerator.MoveNext();
                tree.Remove(1);
                Assert.Throws<InvalidOperationException>(() => enumerator.MoveNext());
            }
        }

        [TestCase(TreeTraversalMode.InOrder)]
        [TestCase(TreeTraversalMode.PostOrder)]
        [TestCase(TreeTraversalMode.PreOrder)]
        public void Enumeration_ThrowsExceptionOnClear(TreeTraversalMode mode)
        {
            var tree = Create<int, int>();
            tree.Add(1, 1);
            using (var enumerator = tree.GetEnumerator(mode))
            {
                enumerator.MoveNext();
                tree.Clear();
                Assert.Throws<InvalidOperationException>(() => enumerator.MoveNext());
            }
        }


        [Test]
        [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void CopyTo_ThrowsException_ForNullArray()
        {
            var tree = Create<int, int>();
            Assert.Throws<ArgumentNullException>(() => tree.CopyTo(null, 0));
        }

        [Test]
        public void CopyTo_ThrowsException_IfIndexIsNegative()
        {
            var tree = Create<int, int>();
            Assert.Throws<ArgumentOutOfRangeException>(() => tree.CopyTo(new KeyValuePair<int, int>[] { }, -1));
        }

        [Test]
        public void CopyTo_ThrowsException_IfNotEnoughSpace1()
        {
            var tree = Create<int, int>();
            tree.Add(1, 1);
            tree.Add(2, 2);
            tree.Add(3, 3);

            Assert.Throws<ArgumentOutOfRangeException>(() => tree.CopyTo(new KeyValuePair<int, int>[3], 1));
        }

        [Test]
        public void CopyTo_ThrowsException_IfNotEnoughSpace2()
        {
            var tree = Create<int, int>();
            tree.Add(1, 1);
            tree.Add(2, 2);
            tree.Add(3, 3);

            Assert.Throws<ArgumentOutOfRangeException>(() => tree.CopyTo(new KeyValuePair<int, int>[2], 0));
        }

        [Test]
        public void CopyTo_CopiesAllItems_ToArray_InOrder()
        {
            var tree = Create<int, int>();
            tree.Add(2, 2);
            tree.Add(1, 1);
            tree.Add(3, 3);

            var array = new KeyValuePair<int, int>[3];

            tree.CopyTo(array, 0);
            TestHelper.AssertSequence(array, 
                new KeyValuePair<int, int>(1, 1), 
                new KeyValuePair<int, int>(2, 2), 
                new KeyValuePair<int, int>(3, 3));
        }

        [Test]
        public void CopyTo_DoesNothing_ForEmptyTree()
        {
            var tree = Create<int, int>();
            var array = new KeyValuePair<int, int>[1];
            tree.CopyTo(array, 0);

            Assert.AreEqual(new KeyValuePair<int, int>(0, 0), array[0]);
        }

        [TestCase(1000, 0, 50)]
        public void Tree_MaintainsBalance(int elements, int min, int max)
        {
            var tree = Create<int, int>();
            var random = new Random(Environment.TickCount);
            var set = new Dictionary<int, int>();
            while (tree.Count < elements)
            {
                elements--;
                var shouldRemove = random.Next(10) < 2;
                var x = random.Next(min, max);

                if (shouldRemove)
                {
                    tree.Remove(x);
                    set.Remove(x);
                }
                else
                {
                    var y = random.Next();
                    tree.AddOrUpdate(x, y);
                    set[x] = y;
                }
            }

            var inTree = tree.ToArray();
            var inSet = set.OrderBy(k => k.Key).Select(s => new KeyValuePair<int, int>(s.Key, s.Value)).ToArray();

            TestHelper.AssertSequence(inTree, inSet);
        }
    }
}