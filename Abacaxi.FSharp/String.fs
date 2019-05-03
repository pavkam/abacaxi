namespace Abacaxi.FSharp

open Abacaxi

/// String related functionality
module String =
    /// <summary>
    ///     Reverses the specified string using "undivided" string chunks.
    /// </summary>
    /// <param name="s">The string to reverse.</param>
    /// <returns>The reserved string.</returns>
    /// <exception cref="System.ArgumentNullException">Thrown if <paramref name="s" /> is <c>null</c>.</exception>
    let inline reverse s = 
        StringExtensions.Reverse(s)

    /// <summary>
    ///     Shortens the specified string up to a maximum length.
    /// </summary>
    /// <param name="s">The string.</param>
    /// <param name="maxLength">The maximum length of the output string.</param>
    /// <param name="ellipsis">The ellipsis string.</param>
    /// <returns>A string of a maximum of <paramref name="maxLength" /> character.</returns>
    /// <exception cref="System.ArgumentNullException">Thrown if <paramref name="s" /> is <c>null</c>.</exception>
    /// <exception cref="System.ArgumentOutOfRangeException">
    ///     Thrown if <paramref name="maxLength" /> is less than one or the
    ///     length of <paramref name="ellipsis" /> is greater than <paramref name="maxLength" />.
    /// </exception>
    let inline shorten2 maxLength ellipsis s = 
        StringExtensions.Shorten(s, maxLength, ellipsis)

    /// <summary>
    ///     Shortens the specified string up to a maximum length.
    /// </summary>
    /// <param name="s">The string.</param>
    /// <param name="maxLength">The maximum length of the output string.</param>
    /// <returns>A string of a maximum of <paramref name="maxLength" /> character.</returns>
    /// <exception cref="System.ArgumentNullException">Thrown if <paramref name="s" /> is <c>null</c>.</exception>
    /// <exception cref="System.ArgumentOutOfRangeException">
    ///     Thrown if <paramref name="maxLength" /> is less than the default ellipsis minimum.
    /// </exception>
    let inline shorten maxLength s =
        StringExtensions.Shorten(s, maxLength)

    /// <summary>
    ///     Escapes the specified string.
    /// </summary>
    /// <remarks>
    ///     This method escapes the special characters and unicode escape characters.
    /// </remarks>
    /// <param name="s">The string to escape.</param>
    /// <returns>The escaped string.</returns>
    let escape s =
        StringExtensions.Escape(s)

    /// <summary>
    ///     Checks whether the given string matches the specified glob pattern.
    /// </summary>
    /// <param name="s">The string to check.</param>
    /// <param name="pattern">The pattern.</param>
    /// <param name="ignoreCase">If set to <c>true</c>, ignores the case.</param>
    /// <returns><c>true</c> if the string matches the pattern; otherwise, <c>false</c>.</returns>
    /// <exception cref="System.ArgumentNullException">
    ///     Thrown if <paramref name="s" /> or <paramref name="pattern" /> are
    ///     <c>null</c>.
    /// </exception>
    let matches2 ignoreCase pattern s =
        StringExtensions.Like(s, pattern, ignoreCase)

    /// <summary>
    ///     Checks whether the given string matches the specified glob pattern.
    /// </summary>
    /// <param name="s">The string to check.</param>
    /// <param name="pattern">The pattern.</param>
    /// <returns><c>true</c> if the string matches the pattern; otherwise, <c>false</c>.</returns>
    /// <exception cref="System.ArgumentNullException">
    ///     Thrown if <paramref name="s" /> or <paramref name="pattern" /> are
    ///     <c>null</c>.
    /// </exception>
    let matches pattern s =
        StringExtensions.Like(s, pattern)

    /// <summary>
    ///     Finds all duplicate characters in a given <paramref name="s" />.
    /// </summary>
    /// <param name="s">The string to inspect.</param>
    /// <returns>A s of element-appearances pairs of the detected duplicates.</returns>
    /// <exception cref="System.ArgumentNullException">Thrown if <paramref name="s" /> is <c>null</c>.</exception>
    let findDuplicateChars s =
        StringExtensions.FindDuplicates(s)
        |> Array.map (fun freq -> (freq.Item, freq.Count))

    /// <summary>
    ///     Splits a given string into separate lines (based on the presence of CRLF or LF sequences).
    /// </summary>
    /// <param name="s">The string to split.</param>
    /// <returns>A sequence of strings, each representing an individual line in the string.</returns>
    /// <exception cref="System.ArgumentNullException">Thrown if <paramref name="s" /> is <c>null</c>.</exception>
    let splitIntoLines  s =
        StringExtensions.SplitIntoLines(s)

    /// <summary>
    ///     Wraps the specified string according to a given line length.
    /// </summary>
    /// <param name="s">The string to word wrap.</param>
    /// <param name="lineLength">Length of the line.</param>
    /// <returns>A sequence of lines containing the word wrapped string.</returns>
    /// <exception cref="System.ArgumentNullException">Thrown if <paramref name="s" /> is <c>null</c>.</exception>
    /// <exception cref="System.ArgumentOutOfRangeException">Thrown if <paramref name="lineLength" /> is less than one.</exception>
    let wordWrap lineLength s =
        StringExtensions.WordWrap(s, lineLength)
      
    /// <summary>
    ///     Strips the diacritics from a given string, replacing the characters in question with equivalent non-diacritic ones.
    /// </summary>
    /// <param name="s">The string.</param>
    /// <returns>A string with stripped diacritics.</returns>
    /// <exception cref="System.ArgumentNullException">Thrown if <paramref name="s" /> is <c>null</c>.</exception>
    let stripDiacritics s =
        StringExtensions.StripDiacritics(s)