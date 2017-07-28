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
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Implements a number of sequence-related helper methods useable across the library (and beyond!).
    /// </summary>
    public static class SequenceHelperMethods
    {
        /// <summary>
        /// Converts a sequence to a set.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="sequence">The sequence.</param>
        /// <param name="comparer">An equality comparer.</param>
        /// <returns>A new set containing the elements in <paramref name="sequence"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown if either of <paramref name="sequence"/> or <paramref name="comparer"/> are <c>null</c>.</exception>
        public static ISet<T> ToSet<T>(this IEnumerable<T> sequence, IEqualityComparer<T> comparer)
        {
            Validate.ArgumentNotNull(nameof(sequence), sequence);
            Validate.ArgumentNotNull(nameof(comparer), comparer);

            return new HashSet<T>(sequence, comparer);
        }

        /// <summary>
        /// Converts a sequence to a set.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="sequence">The sequence.</param>
        /// <returns>A new set containing the elements in <paramref name="sequence"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="sequence"/> is <c>null</c>.</exception>
        public static ISet<T> ToSet<T>(this IEnumerable<T> sequence)
        {
            Validate.ArgumentNotNull(nameof(sequence), sequence);

            return new HashSet<T>(sequence);
        }

        /// <summary>
        /// Interprets a given <paramref name="sequence"/> as a list. The returned list can either be the same object or a new
        /// object.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="sequence">The sequence.</param>
        /// <returns>A list representing the original sequence.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="sequence"/> is <c>null</c>.</exception>
        public static IList<T> AsList<T>(this IEnumerable<T> sequence)
        {
            Validate.ArgumentNotNull(nameof(sequence), sequence);

            if (sequence is IList<T> asList)
            {
                return asList;
            }
            if (sequence is ICollection<T> asCollection)
            {
                var result = new T[asCollection.Count];
                asCollection.CopyTo(result, 0);

                return result;
            }
            if (sequence is IEnumerable<char> asCharSeq)
            {
                if (asCharSeq is string asString)
                {
                    return asString.ToCharArray() as IList<T>;
                }
            }

            return sequence.ToArray();
        }

        /// <summary>
        /// Evaluates the appearace frequency for each item in a <paramref name="sequence"/>.
        /// </summary>
        /// <typeparam name="T">The type of items in the sequence </typeparam>
        /// <param name="sequence">The sequence.</param>
        /// <param name="comparer">The comparer.</param>
        /// <returns>A new dictionary where each key is an item form the <paramref name="sequence"/> and associated values are the frequencies.</returns>
        /// <exception cref="ArgumentNullException">Thrown if either of <paramref name="sequence"/> or <paramref name="comparer"/> are <c>null</c>.</exception>
        public static IDictionary<T, int> GetItemFrequencies<T>(this IEnumerable<T> sequence, IEqualityComparer<T> comparer)
        {
            Validate.ArgumentNotNull(nameof(sequence), sequence);
            Validate.ArgumentNotNull(nameof(comparer), comparer);

            var result = new Dictionary<T, int>(comparer);
            foreach (var item in sequence)
            {
                if (!result.TryGetValue(item, out int frequency))
                {
                    result.Add(item, 1);
                }
                else
                {
                    result[item] = frequency + 1;
                }
            }

            return result;
        }

        /// <summary>
        /// Adds a new key/valuer pair or updates an existing one.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="dict">The dictionary.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="updateFunc">The value update function.</param>
        /// <returns><c>true</c> if the a new key/value pair was added; otherwise <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">Thrown if either of <paramref name="dict"/> or <paramref name="updateFunc"/> are <c>null</c>.</exception>
        public static bool AddOrUpdate<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, TValue value,
            Func<TValue, TValue> updateFunc)
        {
            Validate.ArgumentNotNull(nameof(dict), dict);
            Validate.ArgumentNotNull(nameof(updateFunc), updateFunc);

            if (dict.TryGetValue(key, out TValue existing))
            {
                dict[key] = updateFunc(existing);
                return false;
            }

            dict.Add(key, value);
            return true;
        }

        /// <summary>
        /// Appends the specified <paramref name="item1"/> to an array <paramref name="array"/>.
        /// </summary>
        /// <typeparam name="T">The type of elements in <paramref name="array"/>.</typeparam>
        /// <param name="array">The array.</param>
        /// <param name="item1">The item to append to array.</param>
        /// <returns>A new array consisting <paramref name="array"/> and appended <paramref name="item1"/>.</returns>
        public static T[] Append<T>(this T[] array, T item1)
        {
            var length = array?.Length + 1 ?? 1;
            Array.Resize(ref array, length);
            array[length - 1] = item1;

            return array;
        }

        /// <summary>
        /// Appends the specified items to an array <paramref name="array"/>.
        /// </summary>
        /// <typeparam name="T">The type of elements in <paramref name="array"/>.</typeparam>
        /// <param name="array">The array.</param>
        /// <param name="item1">The first item to append to array.</param>
        /// <param name="item2">The second item to append to array.</param>
        /// <returns>A new array consisting <paramref name="array"/> and appended items.</returns>
        public static T[] Append<T>(this T[] array, T item1, T item2)
        {
            var length = array?.Length + 2 ?? 2;
            Array.Resize(ref array, length);
            array[length - 2] = item1;
            array[length - 1] = item2;

            return array;
        }

        /// <summary>
        /// Appends the specified items to an array <paramref name="array"/>.
        /// </summary>
        /// <typeparam name="T">The type of elements in <paramref name="array"/>.</typeparam>
        /// <param name="array">The array.</param>
        /// <param name="item1">The first item to append to array.</param>
        /// <param name="item2">The second item to append to array.</param>
        /// <param name="item3">The third item to append to array.</param>
        /// <returns>A new array consisting <paramref name="array"/> and appended items.</returns>
        public static T[] Append<T>(this T[] array, T item1, T item2, T item3)
        {
            var length = array?.Length + 3 ?? 3;
            Array.Resize(ref array, length);
            array[length - 3] = item1;
            array[length - 2] = item2;
            array[length - 1] = item3;

            return array;
        }

        /// <summary>
        /// Appends the specified items to an array <paramref name="array"/>.
        /// </summary>
        /// <typeparam name="T">The type of elements in <paramref name="array"/>.</typeparam>
        /// <param name="array">The array.</param>
        /// <param name="item1">The first item to append to array.</param>
        /// <param name="item2">The second item to append to array.</param>
        /// <param name="item3">The third item to append to array.</param>
        /// <param name="item4">The fourth item to append to array.</param>
        /// <returns>A new array consisting <paramref name="array"/> and appended items.</returns>
        public static T[] Append<T>(this T[] array, T item1, T item2, T item3, T item4)
        {
            var length = array?.Length + 4 ?? 4;
            Array.Resize(ref array, length);
            array[length - 4] = item1;
            array[length - 3] = item2;
            array[length - 2] = item3;
            array[length - 1] = item4;

            return array;
        }

        /// <summary>
        /// Appends the specified items to an array <paramref name="array"/>.
        /// </summary>
        /// <typeparam name="T">The type of elements in <paramref name="array"/>.</typeparam>
        /// <param name="array">The array.</param>
        /// <param name="item1">The first item to append to array.</param>
        /// <param name="item2">The second item to append to array.</param>
        /// <param name="item3">The third item to append to array.</param>
        /// <param name="item4">The fourth item to append to array.</param>
        /// <param name="item5">The fifth item to append to array.</param>
        /// <returns>A new array consisting <paramref name="array"/> and appended items.</returns>
        public static T[] Append<T>(this T[] array, T item1, T item2, T item3, T item4, T item5)
        {
            var length = array?.Length + 5 ?? 5;
            Array.Resize(ref array, length);
            array[length - 5] = item1;
            array[length - 4] = item2;
            array[length - 3] = item3;
            array[length - 2] = item4;
            array[length - 1] = item5;

            return array;
        }

        /// <summary>
        /// Appends the specified <paramref name="items"/> to an array <paramref name="array"/>.
        /// </summary>
        /// <typeparam name="T">The type of elements in <paramref name="array"/>.</typeparam>
        /// <param name="array">The array.</param>
        /// <param name="items">The items to append to the array.</param>
        /// <returns>A new array consisting <paramref name="array"/> and appended items.</returns>
        public static T[] Append<T>(this T[] array, params T[] items)
        {
            var il = array?.Length ?? 0;
            var al = items.Length;

            Array.Resize(ref array, il + al);
            for (var i = 0; i < al; i++)
            {
                array[il + i] = items[i];
            }

            return array;
        }

        /// <summary>
        /// Interprets a list as an index-value pair sequence.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="list">The list to convert to a key-value sequence.</param>
        /// <returns>The resulting sequence.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="list"/> is <c>null</c>.</exception>
        public static IEnumerable<KeyValuePair<int, T>> AsIndexedEnumerable<T>(this IList<T> list)
        {
            Validate.ArgumentNotNull(nameof(list), list);

            for (var i = 0; i < list.Count; i++)
            {
                yield return new KeyValuePair<int, T>(i, list[i]);
            }
        }
    }
}
