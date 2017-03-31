using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abacaxi.Graphs
{
    /// <summary>
    /// Generix "tolled" graph. This is a sub-class on the <see cref="Graph{TValue, TIdentifier}"/> class. This class introduces
    /// costs associated with each node-to-node connection.
    /// </summary>
    /// <typeparam name="TValue">The type of the value stored in the graph nodes.</typeparam>
    /// <typeparam name="TIdentifier">The type of the values used to identify nodes in the graph.</typeparam>
    /// <typeparam name="TCost">The cost type.</typeparam>
    public abstract class TolledGraph<TValue, TCost, TIdentifier>: Graph<TValue, TIdentifier>
    {
        /// <summary>
        /// Returns a list of all nodes connected to the node identified by the <paramref name="nodeIdentifier"/> parameter.
        /// Each connection is also associated with a cost.
        /// </summary>
        /// <param name="nodeIdentifier">The unique node identifier.</param>
        /// <returns>The list of connected nodes and the associated connection cost.</returns>
        public abstract IEnumerable<KeyValuePair<TIdentifier, TCost>> GetNodeConnectionsAndCosts(TIdentifier nodeIdentifier);
    }
}
