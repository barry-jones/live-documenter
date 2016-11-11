
namespace TheBoxSoftware.Reflection.Signitures
{
    using System;
    using Core.COFF;

    internal class SignitureBuilder
    {
        private readonly BlobStream _stream;

        public SignitureBuilder(BlobStream underlyingStream)
        {
            _stream = underlyingStream;
        }

        public Signiture Read(int offset)
        {
            if(offset < 0 || offset >= _stream.GetLength())
                return null;

            // first byte is supposed to the length
            throw new System.NotImplementedException();
        }

        public int GetLength(int offset)
        {
            if(offset < 0 || offset >= _stream.GetLength())
                throw new System.IndexOutOfRangeException($"The requested signiture {offset} is outside the range of the blob stream.");

            int length = 0;
            // all stuff in the blob is supposed to be compressed, there
            // can be different types of compressed numbers integer and
            // unsigned integer.
            // Numbers are stored in big endian format.
            byte first = _stream.GetByte(offset);
            if(first >> 7 == 0x00) // 0000 0000
            {
                // compressed as 0bbb bbbb
                length = first;
            }
            else if(first >> 6 == 0x02) // 0000 0010
            {
                length = 0xFF;
            }
            else if(first >> 5 == 0x06) // 0000 0110
            {
                length = 0xFF;
            }

            return length;
        }
    }
}
