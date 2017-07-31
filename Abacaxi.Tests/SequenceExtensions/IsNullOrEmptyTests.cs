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

namespace Abacaxi.Tests.SequenceExtensions
{
    using NUnit.Framework;

    [TestFixture]
    public class IsNullOrEmptyTests
    {
        [Test]
        public void IsNullOrEmpty_ReturnsTrue_IfSequenceIsNull()
        {
            var actual = ((int[]) null).IsNullOrEmpty();
            Assert.IsTrue(actual);
        }

        [Test]
        public void IsNullOrEmpty_ReturnsTrue_IfSequenceIsEmpty()
        {
            var actual = new int[]{}.IsNullOrEmpty();
            Assert.IsTrue(actual);
        }

        [Test]
        public void IsNullOrEmpty_ReturnsFalse_IfSequenceIsNotEmpty()
        {
            var actual = new[] { 1 }.IsNullOrEmpty();
            Assert.IsFalse(actual);
        }
    }
}
