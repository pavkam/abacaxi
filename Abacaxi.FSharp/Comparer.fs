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

/// Contains a number of utilities to deal with .NET comparer interoperability.
module Comparer =
    type private Comparer<'T> (comp, sign) =
           interface IComparer<'T> with
               member ___.Compare(a, b) = sign * (comp a b)

    /// <summary>
    ///     Creates a new .NET comparer based on a given <paramref name="func" />.
    /// </summary>
    /// <param name="func">The comparison function.</param>
    /// <returns>A a new .NET-compatible comparer object.</returns>
    let inline make<'T> func : IComparer<'T> =
        Comparer<_> (func, 1) :> _

    /// <summary>
    ///     Creates a new descending .NET comparer based on a given <paramref name="func" />.
    /// </summary>
    /// <param name="func">The comparison function.</param>
    /// <returns>A a new .NET-compatible comparer object.</returns>
    let inline makeDescending<'T> func : IComparer<'T> =
        Comparer<_> (func, -1) :> _

    /// <summary>
    ///     Creates a .NET comparer based on standard F# <c>Operators.compare</c>.
    /// </summary>
    /// <returns>A a new .NET-compatible comparer object.</returns>
    let makeDefault<'T when 'T: comparison> = make<'T> Operators.compare

    /// <summary>
    ///     Creates a descending .NET comparer based on standard F# <c>Operators.compare</c>.
    /// </summary>
    /// <returns>A a new .NET-compatible comparer object.</returns>
    let makeDefaultDescending<'T when 'T: comparison> = makeDescending<'T> Operators.compare