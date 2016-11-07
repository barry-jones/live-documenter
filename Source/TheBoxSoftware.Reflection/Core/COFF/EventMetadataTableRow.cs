
namespace TheBoxSoftware.Reflection.Core.COFF
{
    using System;

    public class EventMetadataTableRow : MetadataRow
    {
        private CodedIndex _eventTypeIndex;
        private StringIndex _nameIndex;
        private EventAttributes _eventFlags;

        /// <summary>
        /// Initialises a new instance of the EventMetadataTableRow class
        /// </summary>
        /// <param name="contents">The contents of the file</param>
        /// <param name="offset">The offset of the current row</param>
        public EventMetadataTableRow(byte[] contents, Offset offset, ICodedIndexResolver resolver, IIndexDetails indexDetails)
        {
            this.FileOffset = offset;

            int typeDefOrRefIndexSize = resolver.GetSizeOfIndex(CodedIndexes.TypeDefOrRef);
            byte sizeOfStringIndex = indexDetails.GetSizeOfStringIndex();

            _eventFlags = (EventAttributes)BitConverter.ToUInt16(contents, offset.Shift(2));
            _nameIndex = new StringIndex(contents, sizeOfStringIndex, offset);
            _eventTypeIndex = resolver.Resolve(
                CodedIndexes.TypeDefOrRef, 
                FieldReader.ToUInt32(contents, offset.Shift(typeDefOrRefIndexSize), typeDefOrRefIndexSize)
                );
        }

        /// <summary>
        /// EventAttributes mask
        /// </summary>
        public EventAttributes EventFlags
        {
            get { return _eventFlags; }
            set { _eventFlags = value; }
        }

        /// <summary>
        /// An index in the string heap
        /// </summary>
        public StringIndex Name
        {
            get { return _nameIndex; }
            set { _nameIndex = value; }
        }

        /// <summary>
        /// An index into a TypeDef, TypeRef, or TypeSpec table, more precisely
        /// a TypeDefOrRef encoded index
        /// </summary>
        public CodedIndex EventType
        {
            get { return _eventTypeIndex; }
            set { _eventTypeIndex = value; }
        }
    }
}