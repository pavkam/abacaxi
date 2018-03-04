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

namespace Abacaxi.Graphs
{
    using System;
    using System.Collections;
    using System.Linq;
    using System.Collections.Generic;
    using Internal;
    using JetBrains.Annotations;
    using System.Diagnostics.CodeAnalysis;
    using Containers;

    /// <summary>
    /// Generic graph class. This class serves as an abstract base for all concrete implementations.
    /// </summary>
    /// <typeparam name="TVertex">The type of graph vertices.</typeparam>
    [PublicAPI, SuppressMessage("ReSharper", "VirtualMemberNeverOverridden.Global")]
    public abstract class Graph<TVertex> : IEnumerable<TVertex>
    {
        private sealed class PathNode
        {
            public TVertex Vertex;
            [CanBeNull] public PathNode Parent;
            public double TotalCostFromStart;
            public double PotentialCostToDestination;
        }

        private sealed class BfsNode : IBfsNode
        {
            public TVertex Vertex { get; }
            public IBfsNode Parent { get; set; }
            public Edge<TVertex> EntryEdge { get; }

            public BfsNode([NotNull] TVertex vertex)
            {
                Assert.NotNull(vertex);
                Vertex = vertex;
            }

            public BfsNode([NotNull] Edge<TVertex> entryEdge)
            {
                Assert.NotNull(entryEdge);
                Vertex = entryEdge.ToVertex;
                EntryEdge = entryEdge;
            }
        }

        private sealed class DfsNode : IDfsNode
        {
            public TVertex Vertex { get; }
            public IDfsNode Parent { get; set; }
            public Edge<TVertex> EntryEdge { get; }
            public int EntryTime { get; set; }
            public int ExitTime { get; set; }

            public DfsNode([NotNull] TVertex vertex)
            {
                Assert.NotNull(vertex);
                Vertex = vertex;
            }

            public DfsNode([NotNull] Edge<TVertex> entryEdge)
            {
                Assert.NotNull(entryEdge);
                Vertex = entryEdge.ToVertex;
                EntryEdge = entryEdge;
            }
        }

        private static int CompareEdgesByWeight([NotNull] Edge<TVertex> a, [NotNull] Edge<TVertex> b)
        {
            Assert.NotNull(a);
            Assert.NotNull(b);

            return a.Weight.CompareTo(a.Weight);
        }

        [NotNull] private static IComparer<Edge<TVertex>> _edgesByWeightComparer =
            Comparer<Edge<TVertex>>.Create(CompareEdgesByWeight);

        private bool TraverseDfs(
            [NotNull] DfsNode vertexNode,
            ref int time,
            [NotNull] IDictionary<TVertex, DfsNode> visitedNodes,
            [NotNull] Predicate<IDfsNode> handleVertexVisited,
            [NotNull] Predicate<IDfsNode> handleVertexCompleted,
            [NotNull] Func<IDfsNode, IDfsNode, bool> handleCycle)
        {
            Assert.NotNull(vertexNode);
            Assert.NotNull(visitedNodes);
            Assert.NotNull(handleVertexCompleted);
            Assert.NotNull(handleVertexVisited);
            Assert.NotNull(handleCycle);

            visitedNodes.Add(vertexNode.Vertex, vertexNode);
            vertexNode.EntryTime = time++;

            if (!handleVertexVisited(vertexNode))
            {
                return false;
            }

            var breakRequested = false;

            foreach (var edge in GetEdges(vertexNode.Vertex))
            {
                if (breakRequested)
                {
                    break;
                }

                if (!visitedNodes.TryGetValue(edge.ToVertex, out var visitedNode))
                {
                    visitedNode = new DfsNode(edge)
                    {
                        Parent = vertexNode,
                    };

                    breakRequested =
                        !TraverseDfs(visitedNode, ref time, visitedNodes, handleVertexVisited, handleVertexCompleted,
                            handleCycle);
                }
                else if ((IsDirected || visitedNode != vertexNode.Parent) &&
                         visitedNode.EntryTime <= vertexNode.EntryTime)
                {
                    if (!handleCycle(vertexNode, visitedNode))
                    {
                        breakRequested = true;
                    }
                }
            }

            vertexNode.ExitTime = time++;
            if (!handleVertexCompleted(vertexNode))
            {
                breakRequested = true;
            }

            return !breakRequested;
        }

