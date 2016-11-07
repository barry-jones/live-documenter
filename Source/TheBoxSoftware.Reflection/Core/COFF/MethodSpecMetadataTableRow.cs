
namespace TheBoxSoftware.Reflection.Core.COFF
{
    public class MethodSpecMetadataTableRow : MetadataRow
    {
        private uint _instantiation;
        private CodedIndex _method;

        /// <summary>
        /// Initialises a new instance of the MethodSpecMetadataTableRow class
        /// </summary>
        /// <param name="contents">The contents of the file</param>
        /// <param name="offset">The offset of the current row</param>
        public MethodSpecMetadataTableRow(byte[] contents, Offset offset, ICodedIndexResolver resolver, IIndexDetails indexDetails)
        {
            this.FileOffset = offset;

            int sizeOfCodedIndex = resolver.GetSizeOfIndex(CodedIndexes.MethodDefOrRef);
            byte sizeOfBlobIndex = indexDetails.GetSizeOfBlobIndex();

            _method = resolver.Resolve(CodedIndexes.MethodDefOrRef,
                FieldReader.ToUInt32(contents, offset.Shift(sizeOfCodedIndex), sizeOfCodedIndex)
                );
            _instantiation = FieldReader.ToUInt32(contents, offset.Shift(sizeOfBlobIndex), sizeOfBlobIndex);
        }

        /// <summary>
        /// An index in to a MethodDef or MemberRef table, a MethodDefOrRef
        /// encoded index
        /// </summary>
        public CodedIndex Method
        {
            get { return _method; }
            set { _method = value; }
        }

        /// <summary>
        /// An index in to the blob heap
        /// </summary>
        public uint Instantiation
        {
            get { return _instantiation; }
            set { _instantiation = value; }
        }
    }
}