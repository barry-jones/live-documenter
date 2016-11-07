
namespace TheBoxSoftware.Reflection.Core.COFF
{
    /// <summary>
    /// Holds information about unmanaged methods that can be reached from managed
    /// code, using PInvoke dispatch.
    /// </summary>
    public class ImplMapMetadataTableRow : MetadataRow
    {
        private Index _importScope;
        private StringIndex _importName;
        private CodedIndex _memberForward;
        private PInvokeAttributes _mappingFlags;

        /// <summary>
        /// Initialises a new instance of the ImplMapMetadataTableRow class
        /// </summary>
        /// <param name="content">The content of the file</param>
        /// <param name="offset">The offset of the current row</param>
        public ImplMapMetadataTableRow(byte[] content, Offset offset, ICodedIndexResolver resolver, IIndexDetails indexDetails)
        {
            this.FileOffset = offset;

            int sizeOfMemberForwardedIndex = resolver.GetSizeOfIndex(CodedIndexes.MemberForwarded);
            byte sizeOfStringIndex = indexDetails.GetSizeOfStringIndex();
            byte sizeOfModuleRefIndex = indexDetails.GetSizeOfIndex(MetadataTables.ModuleRef);

            _mappingFlags = (PInvokeAttributes)FieldReader.ToUInt16(content, offset.Shift(2));
            _memberForward = resolver.Resolve(CodedIndexes.MemberForwarded,
                FieldReader.ToUInt32(content, offset.Shift(sizeOfMemberForwardedIndex), sizeOfMemberForwardedIndex)
                );
            _importName = new StringIndex(content, sizeOfStringIndex, offset);
            _importScope = new Index(content, offset, sizeOfModuleRefIndex);
        }

        /// <summary>
        /// A 2-byte mask of PInvokeAttributes
        /// </summary>
        public PInvokeAttributes MappingFlags
        {
            get { return _mappingFlags; }
            set { _mappingFlags = value; }
        }

        /// <summary>
        /// An index in to the Field or MethodDef table, a MemberForwarded
        /// coded index. However it only ever references the MethodDef because
        /// Field is never exported
        /// </summary>
        public CodedIndex MemberForward
        {
            get { return _memberForward; }
            set { _memberForward = value; }
        }

        /// <summary>
        /// An index in to the string heap
        /// </summary>
        public StringIndex ImportName
        {
            get { return _importName; }
            set { _importName = value; }
        }

        /// <summary>
        /// An index into the ModuleRef table
        /// </summary>
        public Index ImportScope
        {
            get { return _importScope; }
            set { _importScope = value; }
        }
    }
}