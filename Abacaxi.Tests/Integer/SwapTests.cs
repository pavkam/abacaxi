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

namespace Abacaxi.Tests.Integer
{
    using NUnit.Framework;
    using Integer = Practice.Integer;

    [TestFixture]
    public sealed class SwapTests
    {
        [Test]
        public void Swap_DoesNothing_IfValuesAreEqual()
        {
            var a = 100;
            var b = 100;

            Integer.Swap(ref a, ref b);

            Assert.That(a == b && a == 100);
        }

        [Test]
        public void Swap_SwapsTwoPositiveValues()
        {
            var a = int.MaxValue;
            var b = int.MaxValue / 2;

            Integer.Swap(ref a, ref b);

            Assert.That(a == int.MaxValue / 2 && b == int.MaxValue);
        }

        [Test]
        public void Swap_SwapsTwoNegativeValues()
        {
            var a = int.MinValue;
            var b = int.MinValue / 2;

            Integer.Swap(ref a, ref b);

            Assert.That(a == int.MinValue / 2 && b == int.MinValue);
        }

        [Test]
        public void Swap_SwapsNegativeAndPositiveValues()
        {
            var a = int.MaxValue;
            var b = int.MinValue;

            Integer.Swap(ref a, ref b);

            Assert.That(a == int.MinValue && b == int.MaxValue);
        }
    }
}