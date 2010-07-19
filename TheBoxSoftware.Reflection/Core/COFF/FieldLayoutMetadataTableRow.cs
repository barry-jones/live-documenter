using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection.Core.COFF {
	/// <summary>
	/// 
	/// </summary>
	public class FieldLayoutMetadataTableRow : MetadataRow {
		/// <summary>
		/// Initialises a new instance of the FieldLayoutMetadataTableRow
		/// </summary>
		/// <param name="stream">The stream containing the metadata</param>
		/// <param name="contents">The contents of the file</param>
		/// <param name="offset">The offset of the current row</param>
		public FieldLayoutMetadataTableRow(MetadataStream stream, byte[] contents, Offset offset) {
			this.FileOffset = offset;
			this.Offset = BitConverter.ToUInt32(contents, offset.Shift(4));
			this.Field = new Index(stream, contents, offset, MetadataTables.Field);
		}

		#region Properties
		/// <summary>
		/// A 4-byte constant
		/// </summary>
		public UInt32 Offset {
			get;
			set;
		}

		/// <summary>
		/// An index to the field table
		/// </summary>
		public Index Field {
			get;
			set;
		}
		#endregion
	}
}
