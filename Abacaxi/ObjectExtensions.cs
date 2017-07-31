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
    using System.Collections.Generic;
    using System.Reflection;
    using Internal;
    using JetBrains.Annotations;

    /// <summary>
    /// Implements a number of object-related helper methods useable across the library (and beyond!).
    /// </summary>
    [PublicAPI]
    public static class ObjectExtensions
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

        /// <summary>
        /// Inspects a given object and extract a set of key-value pairs. Each pair is a field/property/method and its associated value. The inspection
        /// only considers public, non-static, non-generic and parameter-less members.
        /// </summary>
        /// <typeparam name="T">The type of object that is inspected.</typeparam>
        /// <param name="value">The object.</param>
        /// <param name="flags">The inspection flags. The default is <see cref="InspectionFlags.IncludeProperties"/>.</param>
        /// <returns>A readonly dictionary containing all object's inspected members.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown if <paramref name="value"/> is <c>null</c>.</exception>
        [NotNull]
        public static IReadOnlyDictionary<string, object> Inspect<T>([NotNull] this T value, InspectionFlags flags = InspectionFlags.IncludeProperties)
        {
            Validate.ArgumentNotNull(nameof(value), value);

            var memberDict = new Dictionary<string, object>();
            if (flags.HasFlag(InspectionFlags.IncludeFields))
            {
                foreach (var f in typeof(T).GetRuntimeFields())
                {
                    if (!f.IsSpecialName && f.IsPublic && !f.IsStatic)
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

            if (flags.HasFlag(InspectionFlags.IncludeMethods))
            {
                foreach (var m in typeof(T).GetRuntimeMethods())
                {
                    if (!m.IsSpecialName && !m.IsGenericMethodDefinition && m.GetParameters().Length == 0 &&
                        m.IsPublic && !m.IsConstructor && !m.IsAbstract && !m.IsStatic)
                    {
                        memberDict[m.Name] = m.Invoke(value, null);
                    }
                }
            }

            return memberDict;
        }
    }
}
