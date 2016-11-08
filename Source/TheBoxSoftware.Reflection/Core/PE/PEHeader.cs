
namespace TheBoxSoftware.Reflection.Core.PE
{
    using System;
    using System.Collections.Generic;

    public class PEHeader
    {
        public const int Size32Bit = 96;
        public const int Size64Bit = 112;

        private Dictionary<DataDirectories, DataDirectory> _dataDirectories;
        private uint _numberOfRvaAndSizes;
        private uint _loaderFlags;
        private ulong _sizeOfHeapCommit;
        private ulong _sizeOfHeapReserve;
        private ulong _sizeOfStackCommit;
        private ulong _sizeOfStackReserve;
        private ushort _dllCharacteristics;
        private ushort _subsystem;
        private uint _checksum;
        private uint _sizeOfHeaders;
        private uint _sizeOfImage;
        private uint _win32VersionValue;
        private ushort _minorSubSystemVersion;
        private ushort _majorSubSystemVersion;
        private ushort _minorImageVersion;
        private ushort _majorImageVersion;
        private ushort _minorOperatingSystemVersion;
        private ushort _majorOperatingSystemVersion;
        private uint _fileAlignment;
        private uint _sectionAlignment;
        private ulong _imageBase;
        private uint _baseOfData;
        private uint _baseOfCode;
        private uint _addressOfEntryPoint;
        private uint _sizeOfUnitializedData;
        private uint _sizeOfInitializedData;
        private uint _sizeOfCode;
        private byte _minorLinkerVersion;
        private byte _majorLinkerVersion;
        private FileMagicNumbers _magic;
        private int _size;

        public PEHeader(byte[] fileContents, Offset offset)
        {
            // standard fields
            _magic = (FileMagicNumbers)BitConverter.ToUInt16(fileContents, offset.Shift(2));
            _majorLinkerVersion = fileContents[offset.Shift(1)];
            _minorLinkerVersion = fileContents[offset.Shift(1)];
            _sizeOfCode = BitConverter.ToUInt32(fileContents, offset.Shift(4));
            _sizeOfInitializedData = BitConverter.ToUInt32(fileContents, offset.Shift(4));
            _sizeOfUnitializedData = BitConverter.ToUInt32(fileContents, offset.Shift(4));
            _addressOfEntryPoint = BitConverter.ToUInt32(fileContents, offset.Shift(4));
            _baseOfCode = BitConverter.ToUInt32(fileContents, offset.Shift(4));
            _baseOfData = this.Is32
                ? BitConverter.ToUInt32(fileContents, offset.Shift(4))
                : 0;  // does not exist in 64 bit files

            // windows nt specific fields
            _imageBase = this.Is32
                ? BitConverter.ToUInt32(fileContents, offset.Shift(4))
                : BitConverter.ToUInt64(fileContents, offset.Shift(8));
            _sectionAlignment = BitConverter.ToUInt32(fileContents, offset.Shift(4));
            _fileAlignment = BitConverter.ToUInt32(fileContents, offset.Shift(4));
            _majorOperatingSystemVersion = BitConverter.ToUInt16(fileContents, offset.Shift(2));
            _minorOperatingSystemVersion = BitConverter.ToUInt16(fileContents, offset.Shift(2));
            _majorImageVersion = BitConverter.ToUInt16(fileContents, offset.Shift(2));
            _minorImageVersion = BitConverter.ToUInt16(fileContents, offset.Shift(2));
            _majorSubSystemVersion = BitConverter.ToUInt16(fileContents, offset.Shift(2));
            _minorSubSystemVersion = BitConverter.ToUInt16(fileContents, offset.Shift(2));
            _win32VersionValue = BitConverter.ToUInt32(fileContents, offset.Shift(4));
            _sizeOfImage = BitConverter.ToUInt32(fileContents, offset.Shift(4));
            _sizeOfHeaders = BitConverter.ToUInt32(fileContents, offset.Shift(4));
            _checksum = BitConverter.ToUInt32(fileContents, offset.Shift(4));
            _subsystem = BitConverter.ToUInt16(fileContents, offset.Shift(2));
            _dllCharacteristics = BitConverter.ToUInt16(fileContents, offset.Shift(2));
            _sizeOfStackReserve = this.Is32
                ? BitConverter.ToUInt32(fileContents, offset.Shift(4))
                : BitConverter.ToUInt64(fileContents, offset.Shift(8));
            _sizeOfStackCommit = this.Is32
                ? BitConverter.ToUInt32(fileContents, offset.Shift(4))
                : BitConverter.ToUInt64(fileContents, offset.Shift(8));
            _sizeOfHeapReserve = this.Is32
                ? BitConverter.ToUInt32(fileContents, offset.Shift(4))
                : BitConverter.ToUInt64(fileContents, offset.Shift(8));
            _sizeOfHeapCommit = this.Is32
                ? BitConverter.ToUInt32(fileContents, offset.Shift(4))
                : BitConverter.ToUInt64(fileContents, offset.Shift(8));
            _loaderFlags = BitConverter.ToUInt32(fileContents, offset.Shift(4));
            _numberOfRvaAndSizes = BitConverter.ToUInt32(fileContents, offset.Shift(4));

            _dataDirectories = new Dictionary<DataDirectories, DataDirectory>();

            // read the data directories
            for(int i = 0; i < 16; i++)
            {
                int startIndex = offset.Shift(DataDirectory.SizeInBytes);
                byte[] directoryContents = new byte[DataDirectory.SizeInBytes];
                for(int j = 0; j < DataDirectory.SizeInBytes; j++)
                {
                    directoryContents[j] = fileContents[startIndex + j];
                }

                _dataDirectories.Add((DataDirectories)i, new DataDirectory(directoryContents, (DataDirectories)i));
            }

            _size = offset;
        }

