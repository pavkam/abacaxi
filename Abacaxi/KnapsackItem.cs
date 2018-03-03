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
    using Internal;

    /// <summary>
    /// Class describes an item that is used in the <seealso cref="Knapsack"/> algorithms.
    /// </summary>
    /// <typeparam name="T">The type of described item.</typeparam>
    [PublicAPI]
    public sealed class KnapsackItem<T>
    {
        /// <summary>
        /// Gets the item.
        /// </summary>
        /// <value>
        /// The item.
        /// </value>
        public T Item { get; }

        /// <summary>
        /// Gets the item weight.
        /// </summary>
        /// <value>
        /// The item weight.
        /// </value>
        public int Weight { get; }

        /// <summary>
        /// Gets the item value.
        /// </summary>
        /// <value>
        /// The item value.
        /// </value>
        public double Value { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="KnapsackItem{T}"/> class.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="value">The item value.</param>
        /// <param name="weight">The item weight.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown if <paramref name="value"/> or <paramref name="weight"/> are less than or equal to zero.</exception>
        public KnapsackItem(T item, double value, int weight)
        {
            Validate.ArgumentGreaterThanZero(nameof(value), value);
            Validate.ArgumentGreaterThanZero(nameof(weight), weight);

            Item = item;
            Value = value;
            Weight = weight;
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

            var e = (KnapsackItem<T>) obj;
            return
                // ReSharper disable once PossibleNullReferenceException
                Equals(e.Item, Item) &&
                Equals(e.Value, Value) &&
                Equals(e.Weight, Weight);
        }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return $"{Item} ({Value:N2}, {Weight})";
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            var hashCode = 17;
            hashCode = hashCode * 23 + Value.GetHashCode();
            hashCode = hashCode * 23 + Weight.GetHashCode();
            hashCode = hashCode * 23 + Item?.GetHashCode() ?? 0;

            return hashCode;
        }
    }
}