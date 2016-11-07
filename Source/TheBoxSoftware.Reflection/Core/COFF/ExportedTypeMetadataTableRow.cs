
namespace TheBoxSoftware.Reflection.Core.COFF
{
    /// <summary>
    /// A type which is defined within other modules of this assembly. In essance, it stores
    /// TypeDef row numbers of all types tha are marked public in other modules that
    /// this Assembly comprises.
    /// </summary>
    public class ExportedTypeMetadataTableRow : MetadataRow
    {
        private CodedIndex _implementationIndex;
        private StringIndex _typeNameIndex;
        private StringIndex _typeNamespaceIndex;
        private uint _typeDefId;
        private TypeAttributes _flags;

        /// <summary>
        /// Initialises a new instance of the ExportedTypeMetadataTableRow class
        /// </summary>
        /// <param name="contents">The contents of the file</param>
        /// <param name="offset">The offset of the current row</param>
        public ExportedTypeMetadataTableRow(byte[] contents, Offset offset, ICodedIndexResolver resolver, byte sizeOfStringIndex)
        {
            this.FileOffset = offset;

            int sizeOfImplementationIndex = resolver.GetSizeOfIndex(CodedIndexes.Implementation);

            _flags = (TypeAttributes)FieldReader.ToUInt32(contents, offset.Shift(4));
            _typeDefId = FieldReader.ToUInt32(contents, offset.Shift(4));
            _typeNameIndex = new StringIndex(contents, sizeOfStringIndex, offset);
            _typeNamespaceIndex = new StringIndex(contents, sizeOfStringIndex, offset);
            _implementationIndex = resolver.Resolve(
                CodedIndexes.Implementation,
                FieldReader.ToUInt32(contents, offset.Shift(sizeOfImplementationIndex), sizeOfImplementationIndex)
                );
        }

        /// <summary>
        /// 4-byte bitmask of TypeAttributes
        /// </summary>
        public TypeAttributes Flags
        {
            get { return _flags; }
            set { _flags = value; }
        }

        /// <summary>
        /// 4-byte index in to the TypeDef table of another module in this
        /// Assembly. Hint, then search.
        /// </summary>
        public uint TypeDefId
        {
            get { return _typeDefId; }
            set { _typeDefId = value; }
        }

        /// <summary>
        /// An index in to the string heap
        /// </summary>
        public StringIndex TypeName
        {
            get { return _typeNameIndex; }
            set { _typeNameIndex = value; }
        }

        /// <summary>
        /// An index in to the string heap
        /// </summary>
        public StringIndex TypeNamespace
        {
            get { return _typeNamespaceIndex; }
            set { _typeNamespaceIndex = value; }
        }

        /// <summary>
        /// An index in to the File, ExportedType or precisely Implementation
        /// coded index
        /// </summary>
        public CodedIndex Implementation
        {
            get { return _implementationIndex; }
            set { _implementationIndex = value; }
        }
    }
}