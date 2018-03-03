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

namespace Abacaxi.Tests.ObjectExtensions
{
    using System;
    using NUnit.Framework;
    using System.Collections;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;

    [TestFixture]
    public sealed class TryConvertTests
    {
        [Test,SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void TryConvert_ThrowsException_ForNullFormatProvider()
        {
            Assert.Throws<ArgumentNullException>(() =>
                "a".TryConvert(null, out int _));
        }

        [Test]
        public void TryConvert_Takes_CultureIntoAccount()
        {
            Assert.IsTrue("1,1".TryConvert(CultureInfo.GetCultureInfo("ro-RO"), out double value1));
            Assert.IsTrue("1,1".TryConvert(CultureInfo.InvariantCulture, out double value2));

            Assert.AreEqual(1.1, value1);
            Assert.AreEqual(11, value2);
        }

        [Test]
        public void TryConvert_Takes_CultureIntoAccount_WhenConvertingToString()
        {
            Assert.IsTrue(1.1.TryConvert(CultureInfo.GetCultureInfo("ro-RO"), out string value1));
            Assert.IsTrue(1.1.TryConvert(CultureInfo.InvariantCulture, out string value2));

            Assert.AreEqual("1,1", value1);
            Assert.AreEqual("1.1", value2);
        }

        [Test]
        public void TryConvert_UsesInvariantCulture_ByDefault()
        {
            Assert.IsTrue("1,1".TryConvert(CultureInfo.InvariantCulture, out double value2));
            Assert.AreEqual(11, value2);
        }

        [Test]
        public void TryConvert_FailsFor_Null_To_ValueType()
        {
            Assert.IsFalse(((object) null).TryConvert(CultureInfo.InvariantCulture, out int _));
        }

        [Test]
        public void TryConvert_SucceedsFor_Null_To_ClassType()
        {
            Assert.IsTrue(((object) null).TryConvert(CultureInfo.InvariantCulture, out string value));
            Assert.IsNull(value);
        }

        [Test]
        public void TryConvert_SucceedsFor_Null_To_ArrayType()
        {
            Assert.IsTrue(((object) null).TryConvert(CultureInfo.InvariantCulture, out int[] value));
            Assert.IsNull(value);
        }

        [Test]
        public void TryConvert_SucceedsFor_Null_To_NullableType()
        {
            Assert.IsTrue(((object) null).TryConvert(CultureInfo.InvariantCulture, out double? value));
            Assert.IsNull(value);
        }

        [Test]
        public void TryConvert_SucceedsFor_Null_To_InterfaceType()
        {
            Assert.IsTrue(((object) null).TryConvert(CultureInfo.InvariantCulture, out ICloneable value));
            Assert.IsNull(value);
        }

        [Test]
        public void TryConvert_FailsFor_BadString_To_EnumType()
        {
            Assert.IsFalse("fail".TryConvert(CultureInfo.InvariantCulture, out EditOperation _));
        }

        [Test]
        public void TryConvert_FailsFor_BadObject_To_EnumType()
        {
            Assert.IsFalse(this.TryConvert(CultureInfo.InvariantCulture, out EditOperation _));
        }

        [Test]
        public void TryConvert_SucceedsFor_Int_To_EnumType()
        {
            Assert.IsTrue(100.TryConvert(CultureInfo.InvariantCulture, out EditOperation result));
            Assert.AreEqual(100, (int) result);
        }

        [Test]
        public void TryConvert_SucceedsFor_String_To_EnumType()
        {
            Assert.IsTrue("Match".TryConvert(CultureInfo.InvariantCulture, out EditOperation result));
            Assert.AreEqual(EditOperation.Match, result);
        }

        [Test]
        public void TryConvert_SucceedsFor_String_To_Object()
        {
            Assert.IsTrue("string".TryConvert(CultureInfo.InvariantCulture, out object result));
            Assert.AreEqual("string", result);
        }

        [Test]
        public void TryConvert_SucceedsFor_String_To_GoodInterface()
        {
            Assert.IsTrue("string".TryConvert(CultureInfo.InvariantCulture, out IEnumerable result));
            Assert.AreEqual("string", result);
        }

        [Test]
        public void TryConvert_FailsFor_String_To_BadInterface()
        {
            Assert.IsFalse("string".TryConvert(CultureInfo.InvariantCulture, out ICustomFormatter _));
        }

        [Test]
        public void TryConvert_FailsFor_String_To_UnrelatedValueType()
        {
            Assert.IsFalse("string".TryConvert(CultureInfo.InvariantCulture, out Edit<int> _));
        }

        [Test]
        public void TryConvert_SucceedsFor_GoodString_To_ValueType()
        {
            Assert.IsTrue("100".TryConvert(CultureInfo.InvariantCulture, out int result));
            Assert.AreEqual(100, result);
        }

        [Test]
        public void TryConvert_FailsFor_BadString_To_ValueType()
        {
            Assert.IsFalse("string".TryConvert(CultureInfo.InvariantCulture, out double _));
        }

        [Test]
        public void TryConvert_SucceedsFor_SameType()
        {
            Assert.IsTrue(this.TryConvert(CultureInfo.InvariantCulture, out TryConvertTests result));
            Assert.AreSame(this, result);
        }

        [Test]
        public void TryConvert_SucceedsFor_AnyObject_To_String()
        {
            Assert.IsTrue(this.TryConvert(CultureInfo.InvariantCulture, out string result));
            Assert.AreEqual(ToString(), result);
        }

        [Test]
        public void TryConvert_SucceedsFor_ValueType_To_NullableOfSameValueType()
        {
            Assert.IsTrue(1.TryConvert(CultureInfo.InvariantCulture, out int? result));
            Assert.AreEqual(1, result);
        }

        [Test]
        public void TryConvert_SucceedsFor_GoodString_To_NullableOfValueType()
        {
            Assert.IsTrue("100".TryConvert(CultureInfo.InvariantCulture, out double? result));
            Assert.AreEqual(100, result);
        }

        [Test]
        public void TryConvert_SucceedsFor_GoodString_To_NullableEnumType()
        {
            Assert.IsTrue("Match".TryConvert(CultureInfo.InvariantCulture, out EditOperation? result));
            Assert.AreEqual(EditOperation.Match, result);
        }
    }
}