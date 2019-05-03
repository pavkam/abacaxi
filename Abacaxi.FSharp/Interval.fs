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

open Abacaxi

/// Interval-related functionality.
module Interval =
    /// <summary>
    ///     Merges a sequence of overlapping intervals.
    /// </summary>
    /// <param name="intervals">The intervals to merge.</param>
    /// <returns>An output sequence of all intervals (merged or not).</returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if either of <paramref name="intervals" /> or <paramref name="comparer" />
    ///     are <c>null</c>.
    /// </exception>
    /// <exception cref="InvalidOperationException">Thrown if any interval is invalid (start is greater than end).</exception>
    let mergeOverlapping<'T>(intervals: seq<_>) =
        Interval.MergeOverlapping(intervals, Comparer.makeDefault)

    /// <summary>
    ///     Selects the non-overlapping intervals that yield the best aggregate score.
    /// </summary>
    /// <param name="intervals">The intervals to merge.</param>
    /// <returns>An output sequence of all selected intervals.</returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if either of <paramref name="intervals" /> or <paramref name="comparer" />
    ///     are <c>null</c>.
    /// </exception>
    /// <exception cref="InvalidOperationException">Thrown if any interval is invalid (start is greater than end).</exception>
    let choseBestNonOverlapping (intervals: seq<_>) =
        Interval.ChoseBestNonOverlapping(intervals, Comparer.makeDefault)