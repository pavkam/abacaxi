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

namespace Abacaxi.Tests.Graphs
{
    using System;
    using Abacaxi.Graphs;
    using NUnit.Framework;

    [TestFixture]
    public class ConnectionTests
    {
        [Test]
        public void From_ReturnsValidValue()
        {
            var connection = new Connection<string, int>("from", "to", 99);

            Assert.AreEqual("from", connection.From);
        }

        [Test]
        public void To_ReturnsValidValue()
        {
            var connection = new Connection<string, int>("from", "to", 99);

            Assert.AreEqual("to", connection.To);
        }

        [Test]
        public void Cost_ReturnsValidValue()
        {
            var connection = new Connection<string, int>("from", "to", 99);

            Assert.AreEqual(99, connection.Cost);
        }

        [Test]
        public void ToString_ReturnsValidValue()
        {
            var connection = new Connection<string, int>("from", "to", 99);

            Assert.AreEqual($"from >=99=> to", connection.ToString());
        }

        [Test]
        public void Equals_ReturnsTrue_ForEqualComponents()
        {
            var connection1 = new Connection<string, int>("from", "to", 99);
            var connection2 = new Connection<string, int>("from", "to", 99);

            Assert.IsTrue(connection1.Equals(connection2));
        }

        [Test]
        public void Equals_ReturnsFalse_ForDifferentFrom()
        {
            var connection1 = new Connection<string, int>("from", "to", 99);
            var connection2 = new Connection<string, int>("from1", "to", 99);

            Assert.IsFalse(connection1.Equals(connection2));
        }

        [Test]
        public void Equals_ReturnsFalse_ForDifferentTo()
        {
            var connection1 = new Connection<string, int>("from", "to", 99);
            var connection2 = new Connection<string, int>("from", "to1", 99);

            Assert.IsFalse(connection1.Equals(connection2));
        }

        [Test]
        public void Equals_ReturnsFalse_ForDifferentCost()
        {
            var connection1 = new Connection<string, int>("from", "to", 99);
            var connection2 = new Connection<string, int>("from", "to", 991);

            Assert.IsFalse(connection1.Equals(connection2));
        }


        [Test]
        public void Equals_ReturnsFalse_ForNonConnectionObject()
        {
            var connection = new Connection<string, int>("from", "to", 99);

            Assert.IsFalse(connection.Equals(this));
        }

        [Test]
        public void Equals_ReturnsFalse_ForNullObject()
        {
            var connection = new Connection<string, int>("from", "to", 99);

            Assert.IsFalse(connection.Equals(null));
        }


        [Test]
        public void GetHashcode_ReturnsEqualHashcodes_ForEqualComponents()
        {
            var connection1 = new Connection<string, int>("from", "to", 99);
            var connection2 = new Connection<string, int>("from", "to", 99);

            Assert.AreEqual(connection1.GetHashCode(), connection2.GetHashCode());
        }

        [Test]
        public void GetHashcode_ReturnsDifferenHashcodes_ForDifferentFrom()
        {
            var connection1 = new Connection<string, int>("from", "to", 99);
            var connection2 = new Connection<string, int>("from1", "to", 99);

            Assert.AreNotEqual(connection1.GetHashCode(), connection2.GetHashCode());
        }

        [Test]
        public void GetHashcode_ReturnsDifferenHashcodes_ForDifferentTo()
        {
            var connection1 = new Connection<string, int>("from", "to", 99);
            var connection2 = new Connection<string, int>("from", "to1", 99);

            Assert.AreNotEqual(connection1.GetHashCode(), connection2.GetHashCode());
        }

        [Test]
        public void GetHashcode_ReturnsDifferenHashcodes_ForDifferentCost()
        {
            var connection1 = new Connection<string, int>("from", "to", 99);
            var connection2 = new Connection<string, int>("from", "to", 991);

            Assert.AreNotEqual(connection1.GetHashCode(), connection2.GetHashCode());
        }
    }
}
