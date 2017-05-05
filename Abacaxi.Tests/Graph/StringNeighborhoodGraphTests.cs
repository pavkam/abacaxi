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

// ReSharper disable ReturnValueOfPureMethodIsNotUsed
// ReSharper disable ObjectCreationAsStatement

namespace Abacaxi.Tests.Graph
{
    using System;
    using System.Linq;
    using NUnit.Framework;
    using Practice.Graphs;

    [TestFixture]
    public class StringNeighborhoodGraphTests
    {
        [Test]
        public void Ctor_ThrowsException_ForNullSequence()
        {
            Assert.Throws<ArgumentNullException>(() => new StringNeighborhoodGraph(null));
        }

        [Test]
        public void Enumeration_ReturnsNothing_ForEmptyGraph()
        {
            var graph = new StringNeighborhoodGraph(new string[] {});

            TestHelper.AssertSequence(graph);
        }

        [Test]
        public void Enumeration_ReturnsSingleVertex()
        {
            var graph = new StringNeighborhoodGraph(new[] { "test" });

            TestHelper.AssertSequence(graph, "test");
        }

        [Test]
        public void Enumeration_ReturnsAllVertices()
        {
            var graph = new StringNeighborhoodGraph(new [] { "a", "b", "c" });

            TestHelper.AssertSequence(graph, "a", "b", "c");
        }

        [Test]
        public void GetEdges_ThrowsException_ForNullVertex()
        {
            var graph = new StringNeighborhoodGraph(new[] { "a", "b", "c" });

            Assert.Throws<ArgumentException>(() => graph.GetEdges(null).ToList());
        }

        [Test]
        public void GetEdges_ThrowsException_ForInvalidVertex()
        {
            var graph = new StringNeighborhoodGraph(new[] { "a", "b", "c" });

            Assert.Throws<ArgumentException>(() => graph.GetEdges("z").ToList());
        }

        [Test]
        public void GetEdges_CorrectlyReports_FromVertex()
        {
            var graph = new StringNeighborhoodGraph(new[] { "a", "b" });

            Assert.AreEqual("a", graph.GetEdges("a").Single().FromVertex);
        }

        [TestCase("a", "b,c")]
        [TestCase("b", "a,c")]
        [TestCase("c", "a,b")]
        public void GetEdges_ProperlyJoinsSingleLetterStrings(string vertex, string expected)
        {
            var graph = new StringNeighborhoodGraph(new[] { "a", "b", "c" });
            var actual = string.Join(",", graph.GetEdges(vertex).Select(s => s.ToVertex));

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void FindShortestPath_WillTraverseTheGraphInExpectedOrder()
        {
            var graph = new StringNeighborhoodGraph(new[] { "aa", "za", "zz", "zb", "ib", "i7", "17", "a6", "16" });
            
            TestHelper.AssertSequence(graph.FindShortestPath("aa", "17"),
                "aa", "a6", "16", "17");
        }
    }
}
