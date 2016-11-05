
namespace TheBoxSoftware.Reflection.Core.COFF
{
    using System;

    public class EventMetadataTableRow : MetadataRow
    {
        /// <summary>
        /// Initialises a new instance of the EventMetadataTableRow class
        /// </summary>
        /// <param name="contents">The contents of the file</param>
        /// <param name="offset">The offset of the current row</param>
        public EventMetadataTableRow(MetadataStream stream, byte[] contents, Offset offset)
        {
            this.FileOffset = offset;
            this.EventFlags = BitConverter.ToUInt16(contents, offset.Shift(2));
            this.Name = new StringIndex(stream, offset);
            this.EventType = new CodedIndex(stream, offset, CodedIndexes.TypeDefOrRef);
        }

        /// <summary>
        /// EventAttributes mask
        /// </summary>
        public UInt16 EventFlags { get; set; }

        /// <summary>
        /// An index in the string heap
        /// </summary>
        public StringIndex Name { get; set; }

        /// <summary>
        /// An index into a TypeDef, TypeRef, or TypeSpec table, more precisely
        /// a TypeDefOrRef encoded index
        /// </summary>
        public CodedIndex EventType { get; set; }
    }
}