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
    /// Class implements the "best path" algorithm (the A*, optimized Djikstra).
    /// </summary>
    public static class BestPath
    {
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
