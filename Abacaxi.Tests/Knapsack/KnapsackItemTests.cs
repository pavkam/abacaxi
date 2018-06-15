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

namespace Abacaxi.Tests.Knapsack
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using NUnit.Framework;

    [TestFixture]
    public class KnapsackItemTests
    {
        [Test, SuppressMessage("ReSharper", "ObjectCreationAsStatement")]
        public void Ctor_ThrowsException_IfValueIsZeroOrLess()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new KnapsackItem<char>('z', 0, 1));
        }

        [Test, SuppressMessage("ReSharper", "ObjectCreationAsStatement")]
        public void Ctor_ThrowsException_IfWeightIsZeroOrLess()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new KnapsackItem<char>('z', 1, 0));
        }

        [Test]
        public void Equals_ReturnsFalse_ForNonEqualItems()
        {
            var e1 = new KnapsackItem<char>('a', 1, 2);
            var e2 = new KnapsackItem<char>('b', 1, 2);

            Assert.IsFalse(e1.Equals(e2));
        }

        [Test]
        public void Equals_ReturnsFalse_ForNonEqualValues()
        {
            var e1 = new KnapsackItem<char>('a', 1, 2);
            var e2 = new KnapsackItem<char>('a', 2, 2);

            Assert.IsFalse(e1.Equals(e2));
        }

        [Test]
        public void Equals_ReturnsFalse_ForNonEqualWeights()
        {
            var e1 = new KnapsackItem<char>('a', 1, 2);
            var e2 = new KnapsackItem<char>('a', 1, 3);

            Assert.IsFalse(e1.Equals(e2));
        }

        [Test, SuppressMessage("ReSharper", "SuspiciousTypeConversion.Global")]
        public void Equals_ReturnsFalse_ForNonKnapsackItemObject()
        {
            var e1 = new KnapsackItem<char>('a', 1, 2);
            Assert.IsFalse(e1.Equals(this));
        }

        [Test]
        public void Equals_ReturnsFalse_ForNullObject()
        {
            var e1 = new KnapsackItem<char>('a', 1, 2);
            Assert.IsFalse(e1.Equals(null));
        }

        [Test]
        public void Equals_ReturnsTrue_ForEqualKnapsackItems()
        {
            var e1 = new KnapsackItem<char>('a', 1, 2);
            var e2 = new KnapsackItem<char>('a', 1, 2);

            Assert.IsTrue(e1.Equals(e2));
        }

        [Test]
        public void GetHashCode_ReturnsDifferentHashCodes_ForDifferentItems()
        {
            var e1 = new KnapsackItem<char>('a', 1, 2);
            var e2 = new KnapsackItem<char>('b', 1, 2);

            Assert.AreNotEqual(e1.GetHashCode(), e2.GetHashCode());
        }

        [Test]
        public void GetHashCode_ReturnsDifferentHashCodes_ForDifferentValues()
        {
            var e1 = new KnapsackItem<char>('a', 1, 2);
            var e2 = new KnapsackItem<char>('a', 2, 2);

            Assert.AreNotEqual(e1.GetHashCode(), e2.GetHashCode());
        }

        [Test]
        public void GetHashCode_ReturnsDifferentHashCodes_ForDifferentWeights()
        {
            var e1 = new KnapsackItem<char>('a', 1, 2);
            var e2 = new KnapsackItem<char>('a', 1, 3);

            Assert.AreNotEqual(e1.GetHashCode(), e2.GetHashCode());
        }

        [Test]
        public void GetHashCode_ReturnsEqualHashCodes_ForEqualKnapsackItems()
        {
            var e1 = new KnapsackItem<char>('a', 1, 2);
            var e2 = new KnapsackItem<char>('a', 1, 2);

            Assert.AreEqual(e1.GetHashCode(), e2.GetHashCode());
        }

        [Test]
        public void Item_ReturnsValidValue()
        {
            var item = new KnapsackItem<char>('a', 10, 20);
            Assert.AreEqual('a', item.Item);
        }

        [Test]
        public void ToString_ReturnsValidValue()
        {
            var item = new KnapsackItem<char>('a', 1.2, 3);
            Assert.AreEqual($"a ({1.20:N2}, 3)", item.ToString());
        }

        [Test]
        public void Value_ReturnsValidValue()
        {
            var item = new KnapsackItem<char>('a', 10, 20);
            Assert.AreEqual(10, item.Value);
        }

        [Test]
        public void Weight_ReturnsValidValue()
        {
            var item = new KnapsackItem<char>('a', 10, 20);
            Assert.AreEqual(20, item.Weight);
        }
    }
}