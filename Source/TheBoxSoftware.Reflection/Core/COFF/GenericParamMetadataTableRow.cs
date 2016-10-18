using System;

namespace TheBoxSoftware.Reflection.Core.COFF
{
    public class GenericParamMetadataTableRow : MetadataRow
    {
        /// <summary>
        /// Initialises a new instance of the GenericParamMetadataTableRow class
        /// </summary>
        /// <param name="stream">The stream containing the metadata</param>
        /// <param name="contents">The contents of the file</param>
        /// <param name="offset">The offset of the current row</param>
        public GenericParamMetadataTableRow(MetadataStream stream, byte[] contents, Offset offset)
        {
            this.FileOffset = offset;
            this.Number = FieldReader.ToUInt16(contents, offset.Shift(2));
            this.Flags = FieldReader.ToUInt16(contents, offset.Shift(2));
            this.Owner = new CodedIndex(stream, offset, CodedIndexes.TypeOrMethodDef);
            this.Name = new StringIndex(stream, offset);
        }

        /// <summary>
        /// A 2-byte index of the generic parameter, numbered left to right
        /// </summary>
        public UInt16 Number { get; set; }

        /// <summary>
        /// A 2-byte bitmask of GenericParamAttributes
        /// </summary>
        public UInt16 Flags { get; set; }

        /// <summary>
        /// An index in to the TypeDef, MethodDef; more precisley a TypeOrMethodDef
        /// encoded index
        /// </summary>
        public CodedIndex Owner { get; set; }

        /// <summary>
        /// An index in to the string heap
        /// </summary>
        public StringIndex Name { get; set; }
    }
}