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

namespace Abacaxi.Internal
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.CompilerServices;
    using JetBrains.Annotations;

    internal static class Assert
    {
        [Conditional("DEBUG"),
         ContractAnnotation("value:notnull => halt"),
         SuppressMessage("ReSharper", "ParameterOnlyUsedForPreconditionCheck.Global")]
        public static void Null(
            [CanBeNull] object value,
            [CanBeNull, CallerMemberName] string callerMemberName = null,
            [CallerLineNumber] int callerLineNumber = 0)
        {
            if (value != null)
            {
                throw new InvalidOperationException(
                    $"Assertion failed. Value must be null at method: {callerMemberName}, line: {callerLineNumber}.");
            }
        }

        [Conditional("DEBUG"),
         ContractAnnotation("value:null => halt"),
         SuppressMessage("ReSharper", "ParameterOnlyUsedForPreconditionCheck.Global")]
        public static void NotNull(
            [CanBeNull] object value,
            [CanBeNull, CallerMemberName] string callerMemberName = null,
            [CallerLineNumber] int callerLineNumber = 0)
        {
            if (value == null)
            {
                throw new InvalidOperationException(
                    $"Assertion failed. Value cannot be null at method: {callerMemberName}, line: {callerLineNumber}.");
            }
        }

        [Conditional("DEBUG"),
         ContractAnnotation("condition:false => halt"),
         SuppressMessage("ReSharper", "ParameterOnlyUsedForPreconditionCheck.Global")]
        public static void Condition(
            bool condition,
            [CanBeNull, CallerMemberName] string callerMemberName = null,
            [CallerLineNumber] int callerLineNumber = 0)
        {
            if (!condition)
            {
                throw new InvalidOperationException(
                    $"Assertion failed. Condition failed at method: {callerMemberName}, line: {callerLineNumber}.");
            }
        }

        [Conditional("DEBUG"),
         ContractAnnotation("=> halt")]
        public static void Fail(
            [CanBeNull, CallerMemberName] string callerMemberName = null,
            [CallerLineNumber] int callerLineNumber = 0)
        {
            throw new InvalidOperationException(
                $"Failure point reached at method: {callerMemberName}, line: {callerLineNumber}.");
        }
    }
}