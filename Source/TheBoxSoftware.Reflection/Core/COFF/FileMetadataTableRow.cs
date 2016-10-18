using System;

namespace TheBoxSoftware.Reflection.Core.COFF
{
    public class FileMetadataTableRow : MetadataRow
    {
        /// <summary>
        /// Initialises a new instance of the FileMetadataTableRow class
        /// </summary>
        /// <param name="stream">The stream containing the metadata</param>
        /// <param name="contents">The contents of the file</param>
        /// <param name="offset">The offset of the current row</param>
        public FileMetadataTableRow(MetadataStream stream, byte[] contents, Offset offset)
        {
            this.FileOffset = offset;
            this.Flags = FieldReader.ToUInt32(contents, offset.Shift(4));
            this.Name = new StringIndex(stream, offset);
            this.HashValue = FieldReader.ToUInt32(contents, offset.Shift(stream.SizeOfBlobIndexes), stream.SizeOfBlobIndexes);
        }

        /// <summary>
        /// A 4-byte bitmask of FileAttributes
        /// </summary>
        public UInt32 Flags { get; set; }

        /// <summary>
        /// An index in to the string heap
        /// </summary>
        public StringIndex Name { get; set; }

        /// <summary>
        /// An index in to the blob heap
        /// </summary>
        public UInt32 HashValue { get; set; }
    }
}