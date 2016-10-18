using System;

namespace TheBoxSoftware.Reflection.Core.COFF
{
    public class AssemblyProcessorMetadataTableRow : MetadataRow
    {
        /// <summary>
        /// Initialises a new instance of the AssemblyProcessorMetadataTableRow class
        /// </summary>
        /// <param cref="stream">The stream containing the metadata</param>
        /// <param name="contents">The contents of the file</param>
        /// <param name="offset">The offset to the current row</param>
        public AssemblyProcessorMetadataTableRow(MetadataStream stream, byte[] contents, Offset offset)
        {
            this.FileOffset = offset;
            this.Processor = FieldReader.ToUInt32(contents, offset.Shift(4));
        }

        /// <summary>
        /// 4-byte constant
        /// </summary>
        public UInt32 Processor { get; set; }
    }
}