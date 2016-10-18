using System;

namespace TheBoxSoftware.Reflection.Core.COFF
{
    public class MethodSpecMetadataTableRow : MetadataRow
    {
        /// <summary>
        /// Initialises a new instance of the MethodSpecMetadataTableRow class
        /// </summary>
        /// <param name="stream">The stream containing the metadata</param>
        /// <param name="contents">The contents of the file</param>
        /// <param name="offset">The offset of the current row</param>
        public MethodSpecMetadataTableRow(MetadataStream stream, byte[] contents, Offset offset)
        {
            this.FileOffset = offset;
            this.Method = new CodedIndex(stream, offset, CodedIndexes.MethodDefOrRef);
            this.Instantiation = FieldReader.ToUInt32(contents, offset.Shift(stream.SizeOfBlobIndexes), stream.SizeOfBlobIndexes);
        }

        /// <summary>
        /// An index in to a MethodDef or MemberRef table, a MethodDefOrRef
        /// encoded index
        /// </summary>
        public CodedIndex Method { get; set; }

        /// <summary>
        /// An index in to the blob heap
        /// </summary>
        public UInt32 Instantiation { get; set; }
    }
}