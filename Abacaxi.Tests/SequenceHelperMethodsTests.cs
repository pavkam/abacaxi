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

using System.Diagnostics.CodeAnalysis;

namespace Abacaxi.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NUnit.Framework;

    [TestFixture]
    public sealed class SequenceHelperMethodsTests
    {
        [Test]
        public void ToSet_ThrowsException_IfSequenceIsNull1()
        {
            Assert.Throws<ArgumentNullException>(() => ((int[]) null).ToSet(EqualityComparer<int>.Default));
        }

        [Test]
        public void ToSet_ThrowsException_IfSequenceIsNull2()
        {
            Assert.Throws<ArgumentNullException>(() => ((int[]) null).ToSet());
        }

        [Test]
        public void ToSet_ThrowsException_IfEqualityComparerIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new[] {1}.ToSet(null));
        }

        [Test]
        public void ToSet_ReturnsAValidSet1()
        {
            var set = new[] {1, 1, 2, 3}.ToSet();

            TestHelper.AssertSequence(set, 1, 2, 3);
        }

        [Test]
        public void ToSet_ReturnsAValidSet2()
        {
            var set = new[] {1, 1, 2, 3}.ToSet(EqualityComparer<int>.Default);

            TestHelper.AssertSequence(set, 1, 2, 3);
        }

        [Test]
        public void ToSet_UsesTheEqualityComparer()
        {
            var set = new[] {"a", "A", "b", "c"}.ToSet(StringComparer.OrdinalIgnoreCase);

            TestHelper.AssertSequence(set, "a", "b", "c");
        }

        [Test]
        public void AsList_ThrowsException_IfSequenceIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => ((int[]) null).AsList());
        }

        [Test]
        public void AsList_ReturnsSameObject_IfItIsAList()
        {
            var list = new List<int>() {1};
            var asList = list.AsList();

            Assert.AreSame(list, asList);
        }

        [Test]
        public void AsList_ReturnsSameObject_IfItIsAnArray()
        {
            var list = new[] {1};
            var asList = list.AsList();

            Assert.AreSame(list, asList);
        }

        [Test]
        public void AsList_ReturnsANewList_FromACollection()
        {
            var coll = new[] {1, 2, 3}.ToSet();
            var asList = coll.AsList();

            TestHelper.AssertSequence(asList, 1, 2, 3);
        }

        [Test]
        public void AsList_ReturnsEmptyList_ForEmptyCollection()
        {
            // ReSharper disable once CollectionNeverUpdated.Local
            var coll = new HashSet<int>();
            var asList = coll.AsList();

            TestHelper.AssertSequence(asList);
        }

        [Test]
        public void AsList_ReturnsANewList_ForAString()
        {
            const string s = "123";
            var asList = SequenceHelperMethods.AsList(s);

            TestHelper.AssertSequence(asList, '1', '2', '3');
        }

        [Test]
        public void AsList_ReturnsANewList_ForAnEnumerable()
        {
            var e = Enumerable.Range(1, 3);
            var asList = e.AsList();

            TestHelper.AssertSequence(asList, 1, 2, 3);
        }

        [Test]
        public void GetItemFrequencies_ThrowsException_IfSequenceIsNull()
        {
            Assert.Throws<ArgumentNullException>(
                () => ((int[]) null).GetItemFrequencies(EqualityComparer<int>.Default));
        }

        [Test]
        public void GetItemFrequencies_ThrowsException_IfEqualityComparerIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new[] {1}.ToSet(null));
        }

        [Test]
        public void GetItemFrequencies_ReturnsEmptyDictionary_ForEmptySequence()
        {
            var list = new int[] { };
            TestHelper.AssertSequence(list.GetItemFrequencies(EqualityComparer<int>.Default));
        }

        [Test]
        public void GetItemFrequencies_ReturnsValidItems()
        {
            var list = new[] {10, 1, 10, 10, 2, 2};
            var freq = list.GetItemFrequencies(EqualityComparer<int>.Default);

            Assert.AreEqual(3, freq.Count);
            Assert.AreEqual(3, freq[10]);
            Assert.AreEqual(2, freq[2]);
            Assert.AreEqual(1, freq[1]);
        }

        [Test]
        public void GetItemFrequencies_UsesTheEqualityComparer()
        {
            var list = new[] {"a", "A"};
            var freq = list.GetItemFrequencies(StringComparer.OrdinalIgnoreCase);

            Assert.AreEqual(2, freq["a"]);
        }

        [Test]
        public void AddOrUpdate_ThrowsException_IfDictIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => ((Dictionary<int, int>) null).AddOrUpdate(1, 1, i => i));
        }

        [Test]
        public void AddOrUpdate_ThrowsException_IfUpdateFuncIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new Dictionary<int, int>().AddOrUpdate(1, 1, null));
        }

        [Test]
        public void AddOrUpdate_ReturnsTrue_IfKeyValueIsAdded()
        {
            var dict = new Dictionary<string, int>();
            Assert.IsTrue(dict.AddOrUpdate("key", 1, i => i));
        }

        [Test]
        public void AddOrUpdate_ReturnsFalse_IfKeyValueIsUpdated()
        {
            var dict = new Dictionary<string, int> {{"key", 1}};
            Assert.IsFalse(dict.AddOrUpdate("key", 1, i => i));
        }

        [Test]
        public void AddOrUpdate_AddsTheKeyValue_IfTheKeyIsNotFound()
        {
            var dict = new Dictionary<string, int>();
            dict.AddOrUpdate("key", 1, i => i);

            Assert.IsTrue(dict.TryGetValue("key", out int value));
            Assert.AreEqual(1, value);
        }

        [Test]
        public void AddOrUpdate_UpdatesTheValue_IfTheKeyIsFound()
        {
            var dict = new Dictionary<string, int> {{"key", 1}};
            dict.AddOrUpdate("key", 2, i => -1);

            Assert.IsTrue(dict.TryGetValue("key", out int value));
            Assert.AreEqual(-1, value);
        }

        [Test]
        public void Append_CreatesNewArray_ForNullArray_AndOneElement()
        {
            var array = ((int[]) null).Append(1);

            TestHelper.AssertSequence(array, 1);
        }

        [Test]
        public void Append_CreatesNewArray_ForNullArray_AndTwoElements()
        {
            var array = ((int[]) null).Append(1, 2);

            TestHelper.AssertSequence(array, 1, 2);
        }

        [Test]
        public void Append_CreatesNewArray_ForNullArray_AndThreeElements()
        {
            var array = ((int[]) null).Append(1, 2, 3);

            TestHelper.AssertSequence(array, 1, 2, 3);
        }

        [Test]
        public void Append_CreatesNewArray_ForNullArray_AndFourElements()
        {
            var array = ((int[]) null).Append(1, 2, 3, 4);

            TestHelper.AssertSequence(array, 1, 2, 3, 4);
        }

        [Test]
        public void Append_CreatesNewArray_ForNullArray_AndFiveElements()
        {
            var array = ((int[]) null).Append(1, 2, 3, 4, 5);

            TestHelper.AssertSequence(array, 1, 2, 3, 4, 5);
        }

        [Test]
        public void Append_CreatesNewArray_ForNullArray_AndSixElements()
        {
            var array = ((int[]) null).Append(1, 2, 3, 4, 5, 6);

            TestHelper.AssertSequence(array, 1, 2, 3, 4, 5, 6);
        }

        [Test]
        public void Append_CreatesNewArray_ForFullArray_AndOneElement()
        {
            var array = new[] {-2, -1, 0}.Append(1);

            TestHelper.AssertSequence(array, -2, -1, 0, 1);
        }

        [Test]
        public void Append_CreatesNewArray_ForFullArray_AndTwoElements()
        {
            var array = new[] {-2, -1, 0}.Append(1, 2);

            TestHelper.AssertSequence(array, -2, -1, 0, 1, 2);
        }

        [Test]
        public void Append_CreatesNewArray_ForFullArray_AndThreeElements()
        {
            var array = new[] {-2, -1, 0}.Append(1, 2, 3);

            TestHelper.AssertSequence(array, -2, -1, 0, 1, 2, 3);
        }

        [Test]
        public void Append_CreatesNewArray_ForFullArray_AndFourElements()
        {
            var array = new[] {-2, -1, 0}.Append(1, 2, 3, 4);

            TestHelper.AssertSequence(array, -2, -1, 0, 1, 2, 3, 4);
        }

        [Test]
        public void Append_CreatesNewArray_ForFullArray_AndFiveElements()
        {
            var array = new[] {-2, -1, 0}.Append(1, 2, 3, 4, 5);

            TestHelper.AssertSequence(array, -2, -1, 0, 1, 2, 3, 4, 5);
        }

        [Test]
        public void Append_CreatesNewArray_ForFullArray_AndSixElements()
        {
            var array = new[] {-2, -1, 0}.Append(1, 2, 3, 4, 5, 6);

            TestHelper.AssertSequence(array, -2, -1, 0, 1, 2, 3, 4, 5, 6);
        }

        [Test]
        [SuppressMessage("ReSharper", "ReturnValueOfPureMethodIsNotUsed")]
        public void AsIndexedEnumerable_ThrowsException_ForNullSequence()
        {
            Assert.Throws<ArgumentNullException>(() =>
                ((int[]) null).AsIndexedEnumerable().ToArray());
        }

        [Test]
        public void AsIndexedEnumerable_ReturnsNothing_ForEmptySequence()
        {
            var result = new int[] { }.AsIndexedEnumerable();

            TestHelper.AssertSequence(result);
        }

        [Test]
        public void AsIndexedEnumerable_ReturnsTheExpected_ForAList()
        {
            var result = new List<string> {"a", "b", "c"}.AsIndexedEnumerable();

            TestHelper.AssertSequence(result,
                new KeyValuePair<int, string>(0, "a"),
                new KeyValuePair<int, string>(1, "b"),
                new KeyValuePair<int, string>(2, "c"));
        }

        [Test]
        public void AsIndexedEnumerable_ReturnsTheExpected_ForAnEnumerable()
        {
            var result = new[] {"a", "b", "c"}.Where(p => true).AsIndexedEnumerable();

            TestHelper.AssertSequence(result,
                new KeyValuePair<int, string>(0, "a"),
                new KeyValuePair<int, string>(1, "b"),
                new KeyValuePair<int, string>(2, "c"));
        }

        [Test]
        public void ToList1_ThrowsException_ForNullSequence()
        {
            Assert.Throws<ArgumentNullException>(() => ((int[]) null).ToList(i => i));
        }

        [Test]
        public void ToList1_ThrowsException_ForNullSelector()
        {
            Assert.Throws<ArgumentNullException>(() => new int[] { }.ToList((Func<int, int>) null));
        }

        [Test]
        public void ToList1_SelectsExpectedItems_ForAList()
        {
            var actual = new List<string> {"1", "2", "3"}.ToList(int.Parse);

            TestHelper.AssertSequence(actual, 1, 2, 3);
        }

        [Test]
        public void ToList1_SelectsExpectedItems_ForEnumerable()
        {
            var actual = new List<string> {"1", "2", "3"}.Where(p => true).ToList(int.Parse);

            TestHelper.AssertSequence(actual, 1, 2, 3);
        }

        [Test]
        public void ToList2_ThrowsException_ForNullSequence()
        {
            Assert.Throws<ArgumentNullException>(() => ((int[]) null).ToList((n, i) => i));
        }

        [Test]
        public void ToList2_ThrowsException_ForNullSelector()
        {
            Assert.Throws<ArgumentNullException>(() => new int[] { }.ToList((Func<int, int, int>) null));
        }

        [Test]
        public void ToList2_SelectsExpectedItems_ForAList()
        {
            var actual = new List<string> {"a", "b", "c"}.ToList((s, i) => $"{i}:{s}");

            TestHelper.AssertSequence(actual, "0:a", "1:b", "2:c");
        }

        [Test]
        public void ToList2_SelectsExpectedItems_ForEnumerable()
        {
            var actual = new List<string> {"a", "b", "c"}.Where(p => true).ToList((s, i) => $"{i}:{s}");

            TestHelper.AssertSequence(actual, "0:a", "1:b", "2:c");
        }

        [Test]
        public void Partition_ThrowsException_ForNullSequence()
        {
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            Assert.Throws<ArgumentNullException>(() => ((int[]) null).Partition(1).ToArray());
        }

        [Test]
        public void Partition_ThrowsException_ForSizeLessThanOne()
        {
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            Assert.Throws<ArgumentOutOfRangeException>(() => new[] {1}.Partition(0).ToArray());
        }

        [Test]
        public void Partition_ReturnsNothing_ForEmptySequence()
        {
            var actual = new string[] { }.Partition(1);

            TestHelper.AssertSequence(actual);
        }

        [Test]
        public void Partition_ReturnsSingleArray_ForSizeOfOne()
        {
            var actual = "Alex".Partition(1);

            TestHelper.AssertSequence(actual,
                new[] {'A'},
                new[] {'l'},
                new[] {'e'},
                new[] {'x'});
        }

        [Test]
        public void Partition_ReturnsFullArraysAndASpill_IfOneExists()
        {
            var actual = "Alex".Partition(3);

            TestHelper.AssertSequence(actual,
                new[] {'A', 'l', 'e'},
                new[] {'x'});
        }
    }
}