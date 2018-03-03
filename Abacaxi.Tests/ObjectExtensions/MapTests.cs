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
    using System.Diagnostics.CodeAnalysis;

    [TestFixture]
    public sealed class MapTests
    {
        [Test,SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void Map_ThrowsException_IfMapFuncIsNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
                1.Map<int, int>(null));
        }

        [Test,SuppressMessage("ReSharper", "AssignNullToNotNullAttribute"),SuppressMessage("ReSharper", "UnthrowableException")]
        public void Map_ThrowsException_IfMapFuncThrowsIt()
        {
            Assert.Throws<InvalidProgramException>(() =>
                1.Map<int, int>(o => throw new InvalidProgramException()));
        }

        [Test]
        public void Map_CallsTheFunctionAndReturnsTheResult()
        {
            var mapped = 100.Map(o => o.ToString());
            Assert.AreEqual("100", mapped);
        }
    }
}