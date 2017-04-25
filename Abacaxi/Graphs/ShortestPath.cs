

namespace Abacaxi.Graphs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public static class ShortestPath
    {
        public static IEnumerable<TVertex> Find<TVertex>(IGraph<TVertex> graph, TVertex startVertex, TVertex endVertex)
        {
            Validate.ArgumentNotNull(nameof(graph), graph);

            IBfsNode<TVertex> solution = null;
            Bfs.Apply(graph, startVertex, node =>
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


        public static IEnumerable<Cell> FindForChessHorse(Cell startCell, Cell endCell)
        {
            const int padding = 2;

            var boardWidth = Math.Abs(endCell.X - startCell.X) + padding * 2;
            var boardHeight = Math.Abs(endCell.Y - startCell.Y) + padding * 2;

            var deltaX = Math.Min(startCell.X, endCell.X) - padding;
            var deltaY = Math.Min(startCell.Y, endCell.Y) - padding;

            var board = new ChessHorsePathGraph(boardWidth, boardHeight);
            foreach (var cell in Find(board, new Cell(startCell.X - deltaX, startCell.Y - deltaY), new Cell(endCell.X - deltaX, endCell.Y - deltaY)))
            {
                yield return new Cell(cell.X + deltaX, cell.Y + deltaY);
            }
        }
    }
}
