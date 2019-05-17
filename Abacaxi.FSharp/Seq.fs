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
open System.Collections.Generic

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

/// Contains sequence extension function.
[<RequireQualifiedAccess>]
[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Seq =
    /// <summary>
    ///     Determines whether all adjacent elements are in a valid "neighboring" relation.
    /// </summary>
    /// <param name="seq">The sequence to check.</param>
    /// <param name="validator">The predicate that is used to validate each two adjacent elements.</param>
    /// <returns>
    ///     <c>true</c> if all adjacent elements in the sequence conform to the given predicate; <c>false</c> otherwise.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if <paramref name="seq" /> or
    ///     <paramref name="validator" /> are <c>null</c>.
    /// </exception>
    let inline satisfiesRelation (seq: seq<'T>) validator =
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
    let inline isOrderedWith comparer (seq: seq<'T>) =
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
        SequenceExtensions.IsOrdered(seq, Comparer.makeDefault)

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
    let inline isStrictlyOrderedWith comparer (seq: seq<'T>) =
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
        SequenceExtensions.IsStrictlyOrdered(seq, Comparer.makeDefault)
    
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
    let inline isOrderedDescendingWith comparer (seq: seq<'T>) =
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
        SequenceExtensions.IsOrderedDescending(seq, Comparer.makeDefault)

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
    let inline isStrictlyOrderedDescendingWith comparer (seq: seq<'T>) =
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
        SequenceExtensions.IsStrictlyOrderedDescending(seq, Comparer.makeDefault)

    /// <summary>
    ///     Returns a random sample of a given sequence of elements.
    /// </summary>
    /// <param name="random">The random instance to use for sampling.</param>
    /// <param name="seq">The sequence of elements.</param>
    /// <param name="size">Length of the sample to be selected.</param>
    /// <returns>
    ///     A random sequence of elements from <paramref name="sequence" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if <paramref name="seq" /> or <paramref name="random" /> are
    ///     <c>null</c>.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="seq" /> is less than one.</exception>
    let randomSample random size (seq: seq<'T>) =
        RandomExtensions.Sample(random, seq, size)

    /// <summary>
    ///     Returns a random item from a given <paramref name="sequence" />.
    /// </summary>
    /// <typeparam name="T">The type of items in the sequence.</typeparam>
    /// <param name="random">The random class instance.</param>
    /// <param name="sequence">The sequence.</param>
    /// <returns>A random element from the given <paramref name="sequence" /></returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if <paramref name="sequence" /> or <paramref name="random" /> are
    ///     <c>null</c>.
    /// </exception>
    /// <exception cref="ArgumentException">Thrown if <paramref name="sequence" /> is empty.</exception>
    let randomItem random (array: 'T[]) = 
        RandomExtensions.NextItem(random, array)

    let inline private algoSortWith comparer algo (sequence: seq<'T>) =
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

        let array = Seq.toArray sequence
        method(array, 0, array.Length, comparer)
        array

    /// <summary>
    ///     Sorts the sequence extracting a comparison key and then using the selected algorithm and the default <see name="Operators.compare" /> operation. 
    /// </summary>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if sequence is <c>null</c>.
    /// </exception>
    let inline sortWith algo = 
        algoSortWith Comparer.makeDefault algo

    /// <summary>
    ///     Sorts the sequence using the selected algorithm using and the default <see name="Operators.compare" /> operation.
    ///     Elements are sorted in descending order. 
    /// </summary>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if sequence is <c>null</c>.
    /// </exception>
    let inline sortDescendingWith algo = 
        algoSortWith Comparer.makeDefaultDescending algo

    /// <summary>
    ///     Sorts the sequence extracting a comparison key and then using the selected algorithm and the default <see name="Operators.compare" /> operation. 
    /// </summary>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if sequence is <c>null</c>.
    /// </exception>
    let inline sortWithBy algo projection = 
        let inline comparer a b = Operators.compare (projection a) (projection b)
        algoSortWith (Comparer.make comparer) algo

    /// <summary>
    ///     Sorts the sequence extracting a comparison key and then using the selected algorithm and the default <see name="Operators.compare" /> operation in.
    ///     Elements are sorted in descending order. 
    /// </summary>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if sequence is <c>null</c>.
    /// </exception>
    let inline sortDescendingWithBy algo projection = 
        let inline comparer a b = Operators.compare (projection b) (projection a)
        algoSortWith (Comparer.make comparer) algo

    /// <summary>
    ///     Finds the longest increasing sequence in a given <paramref name="sequence" />.
    /// </summary>
    /// <param name="seq">The sequence to verify.</param>
    /// <returns>The longest increasing sequence.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the <paramref name="seq" /> is <c>null</c>.</exception>
    let inline longestIncreasingSequence (seq: seq<'T>) : seq<_> = 
        SequenceAlgorithms.FindLongestIncreasingSequence(seq, Comparer.makeDefault)

    /// <summary>
    ///     Determines whether the sequence contains two elements that aggregate to a given <paramref name="target" /> value.
    /// </summary>
    /// <param name="sequence">The sequence to check.</param>
    /// <param name="target">The target value to search for.</param>
    /// <param name="aggregator">The function that aggregates two values.</param>
    /// <param name="comparer">The comparer.</param>
    /// <returns>
    ///     <c>true</c> if the <paramref name="sequence" /> contains two elements that aggregate; otherwise, <c>false</c>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if <paramref name="sequence" /> is <c>null</c>
    /// </exception>
    let inline containsTwoElementsWithAggregate target (seq: seq<'T>) =
        SequenceAlgorithms.ContainsTwoElementsThatAggregateTo(seq, Aggregator<_> (+), Comparer.makeDefault, target)

    /// <summary>
    ///     Finds all duplicate items in a given <paramref name="seq" />.
    /// </summary>
    /// <param name="seq">The sequence to inspect.</param>
    /// <returns>A sequence of element-frequency pairs of the detected duplicates.</returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if <paramref name="seq" /> is <c>null</c>.
    /// </exception>
    let inline getDuplicates (seq: seq<'T>) =
        SequenceAlgorithms.FindDuplicates(seq, EqualityComparer.makeDefault)
        |> Array.map (fun i -> i.Item, i.Count)

    /// <summary>
    ///     Finds all duplicate integers in a given <paramref name="seq" />.
    /// </summary>
    /// <param name="seq">The sequence to inspect.</param>
    /// <param name="min">The minimum possible value of an element part of the <paramref name="seq" />.</param>
    /// <param name="max">The maximum possible value of an element part of the <paramref name="seq" />.</param>
    /// <returns>A seq of element-frequency pairs of the detected duplicates.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="seq" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown if <paramref name="max" /> is less than <paramref name="min" />.
    /// </exception>
    let inline getIntDuplicates (min, max) (seq: seq<_>) =
        SequenceAlgorithms.FindDuplicates(seq, min, max)
        |> Array.map (fun i -> i.Item, i.Count)

    /// <summary>
    ///     Finds all unique items in a given <paramref name="seq" />.
    /// </summary>
    /// <param name="seq">The sequence to inspect.</param>
    /// <param name="comparer">The comparer function.</param>
    /// <param name="hash">The hash function.</param>
    /// <returns>A sequence of detected uniques.</returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if <paramref name="seq" /> is <c>null</c>.
    /// </exception>
    let inline getUniques (seq: seq<_>) =
        SequenceAlgorithms.FindUniques(seq, EqualityComparer.makeDefault)

    /// <summary>
    ///     Finds all unique items in a given <paramref name="seq" /> and returns them in order of appearance.
    /// </summary>
    /// <param name="seq">The sequence to inspect.</param>
    /// <param name="comparer">The comparer function.</param>
    /// <param name="hash">The hash function.</param>
    /// <returns>A sequence of detected uniques in order of appearance.</returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if <paramref name="seq" /> is <c>null</c>.
    /// </exception>
    let inline getUniquesInOrder (seq: seq<_>) =
        SequenceAlgorithms.FindUniquesInOrder(seq, EqualityComparer.makeDefault)

    /// <summary>
    ///     Extracts all nested groups from sequence. The method returns a sequence of sequences.
    /// </summary>
    /// <param name="seq">The sequence.</param>
    /// <param name="openBracket">The element that signifies the start of a group.</param>
    /// <param name="closeBracket">The element that signifies the end of a group.</param>
    /// <returns>The sequence of extracted groups, starting with the inner most ones.</returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if <paramref name="sequence" /> is <c>null</c>.
    /// </exception>
    /// <exception cref="InvalidOperationException">Throws if the number of open and close brackets do not match.</exception>
    let inline getNested (openBracket, closeBracket) (seq: seq<_>) : seq<_> = 
        SequenceAlgorithms.ExtractNestedBlocks(seq, openBracket, closeBracket, EqualityComparer.makeDefault)

    /// <summary>
    ///     Finds the sub-sequences whose aggregated values are equal to a given <paramref name="target" /> value.
    /// </summary>
    /// <typeparam name="T">The type of elements in the sequence.</typeparam>
    /// <param name="seq">The sequence to check.</param>
    /// <param name="target">The target aggregated value.</param>
    /// <returns>
    ///     A sequence of found integers.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="seq" /> is <c>null</c>.</exception>
    let inline subsequencesWithAggregate target (seq: 'T[]) : seq<_> =
        SequenceAlgorithms.GetSubsequencesOfAggregateValue(seq, Aggregator<_> (+), Aggregator<_> (-), 
            EqualityComparer.makeDefault, target)

    /// <summary>
    ///     Interleaves multiple sequences into one output sequence.
    /// </summary>
    /// <typeparam name="T">The type of elements in the sequences.</typeparam>
    /// <param name="comparer">The comparer.</param>
    /// <param name="seq">The first sequence to interleave.</param>
    /// <param name="other">The next sequences to interleave.</param>
    /// <returns>A new interleaved stream.</returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if <paramref name="seq" /> or <paramref name="others" /> are <c>null</c>.
    /// </exception>
    /// <exception cref="ArgumentException">Thrown if the <paramref name="others" /> is empty.</exception>
    /// <exception cref="InvalidOperationException">Thrown if one or more sequences contain unsorted items.</exception>
    let inline interleave (seq: seq<'T>) (others: seq<_>[]) =
        SequenceAlgorithms.Interleave(Comparer.makeDefault, seq, others)

    /// <summary>
    ///     Creates an array whose contents are the elements of the <paramref name="seq" /> repeated
    ///     <paramref name="count" /> times.
    /// </summary>
    /// <param name="seq">The input sequence.</param>
    /// <param name="count">Number of times to repeat the sequence.</param>
    /// <returns>A new array.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="seq" /> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown if the value of <paramref name="count" /> argument is less
    ///     than <c>1</c>.
    /// </exception>
    let inline repeat count (seq: seq<'T>) =
        SequenceAlgorithms.Repeat(seq, count)

    /// <summary>
    ///     Finds the location of <paramref name="item" /> in the given <paramref name="seq" />.
    /// </summary>
    /// <param name="seq">The sequence to search.</param>
    /// <param name="start">The start index in the sequence.</param>
    /// <param name="length">The length of sequence to search..</param>
    /// <param name="item">The item to search for.</param>
    /// <returns>The index in the sequence where the <paramref name="item" /> was found; <c>-1</c> otherwise.</returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if <paramref name="seq" /> is <c>null</c>.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown when the combination of <paramref name="start" /> and
    ///     <paramref name="length" /> is out of bounds.
    /// </exception>
    let inline binarySearch (start, length) item (seq: 'T[]) = 
        SequenceAlgorithms.BinarySearch(seq, start, length, item, Comparer.makeDefault)
 
    /// <summary>
    ///     Finds the location of <paramref name="item" /> in the given <paramref name="seq" /> that is presumed to be sorted in descending order.
    /// </summary>
    /// <param name="seq">The sequence to search.</param>
    /// <param name="start">The start index in the sequence.</param>
    /// <param name="length">The length of sequence to search..</param>
    /// <param name="item">The item to search for.</param>
    /// <returns>The index in the sequence where the <paramref name="item" /> was found; <c>-1</c> otherwise.</returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if <paramref name="seq" /> is <c>null</c>.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown when the combination of <paramref name="start" /> and
    ///     <paramref name="length" /> is out of bounds.
    /// </exception>
    let inline binarySearchDescending (start, length) item (seq: 'T[]) = 
        SequenceAlgorithms.BinarySearch(seq, start, length, item, Comparer.makeDefault, false)

    /// <summary>
    ///     Finds the location of <paramref name="item" /> in the given <paramref name="seq" />. If the item is repeated a
    ///     number of times, this method returns their index range. Otherwise, this method returns the item immediately 
    ///     smaller than the searched value.
    /// </summary>
    /// <param name="seq">The sequence to search.</param>
    /// <param name="start">The start index in the sequence.</param>
    /// <param name="length">The length of sequence to search..</param>
    /// <param name="item">The item to search for.</param>
    /// <returns>An index pair in the sequence where the <paramref name="item" />(s) were found; <c>-1</c> otherwise.</returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if <paramref name="sequence" /> is <c>null</c>.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown when the combination of <paramref name="start" /> and <paramref name="length" /> is out of bounds.
    /// </exception>
    let inline binaryLookup (start, length) item (seq: 'T[]) = 
        SequenceAlgorithms.BinaryLookup(seq, start, length, item, Comparer.makeDefault) 
        |> System.TupleExtensions.ToTuple<_,_>

    /// <summary>
    ///     Finds the location of <paramref name="item" /> in the given descending sequence <paramref name="seq" />. 
    ///     If the item is repeated a number of times, this method returns their index range. Otherwise, 
    ///     this method returns the item immediately smaller than the searched value.
    /// </summary>
    /// <param name="seq">The sequence to search.</param>
    /// <param name="start">The start index in the sequence.</param>
    /// <param name="length">The length of sequence to search..</param>
    /// <param name="item">The item to search for.</param>
    /// <returns>An index pair in the sequence where the <paramref name="item" />(s) were found; <c>-1</c> otherwise.</returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if <paramref name="sequence" /> is <c>null</c>.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown when the combination of <paramref name="start" /> and <paramref name="length" /> is out of bounds.
    /// </exception>
    let inline binaryLookupDescending (start, length) item (seq: 'T[]) = 
        let struct (f, l) = SequenceAlgorithms.BinaryLookup(seq, start, length, item, Comparer.makeDefault, false)
        (f, l)

    /// <summary>
    ///     Evaluates the edit distance between two given sequences <paramref name="seqA" /> and
    ///     <paramref name="seqB" />.
    /// </summary>
    /// <param name="seqA">The sequence to compare to.</param>
    /// <param name="seqB">The sequence to compare with.</param>
    /// <returns>
    ///     A sequence of "edits" applied to the original <paramref name="seqA" /> to obtain the
    ///     <paramref name="seqB" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if either <paramref name="seqA" /> or
    ///     <paramref name="seqB" /> are <c>null</c>.
    /// </exception>
    let inline diff (seqA: 'T[]) (seqB: 'T[]) =
        SequenceAlgorithms.Diff(seqA, seqB)
        |> Array.map (fun i -> i.Item, i.Operation)

    /// <summary>
    ///     Gets the longest common sub-sequence shared by <paramref name="seqA" /> and <paramref name="seqB" />.
    /// </summary>
    /// <param name="seqA">The sequence to compare to.</param>
    /// <param name="seqB">The sequence to compare with.</param>
    /// <returns>The longest common sub-sequence shared by both sequences.</returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if either <paramref name="seqA" /> or
    ///     <paramref name="seqB" /> are <c>null</c>.
    /// </exception>
    let inline lcs (seqA: 'T[]) (seqB: 'T[]) =
        SequenceAlgorithms.GetLongestCommonSubsequence(seqA, seqB)

    /// <summary>
    ///     Evaluates the appearance frequency for each item in a <paramref name="seq" />.
    /// </summary>
    /// <param name="seq">The sequence.</param>
    /// <returns>
    ///     A map where each key is an item form the <paramref name="seq" /> and associated values are the
    ///     frequencies.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if <paramref name="sequence" /> is <c>null</c>.
    /// </exception>
    let inline frequency (seq: seq<'T>) =
        SequenceAlgorithms.GetItemFrequencies(seq, EqualityComparer.makeDefault)
        |> Seq.map (|KeyValue|)
        |> Map.ofSeq

    /// <summary>
    ///     De-constructs a given <paramref name="seq" /> into subsequences known as "terms". Each term is validated/scored
    ///     by <paramref name="arbiter" /> function.
    /// </summary>
    /// <param name="seq">The sequence.</param>
    /// <param name="arbiter">The scoring function.</param>
    /// <returns>A sequence of terms that the original sequence was split into.</returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if <paramref name="seq" /> is <c>null</c>.
    /// </exception>
    let inline deconstructIntoTerms arbiter (seq: 'T[]) =
        let shunt (l: IList<'T>) i1 i2 = arbiter l.[i1] l.[i2]
        SequenceAlgorithms.DeconstructIntoTerms(seq, Func<_,_,_,_> shunt)

    /// <summary>
    ///     Determines whether the specified <paramref name="seq" /> is a palindrome.
    /// </summary>
    /// <param name="seq">The sequence.</param>
    /// <param name="start">The start index.</param>
    /// <param name="length">The length to check.</param>
    /// <returns>
    ///     <c>true</c> if the specified sequence is a palindrome; otherwise, <c>false</c>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if <paramref name="sequence" /> is <c>null</c>.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown when the combination of <paramref name="start" /> and
    ///     <paramref name="length" /> is out of bounds.
    /// </exception>
    let inline isPalindrome (start, length) (seq: 'T[]) = 
        SequenceAlgorithms.IsPalindrome(seq, start, length, EqualityComparer.makeDefault)

    /// <summary>
    ///     Determines whether the given <paramref name="seq" /> is a permutation of a palindrome.
    /// </summary>
    /// <param name="seq">The sequence.</param>
    /// <param name="start">The start index.</param>
    /// <param name="length">The length to check.</param>
    /// <returns>
    ///     <c>true</c> if the specified sequence is a permutation of a palindrome; otherwise, <c>false</c>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if <paramref name="seq" /> is <c>null</c>.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown when the combination of <paramref name="start" /> and
    ///     <paramref name="length" /> is out of bounds.
    /// </exception>
    let inline isPermutationOfPalindrome (start, length) (seq: 'T[]) =
        SequenceAlgorithms.IsPermutationOfPalindrome(seq, start, length, EqualityComparer.makeDefault)

    /// <summary>
    ///     Returns the index of a permutation sub-sequence.
    /// </summary>
    /// <param name="seq">The sequence.</param>
    /// <param name="subSeq">The sub-sequence to check for.</param>
    /// <returns>
    ///     A zero-based index in the <paramref name="seq" /> where a permutation of <paramref name="subSeq" />
    ///     was found. <c>-1</c> if not found.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if <paramref name="seq" /> is <c>null</c>.
    /// </exception>
    let inline indexOfPermutationOf (seq: 'T[]) (subSeq: 'T[]) =
        SequenceAlgorithms.IndexOfPermutationOf(seq, subSeq, EqualityComparer.makeDefault)

    /// <summary>
    ///     Finds the index and length of the unordered subsequence in the given <paramref name="seq" />.
    /// </summary>
    /// <param name="seq">The sequence to check.</param>
    /// <returns>
    ///     A tuple consisting of the index where the sub-sequence starts and its length.<c>(0, 0)</c> is returned if
    ///     there is no such sub-sequence.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if <paramref name="seq" /> is <c>null</c>.
    /// </exception>
    let inline findUnorderedSubsequenceRange (seq: 'T[]) =
        let struct (i, l) = SequenceAlgorithms.FindUnorderedSubsequenceRange(seq, Comparer.makeDefault)
        (i, l)
 
    /// <summary>
    ///     Gets the subsequence with greatest aggregate value.
    /// </summary>
    /// <param name="seq">The sequence.</param>
    /// <returns>The range of the subsequence that satisfies the problem; <c>null</c> otherwise.</returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if <paramref name="seq" /> is <c>null</c>.
    /// </exception>
    let inline rangeWithGreatestAggregate (seq: 'T[]) =
        let r = SequenceAlgorithms.GetRangeWithGreatestAggregateValue(seq, Aggregator<_> (+), Comparer.makeDefault)
        if r.HasValue then 
            let struct (i, f) = r.Value
            Some (i, f)
        else
            None