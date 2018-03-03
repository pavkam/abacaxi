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
    /// Container that caches a value for a specified duration.
    /// </summary>
    /// <typeparam name="T">The type of value cached by this container.</typeparam>
    [PublicAPI]
    public sealed class Cached<T>
    {
        [NotNull] private readonly object _lock = new object();
        [CanBeNull] private readonly Func<T> _valueRefreshFunc;
        private readonly int _valueTtlMillis;

        private T _value;
        private long _expiresAtTicks;

        private long CurrentTicks => DateTime.Now.Ticks;

        [CanBeNull]
        private T GetInternal([CanBeNull] Func<T> valueRefreshFunc = null)
        {
            if (_expiresAtTicks >= CurrentTicks)
            {
                return _value;
            }

            lock (_lock)
            {
                if (_expiresAtTicks >= CurrentTicks)
                {
                    return _value;
                }

                var selectedFunc = valueRefreshFunc ?? _valueRefreshFunc;
                if (selectedFunc == null)
                {
                    throw new InvalidOperationException(
                        "No value refresh function has been supplied when this object was created.");
                }

                _value = selectedFunc();
                _expiresAtTicks = CurrentTicks + _valueTtlMillis * TimeSpan.TicksPerMillisecond;
            }

            return _value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Cached{T}"/> class.
        /// </summary>
        /// <param name="valueRefreshFunc">The function that creates a new value.</param>
        /// <param name="valueLifespanMillis">The lifespan of the created resource (in milliseconds).</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="valueRefreshFunc"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="valueLifespanMillis"/> is less than one.</exception>
        public Cached([NotNull] Func<T> valueRefreshFunc, int valueLifespanMillis) : this(valueLifespanMillis)
        {
            Validate.ArgumentNotNull(nameof(valueRefreshFunc), valueRefreshFunc);

            _valueRefreshFunc = valueRefreshFunc;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Cached{T}"/> class.
        /// </summary>
        /// <param name="valueLifespanMillis">The lifespan of the created resource (in milliseconds).</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="valueLifespanMillis"/> is less than one.</exception>
        public Cached(int valueLifespanMillis)
        {
            Validate.ArgumentGreaterThanOrEqualToZero(nameof(valueLifespanMillis), valueLifespanMillis);

            _valueTtlMillis = valueLifespanMillis;
        }

        /// <summary>
        /// Gets the cached value. This property is thread-safe and is blocking during the refresh.
        /// </summary>
        /// <value>
        /// The cached value.
        /// </value>
        [CanBeNull]
        public T Value => GetInternal();

        /// <summary>
        /// Gets the cached value using a given refresh function.
        /// </summary>
        /// <param name="valueRefreshFunc">The value refresh function.</param>
        /// <returns>The cached value.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="valueRefreshFunc"/> is <c>null</c>.</exception>
        [CanBeNull]
        public T Get([NotNull] Func<T> valueRefreshFunc)
        {
            if (valueRefreshFunc == null)
            {
                throw new ArgumentNullException(nameof(valueRefreshFunc));
            }

            return GetInternal(valueRefreshFunc);
        }

        /// <summary>
        /// Expires current resource value managed by this <see cref="Cached{T}"/>.
        /// </summary>
        /// <remarks>The resource will be refreshed the next time <see cref="Value"/> property is accessed.</remarks>
        public void Expire()
        {
            lock (_lock)
            {
                _expiresAtTicks = 0;
                _value = default(T);
            }
        }
    }
}