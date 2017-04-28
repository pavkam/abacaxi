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
    using System.Collections;
    using System.Diagnostics;
    using System.Linq;
    using System.Collections.Generic;

    /// <summary>
    /// Generic graph class. This class serves as an abstract base for all concrete implementations.
    /// </summary>
    /// <typeparam name="TVertex">The type of graph vertices.</typeparam>
    public abstract class Graph<TVertex> : IEnumerable<TVertex>
    {
        private sealed class BfsNode : IBfsNode
        {
            public TVertex Vertex { get; set; }
            public IBfsNode Parent { get; set; }
        }

        private sealed class DfsNode : IDfsNode
        {
            public bool Articulation { get; set; }
            public TVertex Vertex { get; set; }
            public int EntryTime { get; set; }
            public int ExitTime { get; set; }
            public IDfsNode Parent { get; set; }
            public DfsNode ReachableAncestor { get; set; }
        }

        private bool TraverseDfs(
            DfsNode vertexNode,
            ref int time,
            IDictionary<TVertex, DfsNode> visitedNodes,
            Predicate<IDfsNode> handleVertexCompleted,
            Func<IDfsNode, IDfsNode, bool> handleCycle)
        {
            Debug.Assert(vertexNode != null);
            Debug.Assert(visitedNodes != null);
            Debug.Assert(handleVertexCompleted != null);
            Debug.Assert(handleCycle != null);

            visitedNodes.Add(vertexNode.Vertex, vertexNode);
            vertexNode.EntryTime = time++;

            var breakRequested = false;
            var children = 0;
            foreach (var edge in GetEdges(vertexNode.Vertex))
            {
                children++;

                if (breakRequested)
                {
                    break;
                }

                if (!visitedNodes.TryGetValue(edge.ToVertex, out DfsNode visitedNode))
                {
                    visitedNode = new DfsNode()
                    {
                        Parent = vertexNode,
                        Vertex = edge.ToVertex,
                        ReachableAncestor = vertexNode,
                    };

                    breakRequested =
                        !TraverseDfs(visitedNode, ref time, visitedNodes, handleVertexCompleted, handleCycle);

                    if (vertexNode.ReachableAncestor != null && 
                        vertexNode.ReachableAncestor.EntryTime > visitedNode.ReachableAncestor.EntryTime)
                    {
                        vertexNode.ReachableAncestor = visitedNode.ReachableAncestor;
                    }
                }
                else if (visitedNode != vertexNode.Parent && visitedNode.EntryTime <= vertexNode.EntryTime)
                {
                    if (vertexNode.ReachableAncestor != null &&
                        vertexNode.ReachableAncestor.EntryTime > visitedNode.EntryTime)
                    {
                        vertexNode.ReachableAncestor = visitedNode;
                    }

                    if (!handleCycle(vertexNode, visitedNode))
                    {
                        breakRequested = true;
                    }
                }
            }

            vertexNode.ExitTime = time++;
            if (vertexNode.Parent == null)
            {
                vertexNode.Articulation = children > 1;
            }
            else if (vertexNode.ReachableAncestor == vertexNode.Parent)
            {
                ((DfsNode)vertexNode.Parent).Articulation = true;
            }

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
            TVertex Vertex { get; }

            /// <summary>
            /// Gets the parent BFS node in the traversal tree.
            /// </summary>
            /// <value>
            /// The parent node.
            /// </value>
            IBfsNode Parent { get; }
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
            TVertex Vertex { get; }

            /// <summary>
            /// Gets the parent DFS node in the traversal tree.
            /// </summary>
            /// <value>
            /// The parent node.
            /// </value>
            IDfsNode Parent { get; }

            /// <summary>
            /// Gets a value indicating whether the <see cref="Vertex"/> is an articulation vertex.
            /// </summary>
            /// <value>
            ///   <c>true</c> if articulation vertex; otherwise, <c>false</c>.
            /// </value>
            bool Articulation { get; }

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
        /// Gets a value indicating whether this graph's edges are directed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this graph's edges are directed; otherwise, <c>false</c>.
        /// </value>
        public abstract bool IsDirected { get; }

        /// <summary>
        /// Gets the edges for a given <param name="vertex"/>.
        /// </summary>
        /// <param name="vertex">The vertex to get the edges for.</param>
        /// <returns>A sequence of edges connected to the given <param name="vertex"/></returns>
        /// <exception cref="InvalidOperationException">The <paramref name="vertex"/> is not part of this graph.</exception>
        public abstract IEnumerable<Edge<TVertex>> GetEdges(TVertex vertex);

        /// <summary>
        /// Returns an enumerator that iterates all vertices in the graph.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.
        /// </returns>
        public abstract IEnumerator<TVertex> GetEnumerator();

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Traverses the graph using the breadth-first-search starting from <paramref name="startVertex"/>.
        /// </summary>
        /// <param name="startVertex">The start vertex.</param>
        /// <param name="handleVertexCompleted">The function called when a vertex is completed.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="handleVertexCompleted"/> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">The <paramref name="startVertex"/> is not part of this graph.</exception>
        public void TraverseBfs(TVertex startVertex, Predicate<IBfsNode> handleVertexCompleted)
        {
            Validate.ArgumentNotNull(nameof(handleVertexCompleted), handleVertexCompleted);

            var inspectQueue = new Queue<BfsNode>();
            var discoveredSet = new HashSet<TVertex>();
            var first = new BfsNode()
            {
                Parent = null,
                Vertex = startVertex,
            };

            inspectQueue.Enqueue(first);
            discoveredSet.Add(first.Vertex);

            while (inspectQueue.Count > 0)
            {
                var vertexNode = inspectQueue.Dequeue();
                Debug.Assert(vertexNode != null);
                Debug.Assert(discoveredSet.Contains(vertexNode.Vertex));

                foreach (var edge in GetEdges(vertexNode.Vertex))
                {
                    if (!discoveredSet.Contains(edge.ToVertex))
                    {
                        var connectedNode = new BfsNode()
                        {
                            Parent = vertexNode,
                            Vertex = edge.ToVertex,
                        };

                        discoveredSet.Add(edge.ToVertex);
                        inspectQueue.Enqueue(connectedNode);
                    }
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
        /// <param name="handleVertexCompleted">The function called when a vertex is completed.</param>
        /// <param name="handleCycle">The function called when a cycle is identified.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="handleVertexCompleted"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="handleCycle"/> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">The <paramref name="startVertex"/> is not part of this graph.</exception>
        public void TraverseDfs(TVertex startVertex, Predicate<IDfsNode> handleVertexCompleted, Func<IDfsNode, IDfsNode, bool> handleCycle)
        {
            Validate.ArgumentNotNull(nameof(handleVertexCompleted), handleVertexCompleted);
            Validate.ArgumentNotNull(nameof(handleCycle), handleCycle);

            var discoveredSet = new Dictionary<TVertex, DfsNode>();
            var time = 0;

            TraverseDfs(new DfsNode()
            {
                Vertex = startVertex,
            }, ref time, discoveredSet, handleVertexCompleted, handleCycle);
        }

        /// <summary>
        /// Fills the graph with one color.
        /// </summary>
        /// <param name="startVertex">The start vertex.</param>
        /// <param name="applyColor">Color to apply to each vertex.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="applyColor"/> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">Thrown if <paramref name="startVertex"/> is not part of this graph.</exception>
        public void FillWithOneColor(TVertex startVertex, Action<TVertex> applyColor)
        {
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
        /// <exception cref="InvalidOperationException">Thrown if <paramref name="startVertex"/> is not part of this graph.</exception>
        public IEnumerable<TVertex> FindShortestPath(TVertex startVertex, TVertex endVertex)
        {
            IBfsNode solution = null;
            TraverseBfs(startVertex, node =>
            {
                if (Equals(node.Vertex, endVertex))
                {
                    solution = node;
                    return false;
                }

                return true;
            });

            var result = new List<TVertex>();
            while (solution != null)
            {
                result.Add(solution.Vertex);
                solution = solution.Parent;
            }

            for (var i = result.Count - 1; i >= 0; i--)
            {
                yield return result[i];
            }
        }

        public IEnumerable<Graph<TVertex>> GetComponents()
        {
            var undiscoveredVertices = new HashSet<TVertex>();
            foreach (var vertex in this)
            {
                undiscoveredVertices.Add(vertex);
            }

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
    }
}