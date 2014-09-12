using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection.Core.COFF {
	/// <summary>
	/// Created by a .module extern directive in the assembly
	/// </summary>
	public class ModuleRefMetadataTableRow : MetadataRow {
		/// <summary>
		/// Initialises a new instance of the ModuleRefMetadataTableRow class
		/// </summary>
		/// <param name="stream">The stream containing the metadata</param>
		/// <param name="contents">The contents of the file</param>
		/// <param name="offset">The offset of the current row</param>
		public ModuleRefMetadataTableRow(MetadataStream stream, byte[] contents, Offset offset)  {
			this.FileOffset = offset;
			this.Name = new StringIndex(stream, offset);
		}

		#region Properties
		/// <summary>
		/// An index in to the string heap
		/// </summary>
		public StringIndex Name {
			get;
			set;
		}
		#endregion
	}
}
