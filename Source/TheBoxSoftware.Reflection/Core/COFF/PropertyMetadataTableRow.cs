
namespace TheBoxSoftware.Reflection.Core.COFF
{
    // p241 ECMA-335

    public sealed class PropertyMetadataTableRow : MetadataRow
    {
        private PropertyAttributes _attributes;
        private StringIndex _nameIndex;
        private uint _typeIndex;

        /// <summary>
        /// Initialises a new instance of the PropertyMetadataTableRow class
        /// </summary>
        /// <param name="contents">The contents of the file</param>
        /// <param name="offset">The offset of this row</param>
        public PropertyMetadataTableRow(byte[] contents, Offset offset, byte sizeOfStringIndex, byte sizeOfBlobIndex)
        {
            FileOffset = offset;

            _attributes = (PropertyAttributes)FieldReader.ToUInt16(contents, offset.Shift(2));
            _nameIndex = new StringIndex(contents, sizeOfStringIndex, offset);
            _typeIndex = FieldReader.ToUInt32(contents, offset.Shift(sizeOfBlobIndex), sizeOfBlobIndex);
        }

        /// <summary>
        /// A 2-byte bitmask of PropertyAttributes, only the values set are specified.
        /// </summary>
        public PropertyAttributes Attributes
        {
            get { return _attributes; }
            set { _attributes = value; }
        }

        /// <summary>
        /// An index in to the string heap
        /// </summary>
        public StringIndex NameIndex
        {
            get { return _nameIndex; }
            set { _nameIndex = value; }
        }

        /// <summary>
        /// An index in to the blob heap for the signiture
        /// </summary>
        public uint TypeIndex
        {
            get { return _typeIndex; }
            set { _typeIndex = value; }
        }
    }
}