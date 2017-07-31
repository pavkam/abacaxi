/* Copyright 2017 by Alexandru Ciobanu (alex+git@ciobanu.org)
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

#pragma warning disable 169
#pragma warning disable 414

namespace Abacaxi.Tests.ObjectExtensions
{
    using System.Diagnostics.CodeAnalysis;

    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "FieldCanBeMadeReadOnly.Global")]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "MemberCanBeMadeStatic.Global")]
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    [SuppressMessage("ReSharper", "UnusedParameter.Global")]
    [SuppressMessage("ReSharper", "UnusedTypeParameter")]
    public class InspectMockObject
    {
        public const string PublicConstField = "public_const_field";
        public static string PublicStaticField = "public_static_field";
        public static string PublicStaticProperty => "public_static_property";

        public string PublicField = "public_field";
        internal string InternalField = "internal_field";
        private string PrivateField = "private_field";
        protected string ProtectedField = "protected_field";

        public string PublicProperty => "public_property";
        internal string InternalProperty => "internal_property";
        private string PrivateProperty => "private_property";
        protected string ProtectedProperty => "protected_property";

        public string PublicMethod() => "public_method";
        internal string InternalMethod() => "internal_method";
        private string PrivateMethod() => "private_method";
        protected string ProtectedMethod() => "protected_method";

        public string PublicMethodWithOneArgument(string arg) => "public_method_with_one_argument";
        public string this[string index] => "public_property_with_index_argument";

        public string PublicGenericMethod<T>() => "public_generic_method";
    }        
}
