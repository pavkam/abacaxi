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
    using NUnit.Framework;

    [TestFixture]
    public sealed class AppendTests
    {
        [Test]
        public void Append_CreatesNewArray_ForFullArray_AndFiveElements()
        {
            var array = new[] {-2, -1, 0}.Append(1, 2, 3, 4, 5);

            TestHelper.AssertSequence(array, -2, -1, 0, 1, 2, 3, 4, 5);
        }

        [Test]
        public void Append_CreatesNewArray_ForFullArray_AndFourElements()
        {
            var array = new[] {-2, -1, 0}.Append(1, 2, 3, 4);

            TestHelper.AssertSequence(array, -2, -1, 0, 1, 2, 3, 4);
        }

        [Test]
        public void Append_CreatesNewArray_ForFullArray_AndOneElement()
        {
            var array = new[] {-2, -1, 0}.Append(1);

            TestHelper.AssertSequence(array, -2, -1, 0, 1);
        }

        [Test]
        public void Append_CreatesNewArray_ForFullArray_AndSixElements()
        {
            var array = new[] {-2, -1, 0}.Append(1, 2, 3, 4, 5, 6);

            TestHelper.AssertSequence(array, -2, -1, 0, 1, 2, 3, 4, 5, 6);
        }

        [Test]
        public void Append_CreatesNewArray_ForFullArray_AndThreeElements()
        {
            var array = new[] {-2, -1, 0}.Append(1, 2, 3);

            TestHelper.AssertSequence(array, -2, -1, 0, 1, 2, 3);
        }

        [Test]
        public void Append_CreatesNewArray_ForFullArray_AndTwoElements()
        {
            var array = new[] {-2, -1, 0}.Append(1, 2);

            TestHelper.AssertSequence(array, -2, -1, 0, 1, 2);
        }

        [Test]
        public void Append_CreatesNewArray_ForNullArray_AndFiveElements()
        {
            var array = ((int[]) null).Append(1, 2, 3, 4, 5);

            TestHelper.AssertSequence(array, 1, 2, 3, 4, 5);
        }

        [Test]
        public void Append_CreatesNewArray_ForNullArray_AndFourElements()
        {
            var array = ((int[]) null).Append(1, 2, 3, 4);

            TestHelper.AssertSequence(array, 1, 2, 3, 4);
        }

        [Test]
        public void Append_CreatesNewArray_ForNullArray_AndOneElement()
        {
            var array = ((int[]) null).Append(1);

            TestHelper.AssertSequence(array, 1);
        }

        [Test]
        public void Append_CreatesNewArray_ForNullArray_AndSixElements()
        {
            var array = ((int[]) null).Append(1, 2, 3, 4, 5, 6);

            TestHelper.AssertSequence(array, 1, 2, 3, 4, 5, 6);
        }

        [Test]
        public void Append_CreatesNewArray_ForNullArray_AndThreeElements()
        {
            var array = ((int[]) null).Append(1, 2, 3);

            TestHelper.AssertSequence(array, 1, 2, 3);
        }

        [Test]
        public void Append_CreatesNewArray_ForNullArray_AndTwoElements()
        {
            var array = ((int[]) null).Append(1, 2);

            TestHelper.AssertSequence(array, 1, 2);
        }
    }
}