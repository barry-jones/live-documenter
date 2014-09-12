using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection.Core.COFF {
	/// <summary>
	/// 
	/// </summary>
	/// <remarks>
	/// Updated for 4-byte heap indexes
	/// </remarks>
	[System.Diagnostics.DebuggerDisplay("Namespace[{Namespace}] Name[{Name}]")]
	public class TypeRefMetadataTableRow : MetadataRow {
		/// <summary>
		/// Initialises a new instance of the TypeRefMetadataTableRow class
		/// </summary>
		/// <param name="contents">The file contents</param>
		/// <param name="offset">The offset for this entry</param>
		public TypeRefMetadataTableRow(MetadataStream stream, byte[] contents, Offset offset) {
			this.FileOffset = offset;
			this.ResolutionScope = new CodedIndex(stream, offset, CodedIndexes.ResolutionScope);
			this.Name = new StringIndex(stream, offset);
			this.Namespace = new StringIndex(stream, offset);
		}

		#region Properties
		/// <summary>
		/// An index in to a Module, ModuleRef, AssemblyRef, or TypeRef table, or null.
		/// More precisely a ResolutionScope
		/// </summary>
		public CodedIndex ResolutionScope { get; set; }
		/// <summary>An index in to the string heap</summary>
		public StringIndex Name { get; set; }
		/// <summary>An index in to the string heap</summary>
		public StringIndex Namespace { get; set; }
		#endregion
	}
}
