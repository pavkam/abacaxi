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
        private Trie<char, int> _trie0;
        private Trie<char, int> _trie5;

        private static char[] _(string a)
        {
            return a?.ToCharArray();
        }

        [SetUp]
        public void SetUp()
        {
            _trie0 = new Trie<char, int>();
            _trie5 = new Trie<char, int> { { _("abc"), 1 }, { _("acb"), 2 }, { _("abd"), 3 }, { _("cbc"), 4 }, { _("cba"), 5 } };
        }

        [Test]
        [SuppressMessage("ReSharper", "ObjectCreationAsStatement")]
        [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void Ctor_ThrowsException_IfEnumerableIsNull1()
        {
            Assert.Throws<ArgumentNullException>(() => new Trie<char, int>((IEnumerable<KeyValuePair<char[], int>>)null));
        }

        [Test]
        [SuppressMessage("ReSharper", "ObjectCreationAsStatement")]
        [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void Ctor_ThrowsException_IfEnumerableIsNull2()
        {
            Assert.Throws<ArgumentNullException>(() => new Trie<char, int>(null, EqualityComparer<char>.Default));
        }

        [Test]
        [SuppressMessage("ReSharper", "ObjectCreationAsStatement")]
        [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void Ctor_ThrowsException_IfComparerIsNull1()
        {
            Assert.Throws<ArgumentNullException>(() => new Trie<char, int>((IEqualityComparer<char>)null));
        }

        [Test]
        [SuppressMessage("ReSharper", "ObjectCreationAsStatement")]
        [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void Ctor_ThrowsException_IfComparerIsNull2()
        {
            Assert.Throws<ArgumentNullException>(() => new Trie<char, int>(new KeyValuePair<char[], int>[] { }, null));
        }

        [Test]
        [SuppressMessage("ReSharper", "ObjectCreationAsStatement")]
        public void Ctor_ThrowsException_IfEnumerableContainsDupes()
        {
            var seq = new[]
            {
                new KeyValuePair<char[], int>(_("1"), 1),
                new KeyValuePair<char[], int>(_("1"), 2),
            };

            Assert.Throws<ArgumentException>(() => new Trie<char, int>(seq));
        }

        [Test]
        [SuppressMessage("ReSharper", "ObjectCreationAsStatement")]
        public void Ctor_ThrowsException_IfEnumerableContainsNullKey()
        {
            var seq = new[]
            {
                new KeyValuePair<char[], int>(_(null), 1),
                new KeyValuePair<char[], int>(_("2"), 2),
            };

            Assert.Throws<ArgumentNullException>(() => new Trie<char, int>(seq));
        }

        [Test]
        public void Ctor_SetsUpAnEmptyTrie()
        {
            Assert.AreEqual(0, _trie0.Count);
        }


        [Test]
        public void Query_ReturnsNothing_IfKeyNotFound()
        {
            TestHelper.AssertSequence(_trie0.Query(_("hello")));
        }

        [Test]
        public void Query_ReturnsTheKey_IfOnlyKeyPrefixedWithString()
        {
            TestHelper.AssertSequence(
                _trie5.Query(_("abc")),
                new KeyValuePair<char[], int>(_("abc"), 1));
        }

        [Test]
        public void Query_ReturnsAllKeyValuesForAGivenPrefix()
        {
            TestHelper.AssertSequence(
                _trie5.Query(_("a")),
                new KeyValuePair<char[], int>(_("acb"), 2),
                new KeyValuePair<char[], int>(_("abd"), 3),
                new KeyValuePair<char[], int>(_("abc"), 1));
        }


        [Test]
        public void Query_Fails_IfAddingElement()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                // ReSharper disable once UnusedVariable
                foreach (var kvp in _trie5.Query(_("a")))
                {
                    _trie5.Add(_("test"), 0);
                }
            });
        }

        [Test]
        public void Query_Fails_IfRemovingElement()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                // ReSharper disable once UnusedVariable
                foreach (var kvp in _trie5.Query(_("a")))
                {
                    _trie5.Remove(_("abc"));
                }
            });
        }

        [Test]
        public void Query_Fails_IfClearing()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                // ReSharper disable once UnusedVariable
                foreach (var kvp in _trie5.Query(_("a")))
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
                new KeyValuePair<char[], int>(_("cba"), 5),
                new KeyValuePair<char[], int>(_("cbc"), 4),
                new KeyValuePair<char[], int>(_("acb"), 2),
                new KeyValuePair<char[], int>(_("abd"), 3),
                new KeyValuePair<char[], int>(_("abc"), 1)
            );
        }

        [Test]
        public void ImplicitGetEnumerator_IsActuallyTheGenericOne()
        {
            var enumerator = ((IEnumerable)_trie5).GetEnumerator();
            Assert.IsInstanceOf<IEnumerator<KeyValuePair<char[], int>>>(enumerator);
        }

        [Test]
        public void Enumeration_Fails_IfAddingElement()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                // ReSharper disable once UnusedVariable
                foreach (var kvp in _trie5)
                {
                    _trie5.Add(_("test"), 0);
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
                    _trie5.Remove(_("abc"));
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
            Assert.IsTrue(_trie5.Contains(_("cba")));
        }

        [Test]
        public void Contains_ReturnsFalse_IfKeyIsNotContained()
        {
            Assert.IsFalse(_trie5.Contains(_("cbz")));
        }

        [Test]
        public void Contains_ReturnsTrue_IfKeyAndValueAreContained()
        {
            Assert.IsTrue(_trie5.Contains(new KeyValuePair<char[], int>(_("cba"), 5)));
        }

        [Test]
        public void Contains_ReturnsFalse_IfKeyIsContainedButNotTheValue()
        {
            Assert.IsFalse(_trie5.Contains(new KeyValuePair<char[], int>(_("abc"), -1)));
        }

        [Test]
        public void Contains_ReturnsFalse_IfKeyNotContained()
        {
            Assert.IsFalse(_trie5.Contains(new KeyValuePair<char[], int>(_("zzz"), 4)));
        }

        [Test]
        [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void Contains_ThrowsException_IfKeyIsNull1()
        {
            Assert.Throws<ArgumentNullException>(() => _trie0.Contains(null));
        }

        [Test]
        [SuppressMessage("ReSharper", "ReturnValueOfPureMethodIsNotUsed")]
        public void Contains_ThrowsException_IfKeyIsNull2()
        {
            Assert.Throws<ArgumentNullException>(() => _trie0.Contains(new KeyValuePair<char[], int>(null, -1)));
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
            Assert.Throws<ArgumentException>(() => _trie5.Add(_("abc"), -1));
        }

        [Test]
        public void Add_ThrowsException_IfKeyIsAlreadyInTheTrie2()
        {
            Assert.Throws<ArgumentException>(() => (_trie5 as ICollection<KeyValuePair<char[], int>>).Add(new KeyValuePair<char[], int>(_("abc"), -1)));
        }

        [Test]
        [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void Add_ThrowsException_IfKeyIsNull1()
        {
            Assert.Throws<ArgumentNullException>(() => _trie5.Add(null, -1));
        }

        [Test]
        public void Add_ThrowsException_IfKeyIsNull2()
        {
            Assert.Throws<ArgumentNullException>(() => (_trie5 as ICollection<KeyValuePair<char[], int>>).Add(new KeyValuePair<char[], int>(null, -1)));
        }

        [Test]
        public void Add_IncreasesTheCountByOne_IfKeyValueIsAdded()
        {
            _trie0.Add(_("test"), 1);
            Assert.AreEqual(1, _trie0.Count);
        }

        [Test]
        public void Add_DoesNotIncreaseTheCount_IfKeyValueWasNotAdded()
        {
            var preCount = _trie5.Count;
            try
            {
                _trie5.Add(_("abc"), -1);
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
            _trie0.Add(_("test"), 1);

            Assert.IsTrue(_trie0.Contains(_("test")));
        }

        [Test]
        public void Add_ActuallyAddsTheKeyAndValue()
        {
            _trie0.Add(_("test"), 1);

            Assert.IsTrue(_trie0.Contains(new KeyValuePair<char[], int>(_("test"), 1)));
        }

        [Test]
        public void Add_WorksAsExpectedOn_EmptyString()
        {
            _trie0.Add(_(""), 10);
            Assert.IsTrue(_trie0.Contains(_("")));
        }

        [Test]
        [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void AddOrUpdate_ThrowsException_IfKeyIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => _trie5.AddOrUpdate(null, 1, i => i));
        }

        [Test]
        [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void AddOrUpdate_ThrowsException_IfUpdateFuncIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => _trie5.AddOrUpdate(_("a"), 1, null));
        }

        [Test]
        public void AddOrUpdate_ReturnsTrue_IfKeyValueIsAdded()
        {
            Assert.IsTrue(_trie0.AddOrUpdate(_("test"), 1, i => i));
        }

        [Test]
        public void AddOrUpdate_IncreasesTheCountByOne_IfKeyValueIsAdded()
        {
            _trie0.AddOrUpdate(_("test"), 1, i => i);
            Assert.AreEqual(1, _trie0.Count);
        }

        [Test]
        public void AddOrUpdate_ReturnsFalse_IfKeyValueIsUpdated()
        {
            Assert.IsFalse(_trie5.AddOrUpdate(_("abc"), 1, i => i));
        }

        [Test]
        public void AddOrUpdate_DoesNotIncreaseTheCount_IfValueIsUpdated()
        {
            var preCount = _trie5.Count;
            _trie5.AddOrUpdate(_("abc"), -1, i => -1);

            Assert.AreEqual(preCount, _trie5.Count);
        }

        [Test]
        public void AddOrUpdate_AddsTheKeyValue_IfTheKeyIsNotFound()
        {
            _trie0.AddOrUpdate(_("test"), 1, i => i);

            Assert.IsTrue(_trie0.TryGetValue(_("test"), out var value));
            Assert.AreEqual(1, value);
        }

        [Test]
        public void AddOrUpdate_UpdatesTheValue_IfTheKeyIsFound()
        {
            _trie5.AddOrUpdate(_("abc"), 1, i => -1);

            Assert.IsTrue(_trie5.TryGetValue(_("abc"), out var value));
            Assert.AreEqual(-1, value);
        }

        [Test]
        [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
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
            var a = new KeyValuePair<char[], int>[arrayLength];
            Assert.Throws<ArgumentOutOfRangeException>(() => _trie5.CopyTo(a, arrayIndex));
        }

        [Test]
        public void CopyTo_CopiesTheContentsIntoArray()
        {
            var a = new KeyValuePair<char[], int>[_trie5.Count];
            _trie5.CopyTo(a, 0);
            TestHelper.AssertSequence(_trie5, a);
        }

        [Test]
        public void CopyTo_TakesArrayIndexIntoAccount()
        {
            var a = new KeyValuePair<char[], int>[10];
            _trie0.Add(_("alex"), 100);
            _trie0.CopyTo(a, 9);

            Assert.AreEqual(_trie0.Single(), a[9]);
        }

        [Test]
        [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void Remove_ThrowsException_IfKeyIsNull1()
        {
            Assert.Throws<ArgumentNullException>(() => _trie0.Remove(null));
        }

        [Test]
        public void Remove_ThrowsException_IfKeyIsNull2()
        {
            Assert.Throws<ArgumentNullException>(() => (_trie0 as ICollection<KeyValuePair<char[], int>>).Remove(new KeyValuePair<char[], int>(null, -1)));
        }

        [Test]
        public void Remove_ReturnsTrue_IfKeyValueWasRemoved()
        {
            var result = _trie5.Remove(_("abc"));

            Assert.IsTrue(result);
        }

        [Test]
        public void Remove_ReturnsFalse_IfKeyValueWasNotRemoved()
        {
            var result = _trie5.Remove(_("abz"));

            Assert.IsFalse(result);
        }

        [Test]
        public void Remove_DecreasesTheCountByOne_IfKeyValueWasRemoved()
        {
            var count = _trie5.Count;
            _trie5.Remove(_("abc"));

            Assert.AreEqual(count - 1, _trie5.Count);
        }

        [Test]
        public void Remove_DoesNotChangeTheCount_IfKeyValueWasNotRemoved()
        {
            var count = _trie5.Count;
            _trie5.Remove(_("abz"));

            Assert.AreEqual(count, _trie5.Count);
        }

        [Test]
        public void Remove_ReturnsFalse_IfKeyPresentButValueIsNot()
        {
            var result = (_trie5 as ICollection<KeyValuePair<char[], int>>).Remove(new KeyValuePair<char[], int>(_("abc"), -1));

            Assert.IsFalse(result);
        }

        [Test]
        public void Remove_ReturnsTrue_IfKeyAndValueAreBothPresent()
        {
            var result = (_trie5 as ICollection<KeyValuePair<char[], int>>).Remove(new KeyValuePair<char[], int>(_("abc"), 1));

            Assert.IsTrue(result);
        }

        [Test]
        public void Remove_WorksAsExpectedOn_EmptyString()
        {
            _trie0.Add(_(""), 10);

            Assert.IsTrue(_trie0.Remove(_("")));
        }

        [Test]
        [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void TryGetValue_ThrowsException_IfKeyIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => _trie5.TryGetValue(null, out var dummy));
        }

        [Test]
        public void TryGetValue_ReturnsFalse_IfTheKeyIsNotFound()
        {
            Assert.IsFalse(_trie5.TryGetValue(_("cbz"), out var dummy));
        }

        [Test]
        public void TryGetValue_ReturnsTrue_IfTheKeyIsFound()
        {
            Assert.IsTrue(_trie5.TryGetValue(_("cba"), out var dummy));
        }

        [Test]
        public void TryGetValue_SetsTheOutputValue_IfTheKeyIsFound()
        {
            _trie5.TryGetValue(_("cba"), out var output);

            Assert.AreEqual(5, output);
        }

        [Test]
        public void Count_IsEqualToTheNumberOfKeyValuesReturnedByEnumeration()
        {
            var enumerated = _trie5.Count;

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
                        _trie0.Add(_($"{i}{j}{z}"), 0);
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
                        var removed = _trie0.Remove(_($"{i}{j}{z}"));
                        Assert.IsTrue(removed);
                    }
                }
            }

            Assert.AreEqual(0, _trie0.Count);
        }

        [Test]
        public void Trie_TakesIntoAccountTheComparer()
        {
            var trie = new Trie<string, int>(StringComparer.OrdinalIgnoreCase) {{new[] {"A"}, 1}};

            Assert.IsTrue(trie.Contains(new[] {"a"}));
        }
    }
}
