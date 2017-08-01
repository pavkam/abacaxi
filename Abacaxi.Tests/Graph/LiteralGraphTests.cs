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
    using System.Collections.Generic;
    using System.Linq;
    using Graphs;
    using NUnit.Framework;
    using System.Diagnostics.CodeAnalysis;

    [TestFixture]
    public class LiteralGraphTests
    {
        [Test]
        [SuppressMessage("ReSharper", "ObjectCreationAsStatement")]
        public void LiteralGraph_Ctor_ThrowsException_ForNullRelationships()
        {
            Assert.Throws<ArgumentNullException>(() => new LiteralGraph(null, true));
        }

        [TestCase(".")]
        [TestCase("A.")]
        [TestCase("AA-B")]
        [TestCase("A-")]
        [TestCase("A-BA")]
        [SuppressMessage("ReSharper", "ObjectCreationAsStatement")]
        public void LiteralGraph_Ctor_ThrowsException_ForInvalidFormat(string relationships)
        {
            Assert.Throws<FormatException>(() => new LiteralGraph(relationships, false));
        }

        [TestCase("A>B")]
        [TestCase("A<B")]
        [SuppressMessage("ReSharper", "ObjectCreationAsStatement")]
        public void LiteralGraph_Ctor_ThrowsException_ForInvalidEdgesInUndirectedGraph(string relationships)
        {
            Assert.Throws<FormatException>(() => new LiteralGraph(relationships, false));
        }

        [Test]
        [SuppressMessage("ReSharper", "ObjectCreationAsStatement")]
        public void LiteralGraph_Ctor_IgnoresLastComma_InRelationships()
        {
            Assert.DoesNotThrow(() => new LiteralGraph("A-B,", true));
        }

        [Test]
        [SuppressMessage("ReSharper", "ObjectCreationAsStatement")]
        public void LiteralGraph_Ctor_IgnoresWhitespaces_InRelationships()
        {
            Assert.DoesNotThrow(() => new LiteralGraph("  A -     B   ,      ", true));
        }

        [Test]
        [SuppressMessage("ReSharper", "ObjectCreationAsStatement")]
        public void LiteralGraph_Ctor_AcceptsASingleUnconnectedVertex()
        {
            Assert.DoesNotThrow(() => new LiteralGraph("A", true));
        }

        [Test]
        public void LiteralGraph_Ctor_AcceptsUnconnectedVertices()
        {
            var graph = new LiteralGraph("A,B,C-D", true);

            TestHelper.AssertSequence(graph.GetEdges('A'));
            TestHelper.AssertSequence(graph.GetEdges('B'));
        }

        [Test]
        public void LiteralGraph_Ctor_AcceptsEmptyRelationships()
        {
            var graph = new LiteralGraph("", false);
            TestHelper.AssertSequence(graph);
        }

        [Test]
        [SuppressMessage("ReSharper", "ObjectCreationAsStatement")]
        public void LiteralGraph_Ctor_AcceptsLettersAndDigits()
        {
            Assert.DoesNotThrow(() => new LiteralGraph("a-B,B-0", true));
        }

        [Test]
        public void LiteralGraph_IsDirected_IsTrue_IfTrueSpecifiedAtConstruction()
        {
            var graph = new LiteralGraph("A", true);
            Assert.IsTrue(graph.IsDirected);
        }

        [Test]
        public void LiteralGraph_IsDirected_IsFalse_IfFalseSpecifiedAtConstruction()
        {
            var graph = new LiteralGraph("A", false);
            Assert.IsFalse(graph.IsDirected);
        }

        [Test]
        public void LiteralGraph_IsReadOnly_ReturnsTrue()
        {
            var graph = new LiteralGraph("A,B", false);

            Assert.IsTrue(graph.IsReadOnly);
        }

        [Test]
        public void LiteralGraph_Enumeration_ReturnsAllVertices()
        {
            var graph = new LiteralGraph("A>B,B-Z,K<T", true);

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
        public void LiteralGraph_GetEdges_ReturnsAllEdges(char vertex, string expected)
        {
            var graph = new LiteralGraph("A>B,B-Z,K<T,B>T,B>A,T>A", true);

            var v = graph.GetEdges(vertex).Select(s => s.FromVertex + ">" + s.ToVertex).ToArray();
            var result = string.Join(",", v);

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void LiteralGraph_Preserves_MultipleEdges_ForUndirectedGraphs()
        {
            var graph = new LiteralGraph("A-A,A-B,A-B", true);
            var edgesFromA = string.Join(",", graph.GetEdges('A').Select(s => s.FromVertex + ">" + s.ToVertex));
            var edgesFromB = string.Join(",", graph.GetEdges('B').Select(s => s.FromVertex + ">" + s.ToVertex));

            Assert.AreEqual("A>A,A>B,A>B", edgesFromA);
            Assert.AreEqual("B>A,B>A", edgesFromB);
        }

        [Test]
        public void LiteralGraph_Preserves_MultipleEdges_ForDirectedGraphs()
        {
            var graph = new LiteralGraph("A>A,A>B,A>B", true);
            var edgesFromA = string.Join(",", graph.GetEdges('A').Select(s => s.FromVertex + ">" + s.ToVertex));

            Assert.AreEqual("A>A,A>B,A>B", edgesFromA);
        }
    }
}
