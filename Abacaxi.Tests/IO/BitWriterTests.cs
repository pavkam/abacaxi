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

namespace Abacaxi.Tests.IO
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Text;
    using Abacaxi.IO;
    using JetBrains.Annotations;
    using NUnit.Framework;

    [TestFixture]
    public sealed class BitWriterTests
    {
        [NotNull]
        private static byte[] Encode([NotNull] Encoding encoding, [NotNull] Action<BitWriter> writerAction)
        {
            if (writerAction == null)
            {
                throw new ArgumentNullException(nameof(writerAction));
            }

            using (var stream = new MemoryStream())
            {
                using (var bitWriter = new BitWriter(stream, encoding, true))
                {
                    writerAction(bitWriter);
                }

                var bytes = stream.ToArray();
                return bytes;
            }
        }

        [NotNull]
        private static byte[] WriteBits(uint bits, int count)
        {
            using (var stream = new MemoryStream())
            {
                using (var bitWriter = new BitWriter(stream, Encoding.Default, true))
                {
                    bitWriter.WriteBits(bits, count);
                }

                return stream.ToArray();
            }
        }

        [Test, TestCase('A', (int) 'A')]
        public void Expect_Ascii_WriteChar_WritesCorrectly(char value, int e0)
        {
            var bytes = Encode(Encoding.UTF8, bitEncoder => bitEncoder.Write(value));

            Assert.AreEqual(5, bytes.Length);
            Assert.AreEqual(0x00, bytes[0]);
            Assert.AreEqual(0x00, bytes[1]);
            Assert.AreEqual(0x00, bytes[2]);
            Assert.AreEqual(0x01, bytes[3]);
            Assert.AreEqual(e0, bytes[4]);
        }

        [Test, SuppressMessage("ReSharper", "ObjectCreationAsStatement"),
         SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void Expect_Constructor_ThrowsException_ForNullEncoding()
        {
            var readOnlyStream = new MemoryStream(new byte[] { }, false);
            Assert.Throws<ArgumentNullException>(() => new BitWriter(readOnlyStream, null));
        }

        [Test, SuppressMessage("ReSharper", "ObjectCreationAsStatement"),
         SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void Expect_Constructor_ThrowsException_ForNullStream()
        {
            Assert.Throws<ArgumentNullException>(() => new BitWriter(null, Encoding.Default));
        }

        [Test, SuppressMessage("ReSharper", "ObjectCreationAsStatement")]
        public void Expect_Constructor_ThrowsException_ForReadOnlyStream()
        {
            var readOnlyStream = new MemoryStream(new byte[] { }, false);
            Assert.Throws<ArgumentException>(() => new BitWriter(readOnlyStream, Encoding.Default));
        }

        [Test, TestCase(1, 1), TestCase(8, 1), TestCase(9, 2), TestCase(16, 2), TestCase(17, 3), TestCase(24, 3),
         TestCase(25, 4), TestCase(32, 4)]
        public void Expect_FinalFlush_WritesProperByte(int bitCount, int expectedByteCount)
        {
            var bytes = WriteBits(0, bitCount);
            Assert.AreEqual(expectedByteCount, bytes.Length);
        }

        [Test]
        public void Expect_Stream_AttemptsToCloseUnderlyingStream_OnDispose()
        {
            var testStream = new HelperMemoryStream();
            using (new BitWriter(testStream, Encoding.Default))
            {
            }

            Assert.IsTrue(testStream.IsClosed);
        }

        [Test]
        public void Expect_Stream_IsClosed_AfterClose()
        {
            var testStream = new HelperMemoryStream();
            var bitWriter = new BitWriter(testStream, Encoding.Default);
            bitWriter.Close();

            Assert.IsTrue(testStream.IsClosed);
        }

        [Test]
        public void Expect_Stream_RemainsOpen_AfterClose()
        {
            var testStream = new HelperMemoryStream();
            var bitWriter = new BitWriter(testStream, Encoding.Default, true);
            bitWriter.Close();

            Assert.IsFalse(testStream.IsClosed);
        }

        [Test]
        public void Expect_Utf8_WriteChar_WritesCorrectly()
        {
            var bytes = Encode(Encoding.UTF8, bitEncoder => bitEncoder.Write('\u0162'));

            Assert.AreEqual(6, bytes.Length);
            Assert.AreEqual(0x00, bytes[0]);
            Assert.AreEqual(0x00, bytes[1]);
            Assert.AreEqual(0x00, bytes[2]);
            Assert.AreEqual(0x02, bytes[3]);
            Assert.AreEqual(0xC5, bytes[4]);
            Assert.AreEqual(0xA2, bytes[5]);
        }

        [Test]
        public void Expect_WriteBites_ThrowsException_OnHigherAmount()
        {
            var bitWriter = new BitWriter(new MemoryStream(), Encoding.Default);
            Assert.Throws<ArgumentOutOfRangeException>(() => bitWriter.WriteBits(0, 33));
        }

        [Test]
        public void Expect_WriteBites_ThrowsException_OnZeroCount()
        {
            var bitWriter = new BitWriter(new MemoryStream(), Encoding.Default);
            Assert.Throws<ArgumentOutOfRangeException>(() => bitWriter.WriteBits(0, 0));
        }

        [Test]
        public void Expect_WriteBits_WritesOneByteInProperSequence()
        {
            using (var stream = new MemoryStream())
            {
                using (var bitWriter = new BitWriter(stream, Encoding.Default, true))
                {
                    bitWriter.WriteBits(1, 1);
                    bitWriter.WriteBits(0, 2);
                    bitWriter.WriteBits(6, 3);
                    bitWriter.WriteBits(1, 1);
                    bitWriter.WriteBits(0, 1);
                }

                var bytes = stream.ToArray();
                Assert.AreEqual(1, bytes.Length);
                Assert.AreEqual(0x9A, bytes[0]);
            }
        }

        [Test]
        public void Expect_WriteBits_WritesTwoBytesInProperSequence()
        {
            using (var stream = new MemoryStream())
            {
                using (var bitWriter = new BitWriter(stream, Encoding.Default, true))
                {
                    bitWriter.WriteBits(0, 7);
                    bitWriter.WriteBits(3, 2);
                    bitWriter.WriteBits(0, 2);
                    bitWriter.WriteBits(6, 3);
                    bitWriter.WriteBits(1, 1);
                    bitWriter.WriteBits(0, 1);
                }

                var bytes = stream.ToArray();
                Assert.AreEqual(2, bytes.Length);
                Assert.AreEqual(1, bytes[0]);
                Assert.AreEqual(0x9A, bytes[1]);
            }
        }

        [Test]
        public void Expect_WriteBits_WritesWordInProperSequence()
        {
            using (var stream = new MemoryStream())
            {
                using (var bitWriter = new BitWriter(stream, Encoding.Default, true))
                {
                    bitWriter.WriteBits(1, 8);
                    bitWriter.WriteBits(2, 8);
                    bitWriter.WriteBits(3, 8);
                    bitWriter.WriteBits(4, 8);
                }

                var bytes = stream.ToArray();
                Assert.AreEqual(4, bytes.Length);
                Assert.AreEqual(1, bytes[0]);
                Assert.AreEqual(2, bytes[1]);
                Assert.AreEqual(3, bytes[2]);
                Assert.AreEqual(4, bytes[3]);
            }
        }

        [Test]
        public void Expect_WriteBits_WritesWordsInProperSequence()
        {
            using (var stream = new MemoryStream())
            {
                using (var bitWriter = new BitWriter(stream, Encoding.Default, true))
                {
                    bitWriter.WriteBits(1, 32);
                    bitWriter.WriteBits(2, 32);
                }

                var bytes = stream.ToArray();
                Assert.AreEqual(8, bytes.Length);
                Assert.AreEqual(1, bytes[3]);
                Assert.AreEqual(2, bytes[7]);
            }
        }


        [Test, TestCase(true, 0x01), TestCase(false, 0x00)]
        public void Expect_WriteBoolean_WritesCorrectly(bool value, int expected)
        {
            var bytes = Encode(Encoding.UTF8, bitEncoder => bitEncoder.Write(value));

            Assert.AreEqual(1, bytes.Length);
            Assert.AreEqual(expected, bytes[0]);
        }

        [Test, TestCase((byte) 0x00, 0x00), TestCase((byte) 0xFF, 0xFF)]
        public void Expect_WriteByte_WritesCorrectly(byte value, int expected)
        {
            var bytes = Encode(Encoding.UTF8, bitEncoder => bitEncoder.Write(value));

            Assert.AreEqual(1, bytes.Length);
            Assert.AreEqual(expected, bytes[0]);
        }

        [Test]
        public void Expect_WriteBytes1_DoesNotWrite_EmptyArray()
        {
            var bytes = Encode(Encoding.UTF8, bitEncoder => bitEncoder.WriteBytes(new byte[] {1, 2}, 0, 0));

            Assert.AreEqual(0, bytes.Length);
        }

        [Test]
        public void Expect_WriteBytes1_ThrowsException_ForExcessiveCount()
        {
            Encode(Encoding.UTF8, bitEncoder =>
            {
                var array = new byte[2];
                Assert.Throws<ArgumentOutOfRangeException>(() => bitEncoder.WriteBytes(array, 1, 2));
            });
        }

        [Test]
        public void Expect_WriteBytes1_ThrowsException_ForExcessiveOffset()
        {
            Encode(Encoding.UTF8, bitEncoder =>
            {
                var array = new byte[2];
                Assert.Throws<ArgumentOutOfRangeException>(() => bitEncoder.WriteBytes(array, 2, 1));
            });
        }

        [Test]
        public void Expect_WriteBytes1_ThrowsException_ForNegativeOffset()
        {
            Encode(Encoding.UTF8, bitEncoder =>
            {
                var array = new byte[2];
                Assert.Throws<ArgumentOutOfRangeException>(() => bitEncoder.WriteBytes(array, -1, 1));
            });
        }

        [Test, SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void Expect_WriteBytes1_ThrowsException_IfBytesIsNull()
        {
            Encode(Encoding.UTF8,
                bitEncoder => Assert.Throws<ArgumentNullException>(() => bitEncoder.WriteBytes(null, 0, 1)));
        }


        [Test]
        public void Expect_WriteBytes1_WritesBytesInBitSequence()
        {
            using (var stream = new MemoryStream())
            {
                using (var bitWriter = new BitWriter(stream, Encoding.Default, true))
                {
                    bitWriter.WriteBits(1, 1);
                    bitWriter.WriteBytes(new byte[] {1}, 0, 1);
                }

                var bytes = stream.ToArray();
                Assert.AreEqual(2, bytes.Length);
                Assert.AreEqual(0x80, bytes[0]);
                Assert.AreEqual(0x80, bytes[1]);
            }
        }

        [Test]
        public void Expect_WriteBytes1_WritesCorrectly()
        {
            var bytes = Encode(Encoding.UTF8,
                bitEncoder => bitEncoder.WriteBytes(new byte[] {0xAA, 0xFF, 0xCC, 0x00}, 0, 4));

            Assert.AreEqual(4, bytes.Length);
            Assert.AreEqual(0xAA, bytes[0]);
            Assert.AreEqual(0xFF, bytes[1]);
            Assert.AreEqual(0xCC, bytes[2]);
            Assert.AreEqual(0x00, bytes[3]);
        }

        [Test]
        public void Expect_WriteBytes1_WritesFourUnalignedBytesInSequence()
        {
            using (var stream = new MemoryStream())
            {
                using (var bitWriter = new BitWriter(stream, Encoding.Default, true))
                {
                    bitWriter.WriteBytes(new byte[] {5, 6, 1, 2}, 0, 4);
                }

                var bytes = stream.ToArray();
                Assert.AreEqual(4, bytes.Length);
                Assert.AreEqual(5, bytes[0]);
                Assert.AreEqual(6, bytes[1]);
                Assert.AreEqual(1, bytes[2]);
                Assert.AreEqual(2, bytes[3]);
            }
        }

        [Test]
        public void Expect_WriteBytes1_WritesTwoUnalignedBytesInSequence()
        {
            using (var stream = new MemoryStream())
            {
                using (var bitWriter = new BitWriter(stream, Encoding.Default, true))
                {
                    bitWriter.WriteBytes(new byte[] {5, 6}, 0, 2);
                }

                var bytes = stream.ToArray();
                Assert.AreEqual(2, bytes.Length);
                Assert.AreEqual(5, bytes[0]);
                Assert.AreEqual(6, bytes[1]);
            }
        }

        [Test]
        public void Expect_WriteBytes1_WritesUnalignedBytesInSequence()
        {
            using (var stream = new MemoryStream())
            {
                using (var bitWriter = new BitWriter(stream, Encoding.Default, true))
                {
                    bitWriter.WriteBytes(new byte[] {0, 1, 2, 3, 4, 5, 6, 7}, 1, 6);
                }

                var bytes = stream.ToArray();
                Assert.AreEqual(6, bytes.Length);
                Assert.AreEqual(1, bytes[0]);
                Assert.AreEqual(2, bytes[1]);
                Assert.AreEqual(3, bytes[2]);
                Assert.AreEqual(4, bytes[3]);
                Assert.AreEqual(5, bytes[4]);
                Assert.AreEqual(6, bytes[5]);
            }
        }

        [Test]
        public void Expect_WriteBytes2_ThrowsException_ForExcessiveCount()
        {
            Encode(Encoding.UTF8, bitEncoder =>
            {
                var array = new byte[2];
                Assert.Throws<ArgumentOutOfRangeException>(() => bitEncoder.Write(array, 1, 2));
            });
        }

        [Test]
        public void Expect_WriteBytes2_ThrowsException_ForExcessiveOffset()
        {
            Encode(Encoding.UTF8, bitEncoder =>
            {
                var array = new byte[2];
                Assert.Throws<ArgumentOutOfRangeException>(() => bitEncoder.Write(array, 2, 1));
            });
        }

        [Test]
        public void Expect_WriteBytes2_ThrowsException_ForNegativeOffset()
        {
            Encode(Encoding.UTF8, bitEncoder =>
            {
                var array = new byte[2];
                Assert.Throws<ArgumentOutOfRangeException>(() => bitEncoder.Write(array, -1, 1));
            });
        }

        [Test, SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void Expect_WriteBytes2_ThrowsException_IfBytesIsNull()
        {
            Encode(Encoding.UTF8,
                bitEncoder => Assert.Throws<ArgumentNullException>(() => bitEncoder.Write(null, 0, 1)));
        }

        [Test]
        public void Expect_WriteBytes2_WritesCorrectly()
        {
            var bytes = Encode(Encoding.UTF8,
                bitEncoder => bitEncoder.Write(new byte[] {0xAA, 0xFF, 0xCC, 0x00}, 0, 4));

            Assert.AreEqual(8, bytes.Length);
            Assert.AreEqual(0x00, bytes[0]);
            Assert.AreEqual(0x00, bytes[1]);
            Assert.AreEqual(0x00, bytes[2]);
            Assert.AreEqual(0x04, bytes[3]);
            Assert.AreEqual(0xAA, bytes[4]);
            Assert.AreEqual(0xFF, bytes[5]);
            Assert.AreEqual(0xCC, bytes[6]);
            Assert.AreEqual(0x00, bytes[7]);
        }

        [Test]
        public void Expect_WriteBytes2_WritesCorrectly_EmptyArray()
        {
            var bytes = Encode(Encoding.UTF8, bitEncoder => bitEncoder.Write(new byte[] {1, 2}, 0, 0));

            Assert.AreEqual(4, bytes.Length);
            Assert.AreEqual(0x00, bytes[0]);
            Assert.AreEqual(0x00, bytes[1]);
            Assert.AreEqual(0x00, bytes[2]);
            Assert.AreEqual(0x00, bytes[3]);
        }

        [Test]
        public void Expect_WriteBytes3_WritesCorrectly()
        {
            var bytes = Encode(Encoding.UTF8, bitEncoder => bitEncoder.Write(new byte[] {0xAA, 0xFF, 0xCC, 0x00}));

            Assert.AreEqual(8, bytes.Length);
            Assert.AreEqual(0x00, bytes[0]);
            Assert.AreEqual(0x00, bytes[1]);
            Assert.AreEqual(0x00, bytes[2]);
            Assert.AreEqual(0x04, bytes[3]);
            Assert.AreEqual(0xAA, bytes[4]);
            Assert.AreEqual(0xFF, bytes[5]);
            Assert.AreEqual(0xCC, bytes[6]);
            Assert.AreEqual(0x00, bytes[7]);
        }

        [Test]
        public void Expect_WriteBytes3_WritesCorrectly_EmptyArray()
        {
            var bytes = Encode(Encoding.UTF8, bitEncoder => bitEncoder.Write(new byte[] { }));

            Assert.AreEqual(4, bytes.Length);
            Assert.AreEqual(0x00, bytes[0]);
            Assert.AreEqual(0x00, bytes[1]);
            Assert.AreEqual(0x00, bytes[2]);
            Assert.AreEqual(0x00, bytes[3]);
        }

        [Test]
        public void Expect_WriteChar_ThrowsException_ForSurrogateChar()
        {
            Encode(Encoding.UTF8,
                bitEncoder => Assert.Throws<ArgumentException>(() => bitEncoder.Write((char) 0xD800)));
        }

        [Test,
         TestCase(0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00),
         TestCase(-1, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80)]
        public void Expect_WriteDecimal_WritesCorrectly(double value,
            int e0, int e1, int e2, int e3, int e4, int e5, int e6, int e7,
            int e8, int e9, int e10, int e11, int e12, int e13, int e14, int e15)
        {
            var bytes = Encode(Encoding.UTF8, bitEncoder => bitEncoder.Write((decimal) value));

            Assert.AreEqual(16, bytes.Length);
            Assert.AreEqual(e15, bytes[15]);
            Assert.AreEqual(e14, bytes[14]);
            Assert.AreEqual(e13, bytes[13]);
            Assert.AreEqual(e12, bytes[12]);
            Assert.AreEqual(e11, bytes[11]);
            Assert.AreEqual(e10, bytes[10]);
            Assert.AreEqual(e9, bytes[9]);
            Assert.AreEqual(e8, bytes[8]);
            Assert.AreEqual(e7, bytes[7]);
            Assert.AreEqual(e6, bytes[6]);
            Assert.AreEqual(e5, bytes[5]);
            Assert.AreEqual(e4, bytes[4]);
            Assert.AreEqual(e3, bytes[3]);
            Assert.AreEqual(e2, bytes[2]);
            Assert.AreEqual(e1, bytes[1]);
            Assert.AreEqual(e0, bytes[0]);
        }

        [Test, TestCase((double) 0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00),
         TestCase(double.PositiveInfinity, 0x7F, 0xF0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00),
         TestCase(double.MaxValue, 0x7F, 0xEF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF)]
        public void Expect_WriteDouble_WritesCorrectly(double value,
            int e0, int e1, int e2, int e3, int e4, int e5, int e6, int e7)
        {
            var bytes = Encode(Encoding.UTF8, bitEncoder => bitEncoder.Write(value));

            Assert.AreEqual(8, bytes.Length);
            Assert.AreEqual(e7, bytes[7]);
            Assert.AreEqual(e6, bytes[6]);
            Assert.AreEqual(e5, bytes[5]);
            Assert.AreEqual(e4, bytes[4]);
            Assert.AreEqual(e3, bytes[3]);
            Assert.AreEqual(e2, bytes[2]);
            Assert.AreEqual(e1, bytes[1]);
            Assert.AreEqual(e0, bytes[0]);
        }

        [Test, TestCase((short) -1, 0xFF, 0xFF), TestCase((short) 0x00FF, 0x00, 0xFF)]
        public void Expect_WriteInt16_WritesCorrectly(short value, int e0, int e1)
        {
            var bytes = Encode(Encoding.UTF8, bitEncoder => bitEncoder.Write(value));

            Assert.AreEqual(2, bytes.Length);
            Assert.AreEqual(e1, bytes[1]);
            Assert.AreEqual(e0, bytes[0]);
        }

        [Test, TestCase(-1, 0xFF, 0xFF, 0xFF, 0xFF), TestCase(0x00000000, 0x00, 0x00, 0x00, 0x00)]
        public void Expect_WriteInt32_WritesCorrectly(int value, int e0, int e1, int e2, int e3)
        {
            var bytes = Encode(Encoding.UTF8, bitEncoder => bitEncoder.Write(value));

            Assert.AreEqual(4, bytes.Length);
            Assert.AreEqual(e3, bytes[3]);
            Assert.AreEqual(e2, bytes[2]);
            Assert.AreEqual(e1, bytes[1]);
            Assert.AreEqual(e0, bytes[0]);
        }

        [Test, TestCase((long) -1, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF),
         TestCase(0x0000000000000000L, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00)]
        public void Expect_WriteInt64_WritesCorrectly(long value,
            int e0, int e1, int e2, int e3, int e4, int e5, int e6, int e7)
        {
            var bytes = Encode(Encoding.UTF8, bitEncoder => bitEncoder.Write(value));

            Assert.AreEqual(8, bytes.Length);
            Assert.AreEqual(e7, bytes[7]);
            Assert.AreEqual(e6, bytes[6]);
            Assert.AreEqual(e5, bytes[5]);
            Assert.AreEqual(e4, bytes[4]);
            Assert.AreEqual(e3, bytes[3]);
            Assert.AreEqual(e2, bytes[2]);
            Assert.AreEqual(e1, bytes[1]);
            Assert.AreEqual(e0, bytes[0]);
        }

        [Test, TestCase((sbyte) 0x00, 0x00), TestCase((sbyte) -1, 0xFF)]
        public void Expect_WriteSByte_WritesCorrectly(sbyte value, int expected)
        {
            var bytes = Encode(Encoding.UTF8, bitEncoder => bitEncoder.Write(value));

            Assert.AreEqual(1, bytes.Length);
            Assert.AreEqual(expected, bytes[0]);
        }

        [Test, TestCase((float) 0, 0x00, 0x00, 0x00, 0x00), TestCase(float.PositiveInfinity, 0x7F, 0x80, 0x00, 0x00),
         TestCase(float.MaxValue, 0x7F, 0x7F, 0xFF, 0xFF)]
        public void Expect_WriteSingle_WritesCorrectly(float value, int e0, int e1, int e2, int e3)
        {
            var bytes = Encode(Encoding.UTF8, bitEncoder => bitEncoder.Write(value));

            Assert.AreEqual(4, bytes.Length);
            Assert.AreEqual(e3, bytes[3]);
            Assert.AreEqual(e2, bytes[2]);
            Assert.AreEqual(e1, bytes[1]);
            Assert.AreEqual(e0, bytes[0]);
        }

        [Test]
        public void Expect_WriteString_OneCharString_WritesCorrectly()
        {
            var bytes = Encode(Encoding.UTF8, bitEncoder => bitEncoder.Write("A"));

            Assert.AreEqual(5, bytes.Length);
            Assert.AreEqual(65, bytes[4]);
            Assert.AreEqual(1, bytes[3]);
            Assert.AreEqual(0, bytes[2]);
            Assert.AreEqual(0, bytes[1]);
            Assert.AreEqual(0, bytes[0]);
        }

        [Test, SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void Expect_WriteString_ThrowsException_ForNullValue()
        {
            Encode(Encoding.UTF8,
                bitEncoder => Assert.Throws<ArgumentNullException>(() => bitEncoder.Write((string) null)));
        }

        [Test]
        public void Expect_WriteString_ZeroLengthString_WritesCorrectly()
        {
            var bytes = Encode(Encoding.UTF8, bitEncoder => bitEncoder.Write(string.Empty));

            Assert.AreEqual(4, bytes.Length);
            Assert.AreEqual(0, bytes[3]);
            Assert.AreEqual(0, bytes[2]);
            Assert.AreEqual(0, bytes[1]);
            Assert.AreEqual(0, bytes[0]);
        }

        [Test, TestCase((ushort) 0xFF00, 0xFF, 0x00), TestCase((ushort) 0x00FF, 0x00, 0xFF)]
        public void Expect_WriteUInt16_WritesCorrectly(ushort value, int e0, int e1)
        {
            var bytes = Encode(Encoding.UTF8, bitEncoder => bitEncoder.Write(value));

            Assert.AreEqual(2, bytes.Length);
            Assert.AreEqual(e1, bytes[1]);
            Assert.AreEqual(e0, bytes[0]);
        }

        [Test, TestCase(0xAABBCCDD, 0xAA, 0xBB, 0xCC, 0xDD), TestCase((uint) 0x00000000, 0x00, 0x00, 0x00, 0x00)]
        public void Expect_WriteUInt32_WritesCorrectly(uint value, int e0, int e1, int e2, int e3)
        {
            var bytes = Encode(Encoding.UTF8, bitEncoder => bitEncoder.Write(value));

            Assert.AreEqual(4, bytes.Length);
            Assert.AreEqual(e3, bytes[3]);
            Assert.AreEqual(e2, bytes[2]);
            Assert.AreEqual(e1, bytes[1]);
            Assert.AreEqual(e0, bytes[0]);
        }

        [Test, TestCase((ulong) 0x2233445566778899L, 0x22, 0x33, 0x44, 0x55, 0x66, 0x77, 0x88, 0x99),
         TestCase((ulong) 0x0000000000000000L, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00)]
        public void Expect_WriteUInt64_WritesCorrectly(ulong value,
            int e0, int e1, int e2, int e3, int e4, int e5, int e6, int e7)
        {
            var bytes = Encode(Encoding.UTF8, bitEncoder => bitEncoder.Write(value));

            Assert.AreEqual(8, bytes.Length);
            Assert.AreEqual(e7, bytes[7]);
            Assert.AreEqual(e6, bytes[6]);
            Assert.AreEqual(e5, bytes[5]);
            Assert.AreEqual(e4, bytes[4]);
            Assert.AreEqual(e3, bytes[3]);
            Assert.AreEqual(e2, bytes[2]);
            Assert.AreEqual(e1, bytes[1]);
            Assert.AreEqual(e0, bytes[0]);
        }
    }
}