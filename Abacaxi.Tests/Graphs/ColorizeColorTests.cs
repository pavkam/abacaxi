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
    public sealed class ColorizeColorTests
    {
        [NotNull]
        private static string Colorize([NotNull] string relationships)
        {
            var graph = new LiteralGraph(relationships, false);

            var result = new List<string>();
            graph.Colorize((v, c) => result.Add($"{v}({c})"));

            return string.Join(", ", result);
        }

        [Test]
        public void Colorize_ColorsInFourColors_ForConnectedCenter_3()
        {
            Assert.AreEqual("A(0), C(1), D(2), B(1)", Colorize("A-0-B,A-0-C,A-0-D,D-0-C"));
        }

        [Test]
        public void Colorize_ColorsInOneColor_ForOneNode()
        {
            Assert.AreEqual("A(0)", Colorize("A"));
        }

        [Test]
        public void Colorize_ColorsInOneColorFor_TwoUnconnectedNodes()
        {
            Assert.AreEqual("A(0), B(0)", Colorize("A,B"));
        }

        [Test]
        public void Colorize_ColorsInThreeColors_ForConnectedCenter_1()
        {
            Assert.AreEqual("A(0), B(1), C(1)", Colorize("A-0-B,A-0-C"));
        }

        [Test]
        public void Colorize_ColorsInTwoColors_ForTwoNodes()
        {
            Assert.AreEqual("A(0), B(1)", Colorize("A-0-B"));
        }

        [Test]
        public void Colorize_ColorsInTwoColors_ForTwoThreeChainedNodes()
        {
            Assert.AreEqual("B(0), A(1), C(1)", Colorize("A-0-B,B-0-C"));
        }

        [Test]
        public void Colorize_DoesNothing_ForEmptyGraph()
        {
            Assert.AreEqual("", Colorize(""));
        }

        [Test]
        public void Colorize_SelectsTheBestOption_1()
        {
            Assert.AreEqual("B(0), C(1), D(2), A(2), E(0)", Colorize("A-0-B,A-0-C,B-0-C,B-0-D,C-0-D,D-0-E"));
        }

        [Test]
        public void Colorize_ThrowsException_ForDirectedGraph()
        {
            var graph = new LiteralGraph("A>1>B", true);

            Assert.Throws<InvalidOperationException>(() => graph.Colorize((c, i) => { }));
        }

        [Test, SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void Colorize_ThrowsException_ForNullApplyColor()
        {
            var graph = new LiteralGraph("A", false);

            Assert.Throws<ArgumentNullException>(() => graph.Colorize(null));
        }
    }
}