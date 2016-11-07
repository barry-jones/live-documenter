
namespace TheBoxSoftware.Reflection.Core.COFF
{
    /// <summary>
    /// Records the interfaces a type implements explicitly
    /// </summary>
    /// <remarks>
    /// Updated for 4-byte heap indexes
    /// </remarks>
    public class InterfaceImplMetadataTableRow : MetadataRow
    {
        private Index _class;
        private CodedIndex _interface;

        /// <summary>
        /// Initialises a new instance of the InterfaceImplMetadataTableRow
        /// </summary>
        /// <param name="contents">The contents of the file</param>
        /// <param name="offset">The offset of this row</param>
        public InterfaceImplMetadataTableRow(byte[] contents, Offset offset, ICodedIndexResolver resolver, int sizeOfTypeDefIndex)
        {
            this.FileOffset = offset;

            int sizeOfCodedIndex = resolver.GetSizeOfIndex(CodedIndexes.TypeDefOrRef);

            _class = new Index(contents, offset, sizeOfTypeDefIndex);
            _interface = resolver.Resolve(CodedIndexes.TypeDefOrRef,
                FieldReader.ToUInt32(contents, offset.Shift(sizeOfCodedIndex), sizeOfCodedIndex)
                );
        }

        /// <summary>
        /// An index in to the TypeDef table
        /// </summary>
        public Index Class
        {
            get { return _class; }
            set { _class = value; }
        }

        /// <summary>
        /// An index in to the TypeDef, TypeRef, or TypeSpec table. More precisely
        /// a TypeDefOrRef coded index.
        /// </summary>
        public CodedIndex Interface
        {
            get { return _interface; }
            set { _interface = value; }
        }
    }
}