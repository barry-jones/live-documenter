using System;

namespace TheBoxSoftware.Reflection.Core.COFF
{
    public class AssemblyOSMetadataTableRow : MetadataRow
    {
        /// <summary>
        /// Initialises a new instance of the AssemblyOSMetadataTableRow class
        /// </summary>
        /// <param name="stream">The stream containing the metadata</param>
        /// <param name="contents">The contents of the file</param>
        /// <param name="offset">The offset of the file</param>
        public AssemblyOSMetadataTableRow(MetadataStream stream, byte[] contents, Offset offset)
        {
            this.FileOffset = offset;
            this.OSPlatformID = FieldReader.ToUInt32(contents, offset.Shift(4));
            this.OSMajorVersion = FieldReader.ToUInt32(contents, offset.Shift(4));
            this.OSMinorVersion = FieldReader.ToUInt32(contents, offset.Shift(4));
        }

        /// <summary>
        /// 4-byte constant
        /// </summary>
        public UInt32 OSPlatformID { get; set; }

        /// <summary>
        /// 4-byte constant
        /// </summary>
        public UInt32 OSMajorVersion { get; set; }

        /// <summary>
        /// 4-byte constant
        /// </summary>
        public UInt32 OSMinorVersion { get; set; }
    }
}