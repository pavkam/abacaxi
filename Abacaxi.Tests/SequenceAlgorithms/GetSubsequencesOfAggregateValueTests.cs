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

namespace Abacaxi.Tests.SequenceAlgorithms
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using NUnit.Framework;

    [TestFixture]
    public sealed class GetSubsequencesOfAggregateValueTests
    {
        private static int IntegerAggregator(int a, int b)
        {
            return a + b;
        }

        private static int IntegerDisaggregator(int a, int b)
        {
            return a - b;
        }

        [Test]
        public void GetSubsequencesOfAggregateValue_ReturnsAll_ForNeutrals()
        {
            var array = new[] {1, 0, 0};
            TestHelper.AssertSequence(
                array.GetSubsequencesOfAggregateValue(IntegerAggregator, IntegerDisaggregator,
                    EqualityComparer<int>.Default, 1),
                new[] {1},
                new[] {1, 0},
                new[] {1, 0, 0});
        }

        [Test]
        public void GetSubsequencesOfAggregateValue_ReturnsAllExpectedSubsequences_AllNegative()
        {
            var array = new[] {-1, -2, -3, -4, -1, -1, -6, -1, -1, -2, -1, -1, -1, -3, -3};
            var result = array
                .GetSubsequencesOfAggregateValue(IntegerAggregator, IntegerDisaggregator, EqualityComparer<int>.Default,
                    -6)
                .ToArray();

            TestHelper.AssertSequence(
                result,
                new[] {-1, -2, -3},
                new[] {-4, -1, -1},
                new[] {-6},
                new[] {-1, -1, -2, -1, -1},
                new[] {-1, -2, -1, -1, -1},
                new[] {-1, -1, -1, -3},
                new[] {-3, -3});
        }

        [Test]
        public void GetSubsequencesOfAggregateValue_ReturnsAllExpectedSubsequences_AllPositive()
        {
            var array = new[] {1, 2, 3, 4, 1, 1, 6, 1, 1, 2, 1, 1, 1, 3, 3};
            var result = array
                .GetSubsequencesOfAggregateValue(IntegerAggregator, IntegerDisaggregator, EqualityComparer<int>.Default,
                    6)
                .ToArray();

            TestHelper.AssertSequence(
                result,
                new[] {1, 2, 3},
                new[] {4, 1, 1},
                new[] {6},
                new[] {1, 1, 2, 1, 1},
                new[] {1, 2, 1, 1, 1},
                new[] {1, 1, 1, 3},
                new[] {3, 3});
        }

        [Test]
        public void GetSubsequencesOfAggregateValue_ReturnsAllExpectedSubsequences_MixedSigns()
        {
            var array = new[] {-1, 2, -3, 4, 1, 1, -6, 1, -1, -2, 1, -1, 2, 3, -3};
            var result = array
                .GetSubsequencesOfAggregateValue(IntegerAggregator, IntegerDisaggregator, EqualityComparer<int>.Default,
                    3)
                .ToArray();

            TestHelper.AssertSequence(
                result,
                new[] {2, -3, 4},
                new[] {-1, 2, -3, 4, 1},
                new[] {-3, 4, 1, 1},
                new[] {4, 1, 1, -6, 1, -1, -2, 1, -1, 2, 3},
                new[] {1, -1, -2, 1, -1, 2, 3},
                new[] {-2, 1, -1, 2, 3},
                new[] {3});
        }

        [Test]
        public void GetSubsequencesOfAggregateValue_ReturnsAllSequence_IfAggregates()
        {
            var array = new[] {1, 2, 3, 4, 5, 6};
            TestHelper.AssertSequence(
                array.GetSubsequencesOfAggregateValue(IntegerAggregator, IntegerDisaggregator,
                    EqualityComparer<int>.Default, 21),
                new[] {1, 2, 3, 4, 5, 6});
        }

        [Test]
        public void GetSubsequencesOfAggregateValue_ReturnsNothing_ForEmptySequence()
        {
            var array = new int[] { };
            TestHelper.AssertSequence(
                array.GetSubsequencesOfAggregateValue(IntegerAggregator, IntegerDisaggregator,
                    EqualityComparer<int>.Default, 1)
            );
        }

        [Test]
        public void GetSubsequencesOfAggregateValue_ReturnsNothing_IfDoesNotAggregate()
        {
            var array = new[] {1, 2, 3, 4, 5, 6};
            TestHelper.AssertSequence(
                array.GetSubsequencesOfAggregateValue(IntegerAggregator, IntegerDisaggregator,
                    EqualityComparer<int>.Default, 8));
        }

        [Test]
        public void GetSubsequencesOfAggregateValue_ReturnsSingleElement_IfAggregates()
        {
            var array = new[] {1};
            TestHelper.AssertSequence(
                array.GetSubsequencesOfAggregateValue(IntegerAggregator, IntegerDisaggregator,
                    EqualityComparer<int>.Default, 1),
                new[] {1});
        }

        [Test]
        public void GetSubsequencesOfAggregateValue_ReturnsTwoSequences_IfAggregates()
        {
            var array = new[] {1, 2, 3, 4, 5, 6};
            TestHelper.AssertSequence(
                array.GetSubsequencesOfAggregateValue(IntegerAggregator, IntegerDisaggregator,
                    EqualityComparer<int>.Default, 6),
                new[] {1, 2, 3},
                new[] {6});
        }

        [Test, SuppressMessage("ReSharper", "IteratorMethodResultIsIgnored"),
         SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void GetSubsequencesOfAggregateValue_ThrowsException_IfAggregatorIsNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new[] {1}.GetSubsequencesOfAggregateValue(null, IntegerDisaggregator, EqualityComparer<int>.Default,
                    1));
        }

        [Test, SuppressMessage("ReSharper", "IteratorMethodResultIsIgnored"),
         SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void GetSubsequencesOfAggregateValue_ThrowsException_IfComparerIsNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new[] {1}.GetSubsequencesOfAggregateValue(IntegerAggregator, IntegerDisaggregator, null, 1));
        }

        [Test, SuppressMessage("ReSharper", "IteratorMethodResultIsIgnored"),
         SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void GetSubsequencesOfAggregateValue_ThrowsException_IfDisaggregatorIsNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new[] {1}.GetSubsequencesOfAggregateValue(IntegerAggregator, null, EqualityComparer<int>.Default,
                    1));
        }

        [Test, SuppressMessage("ReSharper", "IteratorMethodResultIsIgnored"),
         SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void GetSubsequencesOfAggregateValue_ThrowsException_IfSequenceIsNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
                ((int[]) null).GetSubsequencesOfAggregateValue(IntegerAggregator, IntegerDisaggregator,
                    EqualityComparer<int>.Default, 1));
        }
    }
}