
namespace TheBoxSoftware.Reflection.Core.COFF
{
    public class FileMetadataTableRow : MetadataRow
    {
        private uint _hashValue;
        private StringIndex _nameIndex;
        private FileAttributes _flags;

        /// <summary>
        /// Initialises a new instance of the FileMetadataTableRow class
        /// </summary>
        /// <param name="contents">The contents of the file</param>
        /// <param name="offset">The offset of the current row</param>
        public FileMetadataTableRow(byte[] contents, Offset offset, byte sizeOfBlobIndex, byte sizeOfStringIndex)
        {
            this.FileOffset = offset;

            _flags = (FileAttributes)FieldReader.ToUInt32(contents, offset.Shift(4));
            _nameIndex = new StringIndex(contents, sizeOfStringIndex, offset);
            _hashValue = FieldReader.ToUInt32(contents, offset.Shift(sizeOfBlobIndex), sizeOfBlobIndex);
        }

        /// <summary>
        /// A 4-byte bitmask of FileAttributes
        /// </summary>
        public FileAttributes Flags
        {
            get { return _flags; }
            set { _flags = value; }
        }

        /// <summary>
        /// An index in to the string heap
        /// </summary>
        public StringIndex Name
        {
            get { return _nameIndex; }
            set { _nameIndex = value; }
        }

        /// <summary>
        /// An index in to the blob heap
        /// </summary>
        public uint HashValue
        {
            get { return _hashValue; }
            set { _hashValue = value; }
        }
    }
}