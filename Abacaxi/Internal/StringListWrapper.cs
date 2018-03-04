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

namespace Abacaxi.Internal
{
    using System;
    using System.Collections;
    using System.Diagnostics;
    using System.Collections.Generic;
    using System.Linq;
    using JetBrains.Annotations;

    internal sealed class StringListWrapper : IList<char>
    {
        [NotNull]
        private readonly string _s;

        public StringListWrapper([NotNull] string s)
        {
            Assert.NotNull(s != null);

            _s = s;
        }

        public IEnumerator<char> GetEnumerator()
        {
            return _s.Cast<char>().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_s).GetEnumerator();
        }

        [ContractAnnotation("=> halt")]
        public void Add(char item)
        {
            throw new NotSupportedException();
        }

        [ContractAnnotation("=> halt")]
        public void Clear()
        {
            throw new NotSupportedException();
        }

        public bool Contains(char item)
        {
            return _s.Contains(item.ToString());
        }

        public void CopyTo(char[] array, int arrayIndex)
        {
            _s.CopyTo(0, array, arrayIndex, _s.Length);
        }

        [ContractAnnotation("=> halt")]
        public bool Remove(char item)
        {
            throw new NotSupportedException();
        }

        public int Count => _s.Length;

        public bool IsReadOnly => true;

        public int IndexOf(char item)
        {
            return _s.IndexOf(item);
        }

        [ContractAnnotation("=> halt")]
        public void Insert(int index, char item)
        {
            throw new NotSupportedException();
        }

        [ContractAnnotation("=> halt")]
        public void RemoveAt(int index)
        {
            throw new NotSupportedException();
        }

        public char this[int index]
        {
            get => _s[index];
            [ContractAnnotation("=> halt")]
            set => throw new NotSupportedException();
        }
    }
}
