﻿(* Copyright 2017-2019 by Alexandru Ciobanu (alex+git@ciobanu.org)
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

open Abacaxi

/// The Fibonacci numbers.
[<RequireQualifiedAccess>]
[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Fibonacci =
    /// <summary>
    ///     Enumerates the first <paramref name="count" /> Fibonacci numbers.
    /// </summary>
    /// <param name="count">The count of Fibonacci "numbers" to enumerate.</param>
    /// <returns>The Fibonacci sequence.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="count" /> is less than zero.</exception>
    let inline numbers count : seq<_> = 
        FibonacciSequence.Enumerate(count)

    /// <summary>
    ///     Gets the Nth Fibonacci number.
    /// </summary>
    /// <param name="index">The index of the Fibonacci number to calculate.</param>
    /// <returns>The Fibonacci number</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="index" /> is less than zero.</exception>
    let inline number index =
        FibonacciSequence.GetMember(index)