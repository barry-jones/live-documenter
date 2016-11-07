
namespace TheBoxSoftware.Reflection.Core.COFF
{
    using System;

    public class FieldLayoutMetadataTableRow : MetadataRow
    {
        private Index _fieldIndex;
        private uint _offset;

        /// <summary>
        /// Initialises a new instance of the FieldLayoutMetadataTableRow
        /// </summary>
        /// <param name="contents">The contents of the file</param>
        /// <param name="offset">The offset of the current row</param>
        public FieldLayoutMetadataTableRow(byte[] contents, Offset offset, IIndexDetails indexDetails)
        {
            this.FileOffset = offset;

            byte sizeOfFieldIndex = indexDetails.GetSizeOfIndex(MetadataTables.Field);

            _offset = BitConverter.ToUInt32(contents, offset.Shift(4));
            _fieldIndex = new Index(contents, offset, sizeOfFieldIndex);
        }

        public uint Offset
        {
            get { return _offset; }
            set { _offset = value; }
        }

        /// <summary>
        /// An index to the field table
        /// </summary>
        public Index Field
        {
            get { return _fieldIndex; }
            set { _fieldIndex = value; }
        }
    }
}