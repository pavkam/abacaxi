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

namespace Abacaxi.Tests.Sequences
{
    using System;
    using Abacaxi.Sequences;
    using NUnit.Framework;

    [TestFixture]
    public class EnumerableRepeatTests
    {
        [Test]
        public void Repeat_ThrowsException_ForNullString()
        {
            Assert.Throws<ArgumentNullException>(() =>
                EnumerableRepeat.Repeat((int[])null, 1));
        }
        
        [Test]
        public void Repeat_ThrowsException_ForZeroRepetitions()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                EnumerableRepeat.Repeat("A", 0));
        }

        [Test]
        public void Repeat_ReturnsNothing_ForEmptyEnumerable()
        {
            var result = EnumerableRepeat.Repeat("", 5);
            Assert.AreEqual("", result);
        }

        [Test]
        public void Repeat_DoesNothing_ForOneRepetition()
        {
            var result = EnumerableRepeat.Repeat("A", 1);
            Assert.AreEqual("A", result);
        }

        [Test]
        public void Repeat_DoublesString_ForTwoRepetition()
        {
            var result = EnumerableRepeat.Repeat("A", 2);
            Assert.AreEqual("AA", result);
        }

        public void Repeat_TriplesString_ForThreeRepetition()
        {
            var result = EnumerableRepeat.Repeat("A", 3);
            Assert.AreEqual("AAA", result);
        }
    }
}
