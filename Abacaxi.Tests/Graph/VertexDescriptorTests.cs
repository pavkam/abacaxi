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

 namespace Abacaxi.Tests.Graph
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Graphs;
    using NUnit.Framework;

    [TestFixture]
    public class VertexDescriptorTests
    {
        private readonly VertexDescriptor<string> _descriptor = new VertexDescriptor<string>("VERTEX", 10, 1, 2);

        [Test]
        [SuppressMessage("ReSharper", "ObjectCreationAsStatement")]
        public void VertexDescriptor_ctor_ThrowsException_ForNegativeComponentIndex()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new VertexDescriptor<string>("V", -1, 0, 0));
        }

        [Test]
        [SuppressMessage("ReSharper", "ObjectCreationAsStatement")]
        public void VertexDescriptor_ctor_ThrowsException_ForNegativeInDegree()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new VertexDescriptor<string>("V", 0, -1, 0));
        }

        [Test]
        [SuppressMessage("ReSharper", "ObjectCreationAsStatement")]
        public void VertexDescriptor_ctor_ThrowsException_ForNegativeOutDegree()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new VertexDescriptor<string>("V", 0, 0, -1));
        }

        [Test]
        public void VertexDescriptor_Vertex_ReturnsValidValue()
        {
            Assert.AreEqual("VERTEX", _descriptor.Vertex);
        }

        [Test]
        public void VertexDescriptor_InDegree_ReturnsValidValue()
        {
            Assert.AreEqual(1, _descriptor.InDegree);
        }

        [Test]
        public void VertexDescriptor_OutDegree_ReturnsValidValue()
        {
            Assert.AreEqual(2, _descriptor.OutDegree);
        }

        [Test]
        public void VertexDescriptor_ComponentIndex_ReturnsValidValue()
        {
            Assert.AreEqual(10, _descriptor.ComponentIndex);
        }

        [Test]
        public void VertexDescriptor_ToString_ReturnsValidValue()
        {
            Assert.AreEqual("1 => VERTEX (10) => 2", _descriptor.ToString());
        }
    }
}
