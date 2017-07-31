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
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NUnit.Framework;

    [TestFixture]
    public sealed class AsListTests
    {
        [Test]
        public void AsList_ThrowsException_IfSequenceIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => ((int[]) null).AsList());
        }

        [Test]
        public void AsList_ReturnsSameObject_IfItIsAList()
        {
            var list = new List<int>() {1};
            var asList = list.AsList();

            Assert.AreSame(list, asList);
        }

        [Test]
        public void AsList_ReturnsSameObject_IfItIsAnArray()
        {
            var list = new[] {1};
            var asList = list.AsList();

            Assert.AreSame(list, asList);
        }

        [Test]
        public void AsList_ReturnsANewList_FromACollection()
        {
            var coll = new[] {1, 2, 3}.ToSet();
            var asList = coll.AsList();

            TestHelper.AssertSequence(asList, 1, 2, 3);
        }

        [Test]
        public void AsList_ReturnsEmptyList_ForEmptyCollection()
        {
            // ReSharper disable once CollectionNeverUpdated.Local
            var coll = new HashSet<int>();
            var asList = coll.AsList();

            TestHelper.AssertSequence(asList);
        }

        [Test]
        public void AsList_ReturnsANewList_ForAString()
        {
            const string s = "123";
            var asList = ((IEnumerable<char>)s).AsList();

            TestHelper.AssertSequence(asList, '1', '2', '3');
        }

        [Test]
        public void AsList_ReturnsANewList_ForAnEnumerable()
        {
            var e = Enumerable.Range(1, 3);
            var asList = e.AsList();

            TestHelper.AssertSequence(asList, 1, 2, 3);
        }
        
    }
}