        /// <summary>
        /// Describes a node in a BFS traversal tree.
        /// </summary>
        public interface IBfsNode
        {
            /// <summary>
            /// Gets the vertex of the BFS node.
            /// </summary>
            /// <value>
            /// The vertex.
            /// </value>
            [NotNull]
            TVertex Vertex { get; }

            /// <summary>
            /// Gets the parent BFS node in the traversal tree.
            /// </summary>
            /// <value>
            /// The parent node.
            /// </value>
            [CanBeNull]
            IBfsNode Parent { get; }

            /// <summary>
            /// Gets the entry edge (the edge connecting <see cref="Parent"/> and <see cref="Vertex"/>.
            /// </summary>
            /// <value>
            /// The entry edge.
            /// </value>
            [CanBeNull]
            Edge<TVertex> EntryEdge { get; }
        }

        /// <summary>
        /// Describes a node in a DFS traversal tree.
        /// </summary>
        public interface IDfsNode
        {
            /// <summary>
            /// Gets the vertex of the BFS node.
            /// </summary>
            /// <value>
            /// The vertex.
            /// </value>
            [NotNull]
            TVertex Vertex { get; }

            /// <summary>
            /// Gets the parent DFS node in the traversal tree.
            /// </summary>
            /// <value>
            /// The parent node.
            /// </value>
            [CanBeNull]
            IDfsNode Parent { get; }

            /// <summary>
            /// Gets the entry edge (the edge connecting <see cref="Parent"/> and <see cref="Vertex"/>.
            /// </summary>
            /// <value>
            /// The entry edge.
            /// </value>
            [CanBeNull]
            Edge<TVertex> EntryEdge { get; }

            /// <summary>
            /// Gets the vertex "entry time".
            /// </summary>
            /// <value>
            /// The entry time.
            /// </value>
            int EntryTime { get; }

            /// <summary>
            /// Gets the vertex "exit time".
            /// </summary>
            /// <value>
            /// The exit time.
            /// </value>
            int ExitTime { get; }
        }

        /// <summary>
        /// Asserts this graph is undirected.
        /// </summary>
        /// <exception cref="InvalidOperationException">This operation is not allowed on directed graphs.</exception>
        protected void RequireUndirectedGraph()
        {
            if (IsDirected)
            {
                throw new InvalidOperationException("This operation is not allowed on directed graphs.");
            }
        }

