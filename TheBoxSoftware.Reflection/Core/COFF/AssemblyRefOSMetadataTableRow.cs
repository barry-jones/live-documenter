using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection.Core.COFF {
	/// <summary>
	/// 
	/// </summary>
	public class AssemblyRefOSMetadataTableRow : MetadataRow {
		/// <summary>
		/// Initialises a new instance of AssemblyRefOSMetadataTableRow
		/// </summary>
		/// <param name="stream">The stream containing the metadata</param>
		/// <param name="contents">The contents of the file</param>
		/// <param name="offset">The offset of the current row</param>
		public AssemblyRefOSMetadataTableRow(MetadataStream stream, byte[] contents, Offset offset) {
			this.FileOffset = offset;
			this.OSPlatformID = FieldReader.ToUInt32(contents, offset.Shift(4));
			this.OSMajorVersion = FieldReader.ToUInt32(contents, offset.Shift(4));
			this.OSMinorVersion = FieldReader.ToUInt32(contents, offset.Shift(4));
			this.AssemblyRef = new Index(stream, contents, offset, MetadataTables.AssemblyRef);
		}

		#region Properties
		/// <summary>
		/// 4-byte constant
		/// </summary>
		public UInt32 OSPlatformID {
			get;
			set;
		}

		/// <summary>
		/// 4-byte constant
		/// </summary>
		public UInt32 OSMajorVersion {
			get;
			set;
		}

		/// <summary>
		/// 4-byte constant
		/// </summary>
		public UInt32 OSMinorVersion {
			get;
			set;
		}

		/// <summary>
		/// An index in to the AssemblyRef table
		/// </summary>
		public Index AssemblyRef {
			get;
			set;
		}
		#endregion
	}
}
