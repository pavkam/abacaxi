/* Copyright 2017-2018 by Alexandru Ciobanu (alex+git@ciobanu.org)
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
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using JetBrains.Annotations;
    using NUnit.Framework;
    using Practice.Graphs;

    [TestFixture]
    public class StringNeighborhoodGraphTests
    {
        [TestCase("a", "b,c"), TestCase("b", "a,c"), TestCase("c", "a,b")]
        public void GetEdges_ProperlyJoinsSingleLetterStrings([NotNull] string vertex, string expected)
        {
            var graph = new StringNeighborhoodGraph(new[] {"a", "b", "c"});
            var actual = string.Join(",", graph.GetEdges(vertex).Select(s => s.ToVertex));

            Assert.AreEqual(expected, actual);
        }

        [Test, SuppressMessage("ReSharper", "ObjectCreationAsStatement"),
         SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void Ctor_ThrowsException_ForNullSequence()
        {
            Assert.Throws<ArgumentNullException>(() => new StringNeighborhoodGraph(null));
        }

        [Test]
        public void Enumeration_ReturnsAllVertices()
        {
            var graph = new StringNeighborhoodGraph(new[] {"a", "b", "c"});

            TestHelper.AssertSequence(graph, "a", "b", "c");
        }

        [Test]
        public void Enumeration_ReturnsNothing_ForEmptyGraph()
        {
            var graph = new StringNeighborhoodGraph(new string[] { });

            TestHelper.AssertSequence(graph);
        }

        [Test]
        public void Enumeration_ReturnsSingleVertex()
        {
            var graph = new StringNeighborhoodGraph(new[] {"test"});

            TestHelper.AssertSequence(graph, "test");
        }

        [Test]
        public void FindShortestPath_WillTraverseTheGraphInExpectedOrder()
        {
            var graph = new StringNeighborhoodGraph(new[] {"aa", "za", "zz", "zb", "ib", "i7", "17", "a6", "16"});

            TestHelper.AssertSequence(graph.FindShortestPath("aa", "17"),
                "aa", "a6", "16", "17");
        }

        [Test]
        public void GetEdges_CorrectlyReports_FromVertex()
        {
            var graph = new StringNeighborhoodGraph(new[] {"a", "b"});

            Assert.IsTrue(graph.GetEdges("a").All(e => e.FromVertex == "a"));
        }

        [Test]
        public void GetEdges_CorrectlyReports_Weight()
        {
            var graph = new StringNeighborhoodGraph(new[] {"a", "b"});

            // ReSharper disable once CompareOfFloatsByEqualityOperator
            Assert.IsTrue(graph.GetEdges("a").All(e => e.Weight == 1));
        }

        [Test, SuppressMessage("ReSharper", "IteratorMethodResultIsIgnored")]
        public void GetEdges_ThrowsException_ForInvalidVertex()
        {
            var graph = new StringNeighborhoodGraph(new[] {"a", "b", "c"});

            Assert.Throws<ArgumentException>(() => graph.GetEdges("z"));
        }

        [Test, SuppressMessage("ReSharper", "IteratorMethodResultIsIgnored"),
         SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void GetEdges_ThrowsException_ForNullVertex()
        {
            var graph = new StringNeighborhoodGraph(new[] {"a", "b", "c"});

            Assert.Throws<ArgumentNullException>(() => graph.GetEdges(null));
        }

        [Test]
        public void GetPotentialWeight_ThrowsException_Always()
        {
            Assert.Throws<NotSupportedException>(
                () => new StringNeighborhoodGraph(new[] {"a", "b", "c"}).GetPotentialWeight("a", "b"));
        }

        [Test]
        public void GetPotentialWeight_ThrowsException_IfFromVertexNotPartOfGraph()
        {
            Assert.Throws<ArgumentException>(
                () => new StringNeighborhoodGraph(new[] {"a", "b", "c"}).GetPotentialWeight("z", "a"));
        }

        [Test]
        public void GetPotentialWeight_ThrowsException_IfToVertexNotPartOfGraph()
        {
            Assert.Throws<ArgumentException>(
                () => new StringNeighborhoodGraph(new[] {"a", "b", "c"}).GetPotentialWeight("a", "z"));
        }

        [Test]
        public void SupportsPotentialWeightEvaluation_ReturnsFalse()
        {
            var graph = new StringNeighborhoodGraph(new[] {"a", "b", "c"});
            Assert.IsFalse(graph.SupportsPotentialWeightEvaluation);
        }
    }
}