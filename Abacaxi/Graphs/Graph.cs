using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abacaxi.Graphs
{
    /// <summary>
    /// Generic graph class. Does not actually implement the storage of the graph nodes, but provides a common public interface
    /// that is implemented by concrete classes.
    /// </summary>
    /// <typeparam name="TValue">The type of the value stored in the graph nodes.</typeparam>
    /// <typeparam name="TIdentifier">The type of the values used to identify nodes in the graph.</typeparam>
    public abstract class Graph<TValue, TIdentifier>
    {
        /// <summary>
        /// Gets the value of the node identified by the <paramref name="nodeIdentifier"/> parameter.
        /// </summary>
        /// <param name="nodeIdentifier">The unique node identifier.</param>
        /// <returns>The value of the node.</returns>
        public abstract TValue GetNodeValue(TIdentifier nodeIdentifier);

        /// <summary>
        /// Sets the value of the node identified by the <paramref name="nodeIdentifier"/> parameter.
        /// </summary>
        /// <param name="nodeIdentifier">The unique node identifier.</param>
        /// <param name="value">The new value of the node.</param>
        public abstract void SetNodeValue(TIdentifier nodeIdentifier, TValue value);

        /// <summary>
        /// Returns a list of all nodes connected to the node identified by the <paramref name="nodeIdentifier"/> parameter.
        /// </summary>
        /// <param name="nodeIdentifier">The unique node identifier.</param>
        /// <returns>The list of connected nodes.</returns>
        public abstract IEnumerable<TIdentifier> GetNodeConnections(TIdentifier nodeIdentifier);
    }
}
