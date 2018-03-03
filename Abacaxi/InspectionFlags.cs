/* Copyright 2017-2018 by Alexandru Ciobanu (alex+git@ciobanu.org)
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
 */

namespace Abacaxi
{
    using System;

    /// <summary>
    /// Defines the three types of members that can be extracted from an object using <seealso cref="ObjectExtensions.Inspect{T}"/> method.
    /// </summary>
    [Flags]
    public enum InspectionFlags
    {
        /// <summary>
        /// Fields are included into the resulting dictionary.
        /// </summary>
        IncludeFields = 1,

        /// <summary>
        /// Properties are included into the resulting dictionary.
        /// </summary>
        IncludeProperties = 2,

        /// <summary>
        /// Parameter-less methods are included into the resulting dictionary.
        /// </summary>
        IncludeMethods = 4,

        /// <summary>
        /// All members are included into the resulting dictionary.
        /// </summary>
        IncludeAll = IncludeFields | IncludeProperties | IncludeMethods,
    }
}
