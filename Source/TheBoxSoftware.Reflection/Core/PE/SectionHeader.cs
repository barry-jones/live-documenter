
namespace TheBoxSoftware.Reflection.Core.PE
{
    using System;

    public class SectionHeader
    {
        private SectionCharacteristics _characteristics;
        private ushort _numberOfLines;
        private ushort _numberOfRelocations;
        private uint _pointerToLineNumbers;
        private uint _pointerToRelocations;
        private uint _pointerToRawData;
        private uint _sizeOfRawData;
        private uint _virtualAddress;
        private uint _virtualSize;
        private string _name;

        public SectionHeader() { }

        public SectionHeader(byte[] fileContents, Offset offset)
        {
            _name = ReadName(fileContents, offset);
            _virtualSize = BitConverter.ToUInt32(fileContents, offset.Shift(4));
            _virtualAddress = BitConverter.ToUInt32(fileContents, offset.Shift(4));
            _sizeOfRawData = BitConverter.ToUInt32(fileContents, offset.Shift(4));
            _pointerToRawData = BitConverter.ToUInt32(fileContents, offset.Shift(4));
            _pointerToRelocations = BitConverter.ToUInt32(fileContents, offset.Shift(4));
            _pointerToLineNumbers = BitConverter.ToUInt32(fileContents, offset.Shift(4));
            _numberOfRelocations = BitConverter.ToUInt16(fileContents, offset.Shift(2));
            _numberOfLines = BitConverter.ToUInt16(fileContents, offset.Shift(2));
            _characteristics = (SectionCharacteristics)BitConverter.ToUInt32(fileContents, offset.Shift(4));
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

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public uint VirtualSize
        {
            get { return _virtualSize; }
            set { _virtualSize = value; }
        }

        public uint VirtualAddress
        {
            get { return _virtualAddress; }
            set { _virtualAddress = value; }
        }

        public uint SizeOfRawData
        {
            get { return _sizeOfRawData; }
            set { _sizeOfRawData = value; }
        }

        public uint PointerToRawData
        {
            get { return _pointerToRawData; }
            set { _pointerToRawData = value; }
        }

        public uint PointerToRelocations
        {
            get { return _pointerToRelocations; }
            set { _pointerToRelocations = value; }
        }

        public uint PointerToLinenumbers
        {
            get { return _pointerToLineNumbers; }
            set { _pointerToLineNumbers = value; }
        }

        public ushort NumberOfRelocations
        {
            get { return _numberOfRelocations; }
            set { _numberOfRelocations = value; }
        }

        public ushort NumberOfLineNumbers
        {
            get { return _numberOfLines; }
            set { _numberOfLines = value; }
        }

        public SectionCharacteristics Characteristics
        {
            get { return _characteristics; }
            set { _characteristics = value; }
        }
    }
}