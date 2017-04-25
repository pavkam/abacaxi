using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abacaxi.Graphs
{
    public interface IDfsNode<TVertex>
    {
        TVertex Vertex { get; }
        IDfsNode<TVertex> Parent { get; }

        bool Articulation { get; set; }

        int EntryTime { get; }
        int ExitTime { get; }
    }
}
