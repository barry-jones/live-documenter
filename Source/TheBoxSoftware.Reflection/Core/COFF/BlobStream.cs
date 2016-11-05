﻿
namespace TheBoxSoftware.Reflection.Core.COFF
{
    using System;

    /// <summary>
    /// The blob stream controls access to signitures contained in the pe/coff file.
    /// </summary>
    internal sealed class BlobStream : Stream
    {
        private byte[] _streamContents;

        /// <summary>
        /// Initialises a new instance of the BlobStream class
        /// </summary>
        /// <param name="copyFromContents">
        /// The array from which to copy the contents of the BlobStream from. Generall PeCoffFile.FileContents.
        /// </param>
        /// <param name="address">The start address of the blob stream</param>
        /// <param name="size">The size of the stream</param>
        internal BlobStream(byte[] copyFromContents, uint address, int size)
        {
            if((address + size) > copyFromContents.Length)
                throw new InvalidOperationException($"Not enough bytes to read from '{nameof(copyFromContents)}' to complete the operation.");

            // Read and store the underlying data for this stream
            _streamContents = new byte[size];
            uint endAddress = address + (uint)size;
            for(uint i = address; i < endAddress; i++)
            {
                _streamContents[i - address] = copyFromContents[i];
            }
        }

        /// <summary>
        /// Retrives a parsed <see cref="Signitures.Signiture"/> for the specified
        /// <paramref name="startOffset"/> and <paramref name="signiture"/> type.
        /// </summary>
        /// <param name="startOffset">The start of the signiture in the stream.</param>
        /// <param name="signiture">The type of signiture to parse.</param>
        /// <returns>The parsed signiture.</returns>
        public Signitures.Signiture GetSigniture(uint startOffset, Signitures.Signitures signiture)
        {
            return Signitures.Signiture.Create(
                _streamContents,
                (int)startOffset,
                signiture
                );
        }

        /// <summary>
        /// Obtains the contents of the signiture as a byte array.
        /// </summary>
        /// <param name="startOffset">The start offset of the signiture in the stream.</param>
        /// <returns>The contents of the signiture as a byte array.</returns>
        public byte[] GetSignitureContents(int startOffset)
        {
            byte length = _streamContents[startOffset++];
            byte[] signitureContents = new byte[length];
            for(int i = startOffset; i < startOffset + length; i++)
            {
                signitureContents[i - startOffset] = _streamContents[i];
            }
            return signitureContents;
        }
    }
}