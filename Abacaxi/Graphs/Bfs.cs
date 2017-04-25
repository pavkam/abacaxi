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

    public static class Bfs
    {
        private sealed class Node<TVertex> : IBfsNode<TVertex>
        {
            public TVertex Vertex { get; set; }
            public IBfsNode<TVertex> Parent { get; set; }
        }

        public static void Apply<TVertex>(
            IGraph<TVertex> graph, 
            TVertex startVertex, 
            Predicate<IBfsNode<TVertex>> handleVertexCompleted)
        {
            Validate.ArgumentNotNull(nameof(graph), graph);
            Validate.ArgumentNotNull(nameof(handleVertexCompleted), handleVertexCompleted);

            var inspectQueue = new Queue<Node<TVertex>>();
            var discoveredSet = new HashSet<TVertex>();
            var first = new Node<TVertex>()
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

                foreach (var edge in graph.GetEdges(vertexNode.Vertex))
                {
                    if (!discoveredSet.Contains(edge.ToVertex))
                    {
                        var connectedNode = new Node<TVertex>()
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
    }
}
