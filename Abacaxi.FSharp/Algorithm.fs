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
open System.Collections.Generic
open System

/// Contains all Abacaxi algorithms.
[<RequireQualifiedAccess>]
[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Algorithm =
    /// <summary>
    ///     Finds the best combination of items to be placed in a knapsack of given <paramref name="knapsackWeight" /> weight.
    /// </summary>
    /// <param name="sequence">The sequence of item/value/weight elements.</param>
    /// <param name="weight">The total knapsack weight.</param>
    /// <returns>
    ///     The best selection of items filling the knapsack and maximizing total value.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="sequence" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="weight" /> is less than one.</exception>
    let inline fillKnapsack<'T> weight (sequence: seq<'T * double * int>) =
        let sequence = sequence |> Seq.map KnapsackItem<_>
        Knapsack.Fill(sequence, weight)

    /// <summary>
    ///     Enumerates the first <paramref name="count" /> Fibonacci numbers.
    /// </summary>
    /// <param name="count">The count of Fibonacci "numbers" to enumerate.</param>
    /// <returns>The Fibonacci sequence.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="count" /> is less than zero.</exception>
    let inline listFibonacciNumbers count : seq<_> = 
        FibonacciSequence.Enumerate(count)

    /// <summary>
    ///     Gets the Nth Fibonacci number.
    /// </summary>
    /// <param name="index">The index of the Fibonacci number to calculate.</param>
    /// <returns>The Fibonacci number</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="index" /> is less than zero.</exception>
    let inline getFibonacciNumber index =
        FibonacciSequence.GetMember(index)

    /// <summary>
    ///     Computes the Z-array for the given <paramref name="sequence" />.
    /// </summary>
    /// <param name="sequence">The sequence to compute the Z-array for.</param>
    /// <returns>A new, computed Z-array (of integers).</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="sequence" /> is <c>null</c>.</exception>
    let inline constructZArray (sequence: 'T seq) =
        let array = sequence |> Seq.toArray
        ZArray.Construct(array, 0, array.Length, EqualityComparer.makeDefault<'T>)

    /// <summary>
    ///     Evaluates all combinations of items in <paramref name="sequence" /> divided into <paramref name="subsets" />.
    /// </summary>
    /// <param name="set">The set of elements.</param>
    /// <param name="subsets">Number of sub-sets.</param>
    /// <returns>All the combinations of subsets.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="set" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="subsets" /> is less than one.</exception>
    let inline listSubsetCombinations subsets (set: Set<'T>) =
        Set.EnumerateSubsetCombinations(set |> Array.ofSeq, subsets) 
        |> Seq.map (fun c -> c |> Array.map Set<_>)

    /// <summary>
    ///     Finds the subsets with equal aggregate value.
    /// </summary>
    /// <param name="set">The set of elements.</param>
    /// <param name="subsets">The number of subsets to split into.</param>
    /// <returns>The first sequence of subsets that have the same aggregated value.</returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if <paramref name="set" /> is <c>null</c>.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="subsets" /> is less than one.</exception>
    let inline splitIntoSubsetsOfEqualValue subsets (set: Set<'T>) =
        Set.SplitIntoSubsetsOfEqualValue(set |> Array.ofSeq, Aggregator<_> (+), Comparer.makeDefault, subsets)
        |> Array.map Set<_>

    /// <summary>
    ///     Finds the <paramref name="set" /> of integers, which summed, return the closest sum to a given
    ///     <paramref name="target" />.
    /// </summary>
    /// <param name="set">The set of natural integers.</param>
    /// <param name="target">The target sum to aim for.</param>
    /// <returns>A sequence of found integers.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the <paramref name="set" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown if <paramref name="target" /> is less than <c>1</c> or the
    ///     <paramref name="sequence" /> contains negative number.
    /// </exception>
    let inline subsetWithClosestValue target (set: Set<_>) = 
        Set.GetSubsetWithNearValue(set, target) |> Set<_>

    /// <summary>
    ///     Checks if the <paramref name="set" /> contains elements, which, summed, yield a given target
    ///     <paramref name="target" />.
    /// </summary>
    /// <param name="set">The sequence of natural integers.</param>
    /// <param name="target">The sum to target for.</param>
    /// <returns><c>true</c> if the condition is satisfied; <c>false</c> otherwise.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the <paramref name="set" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown if <paramref name="target" /> is less than <c>1</c> or the
    ///     <paramref name="set" /> contains negative number.
    /// </exception>
    let inline containsSubsetWithExactValue target (set: Set<_>) =
        Set.ContainsSubsetWithExactValue(set, target)

    /// <summary>
    ///     Finds the first N elements, which summed, yield the biggest sum.
    /// </summary>
    /// <param name="set">The sequence of elements.</param>
    /// <param name="size">The size of subset to consider.</param>
    /// <param name="aggregator">The aggregator function which sums elements.</param>
    /// <param name="comparer">The comparer.</param>
    /// <returns>An array of elements with the highest sum.</returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if <paramref name="set" />, <paramref name="aggregator" /> are null.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown if the <paramref name="size" /> is greater than the number of
    ///     elements in <paramref name="set" />.
    /// </exception>
    let inline subsetWithGreatestValue aggregator comparer size (set: Set<'T>) =
        Set.GetSubsetWithGreatestValue(set, size, Aggregator<_> aggregator, Comparer.make comparer)
        |> Set<_>

    /// <summary>
    ///     Gets all the permutations for a given set.
    /// </summary>
    /// <param name="set">The sequence of elements.</param>
    /// <returns>An array of permutations of <paramref name="sequence" />.</returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if <paramref name="set" /> is null.
    /// </exception>
    let inline listPermutations (set: Set<'T>) =
        Set.GetPermutations(set |> Set.toArray)
        |> Array.map Set<_>

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
    let inline containsTwoElementsThatAggregateTo target (seq: seq<'T>) =
        SequenceAlgorithms.ContainsTwoElementsThatAggregateTo(seq, Aggregator<_> (+), Comparer.makeDefault, target)

    /// <summary>
    ///     Finds all duplicate items in a given <paramref name="seq" />.
    /// </summary>
    /// <param name="seq">The sequence to inspect.</param>
    /// <returns>A sequence of element-frequency pairs of the detected duplicates.</returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if <paramref name="seq" /> is <c>null</c>.
    /// </exception>
    let inline listDuplicates (seq: seq<'T>) =
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
    let inline listIntDuplicates (min, max) (seq: seq<_>) =
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
    let inline listUniques (seq: seq<_>) =
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
    let inline listUniquesInOrder (seq: seq<_>) =
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
    let inline listNestedBlocks (openBracket, closeBracket) (seq: seq<_>) : seq<_> = 
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
    let inline listSubsequencesOfAggregateValue target (seq: 'T[]) : seq<_> =
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
        let struct (f, l) = SequenceAlgorithms.BinaryLookup(seq, start, length, item, Comparer.makeDefault)
        (f, l)

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
    let inline freq (seq: seq<'T>) =
        SequenceAlgorithms.GetItemFrequencies(seq, EqualityComparer.makeDefault)
        |> Seq.map (|KeyValue|)
        |> Map.ofSeq

    /// <summary>
    ///     Deconstructs a given <paramref name="seq" /> into subsequences known as "terms". Each term is validated/scored
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
    let inline getRangeWithGreatestAggregateValue (seq: 'T[]) =
        let r = SequenceAlgorithms.GetRangeWithGreatestAggregateValue(seq, Aggregator<_> (+), Comparer.makeDefault)
        if r.HasValue then 
            let struct (i, f) = r.Value
            Some (i, f)
        else
            None

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
    let mergeOverlappingIntervals<'T>(intervals: seq<_>) =
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
    let choseBestNonOverlappingInterval (intervals: seq<_>) =
        Interval.ChoseBestNonOverlapping(intervals, Comparer.makeDefault)