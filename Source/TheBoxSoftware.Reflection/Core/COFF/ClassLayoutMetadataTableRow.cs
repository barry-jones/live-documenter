
namespace TheBoxSoftware.Reflection.Core.COFF
{
    using System;

    /// <summary>
    /// Describes a layout of a type when it is required to be layed out the same as unmanaged structures
    /// </summary>
    public class ClassLayoutMetadataTableRow : MetadataRow
    {
        private Index _parentIndex;
        private uint _classSize;
        private ushort _packingSize;

        /// <summary>
        /// Initialises a new instance of the ClassLayoutMetadataTableRow
        /// </summary>
        /// <param name="contents">The contents of the file</param>
        /// <param name="offset">The offset of this row</param>
        /// <param name="sizeOfTypeDefIndex">The size of the indexes to the type def table.</param>
        public ClassLayoutMetadataTableRow(byte[] contents, Offset offset, int sizeOfTypeDefIndex)
        {
            this.FileOffset = offset;

            _packingSize = BitConverter.ToUInt16(contents, offset.Shift(2));
            _classSize = BitConverter.ToUInt32(contents, offset.Shift(4));
            _parentIndex = new Index(contents, offset, sizeOfTypeDefIndex);
        }

        public ushort PackingSize
        {
            get { return _packingSize; }
            set { _packingSize = value; }
        }

        public uint ClassSize
        {
            get { return _classSize; }
            set { _classSize = value; }
        }

        public Index Parent
        {
            get { return _parentIndex; }
            set { _parentIndex = value; }
        }
    }
}