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
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Abacaxi.Containers;
    using NUnit.Framework;

    [TestFixture]
    public sealed class MeanHeapTests
    {
        [SetUp]
        public void SetUp()
        {
            _empty = new MeanHeap<int>(Comparer<int>.Default, Avg);
            _three = new MeanHeap<int>(new[] {2, 3, 1}, Comparer<int>.Default, Avg);
        }

        private static int Avg(int a, int b)
        {
            return (a + b) / 2;
        }

        private MeanHeap<int> _empty;
        private MeanHeap<int> _three;

        [Test]
        public void Add_AddsNewElement_ToEmptyHeap()
        {
            _empty.Add(1);

            Assert.IsTrue(_empty.Contains(1));
        }

        [Test]
        public void Add_DoesNotReorderTheHeap_IfNotAddingBiggestElement()
        {
            _empty.Add(2);
            _empty.Add(1);

            Assert.IsTrue(_empty.Contains(2));
        }

        [Test]
        public void Add_ReordersTheHeap_IfAddingBiggestElement()
        {
            _empty.Add(1);
            _empty.Add(2);

            Assert.IsTrue(_empty.Contains(2));
        }

        [Test]
        public void Clear_WipesTheHeapClean()
        {
            _empty.Add(1);
            _empty.Add(2);
            _empty.Clear();

            TestHelper.AssertSequence(_empty);
        }

        [Test]
        public void Contains_ReturnsFalse_IfElementNotInHeap()
        {
            Assert.IsFalse(_empty.Contains(1));
        }

        [Test]
        public void Contains_ReturnsTrue_IfElementIsInHeap()
        {
            _empty.Add(1);
            Assert.IsTrue(_empty.Contains(1));
        }

        [Test]
        public void Contains_ReturnsTrue_IfElementIsInHeap_AndMean()
        {
            _empty.Add(1);
            _empty.Add(2);
            _empty.Add(3);

            Assert.IsTrue(_empty.Contains(2));
        }

        [Test]
        public void Contains_ReturnsTrue_IfElementIsInHeap_AndNotMean()
        {
            _empty.Add(1);
            _empty.Add(2);
            _empty.Add(3);

            Assert.IsTrue(_empty.Contains(3));
        }

        [Test]
        public void Contains_ReturnsTrue_IfGreatest()
        {
            _empty.Add(100);
            _empty.Add(200);

            Assert.IsTrue(_empty.Contains(200));
        }

        [Test]
        public void Contains_ReturnsTrue_IfSmallest()
        {
            _empty.Add(100);
            _empty.Add(200);

            Assert.IsTrue(_empty.Contains(100));
        }

        [Test]
        public void CopyTo_CopiesAllItems_ToArrayInPriorityOrder()
        {
            var array = new int[3];

            _three.CopyTo(array, 0);
            TestHelper.AssertSequence(array, 3, 2, 1);
        }

        [Test]
        public void CopyTo_DoesNothing_ForEmptyHeap()
        {
            var array = new int[1];
            _empty.CopyTo(array, 0);

            Assert.AreEqual(0, array[0]);
        }


        [Test, SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void CopyTo_ThrowsException_ForNullArray()
        {
            Assert.Throws<ArgumentNullException>(() => _three.CopyTo(null, 0));
        }

        [Test]
        public void CopyTo_ThrowsException_IfIndexIsNegative()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _three.CopyTo(new int[] { }, -1));
        }

        [Test]
        public void CopyTo_ThrowsException_IfNotEnoughSpace1()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _three.CopyTo(new int[3], 1));
        }

        [Test]
        public void CopyTo_ThrowsException_IfNotEnoughSpace2()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _three.CopyTo(new int[2], 0));
        }

        [Test]
        public void Count_DecreasesByOne_WhenElementIsRemoved()
        {
            _empty.Add(1);
            _empty.Remove(1);

            Assert.AreEqual(0, _empty.Count);
        }

        [Test]
        public void Count_IncreasesByOne_WhenElementIsAdded()
        {
            _empty.Add(1);
            Assert.AreEqual(1, _empty.Count);
        }

        [Test, SuppressMessage("ReSharper", "IdentifierTypo")]
        public void Count_IsNotZero_ForHeapifiedCollections()
        {
            var heap = new MeanHeap<int>(new[] {1, 2, 3}, Comparer<int>.Default, Avg);

            Assert.AreEqual(3, heap.Count);
        }

        [Test]
        public void Count_IsZero_ForNewHeap()
        {
            Assert.AreEqual(0, _empty.Count);
        }

        [Test, SuppressMessage("ReSharper", "IdentifierTypo")]
        public void Ctor_Heapifies_TheCollection1()
        {
            var heap = new MeanHeap<int>(new[] {1}, Comparer<int>.Default, Avg);

            TestHelper.AssertSequence(heap,
                1);
        }

        [Test, SuppressMessage("ReSharper", "IdentifierTypo")]
        public void Ctor_Heapifies_TheCollection2()
        {
            var heap = new MeanHeap<int>(new[] {1, 2}, Comparer<int>.Default, Avg);

            TestHelper.AssertSequence(heap,
                2, 1);
        }

        [Test, SuppressMessage("ReSharper", "IdentifierTypo")]
        public void Ctor_Heapifies_TheCollection3()
        {
            var heap = new MeanHeap<int>(new[] {3, 1, 5, 2}, Comparer<int>.Default, Avg);

            TestHelper.AssertSequence(heap,
                5, 3, 2, 1);
        }

        [Test, SuppressMessage("ReSharper", "CollectionNeverUpdated.Local")]
        public void Ctor_InitializesEmptyCollection1()
        {
            var heap = new MeanHeap<int>(Comparer<int>.Default, Avg);

            TestHelper.AssertSequence(heap);
        }

        [Test]
        public void Ctor_InitializesEmptyCollection2()
        {
            var heap = new MeanHeap<int>(new int[] { }, Comparer<int>.Default, Avg);

            TestHelper.AssertSequence(heap);
        }

        [Test, SuppressMessage("ReSharper", "ObjectCreationAsStatement"),
         SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void Ctor_ThrowsException_WhenAvgFuncIsNull1()
        {
            Assert.Throws<ArgumentNullException>(() => new MeanHeap<int>(Comparer<int>.Default, null));
        }

        [Test, SuppressMessage("ReSharper", "ObjectCreationAsStatement"),
         SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void Ctor_ThrowsException_WhenCollectionIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new MeanHeap<int>(null, Comparer<int>.Default, Avg));
        }

        [Test, SuppressMessage("ReSharper", "ObjectCreationAsStatement"),
         SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void Ctor_ThrowsException_WhenComparerIsNull1()
        {
            Assert.Throws<ArgumentNullException>(() => new MeanHeap<int>(null, Avg));
        }


        [Test, SuppressMessage("ReSharper", "ObjectCreationAsStatement"),
         SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void Ctor_ThrowsException_WhenComparerIsNull2()
        {
            Assert.Throws<ArgumentNullException>(() => new MeanHeap<int>(new int[] { }, null, Avg));
        }

        [Test]
        public void Enumeration_ThrowsException_IfAddingElement()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                foreach (var _ in _three)
                {
                    _three.Add(4);
                }
            });
        }

        [Test, SuppressMessage("ReSharper", "GenericEnumeratorNotDisposed")]
        public void GetEnumerator_EnumeratesAllElements()
        {
            var enumerator = _three.GetEnumerator();
            var elements = new List<int>();
            while (enumerator.MoveNext())
            {
                elements.Add(enumerator.Current);
            }

            TestHelper.AssertSequence(elements, 3, 2, 1);
        }

        [Test, SuppressMessage("ReSharper", "GenericEnumeratorNotDisposed")]
        public void GetEnumerator_MoveNext_ReturnsFalse_ForEmptyHeap()
        {
            var enumerator = _empty.GetEnumerator();
            Assert.IsFalse(enumerator.MoveNext());
        }

        [Test]
        public void GetEnumerator_ReturnsNonNullObject()
        {
            var enumerator = _empty.GetEnumerator();
            Assert.NotNull(enumerator);
        }

        [Test]
        public void Heap_AllowsDuplicates()
        {
            var heap = new MeanHeap<int>(new[] {1, 1, 1}, Comparer<int>.Default, Avg);

            TestHelper.AssertSequence(heap, 1, 1, 1);
        }

        [Test]
        public void Heap_TakesComparerIntoAccount()
        {
            var comparer = Comparer<int>.Create((a, b) => b - a);

            var heap = new MeanHeap<int>(new[] {10, 8, 1}, comparer, Avg);

            TestHelper.AssertSequence(heap, 1, 8, 10);
        }

        [Test]
        public void ImplicitGetEnumerator_EnumeratesAllElements_InPriorityOrder()
        {
            var enumerator = ((IEnumerable) _three).GetEnumerator();
            var elements = new List<int>();
            while (enumerator.MoveNext())
            {
                Assert.NotNull(enumerator.Current);
                elements.Add((int) enumerator.Current);
            }

            TestHelper.AssertSequence(elements, 3, 2, 1);
        }

        [Test]
        public void ImplicitGetEnumerator_MoveNext_ReturnsFalse_ForEmptyHeap()
        {
            var enumerator = ((IEnumerable) _empty).GetEnumerator();
            Assert.IsFalse(enumerator.MoveNext());
        }


        [Test]
        public void ImplicitGetEnumerator_ReturnsNonNullObject()
        {
            var enumerator = ((IEnumerable) _empty).GetEnumerator();
            Assert.NotNull(enumerator);
        }

        [Test]
        public void IsReadOnly_ReturnsFalse()
        {
            Assert.IsFalse(_empty.IsReadOnly);
        }

        [Test]
        public void Mean_ActuallyUsesTheAvgFunction()
        {
            var heap = new MeanHeap<int>(Comparer<int>.Default, (a, b) => -1) {1, 2};

            Assert.AreEqual(-1, heap.Mean);
        }

        [Test]
        public void Mean_ReturnsAvg_IfOnlyTwoElementsInHeap()
        {
            _empty.Add(4);
            _empty.Add(2);

            Assert.AreEqual(3, _empty.Mean);
        }

        [Test]
        public void Mean_ReturnsSingleElement_ForOneElementHeap()
        {
            _empty.Add(1);

            Assert.AreEqual(1, _empty.Mean);
        }

        [Test]
        public void Mean_ReturnsTheAvgMeanElements_InHeap()
        {
            _empty.Add(1);
            _empty.Add(2);
            _empty.Add(10);
            _empty.Add(4);
            _empty.Add(8);
            _empty.Add(6);

            Assert.AreEqual(5, _empty.Mean);
        }

        [Test]
        public void Mean_ReturnsTheMeanElement_InHeap()
        {
            _empty.Add(1);
            _empty.Add(2);
            _empty.Add(10);
            _empty.Add(4);
            _empty.Add(8);

            Assert.AreEqual(4, _empty.Mean);
        }

        [Test]
        public void Mean_ThrowsException_ForEmptyHeap()
        {
            Assert.Throws<InvalidOperationException>(() => Assert.AreEqual(0, _empty.Mean));
        }

        [Test]
        public void Remove_ReordersHeap_ForUnbalancedLeft()
        {
            var unb = new MeanHeap<int>(new[] {100, 90, 50, 80, 81, 40, 41}, Comparer<int>.Default, Avg);
            unb.Remove(90);

            TestHelper.AssertSequence(unb, 100, 81, 80, 50, 41, 40);
        }

        [Test]
        public void Remove_ReordersHeap_ForUnbalancedRight()
        {
            var unb = new MeanHeap<int>(new[] {100, 50, 90, 41, 42, 81, 82}, Comparer<int>.Default, Avg);
            unb.Remove(41);

            TestHelper.AssertSequence(unb, 100, 90, 82, 81, 50, 42);
        }

        [Test]
        public void Remove_ReturnsFalse_ForUnknownElement()
        {
            Assert.IsFalse(_empty.Remove(1));
        }

        [Test]
        public void Remove_ReturnsTrue_ForKnownElement()
        {
            _empty.Add(1);
            Assert.IsTrue(_empty.Remove(1));
        }
    }
}