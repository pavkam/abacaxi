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

using System.Diagnostics.CodeAnalysis;

namespace Abacaxi.Costs
{
    /// <summary>
    /// Class implements the <see cref="IAccountant{T}"/> interface for <c>int</c>-based costs.
    /// </summary>
    [SuppressMessage("ReSharper", "ClassWithVirtualMembersNeverInherited.Global")]
    public class IntegerAccountant : IAccountant<int>
    {
        /// <summary>
        /// Gets the zero cost.
        /// </summary>
        /// <value>
        /// The zero cost.
        /// </value>
        public int Zero => 0;

        /// <summary>
        /// Adds two costs.
        /// </summary>
        /// <param name="left">The left cost.</param>
        /// <param name="right">The right cost.</param>
        /// <returns>The aggregated cost.</returns>
        public int Add(int left, int right) => left + right;

        /// <summary>
        /// Subtracts two costs.
        /// </summary>
        /// <param name="left">The left cost.</param>
        /// <param name="right">The right cost.</param>
        /// <returns>The value of <paramref name="left"/> minus <paramref name="right"/>.</returns>
        public int Subtract(int left, int right) => left - right;

        /// <summary>
        /// Multiplies the specified cost with a given <paramref name="multiplier"/>.
        /// </summary>
        /// <param name="cost">The cost to multiply.</param>
        /// <param name="multiplier">The multiplier.</param>
        /// <returns>The result of multiplication.</returns>
        public int Multiply(int cost, int multiplier) => cost * multiplier;

        /// <summary>
        /// Compares two costs.
        /// </summary>
        /// <param name="left">The left cost.</param>
        /// <param name="right">The right cost.</param>
        /// <returns>The result of comparison of the two costs.</returns>
        public virtual int Compare(int left, int right) => left - right;
    }
}
