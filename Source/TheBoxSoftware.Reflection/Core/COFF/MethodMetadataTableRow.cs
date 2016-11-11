
namespace TheBoxSoftware.Reflection.Core.COFF
{
    /// <summary>
    /// Describes a method definition loaded from the metadata tables in the pe/coff
    /// file.
    /// </summary>
    public class MethodMetadataTableRow : MetadataRow
    {
        private Index _paramList;
        private BlobIndex _signiture;
        private StringIndex _name;
        private MethodAttributes _flags;
        private MethodImplFlags _implFlags;
        private uint _rva;

        /// <summary>
        /// Initialises a new instance of the MethodMetadataTableRow class
        /// </summary>
        /// <param name="contents">The contents of the file</param>
        /// <param name="offset">The offset of the current row</param>
        public MethodMetadataTableRow(byte[] contents, Offset offset, IIndexDetails indexDetails)
        {
            this.FileOffset = offset;

            byte sizeOfStringIndex = indexDetails.GetSizeOfStringIndex();
            byte sizeOfBlobIndex = indexDetails.GetSizeOfBlobIndex();
            byte sizeOfParamIndex = indexDetails.GetSizeOfIndex(MetadataTables.Param);

            _rva = FieldReader.ToUInt32(contents, offset.Shift(4));
            _implFlags = (MethodImplFlags)FieldReader.ToUInt16(contents, offset.Shift(2));
            _flags = (MethodAttributes)FieldReader.ToUInt16(contents, offset.Shift(2));
            _name = new StringIndex(contents, sizeOfStringIndex, offset);
            _signiture = new BlobIndex(sizeOfBlobIndex, contents, Signatures.Signatures.MethodDef, offset);
            _paramList = new Index(contents, offset, sizeOfParamIndex);
        }

        /// <summary>
        /// Address of the CIL method data
        /// </summary>
        public uint RVA
        {
            get { return _rva; }
            set { _rva = value; }
        }

        /// <summary>
        /// 2-byte bitmask of MethodImplAttributes
        /// </summary>
        public MethodImplFlags ImplFlags
        {
            get { return _implFlags; }
            set { _implFlags = value; }
        }

        /// <summary>
        /// A 2-byte bitmask of MethodAttributes
        /// </summary>
        public MethodAttributes Flags
        {
            get { return _flags; }
            set { _flags = value; }
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

        /// <summary>
        /// An index in to the param table
        /// </summary>
        public Index ParamList
        {
            get { return _paramList; }
            set { _paramList = value; }
        }
    }
}