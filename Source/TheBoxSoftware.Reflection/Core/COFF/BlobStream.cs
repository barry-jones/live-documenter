
namespace TheBoxSoftware.Reflection.Core.COFF
{
    using System;

    /// <summary>
    /// The blob stream controls access to Signatures contained in the pe/coff file.
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
        public BlobStream(byte[] copyFromContents, uint address, int size)
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

        public byte[] GetRange(int offset, uint size)
        {
            byte[] contents = new byte[size];
            for(int i= 0; i < size; i++)
            {
                contents[i] = _streamContents[i + offset];
            }
            return contents;
        }

        /// <summary>
        /// Retrives a parsed <see cref="Signatures.Signature"/> for the specified
        /// <paramref name="startOffset"/> and <paramref name="signiture"/> type.
        /// </summary>
        /// <param name="startOffset">The start of the signiture in the stream.</param>
        /// <param name="signiture">The type of signiture to parse.</param>
        /// <returns>The parsed signiture.</returns>
        public Signatures.Signature GetSigniture(uint startOffset, Signatures.Signatures signiture)
        {
            return Signatures.Signature.Create(
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
            uint length = Signatures.SignatureToken.GetCompressedValue(_streamContents, startOffset++);    // The first byte is always the length
            //byte length = _streamContents[startOffset++];
            byte[] signitureContents = new byte[length];
            for(int i = startOffset; i < startOffset + length; i++)
            {
                signitureContents[i - startOffset] = _streamContents[i];
            }
            return signitureContents;
        }

        public uint GetLength(int offset)
        {
            return Signatures.SignatureToken.GetCompressedValue(_streamContents, offset);    // The first byte is always the length
        }

        internal int GetLength()
        {
            return _streamContents.Length;
        }

        public byte GetByte(int offset)
        {
            return _streamContents[offset];
        }
    }
}