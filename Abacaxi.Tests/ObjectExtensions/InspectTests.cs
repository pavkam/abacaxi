/* Copyright 2017-2019 by Alexandru Ciobanu (alex+git@ciobanu.org)
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
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using NUnit.Framework;

    [TestFixture]
    public sealed class InspectTests
    {
        private static readonly InspectMockObject Mock = new InspectMockObject();

        private static KeyValuePair<string, object> Kvp(string key, object value)
        {
            return new KeyValuePair<string, object>(key, value);
        }

        [Test]
        public void Inspect_ReturnsAll_IfChosenTo()
        {
            var dictionary = Mock.Inspect(InspectionFlags.IncludeAll).OrderBy(s => s.Key).AsList();

            TestHelper.AssertSequence(dictionary,
                Kvp(nameof(GetHashCode), Mock.GetHashCode()),
                Kvp(nameof(GetType), Mock.GetType()),
                Kvp(nameof(Mock.PublicField), Mock.PublicField),
                Kvp(nameof(Mock.PublicMethod), Mock.PublicMethod()),
                Kvp(nameof(Mock.PublicProperty), Mock.PublicProperty),
                Kvp(nameof(ToString), Mock.ToString())
            );
        }

        [Test]
        public void Inspect_ReturnsAllTypesOfProperties()
        {
            var dictionary = new
                {
                    I = 100,
                    S = "S",
                    B = true,
                    T = this
                }.Inspect()
                .OrderBy(s => s.Key)
                .AsList();

            TestHelper.AssertSequence(dictionary,
                Kvp("B", true),
                Kvp("I", 100),
                Kvp("S", "S"),
                Kvp("T", this)
            );
        }

        [Test]
        public void Inspect_ReturnsNothing_IfFlagsIsZero()
        {
            var dictionary = Mock.Inspect(0).OrderBy(s => s.Key).AsList();

            TestHelper.AssertSequence(dictionary);
        }

        [Test]
        public void Inspect_ReturnsOnlyFields_IfChosenTo()
        {
            var dictionary = Mock.Inspect(InspectionFlags.IncludeFields).OrderBy(s => s.Key).AsList();

            TestHelper.AssertSequence(dictionary,
                Kvp(nameof(Mock.PublicField), Mock.PublicField)
            );
        }

        [Test]
        public void Inspect_ReturnsOnlyMethods_IfChosenTo()
        {
            var dictionary = Mock.Inspect(InspectionFlags.IncludeMethods).OrderBy(s => s.Key).AsList();

            TestHelper.AssertSequence(dictionary,
                Kvp(nameof(GetHashCode), Mock.GetHashCode()),
                Kvp(nameof(GetType), Mock.GetType()),
                Kvp(nameof(Mock.PublicMethod), Mock.PublicMethod()),
                Kvp(nameof(ToString), Mock.ToString())
            );
        }

        [Test]
        public void Inspect_ReturnsOnlyProperties_IfChosenTo()
        {
            var dictionary = Mock.Inspect().OrderBy(s => s.Key).AsList();

            TestHelper.AssertSequence(dictionary,
                Kvp(nameof(Mock.PublicProperty), Mock.PublicProperty)
            );
        }

        [Test, SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void Inspect_ThrowsException_ForNullValue()
        {
            Assert.Throws<ArgumentNullException>(() =>
                ((object) null).Inspect());
        }
    }
}