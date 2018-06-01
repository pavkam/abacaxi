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

namespace Abacaxi.Tests.SequenceExtensions
{
    using System.Diagnostics.CodeAnalysis;
    using NUnit.Framework;

    [TestFixture]
    public class EditTests
    {
        [TestCase(EditOperation.Match, 'a', "=a"), TestCase(EditOperation.Delete, 'b', "-b"),
         TestCase(EditOperation.Insert, 'c', "+c"), TestCase(EditOperation.Substitute, 'd', "#d")]
        public void ToString_ReturnsValidValue(EditOperation op, char ch, string expected)
        {
            var edit = new Edit<char>(op, ch);
            Assert.AreEqual(expected, edit.ToString());
        }

        [Test, SuppressMessage("ReSharper", "SuspiciousTypeConversion.Global")]
        public void Equals_ReturnsFalse_ForNonEditObject()
        {
            var e1 = new Edit<char>(EditOperation.Match, 'a');

            Assert.IsFalse(e1.Equals(this));
        }


        [Test]
        public void Equals_ReturnsFalse_ForNonEqualItems()
        {
            var e1 = new Edit<char>(EditOperation.Match, 'a');
            var e2 = new Edit<char>(EditOperation.Match, 'b');

            Assert.IsFalse(e1.Equals(e2));
        }

        [Test]
        public void Equals_ReturnsFalse_ForNonEqualOperations()
        {
            var e1 = new Edit<char>(EditOperation.Insert, 'a');
            var e2 = new Edit<char>(EditOperation.Match, 'a');

            Assert.IsFalse(e1.Equals(e2));
        }

        [Test]
        public void Equals_ReturnsFalse_ForNullObject()
        {
            var e1 = new Edit<char>(EditOperation.Match, 'a');

            Assert.IsFalse(e1.Equals(null));
        }

        [Test]
        public void Equals_ReturnsTrue_ForEqualEdits()
        {
            var e1 = new Edit<char>(EditOperation.Insert, 'a');
            var e2 = new Edit<char>(EditOperation.Insert, 'a');

            Assert.IsTrue(e1.Equals(e2));
        }

        [Test]
        public void GetHashCode_ReturnsDifferentHashCodes_ForDifferentItems()
        {
            var e1 = new Edit<char>(EditOperation.Match, 'a');
            var e2 = new Edit<char>(EditOperation.Match, 'b');

            Assert.AreNotEqual(e1.GetHashCode(), e2.GetHashCode());
        }

        [Test]
        public void GetHashCode_ReturnsDifferentHashCodes_ForDifferentOperations()
        {
            var e1 = new Edit<char>(EditOperation.Match, 'a');
            var e2 = new Edit<char>(EditOperation.Insert, 'a');

            Assert.AreNotEqual(e1.GetHashCode(), e2.GetHashCode());
        }

        [Test]
        public void GetHashCode_ReturnsEqualHashCodes_ForEqualEdits()
        {
            var e1 = new Edit<char>(EditOperation.Match, 'a');
            var e2 = new Edit<char>(EditOperation.Match, 'a');

            Assert.AreEqual(e1.GetHashCode(), e2.GetHashCode());
        }

        [Test]
        public void Item_ReturnsValidValue()
        {
            var edit = new Edit<char>(EditOperation.Insert, 'a');

            Assert.AreEqual('a', edit.Item);
        }

        [Test]
        public void Operation_ReturnsValidValue()
        {
            var edit = new Edit<char>(EditOperation.Insert, 'a');

            Assert.AreEqual(EditOperation.Insert, edit.Operation);
        }
    }
}