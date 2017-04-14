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
    using System.Linq;
    using System.Collections.Generic;
    using NUnit.Framework;
    using Abacaxi.Graphs;

    internal sealed class IntegerSequenceGraph : IGraph<int, int, int>
    {
        private int[] _elements;

        public IntegerSequenceGraph(IEnumerable<int> elements)
        {
            Assert.NotNull(elements);

            _elements = elements.ToArray();
        }

        public int GetValue(int index)
        {
            Assert.IsTrue(index >= 0 && index < _elements.Length);
            return _elements[index];
        }

        public void SetValue(int index, int value)
        {
            Assert.IsTrue(index >= 0 && index < _elements.Length);
            _elements[index] = value;
        }

        public IEnumerable<Connection<int, int>> GetConnections(int index)
        {
            Assert.IsTrue(index >= 0 && index < _elements.Length);

            for (var i = 0; i < _elements.Length; i++)
            {
                if (i != index)
                    yield return new Connection<int, int>(index, i, Math.Abs(_elements[i] - _elements[index]));
            }
        }

        public IEnumerable<int> GetNodes()
        {
            for (var i = 0; i < _elements.Length; i++)
            {
                yield return i;
            }
        }

        public int AddConnectionCosts(int a, int b)
        {
            Assert.IsTrue(a >= 0);
            Assert.IsTrue(b >= 0);

            return a + b;
        }

        public int CompareConnectionCosts(int a, int b)
        {
            Assert.IsTrue(a >= 0);
            Assert.IsTrue(b >= 0);

            return a - b;
        }

        public int EvaluatePotentialConnectionCost(int fromIndex, int toIndex)
        {
            Assert.IsTrue(fromIndex >= 0 && fromIndex < _elements.Length);
            Assert.IsTrue(toIndex >= 0 && toIndex < _elements.Length);

            return Math.Abs(_elements[fromIndex] - _elements[toIndex]);
        }
    }
}
