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

namespace Abacaxi
{
    using JetBrains.Annotations;

    /// <summary>
    /// Implements a number of object-related helper methods useable across the library (and beyond!).
    /// </summary>
    [PublicAPI]
    public static class Value
    {
        /// <summary>
        /// Determines whether <paramref name="value"/> is equal to any of the given candidates.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="value">The value.</param>
        /// <param name="candidates">The candidates to check against.</param>
        /// <returns>
        ///   <c>true</c> if the value is contained in the given candidate list; otherwise, <c>false</c>.
        /// </returns>
        [ContractAnnotation("candidates:null => halt")]
        public static bool IsAnyOf<T>(this T value, [NotNull] params T[] candidates)
        {
            // ReSharper disable once LoopCanBeConvertedToQuery
            // ReSharper disable once ForCanBeConvertedToForeach
            for (var i = 0; i < candidates.Length; i++)
            {
                if (Equals(value, candidates[i]))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
