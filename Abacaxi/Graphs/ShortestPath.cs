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

    public static class ShortestPath
    {
        private sealed class Path<TIndentifier>
        {
            public TIndentifier _entryNodeIdentifier;
            public int _numberOfHops;

            public Path(TIndentifier entryNodeIdentifier, int numberOfHops)
            {
                Debug.Assert(numberOfHops >= 0);

                _entryNodeIdentifier = entryNodeIdentifier;
                _numberOfHops = numberOfHops;
            }
        }

        public static IEnumerable<TIdentifier> Find<TValue, TIdentifier>(
            Graph<TValue, TIdentifier> graph,
            TIdentifier startingNodeIdentifier,
            TIdentifier endingNodeIdentifier)
        {
            Validate.ArgumentNotNull(nameof(graph), graph);

            var paths = new Dictionary<TIdentifier, Path<TIdentifier>>();
            paths.Add(startingNodeIdentifier, new Path<TIdentifier>(null, 0));

            var nodesToVisitNext = new Queue<TIdentifier>();
            nodesToVisitNext.Enqueue(startingNodeIdentifier);

            while (nodesToVisitNext.Count > 0)
            {
                var visitedNodeIdentifier = nodesToVisitNext.Dequeue();
                var visitedNodePath = paths[visitedNodeIdentifier];

                foreach (var connectedNodeIdentifier in graph.GetNodeConnections(visitedNodeIdentifier))
                {
                    Path<TIdentifier> connectedNodePath;

                    if (!paths.TryGetValue(connectedNodeIdentifier, out connectedNodePath))
                    {
                        connectedNodePath = new Path<TIdentifier>(visitedNodeIdentifier, visitedNodePath._numberOfHops + 1);
                        paths.Add(connectedNodeIdentifier, connectedNodePath);

                        if (Equals(connectedNodeIdentifier, endingNodeIdentifier))
                        {
                            break;
                        }

                        nodesToVisitNext.Enqueue(connectedNodeIdentifier);
                    }
                    else if (connectedNodePath._numberOfHops > visitedNodePath._numberOfHops + 1)
                    {
                        connectedNodePath._numberOfHops = visitedNodePath._numberOfHops + 1;
                        connectedNodePath._entryNodeIdentifier = visitedNodeIdentifier;
                        nodesToVisitNext.Enqueue(connectedNodeIdentifier);
                    }
                }
            }
        }
    }
}
