using System;
using System.Collections.Generic;

namespace TheBoxSoftware.Reflection.Core.PE
{
    public class PEHeader
    {
        public const int Size32Bit = 96;
        public const int Size64Bit = 112;

        public PEHeader(byte[] fileContents, Offset offset)
        {
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
            DataDirectories = new Dictionary<DataDirectories, DataDirectory>();

            // read the data directories
            for(int i = 0; i < 16; i++)
            {
                int startIndex = offset.Shift(DataDirectory.SizeInBytes);
                byte[] directoryContents = new byte[DataDirectory.SizeInBytes];
                for(int j = 0; j < DataDirectory.SizeInBytes; j++)
                {
                    directoryContents[j] = fileContents[startIndex + j];
                }

                this.DataDirectories.Add((DataDirectories)i, new DataDirectory(directoryContents, (DataDirectories)i));
            }

            this.Size = offset;
        }

        public bool Is32
        {
            get { return this.Magic == FileMagicNumbers.Bit32; }
        }

        public int Size { get; set; }

        public FileMagicNumbers Magic { get; set; }

        public byte MajorLinkerVersion { get; set; }

        public byte MinorLinkerVersion { get; set; }

        public UInt32 SizeOfCode { get; set; }

        public UInt32 SizeOfInitializedData { get; set; }

        public UInt32 SizeOfUnitializedData { get; set; }

        public UInt32 AddressOfEntryPoint { get; set; }

        public UInt32 BaseOfCode { get; set; }

        public UInt32 BaseOfData { get; set; }

        public UInt64 ImageBase { get; set; }

        public UInt32 SectionAlignment { get; set; }

        public UInt32 FileAlignment { get; set; }

        public UInt16 MajorOperatingSystemVersion { get; set; }

        public UInt16 MinorOperatingSystemVersion { get; set; }

        public UInt16 MajorImageVersion { get; set; }

        public UInt16 MinorImageVersion { get; set; }

        public UInt16 MajorSubSystemVersion { get; set; }

        public UInt16 MinorSubSystemVersion { get; set; }

        public UInt32 Win32VersionValue { get; set; }

        public UInt32 SizeOfImage { get; set; }

        public UInt32 SizeOfHeaders { get; set; }

        public UInt32 Checksum { get; set; }

        public UInt16 Subsystem { get; set; }

        public UInt16 DllCharacteristics { get; set; }

        public UInt64 SizeOfStackReserve { get; set; }

        public UInt64 SizeOfStackCommit { get; set; }

        public UInt64 SizeOfHeapReserve { get; set; }

        public UInt64 SizeOfHeapCommit { get; set; }

        public UInt32 LoaderFlags { get; set; }

        public UInt32 NumberOfRVAAndSizes { get; set; }

        public Dictionary<DataDirectories, DataDirectory> DataDirectories { get; set; }
    }
}