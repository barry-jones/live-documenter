
namespace TheBoxSoftware.Reflection.Core.COFF
{
    public class MemberRefMetadataTableRow : MetadataRow
    {
        private CodedIndex _class;
        private StringIndex _name;
        private BlobIndex _signiture;

        /// <summary>
        /// Initialises a new instance of the MemberRefMetadataTableRow
        /// </summary>
        /// <param name="contents">The contents of the file</param>
        /// <param name="offset">The offset of this row</param>
        public MemberRefMetadataTableRow(byte[] contents, Offset offset, ICodedIndexResolver resolver, IIndexDetails indexDetails)
        {
            this.FileOffset = offset;

            int sizeOfMemberRefParentIndex = resolver.GetSizeOfIndex(CodedIndexes.MemberRefParent);
            byte sizeOfStringIndex = indexDetails.GetSizeOfStringIndex();
            byte sizeOfBlobIndex = indexDetails.GetSizeOfBlobIndex();

            _class = resolver.Resolve(CodedIndexes.MemberRefParent,
                FieldReader.ToUInt32(contents, offset.Shift(sizeOfMemberRefParentIndex), sizeOfMemberRefParentIndex)
                );
            _name = new StringIndex(contents, sizeOfStringIndex, offset);
            _signiture = new BlobIndex(sizeOfBlobIndex, contents, Reflection.Signatures.Signatures.MethodDef, offset);
        }

        /// <summary>
        /// An index in to the MethodDef, ModuleRef, TypeRef, or TypeSpec tables,
        /// more precisely a MemberRefParent coded index.
        /// </summary>
        public CodedIndex Class
        {
            get { return _class; }
            set { _class = value; }
        }

        /// <summary>
        /// An index in to the string heap
        /// </summary>
        public StringIndex Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// An index in to the blob heap
        /// </summary>
        public BlobIndex Signiture
        {
            get { return _signiture; }
            set { _signiture = value; }
        }
    }
}