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

namespace Abacaxi.Tests.SequenceExtensions
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using NUnit.Framework;

    [TestFixture]
    public sealed class UnzipTests
    {
        [Test]
        public void Unzip2_ReturnsEmptyArrays_IfEmptyInput()
        {
            var empty = new (int, int)[0];
            var (s1, s2) = empty.Unzip();

            TestHelper.AssertSequence(s1);
            TestHelper.AssertSequence(s2);
        }

        [Test]
        public void Unzip2_ReturnsSeparatedElements_ForOneElementInput()
        {
            var input = new[] { (1, "alex") };
            var (s1, s2) = input.Unzip();

            TestHelper.AssertSequence(s1, 1);
            TestHelper.AssertSequence(s2, "alex");
        }

        [Test]
        public void Unzip2_ReturnsSeparatedElements_ForTwoElementInput()
        {
            var input = new[] { (1, "alex"), (2, "john") };
            var (s1, s2) = input.Unzip();

            TestHelper.AssertSequence(s1, 1, 2);
            TestHelper.AssertSequence(s2, "alex", "john");
        }

        [Test, SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void Unzip2_ThrowsException_IfSequenceIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => { (((int, int)[]) null).Unzip(); });
        }

        [Test]
        public void Unzip3_ReturnsEmptyArrays_IfEmptyInput()
        {
            var empty = new (int, int, int)[0];
            var (s1, s2, s3) = empty.Unzip();

            TestHelper.AssertSequence(s1);
            TestHelper.AssertSequence(s2);
            TestHelper.AssertSequence(s3);
        }

        [Test]
        public void Unzip3_ReturnsSeparatedElements_ForOneElementInput()
        {
            var input = new[] { (1, "alex", true) };
            var (s1, s2, s3) = input.Unzip();

            TestHelper.AssertSequence(s1, 1);
            TestHelper.AssertSequence(s2, "alex");
            TestHelper.AssertSequence(s3, true);
        }

        [Test]
        public void Unzip3_ReturnsSeparatedElements_ForTwoElementInput()
        {
            var input = new[] { (1, "alex", true), (2, "john", false) };
            var (s1, s2, s3) = input.Unzip();

            TestHelper.AssertSequence(s1, 1, 2);
            TestHelper.AssertSequence(s2, "alex", "john");
            TestHelper.AssertSequence(s3, true, false);
        }

        [Test, SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void Unzip3_ThrowsException_IfSequenceIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => { (((int, int, int)[])null).Unzip(); });
        }

        [Test]
        public void UnzipKvp_ReturnsEmptyArrays_IfEmptyInput()
        {
            var empty = new KeyValuePair<int, int>[0];
            var (s1, s2) = empty.Unzip();

            TestHelper.AssertSequence(s1);
            TestHelper.AssertSequence(s2);
        }

        [Test]
        public void UnzipKvp_ReturnsSeparatedElements_ForOneElementInput()
        {
            var input = new[] { new KeyValuePair<int, string>(1, "alex") };
            var (s1, s2) = input.Unzip();

            TestHelper.AssertSequence(s1, 1);
            TestHelper.AssertSequence(s2, "alex");
        }

        [Test]
        public void UnzipKvp_ReturnsSeparatedElements_ForTwoElementInput()
        {
            var input = new[] { new KeyValuePair<int, string>(1, "alex"), new KeyValuePair<int, string>(2, "john") };
            var (s1, s2) = input.Unzip();

            TestHelper.AssertSequence(s1, 1, 2);
            TestHelper.AssertSequence(s2, "alex", "john");
        }

        [Test, SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void UnzipKvp_ThrowsException_IfSequenceIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => { ((KeyValuePair<int, string>[])null).Unzip(); });
        }
    }
}