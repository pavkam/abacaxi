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
    /// The "flood fill" algorithm. For any given graph the algorithm will start at a given position and will fill in the
    /// matrix with a given "color". This class contains two implementations, one recursive and the other one - iterative.
    /// </summary>
    public static class FloodFill
    {
        private static void ApplyRecursiveNoChecks<TColor, TIdentifier>(
            Graph<TColor, TIdentifier> graph,
            TIdentifier startingNodeIdentifier,
            NodePredicate<TColor, TIdentifier> nodePredicate,
            TColor color)
        {
            Debug.Assert(graph != null);
            Debug.Assert(startingNodeIdentifier != null);

            if (nodePredicate(graph, startingNodeIdentifier))
            {
                graph.SetNodeValue(startingNodeIdentifier, color);
                foreach (var connectedNodeIdentifier in graph.GetNodeConnections(startingNodeIdentifier))
                {
                    ApplyRecursiveNoChecks(graph, connectedNodeIdentifier, nodePredicate, color);
                }
            }
        }

        /// <summary>
        /// Flood-fills a graph recursively with a given <paramref name="color"/>.
        /// </summary>
        /// <typeparam name="TColor">The type of the "color" of graph nodes.</typeparam>
        /// <typeparam name="TIdentifier">The type of the graph node identifiers.</typeparam>
        /// <param name="graph">The graph to fill.</param>
        /// <param name="startingNodeIdentifier">The starting node identifier.</param>
        /// <param name="nodePredicate">Predicate to check whether a certain node can be colored.</param>
        /// <param name="color">The color to fill the nodes with.</param>
        /// <exception cref="ArgumentNullException">Thrown if either <paramref name="graph"/> or <paramref name="nodePredicate"/> are null.</exception>
        public static void ApplyRecursive<TColor, TIdentifier>(
            Graph<TColor, TIdentifier> graph,
            TIdentifier startingNodeIdentifier,
            NodePredicate<TColor, TIdentifier> nodePredicate,
            TColor color)
        {
            Validate.ArgumentNotNull(nameof(graph), graph);
            Validate.ArgumentNotNull(nameof(nodePredicate), nodePredicate);

            ApplyRecursiveNoChecks(graph, startingNodeIdentifier, nodePredicate, color);
        }

        /// <summary>
        /// Flood-fills a graph iteratively with a given <paramref name="color"/>.
        /// </summary>
        /// <typeparam name="TColor">The type of the "color" of graph nodes.</typeparam>
        /// <typeparam name="TIdentifier">The type of the graph node identifiers.</typeparam>
        /// <param name="graph">The graph to fill.</param>
        /// <param name="startingNodeIdentifier">The starting node identifier.</param>
        /// <param name="nodePredicate">Predicate to check whether a certain node can be colored.</param>
        /// <param name="color">The color to fill the nodes with.</param>
        /// <exception cref="ArgumentNullException">Thrown if either <paramref name="graph"/> or <paramref name="nodePredicate"/> are null.</exception>
        public static void ApplyIterative<TColor, TIdentifier>(
            Graph<TColor, TIdentifier> graph,
            TIdentifier startingNodeIdentifier,
            NodePredicate<TColor, TIdentifier> nodePredicate,
            TColor color)
        {
            Validate.ArgumentNotNull(nameof(graph), graph);
            Validate.ArgumentNotNull(nameof(nodePredicate), nodePredicate);

            var nodesToVisitNext = new Queue<TIdentifier>();
            nodesToVisitNext.Enqueue(startingNodeIdentifier);

            while (nodesToVisitNext.Count > 0)
            {
                var visitiedNodeIdentifier = nodesToVisitNext.Dequeue();
                if (nodePredicate(graph, visitiedNodeIdentifier))
                {
                    graph.SetNodeValue(visitiedNodeIdentifier, color);
                    foreach (var connectedNodeIdentifier in graph.GetNodeConnections(visitiedNodeIdentifier))
                    {
                        nodesToVisitNext.Enqueue(connectedNodeIdentifier);
                    }
                }
            }
        }
    }
}
