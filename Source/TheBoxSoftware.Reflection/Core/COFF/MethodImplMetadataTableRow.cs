
namespace TheBoxSoftware.Reflection.Core.COFF
{
    /// <summary>
    /// Allows the compiler to override the default inheritance rules provided
    /// by the CLI.
    /// </summary>
    public class MethodImplMetadataTableRow : MetadataRow
    {
        private CodedIndex _methodDeclaration;
        private CodedIndex _methodBody;
        private Index _class;

        /// <summary>
        /// Initialises a new instance of the MethodImplMetadataTableRow class
        /// </summary>
        /// <param name="contents">The contents of the file</param>
        /// <param name="offset">The offset of the current row</param>
        public MethodImplMetadataTableRow(byte[] contents, Offset offset, ICodedIndexResolver resolver, int sizeOfTypeDefIndex)
        {
            this.FileOffset = offset;

            int sizeOfMethodDefOrRefIndex = resolver.GetSizeOfIndex(CodedIndexes.MethodDefOrRef);

            _class = new Index(contents, offset, sizeOfTypeDefIndex);
            _methodBody = resolver.Resolve(CodedIndexes.MethodDefOrRef,
                FieldReader.ToUInt32(contents, offset.Shift(sizeOfMethodDefOrRefIndex), sizeOfMethodDefOrRefIndex)
                );
            _methodDeclaration = resolver.Resolve(CodedIndexes.MethodDefOrRef,
                FieldReader.ToUInt32(contents, offset.Shift(sizeOfMethodDefOrRefIndex), sizeOfMethodDefOrRefIndex)
                );
        }

        /// <summary>
        /// An index into the TypeDef table
        /// </summary>
        public Index Class
        {
            get { return _class; }
            set { _class = value; }
        }

        /// <summary>
        /// An index in to a MethodDef or MemberRef, a MethodDefOrRef
        /// encoded index
        /// </summary>
        public CodedIndex MethodBody
        {
            get { return _methodBody; }
            set { _methodBody = value; }
        }

        /// <summary>
        /// An index in to a MethodDef or MemberRef table, a MethodDefOrRef
        /// encoded index
        /// </summary>
        public CodedIndex MethodDeclaration
        {
            get { return _methodDeclaration; }
            set { _methodDeclaration = value; }
        }
    }
}