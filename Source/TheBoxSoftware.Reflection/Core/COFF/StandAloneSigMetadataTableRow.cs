
namespace TheBoxSoftware.Reflection.Core.COFF
{
    public class StandAloneSigMetadataTableRow : MetadataRow
    {
        private BlobIndex _signiture;

        /// <summary>
        /// Initialises a new instnace of the StandAloneSigMetadataTableRow
        /// </summary>
        /// <param name="contents">The contents of teh file</param>
        /// <param name="offset">The offset for this row</param>
        public StandAloneSigMetadataTableRow(byte[] contents, Offset offset, byte sizeOfBlobIndexes)
        {
            this.FileOffset = offset;

            // TODO: Fix; stand alone is not forced to be a methoddef.. i think
            _signiture = new BlobIndex(
                sizeOfBlobIndexes,
                contents,
                TheBoxSoftware.Reflection.Signitures.Signitures.MethodDef,
                offset);
        }

        /// <summary>
        /// An index in to the blob heap, which points to a signiture which is
        /// note referenced by a normal member
        /// </summary>
        public BlobIndex Signiture
        {
            get { return _signiture; }
            set { _signiture = value; }
        }
    }
}