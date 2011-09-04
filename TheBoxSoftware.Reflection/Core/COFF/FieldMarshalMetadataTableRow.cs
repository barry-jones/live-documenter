using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection.Core.COFF {
	/// <summary>
	/// Links an existing row in the Field or Param table to information
	/// in the blob heap that defines how that field or parameter should
	/// be marshalled.
	/// </summary>
	public class FieldMarshalMetadataTableRow : MetadataRow {
		/// <summary>
		/// Initialises a new instance of the FieldMarshalMEtadataTableRow class
		/// </summary>
		/// <param name="stream">The stream containing the metadata</param>
		/// <param name="contents">The contents of the file</param>
		/// <param name="offset">The offset of the current row</param>
		public FieldMarshalMetadataTableRow(MetadataStream stream, byte[] contents, Offset offset) {
			this.FileOffset = offset;
			this.Parent = new CodedIndex(stream, offset, CodedIndexes.HasFieldMarshall);
			this.NativeType = FieldReader.ToUInt32(contents, offset.Shift(stream.SizeOfBlobIndexes), stream.SizeOfBlobIndexes);
		}

		#region Properties
		/// <summary>
		/// A HasFieldMarshal encoded index to the Field or Param tables
		/// </summary>
		public CodedIndex Parent {
			get;
			set;
		}

		/// <summary>
		/// An index in to the blob heap
		/// </summary>
		public UInt32 NativeType {
			get;
			set;
		}
		#endregion
	}
}
