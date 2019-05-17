(* Copyright 2017-2019 by Alexandru Ciobanu (alex+git@ciobanu.org)
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
*)

namespace Abacaxi.FSharp

open System.Collections.Generic

/// Contains a number of utilities to deal with .NET equality comparer interoperability.
[<RequireQualifiedAccess>]
[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module EqualityComparer =
    type private EqualityComparer<'T> (eq, hc) =
        interface IEqualityComparer<'T> with
            member ___.Equals(a, b) = eq a b
            member ___.GetHashCode(value) = hc value

    /// <summary>
    ///     Creates a new .NET equality comparer based on a given <paramref name="func" />.
    /// </summary>
    /// <param name="func">The comparison function.</param>
    /// <returns>A a new .NET-compatible comparer object.</returns>
    let inline make<'T> compare hashCode : IEqualityComparer<'T> =
        EqualityComparer<'T> (compare, hashCode)  :> _

    /// <summary>
    ///     Creates a .NET comparer based on standard F# <c>Operators.compare</c>.
    /// </summary>
    /// <returns>A a new .NET-compatible comparer object.</returns>
    let makeDefault<'T when 'T: equality> = make<'T> (=) hash