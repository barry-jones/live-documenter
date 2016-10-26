
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
        /// <summary>
        /// Initialises the ImageFileHeader class and populates it with
        /// the specific data from the file contents
        /// </summary>
        /// <param name="fileContents">The contents of the file being read as an array</param>
        /// <param name="offset">The offset for the image file header</param>
        public FileHeader(byte[] fileContents, Offset offset)
        {
            this.Machine = (MachineTypes)BitConverter.ToUInt16(fileContents, offset.Shift(2));
            this.NumberOfSections = BitConverter.ToUInt16(fileContents, offset.Shift(2));
            this.TimeDateStamp = BitConverter.ToUInt32(fileContents, offset.Shift(4));
            this.PointerToSymbolTable = BitConverter.ToUInt32(fileContents, offset.Shift(4));
            this.NumberOfSymbols = BitConverter.ToUInt32(fileContents, offset.Shift(4));
            this.SizeOfOptionalHeader = BitConverter.ToUInt16(fileContents, offset.Shift(2));
            this.Characteristics = (FileCharacteristics)BitConverter.ToUInt16(fileContents, offset.Shift(2));
        }

        public MachineTypes Machine { get; set; }

        public UInt16 NumberOfSections { get; set; }

        public UInt32 TimeDateStamp { get; set; }

        public UInt32 PointerToSymbolTable { get; set; }

        public UInt32 NumberOfSymbols { get; set; }

        public UInt16 SizeOfOptionalHeader { get; set; }

        public FileCharacteristics Characteristics { get; set; }
    }
}