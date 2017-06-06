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

namespace Abacaxi.Tests.Containers
{
    using System;
    using System.Collections.Generic;
    using System.Collections;
    using Abacaxi.Containers;
    using NUnit.Framework;

    [TestFixture]
    public class BitSetTests
    {
        private BitSet _one;
        private BitSet _three;
        private BitSet _full;

        [SetUp]
        public void SetUp()
        {
            _one = new BitSet(1);
            _three = new BitSet(-1, 1);
            _full = new BitSet(4);

            for (var i = 0; i < 4; i++)
            {
                _full.Add(i);
            }
        }

        [Test]
        public void Ctor_ThrowsException_WhenMinGreaterThanMax()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new BitSet(2, 1));
        }

        [Test]
        public void Ctor_ThrowsException_WhenCountIsZero()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new BitSet(0));
        }

        [Test]
        public void IsReadOnly_ReturnsFalse()
        {
            Assert.IsFalse(new BitSet(1).IsReadOnly);
        }

        [Test]
        public void Ctor_CreatesEmptySet()
        {
            Assert.IsFalse(_one.Contains(0));
        }

        [Test]
        public void Add_ReturnsTrue_ForNewElement()
        {
            Assert.IsTrue(_one.Add(0));
        }

        [Test]
        public void Add_ReturnsFalse_ForExistingElement()
        {
            _one.Add(0);

            Assert.IsFalse(_one.Add(0));
        }

        [Test]
        public void Add_ThrowsException_ForElementGreaterThanMax()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _three.Add(2));
        }

        [Test]
        public void Add_ThrowsException_ForElementLessThanMin()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _three.Add(-2));
        }

        [Test]
        public void ImplicitAdd_AddsTheElement()
        {
            ((ICollection<int>)_one).Add(0);
            Assert.IsTrue(_one.Contains(0));
        }

        [Test]
        public void ImplicitAdd_ThrowsException_ForElementGreaterThanMax()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => ((ICollection<int>)_three).Add(2));
        }

        [Test]
        public void ImplicitAdd_ThrowsException_ForElementLessThanMin()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => ((ICollection<int>)_three).Add(-2));
        }

        [Test]
        public void Remove_ReturnsFalse_ForNonExistingElement()
        {
            Assert.IsFalse(_one.Remove(0));
        }

        [Test]
        public void Remove_ReturnsTrue_ForExistingElement()
        {
            Assert.IsTrue(_full.Remove(0));
        }

        [Test]
        public void Remove_RemovesTheElement()
        {
            _full.Remove(0);

            Assert.IsFalse(_full.Contains(0));
        }

        [Test]
        public void Remove_CausesCountToDecrease()
        {
            _full.Remove(1);

            Assert.AreEqual(3, _full.Count);
        }

        [Test]
        public void Remove_ReturnsFalse_ForElementOutofBounds()
        {
            Assert.IsFalse(_one.Remove(-1));
        }

        [Test]
        public void Count_IsZero_ForNewSet()
        {
            Assert.AreEqual(0, _one.Count);
        }

        [Test]
        public void Count_IsFour_ForFourItems()
        {
            Assert.AreEqual(4, _full.Count);
        }

        [Test]
        public void Count_Increases_AfterAddingOneElement()
        {
            _one.Add(0);
            Assert.AreEqual(1, _one.Count);
        }

        [Test]
        public void Count_Decreases_AfterRemovingOneElement()
        {
            _one.Add(0);
            _one.Remove(0);
            Assert.AreEqual(0, _one.Count);
        }

        [Test]
        public void Clear_RemovesAllItems()
        {
            _full.Clear();

            Assert.IsFalse(_full.Contains(0));
            Assert.IsFalse(_full.Contains(1));
            Assert.IsFalse(_full.Contains(2));
            Assert.IsFalse(_full.Contains(3));
        }

        [Test]
        public void Clear_SetsCountToZero()
        {
            _full.Clear();

            Assert.AreEqual(0, _full.Count);
        }

        [Test]
        public void Contains_ReturnsTrue_ForExistingItem()
        {
            Assert.IsTrue(_full.Contains(0));
        }

        [Test]
        public void Contains_ReturnsFalse_ForNonExistingItem()
        {
            Assert.IsFalse(_three.Contains(0));
        }

        [Test]
        public void Contains_ReturnsFalse_ForItemOutOfBounds()
        {
            Assert.IsFalse(_full.Contains(-1));
        }

        [Test]
        public void ExceptWith_ThrowsException_ForNullOther()
        {
            Assert.Throws<ArgumentNullException>(() => _one.ExceptWith(null));
        }

        [Test]
        public void ExceptWith_DoesNothing_ForEmptyOther()
        {
            _full.ExceptWith(new int[] { });

            Assert.IsTrue(_full.Contains(0));
            Assert.IsTrue(_full.Contains(1));
            Assert.IsTrue(_full.Contains(2));
            Assert.IsTrue(_full.Contains(3));
        }

        [Test]
        public void ExceptWith_RemovesElements_EqualToOther()
        {
            _full.ExceptWith(new[] { 0, 1, 2, -8 });

            Assert.IsFalse(_full.Contains(0));
            Assert.IsFalse(_full.Contains(1));
            Assert.IsFalse(_full.Contains(2));
            Assert.IsTrue(_full.Contains(3));
        }

        [Test]
        public void SymmetricExceptWith_ThrowsException_ForNullOther()
        {
            Assert.Throws<ArgumentNullException>(() => _one.SymmetricExceptWith(null));
        }

        [Test]
        public void SymmetricExceptWith_DoesNothing_ForEmptyOther()
        {
            _full.SymmetricExceptWith(new int[] { });

            Assert.IsTrue(_full.Contains(0));
            Assert.IsTrue(_full.Contains(1));
            Assert.IsTrue(_full.Contains(2));
            Assert.IsTrue(_full.Contains(3));
        }

        [Test]
        public void SymmetricExceptWith_LeavesTheDifferenceFromBoth()
        {
            var set = new BitSet(10)
            {
                1,
                2,
                3
            };
            set.SymmetricExceptWith(new[] { 1, 2, 4 });

            Assert.IsFalse(set.Contains(1));
            Assert.IsFalse(set.Contains(2));
            Assert.IsTrue(set.Contains(3));
            Assert.IsTrue(set.Contains(4));
        }

        [Test]
        public void SymmetricExceptWith_ThrowsException_TryingToAddElementLessThanMin()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => 
            _full.SymmetricExceptWith(new[] { 0, -1 }));
        }

        [Test]
        public void SymmetricExceptWith_ThrowsException_TryingToAddElementGreaterThanMax()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            _full.SymmetricExceptWith(new[] { 0, 4 }));
        }

        [Test]
        public void UnionWith_ThrowsException_ForNullOther()
        {
            Assert.Throws<ArgumentNullException>(() => _one.UnionWith(null));
        }

        [Test]
        public void UnionWith_DoesNothing_ForEmptyOther()
        {
            _full.UnionWith(new int[] { });

            Assert.IsTrue(_full.Contains(0));
            Assert.IsTrue(_full.Contains(1));
            Assert.IsTrue(_full.Contains(2));
            Assert.IsTrue(_full.Contains(3));
        }

        [Test]
        public void UnionWith_KeepsElementsFromBothSets()
        {
            var set = new BitSet(100)
            {
                1,
                2,
                3
            };
            set.UnionWith(new[] { 1, 2, 4 });

            Assert.IsTrue(set.Contains(1));
            Assert.IsTrue(set.Contains(2));
            Assert.IsTrue(set.Contains(3));
            Assert.IsTrue(set.Contains(4));
        }

        [Test]
        public void UnionWith_ThrowsException_TryingToAddElementLessThanMin()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            _full.UnionWith(new[] { 0, -1 }));
        }

        [Test]
        public void UnionWith_ThrowsException_TryingToAddElementGreaterThanMax()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            _full.UnionWith(new[] { 0, 4 }));
        }

        [Test]
        public void IntersectWith_ThrowsException_ForNullOther()
        {
            Assert.Throws<ArgumentNullException>(() => _one.IntersectWith(null));
        }

        [Test]
        public void IntersectWith_ClearsSet_ForEmptyOther()
        {
            _full.IntersectWith(new int[] { });

            Assert.IsFalse(_full.Contains(0));
            Assert.IsFalse(_full.Contains(1));
            Assert.IsFalse(_full.Contains(2));
            Assert.IsFalse(_full.Contains(3));
        }

        [Test]
        public void IntersectWith_RemovesElements_NotInOther()
        {
            _full.IntersectWith(new[] { 0, -7 });

            Assert.IsTrue(_full.Contains(0));
            Assert.IsFalse(_full.Contains(1));
            Assert.IsFalse(_full.Contains(2));
            Assert.IsFalse(_full.Contains(3));
        }

        [Test]
        public void IsProperSubsetOf_ThrowsException_ForNullOther()
        {
            Assert.Throws<ArgumentNullException>(() => _one.IsProperSubsetOf(null));
        }

        [Test]
        public void IsProperSubsetOf_ReturnsFalse_ForEmptyOther()
        {
            Assert.IsFalse(_full.IsProperSubsetOf(new int[] { }));
        }

        [Test]
        public void IsProperSubsetOf_ReturnsFalse_IfNotSubset()
        {
            Assert.IsFalse(_full.IsProperSubsetOf(new[] { 3, 2, 1, 8 }));
        }

        [Test]
        public void IsProperSubsetOf_ReturnsFalse_IfNotProperSubset()
        {
            Assert.IsFalse(_full.IsProperSubsetOf(new[] { 3, 2, 1, 0 }));
        }

        [Test]
        public void IsProperSubsetOf_ReturnsTrue_IfProperSubset()
        {
            Assert.IsTrue(_full.IsProperSubsetOf(new[] { 8, 3, 2, 1, 0 }));
        }

        [Test]
        public void IsSubsetOf_ThrowsException_ForNullOther()
        {
            Assert.Throws<ArgumentNullException>(() => _one.IsSubsetOf(null));
        }

        [Test]
        public void IsSubsetOf_ReturnsFalse_ForEmptyOther()
        {
            Assert.IsFalse(_full.IsSubsetOf(new int[] { }));
        }

        [Test]
        public void IsSubsetOf_ReturnsFalse_IfNotSubset()
        {
            Assert.IsFalse(_full.IsSubsetOf(new[] { 3, 2, 1, 8 }));
        }

        [Test]
        public void IsSubsetOf_ReturnsTrue_IfEqualSet()
        {
            Assert.IsTrue(_full.IsSubsetOf(new[] { 3, 2, 1, 0 }));
        }

        [Test]
        public void IsSubsetOf_ReturnsTrue_IfProperSubset()
        {
            Assert.IsTrue(_full.IsSubsetOf(new[] { 3, 2, 1, 0, -1 }));
        }

        [Test]
        public void IsProperSupersetOf_ThrowsException_ForNullOther()
        {
            Assert.Throws<ArgumentNullException>(() => _one.IsProperSupersetOf(null));
        }

        [Test]
        public void IsProperSupersetOf_ReturnsFalse_ForEmptyOther()
        {
            Assert.IsFalse(_full.IsProperSubsetOf(new int[] { }));
        }

        [Test]
        public void IsProperSupersetOf_ReturnsFalse_IfNotSuperset1()
        {
            Assert.IsFalse(_full.IsProperSupersetOf(new[] { 8 }));
        }

        [Test]
        public void IsProperSupersetOf_ReturnsFalse_IfNotSuperset2()
        {
            Assert.IsFalse(_full.IsProperSupersetOf(new[] { 0, 1, 2, 3, 8 }));
        }

        [Test]
        public void IsProperSupersetOf_ReturnsFalse_IfNotProperSuperset()
        {
            Assert.IsFalse(_full.IsProperSupersetOf(new[] { 3, 2, 1, 0 }));
        }

        [Test]
        public void IsProperSupersetOf_ReturnsTrue_IfProperSuperset()
        {
            Assert.IsTrue(_full.IsProperSupersetOf(new[] { 3, 2, 1 }));
        }

        [Test]
        public void IsSupersetOf_ThrowsException_ForNullOther()
        {
            Assert.Throws<ArgumentNullException>(() => _one.IsSupersetOf(null));
        }

        [Test]
        public void IsSupersetOf_ReturnsTrue_ForEmptyOther()
        {
            Assert.IsTrue(_full.IsSupersetOf(new int[] { }));
        }

        [Test]
        public void IsSupersetOf_ReturnsFalse_IfNotSuperset1()
        {
            Assert.IsFalse(_full.IsSupersetOf(new[] { 3, 2, 1, 8 }));
        }

        [Test]
        public void IsSupersetOf_ReturnsFalse_IfNotSuperset2()
        {
            Assert.IsFalse(_full.IsSupersetOf(new[] { 3, 2, 1, 0, 8 }));
        }

        [Test]
        public void IsSupersetOf_ReturnsTrue_IfEqualSet()
        {
            Assert.IsTrue(_full.IsSupersetOf(new[] { 3, 2, 1, 0 }));
        }

        [Test]
        public void IsSupersetOf_ReturnsTrue_IfProperSuperset()
        {
            Assert.IsTrue(_full.IsSupersetOf(new[] { 3, 2, 0 }));
        }

        [Test]
        public void Overlaps_ThrowsException_ForNullOther()
        {
            Assert.Throws<ArgumentNullException>(() => _one.Overlaps(null));
        }

        [Test]
        public void Overlaps_ReturnsTrue_ForEmptyOther()
        {
            Assert.IsTrue(_full.Overlaps(new int[] { }));
        }

        [Test]
        public void Overlaps_ReturnsFalse_IfNoCommonElements()
        {
            Assert.IsFalse(_full.Overlaps(new[] { 8, 9, 10 }));
        }

        [Test]
        public void Overlaps_ReturnsTrue_IfHaveCommonElements()
        {
            Assert.IsTrue(_full.Overlaps(new[] { 0, -1 }));
        }


        [Test]
        public void SetEquals_ThrowsException_ForNullOther()
        {
            Assert.Throws<ArgumentNullException>(() => _one.SetEquals(null));
        }


        [Test]
        public void SetEquals_ReturnsFalse_ForEmptyOther()
        {
            Assert.IsFalse(_full.SetEquals(new int[] { }));
        }

        [Test]
        public void SetEquals_ReturnsFalse_IfNotSame1()
        {
            Assert.IsFalse(_full.SetEquals(new[] { 0, 1, 2, 3, 4}));
        }

        [Test]
        public void SetEquals_ReturnsFalse_IfNotSame2()
        {
            Assert.IsFalse(_full.SetEquals(new[] { 1, 2, 3, 4 }));
        }

        [Test]
        public void SetEquals_ReturnsTrue_IfSame()
        {
            Assert.IsTrue(_full.SetEquals(new[] { 3, 2, 1, 0 }));
        }

        [Test]
        public void SetEquals_ReturnsTrue_IfSameWithDupalicates()
        {
            Assert.IsTrue(_full.SetEquals(new[] { 3, 2, 1, 0, 0, 3 }));
        }

        [Test]
        public void SetEquals_ReturnsFalse_IfSameCountWithDifferences()
        {
            Assert.IsFalse(_full.SetEquals(new[] { 3, 3, 3, 3 }));
        }

        [Test]
        public void GetEnumerator_ReturnsNonNullObject()
        {
            Assert.NotNull(_one.GetEnumerator());
        }

        [Test]
        public void GetEnumerator_MoveNextIsFalse_ForEmptySet()
        {
            var e = _one.GetEnumerator();
            Assert.IsFalse(e.MoveNext());
        }

        [Test]
        public void GetEnumerator_EnumeratesAllElementInSet()
        {
            var e = _full.GetEnumerator();
            var list = new List<int>();

            while (e.MoveNext())
            {
                list.Add(e.Current);
            }

            Assert.IsTrue(_full.SetEquals(list));
        }

        [Test]
        public void GetEnumerator_ErrsIfSetIsModified1()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                foreach (var i in _full)
                {
                    _full.Remove(i);
                }
            });
        }

        [Test]
        public void GetEnumerator_ErrsIfSetIsModified2()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                _three.Add(-1);
                _three.Add(0);
                foreach (var i in _three)
                {
                    _three.Add(1);
                }
            });
        }

        [Test]
        public void GetEnumerator_ErrsIfSetIsModified3()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                foreach (var i in _full)
                {
                    _full.Clear();
                }
            });
        }

        [Test]
        public void ImplicitGetEnumerator_ReturnsNonNullObject()
        {
            Assert.NotNull(((IEnumerable)_one).GetEnumerator());
        }

        [Test]
        public void ImplicitGetEnumerator_MoveNextIsFalse_ForEmptySet()
        {
            var e = ((IEnumerable)_one).GetEnumerator();
            Assert.IsFalse(e.MoveNext());
        }

        [Test]
        public void ImplicitGetEnumerator_EnumeratesAllElementInSet()
        {
            var e = ((IEnumerable)_full).GetEnumerator();
            var list = new List<int>();

            while (e.MoveNext())
            {
                list.Add((int)e.Current);
            }

            Assert.IsTrue(_full.SetEquals(list));
        }

        [Test]
        public void CopyTo_ThrowsException_ForNullArray()
        {
            Assert.Throws<ArgumentNullException>(() => _one.CopyTo(null, 0));
        }

        [Test]
        public void CopyTo_ThrowsException_IfIndexIsNegative()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _one.CopyTo(new int[] { }, -1));
        }

        [Test]
        public void CopyTo_ThrowsException_IfNotEnoughSpace1()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _full.CopyTo(new int[4], 1));
        }

        [Test]
        public void CopyTo_ThrowsException_IfNotEnoughSpace2()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _full.CopyTo(new int[3], 0));
        }

        [Test]
        public void CopyTo_CopiesAllItems_ToArray()
        {
            var array = new int[4];

            _full.CopyTo(array, 0);
            Assert.IsTrue(_full.SetEquals(array));
        }

        [Test]
        public void CopyTo_DoesNothing_ForEmptySet()
        {
            var array = new int[1];
            _one.CopyTo(array, 0);

            Assert.AreEqual(0, array[0]);
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(31)]
        [TestCase(32)]
        [TestCase(33)]
        [TestCase(63)]
        [TestCase(64)]
        public void Set_IsFilled_Properly(int count)
        {
            var set = new BitSet(count);
            for(var i = 0; i < count; i ++)
            {
                set.Add(i);

                Assert.AreEqual(i + 1, set.Count);
            }

            for (var i = 0; i < count; i++)
            {
                Assert.IsTrue(set.Contains(i));
            }
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(31)]
        [TestCase(32)]
        [TestCase(33)]
        [TestCase(63)]
        [TestCase(64)]
        public void Set_EnumeratesPropely(int count)
        {
            var set = new BitSet(count);
            for (var i = 0; i < count; i++)
            {
                set.Add(i);
            }

            var list = new List<int>();
            foreach (var i in set)
            {
                list.Add(i);
            }

            set.SetEquals(list);
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(31)]
        [TestCase(32)]
        [TestCase(33)]
        [TestCase(63)]
        [TestCase(64)]
        public void Set_CopiestoArrayProperly(int count)
        {
            var set = new BitSet(count);
            for (var i = 0; i < count; i++)
            {
                set.Add(i);
            }

            var array = new int[set.Count];
            set.CopyTo(array, 0);

            set.SetEquals(array);
        }
    }
}
