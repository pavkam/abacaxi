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


using System.Diagnostics;
using Abacaxi.Containers;

namespace Abacaxi.Graphs
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    /// <summary>
    /// Describes a weighted graph class.
    /// </summary>
    /// <typeparam name="TVertex">The type of graph vertices.</typeparam>
    /// <typeparam name="TWeight">The type of edge weights.</typeparam>
    public abstract class WeightedGraph<TVertex, TWeight> : Graph<TVertex>
    {
        private sealed class PathNode
        {
            public TVertex Vertex;
            public PathNode Parent;
            public TWeight TotalCostFromStart;
            public TWeight PotentialCostToDestination;
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
            return GetEdgesAndWeights(vertex).Select(weightedEdge => new Edge<TVertex>(weightedEdge.FromVertex, weightedEdge.ToVertex));
        }

        /// <summary>
        /// Gets the edges of a given <paramref name="vertex"/>.
        /// </summary>
        /// <param name="vertex">The vertex.</param>
        /// <returns>A sequence of edges connecting the <paramref name="vertex"/> to other vertices.</returns>
        public abstract IEnumerable<WeightedEdge<TVertex, TWeight>> GetEdgesAndWeights(TVertex vertex);

        /// <summary>
        /// Adds two weights.
        /// </summary>
        /// <param name="left">The left weight.</param>
        /// <param name="right">The right weight.</param>
        /// <returns>The sum of two weights.</returns>
        public abstract TWeight AddWeights(TWeight left, TWeight right);

        /// <summary>
        /// Compares two weights.
        /// </summary>
        /// <param name="left">The left weight.</param>
        /// <param name="right">The right weight.</param>
        /// <returns>The comparison result.</returns>
        public abstract int CompareWeights(TWeight left, TWeight right);

        /// <summary>
        /// Gets the potential total weight connecting <paramref name="fromVertex"/> and <paramref name="toVertex"/> vertices.
        /// </summary>
        /// <param name="fromVertex">The first vertex.</param>
        /// <param name="toVertex">The destination vertex.</param>
        /// <returns>The potential total cost.</returns>
        public abstract TWeight GetPotentialWeight(TVertex fromVertex, TVertex toVertex);


        public IEnumerable<TVertex> FindCheapestPath(TVertex fromVertex, TVertex toVertex)
        {
            var comparer = Comparer<PathNode>.Create((a, b) =>
            {
                Debug.Assert(a != null);
                Debug.Assert(b != null);

                return CompareWeights(a.PotentialCostToDestination, b.PotentialCostToDestination);
            });
            
            var startVertexNode = new PathNode
            {
                Vertex = fromVertex,
                TotalCostFromStart = default(TWeight),
                PotentialCostToDestination =
                    SupportsPotentialWeightEvaluation ? GetPotentialWeight(fromVertex, toVertex) : default(TWeight),
            };

            var discoveredVertices = new Dictionary<TVertex, PathNode> {{fromVertex, startVertexNode}};
            var visitationQueue = new Heap<PathNode>(comparer) {startVertexNode};
            var foundAPath = false;
            var cheapestCostSoFar = default(TWeight);

            while (visitationQueue.Count > 0)
            {
                var vertexNode = visitationQueue.RemoveTop();
                Debug.Assert(vertexNode != null && discoveredVertices.ContainsKey(vertexNode.Vertex));

                if (Equals(vertexNode.Vertex, toVertex))
                {
                    if (!foundAPath || CompareWeights(cheapestCostSoFar, vertexNode.TotalCostFromStart) > 0)
                    {
                        foundAPath = true;
                        cheapestCostSoFar = vertexNode.TotalCostFromStart;
                    }
                }
                else if (foundAPath && CompareWeights(cheapestCostSoFar, vertexNode.TotalCostFromStart) < 0)
                {
                    continue;
                }

                foreach (var edge in GetEdgesAndWeights(vertexNode.Vertex))
                {
                    var costFromStartForThisPath = AddWeights(vertexNode.TotalCostFromStart, edge.Weight);

                    if (!discoveredVertices.TryGetValue(edge.ToVertex, out PathNode discoveredNode))
                    {
                        discoveredNode = new PathNode
                        {
                            Vertex = edge.ToVertex,
                            Parent = vertexNode,
                            TotalCostFromStart = costFromStartForThisPath,
                            PotentialCostToDestination =
                                SupportsPotentialWeightEvaluation
                                    ? AddWeights(costFromStartForThisPath, GetPotentialWeight(edge.ToVertex, toVertex))
                                    : costFromStartForThisPath,
                        };

                        discoveredVertices.Add(discoveredNode.Vertex, discoveredNode);
                        visitationQueue.Add(discoveredNode);
                    }
                    else
                    {
                        if (CompareWeights(costFromStartForThisPath, discoveredNode.TotalCostFromStart) < 0)
                        {
                            discoveredNode.TotalCostFromStart = costFromStartForThisPath;
                            discoveredNode.Parent = vertexNode;
                            discoveredNode.PotentialCostToDestination =
                                SupportsPotentialWeightEvaluation
                                    ? AddWeights(costFromStartForThisPath, GetPotentialWeight(discoveredNode.Vertex, toVertex))
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
