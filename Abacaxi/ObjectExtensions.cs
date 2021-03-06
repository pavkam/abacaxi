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

namespace Abacaxi
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Reflection;
    using Internal;
    using JetBrains.Annotations;

    /// <summary>
    ///     Implements a number of object-related helper methods usable across the library (and beyond!).
    /// </summary>
    [PublicAPI]
    public static class ObjectExtensions
    {
        [CanBeNull]
        private static object[] Deconstruct([NotNull] Type type, [NotNull] object @object)
        {
            Assert.NotNull(type);
            Assert.NotNull(@object);

            if (type.IsConstructedGenericType &&
                type.GetGenericTypeDefinition() == typeof(KeyValuePair<,>))
            {
                return new[]
                {
                    type.GetProperty("Key")?.GetValue(@object),
                    type.GetProperty("Value")?.GetValue(@object)
                };
            }

            var intfType = type.GetInterface("ITuple");
            if (intfType != null && type.IsConstructedGenericType)
            {
                var itemProperty = intfType.GetProperty("Item");
                if (itemProperty == null || 
                   itemProperty.GetIndexParameters().Length != 1 || 
                   itemProperty.GetIndexParameters()[0].ParameterType != typeof(int))
                {
                    return null;
                }

                var lengthProperty = intfType.GetProperty("Length");
                if (lengthProperty == null || lengthProperty.PropertyType != typeof(int))
                {
                    return null;
                }

                var count = (int)lengthProperty.GetValue(@object);
                if (count == 0)
                {
                    return null;
                }

                var result = new object[count];
                var argArray = new object[1];
                for (var i = 0; i < count; i++)
                {
                    argArray[0] = i;
                    result[i] = itemProperty.GetValue(@object, argArray);
                }

                return result;
            }

            return null;
        }

        private static bool TryConvert(
            [CanBeNull] object @object, [NotNull] Type toType, [NotNull] IFormatProvider formatProvider, 
            out object result)
        {
            Assert.NotNull(formatProvider);
            Assert.NotNull(toType);

            /* nulls */
            // ReSharper disable once AssignNullToNotNullAttribute
            result = null;
            if (@object == null)
            {
                return !toType.IsValueType || Nullable.GetUnderlyingType(toType) != null;
            }

            /* Quick and dirty */
            var objType = @object.GetType();

            if (objType == toType)
            {
                result = @object;
                return true;
            }

            var underlyingType = Nullable.GetUnderlyingType(toType);
            if (underlyingType != null)
            {
                toType = underlyingType;
            }

            var toTypeInfo = toType.GetTypeInfo();
            var objTypeInfo = objType.GetTypeInfo();

            /* Sub-class/interface */
            if (toTypeInfo.IsAssignableFrom(objTypeInfo))
            {
                result = @object;
                return true;
            }

            /* Enums */
            if (toTypeInfo.IsEnum)
            {
                try
                {
                    result = Enum.Parse(toType, @object.ToString());
                    return true;
                }
                catch
                {
                    return false;
                }
            }

            if (toType == typeof(string))
            {
                if (@object is IFormattable formattable)
                {
                    result = formattable.ToString(null, formatProvider);
                }
                else
                {
                    result = @object.ToString();
                }

                return true;
            }

            /* Tuple conversion */
            var inputTuple = Deconstruct(objType, @object);
            if (inputTuple != null)
            {
                var genArgs = toType.GetGenericArguments();
                if (inputTuple.Length == genArgs.Length)
                {
                    var failed = false;
                    for (var i = 0; i < genArgs.Length; i++)
                    {
                        if (!TryConvert(inputTuple[i], genArgs[i], formatProvider, out inputTuple[i]))
                        {
                            failed = true;
                            break;
                        }
                    }

                    if (!failed)
                    {
                        result = Activator.CreateInstance(toType, inputTuple);
                        return true;
                    }
                }
            }

            /* Last resort */
            try
            {
                result = Convert.ChangeType(@object, toType, formatProvider);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        ///     Determines whether <paramref name="value" /> is equal to any of the given candidates.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="value">The value.</param>
        /// <param name="candidates">The candidates to check against.</param>
        /// <returns>
        ///     <c>true</c> if the value is contained in the given candidate list; otherwise, <c>false</c>.
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

        /// <summary>
        ///     Inspects a given object and extract a set of key-value pairs. Each pair is a field/property/method and its
        ///     associated value. The inspection
        ///     only considers public, non-static, non-generic and parameter-less members.
        /// </summary>
        /// <typeparam name="T">The type of object that is inspected.</typeparam>
        /// <param name="value">The object.</param>
        /// <param name="flags">The inspection flags. The default is <see cref="InspectionFlags.IncludeProperties" />.</param>
        /// <returns>A readonly dictionary containing all object's inspected members.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value" /> is <c>null</c>.</exception>
        [NotNull]
        public static IReadOnlyDictionary<string, object> Inspect<T>([NotNull] this T value,
            InspectionFlags flags = InspectionFlags.IncludeProperties)
        {
            Validate.ArgumentNotNull(nameof(value), value);

            var memberDict = new Dictionary<string, object>();
            if (flags.HasFlag(InspectionFlags.IncludeFields))
            {
                foreach (var f in typeof(T).GetRuntimeFields())
                {
                    if (!f.IsSpecialName &&
                        f.IsPublic &&
                        !f.IsStatic)
                    {
                        memberDict[f.Name] = f.GetValue(value);
                    }
                }
            }

            if (flags.HasFlag(InspectionFlags.IncludeProperties))
            {
                foreach (var p in typeof(T).GetRuntimeProperties())
                {
                    if (!p.IsSpecialName &&
                        p.GetIndexParameters().Length == 0 &&
                        p.CanRead &&
                        p.GetMethod.IsPublic &&
                        !p.GetMethod.IsStatic)
                    {
                        memberDict[p.Name] = p.GetValue(value);
                    }
                }
            }

            if (!flags.HasFlag(InspectionFlags.IncludeMethods))
            {
                return memberDict;
            }

            foreach (var m in typeof(T).GetRuntimeMethods())
            {
                if (!m.IsSpecialName &&
                    !m.IsGenericMethodDefinition &&
                    m.GetParameters().Length == 0 &&
                    m.IsPublic &&
                    !m.IsConstructor &&
                    !m.IsAbstract &&
                    !m.IsStatic)
                {
                    memberDict[m.Name] = m.Invoke(value, null);
                }
            }

            return memberDict;
        }

        /// <summary>
        ///     Tries the cast or convert a given <paramref name="object" /> to a value of type <typeparamref name="T" />.
        /// </summary>
        /// <typeparam name="T">The type to convert to.</typeparam>
        /// <param name="object">The object to convert.</param>
        /// <param name="formatProvider">The format provider.</param>
        /// <param name="result">The resulting converted value.</param>
        /// <returns><c>true</c> if the conversion succeeded; otherwise, <c>false</c>.</returns>
        public static bool TryConvert<T>([CanBeNull] this object @object, [NotNull] IFormatProvider formatProvider,
            [CanBeNull] out T result)
        {
            Validate.ArgumentNotNull(nameof(formatProvider), formatProvider);

            var r = TryConvert(@object, typeof(T), formatProvider, out var d);
            if (r)
            {
                result = (T) d;
                return true;
            }

            result = default(T);
            return false;
        }

        /// <summary>
        ///     Tries the cast or convert a given <paramref name="object" /> to a value of type <typeparamref name="T" />.
        ///     This method uses <seealso cref="CultureInfo.InvariantCulture" /> for the conversion.
        /// </summary>
        /// <typeparam name="T">The type to convert to.</typeparam>
        /// <param name="object">The object to convert.</param>
        /// <param name="result">The resulting converted value.</param>
        /// <returns><c>true</c> if the conversion succeeded; otherwise, <c>false</c>.</returns>
        public static bool TryConvert<T>([CanBeNull] this object @object, out T result)
        {
            return TryConvert(@object, CultureInfo.InvariantCulture, out result);
        }

        /// <summary>
        ///     Converts a given <paramref name="object" /> to a given type <typeparamref name="T" />.
        /// </summary>
        /// <typeparam name="T">The type to convert to.</typeparam>
        /// <param name="object">The value to convert.</param>
        /// <param name="formatProvider">The format provider.</param>
        /// <param name="validateFunc">The validation function.</param>
        /// <returns>
        ///     The converted value.
        /// </returns>
        /// <exception cref="FormatException">Thrown if the conversion failed.</exception>
        /// <exception cref="InvalidOperationException">Thrown if the validation failed.</exception>
        /// <exception cref="ArgumentNullException">
        ///     Thrown if either <paramref name="formatProvider" /> or
        ///     <paramref name="validateFunc" /> is <c>null</c>.
        /// </exception>
        [CanBeNull]
        public static T As<T>([CanBeNull] this object @object,
            [NotNull] IFormatProvider formatProvider, [NotNull] Func<T, bool> validateFunc)
        {
            Validate.ArgumentNotNull(nameof(validateFunc), validateFunc);

            if (!TryConvert(@object, formatProvider, out T result))
            {
                throw new FormatException(
                    $"Failed to convert object \"{@object}\" to a value of type {typeof(T).Name}.");
            }

            if (!validateFunc(result))
            {
                throw new InvalidOperationException(
                    $"Failed to validate object \"{@object}\" (converted to type {typeof(T).Name} as {result}).");
            }

            return result;
        }

        /// <summary>
        ///     Converts a given <paramref name="object" /> to a given type <typeparamref name="T" />.
        /// </summary>
        /// <typeparam name="T">The type to convert to.</typeparam>
        /// <param name="object">The value to convert.</param>
        /// <param name="formatProvider">The format provider.</param>
        /// <returns>
        ///     The converted value.
        /// </returns>
        /// <exception cref="FormatException">Thrown if the conversion failed.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="formatProvider" /> is <c>null</c>.</exception>
        [CanBeNull]
        public static T As<T>([CanBeNull] this object @object, [NotNull] IFormatProvider formatProvider)
        {
            return As<T>(@object, formatProvider, v => true);
        }

        /// <summary>
        ///     Converts a given <paramref name="object" /> to a given type <typeparamref name="T" />.
        ///     This method uses <seealso cref="CultureInfo.InvariantCulture" /> for the conversion.
        /// </summary>
        /// <typeparam name="T">The type to convert to.</typeparam>
        /// <param name="object">The value to convert.</param>
        /// <param name="validateFunc">The validation function.</param>
        /// <returns>
        ///     The converted value.
        /// </returns>
        /// <exception cref="FormatException">Thrown if the conversion failed.</exception>
        /// <exception cref="InvalidOperationException">Thrown if the validation failed.</exception>
        [CanBeNull]
        public static T As<T>([CanBeNull] this object @object, [NotNull] Func<T, bool> validateFunc)
        {
            return As(@object, CultureInfo.InvariantCulture, validateFunc);
        }

        /// <summary>
        ///     Converts a given <paramref name="object" /> to a given type <typeparamref name="T" />.
        ///     This method uses <seealso cref="CultureInfo.InvariantCulture" /> for the conversion.
        /// </summary>
        /// <typeparam name="T">The type to convert to.</typeparam>
        /// <param name="object">The value to convert.</param>
        /// <returns>
        ///     The converted value.
        /// </returns>
        /// <exception cref="FormatException">Thrown if the conversion failed.</exception>
        [CanBeNull]
        public static T As<T>([CanBeNull] this object @object)
        {
            return As<T>(@object, CultureInfo.InvariantCulture, v => true);
        }

        /// <summary>
        ///     Maps the input <paramref name="object" /> to a resulting output. Mostly useful for
        ///     continuations of anonymous types.
        /// </summary>
        /// <typeparam name="T">The type of input object.</typeparam>
        /// <typeparam name="TMapped">The type of the resulting object.</typeparam>
        /// <param name="object">The object.</param>
        /// <param name="mapFunc">The mapping function.</param>
        /// <returns>The result of the mapping.</returns>
        [CanBeNull]
        public static TMapped Map<T, TMapped>(this T @object, [NotNull] Func<T, TMapped> mapFunc)
        {
            Validate.ArgumentNotNull(nameof(mapFunc), mapFunc);
            return mapFunc(@object);
        }
    }
}