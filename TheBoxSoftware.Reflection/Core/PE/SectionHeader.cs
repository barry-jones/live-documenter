using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection.Core.PE {

    public class SectionHeader {
        public static int Size = 40;

        public string Name;
        // public UInt32 PhysicalAddress;
        public UInt32 VirtualSize;
        public UInt32 VirtualAddress;
        public UInt32 SizeOfRawData;
        public UInt32 PointerToRawData;
        public UInt32 PointerToRelocations;
        public UInt32 PointerToLinenumbers;
        public UInt16 NumberOfRelocations;
        public UInt16 NumberOfLineNumbers;
        public SectionCharacteristics Characteristics;

        public SectionHeader(byte[] fileContents, Offset offset) {
            char[] tempName = new char[8];
            for (int i = 0; i < 8; i++) {
				char current = Convert.ToChar(fileContents.GetValue(offset.Shift(1)));
                if (current != '\0') {
                    tempName[i] = current;
                }
            }
            this.Name = new string(tempName);
            this.Name = this.Name.TrimEnd('\0');
            // this.PhysicalAddress = BitConverter.ToUInt32(sectionHeader, offset += 4);
			this.VirtualSize = BitConverter.ToUInt32(fileContents, offset.Shift(4));
			this.VirtualAddress = BitConverter.ToUInt32(fileContents, offset.Shift(4));
			this.SizeOfRawData = BitConverter.ToUInt32(fileContents, offset.Shift(4));
			this.PointerToRawData = BitConverter.ToUInt32(fileContents, offset.Shift(4));
			this.PointerToRelocations = BitConverter.ToUInt32(fileContents, offset.Shift(4));
			this.PointerToLinenumbers = BitConverter.ToUInt32(fileContents, offset.Shift(4));
			this.NumberOfRelocations = BitConverter.ToUInt16(fileContents, offset.Shift(2));
			this.NumberOfLineNumbers = BitConverter.ToUInt16(fileContents, offset.Shift(2));
			this.Characteristics = (SectionCharacteristics)BitConverter.ToUInt32(fileContents, offset.Shift(4));
        }
    }
}
