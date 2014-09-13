using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection.Core.PE {
    /// <summary>
    /// A data directory entry is a row in the data directory table
    /// in a PE file that refers to one of 16 predefined tables.
    /// </summary>
    /// <remarks>
    /// The table of directories start at offset 96 in a 32bit PE and
    /// 112 in a 64bit PE.
    /// </remarks>
    public class DataDirectory {
        public const int SizeInBytes = 8;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="data">The byte data to populate the instance with</param>
        /// <exception cref="ArgumentException">Not enough byte data supplied</exception>
		public DataDirectory(byte[] data, DataDirectories directory) {
			if (data.Length < 8)
				throw new ArgumentException("Not enough byte data supplied to populate DataDirectory");
			this.Directory = directory;
			Offset offset = 0;

			this.VirtualAddress = BitConverter.ToUInt32(data, offset.Shift(4));
			this.Size = BitConverter.ToUInt32(data, offset.Shift(4));
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="data">The byte data to populate the instance with</param>
		/// <exception cref="ArgumentException">Not enough byte data supplied</exception>
		public DataDirectory(byte[] data) {
			if (data.Length < 8)
				throw new ArgumentException("Not enough byte data supplied to populate DataDirectory");
			Offset offset = 0;

			this.VirtualAddress = BitConverter.ToUInt32(data, offset.Shift(4));
			this.Size = BitConverter.ToUInt32(data, offset.Shift(4));
		}

		#region Properties
		/// <summary>
		/// Relative virtual address of the directory
		/// </summary>
		public uint VirtualAddress {
			get;
			set;
		}

		/// <summary>
		/// Size of directory in bytes
		/// </summary>
		public uint Size {
			get;
			set;
		}

		/// <summary>
		/// The directory being represented
		/// </summary>
		public DataDirectories Directory {
			get;
			set;
		}

		/// <summary>
		/// Boolean value indicating if this directory is in use
		/// </summary>
		public bool IsUsed {
			get {
				return this.VirtualAddress != 0x0 && this.Size != 0x0;
			}
		}
		#endregion
	}
}
