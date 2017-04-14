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

namespace Abacaxi.Graphs
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using LinkedLists;

    /// <summary>
    /// Class implements the algorithms that return the full path that includes all nodes in a graph.
    /// </summary>
    public static class CompletePath
    {
        /// <summary>
        /// Finds "a good" sequence which cycles through all the nodes in a given <paramref name="graph"/>.
        /// </summary>
        /// <typeparam name="TValue">The type of the value stored in the graph nodes.</typeparam>
        /// <typeparam name="TIdentifier">The type of the values used to identify nodes in the graph.</typeparam>
        /// <typeparam name="TCost">The type of the cost of connections.</typeparam>
        /// <param name="graph">The graphe.</param>
        /// <returns>The sequence of node identifiers ordered in the visitation path..</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="graph"/> is <c>null</c>.</exception>
        public static IEnumerable<TIdentifier> FindHeuristical<TValue, TIdentifier, TCost>(IGraph<TValue, TIdentifier, TCost> graph)
        {
            Validate.ArgumentNotNull(nameof(graph), graph);

            var allNodes = graph.GetNodes().ToArray();
            if (allNodes.Length == 0)
            {
                yield break;
            }
            else if (allNodes.Length == 1)
            {
                yield return allNodes[0];
                yield break;
            }

            var nodeToIndex = new Dictionary<TIdentifier, int>();
            for (var i = 0; i < allNodes.Length; i++)
            {
                nodeToIndex.Add(allNodes[i], i);
            }

            var costs = new TCost[allNodes.Length, allNodes.Length];
            var allowed = new bool[allNodes.Length, allNodes.Length];

            foreach (var node in allNodes)
            {
                var nodeConnections = graph.GetConnections(node);
                foreach (var connection in nodeConnections)
                {
                    var indexOfTo = nodeToIndex[connection.To];
                    var indexOfNode = nodeToIndex[node];

                    allowed[indexOfNode, indexOfTo] = true;
                    costs[indexOfNode, indexOfTo] = connection.Cost;
                }
            }

            var connections = new int[allNodes.Length];
            for(int i = 0; i < connections.Length; i++)
            {
                connections[i] = -1;
            }

            for (;;)
            {
                Tuple<int, int, TCost> minPair = null; 

                for (var x = 0; x < allNodes.Length; x++)
                {
                    for (var y = 0; y < allNodes.Length; y++)
                    {
                        if (allowed[x, y] && (minPair == null || graph.CompareConnectionCosts(minPair.Item3, costs[x, y]) > 0))
                        {
                            minPair = Tuple.Create(x, y, costs[x, y]);
                        }
                    }
                }

                if (minPair != null)
                {
                    connections[minPair.Item1] = minPair.Item2;

                    for (var i = 0; i < allNodes.Length; i++)
                    {
                        allowed[i, minPair.Item2] = false;
                        allowed[minPair.Item1, i] = false;
                    }

                    allowed[minPair.Item2, minPair.Item1] = false;
                }
                else
                {
                    break;
                }
            }

            var result = new List<TIdentifier>();
            var currentIndex = 0;
            do
            {
                result.Add(allNodes[currentIndex]);
                currentIndex = connections[currentIndex];
            } while (currentIndex > 0);

            if (result.Count != allNodes.Length)
            {
                yield break;
            }
            else
            {
                foreach (var node in result)
                {
                    yield return node;
                }
            }
        }
    }
}
