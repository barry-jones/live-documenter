
namespace TheBoxSoftware.Reflection.Core.COFF
{
    public class CustomAttributeMetadataTableRow : MetadataRow
    {
        private CodedIndex _parentIndex;
        private CodedIndex _typeIndex;
        private uint _value;

        /// <summary>
        /// Initialises a new instance of the CustomAttributeMetadataTableRow
        /// </summary>
        /// <param name="contents">The contents of the file</param>
        /// <param name="offset">The offset of the current row</param>
        public CustomAttributeMetadataTableRow(byte[] contents, Offset offset, ICodedIndexResolver resolver, IIndexDetails indexDetails)
        {
            int hasCustomAttributeIndexSize = resolver.GetSizeOfIndex(CodedIndexes.HasCustomAttribute);
            int customAttributeIndexSize = resolver.GetSizeOfIndex(CodedIndexes.CustomAttributeType);
            byte sizeOfBlobIndex = indexDetails.GetSizeOfBlobIndex();

            this.FileOffset = offset;

            _parentIndex = resolver.Resolve(
                CodedIndexes.HasCustomAttribute,
                FieldReader.ToUInt32(contents, offset.Shift(hasCustomAttributeIndexSize), hasCustomAttributeIndexSize)
                );
            _typeIndex = resolver.Resolve(CodedIndexes.CustomAttributeType,
                FieldReader.ToUInt32(contents, offset.Shift(customAttributeIndexSize), customAttributeIndexSize)
                );
            _value = FieldReader.ToUInt32(contents, offset.Shift(sizeOfBlobIndex), sizeOfBlobIndex);
        }

        /// <summary>
        /// A HasCustomAttribute encoded index
        /// </summary>
        public CodedIndex Parent
        {
            get { return _parentIndex; }
            set { _parentIndex = value; }
        }

        /// <summary>
        /// A CustomAttributeType encoded index (Def or Ref tables)
        /// </summary>
        public CodedIndex Type
        {
            get { return _typeIndex; }
            set { _typeIndex = value; }
        }

        /// <summary>
        /// An index in to the blob heap
        /// </summary>
        public uint Value
        {
            get { return _value; }
            set { _value = value; }
        }
    }
}