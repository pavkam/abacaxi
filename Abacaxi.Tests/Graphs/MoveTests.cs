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

namespace Abacaxi.Tests.Graphs
{
    using System;
    using Abacaxi.Graphs;
    using NUnit.Framework;

    [TestFixture]
    public class MoveTests
    {
        [Test]
        public void X_ReturnsValidValue()
        {
            var coords = new Move(99, 0);

            Assert.AreEqual(99, coords.X);
        }

        [Test]
        public void Y_ReturnsValidValue()
        {
            var coords = new Move(0, 99);

            Assert.AreEqual(99, coords.Y);
        }

        [Test]
        public void ToString_ReturnsValidValue()
        {
            var coords = new Move(11, 22);

            Assert.AreEqual($"(11, 22)", coords.ToString());
        }

        [Test]
        public void Equals_ReturnsTrue_ForEqualCoordinates()
        {
            var c1 = new Move(11, 22);
            var c2 = new Move(11, 22);

            Assert.IsTrue(c1.Equals(c2));
        }

        [Test]
        public void Equals_ReturnsFalse_ForNonEqualCoordinates()
        {
            var c1 = new Move(10, 22);
            var c2 = new Move(11, 22);

            Assert.IsFalse(c1.Equals(c2));
        }

        [Test]
        public void Equals_ReturnsFalse_ForNonCoordinateObject()
        {
            var c1 = new Move(10, 22);

            Assert.IsFalse(c1.Equals(this));
        }

        [Test]
        public void Equals_ReturnsFalse_ForNullObject()
        {
            var c1 = new Move(10, 22);

            Assert.IsFalse(c1.Equals(null));
        }


        [Test]
        public void GetHashcode_ReturnsEqualHashcodes_ForEqualCoordinates()
        {
            var c1 = new Move(11, 22);
            var c2 = new Move(11, 22);

            Assert.AreEqual(c1.GetHashCode(), c2.GetHashCode());
        }

        [Test]
        public void GetHashcode_ReturnsDifferentHashcodes_ForDifferentCoordinates()
        {
            var c1 = new Move(10, 22);
            var c2 = new Move(11, 22);

            Assert.AreNotEqual(c1.GetHashCode(), c2.GetHashCode());
        }
    }
}
