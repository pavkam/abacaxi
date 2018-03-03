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


namespace Abacaxi.Tests.Containers
{
    using System;
    using Abacaxi.Containers;
    using NUnit.Framework;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    [TestFixture]
    public class DisjointSetTests
    {
        [Test, SuppressMessage("ReSharper", "ObjectCreationAsStatement"),
         SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void Ctor_ThrowsException_WhenComparerIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new DisjointSet<int>(null));
        }

        [Test]
        public void Ctor_CreatesEmptySet()
        {
            Assert.IsEmpty(new DisjointSet<int>());
        }

        [Test]
        public void Ctor_TakesIntoAccountTheEqualityComparer()
        {
            var set = new DisjointSet<string>(StringComparer.OrdinalIgnoreCase);
            set.Merge("test");

            Assert.AreEqual("test", set["TEST"]);
        }

        [Test]
        public void Ctor_UsesDefaultEqualityComparerIfNotSpecified()
        {
            var set = new DisjointSet<string>();
            set.Merge("test");

            Assert.AreEqual("TEST", set["TEST"]);
        }

        [Test, SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void Indexer_ThrowsException_IfObjectIsNull()
        {
            var set = new DisjointSet<string>();
            Assert.Throws<ArgumentNullException>(
                () => Assert.NotNull(set[null]));
        }

        [Test]
        public void Indexer_ReturnsSameObject_IfTheObjectIsNotInside()
        {
            var set = new DisjointSet<string>();
            const string o = "first";
            Assert.AreSame(o, set[o]);
        }

        [Test]
        public void Indexer_ReturnsInitialObject_IfTheObjectIsInside()
        {
            var set = new DisjointSet<string>();
            const string o = "first";
            var s = new string(o.ToCharArray());

            set.Merge(o);
            Assert.AreSame(o, set[s]);
        }

        [Test]
        public void Indexer_ActuallyAddsTheObject_IfReferenced()
        {
            var set = new DisjointSet<string>();
            const string o = "first";
            if (set[o] == o)
            {
                TestHelper.AssertSequence(set, o);
            }
        }

        [Test]
        public void Indexer_DoesNotAddTheObject_IfAlreadyThere()
        {
            var set = new DisjointSet<string>();
            const string o = "first";
            if (set[o] != "not" && set[o] != "this")
            {
                TestHelper.AssertSequence(set, o);
            }
        }

        [Test]
        public void Indexer_CausesTheEnumerationToFail_IfTheObjectIsNew_AndSetIsEmpty()
        {
            var set = new DisjointSet<string>();
            const string o = "first";
            using (var enumerator = set.GetEnumerator())
            {
                if (set[o] == o)
                {
                    Assert.Throws<InvalidOperationException>(() => enumerator.MoveNext());
                }
            }
        }

        [Test]
        public void Indexer_CausesTheEnumerationToFail_IfTheObjectIsNew_AndSetIsNotEmpty()
        {
            var set = new DisjointSet<string>();
            const string o = "second";
            set.Merge("first");

            using (var enumerator = set.GetEnumerator())
            {
                enumerator.MoveNext();
                if (set[o] == o)
                {
                    Assert.Throws<InvalidOperationException>(() => enumerator.MoveNext());
                }
            }
        }

        [Test]
        public void Indexer_ReturnsTheSameSetLabel_ForItemsInTheSameSet()
        {
            var set = new DisjointSet<string>();
            set.Merge("a", "b");

            Assert.AreEqual(set["a"], set["b"]);
        }

        [Test]
        public void Indexer_ReturnsDifferentSetLabels_ForItemsInDifferentSets()
        {
            var set = new DisjointSet<string>();
            set.Merge("a");
            set.Merge("b");

            Assert.AreNotEqual(set["a"], set["b"]);
        }

        [Test, SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void Merge_ThrowsException_IfObjectIsNull()
        {
            var set = new DisjointSet<string>();
            Assert.Throws<ArgumentNullException>(() => set.Merge(null));
        }

        [Test, SuppressMessage("ReSharper", "AssignNullToNotNullAttribute"),
         SuppressMessage("ReSharper", "RedundantCast")]
        public void Merge_ThrowsException_IfOtherObjectsIsNull()
        {
            var set = new DisjointSet<string>();
            Assert.Throws<ArgumentNullException>(() => set.Merge("a", (string[]) null));
        }

        [Test, SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void Merge_ThrowsException_IfOneOfOtherObjectsIsNull()
        {
            var set = new DisjointSet<string>();
            Assert.Throws<ArgumentNullException>(() => set.Merge("a", "b", null));
        }

        [Test]
        public void Merge_AddsAnObjectToTheSet_IfTheObjectIsNew()
        {
            var set = new DisjointSet<string>();
            set.Merge("first");

            TestHelper.AssertSequence(set, "first");
        }

        [Test]
        public void Merge_DoesNotDoAnythingIfTheObjectIsInTheSet()
        {
            var set = new DisjointSet<string>();
            set.Merge("first");
            set.Merge("first");
            TestHelper.AssertSequence(set, "first");
        }

        [Test]
        public void Merge_WillAddTwoItemsTheSet()
        {
            var set = new DisjointSet<string>();
            set.Merge("a", "b");
            TestHelper.AssertSequence(set, "a", "b");
        }

        [Test]
        public void Merge_WillMergeSetsForExistingItemAndNew()
        {
            var set = new DisjointSet<string>();
            if (set["a"] == "a")
            {
                set.Merge("a", "b", "c");
            }

            Assert.IsTrue(set["a"] == set["b"] && set["b"] == set["c"]);
        }

        [Test]
        public void Merge_Individually_AddsNewSets()
        {
            var set = new DisjointSet<string>();
            set.Merge("a");
            set.Merge("b");
            set.Merge("c");

            Assert.IsTrue(set["a"] != set["b"] && set["b"] != set["c"] && set["a"] != set["c"]);
        }

        [Test]
        public void Merge_WillMergeThreeDistinctSetsTogether()
        {
            var set = new DisjointSet<string>();
            set.Merge("a");
            set.Merge("b");
            set.Merge("c");

            set.Merge("a", "b", "c");

            Assert.IsTrue(set["a"] == set["b"] && set["b"] == set["c"]);
        }

        [Test]
        public void Merge_ReturnsTheObjectIfItsAlone()
        {
            var set = new DisjointSet<string>();
            Assert.AreEqual("first", set.Merge("first"));
        }

        [Test]
        public void Merge_ReturnsTheSetLabelObjectIfMultipleElementsMerged()
        {
            var set = new DisjointSet<string>();
            var label = set.Merge("a", "b", "c");

            Assert.IsTrue(set["a"] == label && set["b"] == label && set["c"] == label);
        }

        [Test]
        public void Merge_SelectsHeaviestSetForItsTree()
        {
            var set = new DisjointSet<string>();
            var heavy = set.Merge("a", "b");

            Assert.AreEqual(heavy, set.Merge("a", "c"));
        }

        [Test]
        public void Merge_CombinesTwoSubTreesIfOneElementIsUsedForMerge()
        {
            var set = new DisjointSet<string>();
            set.Merge("a", "b", "c");
            set.Merge("d", "e", "f");
            var label = set.Merge("f", "a");

            Assert.IsTrue(set.All(item => set[item] == label));
        }

        [Test]
        public void Merge_CausesTheEnumerationToFail_IfTheObjectIsNew_AndSetIsEmpty()
        {
            var set = new DisjointSet<string>();
            const string o = "first";
            using (var enumerator = set.GetEnumerator())
            {
                set.Merge(o);
                Assert.Throws<InvalidOperationException>(() => enumerator.MoveNext());
            }
        }

        [Test]
        public void Merge_CausesTheEnumerationToFail_IfTheObjectIsNew_AndSetIsNotEmpty()
        {
            var set = new DisjointSet<string>();
            const string o = "second";
            set.Merge("first");

            using (var enumerator = set.GetEnumerator())
            {
                enumerator.MoveNext();
                set.Merge(o);
                Assert.Throws<InvalidOperationException>(() => enumerator.MoveNext());
            }
        }

        [Test, SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void CheckAndMerge_ThrowsException_IfObject1IsNull()
        {
            var set = new DisjointSet<string>();
            Assert.Throws<ArgumentNullException>(() => set.CheckAndMerge(null, "o2"));
        }

        [Test, SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void CheckAndMerge_ThrowsException_IfObject2IsNull()
        {
            var set = new DisjointSet<string>();
            Assert.Throws<ArgumentNullException>(() => set.CheckAndMerge("o1", null));
        }

        [Test]
        public void CheckAndMerge_ReturnsTrue_IfBothObjectsAreInTheSameSet()
        {
            var set = new DisjointSet<string>();
            set.Merge("1", "2");

            Assert.IsTrue(set.CheckAndMerge("1", "2"));
        }

        [Test]
        public void CheckAndMerge_ReturnsFalse_IfBothObjectsAreInDifferentSets()
        {
            var set = new DisjointSet<string>();
            set.Merge("1");
            set.Merge("2");

            Assert.IsFalse(set.CheckAndMerge("1", "2"));
        }

        [Test]
        public void CheckAndMerge_MergesTheSets_IfRequired()
        {
            var set = new DisjointSet<string>();
            set.CheckAndMerge("1", "2");

            Assert.AreSame(set["1"], set["2"]);
        }

        [Test]
        public void CheckAndMerge_SelectsTheHeaviestSet_ForMerging()
        {
            var set = new DisjointSet<string>();
            var expectedRoot = set.Merge("1", "2");
            set.Merge("3");

            set.CheckAndMerge("2", "3");

            Assert.AreSame(expectedRoot, set["1"]);
            Assert.AreSame(expectedRoot, set["2"]);
            Assert.AreSame(expectedRoot, set["3"]);
        }
    }
}