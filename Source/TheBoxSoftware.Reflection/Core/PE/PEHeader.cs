using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection.Core.PE {

    public class PEHeader {
        #region Contents
        public const int Size32Bit = 96;
        public const int Size64Bit = 112;
        #endregion

        #region Fields
        public int Size;
        public FileMagicNumbers Magic;
        public byte MajorLinkerVersion;
        public byte MinorLinkerVersion;
        public UInt32 SizeOfCode;
        public UInt32 SizeOfInitializedData;
        public UInt32 SizeOfUnitializedData;
        public UInt32 AddressOfEntryPoint;
        public UInt32 BaseOfCode;
        public UInt32 BaseOfData;
        public UInt64 ImageBase;
        public UInt32 SectionAlignment;
        public UInt32 FileAlignment;
        public UInt16 MajorOperatingSystemVersion;
        public UInt16 MinorOperatingSystemVersion;
        public UInt16 MajorImageVersion;
        public UInt16 MinorImageVersion;
        public UInt16 MajorSubSystemVersion;
        public UInt16 MinorSubSystemVersion;
        public UInt32 Win32VersionValue;
        public UInt32 SizeOfImage;
        public UInt32 SizeOfHeaders;
        public UInt32 Checksum;
        public UInt16 Subsystem;
        public UInt16 DllCharacteristics;
        public UInt64 SizeOfStackReserve;
        public UInt64 SizeOfStackCommit;
        public UInt64 SizeOfHeapReserve;
        public UInt64 SizeOfHeapCommit;
        public UInt32 LoaderFlags;
        public UInt32 NumberOfRVAAndSizes;
        public Dictionary<DataDirectories, DataDirectory> DataDirectories = new Dictionary<DataDirectories, DataDirectory>();
        #endregion

        public PEHeader(byte[] fileContents, Offset offset) {

            // read the magic number to determine if this is a 32 bit file
			this.Magic = (FileMagicNumbers)BitConverter.ToUInt16(fileContents, offset.Shift(2));
			this.MajorLinkerVersion = fileContents[offset.Shift(1)];
			this.MinorLinkerVersion = fileContents[offset.Shift(1)];
			this.SizeOfCode = BitConverter.ToUInt32(fileContents, offset.Shift(4));
			this.SizeOfInitializedData = BitConverter.ToUInt32(fileContents, offset.Shift(4));
			this.SizeOfUnitializedData = BitConverter.ToUInt32(fileContents, offset.Shift(4));
			this.AddressOfEntryPoint = BitConverter.ToUInt32(fileContents, offset.Shift(4));
			this.BaseOfCode = BitConverter.ToUInt32(fileContents, offset.Shift(4));
            this.BaseOfData = this.Is32
				? BitConverter.ToUInt32(fileContents, offset.Shift(4))
                : 0;  // does not exist in 64 bit files
            this.ImageBase = this.Is32
				? BitConverter.ToUInt32(fileContents, offset.Shift(4))
				: BitConverter.ToUInt64(fileContents, offset.Shift(8));
			this.SectionAlignment = BitConverter.ToUInt32(fileContents, offset.Shift(4));
			this.FileAlignment = BitConverter.ToUInt32(fileContents, offset.Shift(4));
			this.MajorOperatingSystemVersion = BitConverter.ToUInt16(fileContents, offset.Shift(2));
			this.MinorOperatingSystemVersion = BitConverter.ToUInt16(fileContents, offset.Shift(2));
			this.MajorImageVersion = BitConverter.ToUInt16(fileContents, offset.Shift(2));
			this.MinorImageVersion = BitConverter.ToUInt16(fileContents, offset.Shift(2));
			this.MajorSubSystemVersion = BitConverter.ToUInt16(fileContents, offset.Shift(2));
			this.MinorSubSystemVersion = BitConverter.ToUInt16(fileContents, offset.Shift(2));
			this.Win32VersionValue = BitConverter.ToUInt32(fileContents, offset.Shift(4));
			this.SizeOfImage = BitConverter.ToUInt32(fileContents, offset.Shift(4));
			this.SizeOfHeaders = BitConverter.ToUInt32(fileContents, offset.Shift(4));
			this.Checksum = BitConverter.ToUInt32(fileContents, offset.Shift(4));
			this.Subsystem = BitConverter.ToUInt16(fileContents, offset.Shift(2));
			this.DllCharacteristics = BitConverter.ToUInt16(fileContents, offset.Shift(2));
            this.SizeOfStackReserve = this.Is32
				? BitConverter.ToUInt32(fileContents, offset.Shift(4))
				: BitConverter.ToUInt64(fileContents, offset.Shift(8));
            this.SizeOfStackCommit = this.Is32
				? BitConverter.ToUInt32(fileContents, offset.Shift(4))
				: BitConverter.ToUInt64(fileContents, offset.Shift(8));
            this.SizeOfHeapReserve = this.Is32
				? BitConverter.ToUInt32(fileContents, offset.Shift(4))
				: BitConverter.ToUInt64(fileContents, offset.Shift(8));
            this.SizeOfHeapCommit = this.Is32
				? BitConverter.ToUInt32(fileContents, offset.Shift(4))
				: BitConverter.ToUInt64(fileContents, offset.Shift(8));
			this.LoaderFlags = BitConverter.ToUInt32(fileContents, offset.Shift(4));
			this.NumberOfRVAAndSizes = BitConverter.ToUInt32(fileContents, offset.Shift(4));

            // read the data directories
			List<byte> bytes = new List<byte>(fileContents);
            for (int i = 0; i < 16; i++) {
                this.DataDirectories.Add(
                    (DataDirectories)i, 
                    new DataDirectory(
                        bytes.GetRange(
                            offset.Shift(DataDirectory.SizeInBytes), 
                            DataDirectory.SizeInBytes).ToArray(),
							(DataDirectories)i
                        )
                    );
            }

            this.Size = offset;
        }

        #region Properties
        public bool Is32 {
            get { return this.Magic == FileMagicNumbers.Bit32; }
        }
        #endregion
    }
}
