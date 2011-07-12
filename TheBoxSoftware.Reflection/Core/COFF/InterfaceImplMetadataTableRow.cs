using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection.Core.COFF {
	/// <summary>
	/// Records the interfaces a type implements explicitly
	/// </summary>
	/// <remarks>
	/// Updated for 4-byte heap indexes
	/// </remarks>
	public class InterfaceImplMetadataTableRow : MetadataRow {
		/// <summary>
		/// Initialises a new instance of the InterfaceImplMetadataTableRow
		/// </summary>
		/// <param name="contents">The contents of the file</param>
		/// <param name="offset">The offset of this row</param>
		public InterfaceImplMetadataTableRow(MetadataStream stream, byte[] contents, Offset offset) {
			this.FileOffset = offset;
			this.Class = new Index(stream, contents, offset, MetadataTables.TypeDef);
			this.Interface = new CodedIndex(stream, offset, CodedIndexes.TypeDefOrRef);
		}

		#region Properties
		/// <summary>
		/// An index in to the TypeDef table
		/// </summary>
		public Index Class {
			get;
			set;
		}

		/// <summary>
		/// An index in to the TypeDef, TypeRef, or TypeSpec table. More precisely
		/// a TypeDefOrRef coded index.
		/// </summary>
		public CodedIndex Interface {
			get;
			set;
		}
		#endregion
	}
}
