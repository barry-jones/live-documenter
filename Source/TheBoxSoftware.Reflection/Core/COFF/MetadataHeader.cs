using System;
using System.Collections.Generic;

namespace TheBoxSoftware.Reflection.Core.COFF
{
    public class MetadataHeader
    {
        public MetadataHeader(byte[] contents, uint address)
        {
            Offset offset = (int)address;
            List<byte> data = new List<byte>(contents);
            char[] tempName = new char[8];

            this.Signiture = BitConverter.ToUInt32(contents, offset.Shift(4));
            this.MajorVersion = BitConverter.ToUInt16(contents, offset.Shift(2));
            this.MinorVersion = BitConverter.ToUInt16(contents, offset.Shift(2));
            this.Reserved = BitConverter.ToUInt32(contents, offset.Shift(4));
            this.VersionLength = BitConverter.ToUInt32(contents, offset.Shift(4));

            // The length of the version string is on a 4 byte boundary so we need
            // to make sure the length is a multiple of 4
            int actualVersionLength = ((int)this.VersionLength % 4) + (int)this.VersionLength;
            tempName = new char[actualVersionLength];
            for(int j = 0; j < actualVersionLength; j++)
            {
                tempName[j] = Convert.ToChar(contents.GetValue(offset++));
            }
            this.Version = new string(tempName);
            this.Version = this.Version.TrimEnd('\0');
            this.Flags = BitConverter.ToUInt16(contents, offset.Shift(2));
            this.NumberOfMetaDataStreams = BitConverter.ToUInt16(contents, offset.Shift(2));
            this.Headers = new MetadataStreamHeader[this.NumberOfMetaDataStreams];
            for(int i = 0; i < this.NumberOfMetaDataStreams; i++)
            {
                this.Headers[i] = new MetadataStreamHeader(contents, ref offset);
            }
        }

        public uint Signiture { get; set; }                  // d-word always 0x424a5342

        public ushort MajorVersion { get; set; }

        public ushort MinorVersion { get; set; }

        public uint Reserved { get; set; }

        public uint VersionLength { get; set; }

        public string Version { get; set; }

        public ushort Flags { get; set; }

        public ushort NumberOfMetaDataStreams { get; set; }

        public MetadataStreamHeader[] Headers { get; set; }
    }
}