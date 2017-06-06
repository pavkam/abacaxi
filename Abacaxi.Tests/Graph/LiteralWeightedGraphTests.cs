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
    public class LiteralWeightedGraphTests
    {
        [Test]
        public void Ctor_ThrowsException_ForNullRelationships()
        {
            Assert.Throws<ArgumentNullException>(() => new LiteralWeightedGraph(null, true));
        }

        [TestCase(".")]
        [TestCase("A.")]
        [TestCase("AA-B")]
        [TestCase("A-")]
        [TestCase("A-5 5-B")]
        [TestCase("A-BA")]
        [TestCase("A<BA")]
        [TestCase("A-<B")]
        [TestCase("A-B")]
        [TestCase("A>B")]
        [TestCase("A<B")]
        [TestCase("A-1B")]
        [TestCase("A-1>B")]
        [TestCase("A-1<B")]
        [TestCase("A>1-B")]
        [TestCase("A>1<B")]
        [TestCase("A<1-B")]
        [TestCase("A<1>B")]
        public void Ctor_ThrowsException_ForInvalidFormat(string relationships)
        {
            Assert.Throws<FormatException>(() => new LiteralWeightedGraph(relationships, true));
        }

        [TestCase("A>1>B")]
        [TestCase("A<1<B")]
        public void Ctor_ThrowsException_ForInvalidEdgesInUndirectedGraph(string relationships)
        {
            Assert.Throws<FormatException>(() => new LiteralWeightedGraph(relationships, false));
        }

        [Test]
        public void Ctor_IgnoresLastComma_InRelationships()
        {
            Assert.DoesNotThrow(() => new LiteralWeightedGraph("A-1-B,", true));
        }

        [Test]
        public void Ctor_IgnoresWhitespaces_InRelationships()
        {
            Assert.DoesNotThrow(() => new LiteralWeightedGraph("  A -   4-  B   ,      ", true));
        }

        [Test]
        public void Ctor_AcceptsMultiDigitWeights()
        {
            var graph = new LiteralWeightedGraph("A-1234-B", false);
            Assert.AreEqual(1234, graph.GetEdgesAndWeights('A').Single().Weight);
        }

        [Test]
        public void Ctor_AcceptsASingleUnconnectedVertex()
        {
            Assert.DoesNotThrow(() => new LiteralWeightedGraph("A", true));
        }

        [Test]
        public void Ctor_AcceptsASingleUnconnectedVertex_WithFollowingComma()
        {
            Assert.DoesNotThrow(() => new LiteralWeightedGraph("A,", true));
        }

        [Test]
        public void Ctor_AcceptsUnconnectedVertices()
        {
            var graph = new LiteralWeightedGraph("A,B,C-6-D", true);

            TestHelper.AssertSequence(graph.GetEdges('A'));
            TestHelper.AssertSequence(graph.GetEdges('B'));
        }

        [Test]
        public void Ctor_AcceptsEmptyRelationships()
        {
            var graph = new LiteralWeightedGraph("", false);
            TestHelper.AssertSequence(graph);
        }

        [Test]
        public void Ctor_AcceptsLettersAndDigits()
        {
            Assert.DoesNotThrow(() => new LiteralWeightedGraph("a-1-B,B-1-0", true));
        }

        [Test]
        public void IsDirected_IsTrue_IfTrueSpecifiedAtConstruction()
        {
            var graph = new LiteralWeightedGraph("A", true);
            Assert.IsTrue(graph.IsDirected);
        }

        [Test]
        public void IsDirected_IsFalse_IfFalseSpecifiedAtConstruction()
        {
            var graph = new LiteralWeightedGraph("A", false);
            Assert.IsFalse(graph.IsDirected);
        }

        [Test]
        public void IsReadOnly_ReturnsTrue()
        {
            var graph = new LiteralWeightedGraph("A,B", false);

            Assert.IsTrue(graph.IsReadOnly);
        }

        [Test]
        public void Enumeration_ReturnsAllVertices()
        {
            var graph = new LiteralWeightedGraph("A>1>B,B-1-Z,K<1<T", true);

            var v = graph.ToArray();
            v.QuickSort(0, v.Length, Comparer<char>.Default);

            TestHelper.AssertSequence(v,
                'A','B','K','T','Z');
        }

        [TestCase("A>1>B,B-2-Z,K<3<T,B>4>T,B>5>A,T>6>A", 'A', "A1B")]
        [TestCase("A>1>B,B-2-Z,K<3<T,B>4>T,B>5>A,T>6>A", 'B', "B2Z,B4T,B5A")]
        [TestCase("A>1>B,B-2-Z,K<3<T,B>4>T,B>5>A,T>6>A", 'K', "")]
        [TestCase("A>1>B,B-2-Z,K<3<T,B>4>T,B>5>A,T>6>A", 'T', "T3K,T6A")]
        [TestCase("A>1>B,B-2-Z,K<3<T,B>4>T,B>5>A,T>6>A", 'Z', "Z2B")]
        public void GetEdgesAndWeights_ReturnsAllEdges(string relationships, char vertex, string expected)
        {
            var graph = new LiteralWeightedGraph(relationships, true);

            var v = graph.GetEdgesAndWeights(vertex).Select(s => $"{s.FromVertex}{s.Weight}{s.ToVertex}").ToArray();
            var result = string.Join(",", v);

            Assert.AreEqual(expected, result);
        }

        [TestCase("A>1>B,B-2-Z,K<3<T,B>4>T,B>5>A,T>6>A", 'A', "AB")]
        [TestCase("A>1>B,B-2-Z,K<3<T,B>4>T,B>5>A,T>6>A", 'B', "BZ,BT,BA")]
        [TestCase("A>1>B,B-2-Z,K<3<T,B>4>T,B>5>A,T>6>A", 'K', "")]
        [TestCase("A>1>B,B-2-Z,K<3<T,B>4>T,B>5>A,T>6>A", 'T', "TK,TA")]
        [TestCase("A>1>B,B-2-Z,K<3<T,B>4>T,B>5>A,T>6>A", 'Z', "ZB")]
        public void GetEdges_ReturnsAllEdges(string relationships, char vertex, string expected)
        {
            var graph = new LiteralWeightedGraph(relationships, true);

            var v = graph.GetEdges(vertex).Select(s => $"{s.FromVertex}{s.ToVertex}").ToArray();
            var result = string.Join(",", v);

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void SupportsPotentialWeightEvaluation_ReturnsFalse()
        {
            var graph = new LiteralWeightedGraph("", true);

            Assert.IsFalse(graph.SupportsPotentialWeightEvaluation);
        }

        [Test]
        public void GetPotentialWeight_ThrowsException_IfFromVertexNotPartOfGraph()
        {
            Assert.Throws<ArgumentException>(() => new LiteralWeightedGraph("A,B", true).GetPotentialWeight('Z', 'A'));
        }

        [Test]
        public void GetPotentialWeight_ThrowsException_IfToVertexNotPartOfGraph()
        {
            Assert.Throws<ArgumentException>(() => new LiteralWeightedGraph("A,B", true).GetPotentialWeight('A', 'Z'));
        }

        [Test]
        public void GetPotentialWeight_ThrowsException_Always()
        {
            Assert.Throws<NotSupportedException>(() => new LiteralWeightedGraph("A,B", true).GetPotentialWeight('A', 'B'));
        }
    }
}
