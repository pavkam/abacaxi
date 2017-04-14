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
    /// Generic graph interface.
    /// </summary>
    /// <typeparam name="TValue">The type of the value stored in the graph nodes.</typeparam>
    /// <typeparam name="TIdentifier">The type of the values used to identify nodes in the graph.</typeparam>
    /// <typeparam name="TCost">The type of the cost of connections.</typeparam>
    public interface IGraph<TValue, TIdentifier, TCost>
    {
        /// <summary>
        /// Gets the value of the node identified by the <paramref name="nodeIdentifier"/> parameter.
        /// </summary>
        /// <param name="nodeIdentifier">The unique node identifier.</param>
        /// <returns>The value of the node.</returns>
        TValue GetValue(TIdentifier nodeIdentifier);

        /// <summary>
        /// Sets the value of the node identified by the <paramref name="nodeIdentifier"/> parameter.
        /// </summary>
        /// <param name="nodeIdentifier">The unique node identifier.</param>
        /// <param name="value">The new value of the node.</param>
        void SetValue(TIdentifier nodeIdentifier, TValue value);

        /// <summary>
        /// Returns a list of all nodes connected to the node identified by the <paramref name="nodeIdentifier"/> parameter.
        /// </summary>
        /// <param name="nodeIdentifier">The unique node identifier.</param>
        /// <returns>The list of connected nodes.</returns>
        IEnumerable<Connection<TIdentifier, TCost>> GetConnections(TIdentifier nodeIdentifier);

        /// <summary>
        /// Returns all nodes in the graph.
        /// </summary>
        /// <returns>A sequence of nodes.</returns>
        IEnumerable<TIdentifier> GetNodes();

        /// <summary>
        /// Adds up two connection costs to form a sum.
        /// </summary>
        /// <param name="a">The first cost.</param>
        /// <param name="b">The second cost.</param>
        /// <returns>Aggregated cost.</returns>
        TCost AddConnectionCosts(TCost a, TCost b);

        /// <summary>
        /// Compares two connection costs.
        /// </summary>
        /// <param name="a">The first cost.</param>
        /// <param name="b">The second cost.</param>
        /// <returns>Comparison result.</returns>
        int CompareConnectionCosts(TCost a, TCost b);

        /// <summary>
        /// Evaluates the potential connection cost between two nodes in the graph.
        /// </summary>
        /// <param name="from">The first node.</param>
        /// <param name="to">The seccond node.</param>
        /// <returns>The potential connection cost.</returns>
        TCost EvaluatePotentialConnectionCost(TIdentifier from, TIdentifier to);
    }
}
