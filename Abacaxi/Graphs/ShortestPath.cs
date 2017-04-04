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

    /// <summary>
    /// Class implements the "shortest path" algorithm. The method can be applied to any graph.
    /// </summary>
    public static class ShortestPath
    {
        /// <summary>
        /// Finds the shortest path between two nodes in a graph (smallest number of hops).
        /// </summary>
        /// <typeparam name="TValue">The value of the graph nodes.</typeparam>
        /// <typeparam name="TIdentifier">The type used to identify nodes in the graph.</typeparam>
        /// <typeparam name="TCost">The node connection cost type.</typeparam>
        /// <param name="graph">The graph.</param>
        /// <param name="nodePredicate">Predicate used to decide whether a graph node can be visited.</param>
        /// <param name="startingNodeIdentifier">The starting node.</param>
        /// <param name="endingNodeIdentifier">The final node.</param>
        /// <returns>A sequence of graph node identifiers, representing the shortest path.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="graph"/> or <paramref name="nodePredicate"/> are <c>null</c>.</exception>
        public static IEnumerable<TIdentifier> Find<TValue, TIdentifier, TCost>(
            Graph<TValue, TIdentifier, TCost> graph,
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

            var visitedNodeIdentifiers = new Dictionary<TIdentifier, TIdentifier>();
            visitedNodeIdentifiers.Add(startingNodeIdentifier, startingNodeIdentifier);

            var nodesToVisitNext = new Queue<TIdentifier>();
            nodesToVisitNext.Enqueue(startingNodeIdentifier);

            while (nodesToVisitNext.Count > 0)
            {
                var visitedNodeIdentifier = nodesToVisitNext.Dequeue();

                foreach (var connection in graph.GetConnections(visitedNodeIdentifier))
                {
                    if (!nodePredicate(graph, connection.To))
                    {
                        continue;
                    }

                    TIdentifier incomingNodeIdentifier;
                    if (!visitedNodeIdentifiers.TryGetValue(connection.To, out incomingNodeIdentifier))
                    {
                        visitedNodeIdentifiers.Add(connection.To, visitedNodeIdentifier);

                        if (Equals(connection.To, endingNodeIdentifier))
                        {
                            var road = new List<TIdentifier>();

                            while (!Equals(endingNodeIdentifier, startingNodeIdentifier))
                            {
                                var previousNodeIdentifier = visitedNodeIdentifiers[endingNodeIdentifier];
                                road.Add(endingNodeIdentifier);

                                endingNodeIdentifier = previousNodeIdentifier;
                            }

                            road.Add(startingNodeIdentifier);

                            for (var i = road.Count - 1; i >= 0; i--)
                            {
                                yield return road[i];
                            }
                        }

                        nodesToVisitNext.Enqueue(connection.To);
                    }
                }
            }
        }
    }
}
