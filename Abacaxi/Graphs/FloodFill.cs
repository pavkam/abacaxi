

namespace Abacaxi.Graphs
{
    using System;

    public static class FloodFill
    {
        public static void Apply<TVertex>(IGraph<TVertex> graph, TVertex startVertex, Action<TVertex> applyColor)
        {
            Validate.ArgumentNotNull(nameof(graph), graph);
            Validate.ArgumentNotNull(nameof(applyColor), applyColor);

            Bfs.Apply(graph, startVertex, node =>
            {
                applyColor(node.Vertex);
                return true;
            });
        }
    }
}
