using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection.Core.COFF {
	/// <summary>
	/// pdated for 4-byte heap indexes
	/// </summary>
	/// <remarks>
	/// Completed 
	/// </remarks>
	public class ParamMetadataTableRow : MetadataRow {
		/// <summary>
		/// Initialises a new instance of the ParamMetadataTableRow class
		/// </summary>
		/// <param name="stream">The stream containing the metadata</param>
		/// <param name="contents">The contents of the file</param>
		/// <param name="offset">The offset of the current row</param>
		public ParamMetadataTableRow(MetadataStream stream, byte[] contents, Offset offset) {
			this.FileOffset = offset;
			this.Flags = FieldReader.ToUInt16(contents, offset.Shift(2));
			this.Sequence = FieldReader.ToUInt16(contents, offset.Shift(2));
			this.Name = new StringIndex(stream, offset);
		}

		#region Properties
		/// <summary>
		/// A 2-byte bitmask of ParamAttributes
		/// </summary>
		public UInt16 Flags {
			get;
			set;
		}

		/// <summary>
		/// The sequence of the parameter
		/// </summary>
		public UInt16 Sequence {
			get;
			set;
		}

		/// <summary>
		/// Index in to the string heap
		/// </summary>
		public StringIndex Name {
			get;
			set;
		}
		#endregion
	}
}
