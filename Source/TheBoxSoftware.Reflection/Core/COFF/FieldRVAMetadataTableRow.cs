
namespace TheBoxSoftware.Reflection.Core.COFF
{
    using System;

    public class FieldRVAMetadataTableRow : MetadataRow
    {
        /// <summary>
        /// Initialises a new instance of the FieldRVAMetadataTableRow class
        /// </summary>
        /// <param name="content">The content of the file</param>
        /// <param name="offset">The offset for the current row</param>
        public FieldRVAMetadataTableRow(MetadataStream stream, byte[] content, Offset offset)
        {
            this.FileOffset = offset;
            this.RVA = FieldReader.ToUInt32(content, offset.Shift(4));
            this.Field = new Index(stream, content, offset, MetadataTables.Field);
        }

        /// <summary>
        /// The RVA of the field
        /// </summary>
        public UInt32 RVA { get; set; }

        /// <summary>
        /// An index into the Field table
        /// </summary>
        public Index Field { get; set; }
    }
}