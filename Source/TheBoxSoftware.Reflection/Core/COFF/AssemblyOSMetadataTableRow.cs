
namespace TheBoxSoftware.Reflection.Core.COFF
{
    // this record is defined in the ECMA 335, but it states it should never
    // be emitted in to any PE file. If it is all records should be treated as
    // zero. This is here to make sure we move the offset on while reading the
    // file in the event it is found.

    public class AssemblyOSMetadataTableRow : MetadataRow
    {
        private uint _osPlatformId;
        private uint _osMajorVersion;
        private uint _osMinorVersion;

        /// <summary>
        /// Initialises a new instance of the AssemblyOSMetadataTableRow class
        /// </summary>
        /// <param name="contents">The contents of the file</param>
        /// <param name="offset">The offset of the file</param>
        public AssemblyOSMetadataTableRow(byte[] contents, Offset offset)
        {
            this.FileOffset = offset;

            // move the offset on for 3 fields each being 4 bytes
            offset.Shift(4);
            offset.Shift(4);
            offset.Shift(4);

            // set all values to 0 as per ECMA 335 p.212
            _osPlatformId = 0;
            _osMajorVersion = 0;
            _osMinorVersion = 0;
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