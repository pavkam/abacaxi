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
    using NUnit.Framework;

    [TestFixture]
    public sealed class StringHelperMethodsTests
    {
        [Test]
        public void AsList_ThrowsException_IfStringIsNull1()
        {
            Assert.Throws<ArgumentNullException>(() => ((string) null).AsList());
        }

        [TestCase("", "")]
        [TestCase("a", "a")]
        [TestCase("abc", "a,b,c")]
        public void AsList_GetEnumerator_ReturnsTheExpectedSequence(string s, string expected)
        {
            var actual = string.Join(",", s.AsList());
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void AsList_ThenAdd_ThrowsException()
        {
            var list = "test".AsList();
            Assert.Throws<NotSupportedException>(() => list.Add('a'));
        }

        [Test]
        public void AsList_ThenClear_ThrowsException()
        {
            var list = "test".AsList();
            Assert.Throws<NotSupportedException>(() => list.Clear());
        }

        [Test]
        public void AsList_ThenRemove_ThrowsException()
        {
            var list = "test".AsList();
            Assert.Throws<NotSupportedException>(() => list.Remove('t'));
        }

        [Test]
        public void AsList_ThenInsert_ThrowsException()
        {
            var list = "test".AsList();
            Assert.Throws<NotSupportedException>(() => list.Insert(0, 't'));
        }

        [Test]
        public void AsList_ThenRemoveAt_ThrowsException()
        {
            var list = "test".AsList();
            Assert.Throws<NotSupportedException>(() => list.RemoveAt(0));
        }

        [Test]
        public void AsList_ThenIndexer_ThrowsException()
        {
            var list = "test".AsList();
            Assert.Throws<NotSupportedException>(() => list[0] = 'a');
        }

        [Test]
        public void AsList_ThenContains_ReturnsExpectedIndex()
        {
            var list = "test".AsList();
            Assert.AreEqual(1, list.IndexOf('e'));
        }

        [Test]
        public void AsList_ThenCount_ReturnsExpectedLength()
        {
            var list = "test".AsList();
            Assert.AreEqual(4, list.Count);
        }

        [Test]
        public void AsList_ThenIsReadOnly_ReturnsTrue()
        {
            var list = "test".AsList();
            Assert.IsTrue(list.IsReadOnly);
        }

        [Test]
        public void AsList_ThenIndexOf_ReturnsTheExpectedIndex()
        {
            var list = "test".AsList();
            Assert.AreEqual(2, list.IndexOf('s'));
        }

        [Test]
        public void AsList_ThenThis_ReturnsTheExpectedElement()
        {
            var list = "test".AsList();
            Assert.AreEqual('s', list[2]);
        }

        [Test]
        [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void AsList_ThenCopyTo_ThrowsException_IfArrayIsNull()
        {
            var list = "test".AsList();
            Assert.Throws<ArgumentNullException>(() => list.CopyTo(null, 0));
        }

        [Test]
        public void AsList_ThenCopyTo_ThrowsException_IfArrayIsTooSmall()
        {
            var list = "test".AsList();
            var a = new char[3];
            Assert.Throws<ArgumentOutOfRangeException>(() => list.CopyTo(a, 0));
        }

        [Test]
        public void AsList_ThenCopyTo_CopiesTheElements()
        {
            var list = "test".AsList();
            var a = new char[4];
            list.CopyTo(a, 0);

            var actual = string.Join(",", a);
            Assert.AreEqual("t,e,s,t", actual);
        }
    }
}
