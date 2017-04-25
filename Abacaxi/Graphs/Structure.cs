

namespace Abacaxi.Graphs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public static class Structure
    {
        public static IEnumerable<IGraph<TVertex>> GetComponents<TVertex>(IGraph<TVertex> graph)
        {
            Validate.ArgumentNotNull(nameof(graph), graph);

            var undiscoveredVertices = new HashSet<TVertex>();
            foreach (var vertex in graph.GetVertices())
            {
                undiscoveredVertices.Add(vertex);
            }

            while (undiscoveredVertices.Count > 0)
            {
                var parentVertexToInspect = undiscoveredVertices.First();
                var verticesInThisComponent = new List<TVertex>();

                Bfs.Apply(graph, parentVertexToInspect, node =>
                {
                    undiscoveredVertices.Remove(node.Vertex);
                    verticesInThisComponent.Add(node.Vertex);

                    return true;
                });

                yield return new SubGraph<TVertex>(graph, verticesInThisComponent);
            }
        }
    }
}
