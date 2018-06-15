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

namespace Abacaxi.Tests.Knapsack
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using NUnit.Framework;
    using Knapsack = Abacaxi.Knapsack;

    [TestFixture]
    public class FillTests
    {
        [Test]
        public void Fill_FitsTwoItems_1()
        {
            var array = new[]
            {
                new KnapsackItem<string>("item1", 1, 5),
                new KnapsackItem<string>("item2", 1, 5)
            };
            var result = Knapsack.Fill(array, 10);
            TestHelper.AssertSequence(result, "item2", "item1");
        }

        [Test]
        public void Fill_FitsTwoItems_2()
        {
            var array = new[]
            {
                new KnapsackItem<string>("item1", 1, 5),
                new KnapsackItem<string>("item2", 1, 4),
                new KnapsackItem<string>("item3", 1, 3)
            };
            var result = Knapsack.Fill(array, 10);
            TestHelper.AssertSequence(result, "item2", "item3");
        }

        [Test]
        public void Fill_ReturnsNothing_ForEmptySequence()
        {
            var result = Knapsack.Fill(new List<KnapsackItem<int>>(), 1);
            TestHelper.AssertSequence(result);
        }

        [Test]
        public void Fill_ReturnsNothingForSingleItem_IfCannotBeFitted()
        {
            var array = new[] {new KnapsackItem<string>("item1", 1, 10)};
            var result = Knapsack.Fill(array, 9);
            TestHelper.AssertSequence(result);
        }

        [Test]
        public void Fill_ReturnsSingleItem_IfCanBeFitted()
        {
            var array = new[] {new KnapsackItem<string>("item1", 1, 1)};
            var result = Knapsack.Fill(array, 1);
            TestHelper.AssertSequence(result, "item1");
        }

        [Test]
        public void Fill_SelectsTheMostValuedItem()
        {
            var array = new[]
            {
                new KnapsackItem<string>("item1", 1, 1),
                new KnapsackItem<string>("item2", 2, 1)
            };
            var result = Knapsack.Fill(array, 1);
            TestHelper.AssertSequence(result, "item2");
        }

        [Test, SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void Fill_ThrowsException_ForNullSequence()
        {
            Assert.Throws<ArgumentNullException>(
                () => Knapsack.Fill<int>(null, 1));
        }

        [Test]
        public void Fill_ThrowsException_ForZeroOrLessKnapsackWeight()
        {
            Assert.Throws<ArgumentOutOfRangeException>(
                () => Knapsack.Fill(new List<KnapsackItem<int>>(), 0));
        }

        [Test]
        public void Fill_WillChoseMoreItemsIfTheTotalValueIsMaximized()
        {
            var array = new[]
            {
                new KnapsackItem<string>("item1", 2, 10),
                new KnapsackItem<string>("item2", 1, 3),
                new KnapsackItem<string>("item3", 1, 3),
                new KnapsackItem<string>("item4", 1, 4)
            };
            var result = Knapsack.Fill(array, 10);
            TestHelper.AssertSequence(result, "item4", "item3", "item2");
        }

        [Test]
        public void Fill_WillChoseSingleItemIfValueCannotBeMaximized()
        {
            var array = new[]
            {
                new KnapsackItem<string>("item1", 4, 10),
                new KnapsackItem<string>("item2", 1, 3),
                new KnapsackItem<string>("item3", 1, 3),
                new KnapsackItem<string>("item4", 1, 4)
            };
            var result = Knapsack.Fill(array, 10);
            TestHelper.AssertSequence(result, "item1");
        }
    }
}