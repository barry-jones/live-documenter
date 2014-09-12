using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection.Core.PE {

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

    public sealed class FileHeader {
        public MachineTypes Machine;
        public UInt16 NumberOfSections;
        public UInt32 TimeDateStamp;
        public UInt32 PointerToSymbolTable;
        public UInt32 NumberOfSymbols;
        public UInt16 SizeOfOptionalHeader;
        public FileCharacteristics Characteristics;

		/// <summary>
		/// Initialises the ImageFileHeader class and populates it with
		/// the specific data from the file contents
		/// </summary>
		/// <param name="fileContents">The contents of the file being read as an array</param>
		/// <param name="offset">The offset for the image file header</param>
        public FileHeader(byte[] fileContents, Offset offset) {
			this.Machine = (MachineTypes)BitConverter.ToUInt16(fileContents, offset.Shift(2));
			this.NumberOfSections = BitConverter.ToUInt16(fileContents, offset.Shift(2));
			this.TimeDateStamp = BitConverter.ToUInt32(fileContents, offset.Shift(4));
			this.PointerToSymbolTable = BitConverter.ToUInt32(fileContents, offset.Shift(4));
			this.NumberOfSymbols = BitConverter.ToUInt32(fileContents, offset.Shift(4));
			this.SizeOfOptionalHeader = BitConverter.ToUInt16(fileContents, offset.Shift(2));
			this.Characteristics = (FileCharacteristics)BitConverter.ToUInt16(fileContents, offset.Shift(2));
        }
    }
}
