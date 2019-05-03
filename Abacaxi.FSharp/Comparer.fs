module Abacaxi.FSharp

open System.Collections.Generic

/// Contains a number of utilities to deal with .NET comparer interoperability.
module Comparer =
    type private Comparer<'T> (comp, sign) =
           interface IComparer<'T> with
               member ___.Compare(a, b) = sign * (comp a b)

    /// <summary>
    ///     Creates a new .NET comparer based on a given <paramref name="func" />.
    /// </summary>
    /// <param name="func">The comparison function.</param>
    /// <returns>A a new .NET-compatible comparer object.</returns>
    let inline make<'T> func : IComparer<'T> =
        Comparer<_> (func, 1) :> _

    /// <summary>
    ///     Creates a new descending .NET comparer based on a given <paramref name="func" />.
    /// </summary>
    /// <param name="func">The comparison function.</param>
    /// <returns>A a new .NET-compatible comparer object.</returns>
    let inline makeDescending<'T> func : IComparer<'T> =
        Comparer<_> (func, -1) :> _

    /// <summary>
    ///     Creates a .NET comparer based on standard F# <c>Operators.compare</c>.
    /// </summary>
    /// <returns>A a new .NET-compatible comparer object.</returns>
    let makeDefault<'T when 'T: comparison> = make<'T> Operators.compare

    /// <summary>
    ///     Creates a descending .NET comparer based on standard F# <c>Operators.compare</c>.
    /// </summary>
    /// <returns>A a new .NET-compatible comparer object.</returns>
    let makeDefaultDescending<'T when 'T: comparison> = makeDescending<'T> Operators.compare

/// Contains a number of utilities to deal with .NET equality comparer interoperability.
module EqualityComparer =

    type private EqualityComparer<'T> (eq, hc) =
        interface IEqualityComparer<'T> with
            member ___.Equals(a, b) = eq a b
            member ___.GetHashCode(value) = hc value

    /// <summary>
    ///     Creates a new .NET equality comparer based on a given <paramref name="func" />.
    /// </summary>
    /// <param name="func">The comparison function.</param>
    /// <returns>A a new .NET-compatible comparer object.</returns>
    let inline make<'T> compare hashCode : IEqualityComparer<'T> =
        EqualityComparer<'T> (compare, hashCode)  :> _

    /// <summary>
    ///     Creates a .NET comparer based on standard F# <c>Operators.compare</c>.
    /// </summary>
    /// <returns>A a new .NET-compatible comparer object.</returns>
    let makeDefault<'T when 'T: equality> = make<'T> (=) hash