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

namespace Abacaxi.Tests.Containers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Abacaxi.Containers;
    using JetBrains.Annotations;
    using NUnit.Framework;

    [TestFixture]
    public sealed class DependencySquidTests
    {
        [SetUp]
        public void SetUp()
        {
            _empty = new DependencySquid<string>();
        }

        private DependencySquid<string> _empty;

        private static void AssertSet<T>([NotNull] IEnumerable<T> set, [NotNull] params T[] items)
        {
            if (items.Length == 0)
            {
                Assert.IsEmpty(set);
            }
            else
            {
                var ordered1 = set.OrderBy(k => k).ToArray();
                var ordered2 = items.OrderBy(k => k).ToArray();

                TestHelper.AssertSequence(ordered1, ordered2);
            }
        }

        [Test,
         SuppressMessage("ReSharper", "AssignNullToNotNullAttribute"),
         SuppressMessage("ReSharper", "ObjectCreationAsStatement")]
        public void Constructor_ThrowsException_ForNullEqualityComparer()
        {
            Assert.Throws<ArgumentNullException>(() => new DependencySquid<string>(null));
        }

        [Test]
        public void Selection_IsEmpty_ByDefault()
        {
            Assert.AreEqual(0, _empty.Selection.Length);
        }

        [Test,SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void AddDependencies_ThrowsException_ForNullDependencies()
        {
            Assert.Throws<ArgumentNullException>(() => _empty.AddDependencies("1", null));
        }

        [Test, SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void AddConflicts_ThrowsException_ForNullDependencies()
        {
            Assert.Throws<ArgumentNullException>(() => _empty.AddConflicts("1", null));
        }

        [Test, SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void RemoveDependencies_ThrowsException_ForNullDependencies()
        {
            Assert.Throws<ArgumentNullException>(() => _empty.RemoveDependencies("1", null));
        }

        [Test, SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void RemoveConflicts_ThrowsException_ForNullDependencies()
        {
            Assert.Throws<ArgumentNullException>(() => _empty.RemoveConflicts("1", null));
        }

        [Test, SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void AddDependencies_ThrowsException_ForNullDependent()
        {
            Assert.Throws<ArgumentNullException>(() => _empty.AddDependencies(null, "1"));
        }

        [Test, SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void AddConflicts_ThrowsException_ForNullDependent()
        {
            Assert.Throws<ArgumentNullException>(() => _empty.AddConflicts(null, "1"));
        }

        [Test, SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void RemoveDependencies_ThrowsException_ForNullDependent()
        {
            Assert.Throws<ArgumentNullException>(() => _empty.RemoveDependencies(null, "1"));
        }

        [Test, SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void RemoveConflicts_ThrowsException_ForNullDependent()
        {
            Assert.Throws<ArgumentNullException>(() => _empty.RemoveConflicts(null, "1"));
        }

        [Test, SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void Toggle_ThrowsException_ForNullTag()
        {
            Assert.Throws<ArgumentNullException>(() => _empty.Toggle(null, true));
        }

        [Test]
        public void Toggle_AddsTagIfDoesNotExist()
        {
            _empty.Toggle("A", true);
            AssertSet(_empty.Selection, "A");
        }

        [Test]
        public void EqualityComparer_IsTakenIntoAccount()
        {
            var squid = new DependencySquid<string>(StringComparer.OrdinalIgnoreCase);

            squid.AddDependencies("A", "b");
            squid.AddDependencies("B", "c", "D", "E");
            squid.RemoveDependencies("B", "e");

            squid.Toggle("a", true);
            AssertSet(squid.Selection, "A", "b", "c", "D");
        }

        [Test]
        public void Toggle_DoesNothingIfTheTagDoesNotExist_AndRemoving()
        {
            _empty.Toggle("A", true);
            _empty.Toggle("B", false);

            AssertSet(_empty.Selection, "A");
        }

        [Test]
        public void Toggle_UnSelectsTagIfPreviouslySelected()
        {
            _empty.Toggle("A", true);
            _empty.Toggle("A", false);

            AssertSet(_empty.Selection);
        }

        [Test]
        public void Clear_ClearsTheSelection()
        {
            _empty.AddDependencies("A", "B");
            _empty.Toggle("A", true);
            _empty.Clear();

            AssertSet(_empty.Selection);
        }

        [Test]
        public void Dependencies_WorkAsExpected_ForCase1()
        {
            _empty.AddDependencies("A", "A");
            _empty.Toggle("A", true);
            AssertSet(_empty.Selection, "A");

            _empty.Toggle("A", false);
            AssertSet(_empty.Selection);

            _empty.AddDependencies("A", "B");
            _empty.Toggle("A", true);
            AssertSet(_empty.Selection, "A", "B");

            _empty.Toggle("A", false);
            AssertSet(_empty.Selection, "B");

            _empty.Toggle("B", false);
            AssertSet(_empty.Selection);

            _empty.Toggle("B", true);
            AssertSet(_empty.Selection, "B");

            _empty.Toggle("B", true);
            AssertSet(_empty.Selection, "B");
        }

        [Test]
        public void Dependencies_WorkAsExpected_ForCase2()
        {
            _empty.AddDependencies("A", "B");
            _empty.AddDependencies("B", "C");
            _empty.AddDependencies("C", "D");
            _empty.AddDependencies("C", "E");

            _empty.Toggle("A", true);
            AssertSet(_empty.Selection, "A", "B", "C", "D", "E");

            _empty.Toggle("C", false);
            AssertSet(_empty.Selection, "E", "D");

            _empty.Toggle("E", false);
            AssertSet(_empty.Selection, "D");

            _empty.Toggle("D", false);
            _empty.Toggle("C", true);
            AssertSet(_empty.Selection, "C", "D", "E");

            _empty.Toggle("E", false);
            AssertSet(_empty.Selection, "D");
        }

        [Test]
        public void Dependencies_WorkAsExpected_ForCase3()
        {
            _empty.AddDependencies("A", "C");
            _empty.AddDependencies("B", "C");
            _empty.AddDependencies("A", "C");

            _empty.Toggle("A", true);
            AssertSet(_empty.Selection, "A", "C");

            _empty.Toggle("B", true);
            AssertSet(_empty.Selection, "A", "B", "C");

            _empty.Toggle("C", false);
            AssertSet(_empty.Selection);
        }

        [Test]
        public void Dependencies_WorkAsExpected_ForCase4()
        {
            _empty.AddDependencies("A", "B");
            _empty.AddDependencies("C", "D");

            _empty.Toggle("A", true);
            _empty.AddDependencies("A", "C");
            AssertSet(_empty.Selection, "A", "B", "C", "D");

            _empty.AddDependencies("A", "E");
            AssertSet(_empty.Selection, "A", "B", "C", "D", "E");

            _empty.Toggle("C", false);
            AssertSet(_empty.Selection, "B", "D", "E");
        }

        [Test]
        public void Dependencies_WorkAsExpected_ForCase5()
        {
            _empty.AddDependencies("A", "B");
            _empty.Toggle("A", true);
            _empty.RemoveDependencies("A", "B");
            AssertSet(_empty.Selection, "A", "B");
        }

        [Test]
        public void Dependencies_WorkAsExpected_ForCase6()
        {
            _empty.AddDependencies("A", "B", "C", "D");
            _empty.AddDependencies("D", "A");
            _empty.AddDependencies("C", "D");
            _empty.AddDependencies("B", "D");
            _empty.Toggle("A", true);

            AssertSet(_empty.Selection, "A", "B", "C", "D");
            _empty.Toggle("D", false);

            AssertSet(_empty.Selection);
        }

        [Test]
        public void Dependencies_WorkAsExpected_ForCase7()
        {
            _empty.AddDependencies("A", "B");
            _empty.AddDependencies("B", "A");

            _empty.Toggle("A", true);
            AssertSet(_empty.Selection, "A", "B");

            _empty.RemoveDependencies("B", "A");
            _empty.RemoveDependencies("A", "B");

            _empty.Toggle("A", false);
            AssertSet(_empty.Selection, "B");

            _empty.Toggle("B", false);
            AssertSet(_empty.Selection);
        }

        [Test]
        public void Dependencies_WorkAsExpected_ForCase8()
        {
            _empty.AddDependencies("A", "B");
            _empty.AddDependencies("B", "C");
            _empty.AddDependencies("C", "D");

            _empty.Toggle("A", true);
            _empty.RemoveDependencies("B", "C");
            _empty.Toggle("D", false);

            AssertSet(_empty.Selection, "A", "B");
        }

        [Test]
        public void Dependencies_WorkAsExpected_ForCase9()
        {
            _empty.AddDependencies("A", "A");
            _empty.Toggle("A", true);
            AssertSet(_empty.Selection, "A");
        }

        [Test]
        public void Conflicts_WorkAsExpected_ForCase1()
        {
            _empty.AddConflicts("A", "A");
            _empty.Toggle("A", true);
            AssertSet(_empty.Selection);
        }

        [Test]
        public void Conflicts_WorkAsExpected_ForCase2()
        {
            _empty.AddConflicts("A", "B");

            _empty.Toggle("A", true);
            AssertSet(_empty.Selection, "A");

            _empty.Toggle("B", true);
            AssertSet(_empty.Selection, "B");
        }

        [Test]
        public void Conflicts_WorkAsExpected_ForCase3()
        {
            _empty.AddDependencies("A", "B");
            _empty.AddConflicts("B", "C");

            _empty.Toggle("C", true);
            AssertSet(_empty.Selection, "C");

            _empty.Toggle("A", true);
            AssertSet(_empty.Selection, "A", "B");
        }

        [Test]
        public void Conflicts_WorkAsExpected_ForCase4()
        {
            _empty.AddConflicts("A", "B");

            _empty.Toggle("A", true);
            AssertSet(_empty.Selection, "A");

            _empty.Toggle("B", true);
            AssertSet(_empty.Selection, "B");
        }

        [Test]
        public void Conflicts_WorkAsExpected_ForCase5()
        {
            _empty.AddDependencies("A", "B");
            _empty.AddDependencies("B", "C");
            _empty.AddDependencies("B", "G");
            _empty.AddDependencies("F", "E");
            _empty.AddDependencies("E", "D");
            _empty.AddDependencies("E", "H");
            _empty.AddConflicts("C", "D");

            _empty.Toggle("A", true);
            AssertSet(_empty.Selection, "A", "B", "C", "G");

            _empty.Toggle("H", true);
            AssertSet(_empty.Selection, "A", "B", "C", "G", "H");

            _empty.Toggle("E", true);
            AssertSet(_empty.Selection, "D", "E", "G", "H");

            _empty.Toggle("C", true);
            AssertSet(_empty.Selection, "C", "G", "H");

            _empty.AddConflicts("C", "H");
            AssertSet(_empty.Selection, "C", "G");

            _empty.Toggle("A", true);
            AssertSet(_empty.Selection, "A", "B", "C", "G");

            _empty.Toggle("F", true);
            AssertSet(_empty.Selection, "D", "E", "F", "G", "H");
        }

        [Test]
        public void Conflicts_WorkAsExpected_ForCase6()
        {
            _empty.AddDependencies("A", "B");
            _empty.AddDependencies("B", "C");
            _empty.AddConflicts("A", "C");

            _empty.Toggle("A", true);
            AssertSet(_empty.Selection);

            _empty.Toggle("B", true);
            AssertSet(_empty.Selection, "B", "C");

            _empty.Toggle("C", false);
            AssertSet(_empty.Selection);

            _empty.Toggle("C", true);
            AssertSet(_empty.Selection, "C");
        }

        [Test]
        public void Conflicts_WorkAsExpected_ForCase7()
        {
            _empty.AddDependencies("A", "B");
            _empty.AddDependencies("B", "C");
            _empty.AddConflicts("A", "C");

            _empty.Toggle("A", true);
            _empty.RemoveConflicts("A", "C");
            AssertSet(_empty.Selection);

            _empty.Toggle("A", true);
            AssertSet(_empty.Selection, "A", "B", "C");
        }

        [Test]
        public void Conflicts_WorkAsExpected_ForCase8()
        {
            _empty.Toggle("A", true);
            _empty.Toggle("B", true);
            _empty.AddDependencies("B", "C");

            AssertSet(_empty.Selection, "A", "B", "C");
            _empty.AddConflicts("A", "C");

            AssertSet(_empty.Selection, "A");
        }
    }
}