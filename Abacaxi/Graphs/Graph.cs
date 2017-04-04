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

    /// <summary>
    /// Generic graph class. Does not actually implement the storage of the graph nodes, but provides a common public interface
    /// that is implemented by concrete classes.
    /// </summary>
    /// <typeparam name="TValue">The type of the value stored in the graph nodes.</typeparam>
    /// <typeparam name="TIdentifier">The type of the values used to identify nodes in the graph.</typeparam>
    public abstract class Graph<TValue, TIdentifier, TCost>
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
        public abstract IEnumerable<Connection<TIdentifier, TCost>> GetConnections(TIdentifier nodeIdentifier);
    }
}
