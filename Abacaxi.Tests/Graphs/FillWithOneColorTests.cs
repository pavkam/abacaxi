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
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Abacaxi.Graphs;
    using JetBrains.Annotations;
    using NUnit.Framework;

    [TestFixture]
    public sealed class FillWithOneColorTests
    {
        [TestCase("A>1>B,A>1>C,C<1<F,F-1-E,E-1-D,D>1>B,D>1>C", 'A', "A,B,C"),
         TestCase("A>1>B,A>1>C,C<1<F,F-1-E,E-1-D,D>1>B,D>1>C", 'C', "C"),
         TestCase("A>1>B,A>1>C,C<1<F,F-1-E,E-1-D,D>1>B,D>1>C", 'D', "D,E,B,C,F")]
        public void FillWithOneColor_FillsExpectedVertices([NotNull] string relationships, char startVertex,
            string expected)
        {
            var graph = new LiteralGraph(relationships, true);
            var result = new List<char>();

            graph.FillWithOneColor(startVertex, vertex => result.Add(vertex));

            Assert.AreEqual(expected, string.Join(",", result));
        }

        [Test]
        public void FillWithOneColor_ThrowsException_ForInvalidVertex()
        {
            var graph = new LiteralGraph("A>1>B", true);
            Assert.Throws<ArgumentException>(() => graph.FillWithOneColor('Z', v => { }));
        }

        [Test, SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void FillWithOneColor_ThrowsException_ForNullApplyColor()
        {
            var graph = new LiteralGraph("A>1>B", true);
            Assert.Throws<ArgumentNullException>(() => graph.FillWithOneColor('A', null));
        }
    }
}