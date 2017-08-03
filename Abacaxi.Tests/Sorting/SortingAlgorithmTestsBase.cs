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

namespace Abacaxi.Tests.Sorting
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NUnit.Framework;

    public abstract class SortingAlgorithmTestsBase
    {
        protected abstract void Sort<T>(T[] array, int startIndex, int length, IComparer<T> comparer);

        protected abstract bool IsStable { get; }

        [Test]
        public void Sort_ThrowsException_ForNullArray()
        {
            Assert.Throws<ArgumentNullException>(() =>
                Sort(null, 1, 1, Comparer<int>.Default));
        }
        
        [Test]
        public void Sort_ThrowsException_ForNegativeStartIndex()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                Sort(new[] { 1 }, -1, 1, Comparer<int>.Default));
        }

        [Test]
        public void Sort_ThrowsException_ForNegativeLength()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                Sort(new[] { 1 }, 0, -1, Comparer<int>.Default));
        }

        [Test]
        public void Sort_ThrowsException_ForOutOfBounds1()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                Sort(new[] { 1 }, 0, 2, Comparer<int>.Default));
        }

        [Test]
        public void Sort_ThrowsException_ForOutOfBounds2()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                Sort(new[] { 1 }, 1, 1, Comparer<int>.Default));
        }

        [Test]
        public void Sort_ThrowsException_ForNullComparer()
        {
            Assert.Throws<ArgumentNullException>(() =>
                Sort(new[] { 1 }, 0, 1, null));
        }

        [Test]
        public void Sort_DoesNothing_OnEmptyArray()
        {
            Sort(new int[] { }, 0, 0, Comparer<int>.Default);
        }

        [Test]
        public void Sort_TakesIntoAccount_TheComparer()
        {
            var array = new[] { "B", "A", "a" };
            Sort(array, 0, 3, StringComparer.OrdinalIgnoreCase);

            Assert.IsTrue(StringComparer.OrdinalIgnoreCase.Equals(array[0], "A"));
            Assert.IsTrue(StringComparer.OrdinalIgnoreCase.Equals(array[1], "A"));
            Assert.IsTrue(StringComparer.OrdinalIgnoreCase.Equals(array[2], "B"));
        }

        [Test]
        public void Sort_DoesNothing_OnOneElementArray()
        {
            var array = new[] { 1 };

            Sort(array, 0, 1, Comparer<int>.Default);

            TestHelper.AssertSequence(array,
                1);
        }


        [Test]
        public void Sort_SortsTheArray_ForTwoElementArray()
        {
            var array = new[] { 2, 1 };
            Sort(array, 0, 2, Comparer<int>.Default);

            TestHelper.AssertSequence(array,
                1, 2);
        }

        [Test]
        public void Sort_SortsTheFirstTwoElements_ForThreeElementArray()
        {
            var array = new[] { 3, 2, 1 };
            Sort(array, 0, 2, Comparer<int>.Default);

            TestHelper.AssertSequence(array,
                2, 3, 1);
        }

        [Test]
        public void Sort_DoesNothing_ForAlreadySortedArray()
        {
            var array = new[] { 1, 2, 3 };
            Sort(array, 0, 3, Comparer<int>.Default);

            TestHelper.AssertSequence(array,
                1, 2, 3);
        }

        [Test]
        public void Sort_SortsTheArray_ForTenElementArray()
        {
            var array = new[] { 10, 9, 8, 7, 6, 5, 4, 3, 2, 1 };
            Sort(array, 0, 10, Comparer<int>.Default);

            TestHelper.AssertSequence(array,
                1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
        }

        [Test]
        public void Sort_SortsHalfTheArray_ForTenElementArray()
        {
            var array = new[] { 10, 9, 8, 7, 6, 5, 4, 3, 2, 1 };
            Sort(array, 5, 5, Comparer<int>.Default);

            TestHelper.AssertSequence(array,
                10, 9, 8, 7, 6, 1, 2, 3, 4, 5);
        }

        [Test]
        public void Sort_IsStable()
        {
            if (IsStable)
            {
                var array = new[] { "c", "C", "A", "a", "b", "B", "D", "d" };
                Sort(array, 0, 8, StringComparer.OrdinalIgnoreCase);

                TestHelper.AssertSequence(array,
                    "A", "a", "b", "B", "c", "C", "D", "d");
            }
            else
            {
                Assert.Ignore("This sorting algorithm is not stable.");
            }
        }

        [Test]
        public void Sort_WorksAsExpected_OnLargeArray()
        {
            var expected = Enumerable.Range(0, 1000).ToArray();
            var array = new int[expected.Length];
            Array.Copy(expected, array, array.Length);
            Array.Reverse(array, 0, array.Length);

            Sort(array, 0, array.Length, Comparer<int>.Default);

            TestHelper.AssertSequence(array, expected);
        }
    }
}
