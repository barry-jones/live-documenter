using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheBoxSoftware.Reflection.Core;

namespace TheBoxSoftware.Reflection.Signitures
{
    internal class SignitureToken
    {
        private const int CompressedByteMask = 0x7f;
        private const int CompressedShortMask = 0x3fff;
        private const int CompressedIntMask = 0x1fffffff;

        /// <summary>
        /// Initialises a new instance of the SignitureToken class.
        /// </summary>
        /// <param name="tokenType">The type of token.</param>
        internal SignitureToken(SignitureTokens tokenType)
        {
            this.TokenType = tokenType;
        }

        private SignitureToken() { }

        /// <summary>
        /// Returns the uncompressed value from a compressed field.
        /// </summary>
        /// <param name="contents">The contents of the signiture</param>
        /// <param name="offset">The offset of the values first byte</param>
        /// <returns>The uncompressed value</returns>
        /// <exception cref="InvalidOperationException">
        /// The first byte of the compressed value is not a valid value.
        /// </exception>
        internal static int GetCompressedValue(byte[] contents, Offset offset)
        {
            int currentOffset = offset;
            byte firstByte = contents[offset];
            uint value = 0;

            // These values are always stored as big-endian, big end first. We need to read the
            // contents of the bytes and flip the order for BitConvertor and FieldConvertor to
            // work correctly here.

            if(firstByte <= 127)
            {
                value = (uint)(((int)contents[currentOffset]) & CompressedByteMask);
                offset.Shift(1);
            }
            else if(firstByte <= 191)
            {
                byte[] toRead = new byte[2];
                toRead[0] = contents[currentOffset + 1];
                toRead[1] = contents[currentOffset + 0];
                value = FieldReader.ToUInt32(toRead, 0, 2) & CompressedShortMask;
                offset.Shift(2);        // Dont forget to move the offset on
            }
            else if(firstByte <= 223)
            {
                byte[] toRead = new byte[4];
                toRead[0] = contents[currentOffset + 3];
                toRead[1] = contents[currentOffset + 2];
                toRead[2] = contents[currentOffset + 1];
                toRead[3] = contents[currentOffset + 0];
                value = FieldReader.ToUInt32(toRead, 0, 4) & CompressedIntMask;
                offset.Shift(4);
            }
            else
            {
                throw new InvalidOperationException("The value of the first byte is not a valid compressed value.");
            }

            return (int)value;
        }

        /// <summary>
        /// Indicates the type of SignitureToken
        /// </summary>
        public SignitureTokens TokenType { get; set; }
    }
}