        /// <summary>
        /// Gets a value indicating whether this graph's edges are directed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this graph's edges are directed; otherwise, <c>false</c>.
        /// </value>
        public abstract bool IsDirected { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is read only.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is read only; otherwise, <c>false</c>.
        /// </value>
        public abstract bool IsReadOnly { get; }

        /// <summary>
        /// Gets a value indicating whether this graph supports potential weight evaluation (heuristics).
        /// </summary>
        /// <value>
        ///   <c>true</c> if graph supports potential weight evaluation; otherwise, <c>false</c>.
        /// </value>
        public abstract bool SupportsPotentialWeightEvaluation { get; }

        /// <summary>
        /// Gets the potential total weight connecting <paramref name="fromVertex"/> and <paramref name="toVertex"/> vertices.
        /// </summary>
        /// <param name="fromVertex">The first vertex.</param>
        /// <param name="toVertex">The destination vertex.</param>
        /// <returns>The potential total cost.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="fromVertex"/> or <paramref name="toVertex"/> is <c>null</c>.</exception>
        public abstract double GetPotentialWeight([NotNull] TVertex fromVertex, [NotNull] TVertex toVertex);

        /// <summary>
        /// Gets the edges for a given <paramref name="vertex"/>.
        /// </summary>
        /// <param name="vertex">The vertex to get the edges for.</param>
        /// <returns>A sequence of edges connected to the given <paramref name="vertex"/></returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="vertex"/> is not part of this graph.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="vertex"/> is <c>null</c>.</exception>
        [NotNull, ItemNotNull]
        public abstract IEnumerable<Edge<TVertex>> GetEdges([NotNull] TVertex vertex);

        /// <summary>
        /// Returns an enumerator that iterates all vertices in the graph.
        /// </summary>
        /// <returns>
        /// A <see cref="IEnumerator{TVertex}" /> that can be used to iterate through the collection.
        /// </returns>
        public abstract IEnumerator<TVertex> GetEnumerator();

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// Traverses the graph using the breadth-first-search starting from <paramref name="startVertex"/>.
        /// </summary>
        /// <param name="startVertex">The start vertex.</param>
        /// <param name="handleVertexCompleted">The function called when a vertex is completed.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="startVertex"/> or <paramref name="handleVertexCompleted"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="startVertex"/> is not part of this graph.</exception>
        public virtual void TraverseBfs([NotNull] TVertex startVertex,
            [NotNull] Predicate<IBfsNode> handleVertexCompleted)
        {
            Validate.ArgumentNotNull(nameof(startVertex), startVertex);
            Validate.ArgumentNotNull(nameof(handleVertexCompleted), handleVertexCompleted);

            var inspectQueue = new Queue<BfsNode>();
            var discoveredSet = new HashSet<TVertex>();
            var first = new BfsNode(startVertex);

            inspectQueue.Enqueue(first);
            discoveredSet.Add(first.Vertex);

            while (inspectQueue.Count > 0)
            {
                var vertexNode = inspectQueue.Dequeue();
                Assert.NotNull(vertexNode);
                Assert.Condition(discoveredSet.Contains(vertexNode.Vertex));

                foreach (var edge in GetEdges(vertexNode.Vertex))
                {
                    if (discoveredSet.Contains(edge.ToVertex))
                    {
                        continue;
                    }

                    var connectedNode = new BfsNode(edge)
                    {
                        Parent = vertexNode
                    };

                    discoveredSet.Add(edge.ToVertex);
                    inspectQueue.Enqueue(connectedNode);
                }

                if (!handleVertexCompleted(vertexNode))
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Traverses the graph using the depth-first-search starting from <paramref name="startVertex"/>.
        /// </summary>
        /// <param name="startVertex">The start vertex.</param>
        /// <param name="handleVertexVisited">The function called when a vertex is being visited.</param>
        /// <param name="handleVertexCompleted">The function called when a vertex is completed.</param>
        /// <param name="handleCycle">The function called when a cycle is identified.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="handleVertexVisited"/>, <paramref name="handleVertexCompleted"/>,
        /// <paramref name="handleCycle"/> or <paramref name="startVertex"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">The <paramref name="startVertex"/> is not part of this graph.</exception>
        public virtual void TraverseDfs([NotNull] TVertex startVertex,
            [NotNull] Predicate<IDfsNode> handleVertexVisited,
            [NotNull] Predicate<IDfsNode> handleVertexCompleted, [NotNull] Func<IDfsNode, IDfsNode, bool> handleCycle)
        {
            Validate.ArgumentNotNull(nameof(startVertex), startVertex);
            Validate.ArgumentNotNull(nameof(handleVertexVisited), handleVertexVisited);
            Validate.ArgumentNotNull(nameof(handleVertexCompleted), handleVertexCompleted);
            Validate.ArgumentNotNull(nameof(handleCycle), handleCycle);

            var discoveredSet = new Dictionary<TVertex, DfsNode>();
            var time = 0;

            TraverseDfs(new DfsNode(startVertex), ref time, discoveredSet, handleVertexVisited, handleVertexCompleted,
                handleCycle);
        }

        /// <summary>
        /// Fills the graph with one color.
        /// </summary>
        /// <param name="startVertex">The start vertex.</param>
        /// <param name="applyColor">Color to apply to each vertex.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="applyColor"/> or <paramref name="startVertex"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="startVertex"/> is not part of this graph.</exception>
        public virtual void FillWithOneColor([NotNull] TVertex startVertex, [NotNull] Action<TVertex> applyColor)
        {
            Validate.ArgumentNotNull(nameof(startVertex), startVertex);
            Validate.ArgumentNotNull(nameof(applyColor), applyColor);

            TraverseBfs(startVertex, node =>
            {
                applyColor(node.Vertex);
                return true;
            });
        }

        /// <summary>
        /// Finds the shortest path between two vertices in a graph.
        /// </summary>
        /// <param name="startVertex">The start vertex.</param>
        /// <param name="endVertex">The end vertex.</param>
        /// <returns>Returns a sequence of vertices in visitation order.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="startVertex"/> or <paramref name="endVertex"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="startVertex"/> is not part of this graph.</exception>
        [NotNull]
        public virtual TVertex[] FindShortestPath([NotNull] TVertex startVertex, [NotNull] TVertex endVertex)
        {
            Validate.ArgumentNotNull(nameof(startVertex), startVertex);
            Validate.ArgumentNotNull(nameof(endVertex), endVertex);

            IBfsNode solution = null;
            TraverseBfs(startVertex, node =>
            {
                if (!Equals(node.Vertex, endVertex))
                {
                    return true;
                }

                solution = node;
                return false;

            });

            var result = new List<TVertex>();
            while (solution != null)
            {
                result.Add(solution.Vertex);
                solution = solution.Parent;
            }

            result.Reverse();
            return result.ToArray();
        }

        /// <summary>
        /// Gets all connected components in a given undirected graph.
        /// </summary>
        /// <returns>A sequence of sub-graphs, each representing a connected component.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the graph is directed.</exception>
        [NotNull, ItemNotNull]
        public virtual IEnumerable<Graph<TVertex>> GetComponents()
        {
            var undiscoveredVertices = new HashSet<TVertex>(this);

            while (undiscoveredVertices.Count > 0)
            {
                var parentVertexToInspect = undiscoveredVertices.First();
                var verticesInThisComponent = new List<TVertex>();

                TraverseBfs(parentVertexToInspect, node =>
                {
                    undiscoveredVertices.Remove(node.Vertex);
                    verticesInThisComponent.Add(node.Vertex);

                    return true;
                });

                yield return new SubGraph<TVertex>(this, verticesInThisComponent);
            }
        }

        /// <summary>
        /// Sorts a directed acyclic graph in topological order.
        /// </summary>
        /// <returns>A sequence of vertices sorted in topological order.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the graph is undirected or contains one or more cycles.</exception>
        [NotNull]
        public virtual TVertex[] TopologicalSort()
        {
            var outAdj = new Dictionary<TVertex, ISet<TVertex>>();
            var inAdj = new Dictionary<TVertex, ISet<TVertex>>();

            foreach (var v in this)
            {
                var outSet = new HashSet<TVertex>();
                outAdj.Add(v, outSet);

                if (!inAdj.ContainsKey(v))
                {
                    inAdj.Add(v, new HashSet<TVertex>());
                }

                foreach (var z in GetEdges(v))
                {
                    outSet.Add(z.ToVertex);

                    if (!inAdj.TryGetValue(z.ToVertex, out var inSet))
                    {
                        inSet = new HashSet<TVertex>();
                        inAdj.Add(z.ToVertex, inSet);
                    }

                    inSet.Add(v);
                }
            }

            var nextQueue = new Queue<TVertex>();
            foreach (var v in inAdj.Where(k => k.Value.Count == 0))
            {
                nextQueue.Enqueue(v.Key);
            }

            var result = new List<TVertex>();
            while (nextQueue.Count > 0)
            {
                var next = nextQueue.Dequeue();
                inAdj.Remove(next);

                result.Add(next);

                foreach (var d in outAdj[next])
                {
                    inAdj[d].Remove(next);
                    if (inAdj[d].Count == 0)
                    {
                        nextQueue.Enqueue(d);
                    }
                }
            }

            if (inAdj.Count > 0)
            {
                throw new InvalidOperationException("Topological sorting not supported on cyclical graphs.");
            }

            return result.ToArray();
        }

        /// <summary>
        /// Finds all articulation vertices in the given graph.
        /// </summary>
        /// <returns>A sequence of all articulation vertices.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the graph is directed.</exception>
        [NotNull]
        public virtual IEnumerable<TVertex> FindAllArticulationVertices()
        {
            RequireUndirectedGraph();

            var verticesToCheck = new HashSet<TVertex>(this);
            var articulationVertices = new HashSet<TVertex>();

            while (verticesToCheck.Count > 0)
            {
                var eaDict = new Dictionary<TVertex, IDfsNode>();
                var parentVertexToInspect = verticesToCheck.First();
                var branchesOfRoot = 0;

                TraverseDfs(parentVertexToInspect,
                    node =>
                    {
                        if (node.Parent != null)
                        {
                            eaDict.Add(node.Vertex, node.Parent);
                        }

                        return true;
                    },
                    node =>
                    {
                        if (node.Parent == null)
                        {
                            if (branchesOfRoot > 1)
                            {
                                articulationVertices.Add(node.Vertex);
                            }
                        }
                        else if (node.Parent.Parent != null)
                        {
                            var ea = eaDict[node.Vertex];
                            if (ea == node.Parent)
                            {
                                articulationVertices.Add(node.Parent.Vertex);
                            }
                            else
                            {
                                eaDict[node.Parent.Vertex] = ea;
                            }
                        }
                        else
                        {
                            branchesOfRoot++;
                        }

                        verticesToCheck.Remove(node.Vertex);
                        return true;
                    },
                    (fromNode, toNode) =>
                    {
                        var ea = eaDict[fromNode.Vertex];
                        if (ea == null || ea.EntryTime > toNode.EntryTime)
                        {
                            eaDict[fromNode.Vertex] = toNode;
                        }

                        return true;
                    });
            }

            return articulationVertices;
        }

        /// <summary>
        /// Verifies the current graph is bipartite.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is bipartite; otherwise, <c>false</c>.
        /// </value>
        /// <exception cref="InvalidOperationException">Thrown if the graph is directed.</exception>
        public virtual bool IsBipartite
        {
            get
            {
                RequireUndirectedGraph();

                var remainingVertices = new HashSet<TVertex>(this);
                var isBipartite = true;
                while (remainingVertices.Count > 0 && isBipartite)
                {
                    var assignments = new Dictionary<TVertex, bool>();
                    TraverseDfs(remainingVertices.First(),
                        node =>
                        {
                            remainingVertices.Remove(node.Vertex);

                            if (node.Parent == null)
                            {
                                assignments.Add(node.Vertex, false);
                            }
                            else
                            {
                                assignments.Add(node.Vertex, !assignments[node.Parent.Vertex]);
                            }
                            return true;
                        },
                        node => true,
                        (fromNode, toNode) =>
                        {
                            if (assignments[fromNode.Vertex] == assignments[toNode.Vertex])
                            {
                                isBipartite = false;
                            }

                            return isBipartite;
                        });
                }

                return isBipartite;
            }
        }

        /// <summary>
        /// Describes the vertices of graph (degrees and components).
        /// </summary>
        /// <returns>A list of vertex descriptions.</returns>
        [NotNull, ItemNotNull]
        public virtual IEnumerable<VertexDescriptor<TVertex>> DescribeVertices()
        {
            var inDegrees = new Dictionary<TVertex, int>();
            var outDegrees = new Dictionary<TVertex, int>();
            var componentIndexes = new Dictionary<TVertex, int>();

            var vertices = new HashSet<TVertex>(this);

            foreach (var vertex in vertices)
            {
                var outEdgeCount = 0;
                foreach (var edge in GetEdges(vertex))
                {
                    outEdgeCount++;
                    inDegrees.AddOrUpdate(edge.ToVertex, 1, i => i + 1);
                }

                outDegrees.Add(vertex, outEdgeCount);
                inDegrees.AddOrUpdate(vertex, 0, i => i);
            }

            var componentIndex = 0;
            while (vertices.Count > 0)
            {
                TraverseBfs(vertices.First(), node =>
                {
                    vertices.Remove(node.Vertex);
                    // ReSharper disable once AccessToModifiedClosure
                    componentIndexes.Add(node.Vertex, componentIndex);
                    return true;
                });

                componentIndex++;
            }

            Assert.NotNull(vertices.Count == 0);
            Assert.NotNull(componentIndexes.Count == inDegrees.Count && componentIndexes.Count == outDegrees.Count);

            // ReSharper disable once IdentifierTypo
            foreach (var ckvp in componentIndexes)
            {
                yield return new VertexDescriptor<TVertex>(ckvp.Key, ckvp.Value, inDegrees[ckvp.Key],
                    outDegrees[ckvp.Key]);
            }
        }

        /// <summary>
        /// Finds the cheapest path in a graph between two vertices <paramref name="startVertex"/> and <paramref name="endVertex"/>
        /// </summary>
        /// <param name="startVertex">The start vertex.</param>
        /// <param name="endVertex">The end vertex.</param>
        /// <returns>A sequence of vertices that yield the shortest path. Returns an empty sequence if no path available.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="startVertex"/> is not part of teh graph.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="startVertex"/> or <paramref name="endVertex"/> is <c>null</c>.</exception>
        [NotNull]
        public IEnumerable<TVertex> FindCheapestPath([NotNull] TVertex startVertex, [NotNull] TVertex endVertex)
        {
            Validate.ArgumentNotNull(nameof(startVertex), startVertex);
            Validate.ArgumentNotNull(nameof(endVertex), endVertex);

            var comparer = Comparer<PathNode>.Create((a, b) =>
            {
                Assert.NotNull(a);
                Assert.NotNull(b);

                return a.PotentialCostToDestination.CompareTo(b.PotentialCostToDestination);
            });

            var startVertexNode = new PathNode
            {
                Vertex = startVertex,
                TotalCostFromStart = 0,
                PotentialCostToDestination =
                    SupportsPotentialWeightEvaluation ? GetPotentialWeight(startVertex, endVertex) : 0,
            };

            var discoveredVertices = new Dictionary<TVertex, PathNode> {{startVertex, startVertexNode}};
            var visitationQueue = new Heap<PathNode>(comparer) {startVertexNode};
            var foundAPath = false;
            var cheapestCostSoFar = .0;

            while (visitationQueue.Count > 0)
            {
                var vertexNode = visitationQueue.RemoveTop();
                Assert.Condition(vertexNode != null && discoveredVertices.ContainsKey(vertexNode.Vertex));

                if (Equals(vertexNode.Vertex, endVertex))
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

                foreach (var edge in GetEdges(vertexNode.Vertex))
                {
                    var costFromStartForThisPath = vertexNode.TotalCostFromStart + edge.Weight;

                    if (!discoveredVertices.TryGetValue(edge.ToVertex, out var discoveredNode))
                    {
                        discoveredNode = new PathNode
                        {
                            Vertex = edge.ToVertex,
                            Parent = vertexNode,
                            TotalCostFromStart = costFromStartForThisPath,
                            PotentialCostToDestination =
                                SupportsPotentialWeightEvaluation
                                    ? costFromStartForThisPath + GetPotentialWeight(edge.ToVertex, endVertex)
                                    : costFromStartForThisPath,
                        };

                        discoveredVertices.Add(discoveredNode.Vertex, discoveredNode);
                        visitationQueue.Add(discoveredNode);
                    }
                    else
                    {
                        if (!(costFromStartForThisPath < discoveredNode.TotalCostFromStart))
                        {
                            continue;
                        }

                        discoveredNode.TotalCostFromStart = costFromStartForThisPath;
                        discoveredNode.Parent = vertexNode;
                        discoveredNode.PotentialCostToDestination =
                            SupportsPotentialWeightEvaluation
                                ? costFromStartForThisPath + GetPotentialWeight(discoveredNode.Vertex, endVertex)
                                : costFromStartForThisPath;

                        visitationQueue.Add(discoveredNode);
                    }
                }
            }

            var result = new List<TVertex>();
            if (!foundAPath)
            {
                return result;
            }

            var node = discoveredVertices[endVertex];
            do
            {
                result.Add(node.Vertex);
                node = node.Parent;
            } while (node != null);

            result.Reverse();

            return result;
        }
    }
}