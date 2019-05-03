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
open System.Collections.Generic
open Abacaxi

/// Specifies a supported sorting algorithm. 
[<RequireQualifiedAccess>]
type SortingAlgorithm =
    /// Bubble-sort. Uses <see name="Sorting.BubbleSort" /> method.
    | Bubble
    /// Cocktail-Shaker-sort. Uses <see name="Sorting.CocktailShakerSort" /> method.
    | CocktailShaker
    /// Comb-sort. Uses <see name="Sorting.CombSort" /> method.
    | Comb 
    /// Gnome-sort. Uses <see name="Sorting.GnomeSort" /> method.
    | Gnome 
    /// Heap-sort. Uses <see name="Sorting.HeapSort" /> method.
    | Heap 
    /// Insertion-sort. Uses <see name="Sorting.InsertionSort" /> method.
    | Insertion 
    /// Merge-sort. Uses <see name="Sorting.MergeSort" /> method.
    | Merge 
    /// Odd-Even-sort. Uses <see name="Sorting.OddEvenSort" /> method.
    | OddEven 
    /// Quick-sort. Uses <see name="Sorting.QuickSort" /> method.
    | Quick 
    /// Shell-sort. Uses <see name="Sorting.ShellSort" /> method.
    | Shell

/// Abacaxi sorting extensions to the Seq module.
[<RequireQualifiedAccess>]
[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Seq =

    let inline private algoSortWith comparer algo sequence =
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
    let inline algoSort algo = 
        algoSortWith Operators.compare algo

    /// <summary>
    ///     Sorts the sequence using the selected algorithm using and the default <see name="Operators.compare" /> operation.
    ///     Elements are sorted in descending order. 
    /// </summary>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if sequence is <c>null</c>.
    /// </exception>
    let inline genSortDescending algo = 
        algoSortWith (Operators.compare >> (-)) algo

    /// <summary>
    ///     Sorts the sequence extracting a comparison key and then using the selected algorithm and the default <see name="Operators.compare" /> operation. 
    /// </summary>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if sequence is <c>null</c>.
    /// </exception>
    let inline algoSortBy algo projection = 
        let inline comparer a b = Operators.compare (projection a) (projection b)
        algoSortWith comparer algo

    /// <summary>
    ///     Sorts the sequence extracting a comparison key and then using the selected algorithm and the default <see name="Operators.compare" /> operation in.
    ///     Elements are sorted in descending order. 
    /// </summary>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if sequence is <c>null</c>.
    /// </exception>
    let inline genSortByDescending algo projection = 
        let inline comparer a b = Operators.compare (projection b) (projection a)
        algoSortWith comparer algo