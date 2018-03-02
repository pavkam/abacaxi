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

namespace Abacaxi.Threading
{
    using System;
    using Internal;
    using JetBrains.Annotations;

    /// <summary>
    /// Class that hold a temporary value for a give resource.
    /// </summary>
    /// <typeparam name="T">The type of values stored in this temporary container.</typeparam>
    [PublicAPI]
    public sealed class Temporary<T>
    {
        [NotNull] private readonly object _lock = new object();
        [NotNull] private readonly Func<T> _valueFunc;
        private readonly int _valueTtl;

        private T _value;
        private DateTime _valueExpiryDateTime;

        /// <summary>
        /// Initializes a new instance of the <see cref="Temporary{T}"/> class.
        /// </summary>
        /// <param name="valueFunc">The function that creates a new value.</param>
        /// <param name="valueLifespanMillis">The lifespan of the created resource (in milliseconds).</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="valueFunc"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="valueLifespanMillis"/> is less than one.</exception>
        public Temporary([NotNull] Func<T> valueFunc, int valueLifespanMillis)
        {
            Validate.ArgumentNotNull(nameof(valueFunc), valueFunc);
            Validate.ArgumentGreaterThanZero(nameof(valueLifespanMillis), valueLifespanMillis);

            _valueTtl = valueLifespanMillis;
            _valueExpiryDateTime = DateTime.MinValue;
            _valueFunc = valueFunc;
        }

        /// <summary>
        /// Gets the resource value. This property is thread-safe and might be blocking (during the duration of resource refresh).
        /// </summary>
        /// <value>
        /// The resource value.
        /// </value>
        public T Value
        {
            get
            {
                var now = DateTime.Now;
                if (_valueExpiryDateTime < now)
                {
                    lock (_lock)
                    {
                        if (_valueExpiryDateTime < now)
                        {
                            _valueExpiryDateTime = now.AddMilliseconds(_valueTtl);
                            _value = _valueFunc();
                        }
                    }
                }

                return _value;
            }
        }
    }
}