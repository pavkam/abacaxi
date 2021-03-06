﻿/* Copyright 2017-2019 by Alexandru Ciobanu (alex+git@ciobanu.org)
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
    using System.Linq;
    using Abacaxi.Containers;
    using NUnit.Framework;

    [TestFixture, SuppressMessage("ReSharper", "CollectionNeverUpdated.Local")]
    public sealed class MashTests
    {
        [SetUp]
        public void SetUp()
        {
            _emptyMash = new Mash<string, int>();
            _oneMash = new Mash<string, int> {1};
            _twoMash = new Mash<string, int> {1, 2};
            _threeMash = new Mash<string, int> {1, 2, 3};
        }

        private Mash<string, int> _emptyMash;
        private Mash<string, int> _oneMash;
        private Mash<string, int> _twoMash;
        private Mash<string, int> _threeMash;

        [TestCase(0, false)]
        public void Contains_ReturnsExpectedResult_ForEmpty(int i, bool expected)
        {
            var actual = _emptyMash.Contains(i);
            Assert.AreEqual(expected, actual);
        }

        [TestCase(0, false), TestCase(1, true)]
        public void Contains_ReturnsExpectedResult_ForOne(int i, bool expected)
        {
            var actual = _oneMash.Contains(i);
            Assert.AreEqual(expected, actual);
        }

        [TestCase(0, false), TestCase(1, true), TestCase(2, true)]
        public void Contains_ReturnsExpectedResult_ForTwo(int i, bool expected)
        {
            var actual = _twoMash.Contains(i);
            Assert.AreEqual(expected, actual);
        }

        [TestCase(0, false), TestCase(1, true), TestCase(2, true), TestCase(3, true)]
        public void Contains_ReturnsExpectedResult_ForThree(int i, bool expected)
        {
            var actual = _threeMash.Contains(i);
            Assert.AreEqual(expected, actual);
        }

        [TestCase(0, -1)]
        public void IndexOf_ReturnsExpectedResult_ForEmpty(int i, int expected)
        {
            var actual = _emptyMash.IndexOf(i);
            Assert.AreEqual(expected, actual);
        }

        [TestCase(0, -1), TestCase(1, 0)]
        public void IndexOf_ReturnsExpectedResult_ForOne(int i, int expected)
        {
            var actual = _oneMash.IndexOf(i);
            Assert.AreEqual(expected, actual);
        }

        [TestCase(0, -1), TestCase(1, 0), TestCase(2, 1)]
        public void IndexOf_ReturnsExpectedResult_ForTwo(int i, int expected)
        {
            var actual = _twoMash.IndexOf(i);
            Assert.AreEqual(expected, actual);
        }

        [TestCase(0, -1), TestCase(1, 0), TestCase(2, 1), TestCase(3, 2)]
        public void IndexOf_ReturnsExpectedResult_ForThree(int i, int expected)
        {
            var actual = _threeMash.IndexOf(i);
            Assert.AreEqual(expected, actual);
        }

        [TestCase(-1), TestCase(1)]
        public void Insert_ThrowsException_IfIndexOutOfRange_ForEmpty(int index)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _emptyMash.Insert(index, 0));
        }

        [TestCase(-1), TestCase(2)]
        public void Insert_ThrowsException_IfIndexOutOfRange_ForOne(int index)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _oneMash.Insert(index, 0));
        }

        [TestCase(-1), TestCase(3)]
        public void Insert_ThrowsException_IfIndexOutOfRange_ForTwo(int index)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _twoMash.Insert(index, 0));
        }

        [TestCase(-1), TestCase(4)]
        public void Insert_ThrowsException_IfIndexOutOfRange_ForThree(int index)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _threeMash.Insert(index, 0));
        }

        [TestCase(-1), TestCase(1)]
        public void RemoveAt_ThrowsException_IfIndexOutOfRange_ForEmpty(int index)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _emptyMash.RemoveAt(index));
        }

        [TestCase(-1), TestCase(2)]
        public void RemoveAt_ThrowsException_IfIndexOutOfRange_ForOne(int index)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _oneMash.RemoveAt(index));
        }

        [TestCase(-1), TestCase(3)]
        public void RemoveAt_ThrowsException_IfIndexOutOfRange_ForTwo(int index)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _twoMash.RemoveAt(index));
        }

        [TestCase(-1), TestCase(4)]
        public void RemoveAt_ThrowsException_IfIndexOutOfRange_ForThree(int index)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _threeMash.RemoveAt(index));
        }

        [TestCase(-1), TestCase(1)]
        public void Indexer_Getter_ThrowsException_IfIndexOutOfRange_ForEmpty(int index)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Assert.AreEqual(1, _emptyMash[index]));
        }

        [TestCase(-1), TestCase(2)]
        public void Indexer_Getter_ThrowsException_IfIndexOutOfRange_ForOne(int index)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Assert.AreEqual(1, _oneMash[index]));
        }

        [TestCase(-1), TestCase(3)]
        public void Indexer_Getter_ThrowsException_IfIndexOutOfRange_ForTwo(int index)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Assert.AreEqual(1, _twoMash[index]));
        }

        [TestCase(-1), TestCase(4)]
        public void Indexer_Getter_ThrowsException_IfIndexOutOfRange_ForThree(int index)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Assert.AreEqual(1, _threeMash[index]));
        }

        [TestCase(0)]
        public void Indexer_Getter_ReturnsTheExpectedValue_ForOne(int index)
        {
            var all = _oneMash.ToArray();
            Assert.AreEqual(all[index], _oneMash[index]);
        }

        [TestCase(0), TestCase(1)]
        public void Indexer_Getter_ReturnsTheExpectedValue_ForTwo(int index)
        {
            var all = _twoMash.ToArray();
            Assert.AreEqual(all[index], _twoMash[index]);
        }

        [TestCase(0), TestCase(1), TestCase(2)]
        public void Indexer_Getter_ReturnsTheExpectedValue_ForThree(int index)
        {
            var all = _threeMash.ToArray();
            Assert.AreEqual(all[index], _threeMash[index]);
        }

        [TestCase(-1), TestCase(1)]
        public void Indexer_Setter_ThrowsException_IfIndexOutOfRange_ForEmpty(int index)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _emptyMash[index] = 1);
        }

        [TestCase(-1), TestCase(2)]
        public void Indexer_Setter_ThrowsException_IfIndexOutOfRange_ForOne(int index)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _oneMash[index] = 1);
        }

        [TestCase(-1), TestCase(3)]
        public void Indexer_Setter_ThrowsException_IfIndexOutOfRange_ForTwo(int index)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _twoMash[index] = 1);
        }

        [TestCase(-1), TestCase(4)]
        public void Indexer_Setter_ThrowsException_IfIndexOutOfRange_ForThree(int index)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _threeMash[index] = 1);
        }

        [TestCase(0)]
        public void Indexer_Setter_SetsTheExpectedValue_ForOne(int index)
        {
            var all = _oneMash.ToArray();
            all[index] = -1;
            _oneMash[index] = -1;
            TestHelper.AssertSequence(_oneMash, all);
        }

        [TestCase(0), TestCase(1)]
        public void Indexer_Setter_SetsTheExpectedValue_ForTwo(int index)
        {
            var all = _twoMash.ToArray();
            all[index] = -1;
            _twoMash[index] = -1;
            TestHelper.AssertSequence(_twoMash, all);
        }

        [TestCase(0), TestCase(1), TestCase(2)]
        public void Indexer_Setter_SetsTheExpectedValue_ForThree(int index)
        {
            var all = _threeMash.ToArray();
            all[index] = -1;
            _threeMash[index] = -1;
            TestHelper.AssertSequence(_threeMash, all);
        }

        [TestCase(0, 6), TestCase(1, 6), TestCase(2, 6), TestCase(3, 6), TestCase(4, 6), TestCase(5, 6), TestCase(0, 5),
         TestCase(1, 5), TestCase(2, 5), TestCase(3, 5), TestCase(4, 5), TestCase(0, 4), TestCase(1, 4), TestCase(2, 4),
         TestCase(3, 4), TestCase(0, 3), TestCase(1, 3), TestCase(2, 3), TestCase(0, 2), TestCase(1, 2), TestCase(0, 1)]
        public void Unlink_RemovesTheExpectedElement(int index, int count)
        {
            var mash = new Mash<int, string>();
            var ms = new Mash<int, string>[count];
            for (var i = 0; i < count; i++)
            {
                ms[i] = new Mash<int, string>();
                mash.Link(i, ms[i]);
            }

            mash.Unlink(index);

            for (var i = 0; i < count; i++)
            {
                if (i != index)
                {
                    Assert.AreSame(ms[i], mash.GetLinked(i));
                }
            }

            Assert.AreNotSame(ms[index], mash.GetLinked(index));
        }

        [Test]
        public void Add_AppendAnElement()
        {
            _emptyMash.Add(1);
            TestHelper.AssertSequence(_emptyMash, 1);
        }

        [Test]
        public void Add_IncrementCountByOne()
        {
            _emptyMash.Add(1);
            Assert.AreEqual(1, _emptyMash.Count);
        }

        [Test]
        public void Add_PreservesInternalStructure()
        {
            _emptyMash.Add(1);
            TestHelper.AssertSequence(_emptyMash, 1);
            _emptyMash.Add(2);
            TestHelper.AssertSequence(_emptyMash, 1, 2);
            _emptyMash.Add(3);
            TestHelper.AssertSequence(_emptyMash, 1, 2, 3);
            _emptyMash.Add(4);
            TestHelper.AssertSequence(_emptyMash, 1, 2, 3, 4);
        }

        [Test]
        public void Clear_RemovesAllItems()
        {
            _threeMash.Clear();
            TestHelper.AssertSequence(_threeMash);
        }

        [Test]
        public void Clear_SetsCountToZero()
        {
            _threeMash.Clear();
            Assert.AreEqual(0, _threeMash.Count);
        }

        [Test]
        public void CopyTo_CopiesAllItems_ToArray_ForOne()
        {
            var array = new int[1];

            _oneMash.CopyTo(array, 0);
            TestHelper.AssertSequence(array, 1);
        }

        [Test]
        public void CopyTo_CopiesAllItems_ToArray_ForThree()
        {
            var array = new int[3];

            _threeMash.CopyTo(array, 0);
            TestHelper.AssertSequence(array, 1, 2, 3);
        }

        [Test]
        public void CopyTo_CopiesAllItems_ToArray_ForTwo()
        {
            var array = new int[2];

            _twoMash.CopyTo(array, 0);
            TestHelper.AssertSequence(array, 1, 2);
        }

        [Test]
        public void CopyTo_DoesNothing_ForEmpty()
        {
            var array = new int[1];
            _emptyMash.CopyTo(array, 0);

            Assert.AreEqual(0, array[0]);
        }

        [Test, SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void CopyTo_ThrowsException_ForNullArray()
        {
            Assert.Throws<ArgumentNullException>(() => _oneMash.CopyTo(null, 0));
        }

        [Test]
        public void CopyTo_ThrowsException_IfIndexIsNegative()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _oneMash.CopyTo(new int[] { }, -1));
        }

        [Test]
        public void CopyTo_ThrowsException_IfNotEnoughSpace1()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _threeMash.CopyTo(new int[3], 1));
        }

        [Test]
        public void CopyTo_ThrowsException_IfNotEnoughSpace2()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _threeMash.CopyTo(new int[2], 0));
        }

        [Test]
        public void Count_IsValidInContextOfDifferentInternalStructures()
        {
            Assert.AreEqual(0, _emptyMash.Count);
            _emptyMash.Add(1);
            Assert.AreEqual(1, _emptyMash.Count);
            _emptyMash.Add(2);
            Assert.AreEqual(2, _emptyMash.Count);
            _emptyMash.Add(3);
            Assert.AreEqual(3, _emptyMash.Count);
            _emptyMash.Add(4);
            Assert.AreEqual(4, _emptyMash.Count);
        }

        [Test]
        public void Ctor_TakesIntoAccountTheEqualityComparer()
        {
            var mash = new Mash<string, int>(StringComparer.OrdinalIgnoreCase);

            Assert.AreSame(mash["A"], mash["a"]);
        }

        [Test, SuppressMessage("ReSharper", "AssignNullToNotNullAttribute"),
         SuppressMessage("ReSharper", "ObjectCreationAsStatement")]
        public void Ctor_ThrowsException_IfEqualityComparerIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new Mash<string, int>(null));
        }

        [Test]
        public void Ctor_UsesDefaultComparer_IfNotSpecified()
        {
            var mash = new Mash<string, int>();

            Assert.AreNotSame(mash["A"], mash["a"]);
        }

        [Test, SuppressMessage("ReSharper", "LoopCanBeConvertedToQuery")]
        public void GetEnumerator_ReturnsElements_ForOne()
        {
            var result = new List<int>();
            foreach (var i in _oneMash)
            {
                result.Add(i);
            }

            TestHelper.AssertSequence(result, 1);
        }

        [Test, SuppressMessage("ReSharper", "LoopCanBeConvertedToQuery")]
        public void GetEnumerator_ReturnsElements_ForThree()
        {
            var result = new List<int>();
            foreach (var i in _threeMash)
            {
                result.Add(i);
            }

            TestHelper.AssertSequence(result, 1, 2, 3);
        }

        [Test, SuppressMessage("ReSharper", "LoopCanBeConvertedToQuery")]
        public void GetEnumerator_ReturnsElements_ForTwo()
        {
            var result = new List<int>();
            foreach (var i in _twoMash)
            {
                result.Add(i);
            }

            TestHelper.AssertSequence(result, 1, 2);
        }

        [Test, SuppressMessage("ReSharper", "LoopCanBeConvertedToQuery")]
        public void GetEnumerator_ReturnsNothing_ForEmpty()
        {
            var result = new List<int>();
            foreach (var i in _emptyMash)
            {
                result.Add(i);
            }

            TestHelper.AssertSequence(result);
        }

        [Test]
        public void GetLinked_IsTheSameAs_MashIndexer_Getter()
        {
            var mash = new Mash<string, int>();
            Assert.AreSame(mash["a"], mash.GetLinked("a"));
            Assert.AreSame(mash.GetLinked("b"), mash["b"]);
        }

        [Test, SuppressMessage("ReSharper", "LoopCanBeConvertedToQuery")]
        public void Implicit_GetEnumerator_FunctionsAsExpected()
        {
            var result = new List<int>();
            foreach (var i in (IEnumerable) _threeMash)
            {
                result.Add((int) i);
            }

            TestHelper.AssertSequence(result, 1, 2, 3);
        }

        [Test]
        public void Insert_AppendsItem_IfIndexEqualsToLength_ForEmpty()
        {
            _emptyMash.Insert(0, 10);
            TestHelper.AssertSequence(_emptyMash, 10);
        }

        [Test]
        public void Insert_AppendsItem_IfIndexEqualsToLength_ForOne()
        {
            _oneMash.Insert(1, 10);
            TestHelper.AssertSequence(_oneMash, 1, 10);
        }

        [Test]
        public void Insert_AppendsItem_IfIndexEqualsToLength_ForThree()
        {
            _threeMash.Insert(3, 10);
            TestHelper.AssertSequence(_threeMash, 1, 2, 3, 10);
        }

        [Test]
        public void Insert_AppendsItem_IfIndexEqualsToLength_ForTwo()
        {
            _twoMash.Insert(2, 10);
            TestHelper.AssertSequence(_twoMash, 1, 2, 10);
        }

        [Test]
        public void Insert_IncrementsCountByOne()
        {
            _emptyMash.Insert(0, 1);
            Assert.AreEqual(1, _emptyMash.Count);
        }

        [Test]
        public void Insert_PrependsItem_IfIndexEqualsToZero_ForOne()
        {
            _oneMash.Insert(0, 10);
            TestHelper.AssertSequence(_oneMash, 10, 1);
        }

        [Test]
        public void Insert_PrependsItem_IfIndexEqualsToZero_ForThree()
        {
            _threeMash.Insert(0, 10);
            TestHelper.AssertSequence(_threeMash, 10, 1, 2, 3);
        }

        [Test]
        public void Insert_PrependsItem_IfIndexEqualsToZero_ForTwo()
        {
            _twoMash.Insert(0, 10);
            TestHelper.AssertSequence(_twoMash, 10, 1, 2);
        }

        [Test]
        public void Insert_PreservesStructure()
        {
            _emptyMash.Insert(0, 10);
            TestHelper.AssertSequence(_emptyMash, 10);
            _emptyMash.Insert(1, 20);
            TestHelper.AssertSequence(_emptyMash, 10, 20);
            _emptyMash.Insert(0, 30);
            TestHelper.AssertSequence(_emptyMash, 30, 10, 20);
        }

        [Test]
        public void IsReadOnly_ReturnsFalse()
        {
            Assert.IsFalse(_emptyMash.IsReadOnly);
        }

        [Test]
        public void Link_AddsTheMash_IfNew()
        {
            _emptyMash.Link("a", _threeMash);

            Assert.AreSame(_threeMash, _emptyMash["a"]);
        }

        [Test]
        public void Link_DoesNotIncrementLinkedCount_IfReplaced()
        {
            _emptyMash.Link("a", _threeMash);
            _emptyMash.Link("a", _twoMash);

            Assert.AreEqual(1, _emptyMash.LinkedCount);
        }

        [Test]
        public void Link_IncrementsLinkedCount_IfAdded()
        {
            _emptyMash.Link("a", _threeMash);

            Assert.AreEqual(1, _emptyMash.LinkedCount);
        }

        [Test]
        public void Link_MaintainsState_OnAdd()
        {
            var mash = new Mash<string, int>();
            var m1 = new Mash<string, int>();
            var m2 = new Mash<string, int>();
            var m3 = new Mash<string, int>();
            var m4 = new Mash<string, int>();
            var m5 = new Mash<string, int>();
            var m6 = new Mash<string, int>();

            mash.Link("1", m1);
            Assert.AreSame(m1, mash["1"]);

            mash.Link("2", m2);
            Assert.AreSame(m1, mash["1"]);
            Assert.AreSame(m2, mash["2"]);

            mash.Link("3", m3);
            Assert.AreSame(m1, mash["1"]);
            Assert.AreSame(m2, mash["2"]);
            Assert.AreSame(m3, mash["3"]);

            mash.Link("4", m4);
            Assert.AreSame(m1, mash["1"]);
            Assert.AreSame(m2, mash["2"]);
            Assert.AreSame(m3, mash["3"]);
            Assert.AreSame(m4, mash["4"]);

            mash.Link("5", m5);
            Assert.AreSame(m1, mash["1"]);
            Assert.AreSame(m2, mash["2"]);
            Assert.AreSame(m3, mash["3"]);
            Assert.AreSame(m4, mash["4"]);
            Assert.AreSame(m5, mash["5"]);

            mash.Link("6", m6);
            Assert.AreSame(m1, mash["1"]);
            Assert.AreSame(m2, mash["2"]);
            Assert.AreSame(m3, mash["3"]);
            Assert.AreSame(m4, mash["4"]);
            Assert.AreSame(m5, mash["5"]);
            Assert.AreSame(m6, mash["6"]);
        }

        [Test]
        public void Link_MaintainsState_OnReplace_1()
        {
            var mash = new Mash<string, int>();
            var m1 = new Mash<string, int>();
            var shunt = new Mash<string, int>();

            mash.Link("1", m1);
            mash.Link("1", shunt);
            Assert.AreSame(shunt, mash["1"]);
        }

        [Test]
        public void Link_MaintainsState_OnReplace_2()
        {
            var mash = new Mash<string, int>();
            var m1 = new Mash<string, int>();
            var m2 = new Mash<string, int>();
            var shunt = new Mash<string, int>();

            mash.Link("1", m1);
            mash.Link("2", m2);
            mash.Link("1", shunt);
            Assert.AreSame(shunt, mash["1"]);
            Assert.AreSame(m2, mash["2"]);
            mash.Link("2", shunt);
            Assert.AreSame(shunt, mash["2"]);
        }

        [Test]
        public void Link_MaintainsState_OnReplace_3()
        {
            var mash = new Mash<string, int>();
            var m1 = new Mash<string, int>();
            var m2 = new Mash<string, int>();
            var m3 = new Mash<string, int>();
            var shunt = new Mash<string, int>();

            mash.Link("1", m1);
            mash.Link("2", m2);
            mash.Link("3", m3);
            mash.Link("1", shunt);
            Assert.AreSame(shunt, mash["1"]);
            Assert.AreSame(m2, mash["2"]);
            Assert.AreSame(m3, mash["3"]);
            mash.Link("2", shunt);
            Assert.AreSame(shunt, mash["2"]);
            Assert.AreSame(m3, mash["3"]);
            Assert.AreSame(m3, mash["3"]);
            mash.Link("3", shunt);
            Assert.AreSame(shunt, mash["3"]);
        }

        [Test]
        public void Link_MaintainsState_OnReplace_4()
        {
            var mash = new Mash<string, int>();
            var m1 = new Mash<string, int>();
            var m2 = new Mash<string, int>();
            var m3 = new Mash<string, int>();
            var m4 = new Mash<string, int>();
            var shunt = new Mash<string, int>();

            mash.Link("1", m1);
            mash.Link("2", m2);
            mash.Link("3", m3);
            mash.Link("4", m4);
            mash.Link("1", shunt);
            Assert.AreSame(shunt, mash["1"]);
            Assert.AreSame(m2, mash["2"]);
            Assert.AreSame(m3, mash["3"]);
            Assert.AreSame(m4, mash["4"]);
            mash.Link("2", shunt);
            Assert.AreSame(shunt, mash["2"]);
            Assert.AreSame(m3, mash["3"]);
            Assert.AreSame(m4, mash["4"]);
            mash.Link("3", shunt);
            Assert.AreSame(shunt, mash["3"]);
            Assert.AreSame(m4, mash["4"]);
            mash.Link("4", shunt);
            Assert.AreSame(shunt, mash["4"]);
        }

        [Test]
        public void Link_MaintainsState_OnReplace_5()
        {
            var mash = new Mash<string, int>();
            var m1 = new Mash<string, int>();
            var m2 = new Mash<string, int>();
            var m3 = new Mash<string, int>();
            var m4 = new Mash<string, int>();
            var m5 = new Mash<string, int>();
            var shunt = new Mash<string, int>();

            mash.Link("1", m1);
            mash.Link("2", m2);
            mash.Link("3", m3);
            mash.Link("4", m4);
            mash.Link("5", m5);
            mash.Link("1", shunt);
            Assert.AreSame(shunt, mash["1"]);
            Assert.AreSame(m2, mash["2"]);
            Assert.AreSame(m3, mash["3"]);
            Assert.AreSame(m4, mash["4"]);
            Assert.AreSame(m5, mash["5"]);
            mash.Link("2", shunt);
            Assert.AreSame(shunt, mash["2"]);
            Assert.AreSame(m3, mash["3"]);
            Assert.AreSame(m4, mash["4"]);
            Assert.AreSame(m5, mash["5"]);
            mash.Link("3", shunt);
            Assert.AreSame(shunt, mash["3"]);
            Assert.AreSame(m4, mash["4"]);
            Assert.AreSame(m5, mash["5"]);
            mash.Link("4", shunt);
            Assert.AreSame(shunt, mash["4"]);
            Assert.AreSame(m5, mash["5"]);
            mash.Link("5", shunt);
            Assert.AreSame(shunt, mash["5"]);
        }

        [Test]
        public void Link_MaintainsState_OnReplace_6()
        {
            var mash = new Mash<string, int>();
            var m1 = new Mash<string, int>();
            var m2 = new Mash<string, int>();
            var m3 = new Mash<string, int>();
            var m4 = new Mash<string, int>();
            var m5 = new Mash<string, int>();
            var m6 = new Mash<string, int>();
            var shunt = new Mash<string, int>();

            mash.Link("1", m1);
            mash.Link("2", m2);
            mash.Link("3", m3);
            mash.Link("4", m4);
            mash.Link("5", m5);
            mash.Link("6", m6);
            mash.Link("1", shunt);
            Assert.AreSame(shunt, mash["1"]);
            Assert.AreSame(m2, mash["2"]);
            Assert.AreSame(m3, mash["3"]);
            Assert.AreSame(m4, mash["4"]);
            Assert.AreSame(m5, mash["5"]);
            Assert.AreSame(m6, mash["6"]);
            mash.Link("2", shunt);
            Assert.AreSame(shunt, mash["2"]);
            Assert.AreSame(m3, mash["3"]);
            Assert.AreSame(m4, mash["4"]);
            Assert.AreSame(m5, mash["5"]);
            Assert.AreSame(m6, mash["6"]);
            mash.Link("3", shunt);
            Assert.AreSame(shunt, mash["3"]);
            Assert.AreSame(m4, mash["4"]);
            Assert.AreSame(m5, mash["5"]);
            Assert.AreSame(m6, mash["6"]);
            mash.Link("4", shunt);
            Assert.AreSame(shunt, mash["4"]);
            Assert.AreSame(m5, mash["5"]);
            Assert.AreSame(m6, mash["6"]);
            mash.Link("5", shunt);
            Assert.AreSame(shunt, mash["5"]);
            Assert.AreSame(m6, mash["6"]);
            mash.Link("6", shunt);
            Assert.AreSame(shunt, mash["6"]);
        }

        [Test]
        public void Link_ReplacesTheMash_IfExists()
        {
            _emptyMash.Link("a", _threeMash);
            _emptyMash.Link("a", _twoMash);

            Assert.AreSame(_twoMash, _emptyMash["a"]);
        }

        [Test, SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void Link_ThrowsException_IfKeyIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => _emptyMash.Link(null, _threeMash));
        }

        [Test, SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void Link_ThrowsException_IfMashIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => _emptyMash.Link("a", null));
        }

        [Test, SuppressMessage("ReSharper", "UnusedVariable")]
        public void LinkedCount_IsIncremented_WhenASubMashIsAccessed()
        {
            var mash = new Mash<string, int>();
            var sub = mash["A"];
            Assert.AreEqual(1, mash.LinkedCount);
        }

        [Test, SuppressMessage("ReSharper", "NotAccessedVariable"), SuppressMessage("ReSharper", "RedundantAssignment")]
        public void LinkedCount_IsNotIncremented_WhenExistingSubMashIsAccessed()
        {
            var mash = new Mash<string, int>();
            var sub = mash["A"];
            sub = mash["A"];

            Assert.AreEqual(1, mash.LinkedCount);
        }

        [Test, SuppressMessage("ReSharper", "UnusedVariable")]
        public void LinkedCount_IsProperlyTranslatedInternalState()
        {
            var mash = new Mash<string, int>();
            var sub1 = mash["1"];
            Assert.AreEqual(1, mash.LinkedCount);

            var sub2 = mash["2"];
            Assert.AreEqual(2, mash.LinkedCount);

            var sub3 = mash["3"];
            Assert.AreEqual(3, mash.LinkedCount);

            var sub4 = mash["4"];
            Assert.AreEqual(4, mash.LinkedCount);

            var sub5 = mash["5"];
            Assert.AreEqual(5, mash.LinkedCount);

            var sub6 = mash["6"];
            Assert.AreEqual(6, mash.LinkedCount);
        }

        [Test]
        public void LinkedCount_IsZero_NewMash()
        {
            var mash = new Mash<string, int>();
            Assert.AreEqual(0, mash.LinkedCount);
        }

        [Test]
        public void MashIndexer_Getter_ProperlyTranslatedInternalState()
        {
            var mash = new Mash<string, int>();
            var sub1 = mash["1"];
            Assert.AreSame(sub1, mash["1"]);

            var sub2 = mash["2"];
            Assert.AreSame(sub1, mash["1"]);
            Assert.AreSame(sub2, mash["2"]);
            var sub3 = mash["3"];
            Assert.AreSame(sub1, mash["1"]);
            Assert.AreSame(sub2, mash["2"]);
            Assert.AreSame(sub3, mash["3"]);
            var sub4 = mash["4"];
            Assert.AreSame(sub1, mash["1"]);
            Assert.AreSame(sub2, mash["2"]);
            Assert.AreSame(sub3, mash["3"]);
            Assert.AreSame(sub4, mash["4"]);
            var sub5 = mash["5"];
            Assert.AreSame(sub1, mash["1"]);
            Assert.AreSame(sub2, mash["2"]);
            Assert.AreSame(sub3, mash["3"]);
            Assert.AreSame(sub4, mash["4"]);
            Assert.AreSame(sub5, mash["5"]);
            var sub6 = mash["6"];
            Assert.AreSame(sub1, mash["1"]);
            Assert.AreSame(sub2, mash["2"]);
            Assert.AreSame(sub3, mash["3"]);
            Assert.AreSame(sub4, mash["4"]);
            Assert.AreSame(sub5, mash["5"]);
            Assert.AreSame(sub6, mash["6"]);
        }

        [Test]
        public void MashIndexer_Getter_ReturnsANewlyCreatedMash_IfNotRegistered()
        {
            var mash = new Mash<string, int>();

            Assert.IsNotNull(mash["test"]);
        }

        [Test]
        public void MashIndexer_Getter_ReturnsExistingMash_IfRegistered()
        {
            var mash = new Mash<string, int>();
            var sub = mash["test"];
            Assert.AreSame(sub, mash["test"]);
        }

        [Test]
        public void Remove_DecrementsCount_IfElementWasRemoved()
        {
            _threeMash.Remove(3);
            Assert.AreEqual(2, _threeMash.Count);
        }

        [Test]
        public void Remove_PreservesInternalStructure()
        {
            _threeMash.Remove(1);
            TestHelper.AssertSequence(_threeMash, 2, 3);
            _threeMash.Remove(3);
            TestHelper.AssertSequence(_threeMash, 2);
            _threeMash.Remove(2);
            TestHelper.AssertSequence(_threeMash);
        }

        [Test]
        public void Remove_ReturnsFalse_IfItemNotFound_ForEmpty()
        {
            Assert.IsFalse(_emptyMash.Remove(0));
        }

        [Test]
        public void Remove_ReturnsFalse_IfItemNotFound_ForOne()
        {
            Assert.IsFalse(_oneMash.Remove(0));
        }

        [Test]
        public void Remove_ReturnsFalse_IfItemNotFound_ForThree()
        {
            Assert.IsFalse(_threeMash.Remove(0));
        }

        [Test]
        public void Remove_ReturnsFalse_IfItemNotFound_ForTwo()
        {
            Assert.IsFalse(_twoMash.Remove(0));
        }

        [Test]
        public void Remove_ReturnsTrue_IfItemFound_ForOne()
        {
            Assert.IsTrue(_oneMash.Remove(1));
        }

        [Test]
        public void Remove_ReturnsTrue_IfItemFound_ForTwo()
        {
            Assert.IsTrue(_twoMash.Remove(2));
        }

        [Test]
        public void RemoveAt_DecrementsCountByOne()
        {
            _threeMash.RemoveAt(0);
            Assert.AreEqual(2, _threeMash.Count);
        }

        [Test]
        public void RemoveAt_PreservesStructure()
        {
            _threeMash.RemoveAt(0);
            TestHelper.AssertSequence(_threeMash, 2, 3);
            _threeMash.RemoveAt(0);
            TestHelper.AssertSequence(_threeMash, 3);
            _threeMash.RemoveAt(0);
            TestHelper.AssertSequence(_threeMash);
        }

        [Test]
        public void RemoveAt_RemovesFirstElement_ForOne()
        {
            _oneMash.RemoveAt(0);
            TestHelper.AssertSequence(_oneMash);
        }

        [Test]
        public void RemoveAt_RemovesFirstElement_ForThree()
        {
            _threeMash.RemoveAt(0);
            TestHelper.AssertSequence(_threeMash, 2, 3);
        }

        [Test]
        public void RemoveAt_RemovesFirstElement_ForTwo()
        {
            _twoMash.RemoveAt(0);
            TestHelper.AssertSequence(_twoMash, 2);
        }

        [Test]
        public void RemoveAt_RemovesLastElement_ForThree()
        {
            _threeMash.RemoveAt(2);
            TestHelper.AssertSequence(_threeMash, 1, 2);
        }

        [Test]
        public void RemoveAt_RemovesLastElement_ForTwo()
        {
            _twoMash.RemoveAt(1);
            TestHelper.AssertSequence(_twoMash, 1);
        }

        [Test]
        public void Unlink_ActuallyRemovesTheItem()
        {
            _emptyMash.Link("a", _threeMash);
            _emptyMash.Unlink("a");

            Assert.AreNotSame(_threeMash, _emptyMash["a"]);
        }

        [Test]
        public void Unlink_DecrementsLinkedCount_IfRemoved()
        {
            _emptyMash.Link("a", _threeMash);
            _emptyMash.Unlink("a");

            Assert.AreEqual(0, _emptyMash.LinkedCount);
        }

        [Test]
        public void Unlink_DoesNotDecrementLinkedCount_IfNotRemoved()
        {
            _emptyMash.Link("a", _threeMash);
            _emptyMash.Unlink("b");

            Assert.AreEqual(1, _emptyMash.LinkedCount);
        }

        [Test]
        public void Unlink_ReturnsFalse_IfNothingRemoved()
        {
            _emptyMash.Link("a", _threeMash);

            Assert.IsFalse(_emptyMash.Unlink("b"));
        }

        [Test]
        public void Unlink_ReturnsTrue_IfRemoved()
        {
            _emptyMash.Link("a", _threeMash);

            Assert.IsTrue(_emptyMash.Unlink("a"));
        }


        [Test, SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void Unlink_ThrowsException_IfKeyIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => _emptyMash.Unlink(null));
        }

        [Test]
        public void Value_Getter_ReturnsDefaultValue_ForEmpty()
        {
            Assert.AreEqual(0, _emptyMash.Value);
        }

        [Test]
        public void Value_Getter_ReturnsFirstValue_ForOne()
        {
            Assert.AreEqual(1, _oneMash.Value);
        }

        [Test]
        public void Value_Getter_ReturnsFirstValue_ForThree()
        {
            Assert.AreEqual(1, _threeMash.Value);
        }

        [Test]
        public void Value_Getter_ReturnsFirstValue_ForTwo()
        {
            Assert.AreEqual(1, _twoMash.Value);
        }

        [Test]
        public void Value_Setter_AddsNewValue_ForEmpty()
        {
            _emptyMash.Value = 1;

            Assert.AreEqual(1, _emptyMash[0]);
        }

        [Test]
        public void Value_Setter_IncrementsCountByOne_ForEmpty()
        {
            _emptyMash.Value = 1;
            Assert.AreEqual(1, _emptyMash.Count);
        }

        [Test]
        public void Value_Setter_SetsElementZero_ForOne()
        {
            _oneMash.Value = 10;
            TestHelper.AssertSequence(_oneMash, 10);
        }

        [Test]
        public void Value_Setter_SetsElementZero_ForThree()
        {
            _threeMash.Value = 10;
            TestHelper.AssertSequence(_threeMash, 10, 2, 3);
        }

        [Test]
        public void Value_Setter_SetsElementZero_ForTwo()
        {
            _twoMash.Value = 10;
            TestHelper.AssertSequence(_twoMash, 10, 2);
        }
    }
}