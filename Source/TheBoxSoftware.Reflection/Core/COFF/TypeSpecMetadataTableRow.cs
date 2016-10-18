namespace TheBoxSoftware.Reflection.Core.COFF
{
    public class TypeSpecMetadataTableRow : MetadataRow
    {
        /// <summary>
        /// Initialises a new instance of the TypeSpecMetadataTableRow class
        /// </summary>
        /// <param name="stream">The stream containing the metadata</param>
        /// <param name="contents">The contents of the data</param>
        /// <param name="offset">The offset of this row</param>
        public TypeSpecMetadataTableRow(MetadataStream stream, byte[] contents, Offset offset)
        {
            this.FileOffset = offset;
            this.Signiture = new BlobIndex(stream.SizeOfBlobIndexes, contents, Reflection.Signitures.Signitures.TypeSpecification, offset);
        }

        /// <summary>
        /// An index in to the blob heap
        /// </summary>
        public BlobIndex Signiture { get; set; }
    }
}