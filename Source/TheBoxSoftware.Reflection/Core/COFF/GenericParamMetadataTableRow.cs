
namespace TheBoxSoftware.Reflection.Core.COFF
{
    public class GenericParamMetadataTableRow : MetadataRow
    {
        private StringIndex _nameIndex;
        private CodedIndex _ownerIndex;
        private GenericParamAttributes _flags;
        private ushort _number;

        /// <summary>
        /// Initialises a new instance of the GenericParamMetadataTableRow class
        /// </summary>
        /// <param name="contents">The contents of the file</param>
        /// <param name="offset">The offset of the current row</param>
        public GenericParamMetadataTableRow(byte[] contents, Offset offset, ICodedIndexResolver resolver, IIndexDetails indexDetails)
        {
            this.FileOffset = offset;

            int sizeOfCodeIndex = resolver.GetSizeOfIndex(CodedIndexes.TypeOrMethodDef);
            byte sizeOfStringIndex = indexDetails.GetSizeOfStringIndex();

            _number = FieldReader.ToUInt16(contents, offset.Shift(2));
            _flags = (GenericParamAttributes)FieldReader.ToUInt16(contents, offset.Shift(2));
            _ownerIndex = resolver.Resolve(CodedIndexes.TypeOrMethodDef,
                FieldReader.ToUInt32(contents, offset.Shift(sizeOfCodeIndex), sizeOfCodeIndex)
                );
            _nameIndex = new StringIndex(contents, sizeOfStringIndex, offset);
        }

        /// <summary>
        /// A 2-byte index of the generic parameter, numbered left to right
        /// </summary>
        public ushort Number
        {
            get { return _number; }
            set { _number = value; }
        }

        /// <summary>
        /// A 2-byte bitmask of GenericParamAttributes
        /// </summary>
        public GenericParamAttributes Flags
        {
            get { return _flags; }
            set { _flags = value; }
        }

        /// <summary>
        /// An index in to the TypeDef, MethodDef; more precisley a TypeOrMethodDef
        /// encoded index
        /// </summary>
        public CodedIndex Owner
        {
            get { return _ownerIndex; }
            set { _ownerIndex = value; }
        }

        /// <summary>
        /// An index in to the string heap
        /// </summary>
        public StringIndex Name
        {
            get { return _nameIndex; }
            set { _nameIndex = value; }
        }
    }
}