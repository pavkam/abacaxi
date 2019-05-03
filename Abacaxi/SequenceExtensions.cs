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
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Text;
    using Internal;
    using JetBrains.Annotations;

    /// <summary>
    ///     Class provides a large number of simple operations to use on sequences.
    /// </summary>
    [PublicAPI]
    public static class SequenceExtensions
    {
        /// <summary>
        ///     Converts a sequence to a set.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="sequence">The sequence.</param>
        /// <param name="comparer">An equality comparer.</param>
        /// <returns>A new set containing the elements in <paramref name="sequence" />.</returns>
        /// <exception cref="ArgumentNullException">
        ///     Thrown if either of <paramref name="sequence" /> or
        ///     <paramref name="comparer" /> are <c>null</c>.
        /// </exception>
        [NotNull]
        public static ISet<T> ToSet<T>(
            [NotNull] this IEnumerable<T> sequence,
            [NotNull] IEqualityComparer<T> comparer)
        {
            Validate.ArgumentNotNull(nameof(sequence), sequence);
            Validate.ArgumentNotNull(nameof(comparer), comparer);

            return new HashSet<T>(sequence, comparer);
        }

        /// <summary>
        ///     Converts a sequence to a set.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="sequence">The sequence.</param>
        /// <returns>A new set containing the elements in <paramref name="sequence" />.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="sequence" /> is <c>null</c>.</exception>
        [NotNull]
        public static ISet<T> ToSet<T>([NotNull] this IEnumerable<T> sequence)
        {
            Validate.ArgumentNotNull(nameof(sequence), sequence);

            return new HashSet<T>(sequence);
        }

        /// <summary>
        ///     Converts a sequence to a set using a selector.
        /// </summary>
        /// <typeparam name="TSource">The type of items in the source sequence.</typeparam>
        /// <typeparam name="TResult">The type of elements in the resulting set.</typeparam>
        /// <param name="sequence">The sequence.</param>
        /// <param name="selector">The selector.</param>
        /// <returns>
        ///     A new set containing the values selected from elements in <paramref name="sequence" />.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     Thrown if <paramref name="sequence" /> or <paramref name="selector" /> is
        ///     <c>null</c>.
        /// </exception>
        [NotNull]
        public static ISet<TResult> ToSet<TSource, TResult>([NotNull] this IEnumerable<TSource> sequence,
            [NotNull] Func<TSource, TResult> selector)
        {
            Validate.ArgumentNotNull(nameof(sequence), sequence);
            Validate.ArgumentNotNull(nameof(selector), selector);

            var set = new HashSet<TResult>();
            foreach (var item in sequence)
            {
                set.Add(selector(item));
            }

            return set;
        }

        /// <summary>
        ///     Interprets a given <paramref name="sequence" /> as a list. The returned list can either be the same object or a new
        ///     object.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="sequence">The sequence.</param>
        /// <returns>A list representing the original sequence.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="sequence" /> is <c>null</c>.</exception>
        [NotNull]
        public static IList<T> AsList<T>([NotNull] this IEnumerable<T> sequence)
        {
            Validate.ArgumentNotNull(nameof(sequence), sequence);

            switch (sequence)
            {
                case IList<T> asList:
                    return asList;
                case ICollection<T> asCollection:
                    var result = new T[asCollection.Count];
                    asCollection.CopyTo(result, 0);

                    return result;
            }

            return sequence.ToArray();
        }

        /// <summary>
        ///     Adds a new key/valuer pair or updates an existing one.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="dict">The dictionary.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="updateFunc">The value update function.</param>
        /// <returns><c>true</c> if the a new key/value pair was added; otherwise <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">
        ///     Thrown if either of <paramref name="dict" /> or <paramref name="updateFunc" />
        ///     are <c>null</c>.
        /// </exception>
        public static bool AddOrUpdate<TKey, TValue>(
            [NotNull] this IDictionary<TKey, TValue> dict,
            [NotNull] TKey key, TValue value,
            [NotNull] Func<TValue, TValue> updateFunc)
        {
            Validate.ArgumentNotNull(nameof(dict), dict);
            Validate.ArgumentNotNull(nameof(updateFunc), updateFunc);

            if (dict.TryGetValue(key, out var existing))
            {
                dict[key] = updateFunc(existing);
                return false;
            }

            dict.Add(key, value);
            return true;
        }

        /// <summary>
        ///     Appends the specified <paramref name="item1" /> to an array <paramref name="array" />.
        /// </summary>
        /// <typeparam name="T">The type of elements in <paramref name="array" />.</typeparam>
        /// <param name="array">The array.</param>
        /// <param name="item1">The item to append to array.</param>
        /// <returns>A new array consisting <paramref name="array" /> and appended <paramref name="item1" />.</returns>
        [NotNull]
        public static T[] Append<T>([CanBeNull] this T[] array, T item1)
        {
            var length = array?.Length + 1 ?? 1;
            Array.Resize(ref array, length);
            array[length - 1] = item1;

            return array;
        }

        /// <summary>
        ///     Appends the specified items to an array <paramref name="array" />.
        /// </summary>
        /// <typeparam name="T">The type of elements in <paramref name="array" />.</typeparam>
        /// <param name="array">The array.</param>
        /// <param name="item1">The first item to append to array.</param>
        /// <param name="item2">The second item to append to array.</param>
        /// <returns>A new array consisting <paramref name="array" /> and appended items.</returns>
        [NotNull]
        public static T[] Append<T>([CanBeNull] this T[] array, T item1, T item2)
        {
            var length = array?.Length + 2 ?? 2;
            Array.Resize(ref array, length);
            array[length - 2] = item1;
            array[length - 1] = item2;

            return array;
        }

        /// <summary>
        ///     Appends the specified items to an array <paramref name="array" />.
        /// </summary>
        /// <typeparam name="T">The type of elements in <paramref name="array" />.</typeparam>
        /// <param name="array">The array.</param>
        /// <param name="item1">The first item to append to array.</param>
        /// <param name="item2">The second item to append to array.</param>
        /// <param name="item3">The third item to append to array.</param>
        /// <returns>A new array consisting <paramref name="array" /> and appended items.</returns>
        [NotNull]
        public static T[] Append<T>([CanBeNull] this T[] array, T item1, T item2, T item3)
        {
            var length = array?.Length + 3 ?? 3;
            Array.Resize(ref array, length);
            array[length - 3] = item1;
            array[length - 2] = item2;
            array[length - 1] = item3;

            return array;
        }

        /// <summary>
        ///     Appends the specified items to an array <paramref name="array" />.
        /// </summary>
        /// <typeparam name="T">The type of elements in <paramref name="array" />.</typeparam>
        /// <param name="array">The array.</param>
        /// <param name="item1">The first item to append to array.</param>
        /// <param name="item2">The second item to append to array.</param>
        /// <param name="item3">The third item to append to array.</param>
        /// <param name="item4">The fourth item to append to array.</param>
        /// <returns>A new array consisting <paramref name="array" /> and appended items.</returns>
        [NotNull]
        public static T[] Append<T>([CanBeNull] this T[] array, T item1, T item2, T item3, T item4)
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
        ///     Appends the specified items to an array <paramref name="array" />.
        /// </summary>
        /// <typeparam name="T">The type of elements in <paramref name="array" />.</typeparam>
        /// <param name="array">The array.</param>
        /// <param name="item1">The first item to append to array.</param>
        /// <param name="item2">The second item to append to array.</param>
        /// <param name="item3">The third item to append to array.</param>
        /// <param name="item4">The fourth item to append to array.</param>
        /// <param name="item5">The fifth item to append to array.</param>
        /// <returns>A new array consisting <paramref name="array" /> and appended items.</returns>
        [NotNull]
        public static T[] Append<T>([CanBeNull] this T[] array, T item1, T item2, T item3, T item4, T item5)
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
        ///     Appends the specified <paramref name="items" /> to an array <paramref name="array" />.
        /// </summary>
        /// <typeparam name="T">The type of elements in <paramref name="array" />.</typeparam>
        /// <param name="array">The array.</param>
        /// <param name="items">The items to append to the array.</param>
        /// <returns>A new array consisting <paramref name="array" /> and appended items.</returns>
        [NotNull]
        public static T[] Append<T>([CanBeNull] this T[] array, [NotNull] params T[] items)
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
        ///     Converts a given sequence to a list by applying a <paramref name="selector" /> to each element of the
        ///     <paramref name="sequence" />.
        /// </summary>
        /// <typeparam name="T">/The type of elements in the sequence.</typeparam>
        /// <typeparam name="TResult">The type of the resulting elements.</typeparam>
        /// <param name="sequence">The input sequence.</param>
        /// <param name="selector">The selector function.</param>
        /// <returns>A new list which contains the selected values.</returns>
        /// <exception cref="ArgumentNullException">
        ///     Thrown if <paramref name="sequence" /> or <paramref name="sequence" /> are
        ///     <c>null</c>.
        /// </exception>
        [NotNull]
        public static List<TResult> ToList<T, TResult>([NotNull] this IEnumerable<T> sequence,
            [NotNull] Func<T, TResult> selector)
        {
            Validate.ArgumentNotNull(nameof(sequence), sequence);
            Validate.ArgumentNotNull(nameof(selector), selector);

            List<TResult> result;
            if (sequence is IList<T> list)
            {
                result = new List<TResult>(list.Count);
                // ReSharper disable once LoopCanBeConvertedToQuery
                // ReSharper disable once ForCanBeConvertedToForeach
                for (var i = 0; i < list.Count; i++)
                {
                    result.Add(selector(list[i]));
                }
            }
            else
            {
                result = new List<TResult>();
                // ReSharper disable once LoopCanBeConvertedToQuery
                foreach (var item in sequence)
                {
                    result.Add(selector(item));
                }
            }

            return result;
        }

        /// <summary>
        ///     Converts a given sequence to a list by applying a <paramref name="selector" /> to each element of the
        ///     <paramref name="sequence" />.
        /// </summary>
        /// <typeparam name="T">/The type of elements in the sequence.</typeparam>
        /// <typeparam name="TResult">The type of the resulting elements.</typeparam>
        /// <param name="sequence">The input sequence.</param>
        /// <param name="selector">The selector function.</param>
        /// <remarks>
        ///     The second argument to <paramref name="selector" /> is the index of the element in the original
        ///     <paramref name="sequence" />.
        /// </remarks>
        /// <returns>A new list which contains the selected values.</returns>
        /// <exception cref="ArgumentNullException">
        ///     Thrown if <paramref name="sequence" /> or <paramref name="sequence" /> are
        ///     <c>null</c>.
        /// </exception>
        [NotNull]
        public static List<TResult> ToList<T, TResult>([NotNull] this IEnumerable<T> sequence,
            [NotNull] Func<T, int, TResult> selector)
        {
            Validate.ArgumentNotNull(nameof(sequence), sequence);
            Validate.ArgumentNotNull(nameof(selector), selector);

            List<TResult> result;
            if (sequence is IList<T> list)
            {
                result = new List<TResult>(list.Count);
                // ReSharper disable once LoopCanBeConvertedToQuery
                // ReSharper disable once ForCanBeConvertedToForeach
                for (var i = 0; i < list.Count; i++)
                {
                    result.Add(selector(list[i], i));
                }
            }
            else
            {
                result = new List<TResult>();
                var index = 0;

                // ReSharper disable once LoopCanBeConvertedToQuery
                foreach (var item in sequence)
                {
                    result.Add(selector(item, index++));
                }
            }

            return result;
        }

        /// <summary>
        ///     Copies a number of elements from the given <paramref name="sequence" /> into a new array.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="sequence">The sequence to copy from.</param>
        /// <param name="startIndex">The start index to copy from.</param>
        /// <param name="count">The count of elements to copy.</param>
        /// <returns>A new array containing the copied elements.</returns>
        /// <exception cref="ArgumentNullException">
        ///     Thrown if either <paramref name="sequence" /> is  <c>null</c>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Thrown when the combination of <paramref name="startIndex" /> and
        ///     <paramref name="count" /> is out of bounds.
        /// </exception>
        [NotNull]
        public static T[] Copy<T>([NotNull] this IList<T> sequence, int startIndex, int count)
        {
            Validate.CollectionArgumentsInBounds(nameof(sequence), sequence, startIndex, count);

            var result = new T[count];
            switch (sequence)
            {
                case T[] arraySequence:
                    Array.Copy(arraySequence, startIndex, result, 0, count);
                    break;
                case List<T> listSequence:
                    listSequence.CopyTo(startIndex, result, 0, count);
                    break;
                default:
                {
                    for (var i = 0; i < count; i++)
                    {
                        result[i] = sequence[i + startIndex];
                    }

                    break;
                }
            }

            return result;
        }

        [NotNull, ItemNotNull]
        private static IEnumerable<T[]> PartitionIterate<T>([NotNull] this IEnumerable<T> sequence, int size)
        {
            Assert.NotNull(sequence);
            Assert.Condition(size > 0);

            var temp = new T[size];
            var count = 0;
            foreach (var item in sequence)
            {
                temp[count++] = item;
                if (count != size)
                {
                    continue;
                }

                count = 0;
                var res = new T[size];
                Array.Copy(temp, res, size);

                yield return res;
            }

            if (count <= 0)
            {
                yield break;
            }

            var rem = new T[count];
            Array.Copy(temp, rem, count);

            yield return rem;
        }

        /// <summary>
        ///     Partitions a specified <paramref name="sequence" /> into chunks of given <paramref name="size" />.
        /// </summary>
        /// <typeparam name="T">The type of elements in the input sequence.</typeparam>
        /// <param name="sequence">The sequence to partition.</param>
        /// <param name="size">The size of each partition.</param>
        /// <returns>
        ///     A sequence of partitioned items. Each partition is of the specified <paramref name="size" /> (or less, if no
        ///     elements are left).
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="sequence" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="size" /> is less than one.</exception>
        [NotNull, ItemNotNull]
        public static IEnumerable<T[]> Partition<T>([NotNull] this IEnumerable<T> sequence, int size)
        {
            Validate.ArgumentNotNull(nameof(sequence), sequence);
            Validate.ArgumentGreaterThanZero(nameof(size), size);

            return PartitionIterate(sequence, size);
        }

        /// <summary>
        ///     Returns either the given <paramref name="sequence" /> or an empty one if <paramref name="sequence" /> is
        ///     <c>null</c>.
        /// </summary>
        /// <typeparam name="T">The type of elements in the given sequence.</typeparam>
        /// <param name="sequence">The sequence.</param>
        /// <returns>The original sequence or an empty one.</returns>
        [NotNull]
        public static IEnumerable<T> EmptyIfNull<T>([CanBeNull] this IEnumerable<T> sequence)
        {
            return sequence ?? Enumerable.Empty<T>();
        }

        /// <summary>
        ///     Determines whether the given <paramref name="sequence" /> is null or empty.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="sequence">The sequence.</param>
        /// <returns>
        ///     <c>true</c> if the sequence is null or empty; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNullOrEmpty<T>([CanBeNull] this IEnumerable<T> sequence)
        {
            return sequence?.Any() != true;
        }

        /// <summary>
        ///     Returns a <see cref="string" /> that represents this sequence of elements.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <typeparam name="TResult">The type of the result of the selector.</typeparam>
        /// <param name="sequence">The sequence.</param>
        /// <param name="selector">The result selector.</param>
        /// <param name="separator">The separator used between selected items.</param>
        /// <returns>
        ///     A <see cref="string" /> that contains all the elements of the <paramref name="sequence" />.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     Thrown if <paramref name="sequence" /> or <paramref name="selector" /> or
        ///     <paramref name="separator" /> are <c>null</c>.
        /// </exception>
        [NotNull]
        public static string ToString<T, TResult>(
            [NotNull] this IEnumerable<T> sequence,
            [NotNull] Func<T, TResult> selector,
            [NotNull] string separator)
        {
            Validate.ArgumentNotNull(nameof(sequence), sequence);
            Validate.ArgumentNotNull(nameof(selector), selector);
            Validate.ArgumentNotNull(nameof(separator), separator);

            var sb = new StringBuilder();
            foreach (var item in sequence)
            {
                var selected = selector(item);
                if (sb.Length > 0)
                {
                    sb.Append(separator);
                }

                sb.Append(selected);
            }

            return sb.ToString();
        }

        /// <summary>
        ///     Returns a <see cref="string" /> that represents this sequence of elements.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="sequence">The sequence.</param>
        /// <param name="separator">The separator used between selected items.</param>
        /// <returns>
        ///     A <see cref="string" /> that contains all the elements of the <paramref name="sequence" />.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     Thrown if <paramref name="sequence" /> or <paramref name="separator" /> are
        ///     <c>null</c>.
        /// </exception>
        [NotNull]
        public static string ToString<T>(
            [NotNull] this IEnumerable<T> sequence,
            [NotNull] string separator)
        {
            Validate.ArgumentNotNull(nameof(sequence), sequence);
            Validate.ArgumentNotNull(nameof(separator), separator);

            var sb = new StringBuilder();
            foreach (var item in sequence)
            {
                if (sb.Length > 0)
                {
                    sb.Append(separator);
                }

                sb.Append(item);
            }

            return sb.ToString();
        }

        /// <summary>
        ///     Finds the object that has a given minimum <typeparamref name="TKey" />.
        /// </summary>
        /// <typeparam name="T">The type of elements in the <paramref name="sequence" />.</typeparam>
        /// <typeparam name="TKey">The type of the key selected by <paramref name="selector" />.</typeparam>
        /// <param name="sequence">The input sequence.</param>
        /// <param name="selector">The selector that return the key to compare.</param>
        /// <param name="comparer">The comparer used to compare the keys.</param>
        /// <returns>The item that has the minimum key.</returns>
        /// <exception cref="ArgumentNullException">
        ///     Thrown if <paramref name="sequence" /> or <paramref name="selector" /> or
        ///     <paramref name="comparer" /> are <c>null</c>.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     Thrown if the <paramref name="sequence" /> is empty and
        ///     <typeparamref name="T" /> is a value type.
        /// </exception>
        [CanBeNull, SuppressMessage("ReSharper", "CompareNonConstrainedGenericWithNull")]
        public static T Min<T, TKey>(
            [NotNull] this IEnumerable<T> sequence,
            [NotNull] Func<T, TKey> selector,
            [NotNull] IComparer<TKey> comparer)
        {
            Validate.ArgumentNotNull(nameof(sequence), sequence);
            Validate.ArgumentNotNull(nameof(selector), selector);
            Validate.ArgumentNotNull(nameof(comparer), comparer);

            var item = default(T);
            using (var enumerator = sequence.GetEnumerator())
            {
                TKey min;
                if (enumerator.MoveNext())
                {
                    item = enumerator.Current;
                    min = selector(item);
                }
                else
                {
                    if (item == null)
                    {
                        return default;
                    }

                    throw new InvalidOperationException($"The {nameof(sequence)} does not contain any elements.");
                }

                while (enumerator.MoveNext())
                {
                    var nextItem = enumerator.Current;
                    var nextMin = selector(nextItem);

                    if (nextMin == null ||
                        min != null && comparer.Compare(min, nextMin) <= 0)
                    {
                        continue;
                    }

                    min = nextMin;
                    item = nextItem;
                }
            }

            return item;
        }

        /// <summary>
        ///     Finds the object that has a given minimum <typeparamref name="TKey" />.
        /// </summary>
        /// <typeparam name="T">The type of elements in the <paramref name="sequence" />.</typeparam>
        /// <typeparam name="TKey">The type of the key selected by <paramref name="selector" />.</typeparam>
        /// <param name="sequence">The input sequence.</param>
        /// <param name="selector">The selector that return the key to compare.</param>
        /// <remarks>The default comparer is used to compare values of type <typeparamref name="TKey" />.</remarks>
        /// <returns>The item that has the minimum key.</returns>
        /// <exception cref="ArgumentNullException">
        ///     Thrown if <paramref name="sequence" /> or <paramref name="selector" /> are
        ///     <c>null</c>.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     Thrown if the <paramref name="sequence" /> is empty and
        ///     <typeparamref name="T" /> is a value type.
        /// </exception>
        [CanBeNull]
        public static T Min<T, TKey>(
            [NotNull] this IEnumerable<T> sequence,
            [NotNull] Func<T, TKey> selector)
        {
            return Min(sequence, selector, Comparer<TKey>.Default);
        }

        /// <summary>
        ///     Finds the object that has a given maximum <typeparamref name="TKey" />.
        /// </summary>
        /// <typeparam name="T">The type of elements in the <paramref name="sequence" />.</typeparam>
        /// <typeparam name="TKey">The type of the key selected by <paramref name="selector" />.</typeparam>
        /// <param name="sequence">The input sequence.</param>
        /// <param name="selector">The selector that return the key to compare.</param>
        /// <param name="comparer">The comparer used to compare the keys.</param>
        /// <returns>The item that has the maximum key.</returns>
        /// <exception cref="ArgumentNullException">
        ///     Thrown if <paramref name="sequence" /> or <paramref name="selector" /> or
        ///     <paramref name="comparer" /> are <c>null</c>.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     Thrown if the <paramref name="sequence" /> is empty and
        ///     <typeparamref name="T" /> is a value type.
        /// </exception>
        [CanBeNull, SuppressMessage("ReSharper", "CompareNonConstrainedGenericWithNull")]
        public static T Max<T, TKey>(
            [NotNull] this IEnumerable<T> sequence,
            [NotNull] Func<T, TKey> selector,
            [NotNull] IComparer<TKey> comparer)
        {
            Validate.ArgumentNotNull(nameof(sequence), sequence);
            Validate.ArgumentNotNull(nameof(selector), selector);
            Validate.ArgumentNotNull(nameof(comparer), comparer);

            var item = default(T);
            using (var enumerator = sequence.GetEnumerator())
            {
                TKey min;
                if (enumerator.MoveNext())
                {
                    item = enumerator.Current;
                    min = selector(item);
                }
                else
                {
                    if (item == null)
                    {
                        return default;
                    }

                    throw new InvalidOperationException($"The {nameof(sequence)} does not contain any elements.");
                }

                while (enumerator.MoveNext())
                {
                    var nextItem = enumerator.Current;
                    var nextMin = selector(nextItem);

                    if (nextMin == null ||
                        min != null && comparer.Compare(min, nextMin) >= 0)
                    {
                        continue;
                    }

                    min = nextMin;
                    item = nextItem;
                }
            }

            return item;
        }

        /// <summary>
        ///     Finds the object that has a given maximum <typeparamref name="TKey" />.
        /// </summary>
        /// <typeparam name="T">The type of elements in the <paramref name="sequence" />.</typeparam>
        /// <typeparam name="TKey">The type of the key selected by <paramref name="selector" />.</typeparam>
        /// <param name="sequence">The input sequence.</param>
        /// <param name="selector">The selector that return the key to compare.</param>
        /// <remarks>The default comparer is used to compare values of type <typeparamref name="TKey" />.</remarks>
        /// <returns>The item that has the maximum key.</returns>
        /// <exception cref="ArgumentNullException">
        ///     Thrown if <paramref name="sequence" /> or <paramref name="selector" /> are
        ///     <c>null</c>.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     Thrown if the <paramref name="sequence" /> is empty and
        ///     <typeparamref name="T" /> is a value type.
        /// </exception>
        [CanBeNull]
        public static T Max<T, TKey>(
            [NotNull] this IEnumerable<T> sequence,
            [NotNull] Func<T, TKey> selector)
        {
            return Max(sequence, selector, Comparer<TKey>.Default);
        }

        /// <summary>
        ///     Obtains a dedicated view into a segment of a given list. The returned list is a wrapper object that
        ///     acts as an intermediary to the original one. All operations on the intermediary list will be applied to the
        ///     original one.
        /// </summary>
        /// <typeparam name="T">The type of items in the sequence.</typeparam>
        /// <param name="sequence">The sequence.</param>
        /// <param name="startIndex">The start index for the segment.</param>
        /// <param name="length">The length of the segment.</param>
        /// <returns>A new list that wraps the given segment.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="sequence" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Thrown when the combination of <paramref name="startIndex" /> and
        ///     <paramref name="length" /> is out of bounds.
        /// </exception>
        [NotNull]
        public static IList<T> Segment<T>([NotNull] this IList<T> sequence, int startIndex, int length)
        {
            Validate.CollectionArgumentsInBounds(nameof(sequence), sequence, startIndex, length);
            return new ListViewWrapper<T>(sequence, startIndex, length);
        }

        /// <summary>
        ///     Determines whether all adjacent elements are in a valid "neighboring" relation.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="sequence">The sequence to check.</param>
        /// <param name="validAdjacencyFunc">The predicate that ise used to validate each two adjacent elements.</param>
        /// <returns>
        ///     <c>true</c> if all adjacent elements in the sequence conform to the given predicate; <c>false</c> otherwise.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     Thrown if <paramref name="sequence" /> or
        ///     <paramref name="validAdjacencyFunc" /> are <c>null</c>.
        /// </exception>
        public static bool IsValidAdjacency<T>(
            [NotNull] this IEnumerable<T> sequence,
            [NotNull] Func<T, T, bool> validAdjacencyFunc)
        {
            Validate.ArgumentNotNull(nameof(sequence), sequence);
            Validate.ArgumentNotNull(nameof(validAdjacencyFunc), validAdjacencyFunc);

            using (var enumerator = sequence.GetEnumerator())
            {
                if (!enumerator.MoveNext())
                {
                    return true;
                }

                var prev = enumerator.Current;
                while (enumerator.MoveNext())
                {
                    var current = enumerator.Current;

                    var comp = validAdjacencyFunc(prev, current);
                    if (!comp)
                    {
                        return false;
                    }

                    prev = current;
                }
            }

            return true;
        }

        /// <summary>
        ///     Checks whether a given sequence is ordered (ascending).
        /// </summary>
        /// <typeparam name="T">The type of elements in the <paramref name="sequence" />.</typeparam>
        /// <param name="sequence">The input sequence.</param>
        /// <param name="comparer">The comparer used to compare the keys.</param>
        /// <returns>
        ///     <c>true</c> if all elements in <paramref name="sequence" /> are greater or equal than their predecessors.
        ///     <c>false</c> otherwise.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     Thrown if <paramref name="sequence" /> or
        ///     <paramref name="comparer" /> are <c>null</c>.
        /// </exception>
        public static bool IsOrdered<T>(
            [NotNull] this IEnumerable<T> sequence,
            [NotNull] IComparer<T> comparer)
        {
            Validate.ArgumentNotNull(nameof(comparer), comparer);

            return IsValidAdjacency(sequence, (l, r) => comparer.Compare(l, r) <= 0);
        }

        /// <summary>
        ///     Checks whether a given sequence is ordered (ascending) using the default comparer.
        /// </summary>
        /// <typeparam name="T">The type of elements in the <paramref name="sequence" />.</typeparam>
        /// <param name="sequence">The input sequence.</param>
        /// <returns>
        ///     <c>true</c> if all elements in <paramref name="sequence" /> are greater or equal than their predecessors.
        ///     <c>false</c> otherwise.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     Thrown if <paramref name="sequence" /> is <c>null</c>.
        /// </exception>
        public static bool IsOrdered<T>(
            [NotNull] this IEnumerable<T> sequence)
        {
            Validate.ArgumentNotNull(nameof(sequence), sequence);

            return IsOrdered(sequence, Comparer<T>.Default);
        }

        /// <summary>
        ///     Checks whether a given sequence is strictly ordered (ascending).
        /// </summary>
        /// <typeparam name="T">The type of elements in the <paramref name="sequence" />.</typeparam>
        /// <param name="sequence">The input sequence.</param>
        /// <param name="comparer">The comparer used to compare the keys.</param>
        /// <returns>
        ///     <c>true</c> if all elements in <paramref name="sequence" /> are strictly greater than their predecessors.
        ///     <c>false</c> otherwise.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     Thrown if <paramref name="sequence" /> or
        ///     <paramref name="comparer" /> are <c>null</c>.
        /// </exception>
        public static bool IsStrictlyOrdered<T>(
            [NotNull] this IEnumerable<T> sequence,
            [NotNull] IComparer<T> comparer)
        {
            Validate.ArgumentNotNull(nameof(comparer), comparer);

            return IsValidAdjacency(sequence, (l, r) => comparer.Compare(l, r) < 0);
        }

        /// <summary>
        ///     Checks whether a given sequence is strictly ordered (ascending) using the default comparer.
        /// </summary>
        /// <typeparam name="T">The type of elements in the <paramref name="sequence" />.</typeparam>
        /// <param name="sequence">The input sequence.</param>
        /// <returns>
        ///     <c>true</c> if all elements in <paramref name="sequence" /> are strictly greater than their predecessors.
        ///     <c>false</c> otherwise.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     Thrown if <paramref name="sequence" /> is <c>null</c>.
        /// </exception>
        public static bool IsStrictlyOrdered<T>(
            [NotNull] this IEnumerable<T> sequence)
        {
            Validate.ArgumentNotNull(nameof(sequence), sequence);

            return IsStrictlyOrdered(sequence, Comparer<T>.Default);
        }

        /// <summary>
        ///     Checks whether a given sequence is ordered (descending).
        /// </summary>
        /// <typeparam name="T">The type of elements in the <paramref name="sequence" />.</typeparam>
        /// <param name="sequence">The input sequence.</param>
        /// <param name="comparer">The comparer used to compare the keys.</param>
        /// <returns>
        ///     <c>true</c> if all elements in <paramref name="sequence" /> are less than or equal to their predecessors.
        ///     <c>false</c> otherwise.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     Thrown if <paramref name="sequence" /> or
        ///     <paramref name="comparer" /> are <c>null</c>.
        /// </exception>
        public static bool IsOrderedDescending<T>(
            [NotNull] this IEnumerable<T> sequence,
            [NotNull] IComparer<T> comparer)
        {
            Validate.ArgumentNotNull(nameof(comparer), comparer);

            return IsValidAdjacency(sequence, (l, r) => comparer.Compare(l, r) >= 0);
        }

        /// <summary>
        ///     Checks whether a given sequence is ordered (descending) using the default comparer.
        /// </summary>
        /// <typeparam name="T">The type of elements in the <paramref name="sequence" />.</typeparam>
        /// <param name="sequence">The input sequence.</param>
        /// <returns>
        ///     <c>true</c> if all elements in <paramref name="sequence" /> are less than or equal to their predecessors.
        ///     <c>false</c> otherwise.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     Thrown if <paramref name="sequence" /> is <c>null</c>.
        /// </exception>
        public static bool IsOrderedDescending<T>(
            [NotNull] this IEnumerable<T> sequence)
        {
            Validate.ArgumentNotNull(nameof(sequence), sequence);

            return IsOrderedDescending(sequence, Comparer<T>.Default);
        }

        /// <summary>
        ///     Checks whether a given sequence is strictly ordered (descending).
        /// </summary>
        /// <typeparam name="T">The type of elements in the <paramref name="sequence" />.</typeparam>
        /// <param name="sequence">The input sequence.</param>
        /// <param name="comparer">The comparer used to compare the keys.</param>
        /// <returns>
        ///     <c>true</c> if all elements in <paramref name="sequence" /> are strictly smaller than their predecessors.
        ///     <c>false</c> otherwise.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     Thrown if <paramref name="sequence" /> or
        ///     <paramref name="comparer" /> are <c>null</c>.
        /// </exception>
        public static bool IsStrictlyOrderedDescending<T>(
            [NotNull] this IEnumerable<T> sequence,
            [NotNull] IComparer<T> comparer)
        {
            Validate.ArgumentNotNull(nameof(comparer), comparer);

            return IsValidAdjacency(sequence, (l, r) => comparer.Compare(l, r) > 0);
        }

        /// <summary>
        ///     Checks whether a given sequence is strictly ordered (descending) using the default comparer.
        /// </summary>
        /// <typeparam name="T">The type of elements in the <paramref name="sequence" />.</typeparam>
        /// <param name="sequence">The input sequence.</param>
        /// <returns>
        ///     <c>true</c> if all elements in <paramref name="sequence" /> are strictly smaller than their predecessors.
        ///     <c>false</c> otherwise.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     Thrown if <paramref name="sequence" /> is <c>null</c>.
        /// </exception>
        public static bool IsStrictlyOrderedDescending<T>(
            [NotNull] this IEnumerable<T> sequence)
        {
            Validate.ArgumentNotNull(nameof(sequence), sequence);

            return IsStrictlyOrderedDescending(sequence, Comparer<T>.Default);
        }

        /// <summary>
        ///     Filters a sequence of nullable items and select the values of those items.
        /// </summary>
        /// <typeparam name="T">The type of values in the <paramref name="sequence" />.</typeparam>
        /// <param name="sequence">The input sequence.</param>
        /// <returns>
        ///     A new sequence that contains only the values of nullable items that had a value.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     Thrown if <paramref name="sequence" /> is <c>null</c>.
        /// </exception>
        public static IEnumerable<T> SelectValues<T>(
            [NotNull] this IEnumerable<T?> sequence) where T : struct
        {
            Validate.ArgumentNotNull(nameof(sequence), sequence);

            return sequence
                .Where(p => p.HasValue)
                .Select(s => s.Value);
        }

        /// <summary>
        ///     Separates the items in <paramref name="sequence" /> into two arrays based on a predicate.
        /// </summary>
        /// <typeparam name="T">The type of values in the <paramref name="sequence" />.</typeparam>
        /// <param name="sequence">The input sequence.</param>
        /// <param name="predicate">The separation predicate.</param>
        /// <returns>
        ///     A tuple that contains the array of matching items and the array of the others.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     Thrown if <paramref name="sequence" /> or <paramref name="predicate" /> are <c>null</c>.
        /// </exception>
        public static (T[], T[]) Separate<T>(
            [NotNull] this IEnumerable<T> sequence, [NotNull] Func<T, bool> predicate)
        {
            Validate.ArgumentNotNull(nameof(sequence), sequence);
            Validate.ArgumentNotNull(nameof(predicate), predicate);

            var matching = new List<T>();
            var notMatching = new List<T>();

            foreach (var item in sequence)
            {
                if (predicate(item))
                {
                    matching.Add(item);
                }
                else
                {
                    notMatching.Add(item);
                }
            }

            return (matching.ToArray(), notMatching.ToArray());
        }

        /// <summary>
        ///     Splits the tuples in a <paramref name="sequence" /> into two arrays.
        /// </summary>
        /// <typeparam name="T1">The type of first item of the tuple in <paramref name="sequence" />.</typeparam>
        /// <typeparam name="T2">The type of second item of the tuple in <paramref name="sequence" />.</typeparam>
        /// <param name="sequence">The input sequence.</param>
        /// <returns>
        ///     A tuple that contains the two arrays containing the separated tuples.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     Thrown if <paramref name="sequence" /> is <c>null</c>.
        /// </exception>
        public static (T1[], T2[]) Unzip<T1, T2>([NotNull] this IEnumerable<(T1, T2)> sequence)
        {
            Validate.ArgumentNotNull(nameof(sequence), sequence);

            var seq1 = new List<T1>();
            var seq2 = new List<T2>();
            
            foreach (var (item1, item2) in sequence)
            {
                seq1.Add(item1);
                seq2.Add(item2);
            }

            return (seq1.ToArray(), seq2.ToArray());
        }

        /// <summary>
        ///     Splits the tuples in a <paramref name="sequence" /> into three arrays.
        /// </summary>
        /// <typeparam name="T1">The type of first item of the tuple in <paramref name="sequence" />.</typeparam>
        /// <typeparam name="T2">The type of second item of the tuple in <paramref name="sequence" />.</typeparam>
        /// <typeparam name="T3">The type of third item of the tuple in <paramref name="sequence" />.</typeparam>
        /// <param name="sequence">The input sequence.</param>
        /// <returns>
        ///     A tuple that contains the three arrays containing the separated tuples.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     Thrown if <paramref name="sequence" /> is <c>null</c>.
        /// </exception>
        public static (T1[], T2[], T3[]) Unzip<T1, T2, T3>([NotNull] this IEnumerable<(T1, T2, T3)> sequence)
        {
            Validate.ArgumentNotNull(nameof(sequence), sequence);

            var seq1 = new List<T1>();
            var seq2 = new List<T2>();
            var seq3 = new List<T3>();
                
            foreach (var (item1, item2, item3) in sequence)
            {
                seq1.Add(item1);
                seq2.Add(item2);
                seq3.Add(item3);
            }

            return (seq1.ToArray(), seq2.ToArray(), seq3.ToArray());
        }

        /// <summary>
        ///     Splits the key-value-pair (as tuple) in a <paramref name="sequence" /> into two arrays.
        /// </summary>
        /// <typeparam name="T1">The type of first item of the tuple in <paramref name="sequence" />.</typeparam>
        /// <typeparam name="T2">The type of second item of the tuple in <paramref name="sequence" />.</typeparam>
        /// <param name="sequence">The input sequence.</param>
        /// <returns>
        ///     A tuple that contains the two arrays containing the separated tuples.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     Thrown if <paramref name="sequence" /> is <c>null</c>.
        /// </exception>
        public static (T1[], T2[]) Unzip<T1, T2>([NotNull] this IEnumerable<KeyValuePair<T1, T2>> sequence)
        {
            Validate.ArgumentNotNull(nameof(sequence), sequence);

            var seq1 = new List<T1>();
            var seq2 = new List<T2>();

            foreach (var kvp in sequence)
            {
                seq1.Add(kvp.Key);
                seq2.Add(kvp.Value);
            }

            return (seq1.ToArray(), seq2.ToArray());
        }
    }
}