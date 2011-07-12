using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection.Core.COFF {
	public class CustomAttributeMetadataTableRow : MetadataRow {
		/// <summary>
		/// Initialises a new instance of the CustomAttributeMetadataTableRow
		/// </summary>
		/// <param name="stream">The stream containing the metadata</param>
		/// <param name="contents">The contents of the file</param>
		/// <param name="offset">The offset of the current row</param>
		public CustomAttributeMetadataTableRow(MetadataStream stream, byte[] contents, Offset offset) {
			this.FileOffset = offset;
			this.Parent = new CodedIndex(stream, offset, CodedIndexes.HasCustomAttribute);
			this.Type = new CodedIndex(stream, offset, CodedIndexes.CustomAttributeType);
			this.Value = FieldReader.ToUInt32(contents, offset.Shift(stream.SizeOfBlobIndexes), stream.SizeOfBlobIndexes);
		}

		#region Properties
		/// <summary>
		/// A HasCustomAttribute encoded index
		/// </summary>
		public CodedIndex Parent {
			get;
			set;
		}

		/// <summary>
		/// A CustomAttributeType encoded index (Def or Ref tables)
		/// </summary>
		public CodedIndex Type {
			get;
			set;
		}

		/// <summary>
		/// An index in to the blob heap
		/// </summary>
		public UInt32 Value {
			get;
			set;
		}
		#endregion
	}
}
