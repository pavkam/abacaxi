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

namespace Abacaxi.Tests.SequenceExtensions
{
    using NUnit.Framework;
    using System.Diagnostics.CodeAnalysis;

    [TestFixture]
    public class FrequencyTests
    {
        [Test]
        public void Count_ReturnsValidValue()
        {
            var freq = new Frequency<char>('a', 1);

            Assert.AreEqual(1, freq.Count);
        }

        [Test]
        public void Item_ReturnsValidValue()
        {
            var freq = new Frequency<char>('a', 1);

            Assert.AreEqual('a', freq.Item);
        }

        [Test]
        public void ToString_ReturnsValidValue()
        {
            var freq = new Frequency<char>('a', 10);
            Assert.AreEqual("a (10)", freq.ToString());
        }

        [Test]
        public void Equals_ReturnsTrue_ForEqualFrequencies()
        {
            var e1 = new Frequency<char>('a', 1);
            var e2 = new Frequency<char>('a', 1);

            Assert.IsTrue(e1.Equals(e2));
        }

        [Test]
        public void Equals_ReturnsFalse_ForNonEqualCounts()
        {
            var e1 = new Frequency<char>('a', 1);
            var e2 = new Frequency<char>('a', 2);

            Assert.IsFalse(e1.Equals(e2));
        }


        [Test]
        public void Equals_ReturnsFalse_ForNonEqualItems()
        {
            var e1 = new Frequency<char>('a', 1);
            var e2 = new Frequency<char>('b', 1);

            Assert.IsFalse(e1.Equals(e2));
        }

        [Test]
        [SuppressMessage("ReSharper", "SuspiciousTypeConversion.Global")]
        public void Equals_ReturnsFalse_ForNonFrequencyObject()
        {
            var e1 = new Frequency<char>('a', 1);

            Assert.IsFalse(e1.Equals(this));
        }

        [Test]
        public void Equals_ReturnsFalse_ForNullObject()
        {
            var e1 = new Frequency<char>('a', 1);

            Assert.IsFalse(e1.Equals(null));
        }

        [Test]
        public void GetHashCode_ReturnsEqualHashCodes_ForEqualFrequencies()
        {
            var e1 = new Frequency<char>('a', 1);
            var e2 = new Frequency<char>('a', 1);

            Assert.AreEqual(e1.GetHashCode(), e2.GetHashCode());
        }

        [Test]
        public void GetHashCode_ReturnsDifferentHashCodes_ForDifferentCounts()
        {
            var e1 = new Frequency<char>('a', 1);
            var e2 = new Frequency<char>('a', 2);

            Assert.AreNotEqual(e1.GetHashCode(), e2.GetHashCode());
        }

        [Test]
        public void GetHashCode_ReturnsDifferentHashCodes_ForDifferentItems()
        {
            var e1 = new Frequency<char>('a', 1);
            var e2 = new Frequency<char>('b', 1);

            Assert.AreNotEqual(e1.GetHashCode(), e2.GetHashCode());
        }
    }
}
