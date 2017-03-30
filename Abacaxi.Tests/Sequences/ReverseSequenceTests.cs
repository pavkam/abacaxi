﻿/* Copyright 2017 by Alexandru Ciobanu (alex+git@ciobanu.org)
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
    public class ReverseSequenceTests
    {
        [Test]
        public void Reverse_ThrowsException_ForNullArray()
        {
            Assert.Throws<ArgumentNullException>(() =>
                ReverseSequence.Reverse((int[])null, 1, 1));
        }
        
        [Test]
        public void Reverse_ThrowsException_ForNegativeStartIndex()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                ReverseSequence.Reverse(new[] { 1 }, -1, 1));
        }

        [Test]
        public void Reverse_ThrowsException_ForNegativeLength()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                ReverseSequence.Reverse(new[] { 1 }, 0, -1));
        }

        [Test]
        public void Reverse_ThrowsException_ForOutOfBounds1()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                ReverseSequence.Reverse(new[] { 1 }, 0, 2));
        }

        [Test]
        public void Reverse_ThrowsException_ForOutOfBounds2()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                ReverseSequence.Reverse(new[] { 1 }, 1, 1));
        }

        [Test]
        public void Reverse_DoesNothing_ForZeroLength()
        {
            var array = new int[] { 1, 2 };
            ReverseSequence.Reverse(array, 1, 0);

            Assert.AreEqual(new[] { 1, 2 }, array);
        }

        [Test]
        public void Reverse_DoesNothing_ForEmptyArray()
        {
            var array = new int[] { };
            ReverseSequence.Reverse(array, 0, 0);

            Assert.AreEqual(new int[] { }, array);
        }

        [Test]
        public void Reverse_Reverses_AnEvenLengthArray()
        {
            var array = new int[] { 1, 2, 3, 4 };
            ReverseSequence.Reverse(array, 0, 4);

            Assert.AreEqual(new[] { 4, 3, 2, 1 }, array);
        }

        [Test]
        public void Reverse_Reverses_AnOddLengthArray()
        {
            var array = new int[] { 1, 2, 3, 4, 5 };
            ReverseSequence.Reverse(array, 0, 5);

            Assert.AreEqual(new[] { 5, 4, 3, 2, 1 }, array);
        }

        [Test]
        public void Reverse_Reverses_ASegmentOfTheArray()
        {
            var array = new int[] { 1, 2, 3, 4, 5 };
            ReverseSequence.Reverse(array, 0, 2);

            Assert.AreEqual(new[] { 2, 1, 3, 4, 5 }, array);
        }
    }
}
