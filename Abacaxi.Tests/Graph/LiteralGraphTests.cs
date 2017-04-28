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
    using System.Collections.Generic;
    using System.Linq;
    using Graphs;
    using NUnit.Framework;

    [TestFixture]
    public class LiteralGraphTests
    {
        [Test]
        public void Ctor_ThrowsException_ForNullRelationships()
        {
            Assert.Throws<ArgumentNullException>(() => new LiteralGraph(null));
        }

        [TestCase("")]
        [TestCase(".")]
        [TestCase("A")]
        [TestCase("A.")]
        [TestCase("AA-B")]
        [TestCase("A-")]
        [TestCase("A-BA")]
        [TestCase("A-B,A")]
        public void Ctor_ThrowsException_ForInvalidFormat(string relationships)
        {
            Assert.Throws<FormatException>(() => new LiteralGraph(relationships));
        }

        [TestCase("A>B,A>B")]
        [TestCase("A-B,A>B")]
        [TestCase("A<B,A-B")]
        public void Ctor_ThrowsException_ForInvalidRelationships(string relationships)
        {
            Assert.Throws<InvalidOperationException>(() => new LiteralGraph(relationships));
        }

        [Test]
        public void Ctor_IgnoresLastComma_InRelationships()
        {
            Assert.DoesNotThrow(() => new LiteralGraph("A-B,"));
        }

        [Test]
        public void Ctor_IgnoresWhitespaces_InRelationships()
        {
            Assert.DoesNotThrow(() => new LiteralGraph("  A -     B   ,      "));
        }

        [TestCase("0-1")]
        [TestCase("0>1,0<1")]
        [TestCase("0>1,0>2,1>0,2>0")]
        public void IsDirected_IsTrue_IfTheRelationshipsAreTrulyUndirected(string relationships)
        {
            var graph = new LiteralGraph(relationships);

            Assert.IsTrue(graph.IsDirected);
        }

        [TestCase("0>1")]
        [TestCase("0<1,0<2")]
        [TestCase("0>1,1>2,2>0")]
        public void IsDirected_IsFalse_IfTheRelationshipsAreNotUndirected(string relationships)
        {
            var graph = new LiteralGraph(relationships);

            Assert.IsFalse(graph.IsDirected);
        }

        [Test]
        public void Enumeration_ReturnsAllVertices()
        {
            var graph = new LiteralGraph("A>B,B-Z,K<T");

            var v = graph.ToArray();
            v.QuickSort(0, v.Length, Comparer<char>.Default);

            TestHelper.AssertSequence(v,
                'A','B','K','T','Z');
        }

        [TestCase('A', "A>B")]
        [TestCase('B', "B>Z,B>T,B>A")]
        [TestCase('K', "")]
        [TestCase('T', "T>K,T>A")]
        [TestCase('Z', "Z>B")]
        public void GetEdges_ReturnsAllEdges(char vertex, string expected)
        {
            var graph = new LiteralGraph("A>B,B-Z,K<T,B>T,B>A,T>A");

            var v = graph.GetEdges(vertex).Select(s => s.FromVertex + ">" + s.ToVertex).ToArray();
            var result = string.Join(",", v);

            Assert.AreEqual(expected, result);
        }
    }
}
