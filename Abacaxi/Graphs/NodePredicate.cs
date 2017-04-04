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
    using System.Collections.Generic;

    /// <summary>
    /// Validates a node for a selection operation.
    /// </summary>
    /// <typeparam name="TValue">The type of the value stored by teh graph nodes.</typeparam>
    /// <typeparam name="TIdentifier">The type of the graph node identifier.</typeparam>
    /// <typeparam name="TCost">The node connection cost type.</typeparam>
    /// <param name="graph">The graph that owes the nodes.</param>
    /// <param name="nodeIdentifier">The identifier of the node to validate.</param>
    /// <returns><c>true</c> if the node is validated; <c>false</c> otherwise.</returns>
    public delegate bool NodePredicate<TValue, TIdentifier, TCost>(Graph<TValue, TIdentifier, TCost> graph, TIdentifier nodeIdentifier);
}
