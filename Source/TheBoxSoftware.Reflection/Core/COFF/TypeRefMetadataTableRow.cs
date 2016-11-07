
namespace TheBoxSoftware.Reflection.Core.COFF
{
    /// <remarks>
    /// Updated for 4-byte heap indexes
    /// </remarks>
    [System.Diagnostics.DebuggerDisplay("Namespace[{Namespace}] Name[{Name}]")]
    public class TypeRefMetadataTableRow : MetadataRow
    {
        private StringIndex _namespaceIndex;
        private StringIndex _nameIndex;
        private CodedIndex _resolutionScope;

        /// <summary>
        /// Initialises a new instance of the TypeRefMetadataTableRow class
        /// </summary>
        /// <param name="contents">The file contents</param>
        /// <param name="offset">The offset for this entry</param>
        public TypeRefMetadataTableRow(byte[] contents, Offset offset, ICodedIndexResolver resolver, byte sizeOfStringIndex)
        {
            this.FileOffset = offset;

            int sizeOfCodedIndex = resolver.GetSizeOfIndex(CodedIndexes.ResolutionScope);

            _resolutionScope = resolver.Resolve(CodedIndexes.ResolutionScope,
                FieldReader.ToUInt32(contents, offset.Shift(sizeOfCodedIndex), sizeOfCodedIndex)
                );
            _nameIndex= new StringIndex(contents, sizeOfStringIndex, offset);
            _namespaceIndex = new StringIndex(contents, sizeOfStringIndex, offset);
        }

        /// <summary>
        /// An index in to a Module, ModuleRef, AssemblyRef, or TypeRef table, or null.
        /// More precisely a ResolutionScope
        /// </summary>
        public CodedIndex ResolutionScope
        {
            get { return _resolutionScope; }
            set { _resolutionScope = value; }
        }

        /// <summary>An index in to the string heap</summary>
        public StringIndex Name
        {
            get { return _nameIndex; }
            set { _nameIndex = value; }
        }

        /// <summary>An index in to the string heap</summary>
        public StringIndex Namespace
        {
            get { return _namespaceIndex; }
            set { _namespaceIndex = value; }
        }
    }
}