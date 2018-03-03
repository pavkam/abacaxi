/* Copyright 2017 by Alexandru Ciobanu (alex+git@ciobanu.org)
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

namespace Abacaxi.IO
{
    using System;
    using System.Text;
    using System.IO;
    using System.Diagnostics;
    using Internal;
    using JetBrains.Annotations;

    /// <summary>
    /// Class implements writing bits/bytes to un underlying stream.
    /// </summary>
    [PublicAPI]
    public sealed class BitWriter : IDisposable
    {
        private const int BytesInWord = sizeof(uint);
        private const int BitsInByte = 8;
        private const int BitsInWord = BitsInByte * BytesInWord;
        private const int Msb = BitsInWord - 1;

        [NotNull]
        private readonly Encoding _encoding;
        [CanBeNull]
        private Stream _stream;
        private readonly bool _leaveOpen;
        private uint _currentWord;
        private int _currentBitIndex;
        [NotNull]
        private readonly byte[] _disassemblyBuffer;
        [NotNull]
        private readonly byte[] _assemblyBuffer;
        [NotNull]
        private readonly char[] _singleCharBuffer;

        [NotNull]
        private byte[] DisassembleWord(uint word)
        {
            _disassemblyBuffer[0] = (byte)(word >> 24);
            _disassemblyBuffer[1] = (byte)(word >> 16);
            _disassemblyBuffer[2] = (byte)(word >> 8);
            _disassemblyBuffer[3] = (byte)word;

            return _disassemblyBuffer;
        }

        private static uint AssembleWord([NotNull] byte[] bytes, int index)
        {
            Debug.Assert(bytes != null);
            Debug.Assert(index + BytesInWord <= bytes.Length);

            var word =
                (uint)bytes[index + 3] << 24 |
                (uint)bytes[index + 2] << 16 |
                (uint)bytes[index + 1] << 8 |
                bytes[index];

            return word;
        }

        private void WriteBitsPartial(uint bits, int count)
        {
            Debug.Assert(count > 0 && count <= BitsInWord);
            Debug.Assert(count <= _currentBitIndex + 1);

            /* Shift the bits to the most left, then to the right (to clear them for OR) */
            bits <<= Msb - count + 1;
            bits >>= Msb - _currentBitIndex;

            _currentWord |= bits;
            _currentBitIndex -= count;

            /* If overloaded, flush the word and reset. */
            Debug.Assert(_currentBitIndex >= -1);
            if (_currentBitIndex == -1)
            {
                var bytes = DisassembleWord(_currentWord);
                WriteToStream(bytes, 0, bytes.Length);

                _currentBitIndex = Msb;
                _currentWord = 0;
            }
        }

        private void WriteToStream([NotNull] byte[] bytes, int offset, int count)
        {
            Debug.Assert(bytes != null);
            Debug.Assert(offset >= 0 && offset < bytes.Length);
            Debug.Assert(count > 0 && offset + count <= bytes.Length);
            Debug.Assert(_stream != null);

            _stream.Write(bytes, offset, count);
        }

        private void Flush()
        {
            if (_currentBitIndex < Msb)
            {
                /* Find the number of bytes to actually flush. */
                var bytes = DisassembleWord(_currentWord);
                var msbFlush = (_currentBitIndex + 1) / BitsInByte;
                WriteToStream(bytes, 0, BytesInWord - msbFlush);

                _currentBitIndex = Msb;
                _currentWord = 0;
            }
        }

        /// <summary>
        /// Specifies whether the writer is word aligned. When word-aligned, bytes can be written
        /// directly into the output stream.
        /// </summary>
        private bool IsWordAligned => _currentBitIndex == Msb;

        /// <summary>
        /// Initializes a new instance of <see cref="BitWriter"/> class.
        /// </summary>
        /// <param name="stream">The stream to write into.</param>
        /// <param name="encoding">The encoding object used to convert strings into bytes.</param>
        /// <param name="leaveOpen">Forces this instance to leave the stream open.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="stream"/> or <paramref name="encoding"/> are <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="stream"/> is not writable.</exception>
        public BitWriter([NotNull] Stream stream, [NotNull] Encoding encoding, bool leaveOpen = false)
        {
            Validate.ArgumentNotNull(nameof(stream), stream);
            Validate.ArgumentNotNull(nameof(encoding), encoding);

            if (!stream.CanWrite)
            {
                throw new ArgumentException("Stream is not writable.", nameof(stream));
            }

            _encoding = encoding;
            _stream = stream;
            _leaveOpen = leaveOpen;
            _currentBitIndex = Msb;
            _disassemblyBuffer = new byte[BytesInWord];
            _assemblyBuffer = new byte[BytesInWord * 4];
            _singleCharBuffer = new char[1];
        }

        /// <summary>
        /// Writes a <paramref name="count"/> of bits (starting with bit 0) from parameter <paramref name="bits"/>.
        /// </summary>
        /// <param name="bits">An <c>uint</c> value holding the bits to be written.</param>
        /// <param name="count">The number of bits to consider for writing.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if count is out of bounds.</exception>
        public void WriteBits(uint bits, int count)
        {
            Validate.ArgumentGreaterThanZero(nameof(count), count);
            Validate.ArgumentLessThanOrEqualTo(nameof(count), count, BitsInWord);

            var numberOfFreeBits = _currentBitIndex + 1;
            if (count > numberOfFreeBits)
            {
                /* The number of requested bits not fitting in the current word. Split into two. */
                var firstPiece = bits >> (count - numberOfFreeBits);
                WriteBitsPartial(firstPiece, numberOfFreeBits);
                count -= numberOfFreeBits;
            }

            /* Write all the remaining bits. */
            WriteBitsPartial(bits, count);
        }

        /// <summary>
        /// Writes an array of bytes to the output stream.
        /// </summary>
        /// <param name="bytes">The byte array.</param>
        /// <param name="offset">Index of the first element in the array.</param>
        /// <param name="count">The number of bytes to write.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="count"/> or <paramref name="offset"/> are out of bounds.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="bytes"/> is <c>null</c>.</exception>
        public void WriteBytes([NotNull] byte[] bytes, int offset, int count)
        {
            Validate.CollectionArgumentsInBounds(nameof(bytes), bytes, offset, count);

            if (count == 0)
            {
                return;
            }

            if (IsWordAligned)
            {
                /* Very special case where bits are "aligned". Simply write everything directly into the stream. */
                WriteToStream(bytes, offset, count);
            }
            else
            {
                var fullWordCount = count / BytesInWord;
                var remainderCount = count % BytesInWord;

                /* Write full words first */
                for (var i = 0; i < fullWordCount; i++)
                {
                    var index = offset + i * BytesInWord;
                    var word = AssembleWord(bytes, index);

                    WriteBits(word, BitsInWord);
                }

                /* Write remnants now */
                for (var i = 0; i < remainderCount; i++)
                {
                    var index = offset + i + fullWordCount * BytesInWord;
                    WriteBits(bytes[index], BitsInByte);
                }
            }
        }

        /// <summary>
        /// Writes an array of bytes to the output stream. Writes a prefix word that identifies the
        /// number of bytes the array contains.
        /// </summary>
        /// <param name="bytes">The byte array.</param>
        /// <param name="offset">Index of the first element in the array.</param>
        /// <param name="count">The number of bytes to write.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="count"/> is out of bounds.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="offset"/> is out of bounds.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="bytes"/> is <c>null</c>.</exception>
        public void Write([NotNull] byte[] bytes, int offset, int count)
        {
            Validate.CollectionArgumentsInBounds(nameof(bytes), bytes, offset, count);

            Write((uint)count);
            if (count > 0)
            {
                WriteBytes(bytes, offset, count);
            }
        }

        /// <summary>
        /// Writes an array of bytes to the output stream. Writes a prefix word that identifies the
        /// number of bytes the array contains.
        /// </summary>
        /// <param name="bytes">The byte array.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="bytes"/> is <c>null</c>.</exception>
        public void Write([NotNull] byte[] bytes)
        {
            Validate.ArgumentNotNull(nameof(bytes), bytes);

            Write(bytes, 0, bytes.Length);
        }

        /// <summary>
        /// Encodes a <c>bool</c> value.
        /// </summary>
        /// <param name="value">The value to encode.</param>
        public void Write(bool value)
        {
            WriteBits(value ? (uint)1 : 0, BitsInByte * sizeof(byte));
        }

        /// <summary>
        /// Encodes a <c>byte</c> value.
        /// </summary>
        /// <param name="value">The value to encode.</param>
        public void Write(byte value)
        {
            WriteBits(value, BitsInByte * sizeof(byte));
        }

        /// <summary>
        /// Encodes a <c>sbyte</c> value.
        /// </summary>
        /// <param name="value">The value to encode.</param>
        public void Write(sbyte value)
        {
            WriteBits((byte)value, BitsInByte * sizeof(byte));
        }

        /// <summary>
        /// Encodes a <c>ushort</c> value.
        /// </summary>
        /// <param name="value">The value to encode.</param>
        public void Write(ushort value)
        {
            WriteBits(value, BitsInByte * sizeof(ushort));
        }

        /// <summary>
        /// Encodes a <c>short</c> value.
        /// </summary>
        /// <param name="value">The value to encode.</param>
        public void Write(short value)
        {
            WriteBits((ushort)value, BitsInByte * sizeof(ushort));
        }

        /// <summary>
        /// Encodes an <c>uint</c> value.
        /// </summary>
        /// <param name="value">The value to encode.</param>
        public void Write(uint value)
        {
            WriteBits(value, BitsInByte * sizeof(uint));
        }

        /// <summary>
        /// Encodes an <c>int</c> value.
        /// </summary>
        /// <param name="value">The value to encode.</param>
        public void Write(int value)
        {
            WriteBits((uint)value, BitsInByte * sizeof(uint));
        }

        /// <summary>
        /// Encodes an <c>ulong</c> value.
        /// </summary>
        /// <param name="value">The value to encode.</param>
        public void Write(ulong value)
        {
            WriteBits((uint)(value >> BitsInByte * sizeof(uint)), BitsInByte * sizeof(uint));
            WriteBits((uint)value, BitsInByte * sizeof(uint));
        }

        /// <summary>
        /// Encodes a <c>long</c> value.
        /// </summary>
        /// <param name="value">The value to encode.</param>
        public void Write(long value)
        {
            WriteBits((uint)(value >> BitsInByte * sizeof(uint)), BitsInByte * sizeof(uint));
            WriteBits((uint)value, BitsInByte * sizeof(uint));
        }

        /// <summary>
        /// Encodes a <c>float</c> value.
        /// </summary>
        /// <param name="value">The value to encode.</param>
        public unsafe void Write(float value)
        {
            // ReSharper disable once IdentifierTypo
            var repr = *(uint*)&value;
            WriteBits(repr, BitsInByte * sizeof(uint));
        }

        /// <summary>
        /// Encodes a <c>double</c> value.
        /// </summary>
        /// <param name="value">The value to encode.</param>
        public unsafe void Write(double value)
        {
            // ReSharper disable once IdentifierTypo
            var repr = *(ulong*)&value;

            WriteBits((uint)(repr >> BitsInByte * sizeof(uint)), BitsInByte * sizeof(uint));
            WriteBits((uint)repr, BitsInByte * sizeof(uint));
        }

        /// <summary>
        /// Encodes a <c>decimal</c> value.
        /// </summary>
        /// <param name="value">The value to encode.</param>
        public void Write(decimal value)
        {
            // ReSharper disable once IdentifierTypo
            var repr = decimal.GetBits(value);
            var bytesInDecimal = repr.Length * sizeof(int);

            Debug.Assert(bytesInDecimal <= _assemblyBuffer.Length);

            Buffer.BlockCopy(repr, 0, _assemblyBuffer, 0, bytesInDecimal);
            WriteBytes(_assemblyBuffer, 0, bytesInDecimal);
        }

        /// <summary>
        /// Encodes a <c>char</c> value using the encoding supplied at construction time.
        /// </summary>
        /// <param name="value">The value to encode.</param>
        /// <exception cref="ArgumentException">Thrown if the character is a surrogate.</exception>
        public void Write(char value)
        {
            if (char.IsSurrogate(value))
            {
                throw new ArgumentException($"Argument {nameof(value)} cannot be a surrogate.");
            }

            _singleCharBuffer[0] = value;
            var actualByteCount = _encoding.GetBytes(_singleCharBuffer, 0, 1, _assemblyBuffer, BytesInWord);

            Write(_assemblyBuffer, BytesInWord, actualByteCount);
        }

        /// <summary>
        /// Encodes a <c>string</c> value using the encoding supplied at construction time.
        /// </summary>
        /// <param name="value">The value to encode.</param>
        /// <exception cref="ArgumentNullException">Thrown is the supplied string is <c>null</c>.</exception>
        public void Write([NotNull] string value)
        {
            Validate.ArgumentNotNull(nameof(value), value);

            var bytes = _encoding.GetBytes(value);
            Write(bytes, 0, bytes.Length);
        }

        /// <summary>
        /// Closes the underlying stream if this instance was initialized with <c>leaveOpen</c>
        /// set to <c>true</c>.
        /// </summary>
        public void Close()
        {
            GC.SuppressFinalize(this);
            Dispose(true);
        }

        /// <summary>
        /// Finalizes this instance of <see cref="BitWriter"/> class.
        /// </summary>
        ~BitWriter()
        {
            Dispose(false);
        }

        /// <summary>
        /// Disposes this instance of <see cref="BitWriter"/> class and attempts to close
        /// the underlying stream.
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
            Dispose(true);
        }

        /// <summary>
        /// Disposes this instance of <see cref="BitWriter"/> class and attempts to close
        /// the underlying stream.
        /// </summary>
        /// <param name="disposing"><c>true</c> if method called explicitly; <c>false</c> 
        /// if method was called from finalizer.</param>
        private void Dispose(bool disposing)
        {
            if (_stream != null)
            {
                Flush();

                if (!_leaveOpen)
                {
                    Debug.Assert(_stream != null);

                    _stream.Dispose();
                    _stream = null;
                }
            }
        }
    }
}