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
 * FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN edge 
 * WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */

namespace Abacaxi.Tests.Graph
{
    using Graphs;
    using NUnit.Framework;
    using System.Diagnostics.CodeAnalysis;

    [TestFixture]
    public class EdgeTests
    {
        [Test]
        public void FromVertex_ReturnsValidValue()
        {
            var edge = new Edge<string>("from", "to", 99);

            Assert.AreEqual("from", edge.FromVertex);
        }

        [Test]
        public void ToVertex_ReturnsValidValue()
        {
            var edge = new Edge<string>("from", "to", 99);

            Assert.AreEqual("to", edge.ToVertex);
        }

        [Test]
        public void Weight_ReturnsValidValue()
        {
            var edge = new Edge<string>("from", "to", 99);

            Assert.AreEqual(99, edge.Weight);
        }

        [Test]
        public void Weight_DefaultValue_IsOne()
        {
            var edge = new Edge<string>("from", "to");

            Assert.AreEqual(1, edge.Weight);
        }

        [Test]
        public void ToString_ReturnsValidValue()
        {
            var edge = new Edge<string>("from", "to", 99);

            Assert.AreEqual("from >=99=> to", edge.ToString());
        }

        [Test]
        public void Equals_ReturnsTrue_ForEqualComponents()
        {
            var edge1 = new Edge<string>("from", "to");
            var edge2 = new Edge<string>("from", "to");

            Assert.IsTrue(edge1.Equals(edge2));
        }

        [Test]
        public void Equals_ReturnsFalse_ForDifferentFromVertex()
        {
            var edge1 = new Edge<string>("from", "to");
            var edge2 = new Edge<string>("from1", "to");

            Assert.IsFalse(edge1.Equals(edge2));
        }

        [Test]
        public void Equals_ReturnsFalse_ForDifferentToVertex()
        {
            var edge1 = new Edge<string>("from", "to");
            var edge2 = new Edge<string>("from", "to1");

            Assert.IsFalse(edge1.Equals(edge2));
        }

        [Test]
        public void Equals_ReturnsFalse_ForDifferentWeight()
        {
            var edge1 = new Edge<string>("from", "to", 99);
            var edge2 = new Edge<string>("from", "to", 991);

            Assert.IsFalse(edge1.Equals(edge2));
        }

        [Test]
        [SuppressMessage("ReSharper", "SuspiciousTypeConversion.Global")]
        public void Equals_ReturnsFalse_ForNonEdgeObject()
        {
            var edge = new Edge<string>("from", "to");

            Assert.IsFalse(edge.Equals(this));
        }

        [Test]
        public void Equals_ReturnsFalse_ForNullObject()
        {
            var edge = new Edge<string>("from", "to");

            Assert.IsFalse(edge.Equals(null));
        }

        [Test]
        public void GetHashCode_ReturnsEqualHashCodes_ForEqualComponents()
        {
            var edge1 = new Edge<string>("from", "to");
            var edge2 = new Edge<string>("from", "to");

            Assert.AreEqual(edge1.GetHashCode(), edge2.GetHashCode());
        }

        [Test]
        public void GetHashCode_ReturnsDifferentHashCodes_ForDifferentFromVertex()
        {
            var edge1 = new Edge<string>("from", "to");
            var edge2 = new Edge<string>("from1", "to");

            Assert.AreNotEqual(edge1.GetHashCode(), edge2.GetHashCode());
        }

        [Test]
        public void GetHashCode_ReturnsDifferentHashCodes_ForDifferentToVertex()
        {
            var edge1 = new Edge<string>("from", "to");
            var edge2 = new Edge<string>("from", "to1");

            Assert.AreNotEqual(edge1.GetHashCode(), edge2.GetHashCode());
        }

        [Test]
        public void GetHashCode_ReturnsDifferentHashCodes_ForDifferentWeight()
        {
            var edge1 = new Edge<string>("from", "to", 99);
            var edge2 = new Edge<string>("from", "to", 991);

            Assert.AreNotEqual(edge1.GetHashCode(), edge2.GetHashCode());
        }
    }
}
