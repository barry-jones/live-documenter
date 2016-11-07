
namespace TheBoxSoftware.Reflection.Core.COFF
{
    // these records should not be emitted to any PE file as per the specification,
    // if they are present then all values should be zero

    public class AssemblyRefOSMetadataTableRow : MetadataRow
    {
        private Index _assemblyRef;
        private uint _osMinorVersion;
        private uint _osMajorVersion;
        private uint _osPlatformId;

        /// <summary>
        /// Initialises a new instance of AssemblyRefOSMetadataTableRow
        /// </summary>
        /// <param name="contents">The contents of the file</param>
        /// <param name="offset">The offset of the current row</param>
        /// <param name="sizeOfAssemblyRefIndex">The size of the indexes to the assembly ref table</param>
        public AssemblyRefOSMetadataTableRow(byte[] contents, Offset offset, IIndexDetails indexDetails)
        {
            this.FileOffset = offset;

            byte sizeOfAssemblyRefIndex = indexDetails.GetSizeOfIndex(MetadataTables.AssemblyRef);

            // make sure we move the offset on enough if the table is present
            offset.Shift(4);
            offset.Shift(4);
            offset.Shift(4);
            offset.Shift(sizeOfAssemblyRefIndex);

            // set all values to zero as per spec
            _osPlatformId = 0;
            _osMajorVersion = 0;
            _osMinorVersion = 0;
            _assemblyRef = new Index();
        }

        public uint OSPlatformID
        {
            get { return _osPlatformId; }
            set { _osPlatformId = value; }
        }

        public uint OSMajorVersion
        {
            get { return _osMajorVersion; }
            set { _osMajorVersion = value; }
        }

        public uint OSMinorVersion
        {
            get { return _osMinorVersion; }
            set { _osMinorVersion = value; }
        }

        public Index AssemblyRef
        {
            get { return _assemblyRef; }
            set { _assemblyRef = value; }
        }
    }
}