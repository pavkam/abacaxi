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
    /// <typeparam name="TVertex">The type of graph vertices.</typeparam>
    public interface IGraph<TVertex>
    {
        /// <summary>
        /// Gets a value indicating whether this graph's edges are directed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this graph's edges are directed; otherwise, <c>false</c>.
        /// </value>
        bool IsDirected { get; }

        /// <summary>
        /// Gets the edges for a given <param name="vertex"/>.
        /// </summary>
        /// <param name="node">The vertex.</param>
        /// <returns>A sequence of edges connected to the given <param name="vertex"/></returns>
        IEnumerable<Edge<TVertex>> GetEdges(TVertex vertex);


        /// <summary>
        /// Gets all vertices in the graph.
        /// </summary>
        /// <returns>The sequence of all vertices in the graph.</returns>
        IEnumerable<TVertex> GetVertices();
    }
}
