
namespace TheBoxSoftware.Reflection.Signitures
{
    using System;
    using Core;
    using Core.COFF;

    internal class SignitureBuilder
    {
        private const uint CompressedByteMask   = 0x0000007f;
        private const uint CompressedShortMask  = 0x00003fff;
        private const uint CompressedIntMask    = 0x1fffffff;

        private readonly BlobStream _stream;

        public SignitureBuilder(BlobStream underlyingStream)
        {
            _stream = underlyingStream;
        }

        public Signiture Read(int offset)
        {
            if(offset < 0 || offset >= _stream.GetLength())
                return null;

            byte[] signitureBytes = GetSignitureBytes(offset);
            Signitures type = 0x00;

            switch((CallingConventions)(signitureBytes[0] & 0x0F))
            {
                case CallingConventions.Default:
                case CallingConventions.C:
                case CallingConventions.FastCall:
                case CallingConventions.StdCall:
                case CallingConventions.ThisCall:
                case CallingConventions.VarArg:
                    type = Signitures.MethodDef;
                    break;
                default:
                    type = Signitures.MethodRef;
                    break;
            }

            Signiture created = new Signiture();
            created.Type = type;

            return created;
        }

        public byte[] GetSignitureBytes(int offset)
        {
            if(offset < 0 || offset >= _stream.GetLength())
                throw new IndexOutOfRangeException($"The requested signiture {offset} is outside the range of the blob stream.");

            // first byte is supposed to the length
            uint lengthOfSigniture = GetLength(offset);
            return _stream.GetRange(offset + 1, lengthOfSigniture);
        }

        public uint GetLength(int offset)
        {
            if(offset < 0 || offset >= _stream.GetLength())
                throw new IndexOutOfRangeException($"The requested signiture {offset} is outside the range of the blob stream.");
            return DecompressUInt(offset);
        }

        private uint DecompressUInt(int offset)
        {
            uint length = 0;
            byte first = _stream.GetByte(offset);

            // bytes are read in reverse order to get back to little endian format for windows, these
            // numbers are stored in big endian format.

            if(first <= 127)
            {
                length = _stream.GetByte(offset) & CompressedByteMask;
            }
            else if(first <= 191)
            {
                byte[] toRead = new byte[2];
                toRead[0] = _stream.GetByte(offset + 1);
                toRead[1] = _stream.GetByte(offset + 0);
                length = FieldReader.ToUInt32(toRead, 0, 2) & CompressedShortMask;
            }
            else if(first <= 223)
            {
                byte[] toRead = new byte[4];
                toRead[0] = _stream.GetByte(offset + 3);
                toRead[1] = _stream.GetByte(offset + 2);
                toRead[2] = _stream.GetByte(offset + 1);
                toRead[3] = _stream.GetByte(offset + 0);
                length = FieldReader.ToUInt32(toRead, 0, 4) & CompressedIntMask;
            }

            return length;
        }
    }
}
