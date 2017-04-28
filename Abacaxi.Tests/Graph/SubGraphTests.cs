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

// ReSharper disable ObjectCreationAsStatement

namespace Abacaxi.Tests.Graph
{
    using System;
    using Graphs;
    using NUnit.Framework;

    [TestFixture]
    public class SubGraphTests
    {
        [Test]
        public void Ctor_ThrowsException_ForNullGraph()
        {
            Assert.Throws<ArgumentNullException>(() => new SubGraph<int>(null, new[] { 1 }));
        }

        [Test]
        public void Ctor_ThrowsException_ForNullVerticesSequence()
        {
            Assert.Throws<ArgumentNullException>(() => new SubGraph<char>(new LiteralGraph("A-B"), null));
        }

        [Test]
        public void Ctor_ThrowsException_ForVerticesNotPartOfTheGraph()
        {
            Assert.Throws<InvalidOperationException>(() => new SubGraph<char>(new LiteralGraph("A-B"), new[] { 'Z' }));
        }

        [TestCase("A", 'A', "")]
        [TestCase("AB", 'A', "A >==> B")]
        [TestCase("ABC", 'B', "B >==> A, B >==> C")]
        public void GetEdges_ReturnsOnlySuppliedVertices(string vertices, char vertex, string expected)
        {
            var graph = new LiteralGraph("A-B,B-C,C-D,D-A,D<B");
            var sub = new SubGraph<char>(graph, vertices);
            var actual = string.Join(", ", sub.GetEdges(vertex));

            Assert.AreEqual(expected, actual);
        }

        [TestCase("A", "A")]
        [TestCase("AB", "A,B")]
        [TestCase("ABC", "A,B,C")]
        public void Enumeration_ReturnsOnlyIncludedVertices(string vertices, string expected)
        {
            var graph = new LiteralGraph("A-B,B-C,C-D,D-A,D<B");
            var sub = new SubGraph<char>(graph, vertices);
            var actual = string.Join(",", sub);

            Assert.AreEqual(expected, actual);
        }
    }
}
