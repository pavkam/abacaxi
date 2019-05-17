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
open System

/// Object-related functionality and helpers.
[<RequireQualifiedAccess>]
[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Object =
    /// <summary>
    ///     Inspects a given object and extract a set of key-value pairs. Each pair is a field/property/method and its
    ///     associated value. The inspection
    ///     only considers public, non-static, non-generic and parameter-less members.
    /// </summary>
    /// <param name="value">The object.</param>
    /// <param name="flags">The inspection flags.</param>
    /// <returns>A readonly dictionary containing all object's inspected members.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="value" /> is <c>null</c>.</exception>
    let inspectWith<'T> flags value =
        ObjectExtensions.Inspect<'T>(value, flags)
    
    /// <summary>
    ///     Inspects a given object and extract a set of key-value pairs. Each pair is a field/property/method and its
    ///     associated value. The inspection
    ///     only considers public, non-static, non-generic and parameter-less members.
    /// </summary>
    /// <param name="value">The object.</param>
    /// <returns>A readonly dictionary containing all object's inspected members.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="value" /> is <c>null</c>.</exception>
    let inspect<'T> value =
        ObjectExtensions.Inspect<'T>(value)

    /// <summary>
    ///     Tries the cast or convert a given <paramref name="object" /> to a value of a different type.
    /// </summary>
    /// <param name="object">The object to convert.</param>
    /// <param name="formatProvider">The format provider.</param>
    /// <returns>Optional converted value.</returns>
    let tryConvertWith<'T> (formatProvider: IFormatProvider) value =
        match ObjectExtensions.TryConvert<'T>(value, formatProvider) with
        | (true, v) -> Some v
        | (false, _) -> None
       
    /// <summary>
    ///     Tries the cast or convert a given <paramref name="object" /> to a value of a different type.
    ///     This method uses <seealso cref="CultureInfo.InvariantCulture" /> for the conversion.
    /// </summary>
    /// <param name="object">The object to convert.</param>
    /// <returns>Optional converted value.</returns>
    let tryConvert<'T> value =
        match ObjectExtensions.TryConvert<'T>(value) with
        | (true, v) -> Some v
        | (false, _) -> None

    /// <summary>
    ///     Converts a given <paramref name="object" /> to a value of a different type.
    /// </summary>
    /// <param name="object">The value to convert.</param>
    /// <param name="formatProvider">The format provider.</param>
    /// <param name="validator">The validation function.</param>
    /// <returns>
    ///     The converted value.
    /// </returns>
    /// <exception cref="FormatException">Thrown if the conversion failed.</exception>
    /// <exception cref="InvalidOperationException">Thrown if the validation failed.</exception>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if either <paramref name="formatProvider" /> or
    ///     <paramref name="validator" /> is <c>null</c>.
    /// </exception>
    let convertWith<'T> value formatProvider validator =
        ObjectExtensions.As<'T>(value, formatProvider, Func<'T, bool> validator)
       
    /// <summary>
    ///     Converts a given <paramref name="object" /> to a value of a different type.
    ///     This method uses <seealso cref="CultureInfo.InvariantCulture" /> for the conversion.
    /// </summary>
    /// <typeparam name="T">The type to convert to.</typeparam>
    /// <param name="object">The value to convert.</param>
    /// <returns>
    ///     The converted value.
    /// </returns>
    /// <exception cref="FormatException">Thrown if the conversion failed.</exception>
    let convert<'T> value =
        ObjectExtensions.As<'T>(value)