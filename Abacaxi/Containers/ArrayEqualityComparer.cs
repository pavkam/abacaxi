﻿/* Copyright 2017-2019 by Alexandru Ciobanu (alex+git@ciobanu.org)
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

namespace Abacaxi.Containers
{
    using System.Collections.Generic;
    using System.Linq;
    using Internal;
    using JetBrains.Annotations;

    /// <summary>
    ///     A class that provides array equality comparison (based on the array's elements).
    /// </summary>
    /// <typeparam name="TElement">The type of the elements in the arrays.</typeparam>
    [PublicAPI]
    public sealed class ArrayEqualityComparer<TElement> : IEqualityComparer<TElement[]>
    {
        [NotNull] private readonly IEqualityComparer<TElement> _elementComparer;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ArrayEqualityComparer{TElement}" /> class.
        /// </summary>
        /// <param name="elementComparer">The element comparer.</param>
        /// <exception cref="System.ArgumentNullException">Thrown if <paramref name="elementComparer" /> is <c>null</c>.</exception>
        public ArrayEqualityComparer([NotNull] IEqualityComparer<TElement> elementComparer)
        {
            Validate.ArgumentNotNull(nameof(elementComparer), elementComparer);

            _elementComparer = elementComparer;
        }

        /// <summary>
        ///     Gets the default equality comparer for the given array type.
        /// </summary>
        /// <value>
        ///     The default equality comparer.
        /// </value>
        [NotNull]
        public static IEqualityComparer<TElement[]> Default { get; } =
            new ArrayEqualityComparer<TElement>(EqualityComparer<TElement>.Default);

        /// <summary>
        ///     Checks whether <paramref name="array1" /> and <paramref name="array2" /> are structurally equal.
        /// </summary>
        /// <param name="array1">The first array.</param>
        /// <param name="array2">The second array.</param>
        /// <returns><c>true</c> if the array contain the same elements; otherwise, <c>false</c>.</returns>
        public bool Equals([CanBeNull] TElement[] array1, [CanBeNull] TElement[] array2)
        {
            if (array1 == null ||
                array2 == null)
            {
                return array1 == array2;
            }

            if (array1.Length != array2.Length)
            {
                return false;
            }

            return !array1.Where((t, i) => !_elementComparer.Equals(t, array2[i])).Any();
        }

        /// <summary>
        ///     Calculates the hash code for a given <paramref name="array" />.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <returns>
        ///     A hash code for the array instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        public int GetHashCode(TElement[] array)
        {
            Validate.ArgumentNotNull(nameof(array), array);

            var hashCode = array.Length;
            // ReSharper disable once LoopCanBeConvertedToQuery
            // ReSharper disable once ForCanBeConvertedToForeach
            for (var i = 0; i < array.Length; ++i)
            {
                hashCode = unchecked(hashCode * 314159 + _elementComparer.GetHashCode(array[i]));
            }

            return hashCode;
        }
    }
}