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
    using System.Linq;
    using System.Collections.Generic;
    using System.Diagnostics;
    using Containers;

    /// <summary>
    /// Describes a weighted graph class.
    /// </summary>
    /// <typeparam name="TVertex">The type of graph vertices.</typeparam>
    public abstract class WeightedGraph<TVertex> : Graph<TVertex>
    {
        private sealed class PathNode
        {
            public TVertex Vertex;
            public PathNode Parent;
            public double TotalCostFromStart;
            public double PotentialCostToDestination;
        }

        /// <summary>
        /// Gets a value indicating whether this graph supports potential weight evaluation (heuristics).
        /// </summary>
        /// <value>
        ///   <c>true</c> if graph supports potential weight evaluation; otherwise, <c>false</c>.
        /// </value>
        public abstract bool SupportsPotentialWeightEvaluation { get; }

        /// <summary>
        /// Gets the edges for a given <param name="vertex"/>.
        /// </summary>
        /// <param name="vertex">The vertex to get the edges for.</param>
        /// <returns>A sequence of edges connected to the given <param name="vertex"/></returns>
        /// <exception cref="InvalidOperationException">The <paramref name="vertex"/> is not part of this graph.</exception>
        public sealed override IEnumerable<Edge<TVertex>> GetEdges(TVertex vertex)
        {
            return GetEdgesAndWeights(vertex);
        }

        /// <summary>
        /// Gets the edges of a given <paramref name="vertex"/>.
        /// </summary>
        /// <param name="vertex">The vertex.</param>
        /// <returns>A sequence of edges connecting the <paramref name="vertex"/> to other vertices.</returns>
        public abstract IEnumerable<WeightedEdge<TVertex>> GetEdgesAndWeights(TVertex vertex);

        /// <summary>
        /// Gets the potential total weight connecting <paramref name="fromVertex"/> and <paramref name="toVertex"/> vertices.
        /// </summary>
        /// <param name="fromVertex">The first vertex.</param>
        /// <param name="toVertex">The destination vertex.</param>
        /// <returns>The potential total cost.</returns>
        public abstract double GetPotentialWeight(TVertex fromVertex, TVertex toVertex);

        /// <summary>
        /// Finds the cheapest path in a graph between two vertices <paramref name="fromVertex"/> and <paramref name="toVertex"/>
        /// </summary>
        /// <param name="fromVertex">The start vertex.</param>
        /// <param name="toVertex">The end vertex.</param>
        /// <returns>A sequence of vertices that yield the shortest path. Returns an empty sequence if no path available.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="fromVertex"/> is not part of teh graph.</exception>
        public IEnumerable<TVertex> FindCheapestPath(TVertex fromVertex, TVertex toVertex)
        {
            var comparer = Comparer<PathNode>.Create((a, b) =>
            {
                Debug.Assert(a != null);
                Debug.Assert(b != null);

                return a.PotentialCostToDestination.CompareTo(b.PotentialCostToDestination);
            });
            
            var startVertexNode = new PathNode
            {
                Vertex = fromVertex,
                TotalCostFromStart = 0,
                PotentialCostToDestination =
                    SupportsPotentialWeightEvaluation ? GetPotentialWeight(fromVertex, toVertex) : 0,
            };

            var discoveredVertices = new Dictionary<TVertex, PathNode> {{fromVertex, startVertexNode}};
            var visitationQueue = new Heap<PathNode>(comparer) {startVertexNode};
            var foundAPath = false;
            var cheapestCostSoFar = .0;

            while (visitationQueue.Count > 0)
            {
                var vertexNode = visitationQueue.RemoveTop();
                Debug.Assert(vertexNode != null && discoveredVertices.ContainsKey(vertexNode.Vertex));

                if (Equals(vertexNode.Vertex, toVertex))
                {
                    if (!foundAPath || cheapestCostSoFar > vertexNode.TotalCostFromStart)
                    {
                        foundAPath = true;
                        cheapestCostSoFar = vertexNode.TotalCostFromStart;
                    }
                }
                else if (foundAPath && cheapestCostSoFar < vertexNode.TotalCostFromStart)
                {
                    continue;
                }

                foreach (var edge in GetEdgesAndWeights(vertexNode.Vertex))
                {
                    var costFromStartForThisPath = vertexNode.TotalCostFromStart + edge.Weight;

                    if (!discoveredVertices.TryGetValue(edge.ToVertex, out PathNode discoveredNode))
                    {
                        discoveredNode = new PathNode
                        {
                            Vertex = edge.ToVertex,
                            Parent = vertexNode,
                            TotalCostFromStart = costFromStartForThisPath,
                            PotentialCostToDestination =
                                SupportsPotentialWeightEvaluation
                                    ? costFromStartForThisPath + GetPotentialWeight(edge.ToVertex, toVertex)
                                    : costFromStartForThisPath,
                        };

                        discoveredVertices.Add(discoveredNode.Vertex, discoveredNode);
                        visitationQueue.Add(discoveredNode);
                    }
                    else
                    {
                        if (costFromStartForThisPath < discoveredNode.TotalCostFromStart)
                        {
                            discoveredNode.TotalCostFromStart = costFromStartForThisPath;
                            discoveredNode.Parent = vertexNode;
                            discoveredNode.PotentialCostToDestination =
                                SupportsPotentialWeightEvaluation
                                    ? costFromStartForThisPath + GetPotentialWeight(discoveredNode.Vertex, toVertex)
                                    : costFromStartForThisPath;

                            visitationQueue.Add(discoveredNode);
                        }
                    }
                }
            }

            var result = new List<TVertex>();
            if (foundAPath)
            {
                var node = discoveredVertices[toVertex];
                do
                {
                    result.Add(node.Vertex);
                    node = node.Parent;
                } while (node != null);

                result.Reverse();
            }

            return result;
        }
    }
}
