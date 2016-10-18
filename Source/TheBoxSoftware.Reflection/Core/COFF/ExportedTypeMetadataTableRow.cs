using System;

namespace TheBoxSoftware.Reflection.Core.COFF
{
    /// <summary>
    /// A type which is defined within other modules of this assembly. In essance, it stores
    /// TypeDef row numbers of all types tha are marked public in other modules that
    /// this Assembly comprises.
    /// </summary>
    public class ExportedTypeMetadataTableRow : MetadataRow
    {
        /// <summary>
        /// Initialises a new instance of the ExportedTypeMetadataTableRow class
        /// </summary>
        /// <param name="contents">The contents of the file</param>
        /// <param name="offset">The offset of the current row</param>
        public ExportedTypeMetadataTableRow(MetadataStream stream, byte[] contents, Offset offset)
        {
            this.FileOffset = offset;
            this.Flags = FieldReader.ToUInt32(contents, offset.Shift(4));
            this.TypeDefId = FieldReader.ToUInt32(contents, offset.Shift(4));
            this.TypeName = new StringIndex(stream, offset);
            this.TypeNamespace = new StringIndex(stream, offset);
            this.Implementation = new CodedIndex(stream, offset, CodedIndexes.Implementation);
        }

        /// <summary>
        /// 4-byte bitmask of TypeAttributes
        /// </summary>
        public UInt32 Flags { get; set; }

        /// <summary>
        /// 4-byte index in to the TypeDef table of another module in this
        /// Assembly. Hint, then search.
        /// </summary>
        public UInt32 TypeDefId { get; set; }

        /// <summary>
        /// An index in to the string heap
        /// </summary>
        public StringIndex TypeName { get; set; }

        /// <summary>
        /// An index in to the string heap
        /// </summary>
        public StringIndex TypeNamespace { get; set; }

        /// <summary>
        /// An index in to the File, ExportedType or precisely Implementation
        /// coded index
        /// </summary>
        public CodedIndex Implementation { get; set; }
    }
}