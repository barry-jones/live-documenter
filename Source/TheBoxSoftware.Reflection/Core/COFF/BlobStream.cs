
namespace TheBoxSoftware.Reflection.Core.COFF
{
    /// <summary>
    /// The blob stream controls access to signitures contained in the pe/coff file.
    /// </summary>
    internal sealed class BlobStream : Stream
    {
        private byte[] _streamContents;
        /// <summary>
        /// Reference to file which owns the stream, this is stored to make it available to
        /// the signitures.
        /// </summary>
        private PeCoffFile _owningFile;

        /// <summary>
        /// Initialises a new instance of the BlobStream class
        /// </summary>
        /// <param name="file">The file the stream should be read from</param>
        /// <param name="address">The start address of the blob stream</param>
        /// <param name="size">The size of the stream</param>
        internal BlobStream(PeCoffFile file, int address, int size)
        {
            _owningFile = file;
            // Read and store the underlying data for this stream
            _streamContents = new byte[size];
            int endAddress = address + size;
            byte[] fileContents = file.FileContents;
            for(int i = address; i < endAddress; i++)
            {
                this._streamContents[i - address] = fileContents[i];
            }
        }

        /// <summary>
        /// Retrives a parsed <see cref="Signitures.Signiture"/> for the specified
        /// <paramref name="startOffset"/> and <paramref name="signiture"/> type.
        /// </summary>
        /// <param name="startOffset">The start of the signiture in the stream.</param>
        /// <param name="signiture">The type of signiture to parse.</param>
        /// <returns>The parsed signiture.</returns>
        public Signitures.Signiture GetSigniture(int startOffset, Signitures.Signitures signiture)
        {
            return Signitures.Signiture.Create(
                _streamContents,
                startOffset,
                _owningFile,
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
            byte length = this._streamContents[startOffset++];
            byte[] signitureContents = new byte[length];
            for(int i = startOffset; i < startOffset + length; i++)
            {
                signitureContents[i - startOffset] = _streamContents[i];
            }
            return signitureContents;
        }
    }
}