        public bool Is32
        {
            get { return this.Magic == FileMagicNumbers.Bit32; }
        }

        public int Size
        {
            get { return _size; }
            set { _size = value; }
        }

        public FileMagicNumbers Magic
        {
            get { return _magic; }
            set { _magic = value; }
        }

        public byte MajorLinkerVersion
        {
            get { return _majorLinkerVersion; }
            set { _majorLinkerVersion = value; }
        }

        public byte MinorLinkerVersion
        {
            get { return _minorLinkerVersion; }
            set { _minorLinkerVersion = value; }
        }

        public uint SizeOfCode
        {
            get { return _sizeOfCode; }
            set { _sizeOfCode = value; }
        }

        public uint SizeOfInitializedData
        {
            get { return _sizeOfInitializedData; }
            set { _sizeOfUnitializedData = value; }
        }

        public uint SizeOfUnitializedData
        {
            get { return _sizeOfUnitializedData; }
            set { _sizeOfUnitializedData = value; }
        }

        public uint AddressOfEntryPoint
        {
            get { return _addressOfEntryPoint; }
            set { _addressOfEntryPoint = value; }
        }

        public uint BaseOfCode
        {
            get { return _baseOfCode; }
            set { _baseOfCode = value; }
        }

        public uint BaseOfData
        {
            get { return _baseOfData; }
            set { _baseOfData = value; }
        }

        public ulong ImageBase
        {
            get { return _imageBase; }
            set { _imageBase = value; }
        }

        public uint SectionAlignment
        {
            get { return _sectionAlignment; }
            set { _sectionAlignment = value; }
        }

        public uint FileAlignment
        {
            get { return _fileAlignment; }
            set { _fileAlignment = value; }
        }

        public ushort MajorOperatingSystemVersion
        {
            get { return _majorOperatingSystemVersion; }
            set { _majorOperatingSystemVersion = value; }
        }

        public ushort MinorOperatingSystemVersion
        {
            get { return _minorOperatingSystemVersion; }
            set { _minorOperatingSystemVersion = value; }
        }

        public ushort MajorImageVersion
        {
            get { return _majorImageVersion; }
            set { _majorImageVersion = value; }
        }

        public ushort MinorImageVersion
        {
            get { return _minorImageVersion; }
            set { _minorImageVersion = value; }
        }

        public ushort MajorSubSystemVersion
        {
            get { return _majorSubSystemVersion; }
            set { _majorSubSystemVersion = value; }
        }

        public ushort MinorSubSystemVersion
        {
            get { return _minorSubSystemVersion; }
            set { _minorSubSystemVersion = value; }
        }

        public uint Win32VersionValue
        {
            get { return _win32VersionValue; }
            set { _win32VersionValue = value; }
        }

        public uint SizeOfImage
        {
            get { return _sizeOfImage; }
            set { _sizeOfImage = value; }
        }

        public uint SizeOfHeaders
        {
            get { return _sizeOfHeaders; }
            set { _sizeOfHeaders = value; }
        }

        public uint Checksum
        {
            get { return _checksum; }
            set { _checksum = value; }
        }

        public ushort Subsystem
        {
            get { return _subsystem; }
            set { _subsystem = value; }
        }

        public ushort DllCharacteristics
        {
            get { return _dllCharacteristics; }
            set { _dllCharacteristics = value; }
        }

        public ulong SizeOfStackReserve
        {
            get { return _sizeOfStackReserve; }
            set { _sizeOfStackReserve = value; }
        }

        public ulong SizeOfStackCommit
        {
            get { return _sizeOfStackCommit; }
            set { _sizeOfStackCommit = value; }
        }

        public ulong SizeOfHeapReserve
        {
            get { return _sizeOfHeapReserve; }
            set { _sizeOfHeapReserve = value; }
        }

        public ulong SizeOfHeapCommit
        {
            get { return _sizeOfHeapCommit; }
            set { _sizeOfHeapCommit = value; }
        }

        public uint LoaderFlags
        {
            get { return _loaderFlags; }
            set { _loaderFlags = value; }
        }

        public uint NumberOfRVAAndSizes
        {
            get { return _numberOfRvaAndSizes; }
            set { _numberOfRvaAndSizes = value; }
        }

        public Dictionary<DataDirectories, DataDirectory> DataDirectories
        {
            get { return _dataDirectories; }
            set { _dataDirectories = value; }
        }
    }
}