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

open System
open Abacaxi

/// Contains sequence extension function.
module Seq =
    /// <summary>
    ///     Determines whether all adjacent elements are in a valid "neighboring" relation.
    /// </summary>
    /// <param name="seq">The sequence to check.</param>
    /// <param name="validator">The predicate that ise used to validate each two adjacent elements.</param>
    /// <returns>
    ///     <c>true</c> if all adjacent elements in the sequence conform to the given predicate; <c>false</c> otherwise.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if <paramref name="seq" /> or
    ///     <paramref name="validator" /> are <c>null</c>.
    /// </exception>
    let inline validAdjacent (seq: seq<'T>) validator =
        SequenceExtensions.IsValidAdjacency(seq, Func<_,_,_> validator)

    /// <summary>
    ///     Checks whether a given sequence is ordered (ascending).
    /// </summary>
    /// <param name=":">The input sequence.</param>
    /// <param name="comparer">The comparer used to compare the keys.</param>
    /// <returns>
    ///     <c>true</c> if all elements in <paramref name="seq" /> are greater or equal than their predecessors.
    ///     <c>false</c> otherwise.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if <paramref name=":" /> or
    ///     <paramref name="comparer" /> are <c>null</c>.
    /// </exception>
    let inline isOrdered2 comparer (seq: seq<'T>) =
        SequenceExtensions.IsOrdered(seq, Comparer.make comparer)

    /// <summary>
    ///     Checks whether a given sequence is ordered (ascending) using the default comparer.
    /// </summary>
    /// <param name="seq">The input sequence.</param>
    /// <returns>
    ///     <c>true</c> if all elements in <paramref name="seq" /> are greater or equal than their predecessors.
    ///     <c>false</c> otherwise.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if <paramref name="seq" /> is <c>null</c>.
    /// </exception>
    let inline isOrdered (seq: seq<'T>) =
        SequenceExtensions.IsOrdered(seq)

    /// <summary>
    ///     Checks whether a given sequence is strictly ordered (ascending).
    /// </summary>
    /// <typeparam name="T">The type of elements in the <paramref name="seq" />.</typeparam>
    /// <param name="seq">The input sequence.</param>
    /// <param name="comparer">The comparer used to compare the keys.</param>
    /// <returns>
    ///     <c>true</c> if all elements in <paramref name="seq" /> are strictly greater than their predecessors.
    ///     <c>false</c> otherwise.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if <paramref name="seq" /> or
    ///     <paramref name="comparer" /> are <c>null</c>.
    /// </exception>
    let inline isStrictlyOrdered2 comparer (seq: seq<'T>) =
        SequenceExtensions.IsStrictlyOrdered(seq, Comparer.make comparer)

    /// <summary>
    ///     Checks whether a given sequence is strictly ordered (ascending) using the default comparer.
    /// </summary>
    /// <param name="seq">The input sequence.</param>
    /// <returns>
    ///     <c>true</c> if all elements in <paramref name="seq" /> are strictly greater than their predecessors.
    ///     <c>false</c> otherwise.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if <paramref name="seq" /> is <c>null</c>.
    /// </exception>
    let inline isStrictlyOrdered (seq: seq<'T>) =
        SequenceExtensions.IsStrictlyOrdered(seq)
    
    /// <summary>
    ///     Checks whether a given sequence is ordered (descending).
    /// </summary>
    /// <param name="seq">The input sequence.</param>
    /// <param name="comparer">The comparer used to compare the keys.</param>
    /// <returns>
    ///     <c>true</c> if all elements in <paramref name="seq" /> are less than or equal to their predecessors.
    ///     <c>false</c> otherwise.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if <paramref name="seq" /> or
    ///     <paramref name="comparer" /> are <c>null</c>.
    /// </exception>
    let inline isOrderedDescending2 comparer (seq: seq<'T>) =
        SequenceExtensions.IsOrderedDescending(seq, Comparer.make comparer)

    /// <summary>
    ///     Checks whether a given sequence is ordered (descending) using the default comparer.
    /// </summary>
    /// <param name="seq">The input sequence.</param>
    /// <returns>
    ///     <c>true</c> if all elements in <paramref name="seq" /> are less than or equal to their predecessors.
    ///     <c>false</c> otherwise.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if <paramref name="seq" /> is <c>null</c>.
    /// </exception>
    let inline isOrderedDescending (seq: seq<'T>) =
        SequenceExtensions.IsOrderedDescending(seq)

    /// <summary>
    ///     Checks whether a given sequence is strictly ordered (descending).
    /// </summary>
    /// <param name="seq">The input sequence.</param>
    /// <param name="comparer">The comparer used to compare the keys.</param>
    /// <returns>
    ///     <c>true</c> if all elements in <paramref name="seq" /> are strictly smaller than their predecessors.
    ///     <c>false</c> otherwise.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if <paramref name="seq" /> or
    ///     <paramref name="comparer" /> are <c>null</c>.
    /// </exception>
    let inline isStrictlyOrderedDescending2 comparer (seq: seq<'T>) =
        SequenceExtensions.IsStrictlyOrderedDescending(seq, Comparer.make comparer)

    /// <summary>
    ///     Checks whether a given sequence is strictly ordered (descending) using the default comparer.
    /// </summary>
    /// <param name="seq">The input sequence.</param>
    /// <returns>
    ///     <c>true</c> if all elements in <paramref name="seq" /> are strictly smaller than their predecessors.
    ///     <c>false</c> otherwise.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if <paramref name="seq" /> is <c>null</c>.
    /// </exception>
    let inline isStrictlyOrderedDescending (seq: seq<'T>) =
        SequenceExtensions.IsStrictlyOrderedDescending(seq)