/* Copyright 2017-2019 by Alexandru Ciobanu (alex+git@ciobanu.org)
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

namespace Abacaxi
{
    using JetBrains.Annotations;

    /// <summary>
    ///     Defines the allowed set of edit operations used by the <seealso cref="SequenceAlgorithms.Diff{T}" /> method.
    /// </summary>
    [PublicAPI]
    public enum EditOperation
    {
        /// <summary>
        ///     Items from both sequences match at given location.
        /// </summary>
        Match = '=',

        /// <summary>
        ///     An item from a given location in the original sequence is substituted with an item in the result sequence.
        /// </summary>
        Substitute = '#',

        /// <summary>
        ///     An item is inserted into the original sequence at a given location to match the result sequence.
        /// </summary>
        Insert = '+',

        /// <summary>
        ///     An item is removed from the original sequence at a given location to match the result sequence.
        /// </summary>
        Delete = '-'
    }
}