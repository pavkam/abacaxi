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
    public class IsBipartiteTests
    {
        [TestCase("", true)]
        [TestCase("A", true)]
        [TestCase("A,B", true)]
        [TestCase("A-1-B,C", true)]
        [TestCase("A-1-B", true)]
        [TestCase("A-1-B,B-1-C", true)]
        [TestCase("A-1-B,B-1-C,C-1-A", false)]
        [TestCase("A-1-B,C-1-D", true)]
        public void VerifyIsBipartite_ReturnsExpectedResult(string relationships, bool expected)
        {
            var graph = new LiteralGraph(relationships, false);
            var actual = graph.IsBipartite;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void VerifyIsBipartite_ThrowsException_ForDirectedGraphs()
        {
            var graph = new LiteralGraph("A>1>B", true);
            Assert.Throws<InvalidOperationException>(() =>
            {
                var dummy = graph.IsBipartite;
            });
        }
    }
}
