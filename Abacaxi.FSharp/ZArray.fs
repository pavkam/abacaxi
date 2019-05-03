namespace Abacaxi.FSharp

open Abacaxi

/// Z-Array related functionality
module ZArray =
    /// <summary>
    ///     Computes the Z-array for the given <paramref name="sequence" />.
    /// </summary>
    /// <param name="sequence">The sequence to compute the Z-array for.</param>
    /// <returns>A new, computed Z-array (of integers).</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="sequence" /> is <c>null</c>.</exception>
    let construct<'T when 'T: equality> (sequence: 'T seq) =
        let array = sequence |> Seq.toArray
        ZArray.Construct(array, 0, array.Length, EqualityComparer.makeDefault<'T>)