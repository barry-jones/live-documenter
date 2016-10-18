using System;

namespace TheBoxSoftware.Reflection.Core.COFF
{
    public class ManifestResourceMetadataTableRow : MetadataRow
    {
        /// <summary>
        /// Initialises a new instance of the ManifestResourceMetadataTableRow class
        /// </summary>
        /// <param name="stream">The stream containing the metadata</param>
        /// <param name="contents">The contents of the file</param>
        /// <param name="offset">The offset of this current row</param>
        public ManifestResourceMetadataTableRow(MetadataStream stream, byte[] contents, Offset offset)
        {
            this.FileOffset = offset;
            this.Offset = BitConverter.ToUInt32(contents, offset.Shift(4));
            this.Flags = BitConverter.ToUInt32(contents, offset.Shift(4));
            this.Name = new StringIndex(stream, offset);
            this.Implementation = new CodedIndex(stream, offset, CodedIndexes.Implementation);
        }

        /// <summary>
        /// A 4-byte constant
        /// </summary>
        public UInt32 Offset { get; set; }

        /// <summary>
        /// A 4-byte bitmask of ManifestResourceAttributes
        /// </summary>
        public UInt32 Flags { get; set; }

        /// <summary>
        /// An index in to the string heap
        /// </summary>
        public StringIndex Name { get; set; }

        /// <summary>
        /// An index in to a File, AssemblyRef, or null; more precisely an
        /// Implementation coded index
        /// </summary>
        public CodedIndex Implementation { get; set; }
    }
}