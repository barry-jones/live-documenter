using System;
using System.Collections.Generic;

namespace TheBoxSoftware.Reflection.Core.COFF
{
    public struct MetadataStreamHeader
    {
        public uint Offset;
        public uint Size;
        public string Name;

        // The actual contents of the stream

        internal MetadataStreamHeader(byte[] contents, ref Offset offset)
        {
            Name = string.Empty; // default to allow methods to be called
            Offset = BitConverter.ToUInt32(contents, offset.Shift(4));
            Size = BitConverter.ToUInt32(contents, offset.Shift(4));
            Name = ReadName(contents, offset);
        }

        private string ReadName(byte[] contents, Offset offset)
        {
            const char nullCharacter = '\0';
            List<char> tempName = new List<char>(4);

            // Read the name which is a string, of seemingly any length, ending on a
            // 4 byte boundary. We need to look for the null terminating character while
            // continuing to add 4 to the offset. Or if we peek (look ahead) at the next
            // character which is a null termination character we can stop
            for(int j = offset.Shift(4); j < offset; j++)
            {
                char thisChar = Convert.ToChar(contents[j]);
                char nextChar = Convert.ToChar(contents[1 + j]);

                tempName.Add(thisChar);

                if(thisChar != nullCharacter && (j + 1) % 4 == 0 && j > (offset - 4))
                {
                    offset.Shift(4);
                }
            }

            return new string(tempName.ToArray()).TrimEnd(nullCharacter);
        }
    }
}