
namespace TheBoxSoftware.Reflection.Core.COFF
{
    using System;
    using TheBoxSoftware.Reflection.Core.PE;

    // REFERS: IMAGE_COR20_HEADER
    public class Cor20Header
    {
        private ulong _cb;
        private ushort _majorRuntimeVersion;
        private ushort _minorRuntimeVersion;
        private DataDirectory _metadata;
        private Cor20Flags _flags;
        private ulong _entryPointToken;
        private DataDirectory _resources;
        private DataDirectory _strongNameSigniture;
        private DataDirectory _codeManagerTable;
        private DataDirectory _vtableFixups;
        private DataDirectory _exportAddressTableJumps;
        private DataDirectory _managedNativeHeader;

        public Cor20Header(byte[] fileContents, uint address)
        {
            Offset offset = (int)address;

            _cb = BitConverter.ToUInt32(fileContents, offset.Shift(4));
            _majorRuntimeVersion = BitConverter.ToUInt16(fileContents, offset.Shift(2));
            _minorRuntimeVersion = BitConverter.ToUInt16(fileContents, offset.Shift(2));
            _metadata = new DataDirectory(GetRange(fileContents, offset.Shift(8), 8));
            _flags = (Cor20Flags)BitConverter.ToUInt32(fileContents, offset.Shift(4));
            _entryPointToken = BitConverter.ToUInt32(fileContents, offset.Shift(4));
            _resources = new DataDirectory(GetRange(fileContents, offset.Shift(8), 8));
            _strongNameSigniture = new DataDirectory(GetRange(fileContents, offset.Shift(8), 8));
            _codeManagerTable = new DataDirectory(GetRange(fileContents, offset.Shift(8), 8));
            _vtableFixups = new DataDirectory(GetRange(fileContents, offset.Shift(8), 8));
            _exportAddressTableJumps = new DataDirectory(GetRange(fileContents, offset.Shift(8), 8));
            _managedNativeHeader = new DataDirectory(GetRange(fileContents, offset.Shift(8), 8));
        }

        private byte[] GetRange(byte[] fileContents, Offset start, int length)
        {
            byte[] range = new byte[length];
            for(int i = 0; i < length; i++)
            {
                range[i] = fileContents[i + start];
            }
            return range;
        }
       
        /// <summary>
        /// Size of the header in bytes
        /// </summary>
        public ulong CB
        {
            get { return _cb; }
            set { _cb = value; }
        }

        /// <summary>
        /// Major portion of the minimum version of the runtime required
        /// </summary>
        public ushort MajorRuntimeVersion
        {
            get { return _majorRuntimeVersion; }
            set { _majorRuntimeVersion = value; }
        }

        /// <summary>
        /// Minor portion of the minimum version of the runtime required
        /// </summary>
        public ushort MinorRuntimeVersion
        {
            get { return _minorRuntimeVersion; }
            set { _minorRuntimeVersion = value; }
        }

        /// <summary>
        /// RVA and size of the meta data
        /// </summary>
        public DataDirectory MetaData
        {
            get { return _metadata; }
            set { _metadata = value; }
        }

        public Cor20Flags Flags
        {
            get { return _flags; }
            set { _flags = value; }
        }

        /// <summary>
        /// Metadata identifier of the entry poitn for the image file. Can be 0 for DLL
        /// images. This field identifies a method belonging to this module or a module
        /// containing the entry point method.
        /// </summary>
        public ulong EntryPointToken
        {
            get { return _entryPointToken; }
            set { _entryPointToken = value; }
        }

        /// <summary>
        /// RVA and size of managed resources
        /// </summary>
        public DataDirectory Resources
        {
            get { return _resources; }
            set { _resources = value; }
        }

        /// <summary>
        /// RVA and size of the hash data fo rthis PE file, used by the loader for binding
        /// and versioning.
        /// </summary>
        public DataDirectory StrongNameSigniture
        {
            get { return _strongNameSigniture; }
            set { _strongNameSigniture = value; }
        }

        /// <summary>
        /// Reserved must be zero (first release of the runtime)
        /// </summary>
        public DataDirectory CodeManagerTable
        {
            get { return _codeManagerTable; }
            set { _codeManagerTable = value; }
        }

        /// <summary>
        /// RVA and size in butes of an array of virtual table (v-table) fixups. Among current
        /// managed compilers, only the MC++ compiler and linker and the ILAsm compiler can
        /// produce this array.
        /// </summary>
        public DataDirectory VTableFixups
        {
            get { return _vtableFixups; }
            set { _vtableFixups = value; }
        }

        /// <summary>
        /// RVA and size of an array of addresses of jump thunks
        /// </summary>
        public DataDirectory ExportAddressTableJumps
        {
            get { return _exportAddressTableJumps; }
            set { _exportAddressTableJumps = value; }
        }

        /// <summary>
        /// Reserved set to zero
        /// </summary>
        public DataDirectory ManagedNativeHeader
        {
            get { return _managedNativeHeader; }
            set { _managedNativeHeader = value; }
        }
    }
}