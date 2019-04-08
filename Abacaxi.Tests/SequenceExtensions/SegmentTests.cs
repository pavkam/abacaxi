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

namespace Abacaxi.Tests.SequenceExtensions
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using NUnit.Framework;

    [TestFixture]
    public sealed class SegmentTests
    {
        [SetUp]
        public void SetUp()
        {
            _original = new List<int> {0};
            _start = _original.Segment(0, 0);
            _end = _original.Segment(1, 0);
            _middle = _original.Segment(0, 1);

            var original = new List<int> {0, 1, 2, 3, 4};
            _broken = original.Segment(2, 3);
            original.RemoveAt(4);
        }

        private IList<int> _original;
        private IList<int> _start;
        private IList<int> _end;
        private IList<int> _middle;
        private IList<int> _broken;

        [Test]
        public void Segment_Add_End()
        {
            _end.Add(1);

            TestHelper.AssertSequence(_end, 1);
            TestHelper.AssertSequence(_original, 0, 1);
        }

        [Test]
        public void Segment_Add_IncrementsCountByOne()
        {
            _middle.Add(1);
            Assert.AreEqual(2, _middle.Count);
        }

        [Test]
        public void Segment_Add_Middle()
        {
            _middle.Add(1);

            TestHelper.AssertSequence(_middle, 0, 1);
            TestHelper.AssertSequence(_original, 0, 1);
        }

        [Test]
        public void Segment_Add_Start()
        {
            _start.Add(1);

            TestHelper.AssertSequence(_start, 1);
            TestHelper.AssertSequence(_original, 1, 0);
        }

        [Test]
        public void Segment_Add_ThrowsException_IfBroken()
        {
            Assert.Throws<InvalidOperationException>(() => _broken.Add(100));
        }

        [Test]
        public void Segment_Clear_End()
        {
            _end.Clear();

            TestHelper.AssertSequence(_end);
            TestHelper.AssertSequence(_original, 0);
        }

        [Test]
        public void Segment_Clear_Middle()
        {
            _middle.Clear();

            TestHelper.AssertSequence(_middle);
            TestHelper.AssertSequence(_original);
        }

        [Test]
        public void Segment_Clear_SetsCountToZero()
        {
            _middle.Clear();
            Assert.AreEqual(0, _middle.Count);
        }

        [Test]
        public void Segment_Clear_Start()
        {
            _start.Clear();

            TestHelper.AssertSequence(_start);
            TestHelper.AssertSequence(_original, 0);
        }

        [Test]
        public void Segment_Clear_ThrowsException_IfBroken()
        {
            Assert.Throws<InvalidOperationException>(() => _broken.Clear());
        }

        [Test]
        public void Segment_Contains_ReturnsFalse_IfElementNotInSegment()
        {
            var segment = "0123456789".AsList().Segment(1, 5);

            Assert.IsFalse(segment.Contains('6'));
            Assert.IsFalse(segment.Contains('0'));
        }

        [Test]
        public void Segment_Contains_ReturnsTheRelativeIndex_IfElementInSegment()
        {
            var segment = "0123456789".AsList().Segment(1, 5);

            Assert.IsTrue(segment.Contains('1'));
            Assert.IsTrue(segment.Contains('5'));
        }

        [Test, SuppressMessage("ReSharper", "ReturnValueOfPureMethodIsNotUsed")]
        public void Segment_Contains_ThrowsException_IfBroken()
        {
            Assert.Throws<InvalidOperationException>(() => _broken.Contains(100));
        }

        [Test]
        public void Segment_CopyTo_CopiesAllItems_ToArray()
        {
            var array = new int[4];
            _middle.Add(1);
            _middle.Add(2);
            _middle.Add(3);

            _middle.CopyTo(array, 0);
            TestHelper.AssertSequence(array, 0, 1, 2, 3);
        }

        [Test]
        public void Segment_CopyTo_DoesNothing_ForEmptySegment()
        {
            var array = new int[1];
            _start.CopyTo(array, 0);

            Assert.AreEqual(0, array[0]);
        }

        [Test, SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void Segment_CopyTo_ThrowsException_ForNullArray()
        {
            Assert.Throws<ArgumentNullException>(() => _middle.CopyTo(null, 0));
        }

        [Test]
        public void Segment_CopyTo_ThrowsException_IfBroken()
        {
            Assert.Throws<InvalidOperationException>(() => _broken.CopyTo(new int[100], 0));
        }

        [Test]
        public void Segment_CopyTo_ThrowsException_IfIndexIsNegative()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _middle.CopyTo(new int[] { }, -1));
        }

        [Test]
        public void Segment_CopyTo_ThrowsException_IfNotEnoughSpace1()
        {
            _middle.Add(1);
            _middle.Add(2);
            _middle.Add(3);
            Assert.Throws<ArgumentOutOfRangeException>(() => _middle.CopyTo(new int[4], 1));
        }

        [Test]
        public void Segment_CopyTo_ThrowsException_IfNotEnoughSpace2()
        {
            _middle.Add(1);
            _middle.Add(2);
            _middle.Add(3);
            Assert.Throws<ArgumentOutOfRangeException>(() => _middle.CopyTo(new int[3], 0));
        }

        [Test]
        public void Segment_Count_EqualsToTheLengthPassedIn_1()
        {
            var segment = "0123456789".AsList().Segment(3, 5);
            Assert.AreEqual(5, segment.Count);
        }

        [Test]
        public void Segment_Count_EqualsToTheLengthPassedIn_2()
        {
            var segment = "0123456789".AsList().Segment(0, 0);
            Assert.AreEqual(0, segment.Count);
        }

        [Test]
        public void Segment_Count_EqualsToTheLengthPassedIn_3()
        {
            var segment = "0123456789".AsList().Segment(10, 0);
            Assert.AreEqual(0, segment.Count);
        }

        [Test]
        public void Segment_GetEnumerator_EnumeratesTheOriginalList()
        {
            var list = new[] {0, 1, 2, 3};
            var segment = list.Segment(1, 2);
            var actual = new List<int>();
            list[1] = 100;
            list[2] = 200;
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var s in segment)
            {
                actual.Add(s);
            }

            TestHelper.AssertSequence(actual, 100, 200);
        }

        [Test]
        public void Segment_GetEnumerator_EnumeratesTheRangeItWasGiven()
        {
            var list = new[] {0, 1, 2, 3};
            var segment = list.Segment(1, 2);
            var actual = new List<int>();
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var s in segment)
            {
                actual.Add(s);
            }

            TestHelper.AssertSequence(actual, 1, 2);
        }

        [Test, SuppressMessage("ReSharper", "ReturnValueOfPureMethodIsNotUsed")]
        public void Segment_GetEnumerator_ThrowsException_IfBroken()
        {
            Assert.Throws<InvalidOperationException>(() => _broken.GetEnumerator().MoveNext());
        }

        [Test]
        public void Segment_Getter_ReturnsTheElementAtGivenIndex()
        {
            var original = new List<int> {0, 1, 2, 3, 4, 5};
            var segment = original.Segment(1, 4);

            Assert.AreEqual(1, segment[0]);
            Assert.AreEqual(4, segment[3]);
        }

        [Test]
        public void Segment_Getter_ThrowsException_IfBroken()
        {
            Assert.Throws<InvalidOperationException>(() => Assert.AreEqual(10, _broken[0]));
        }

        [Test]
        public void Segment_Getter_ThrowsException_IfIndexIsOutOfBounds_1()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Assert.AreEqual(1, _start[-1]));
        }

        [Test]
        public void Segment_Getter_ThrowsException_IfIndexIsOutOfBounds_2()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Assert.AreEqual(1, _start[0]));
        }

        [Test]
        public void Segment_Getter_ThrowsException_IfIndexIsOutOfBounds_3()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Assert.AreEqual(1, _start[1]));
        }

        [Test]
        public void Segment_Getter_ThrowsException_IfIndexIsOutOfBounds_4()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Assert.AreEqual(1, _end[-1]));
        }

        [Test]
        public void Segment_Getter_ThrowsException_IfIndexIsOutOfBounds_5()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Assert.AreEqual(1, _end[0]));
        }

        [Test]
        public void Segment_Getter_ThrowsException_IfIndexIsOutOfBounds_6()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Assert.AreEqual(1, _end[1]));
        }

        [Test]
        public void Segment_Getter_ThrowsException_IfIndexIsOutOfBounds_7()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Assert.AreEqual(1, _middle[-1]));
        }

        [Test]
        public void Segment_Getter_ThrowsException_IfIndexIsOutOfBounds_8()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Assert.AreEqual(1, _middle[1]));
        }

        [Test]
        public void Segment_ImplicitGetEnumerator_ThrowsException_IfBroken()
        {
            Assert.Throws<InvalidOperationException>(() => ((IEnumerable) _broken).GetEnumerator().MoveNext());
        }

        [Test]
        public void Segment_ImplicitGetEnumerator_WorksAsExpected()
        {
            var actual = "";
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var s in (IEnumerable) "0123456789".AsList().Segment(0, 10))
            {
                actual += s;
            }

            Assert.AreEqual("0123456789", actual);
        }

        [Test]
        public void Segment_IndexOf_ReturnsMinusOne_IfElementNotInSegment()
        {
            var segment = "0123456789".AsList().Segment(1, 5);

            Assert.AreEqual(-1, segment.IndexOf('6'));
            Assert.AreEqual(-1, segment.IndexOf('0'));
        }

        [Test]
        public void Segment_IndexOf_ReturnsTheRelativeIndex_IfElementInSegment()
        {
            var segment = "0123456789".AsList().Segment(1, 5);

            Assert.AreEqual(0, segment.IndexOf('1'));
            Assert.AreEqual(4, segment.IndexOf('5'));
        }

        [Test, SuppressMessage("ReSharper", "ReturnValueOfPureMethodIsNotUsed")]
        public void Segment_IndexOf_ThrowsException_IfBroken()
        {
            Assert.Throws<InvalidOperationException>(() => _broken.IndexOf(100));
        }

        [Test]
        public void Segment_Insert_End()
        {
            _end.Insert(0, 1);

            TestHelper.AssertSequence(_end, 1);
            TestHelper.AssertSequence(_original, 0, 1);
        }

        [Test]
        public void Segment_Insert_IncrementsCountByOne()
        {
            _middle.Insert(0, 1);
            Assert.AreEqual(2, _middle.Count);
        }

        [Test]
        public void Segment_Insert_Middle_1()
        {
            _middle.Insert(0, 1);

            TestHelper.AssertSequence(_middle, 1, 0);
            TestHelper.AssertSequence(_original, 1, 0);
        }

        [Test]
        public void Segment_Insert_Middle_2()
        {
            _middle.Insert(1, 1);

            TestHelper.AssertSequence(_middle, 0, 1);
            TestHelper.AssertSequence(_original, 0, 1);
        }

        [Test]
        public void Segment_Insert_Start()
        {
            _start.Insert(0, 1);

            TestHelper.AssertSequence(_start, 1);
            TestHelper.AssertSequence(_original, 1, 0);
        }

        [Test]
        public void Segment_Insert_ThrowsException_IfBroken()
        {
            Assert.Throws<InvalidOperationException>(() => _broken.Insert(0, 1));
        }

        [Test]
        public void Segment_Insert_ThrowsException_IfIndexIsOutOfBounds_1()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _start.Insert(-1, 1));
        }

        [Test]
        public void Segment_Insert_ThrowsException_IfIndexIsOutOfBounds_2()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _start.Insert(1, 1));
        }

        [Test]
        public void Segment_Insert_ThrowsException_IfIndexIsOutOfBounds_3()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _end.Insert(1, 1));
        }

        [Test]
        public void Segment_Insert_ThrowsException_IfIndexIsOutOfBounds_4()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _middle.Insert(2, 1));
        }

        [Test]
        public void Segment_IsReadOnly_ReturnsFalse_IfTheOriginalListIsNotReadOnly()
        {
            Assert.IsFalse(_middle.IsReadOnly);
        }

        [Test]
        public void Segment_IsReadOnly_ReturnsTrue_IfTheOriginalListIsReadOnly()
        {
            var original = "string".AsList();
            Assert.IsTrue(original.Segment(0, original.Count).IsReadOnly);
        }

        [Test]
        public void Segment_Remove_DecrementsCountByOne()
        {
            var original = new List<int> {0, 1, 2, 3, 4, 5};
            var segment = original.Segment(1, 4);
            segment.Remove(2);

            Assert.AreEqual(3, segment.Count);
        }

        [Test]
        public void Segment_Remove_DoesNothing_IfElementNotInSegment()
        {
            var original = new List<int> {0, 1, 2, 3, 4, 5};
            var segment = original.Segment(1, 4);
            segment.Remove(0);
            segment.Remove(5);

            TestHelper.AssertSequence(segment, 1, 2, 3, 4);
        }

        [Test]
        public void Segment_Remove_ModifiesTheContentsOfTheOriginalList_IfElementWasRemoved()
        {
            var original = new List<int> {0, 1, 2, 3, 4, 5};
            var segment = original.Segment(1, 4);
            segment.Remove(2);

            TestHelper.AssertSequence(original, 0, 1, 3, 4, 5);
        }

        [Test]
        public void Segment_Remove_ModifiesTheContentsOfTheSegment_IfElementWasRemoved()
        {
            var original = new List<int> {0, 1, 2, 3, 4, 5};
            var segment = original.Segment(1, 4);
            segment.Remove(2);

            TestHelper.AssertSequence(segment, 1, 3, 4);
        }

        [Test]
        public void Segment_Remove_ReturnsFalse_IfElementNotInSegment()
        {
            var original = new List<int> {0, 1, 2, 3, 4, 5};
            var segment = original.Segment(1, 4);

            Assert.IsFalse(segment.Remove(0));
            Assert.IsFalse(segment.Remove(5));
        }

        [Test]
        public void Segment_Remove_ReturnsTrue_IfElementInSegment()
        {
            var original = new List<int> {0, 1, 2, 3, 4, 5};
            var segment = original.Segment(1, 4);

            Assert.IsTrue(segment.Remove(2));
        }

        [Test]
        public void Segment_Remove_ThrowsException_IfBroken()
        {
            Assert.Throws<InvalidOperationException>(() => _broken.Remove(100));
        }

        [Test]
        public void Segment_RemoveAt_DecrementsCountByOne()
        {
            var original = new List<int> {0, 1, 2, 3, 4, 5};
            var segment = original.Segment(1, 4);
            segment.RemoveAt(1);

            Assert.AreEqual(3, segment.Count);
        }

        [Test]
        public void Segment_RemoveAt_ModifiesTheContentsOfTheOriginalList_IfElementWasRemoved()
        {
            var original = new List<int> {0, 1, 2, 3, 4, 5};
            var segment = original.Segment(1, 4);
            segment.RemoveAt(1);

            TestHelper.AssertSequence(original, 0, 1, 3, 4, 5);
        }

        [Test]
        public void Segment_RemoveAt_ModifiesTheContentsOfTheSegment_IfElementWasRemoved()
        {
            var original = new List<int> {0, 1, 2, 3, 4, 5};
            var segment = original.Segment(1, 4);
            segment.RemoveAt(1);

            TestHelper.AssertSequence(segment, 1, 3, 4);
        }

        [Test]
        public void Segment_RemoveAt_ThrowsException_IfBroken()
        {
            Assert.Throws<InvalidOperationException>(() => _broken.RemoveAt(0));
        }

        [Test]
        public void Segment_RemoveAt_ThrowsException_IfIndexIsOutOfBounds_1()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _start.RemoveAt(-1));
        }

        [Test]
        public void Segment_RemoveAt_ThrowsException_IfIndexIsOutOfBounds_2()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _start.RemoveAt(0));
        }

        [Test]
        public void Segment_RemoveAt_ThrowsException_IfIndexIsOutOfBounds_3()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _end.RemoveAt(0));
        }

        [Test]
        public void Segment_RemoveAt_ThrowsException_IfIndexIsOutOfBounds_4()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _middle.RemoveAt(1));
        }

        [Test]
        public void Segment_Setter_DoesNotModifyTheCount()
        {
            var original = new List<int> {0, 1, 2, 3, 4, 5};
            var segment = original.Segment(1, 4);

            segment[0] = 100;
            segment[3] = 400;

            Assert.AreEqual(4, segment.Count);
        }

        [Test]
        public void Segment_Setter_ModifiesTheElementAtGivenIndex()
        {
            var original = new List<int> {0, 1, 2, 3, 4, 5};
            var segment = original.Segment(1, 4);

            segment[0] = 100;
            segment[3] = 400;

            TestHelper.AssertSequence(segment, 100, 2, 3, 400);
        }

        [Test]
        public void Segment_Setter_ModifiesTheOriginalCollection()
        {
            var original = new List<int> {0, 1, 2, 3, 4, 5};
            var segment = original.Segment(1, 4);

            segment[0] = 100;
            segment[3] = 400;

            TestHelper.AssertSequence(original, 0, 100, 2, 3, 400, 5);
        }

        [Test]
        public void Segment_Setter_ThrowsException_IfBroken()
        {
            Assert.Throws<InvalidOperationException>(() => _broken[0] = 1);
        }

        [Test]
        public void Segment_Setter_ThrowsException_IfIndexIsOutOfBounds_1()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _start[-1] = 1);
        }

        [Test]
        public void Segment_Setter_ThrowsException_IfIndexIsOutOfBounds_2()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _start[0] = 1);
        }

        [Test]
        public void Segment_Setter_ThrowsException_IfIndexIsOutOfBounds_3()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _start[1] = 1);
        }

        [Test]
        public void Segment_Setter_ThrowsException_IfIndexIsOutOfBounds_4()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _end[-1] = 1);
        }

        [Test]
        public void Segment_Setter_ThrowsException_IfIndexIsOutOfBounds_5()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _end[0] = 1);
        }

        [Test]
        public void Segment_Setter_ThrowsException_IfIndexIsOutOfBounds_6()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _end[1] = 1);
        }

        [Test]
        public void Segment_Setter_ThrowsException_IfIndexIsOutOfBounds_7()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _middle[-1] = 1);
        }

        [Test]
        public void Segment_Setter_ThrowsException_IfIndexIsOutOfBounds_8()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _middle[1] = 1);
        }

        [Test]
        public void Segment_ThrowsException_ForNegativeLength()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                new[] {1}.Segment(0, -1));
        }

        [Test]
        public void Segment_ThrowsException_ForNegativeStartIndex()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                new[] {1}.Segment(-1, 1));
        }

        [Test]
        public void Segment_ThrowsException_ForOutOfBounds1()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                new[] {1}.Segment(0, 2));
        }

        [Test]
        public void Segment_ThrowsException_ForOutOfBounds2()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                new[] {1}.Segment(1, 1));
        }

        [Test, SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void Segment_ThrowsException_IfSequenceIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => ((int[]) null).Segment(0, 1));
        }

        [Test]
        public void Segment_WithFullSize_ReturnsTheOriginalElementsInList()
        {
            var result = "0123456789".AsList().Segment(0, 10).ToString("");
            Assert.AreEqual("0123456789", result);
        }

        [Test]
        public void Segment_WithIndex_ReturnsTheExpectedElementsInList()
        {
            var result = "0123456789".AsList().Segment(1, 9).ToString("");
            Assert.AreEqual("123456789", result);
        }

        [Test]
        public void Segment_WithLength_ReturnsTheExpectedElementsInList()
        {
            var result = "0123456789".AsList().Segment(0, 9).ToString("");
            Assert.AreEqual("012345678", result);
        }

        [Test]
        public void Segment_WithZeroLength_ReturnsNothing()
        {
            var result = "0123456789".AsList().Segment(1, 0).ToString("");
            Assert.AreEqual("", result);
        }
    }
}