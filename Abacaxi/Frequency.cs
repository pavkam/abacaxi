/* Copyright 2017-2018 by Alexandru Ciobanu (alex+git@ciobanu.org)
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
    /// Stores the number of times a certain item appears in a collection.
    /// </summary>
    /// <typeparam name="T">Te type of elements in collection.</typeparam>
    [PublicAPI]
    public struct Frequency<T>
    {
        /// <summary>
        /// Gets the item that is being counted.
        /// </summary>
        /// <value>
        /// The item.
        /// </value>
        public T Item { get; }

        /// <summary>
        /// Gets the number of appearances.
        /// </summary>
        /// <value>
        /// The count.
        /// </value>
        public int Count { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Frequency{T}"/> struct.
        /// </summary>
        /// <param name="item">The item being counted.</param>
        /// <param name="count">The number of appearances.</param>
        public Frequency(T item, int count)
        {
            Count = count;
            Item = item;
        }

        /// <summary>
        /// Determines whether the specified <see cref="object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj?.GetType() != GetType())
            {
                return false;
            }

            // ReSharper disable once PossibleNullReferenceException
            var e = (Frequency<T>) obj;
            return
                Equals(e.Item, Item) &&
                Equals(e.Count, Count);
        }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> that represents this instance.
        /// </returns>
        public override string ToString() => $"{Item} ({Count})";

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        public override int GetHashCode()
        {
            var hashCode = 17;
            hashCode = hashCode * 23 + Count.GetHashCode();
            hashCode = hashCode * 23 + Item?.GetHashCode() ?? 0;

            return hashCode;
        }
    }
}