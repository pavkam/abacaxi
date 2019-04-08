/* Copyright 2017-2019 by Alexandru Ciobanu (alex+git@ciobanu.org)
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
    using System.Linq;
    using Abacaxi.Graphs;
    using JetBrains.Annotations;
    using NUnit.Framework;

    [TestFixture]
    public sealed class GetComponentsTests
    {
        [TestCase("", ""), TestCase("A", "A"), TestCase("A-1-A", "A"), TestCase("A-1-A,A-1-A", "A"),
         TestCase("A-1-B,B-1-C,C-1-D", "A,B,C,D"), TestCase("A,B,C", "A;B;C"),
         TestCase("A-1-B,B-1-C,C-1-A,D-1-E", "A,B,C;D,E")]
        public void GetComponents_ReturnsProperComponents_ForUndirectedGraphs([NotNull] string relationships,
            string expected)
        {
            var graph = new LiteralGraph(relationships, false);
            var result = string.Join(";",
                graph.GetComponents().Select(component => string.Join(",", component)));

            Assert.AreEqual(expected, result);
        }

        [TestCase("", ""), TestCase("A", "A"), TestCase("A>1>A", "A"), TestCase("A>1>A,A>1>A", "A"),
         TestCase("A>1>B,B-1-C,C>1>D", "A,B,C,D"), TestCase("A,B,C", "A;B;C"),
         TestCase("A-1-B,B-1-C,C-1-A,D-1-E", "A,B,C;D,E"), TestCase("A>1>B,C>1>D", "A,B;C,D")]
        public void GetComponents_ReturnsProperComponents_ForDirectedGraphs([NotNull] string relationships,
            string expected)
        {
            var graph = new LiteralGraph(relationships, true);
            var result = string.Join(";",
                graph.GetComponents().Select(component => string.Join(",", component)));

            Assert.AreEqual(expected, result);
        }
    }
}