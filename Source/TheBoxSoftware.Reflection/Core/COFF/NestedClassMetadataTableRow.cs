using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection.Core.COFF {
	/// <summary>
	/// 
	/// </summary>
	public class NestedClassMetadataTableRow : MetadataRow {
		/// <summary>
		/// Initialises a new instance of the NestedClassMetadataTableRow class
		/// </summary>
		/// <param name="stream">The stream containing the metadata</param>
		/// <param name="contents">The contents of the file</param>
		/// <param name="offset">The offset of the current row</param>
		public NestedClassMetadataTableRow(MetadataStream stream, byte[] contents, Offset offset) {
			this.FileOffset = offset;
			this.NestedClass = new Index(stream, contents, offset, MetadataTables.TypeDef);
			this.EnclosingClass = new Index(stream, contents, offset, MetadataTables.TypeDef);
		}

		#region Properties
		/// <summary>
		/// An index in to the TypeDef table
		/// </summary>
		public Index NestedClass { get; set; }

		/// <summary>
		/// An index in to teh TypeDef table
		/// </summary>
		public Index EnclosingClass { get; set; }
		#endregion
	}
}
