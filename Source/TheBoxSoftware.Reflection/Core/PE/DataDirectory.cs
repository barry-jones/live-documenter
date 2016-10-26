
namespace TheBoxSoftware.Reflection.Core.PE
{
    using System;

    /// <include file='..\..\code-documentation\core.pe.xml' path='docs/datadirectory/member[@name="class"]/*' />
    public class DataDirectory
    {
        public const int SizeInBytes = 8;

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
        public uint VirtualAddress { get; set; }

        /// <summary>
        /// Size of directory in bytes
        /// </summary>
        public uint Size { get; set; }

        /// <summary>
        /// The directory being represented
        /// </summary>
        public DataDirectories Directory { get; set; }

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