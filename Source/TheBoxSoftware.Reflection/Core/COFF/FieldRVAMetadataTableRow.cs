
namespace TheBoxSoftware.Reflection.Core.COFF
{
    public class FieldRVAMetadataTableRow : MetadataRow
    {
        private Index _fieldIndex;
        private uint _rva;

        /// <summary>
        /// Initialises a new instance of the FieldRVAMetadataTableRow class
        /// </summary>
        /// <param name="content">The content of the file</param>
        /// <param name="offset">The offset for the current row</param>
        public FieldRVAMetadataTableRow(byte[] content, Offset offset, int sizeOfFieldIndex)
        {
            this.FileOffset = offset;

            _rva = FieldReader.ToUInt32(content, offset.Shift(4));
            _fieldIndex = new Index(content, offset, sizeOfFieldIndex);
        }

        /// <summary>
        /// The RVA of the field
        /// </summary>
        public uint RVA
        {
            get { return _rva; }
            set { _rva = value; }
        }

        /// <summary>
        /// An index into the Field table
        /// </summary>
        public Index Field
        {
            get { return _fieldIndex; }
            set { _fieldIndex = value; }
        }
    }
}