
namespace TheBoxSoftware.Reflection.Core.COFF
{
    public class TypeSpecMetadataTableRow : MetadataRow
    {
        private BlobIndex _signiture;

        public TypeSpecMetadataTableRow(byte sizeOfBlobIndexes, byte[] contents, Offset offset)
        {
            this.FileOffset = offset;
            this.Signiture = new BlobIndex(sizeOfBlobIndexes, contents, Signitures.Signitures.TypeSpecification, offset);
        }

        /// <summary>An index in to the blob heap</summary>
        public BlobIndex Signiture
        {
            get { return _signiture; }
            set { _signiture = value; }
        }
    }
}