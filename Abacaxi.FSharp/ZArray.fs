namespace Abacaxi.FSharp

open Abacaxi

module ZArray =
    /// <summary>
    ///     Computes the Z-array for the given <paramref name="array" />.
    /// </summary>
    /// <param name="sequence">The sequence to compute the Z-array for.</param>
    /// <param name="start">The start index in the sequence.</param>
    /// <param name="length">The length of the sequence.</param>
    /// <param name="comparer">The comparer.</param>
    /// <returns>A new, computed Z-array (of integers).</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="comparer" /> ic <c>null</c>.</exception>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="sequence" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown when the combination of <paramref name="startIndex" /> and
    ///     <paramref name="length" /> is out of bounds.
    /// </exception>
    let constructWith<'T when 'T: equality> (array: 'T array) start length =
        ZArray.Construct(array, start, length, EqualityComparer.makeDefault<'T>)