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

/// Contains all Abacaxi algorithms.
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
    /// <param name="aggregator">The aggregator function.</param>
    /// <param name="comparer">The comparer.</param>
    /// <param name="subsets">The number of subsets to split into.</param>
    /// <returns>The first sequence of subsets that have the same aggregated value.</returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if either <paramref name="set" /> or <paramref name="aggregator" />
    ///     or <paramref name="comparer" /> is <c>null</c>.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="subsets" /> is less than one.</exception>
    let inline splitIntoSubsetsOfEqualValue2 aggregator comparer subsets (set: Set<'T>) =
        Set.SplitIntoSubsetsOfEqualValue(set |> Array.ofSeq, Aggregator<_> aggregator, Comparer.make comparer, subsets)
        |> Array.map Set<_>

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
    ///     Thrown if <paramref name="set" />, <paramref name="aggregator" /> or
    ///     <paramref name="comparer" /> are null.
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