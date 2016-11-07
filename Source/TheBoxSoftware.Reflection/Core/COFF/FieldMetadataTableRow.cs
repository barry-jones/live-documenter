
namespace TheBoxSoftware.Reflection.Core.COFF
{
    /// <remarks>Modified to use 4-byte heap fields</remarks>
    public class FieldMetadataTableRow : MetadataRow
    {
        private FieldAttributes _flags;
        private StringIndex _nameIndex;
        private BlobIndex _signitureIndex;

        /// <summary>
        /// Initialises a new instance of the FieldMetadataTableRow
        /// </summary>
        /// <param name="contents">The contents of the file</param>
        /// <param name="offset">The offset of the current row</param>
        public FieldMetadataTableRow(byte[] contents, Offset offset, byte sizeOfStringIndex, byte sizeOfBlobIndex)
        {
            this.FileOffset = offset;

            _flags = (FieldAttributes)FieldReader.ToUInt16(contents, offset.Shift(2));
            _nameIndex = new StringIndex(contents, sizeOfStringIndex, offset);
            _signitureIndex = new BlobIndex(sizeOfBlobIndex, contents, Reflection.Signitures.Signitures.Field, offset);
        }

        /// <summary>A 2-byte mask of FieldAttributes</summary>
        public FieldAttributes Flags
        {
            get { return _flags; }
            set { _flags = value; }
        }

        /// <summary>An index in to the string heap</summary>
        public StringIndex Name
        {
            get { return _nameIndex; }
            set { _nameIndex = value; }
        }

        /// <summary>An index in to the Blob heap</summary>
        public BlobIndex Signiture
        {
            get { return _signitureIndex; }
            set { _signitureIndex = value; }
        }
    }
}