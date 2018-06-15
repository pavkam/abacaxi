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

#pragma warning disable 628
#pragma warning disable 169
#pragma warning disable 414

namespace Abacaxi.Tests.ObjectExtensions
{
    using System.Diagnostics.CodeAnalysis;
    using JetBrains.Annotations;

    [SuppressMessage("ReSharper", "UnusedMember.Global"), SuppressMessage("ReSharper", "FieldCanBeMadeReadOnly.Global"),
     SuppressMessage("ReSharper", "InconsistentNaming"), SuppressMessage("ReSharper", "MemberCanBeMadeStatic.Global"),
     SuppressMessage("ReSharper", "UnusedMember.Local"), SuppressMessage("ReSharper", "UnusedParameter.Global"),
     SuppressMessage("ReSharper", "UnusedTypeParameter"), SuppressMessage("ReSharper", "MemberCanBeInternal")]
    public sealed class InspectMockObject
    {
        [NotNull] public const string PublicConstField = "public_const_field";
        [NotNull] public static string PublicStaticField = "public_static_field";
        [NotNull] internal string InternalField = "internal_field";
        [NotNull] private string PrivateField = "private_field";
        [NotNull] protected string ProtectedField = "protected_field";

        [NotNull] public string PublicField = "public_field";

        [NotNull]
        public static string PublicStaticProperty => "public_static_property";

        [NotNull]
        public string PublicProperty => "public_property";

        [NotNull]
        internal string InternalProperty => "internal_property";

        [NotNull]
        private string PrivateProperty => "private_property";

        [NotNull]
        protected string ProtectedProperty => "protected_property";

        [NotNull]
        public string this[string index] => "public_property_with_index_argument";

        [NotNull]
        public string PublicMethod()
        {
            return "public_method";
        }

        [NotNull]
        internal string InternalMethod()
        {
            return "internal_method";
        }

        [NotNull]
        private string PrivateMethod()
        {
            return "private_method";
        }

        [NotNull]
        private string ProtectedMethod()
        {
            return "protected_method";
        }

        [NotNull]
        public string PublicMethodWithOneArgument(string arg)
        {
            return "public_method_with_one_argument";
        }

        [NotNull]
        public string PublicGenericMethod<T>()
        {
            return "public_generic_method";
        }
    }
}