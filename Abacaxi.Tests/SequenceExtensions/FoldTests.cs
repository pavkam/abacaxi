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

namespace Abacaxi.Tests.SequenceExtensions
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using NUnit.Framework;

    [TestFixture]
    public sealed class FoldTests
    {
        [Test]
        public void Fold1_DoesNotGroupAll_InSequence()
        {
            var result = new[] {1, 1, 3, 1}.Fold(i => i, (a, b) => a + b, EqualityComparer<int>.Default);
            TestHelper.AssertSequence(result, 2, 3, 1);
        }

        [Test]
        public void Fold1_FoldsComplicatedSequence()
        {
            var result =
                new[] {"aa", "ab", "ac"}.Fold(s => s[0].ToString(), (a, b) => a + b, StringComparer.InvariantCulture);

            // ReSharper disable once StringLiteralTypo
            TestHelper.AssertSequence(result, "aaabac");
        }

        [Test]
        public void Fold1_FoldsSimpleSequence_1()
        {
            var result = new[] {1, 1}.Fold(i => i, (a, b) => a + b, EqualityComparer<int>.Default);
            TestHelper.AssertSequence(result, 2);
        }

        [Test]
        public void Fold1_FoldsSimpleSequence_2()
        {
            var result = new[] {1, 1, 3}.Fold(i => i, (a, b) => a + b, EqualityComparer<int>.Default);
            TestHelper.AssertSequence(result, 2, 3);
        }

        [Test]
        public void Fold1_ReturnsNothing_ForEmptySequence()
        {
            var result = new int[0].Fold(i => i, (a, b) => a + b, EqualityComparer<int>.Default);
            TestHelper.AssertSequence(result);
        }

        [Test]
        public void Fold1_ReturnsSingleElement_ForOneElementSequence()
        {
            var result = new[] {1}.Fold(i => i, (a, b) => a + b, EqualityComparer<int>.Default);
            TestHelper.AssertSequence(result, 1);
        }

        [Test,
         SuppressMessage("ReSharper", "AssignNullToNotNullAttribute"),
         SuppressMessage("ReSharper", "ReturnValueOfPureMethodIsNotUsed")]
        public void Fold1_ThrowsException_ForNullComparer()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new[] {0}.Fold(i => i, (a, b) => a + b, null).ToArray());
        }

        [Test,
         SuppressMessage("ReSharper", "AssignNullToNotNullAttribute"),
         SuppressMessage("ReSharper", "ReturnValueOfPureMethodIsNotUsed")]
        public void Fold1_ThrowsException_ForNullFoldFunc()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new[] {0}.Fold(i => i, null, EqualityComparer<int>.Default).ToArray());
        }

        [Test,
         SuppressMessage("ReSharper", "AssignNullToNotNullAttribute"),
         SuppressMessage("ReSharper", "ReturnValueOfPureMethodIsNotUsed")]
        public void Fold1_ThrowsException_ForNullKeySelector()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new[] {0}.Fold(null, (a, b) => a + b, EqualityComparer<int>.Default).ToArray());
        }

        [Test,
         SuppressMessage("ReSharper", "AssignNullToNotNullAttribute"),
         SuppressMessage("ReSharper", "ReturnValueOfPureMethodIsNotUsed")]
        public void Fold1_ThrowsException_ForNullSequence()
        {
            Assert.Throws<ArgumentNullException>(() =>
                ((int[]) null).Fold(i => i, (a, b) => a + b, EqualityComparer<int>.Default).ToArray());
        }

        [Test]
        public void Fold2_DoesNotGroupAll_InSequence()
        {
            var result = new[] {1, 1, 3, 1}.Fold(i => i, (a, b) => a + b);
            TestHelper.AssertSequence(result, 2, 3, 1);
        }

        [Test]
        public void Fold2_FoldsComplicatedSequence()
        {
            var result =
                new[] {"aa", "ab", "ac"}.Fold(s => s[0].ToString(), (a, b) => a + b);

            // ReSharper disable once StringLiteralTypo
            TestHelper.AssertSequence(result, "aaabac");
        }


        [Test]
        public void Fold2_FoldsSimpleSequence_1()
        {
            var result = new[] {1, 1}.Fold(i => i, (a, b) => a + b);
            TestHelper.AssertSequence(result, 2);
        }

        [Test]
        public void Fold2_FoldsSimpleSequence_2()
        {
            var result = new[] {1, 1, 3}.Fold(i => i, (a, b) => a + b);
            TestHelper.AssertSequence(result, 2, 3);
        }

        [Test]
        public void Fold2_TakesIntoAccountTheDefaultComparer()
        {
            var result =
                new[] {"a", "A"}.Fold(s => s, (a, b) => a + b);

            TestHelper.AssertSequence(result, "a", "A");
        }

        [Test,
         SuppressMessage("ReSharper", "AssignNullToNotNullAttribute"),
         SuppressMessage("ReSharper", "ReturnValueOfPureMethodIsNotUsed")]
        public void Fold2_ThrowsException_ForNullFoldFunc()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new[] {0}.Fold(i => i, null).ToArray());
        }

        [Test,
         SuppressMessage("ReSharper", "AssignNullToNotNullAttribute"),
         SuppressMessage("ReSharper", "ReturnValueOfPureMethodIsNotUsed")]
        public void Fold2_ThrowsException_ForNullKeySelector()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new[] {0}.Fold((Func<int, int>) null, (a, b) => a + b).ToArray());
        }

        [Test,
         SuppressMessage("ReSharper", "AssignNullToNotNullAttribute"),
         SuppressMessage("ReSharper", "ReturnValueOfPureMethodIsNotUsed")]
        public void Fold2_ThrowsException_ForNullSequence()
        {
            Assert.Throws<ArgumentNullException>(() =>
                ((int[]) null).Fold(i => i, (a, b) => a + b).ToArray());
        }
    }
}