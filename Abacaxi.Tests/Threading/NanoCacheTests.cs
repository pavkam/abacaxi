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

namespace Abacaxi.Tests.Threading
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using Abacaxi.Threading;
    using NUnit.Framework;

    [TestFixture]
    public sealed class NanoCacheTests
    {
        [Test]
        public void Count_IncludesExpiredItems()
        {
            var cache = new NanoCache<string, int>(10)
            {
                ["1"] = 1
            };

            Thread.Sleep(100);

            Assert.AreEqual(1, cache.Count);
        }

        [Test]
        public void Count_IsDecrementedWhenItemIsRemoved()
        {
            var cache = new NanoCache<string, int>
            {
                ["1"] = 1
            };

            cache.Remove("1");
            Assert.AreEqual(0, cache.Count);
        }


        [Test]
        public void Count_IsDecrementedWhenItemIsReplacedWithDefaultValue()
        {
            var cache = new NanoCache<string, int>
            {
                ["1"] = 1,
                ["1"] = 0
            };

            Assert.AreEqual(0, cache.Count);
        }

        [Test]
        public void Count_IsIncrementedWhenItemIsAdded()
        {
            var cache = new NanoCache<string, int> { ["1"] = 1 };
            Assert.AreEqual(1, cache.Count);
        }

        [Test]
        public void Count_IsNotIncrementedWhenItemIsUpdated()
        {
            var cache = new NanoCache<string, int>
            {
                ["1"] = 1,
                ["1"] = 2
            };

            Assert.AreEqual(1, cache.Count);
        }

        [Test]
        public void Count_IsUpdatedWhenExpiredItemIsFlushed1()
        {
            var cache = new NanoCache<string, int>(10)
            {
                ["1"] = 1
            };

            Thread.Sleep(100);

            cache.TryGetValue("1", out var _);

            Assert.AreEqual(0, cache.Count);
        }

        [Test]
        public void Count_IsUpdatedWhenExpiredItemIsFlushed2()
        {
            var cache = new NanoCache<string, int>(10)
            {
                ["1"] = 1
            };

            Thread.Sleep(100);

            Assert.AreEqual(0, cache["1"]);
            Assert.AreEqual(0, cache.Count);
        }

        [Test]
        public void Count_IsUpdatedWhenExpiredItemIsFlushed3()
        {
            var cache = new NanoCache<string, int>(10)
            {
                ["1"] = 1
            };

            Thread.Sleep(100);

            cache.Remove("1");

            Assert.AreEqual(0, cache.Count);
        }

        [Test]
        public void Count_IsZeroOnCreation()
        {
            var cache = new NanoCache<string, int>();

            Assert.AreEqual(0, cache.Count);
        }

        [Test]
        public void Ctor_TakesEqualityComparer_IntoConsideration()
        {
            var cache = new NanoCache<string, int>(StringComparer.OrdinalIgnoreCase) { ["a"] = 999 };
            Assert.AreEqual(cache["A"], 999);
        }

        [Test, SuppressMessage("ReSharper", "AssignNullToNotNullAttribute"),
         SuppressMessage("ReSharper", "ObjectCreationAsStatement")]
        public void Ctor_ThrowsException_ForNullEqualityComparer()
        {
            Assert.Throws<ArgumentNullException>(() => new NanoCache<string, object>(null));
        }

        [Test]
        public void Ctor_UsesDefaultEqualityComparer()
        {
            var cache = new NanoCache<string, int> { ["a"] = 999 };
            Assert.AreEqual(cache["A"], 0);
        }

        [Test,
         SuppressMessage("ReSharper", "ObjectCreationAsStatement")]
        public void Ctor1_ThrowsException_ForTtlLessThanMinusOne()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                new NanoCache<string, object>(StringComparer.OrdinalIgnoreCase, -2));
        }

        [Test,
         SuppressMessage("ReSharper", "ObjectCreationAsStatement")]
        public void Ctor2_ThrowsException_ForTtlLessThanMinusOne()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new NanoCache<string, object>(-2));
        }

        [Test]
        public void Flush_DoesNothingForUnExpiredItems()
        {
            var cache = new NanoCache<string, int>
            {
                ["1"] = 1
            };

            cache.Flush();

            Assert.AreEqual(1, cache.Count);
        }

        [Test]
        public void Flush_RemovesTheExpiredItems()
        {
            var cache = new NanoCache<string, int>(10)
            {
                ["1"] = 1
            };

            Thread.Sleep(100);

            cache.Flush();

            Assert.AreEqual(0, cache.Count);
        }

        [Test]
        public void Flush_RemovesTheExpiredItemsWhileLeavingTheNormalOnes()
        {
            var cache = new NanoCache<string, int>(10)
            {
                ["1"] = 1
            };

            Thread.Sleep(100);

            cache["2"] = 2;
            cache.Flush();

            Assert.AreEqual(1, cache.Count);
        }

        [Test]
        public void Indexer_DoesNotAcknowledgeExpiredItems()
        {
            var cache = new NanoCache<string, int>(10) { ["item"] = 1234 };
            Thread.Sleep(100);

            Assert.AreEqual(0, cache["item"]);
        }

        [Test,
         SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void Indexer_Get_ThrowsException_IfKeyIsNull()
        {
            var cache = new NanoCache<string, int>();
            Assert.Throws<ArgumentNullException>(() => Assert.AreEqual(0, cache[null]));
        }

        [Test]
        public void Indexer_RefreshesValueOfExpiredItem()
        {
            var cache = new NanoCache<string, int>(10) { ["item"] = 1234 };
            Thread.Sleep(100);
            cache["item"] = 9876;

            Assert.AreEqual(9876, cache["item"]);
        }

        [Test]
        public void Indexer_RemovesTheValueFromTheCacheIfDefaultProvided()
        {
            var cache = new NanoCache<string, int>
            {
                ["item"] = 999,
                ["item"] = 0
            };

            Assert.IsFalse(cache.Remove("item"));
        }

        [Test]
        public void Indexer_ReplacesAnExistingValueInTheCache()
        {
            var cache = new NanoCache<string, int>
            {
                ["item"] = 999,
                ["item"] = 111
            };

            Assert.AreEqual(cache["item"], 111);
        }

        [Test]
        public void Indexer_ReturnsDefaultValueIfKeyIsUnknown()
        {
            var cache = new NanoCache<string, int>();
            Assert.AreEqual(cache["item"], 0);
        }

        [Test]
        public void Indexer_ReturnsNullValueIfKeyIsUnknown()
        {
            var cache = new NanoCache<string, object>();
            Assert.IsNull(cache["item"]);
        }

        [Test,
         SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void Indexer_Set_ThrowsException_IfKeyIsNull()
        {
            var cache = new NanoCache<string, int>();
            Assert.Throws<ArgumentNullException>(() => cache[null] = 123);
        }

        [Test]
        public void Indexer_StoresANewValueIntoTheCache()
        {
            var cache = new NanoCache<string, int> { ["item"] = 999 };
            Assert.AreEqual(cache["item"], 999);
        }

        [Test]
        public void Remove_DoesNotAcknowledgeExpiredItems()
        {
            var cache = new NanoCache<string, int>(10) { ["item"] = 1234 };
            Thread.Sleep(100);

            Assert.IsFalse(cache.Remove("item"));
        }

        [Test]
        public void Remove_RemovesAnExistingItemFromTheCache()
        {
            var cache = new NanoCache<string, int>
            {
                ["item"] = 999
            };

            cache.Remove("item");
            Assert.AreEqual(cache["item"], 0);
        }

        [Test]
        public void Remove_ReturnsFalseIfItemNotRemoved()
        {
            var cache = new NanoCache<string, int>();
            Assert.IsFalse(cache.Remove("item"));
        }

        [Test]
        public void Remove_ReturnsTrueIfItemRemoved()
        {
            var cache = new NanoCache<string, int>
            {
                ["item"] = 999
            };

            Assert.IsTrue(cache.Remove("item"));
        }

        [Test,
         SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void Remove_ThrowsException_IfKeyIsNull()
        {
            var cache = new NanoCache<string, int>();

            Assert.Throws<ArgumentNullException>(() => cache.Remove(null));
        }

        [Test]
        public void TryGetValue_ReturnsDefaultIfNotExists()
        {
            var cache = new NanoCache<string, int>();

            cache.TryGetValue("item", out var value);
            Assert.AreEqual(0, value);
        }

        [Test]
        public void TryGetValue_ReturnsFalseIfItemDoesNotExist()
        {
            var cache = new NanoCache<string, int>();
            Assert.IsFalse(cache.TryGetValue("item", out var _));
        }

        [Test]
        public void TryGetValue_ReturnsFalseIfItemExpired()
        {
            var cache = new NanoCache<string, int>(10) { ["item"] = 1234 };
            Thread.Sleep(100);

            Assert.IsFalse(cache.TryGetValue("item", out var _));
        }

        [Test]
        public void TryGetValue_ReturnsItemIfExists()
        {
            var cache = new NanoCache<string, int> { ["item"] = 1234 };

            cache.TryGetValue("item", out var value);
            Assert.AreEqual(1234, value);
        }

        [Test]
        public void TryGetValue_ReturnsTrueIfItemDoesExist()
        {
            var cache = new NanoCache<string, int> { ["item"] = 1234 };
            Assert.IsTrue(cache.TryGetValue("item", out var _));
        }

        [Test,
         SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void TryGetValue_ThrowsException_IfKeyIsNull()
        {
            var cache = new NanoCache<string, int>();

            Assert.Throws<ArgumentNullException>(() => cache.TryGetValue(null, out var _));
        }
    }
}