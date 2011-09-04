using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection.Core.COFF {
	public class PropertyMetadataTableRow : MetadataRow {
		/// <summary>
		/// Initialises a new instance of the PropertyMetadataTableRow class
		/// </summary>
		/// <param name="stream">The stream containing the metadata</param>
		/// <param name="contents">The contents of the file</param>
		/// <param name="offset">The offset of this row</param>
		public PropertyMetadataTableRow(MetadataStream stream, byte[] contents, Offset offset) {
			this.FileOffset = offset;
			this.Flags = FieldReader.ToUInt16(contents, offset.Shift(2));
			this.Name = new StringIndex(stream, offset);
			this.Type = FieldReader.ToUInt32(contents, offset.Shift(stream.SizeOfBlobIndexes), stream.SizeOfBlobIndexes);
		}

		#region Properties
		/// <summary>
		/// A 2-byte bitmask of PropertyAttributes
		/// </summary>
		public UInt16 Flags {
			get;
			set;
		}

		/// <summary>
		/// An index in to the string heap
		/// </summary>
		public StringIndex Name {
			get;
			set;
		}

		/// <summary>
		/// An index in to the blob heap for the signiture
		/// </summary>
		public UInt32 Type {
			get;
			set;
		}
		#endregion
	}
}
