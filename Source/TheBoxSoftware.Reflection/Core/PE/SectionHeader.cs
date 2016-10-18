using System;

namespace TheBoxSoftware.Reflection.Core.PE
{
    public class SectionHeader
    {
        public SectionHeader(byte[] fileContents, Offset offset)
        {
            Name = ReadName(fileContents, offset);
            VirtualSize = BitConverter.ToUInt32(fileContents, offset.Shift(4));
            VirtualAddress = BitConverter.ToUInt32(fileContents, offset.Shift(4));
            SizeOfRawData = BitConverter.ToUInt32(fileContents, offset.Shift(4));
            PointerToRawData = BitConverter.ToUInt32(fileContents, offset.Shift(4));
            PointerToRelocations = BitConverter.ToUInt32(fileContents, offset.Shift(4));
            PointerToLinenumbers = BitConverter.ToUInt32(fileContents, offset.Shift(4));
            NumberOfRelocations = BitConverter.ToUInt16(fileContents, offset.Shift(2));
            NumberOfLineNumbers = BitConverter.ToUInt16(fileContents, offset.Shift(2));
            Characteristics = (SectionCharacteristics)BitConverter.ToUInt32(fileContents, offset.Shift(4));
        }

        private string ReadName(byte[] fileContents, Offset offset)
        {
            const byte MaxNameSize = 8;
            const char TerminatingChar = '\0';

            char[] tempName = new char[MaxNameSize];

            for(int i = 0; i < MaxNameSize; i++)
            {
                char current = Convert.ToChar(fileContents.GetValue(offset.Shift(1)));
                if(current != TerminatingChar)
                {
                    tempName[i] = current;
                }
            }

            return new string(tempName).Trim(TerminatingChar);
        }

        public string Name { get; set; }

        public UInt32 VirtualSize { get; set; }

        public UInt32 VirtualAddress { get; set; }

        public UInt32 SizeOfRawData { get; set; }

        public UInt32 PointerToRawData { get; set; }

        public UInt32 PointerToRelocations { get; set; }

        public UInt32 PointerToLinenumbers { get; set; }

        public UInt16 NumberOfRelocations { get; set; }

        public UInt16 NumberOfLineNumbers { get; set; }

        public SectionCharacteristics Characteristics { get; set; }
    }
}