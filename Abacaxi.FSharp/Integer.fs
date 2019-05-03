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

/// Integer manipulation functionality.
module Integer =
    /// <summary>
    ///     Returns a sequence of numbers (powers of two), which summed, result in the original number
    ///     <paramref name="number" />.
    /// </summary>
    /// <param name="number">The number to be decomposed.</param>
    /// <returns>A sequence of numbers.</returns>
    let deconstructIntoPowersOfTwo number : seq<_> =
        Integer.DeconstructIntoPowersOfTwo(number)
      
    /// <summary>
    ///     Returns a sequence of numbers, which, when multiplied produce the value of <paramref name="number" />.
    /// </summary>
    /// <param name="number">The number to be dis-constructed into its prime factors.</param>
    /// <returns>A sequence of primes.</returns>
    let deconstructIntoPrimeFactors number : seq<_> =
        Integer.DeconstructIntoPrimeFactors(int number)

    /// <summary>
    ///     Checks whether a given number is prime.
    /// </summary>
    /// <param name="number">The number to check.</param>
    /// <returns><c>true</c> if the number is prime; <c>false</c> otherwise.</returns>
    let isPrime number =
        Integer.IsPrime(int number)

    /// <summary>
    ///     Zips the digits of two integer numbers to form a new integer number.
    /// </summary>
    /// <param name="x">The first number to zip.</param>
    /// <param name="y">The second number to zip.</param>
    /// <param name="base">The base of the digits.</param>
    /// <returns>A number whose digits are taken from both <paramref name="x" /> and <paramref name="y" />.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown if <paramref name="x" /> or <paramref name="y" /> are less than
    ///     zero; or <paramref name="base" /> is less than two.
    /// </exception>
    let zip2 b x y =
        Integer.Zip(x, y, b)

    /// <summary>
    ///     Zips the digits of two integer numbers to form a new integer number.
    /// </summary>
    /// <param name="x">The first number to zip.</param>
    /// <param name="y">The second number to zip.</param>
    /// <returns>A number whose digits are taken from both <paramref name="x" /> and <paramref name="y" />.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown if <paramref name="x" /> or <paramref name="y" /> are less than zero.
    /// </exception>
    let zip x y =
        Integer.Zip(x, y)
       
    /// <summary>
    ///     Breaks the specified natural number into smaller components minimizing the number of time those components are
    ///     used.
    /// </summary>
    /// <param name="number">The number to break.</param>
    /// <param name="components">The components that can be used.</param>
    /// <returns>
    ///     A list of components and the number of times they are used. An empty array is returned if the number cannot be
    ///     broken into specified components.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown if <paramref name="number" /> is less than zero or items in <paramref name="components" /> are less than
    ///     one.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if <paramref name="components" /> is <c>null</c>.
    /// </exception>
    let breakIntoComponents number components =
        Integer.Break(number, components)