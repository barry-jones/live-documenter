
namespace TheBoxSoftware.Reflection.Core.COFF
{
    public class GenericParamConstraintMetadataTableRow : MetadataRow
    {
        private CodedIndex _constraint;
        private Index _owner;

        /// <summary>
        /// Initialises a new instance of the GenericParamConstraintMetadataTableRow class
        /// </summary>
        /// <param name="stream">The stream containing the metadata</param>
        /// <param name="contents">The contents of the file</param>
        /// <param name="offset">The offset of the current row</param>
        public GenericParamConstraintMetadataTableRow(byte[] contents, Offset offset, ICodedIndexResolver resolver, int sizeOfGenericParamIndex)
        {
            FileOffset = offset;

            int sizeOfCodedIndex = resolver.GetSizeOfIndex(CodedIndexes.TypeDefOrRef);

            _owner = new Index(contents, offset, sizeOfGenericParamIndex);
            _constraint = resolver.Resolve(
                CodedIndexes.TypeDefOrRef,
                FieldReader.ToUInt32(contents, offset.Shift(sizeOfCodedIndex), sizeOfCodedIndex)
                );
        }

        /// <summary>
        /// An index into the GenericParam table
        /// </summary>
        public Index Owner
        {
            get { return _owner; }
            set { _owner = value; }
        }

        /// <summary>
        /// An index in to the TypeDef, TypeRef, TypeSpec table or more precisely
        /// a TypeDefOrRef coded index
        /// </summary>
        public CodedIndex Constraint
        {
            get { return _constraint; }
            set { _constraint = value; }
        }
    }
}