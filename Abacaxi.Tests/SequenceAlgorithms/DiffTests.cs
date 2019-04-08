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

namespace Abacaxi.Tests.SequenceAlgorithms
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Text;
    using JetBrains.Annotations;
    using NUnit.Framework;
    using SequenceAlgorithms = Abacaxi.SequenceAlgorithms;

    [TestFixture]
    public sealed class DiffTests
    {
        [TestCase("", "", ""), TestCase("a", "a", "=a"), TestCase("a", "b", "*b"), TestCase("", "a", "+a"),
         TestCase("a", "", "-a"), TestCase("ab", "a", "=a-b"), TestCase("a", "ab", "=a+b"),
         TestCase("ab", "ba", "*b*a"),
         TestCase("hello my dear friend", "Hello you fiend!", "*H=e=l=l=o= -m=y- -d-e*o*u= =f-r=i=e=n=d+!")]
        public void Diff_ReturnsExpectedSequence([NotNull] string s1, [NotNull] string s2, string expected)
        {
            var result = new StringBuilder();
            foreach (var e in s1.AsList().Diff(s2.AsList()))
            {
                switch (e.Operation)
                {
                    case EditOperation.Match:
                        result.Append($"={e.Item}");
                        break;
                    case EditOperation.Substitute:
                        result.Append($"*{e.Item}");
                        break;
                    case EditOperation.Insert:
                        result.Append($"+{e.Item}");
                        break;
                    case EditOperation.Delete:
                        result.Append($"-{e.Item}");
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            Assert.AreEqual(expected, result.ToString());
        }

        [Test, SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void Diff_ThrowsException_ForNullResultSequence()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new char[] { }.Diff(null));
        }

        [Test, SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void Diff_ThrowsException_ForNullSequence()
        {
            Assert.Throws<ArgumentNullException>(() =>
                SequenceAlgorithms.Diff(null, new char[] { }));
        }
    }
}