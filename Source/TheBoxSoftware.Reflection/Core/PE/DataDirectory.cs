
namespace TheBoxSoftware.Reflection.Core.PE
{
    using System;

    /// <include file='..\..\code-documentation\core.pe.xml' path='docs/datadirectory/member[@name="class"]/*' />
    public class DataDirectory
    {
        public const int SizeInBytes = 8;

        private DataDirectories _directory;
        private uint _size;
        private uint _virtualAddress;

        /// <include file='..\..\code-documentation\core.pe.xml' path='docs/datadirectory/member[@name="ctor"]/*' />
        public DataDirectory(byte[] data, DataDirectories directory)
        {
            if(data.Length < SizeInBytes)
                throw new ArgumentException("Not enough byte data supplied to populate DataDirectory");
            
            Offset offset = 0;

            this.Directory = directory;
            this.VirtualAddress = BitConverter.ToUInt32(data, offset.Shift(4));
            this.Size = BitConverter.ToUInt32(data, offset.Shift(4));
        }

        /// <include file='..\..\code-documentation\core.pe.xml' path='docs/datadirectory/member[@name="ctor2"]/*' />
        public DataDirectory(byte[] data)
        {
            if(data.Length < SizeInBytes)
                throw new ArgumentException("Not enough byte data supplied to populate DataDirectory");

            Offset offset = 0;

            this.VirtualAddress = BitConverter.ToUInt32(data, offset.Shift(4));
            this.Size = BitConverter.ToUInt32(data, offset.Shift(4));
        }

        /// <summary>
        /// Relative virtual address of the directory
        /// </summary>
        public uint VirtualAddress
        {
            get { return _virtualAddress; }
            set { _virtualAddress = value; }
        }

        /// <summary>
        /// Size of directory in bytes
        /// </summary>
        public uint Size
        {
            get { return _size; }
            set { _size = value; }
        }

        /// <summary>
        /// The directory being represented
        /// </summary>
        public DataDirectories Directory
        {
            get { return _directory; }
            set { _directory = value; }
        }

        /// <summary>
        /// Boolean value indicating if this directory is in use
        /// </summary>
        public bool IsUsed
        {
            get
            {
                return this.VirtualAddress != 0x0 && this.Size != 0x0;
            }
        }
    }
}