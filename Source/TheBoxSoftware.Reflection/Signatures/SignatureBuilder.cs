
namespace TheBoxSoftware.Reflection.Signatures
{
    using System;
    using Core;
    using Core.COFF;

    internal class SignatureBuilder
    {
        private const uint CompressedByteMask   = 0x0000007f;
        private const uint CompressedShortMask  = 0x00003fff;
        private const uint CompressedIntMask    = 0x1fffffff;

        private readonly BlobStream _stream;

        public SignatureBuilder(BlobStream underlyingStream)
        {
            _stream = underlyingStream;
        }

        public Signature Read(int offset)
        {
            if(offset < 0 || offset >= _stream.GetLength())
                return null;

            byte[] signitureBytes = GetSignitureBytes(offset);
            Signature created = new Signature();

            int check = signitureBytes[0] & 0x0F;
            if(check == 0x08)
            {
                ReadPropertySignature(created);
            }
            else if (check == 0x06)
            {
                ReadFieldSignature(created);
            }
            else
            {
                ReadMethodSignature(signitureBytes, created);
            }

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

        private void ReadMethodSignature(byte[] signitureBytes, Signature created)
        {
            created.Type = Signatures.MethodDef;

            Offset index = 0;

            var convention = new CallingConventionSignatureToken(signitureBytes, index);
            created.Tokens.Add(convention);
            if((convention.Convention & CallingConventions.Generic) != 0)
            {
                var genericParam = new GenericParamaterCountSignatureToken(signitureBytes, index);
                created.Tokens.Add(genericParam);
            }
            var paramCount = new ParameterCountSignatureToken(signitureBytes, index);
            created.Tokens.Add(paramCount);
            var returnType = new ReturnTypeSignatureToken(signitureBytes, index);
            created.Tokens.Add(returnType);
        }

        private void ReadPropertySignature(Signature created)
        {
            created.Type = Signatures.Property;
        }

        private void ReadFieldSignature(Signature created)
        {
            created.Type = Signatures.Field;
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
