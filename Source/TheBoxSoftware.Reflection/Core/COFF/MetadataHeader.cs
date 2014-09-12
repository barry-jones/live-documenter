using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection.Core.COFF
{
	public class MetadataHeader {
		public MetadataHeader(byte[] contents, int address) {
			Offset offset = address;
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
			for (int j = 0; j < actualVersionLength; j++) {
				tempName[j] = Convert.ToChar(contents.GetValue(offset++));
			}
			this.Version = new string(tempName);
			this.Version = this.Version.TrimEnd('\0');
			this.Flags = BitConverter.ToUInt16(contents, offset.Shift(2));
			this.NumberOfMetaDataStreams = BitConverter.ToUInt16(contents, offset.Shift(2));
			this.Headers = new MetadataStreamHeader[this.NumberOfMetaDataStreams];
			for (int i = 0; i < this.NumberOfMetaDataStreams; i++) {
				this.Headers[i] = new MetadataStreamHeader(contents, ref offset);
			}
		}

		#region Properties
		public uint		Signiture;					// d-word always 0x424a5342
		public ushort	MajorVersion;
		public ushort	MinorVersion;
		public uint		Reserved;
		public uint		VersionLength;
		public string	Version;
		public ushort	Flags;
		public ushort	NumberOfMetaDataStreams;
		public MetadataStreamHeader[] Headers;
		#endregion
	}
}
