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

namespace Abacaxi

open System.Collections.Generic
open System

// <summary>
///     Specifies a supported sorting algorithm. 
/// </summary>
[<RequireQualifiedAccess>]
type SortingAlgorithm =
/// <summary>
///     Bubble-sort. Uses <see name="Sorting.BubbleSort" /> method.
/// </summary>
| Bubble
/// <summary>
///     Cocktail-Shaker-sort. Uses <see name="Sorting.CocktailShakerSort" /> method.
/// </summary>
| CocktailShaker
/// <summary>
///     Comb-sort. Uses <see name="Sorting.CombSort" /> method.
/// </summary>
| Comb 
/// <summary>
///     Gnome-sort. Uses <see name="Sorting.GnomeSort" /> method.
/// </summary>
| Gnome 
/// <summary>
///     Heap-sort. Uses <see name="Sorting.HeapSort" /> method.
/// </summary>
| Heap 
/// <summary>
///     Insertion-sort. Uses <see name="Sorting.InsertionSort" /> method.
/// </summary>
| Insertion 
/// <summary>
///     Merge-sort. Uses <see name="Sorting.MergeSort" /> method.
/// </summary>
| Merge 
/// <summary>
///     Odd-Even-sort. Uses <see name="Sorting.OddEvenSort" /> method.
/// </summary>
| OddEven 
/// <summary>
///     Quick-sort. Uses <see name="Sorting.QuickSort" /> method.
/// </summary>
| Quick 
/// <summary>
///     Shell-sort. Uses <see name="Sorting.ShellSort" /> method.
/// </summary>
| Shell

/// Abacaxi sorting extensions to the Seq module.
[<RequireQualifiedAccess>]
[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Seq =
    /// <summary>
    ///     Sorts the sequence using the provided algorithm and comparer.
    /// </summary>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if either sequence or comparer are <c>null</c>.
    /// </exception>
    [<CompiledName("AlgoSortWith")>]
    let inline algoSortWith algo comparer sequence =
        let method = 
            match algo with 
            | SortingAlgorithm.Bubble -> Sorting.BubbleSort
            | SortingAlgorithm.CocktailShaker -> Sorting.CocktailShakerSort
            | SortingAlgorithm.Comb -> Sorting.CombSort
            | SortingAlgorithm.Gnome -> Sorting.GnomeSort
            | SortingAlgorithm.Heap -> Sorting.HeapSort
            | SortingAlgorithm.Insertion -> Sorting.InsertionSort
            | SortingAlgorithm.Merge -> Sorting.MergeSort
            | SortingAlgorithm.OddEven -> Sorting.OddEvenSort
            | SortingAlgorithm.Quick -> Sorting.QuickSort
            | SortingAlgorithm.Shell -> Sorting.ShellSort

        let comparer = Comparer<'a>.Create(Comparison<'a>(comparer))

        let array = Seq.toArray sequence
        method(array, 0, array.Length, comparer)
        array

    /// <summary>
    ///     Sorts the sequence extracting a comparison key and then using the selected algorithm and the default <see name="Operators.compare" /> operation. 
    /// </summary>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if sequence is <c>null</c>.
    /// </exception>
    [<CompiledName("AlgoSort")>]
    let inline algoSort algo = 
        let inline comparer a b = Operators.compare a b
        algoSortWith algo comparer

    /// <summary>
    ///     Sorts the sequence using the selected algorithm using and the default <see name="Operators.compare" /> operation.
    ///     Elements are sorted in descending order. 
    /// </summary>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if sequence is <c>null</c>.
    /// </exception>
    [<CompiledName("AlgoSortDescending")>]
    let inline genSortDescending algo = 
        let inline comparer a b = Operators.compare b a
        algoSortWith algo comparer

    /// <summary>
    ///     Sorts the sequence extracting a comparison key and then using the selected algorithm and the default <see name="Operators.compare" /> operation. 
    /// </summary>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if sequence is <c>null</c>.
    /// </exception>
    [<CompiledName("AlgoSortBy")>]
    let inline algoSortBy algo projection = 
        let inline comparer a b = Operators.compare (projection a) (projection b)
        algoSortWith algo comparer

    /// <summary>
    ///     Sorts the sequence extracting a comparison key and then using the selected algorithm and the default <see name="Operators.compare" /> operation in.
    ///     Elements are sorted in descending order. 
    /// </summary>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if sequence is <c>null</c>.
    /// </exception>
    [<CompiledName("AlgoSortByDescending")>]
    let inline genSortByDescending algo projection = 
        let inline comparer a b = Operators.compare (projection b) (projection a)
        algoSortWith algo comparer