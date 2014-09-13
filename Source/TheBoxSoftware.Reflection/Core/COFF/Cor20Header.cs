using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection.Core.COFF {
	using TheBoxSoftware.Reflection.Core.PE;

	// REFERS: IMAGE_COR20_HEADER
	/// <summary>
	///
	/// </summary>
	public class Cor20Header {

		/// <summary>
		///
		/// </summary>
		/// <param name="data"></param>
		public Cor20Header(byte[] fileContents, int address) {
			Offset offset = address;
			List<byte> tempData = new List<byte>(fileContents);

			this.CB = BitConverter.ToUInt32(fileContents, offset.Shift(4));
			this.MajorRuntimeVersion = BitConverter.ToUInt16(fileContents, offset.Shift(2));
			this.MinorRuntimeVersion = BitConverter.ToUInt16(fileContents, offset.Shift(2));
			this.MetaData = new DataDirectory(tempData.GetRange(offset.Shift(8), 8).ToArray());
			this.Flags = (Cor20Flags)BitConverter.ToUInt32(fileContents, offset.Shift(4));
			this.EntryPointToken = BitConverter.ToUInt32(fileContents, offset.Shift(4));
			this.Resources = new DataDirectory(tempData.GetRange(offset.Shift(8), 8).ToArray());
			this.StrongNameSigniture = new DataDirectory(tempData.GetRange(offset.Shift(8), 8).ToArray());
			this.CodeManagerTable = new DataDirectory(tempData.GetRange(offset.Shift(8), 8).ToArray());
			this.VTableFixups = new DataDirectory(tempData.GetRange(offset.Shift(8), 8).ToArray());
			this.ExportAddressTableJumps = new DataDirectory(tempData.GetRange(offset.Shift(8), 8).ToArray());
			this.ManagedNativeHeader = new DataDirectory(tempData.GetRange(offset.Shift(8), 8).ToArray());
		}

		#region Properties
		/// <summary>
		/// Size of the header in bytes
		/// </summary>
		public UInt64 CB {
			get;
			set;
		}

		/// <summary>
		/// Major portion of the minimum version of the runtime required
		/// </summary>
		public UInt16 MajorRuntimeVersion {
			get;
			set;
		}

		/// <summary>
		/// Minor portion of the minimum version of the runtime required
		/// </summary>
		public UInt16 MinorRuntimeVersion {
			get;
			set;
		}

		/// <summary>
		/// RVA and size of the meta data
		/// </summary>
		public DataDirectory MetaData {
			get;
			set;
		}

		/// <summary>
		/// </summary>
		public Cor20Flags Flags {
			get;
			set;
		}

		/// <summary>
		/// Metadata identifier of the entry poitn for the image file. Can be 0 for DLL
		/// images. This field identifies a method belonging to this module or a module
		/// containing the entry point method.
		/// </summary>
		public UInt64 EntryPointToken {
			get;
			set;
		}

		/// <summary>
		/// RVA and size of managed resources
		/// </summary>
		public DataDirectory Resources {
			get;
			set;
		}

		/// <summary>
		/// RVA and size of the hash data fo rthis PE file, used by the loader for binding
		/// and versioning.
		/// </summary>
		public DataDirectory StrongNameSigniture {
			get;
			set;
		}

		/// <summary>
		/// Reserved must be zero (first release of the runtime)
		/// </summary>
		public DataDirectory CodeManagerTable {
			get;
			set;
		}

		/// <summary>
		/// RVA and size in butes of an array of virtual table (v-table) fixups. Among current
		/// managed compilers, only the MC++ compiler and linker and the ILAsm compiler can
		/// produce this array.
		/// </summary>
		public DataDirectory VTableFixups {
			get;
			set;
		}

		/// <summary>
		/// RVA and size of an array of addresses of jump thunks
		/// </summary>
		public DataDirectory ExportAddressTableJumps {
			get;
			set;
		}

		/// <summary>
		/// Reserved set to zero
		/// </summary>
		public DataDirectory ManagedNativeHeader {
			get;
			set;
		}
		#endregion
	}
}