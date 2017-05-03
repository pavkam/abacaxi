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

// ReSharper disable ObjectCreationAsStatement
// ReSharper disable CollectionNeverUpdated.Local
// ReSharper disable AssignNullToNotNullAttribute
// ReSharper disable ReturnValueOfPureMethodIsNotUsed

namespace Abacaxi.Tests.Containers
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Collections;
    using Abacaxi.Containers;
    using NUnit.Framework;

    [TestFixture]
    public class TrieTests
    {
        private Trie<int> _trie0;
        private Trie<int> _trie5;

        [SetUp]
        public void SetUp()
        {
            _trie0 = new Trie<int>();
            _trie5 = new Trie<int> { { "abc", 1 }, { "acb", 2 }, { "abd", 3 }, { "cbc", 4 }, { "cba", 5 } };
        }

        [Test]
        public void Ctor_ThrowsException_IfEnumerableIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new Trie<int>(null));
        }

        [Test]
        public void Ctor_ThrowsException_IfEnumerableContainsDupes()
        {
            var seq = new[]
            {
                new KeyValuePair<string, int>("1", 1),
                new KeyValuePair<string, int>("1", 2),
            };

            Assert.Throws<ArgumentException>(() => new Trie<int>(seq));
        }

        [Test]
        public void Ctor_ThrowsException_IfEnumerableContainsNullKey()
        {
            var seq = new[]
            {
                new KeyValuePair<string, int>(null, 1),
                new KeyValuePair<string, int>("2", 2),
            };

            Assert.Throws<ArgumentNullException>(() => new Trie<int>(seq));
        }

        [Test]
        public void Ctor_SetsUpAnEmptyTrie()
        {
            Assert.AreEqual(0, _trie0.Count);
        }


        [Test]
        public void Query_ReturnsNothing_IfKeyNotFound()
        {
            TestHelper.AssertSequence(_trie0.Query("hello"));
        }

        [Test]
        public void Query_ReturnsTheKey_IfOnlyKeyPrefixedWithString()
        {
            TestHelper.AssertSequence(
                _trie5.Query("abc"),
                new KeyValuePair<string, int>("abc", 1));
        }

        [Test]
        public void Query_ReturnsAllKeyValuesForAGivenPrefix()
        {
            TestHelper.AssertSequence(
                _trie5.Query("a"),
                new KeyValuePair<string, int>("acb", 2),
                new KeyValuePair<string, int>("abd", 3),
                new KeyValuePair<string, int>("abc", 1));
        }


        [Test]
        public void Query_Fails_IfAddingElement()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                // ReSharper disable once UnusedVariable
                foreach (var kvp in _trie5.Query("a"))
                {
                    _trie5.Add("test", 0);
                }
            });
        }

        [Test]
        public void Query_Fails_IfRemovingElement()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                // ReSharper disable once UnusedVariable
                foreach (var kvp in _trie5.Query("a"))
                {
                    _trie5.Remove("abc");
                }
            });
        }

        [Test]
        public void Query_Fails_IfClearing()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                // ReSharper disable once UnusedVariable
                foreach (var kvp in _trie5.Query("a"))
                {
                    _trie5.Clear();
                }
            });
        }

        [Test]
        public void Enumeration_ListsNothingForEmptyTrie()
        {
            TestHelper.AssertSequence(_trie0);
        }

        [Test]
        public void Enumeration_ListsAllElements()
        {
            TestHelper.AssertSequence(_trie5,
                new KeyValuePair<string, int>("cba", 5),
                new KeyValuePair<string, int>("cbc", 4),
                new KeyValuePair<string, int>("acb", 2),
                new KeyValuePair<string, int>("abd", 3),
                new KeyValuePair<string, int>("abc", 1)
            );
        }

        [Test]
        public void ImplicitGetEnumerator_IsActuallyTheGenericOne()
        {
            var enumerator = ((IEnumerable)_trie5).GetEnumerator();
            Assert.IsInstanceOf<IEnumerator<KeyValuePair<string, int>>>(enumerator);
        }

        [Test]
        public void Enumeration_Fails_IfAddingElement()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                // ReSharper disable once UnusedVariable
                foreach (var kvp in _trie5)
                {
                    _trie5.Add("test", 0);
                }
            });
        }

        [Test]
        public void Enumeration_Fails_IfRemovingElement()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                // ReSharper disable once UnusedVariable
                foreach (var kvp in _trie5)
                {
                    _trie5.Remove("abc");
                }
            });
        }

        [Test]
        public void Enumeration_Fails_IfClearing()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                // ReSharper disable once UnusedVariable
                foreach (var kvp in _trie5)
                {
                    _trie5.Clear();
                }
            });
        }

        [Test]
        public void Contains_ReturnsTrue_IfKeyIsContained()
        {
            Assert.IsTrue(_trie5.Contains("cba"));
        }

        [Test]
        public void Contains_ReturnsFalse_IfKeyIsNotContained()
        {
            Assert.IsFalse(_trie5.Contains("cbz"));
        }

        [Test]
        public void Contains_ReturnsTrue_IfKeyAndValueAreContained()
        {
            Assert.IsTrue(_trie5.Contains(new KeyValuePair<string, int>("cba", 5)));
        }

        [Test]
        public void Contains_ReturnsFalse_IfKeyIsContainedButNotTheValue()
        {
            Assert.IsFalse(_trie5.Contains(new KeyValuePair<string, int>("abc", -1)));
        }

        [Test]
        public void Contains_ReturnsFalse_IfKeyNotContained()
        {
            Assert.IsFalse(_trie5.Contains(new KeyValuePair<string, int>("zzz", 4)));
        }

        [Test]
        public void Contains_ThrowsException_IfKeyIsNull1()
        {
            Assert.Throws<ArgumentNullException>(() => _trie0.Contains(null));
        }

        [Test]
        public void Contains_ThrowsException_IfKeyIsNull2()
        {
            Assert.Throws<ArgumentNullException>(() => _trie0.Contains(new KeyValuePair<string, int>(null, -1)));
        }


        [Test]
        public void Clear_RemovesAllElementsFromTheTrie()
        {
            _trie5.Clear();

            TestHelper.AssertSequence(_trie5);
        }

        [Test]
        public void Clear_ResetsCountToZero()
        {
            _trie5.Clear();

            Assert.AreEqual(0, _trie5.Count);
        }

        [Test]
        public void Add_ThrowsException_IfKeyIsAlreadyInTheTrie1()
        {
            Assert.Throws<ArgumentException>(() => _trie5.Add("abc", -1));
        }

        [Test]
        public void Add_ThrowsException_IfKeyIsAlreadyInTheTrie2()
        {
            Assert.Throws<ArgumentException>(() => _trie5.Add(new KeyValuePair<string, int>("abc", -1)));
        }

        [Test]
        public void Add_ThrowsException_IfKeyIsNull1()
        {
            Assert.Throws<ArgumentNullException>(() => _trie5.Add(null, -1));
        }

        [Test]
        public void Add_ThrowsException_IfKeyIsNull2()
        {
            Assert.Throws<ArgumentNullException>(() => _trie5.Add(new KeyValuePair<string, int>(null, -1)));
        }

        [Test]
        public void Add_IncreasesTheCountByOne_IfKeyValueIsAdded()
        {
            _trie0.Add("test", 1);
            Assert.AreEqual(1, _trie0.Count);
        }

        [Test]
        public void Add_DoesNotIncreaseTheCount_IfKeyValueWasNotAdded()
        {
            var preCount = _trie5.Count;
            try
            {
                _trie5.Add("abc", -1);
            }
            // ReSharper disable once EmptyGeneralCatchClause
            catch
            {
            }

            Assert.AreEqual(preCount, _trie5.Count);
        }

        [Test]
        public void Add_ActuallyAddsTheKey()
        {
            _trie0.Add("test", 1);

            Assert.IsTrue(_trie0.Contains("test"));
        }

        [Test]
        public void Add_ActuallyAddsTheKeyAndValue()
        {
            _trie0.Add("test", 1);

            Assert.IsTrue(_trie0.Contains(new KeyValuePair<string, int>("test", 1)));
        }

        [Test]
        public void Add_WorksAsExpectedOn_EmptyString()
        {
            _trie0.Add("", 10);
            Assert.IsTrue(_trie0.Contains(""));
        }

        [Test]
        public void CopyTo_ThrowsException_ForNullArray()
        {
            Assert.Throws<ArgumentNullException>(() => _trie0.CopyTo(null, 0));
        }

        [TestCase(0, 0)]
        [TestCase(0, 1)]
        [TestCase(4, 0)]
        [TestCase(5, 1)]
        public void CopyTo_ThrowsException_ForInvalidArrayIndex(int arrayLength, int arrayIndex)
        {
            var a = new KeyValuePair<string, int>[arrayLength];
            Assert.Throws<ArgumentOutOfRangeException>(() => _trie5.CopyTo(a, arrayIndex));
        }

        [Test]
        public void CopyTo_CopiesTheContentsIntoArray()
        {
            var a = new KeyValuePair<string, int>[_trie5.Count];
            _trie5.CopyTo(a, 0);
            TestHelper.AssertSequence(_trie5, a);
        }

        [Test]
        public void CopyTo_TakesArrayIndexIntoAccount()
        {
            var a = new KeyValuePair<string, int>[10];
            _trie0.Add("alex", 100);
            _trie0.CopyTo(a, 9);

            Assert.AreEqual(_trie0.Single(), a[9]);
        }

        [Test]
        public void Remove_ThrowsException_IfKeyIsNull1()
        {
            Assert.Throws<ArgumentNullException>(() => _trie0.Remove(null));
        }

        [Test]
        public void Remove_ThrowsException_IfKeyIsNull2()
        {
            Assert.Throws<ArgumentNullException>(() => _trie0.Remove(new KeyValuePair<string, int>(null, -1)));
        }

        [Test]
        public void Remove_ReturnsTrue_IfKeyValueWasRemoved()
        {
            var result = _trie5.Remove("abc");

            Assert.IsTrue(result);
        }

        [Test]
        public void Remove_ReturnsFalse_IfKeyValueWasNotRemoved()
        {
            var result = _trie5.Remove("abz");

            Assert.IsFalse(result);
        }

        [Test]
        public void Remove_DecreasesTheCountByOne_IfKeyValueWasRemoved()
        {
            var count = _trie5.Count;
            _trie5.Remove("abc");

            Assert.AreEqual(count - 1, _trie5.Count);
        }

        [Test]
        public void Remove_DoesNotChangeTheCount_IfKeyValueWasNotRemoved()
        {
            var count = _trie5.Count;
            _trie5.Remove("abz");

            Assert.AreEqual(count, _trie5.Count);
        }

        [Test]
        public void Remove_ReturnsFalse_IfKeyPresentButValueIsNot()
        {
            var result = _trie5.Remove(new KeyValuePair<string, int>("abc", -1));

            Assert.IsFalse(result);
        }

        [Test]
        public void Remove_ReturnsTrue_IfKeyAndValueAreBothPresent()
        {
            var result = _trie5.Remove(new KeyValuePair<string, int>("abc", 1));

            Assert.IsTrue(result);
        }

        [Test]
        public void Remove_WorksAsExpectedOn_EmptyString()
        {
            _trie0.Add("", 10);

            Assert.IsTrue(_trie0.Remove(""));
        }

        [Test]
        public void Count_IsEqualToTheNumberOfKeyValuesReturnedByEnumeration()
        {
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            var enumerated = _trie5.Count();

            Assert.AreEqual(enumerated, _trie5.Count);
        }

        [Test]
        public void IsReadOnly_ReturnsFalse()
        {
            Assert.IsFalse(_trie0.IsReadOnly);
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(5)]
        public void Trie_StaysConsistent_ForLargeNumberOfElements(int cx)
        {
            for (var i = 'A'; i < 'A' + cx; i++)
            {
                for (var j = 'A'; j < 'A' + cx; j++)
                {
                    for (var z = 'A'; z < 'A' + cx; z++)
                    {
                        _trie0.Add($"{i}{j}{z}", 0);
                    }
                }
            }

            var ex = cx * cx * cx;
            Assert.AreEqual(ex, _trie0.Count);

            for (var i = 'A'; i < 'A' + cx; i++)
            {
                for (var j = 'A'; j < 'A' + cx; j++)
                {
                    for (var z = 'A'; z < 'A' + cx; z++)
                    {
                        var removed = _trie0.Remove($"{i}{j}{z}");
                        Assert.IsTrue(removed);
                    }
                }
            }

            Assert.AreEqual(0, _trie0.Count);
        }
    }
}
