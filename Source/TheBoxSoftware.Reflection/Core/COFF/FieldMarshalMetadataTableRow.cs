
namespace TheBoxSoftware.Reflection.Core.COFF
{
    /// <summary>
    /// Links an existing row in the Field or Param table to information
    /// in the blob heap that defines how that field or parameter should
    /// be marshalled.
    /// </summary>
    public class FieldMarshalMetadataTableRow : MetadataRow
    {
        private uint _nativeTypeIndex;
        private CodedIndex _parentIndex;

        /// <summary>
        /// Initialises a new instance of the FieldMarshalMEtadataTableRow class
        /// </summary>
        /// <param name="stream">The stream containing the metadata</param>
        /// <param name="contents">The contents of the file</param>
        /// <param name="offset">The offset of the current row</param>
        public FieldMarshalMetadataTableRow(byte[] contents, Offset offset, ICodedIndexResolver resolver, byte sizeOfBlobIndex)
        {
            this.FileOffset = offset;

            int sizeOfHasFieldMarshalIndex = resolver.GetSizeOfIndex(CodedIndexes.HasFieldMarshall);

            _parentIndex = resolver.Resolve(
                CodedIndexes.HasFieldMarshall,
                FieldReader.ToUInt32(contents, offset.Shift(sizeOfHasFieldMarshalIndex), sizeOfHasFieldMarshalIndex)
                );
            _nativeTypeIndex = FieldReader.ToUInt32(contents, offset.Shift(sizeOfBlobIndex), sizeOfBlobIndex);
        }

        /// <summary>
        /// A HasFieldMarshal encoded index to the Field or Param tables
        /// </summary>
        public CodedIndex Parent
        {
            get { return _parentIndex; }
            set { _parentIndex = value; }
        }

        /// <summary>
        /// An index in to the blob heap
        /// </summary>
        public uint NativeType
        {
            get { return _nativeTypeIndex; }
            set { _nativeTypeIndex = value; }
        }
    }
}