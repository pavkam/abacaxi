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
    using Containers;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    /// <summary>
    /// Class implements the "cheapest path" algorithm (the A*, optimized Djikstra).
    /// </summary>
    public static class CheapestPath
    {
        private sealed class Mark<TIdentifier, TCost>
        {
            public TIdentifier NodeIdentifier { get; set; }
            public TIdentifier IncomingNodeIdentifier { get; set; }
            public TCost CostFromStart { get; set; }
            public TCost PotentialTotalCost { get; set; }
        }

        /// <summary>
        /// Finds the cheapest path between two nodes in a graph.
        /// </summary>
        /// <typeparam name="TValue">The type of the value stored in graph nodes.</typeparam>
        /// <typeparam name="TIdentifier">The type of the graph node identifier.</typeparam>
        /// <typeparam name="TCost">The type of the graph connection cost.</typeparam>
        /// <param name="graph">The graph.</param>
        /// <param name="nodePredicate">The node predicate.</param>
        /// <param name="startingNodeIdentifier">Starting node identifier.</param>
        /// <param name="endingNodeIdentifier">Ending node identifier.</param>
        /// <returns>The sequence of nodes that define the cheapest path.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="graph"/> or <paramref name="nodePredicate"/> are <c>null</c>.</exception>
        public static IEnumerable<TIdentifier> Find<TValue, TIdentifier, TCost>(
            IGraph<TValue, TIdentifier, TCost> graph,
            NodePredicate<TValue, TIdentifier, TCost> nodePredicate,
            TIdentifier startingNodeIdentifier,
            TIdentifier endingNodeIdentifier)
        {
            Validate.ArgumentNotNull(nameof(graph), graph);
            Validate.ArgumentNotNull(nameof(nodePredicate), nodePredicate);

            if (!nodePredicate(graph, startingNodeIdentifier))
            {
                yield break;
            }

            if (Equals(startingNodeIdentifier, endingNodeIdentifier))
            {
                yield return startingNodeIdentifier;
                yield break;
            }

            Comparison<Mark<TIdentifier, TCost>> comparison = (left, right) =>
            {
                Debug.Assert(left != null);
                Debug.Assert(right != null);

                return graph.CompareConnectionCosts(left.PotentialTotalCost, right.PotentialTotalCost);
            };

            var visitedNodes = new Dictionary<TIdentifier, Mark<TIdentifier, TCost>>();
            var nodesToVisitNext = new Heap<Mark<TIdentifier, TCost>>(Comparer<Mark<TIdentifier, TCost>>.Create(comparison));

            var startingMark = new Mark<TIdentifier, TCost>()
            {
                NodeIdentifier = startingNodeIdentifier,
                IncomingNodeIdentifier = startingNodeIdentifier,
                CostFromStart = graph.EvaluatePotentialConnectionCost(startingNodeIdentifier, startingNodeIdentifier),
                PotentialTotalCost = graph.EvaluatePotentialConnectionCost(startingNodeIdentifier, endingNodeIdentifier)
            };

            visitedNodes.Add(startingNodeIdentifier, startingMark);
            nodesToVisitNext.Add(startingMark);

            var cheapestCostFoundSoFar = default(TCost);
            var pathWasFound = false;

            while (nodesToVisitNext.Count > 0)
            {
                var visitedMark = nodesToVisitNext.RemoveTop();

                if (pathWasFound && graph.CompareConnectionCosts(cheapestCostFoundSoFar, visitedMark.CostFromStart) < 0)
                {
                    continue;
                }
                else if (Equals(endingNodeIdentifier, visitedMark.NodeIdentifier))
                {
                    pathWasFound = true;
                    cheapestCostFoundSoFar = visitedMark.CostFromStart;

                    continue;
                }

                foreach (var connection in graph.GetConnections(visitedMark.NodeIdentifier))
                {
                    if (!nodePredicate(graph, connection.To))
                    {
                        continue;
                    }

                    Mark<TIdentifier, TCost> connectedMark;
                    var costFromStartNodeToConnectedNode = graph.AddConnectionCosts(visitedMark.CostFromStart, connection.Cost);
                    if (visitedNodes.TryGetValue(connection.To, out connectedMark))
                    {
                        if (graph.CompareConnectionCosts(connectedMark.CostFromStart, costFromStartNodeToConnectedNode) > 0)
                        {
                            connectedMark.IncomingNodeIdentifier = visitedMark.NodeIdentifier;
                            connectedMark.CostFromStart = graph.AddConnectionCosts(visitedMark.CostFromStart, connection.Cost);
                            connectedMark.PotentialTotalCost = graph.AddConnectionCosts(connectedMark.CostFromStart,
                                graph.EvaluatePotentialConnectionCost(connectedMark.NodeIdentifier, endingNodeIdentifier));

                            nodesToVisitNext.Add(connectedMark);
                        }
                    }
                    else
                    {
                        connectedMark = new Mark<TIdentifier, TCost>()
                        {
                            NodeIdentifier = connection.To,
                            IncomingNodeIdentifier = visitedMark.NodeIdentifier,
                            CostFromStart = costFromStartNodeToConnectedNode,
                            PotentialTotalCost = graph.AddConnectionCosts(costFromStartNodeToConnectedNode,
                                graph.EvaluatePotentialConnectionCost(connection.To, endingNodeIdentifier))
                        };

                        visitedNodes.Add(connection.To, connectedMark);
                        nodesToVisitNext.Add(connectedMark);
                    }
                }
            }

            if (pathWasFound)
            {
                var road = new List<TIdentifier>();
                var currentNodeIdentifier = endingNodeIdentifier;

                while (!Equals(currentNodeIdentifier, startingNodeIdentifier))
                {
                    var previousNodeIdentifier = visitedNodes[currentNodeIdentifier].IncomingNodeIdentifier;
                    road.Add(currentNodeIdentifier);

                    currentNodeIdentifier = previousNodeIdentifier;
                }

                road.Add(startingNodeIdentifier);

                for (var i = road.Count - 1; i >= 0; i--)
                {
                    yield return road[i];
                }
            }
        }
    }
}
