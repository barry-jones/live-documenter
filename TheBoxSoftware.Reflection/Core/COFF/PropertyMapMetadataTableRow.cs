using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection.Core.COFF {
	public class PropertyMapMetadataTableRow : MetadataRow {
		public PropertyMapMetadataTableRow() {
		}

		/// <summary>
		/// Initialises a new instance of the PropertyMapMetadataTableRow
		/// </summary>
		/// <param name="stream">The stream containing the metadata</param>
		/// <param name="contents">The contents of the file</param>
		/// <param name="offset">The offset for this row</param>
		public PropertyMapMetadataTableRow(MetadataStream stream, byte[] contents, Offset offset) {
			this.FileOffset = offset;
			this.Parent = new Index(stream, contents, offset, MetadataTables.TypeDef);
			this.PropertyList = new Index(stream, contents, offset, MetadataTables.Property);
		}

		#region Properties
		/// <summary>
		/// An index in to the TypeDef table
		/// </summary>
		public Index Parent {
			get;
			set;
		}

		/// <summary>
		/// An index in to the Property table
		/// </summary>
		public Index PropertyList {
			get;
			set;
		}
		#endregion
	}
}
