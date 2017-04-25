using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abacaxi.Graphs
{
    public interface IBfsNode<TVertex>
    {
        TVertex Vertex { get; }
        IBfsNode<TVertex> Parent { get; }
    }
}
