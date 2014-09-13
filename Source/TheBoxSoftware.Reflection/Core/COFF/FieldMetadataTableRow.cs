using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection.Core.COFF {
	/// <summary>
	/// 
	/// </summary>
	/// <remarks>Modified to use 4-byte heap fields</remarks>
	public class FieldMetadataTableRow : MetadataRow {
		/// <summary>
		/// Initialises a new instance of the FieldMetadataTableRow
		/// </summary>
		/// <param name="contents">The contents of the file</param>
		/// <param name="offset">The offset of the current row</param>
		public FieldMetadataTableRow(MetadataStream stream, byte[] contents, Offset offset) {
			this.FileOffset = offset;
			this.Flags = (FieldAttributes)FieldReader.ToUInt16(contents, offset.Shift(2));
			this.Name = new StringIndex(stream, offset);
			this.Signiture = new BlobIndex(stream.SizeOfBlobIndexes, contents, Reflection.Signitures.Signitures.Field, offset);
		}

		#region Properties
		/// <summary>A 2-byte mask of FieldAttributes</summary>
		public FieldAttributes Flags { get; set; }
		/// <summary>An index in to the string heap</summary>
		public StringIndex Name { get; set; }
		/// <summary>An index in to the Blob heap</summary>
		public BlobIndex Signiture { get; set; }
		#endregion
	}
}
