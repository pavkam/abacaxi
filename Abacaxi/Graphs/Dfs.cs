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
    using System.Text;
    using System.Threading.Tasks;

    public sealed class Dfs
    {
        private sealed class Node<TVertex> : IDfsNode<TVertex>
        {
            public bool Articulation { get; set; }
            public TVertex Vertex { get; set; }
            public int EntryTime { get; set; }
            public int ExitTime { get; set; }
            public IDfsNode<TVertex> Parent { get; set; }

            public bool Returning { get; set; }
            public Node<TVertex> ReachableAncestor { get; set; }
        }

        public static void Apply<TVertex>(
            IGraph<TVertex> graph, 
            TVertex startVertex, 
            Predicate<IDfsNode<TVertex>> handleVertexCompleted,
            Func<IDfsNode<TVertex>, IDfsNode<TVertex>, bool> handleCycle)
        {
            Validate.ArgumentNotNull(nameof(graph), graph);
            Validate.ArgumentNotNull(nameof(handleVertexCompleted), handleVertexCompleted);
            Validate.ArgumentNotNull(nameof(handleCycle), handleCycle);

            var inspectStack = new Stack<Node<TVertex>>();
            var discoveredSet = new Dictionary<TVertex, Node<TVertex>>();
            var time = 0;
            var first = new Node<TVertex>()
            {
                EntryTime = time++,
                Vertex = startVertex,
            };

            inspectStack.Push(first);
            discoveredSet.Add(first.Vertex, first);

            while (inspectStack.Count > 0)
            {
                var vertexNode = inspectStack.Pop();

                Debug.Assert(vertexNode != null);
                Debug.Assert(discoveredSet.ContainsKey(vertexNode.Vertex));

                if (vertexNode.Returning)
                {
                    vertexNode.ExitTime = time++;

                    if (vertexNode.ReachableAncestor == null && vertexNode.Parent != null)
                    {
                        vertexNode.Parent.Articulation = true;
                    }

                    if (!handleVertexCompleted(vertexNode))
                    {
                        return;
                    }
                }
                else
                {
                    vertexNode.Returning = true;
                    inspectStack.Push(vertexNode);

                    var edgeCount = 0;
                    foreach (var edge in graph.GetEdges(vertexNode.Vertex))
                    {
                        edgeCount++;

                        Node<TVertex> toVertexNode;
                        if (!discoveredSet.TryGetValue(edge.ToVertex, out toVertexNode))
                        {
                            toVertexNode = new Node<TVertex>()
                            {
                                EntryTime = time++,
                                Parent = vertexNode,
                                Vertex = edge.ToVertex,
                            };

                            discoveredSet.Add(edge.ToVertex, toVertexNode);
                            inspectStack.Push(toVertexNode);
                        }
                        else if (!Equals(toVertexNode.Vertex, vertexNode.Parent.Vertex))
                        {
                            if (handleCycle(vertexNode, toVertexNode))
                            {
                                return;
                            }

                            if (vertexNode.ReachableAncestor == null || toVertexNode.EntryTime < vertexNode.ReachableAncestor.EntryTime)
                            {
                                vertexNode.ReachableAncestor = toVertexNode.ReachableAncestor;
                            }
                        }
                    }

                    if (edgeCount > 1 && vertexNode.Parent == null)
                    {
                        vertexNode.Articulation = true;
                    }
                }
            }
        }
    }
}
