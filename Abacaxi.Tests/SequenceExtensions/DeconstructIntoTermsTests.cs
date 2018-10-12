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
    using JetBrains.Annotations;
    using NUnit.Framework;

    [TestFixture]
    public sealed class DeconstructIntoTermsTests
    {
        [NotNull, SuppressMessage("ReSharper", "StringLiteralTypo")]
        private static readonly HashSet<string> Words = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase)
        {
            "a",
            "aa",
            "b",
            "aba",
            "abadaba"
        };

        private static double ScoreWord(IList<char> sequence, int index, int length)
        {
            var term = new char[length];
            for (var i = 0; i < length; i++)
            {
                term[i] = sequence[i + index];
            }

            var word = new string(term);
            if (word == "g")
            {
                return double.NaN;
            }

            return Words.Contains(word) ? 1000 * word.Length : word.Length;
        }

        [Test]
        public void DeconstructIntoTerms_RefusesATerm()
        {
            var split = "g".AsList().DeconstructIntoTerms(ScoreWord).Select(s => new string(s));

            TestHelper.AssertSequence(split);
        }

        [Test]
        public void DeconstructIntoTerms_ReturnsNothingForEmptyString()
        {
            var split = "".AsList().DeconstructIntoTerms(ScoreWord).Select(s => new string(s));

            TestHelper.AssertSequence(split);
        }

        [Test]
        public void DeconstructIntoTerms_ReturnsTheWholeThingIfNotMatched()
        {
            var split = "1234567890".AsList().DeconstructIntoTerms(ScoreWord).Select(s => new string(s));

            TestHelper.AssertSequence(split, "1234567890");
        }

        [Test]
        public void DeconstructIntoTerms_SelectsTheBestScoredVersions()
        {
            var split = "aaa".AsList().DeconstructIntoTerms(ScoreWord).Select(s => new string(s));

            TestHelper.AssertSequence(split, "a", "aa");
        }

        [Test]
        public void DeconstructIntoTerms_SelectsTheSingleBestScoredVersion()
        {
            var split = "aa".AsList().DeconstructIntoTerms(ScoreWord).Select(s => new string(s));

            TestHelper.AssertSequence(split, "aa");
        }

        [Test]
        public void DeconstructIntoTerms_SplitsAccordingToScoreFunc()
        {
            var split = "aza".AsList().DeconstructIntoTerms(ScoreWord).Select(s => new string(s));

            TestHelper.AssertSequence(split, "a", "z", "a");
        }

        [Test]
        public void DeconstructIntoTerms_SplitsByKnownTerms()
        {
            var split = "ThisIsAGoodPhraseToSplitBecauseItContainsTheWords".AsList().DeconstructIntoTerms(ScoreWord)
                .Select(s => new string(s));

            TestHelper.AssertSequence(split,
                "ThisIs",
                "A",
                "GoodPhr",
                "a",
                "seToSplit",
                "B",
                "ec",
                "a",
                "useItCont",
                "a",
                "insTheWords"
            );
        }

        [Test, SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void DeconstructIntoTerms_ThrowsException_ForNullFunc()
        {
            Assert.Throws<ArgumentNullException>(() => new int[] { }.DeconstructIntoTerms(null));
        }

        [Test, SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void DeconstructIntoTerms_ThrowsException_ForNullSequence()
        {
            Assert.Throws<ArgumentNullException>(() => ((int[]) null).DeconstructIntoTerms((l, s, e) => 0));
        }

        [Test, SuppressMessage("ReSharper", "StringLiteralTypo")]
        public void DeconstructIntoTerms_WillDefinitelyMatchTheLongestThing()
        {
            var split = "abadaba".AsList().DeconstructIntoTerms(ScoreWord).Select(s => new string(s));

            TestHelper.AssertSequence(split, "abadaba");
        }
    }
}