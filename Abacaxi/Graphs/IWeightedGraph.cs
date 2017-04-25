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
    /// Weighted graph interface.
    /// </summary>
    /// <typeparam name="TVertex">The type of graph vertices.</typeparam>
    /// <typeparam name="TWeight">The type of edge weights.</typeparam>
    public interface IWeightedGraph<TVertex, TWeight>: IGraph<TVertex>
    {
        /// <summary>
        /// Gets the edges of a given <paramref name="vertex"/>.
        /// </summary>
        /// <param name="vertex">The vertex.</param>
        /// <returns>A sequence of edges connecting the <paramref name="vertex"/> to other vertices.</returns>
        new IEnumerable<WeightedEdge<TVertex, TWeight>> GetEdges(TVertex vertex);

        /// <summary>
        /// Gets the zero weight.
        /// </summary>
        /// <value>
        /// The zero weight.
        /// </value>
        TWeight ZeroWeight { get; }

        /// <summary>
        /// Adds two weights.
        /// </summary>
        /// <param name="left">The left weight.</param>
        /// <param name="right">The right weight.</param>
        /// <returns>The sum of two weights.</returns>
        TWeight AddWeights(TWeight left, TWeight right);

        /// <summary>
        /// Compares two weights.
        /// </summary>
        /// <param name="left">The left weight.</param>
        /// <param name="right">The right weight.</param>
        /// <returns>The comparison result.</returns>
        int CompareWeights(TWeight left, TWeight right);

        /// <summary>
        /// Gets the potential total weight connecting <paramref name="fromVertex"/> and <paramref name="toVertex"/> vertices.
        /// This is a heuristical function and can be implemented to always return <see cref="ZeroWeight"/>.
        /// </summary>
        /// <param name="fromVertex">The first vertex.</param>
        /// <param name="toVertex">The destination vertex.</param>
        /// <returns>The potential total cost.</returns>
        TWeight GetPotentialWeight(TVertex fromVertex, TVertex toVertex);
    }
}
