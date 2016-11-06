
namespace TheBoxSoftware.Reflection.Core.COFF
{
    using System;

    public class AssemblyOSMetadataTableRow : MetadataRow
    {
        private uint _osPlatformId;
        private uint _osMajorVersion;
        private uint _osMinorVersion;

        /// <summary>
        /// Initialises a new instance of the AssemblyOSMetadataTableRow class
        /// </summary>
        /// <param name="stream">The stream containing the metadata</param>
        /// <param name="contents">The contents of the file</param>
        /// <param name="offset">The offset of the file</param>
        public AssemblyOSMetadataTableRow(MetadataStream stream, byte[] contents, Offset offset)
        {
            this.FileOffset = offset;

            _osPlatformId = FieldReader.ToUInt32(contents, offset.Shift(4));
            _osMajorVersion = FieldReader.ToUInt32(contents, offset.Shift(4));
            _osMinorVersion = FieldReader.ToUInt32(contents, offset.Shift(4));
        }

        /// <summary>
        /// 4-byte constant
        /// </summary>
        public uint OSPlatformID
        {
            get { return _osPlatformId; }
            set { _osPlatformId = value; }
        }

        /// <summary>
        /// 4-byte constant
        /// </summary>
        public uint OSMajorVersion
        {
            get { return _osMajorVersion; }
            set { _osMajorVersion = value; }
        }

        /// <summary>
        /// 4-byte constant
        /// </summary>
        public uint OSMinorVersion
        {
            get { return _osMinorVersion; }
            set { _osMinorVersion = value; }
        }
    }
}