
namespace TheBoxSoftware.Reflection.Core.COFF
{
    public class TypeSpecMetadataTableRow : MetadataRow
    {
        private BlobIndex _signiture;

        public TypeSpecMetadataTableRow(byte[] contents, Offset offset, IIndexDetails indexDetails)
        {
            this.FileOffset = offset;

            byte sizeOfBlobIndex = indexDetails.GetSizeOfBlobIndex();

            _signiture = new BlobIndex(sizeOfBlobIndex, contents, Signitures.Signatures.TypeSpecification, offset);
        }

        /// <summary>An index in to the blob heap</summary>
        public BlobIndex Signiture
        {
            get { return _signiture; }
            set { _signiture = value; }
        }
    }
}