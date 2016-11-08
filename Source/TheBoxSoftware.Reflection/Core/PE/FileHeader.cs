
namespace TheBoxSoftware.Reflection.Core.PE
{
    using System;

    //
    // Byte structure of File Header
    // 
    // offset   size    field name
    // 0        2       machine
    // 2        2       number of sections
    // 4        4       time date stamp
    // 8        4       pointer to symbol table
    // 12       4       number of symbols
    // 16       2       size of optional header
    // 18       2       characteristics

    public sealed class FileHeader
    {
        private FileCharacteristics _characteristics;
        private ushort _sizeOfOptionalHeader;
        private uint _numberOfSymbols;
        private uint _pointerToSymbolTable;
        private uint _timeDateStamp;
        private ushort _numberOfSections;
        private MachineTypes _machine;

        /// <summary>
        /// Initialises the ImageFileHeader class and populates it with
        /// the specific data from the file contents
        /// </summary>
        /// <param name="fileContents">The contents of the file being read as an array</param>
        /// <param name="offset">The offset for the image file header</param>
        public FileHeader(byte[] fileContents, Offset offset)
        {
            _machine = (MachineTypes)BitConverter.ToUInt16(fileContents, offset.Shift(2));
            _numberOfSections = BitConverter.ToUInt16(fileContents, offset.Shift(2));
            _timeDateStamp = BitConverter.ToUInt32(fileContents, offset.Shift(4));
            _pointerToSymbolTable = BitConverter.ToUInt32(fileContents, offset.Shift(4));
            _numberOfSymbols = BitConverter.ToUInt32(fileContents, offset.Shift(4));
            _sizeOfOptionalHeader = BitConverter.ToUInt16(fileContents, offset.Shift(2));
            _characteristics = (FileCharacteristics)BitConverter.ToUInt16(fileContents, offset.Shift(2));
        }

        public MachineTypes Machine
        {
            get { return _machine; }
            set { _machine = value; }
        }

        public ushort NumberOfSections
        {
            get { return _numberOfSections; }
            set { _numberOfSections = value; }
        }

        public uint TimeDateStamp
        {
            get { return _timeDateStamp; }
            set { _timeDateStamp = value; }
        }

        public uint PointerToSymbolTable
        {
            get { return _pointerToSymbolTable; }
            set { _pointerToSymbolTable = value; }
        }

        public uint NumberOfSymbols
        {
            get { return _numberOfSymbols; }
            set { _numberOfSymbols = value; }
        }

        public ushort SizeOfOptionalHeader
        {
            get { return _sizeOfOptionalHeader; }
            set { _sizeOfOptionalHeader = value; }
        }

        public FileCharacteristics Characteristics
        {
            get { return _characteristics; }
            set { _characteristics = value; }
        }
    }
}