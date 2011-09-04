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
	public class TypeDefMetadataTableRow : MetadataRow {
		/// <summary>
		/// Initialises an instance of the TypeDefMetadataTableRow
		/// </summary>
		/// <param name="contents">The contents of the file</param>
		/// <param name="offset">The offset of the current row</param>
		public TypeDefMetadataTableRow(MetadataStream stream, byte[] contents, Offset offset) {
			this.FileOffset = offset;
			this.Flags = (TypeAttributes)FieldReader.ToUInt32(contents, offset.Shift(4));
			this.Name = new StringIndex(stream, offset);
			this.Namespace = new StringIndex(stream, offset);
			this.Extends = new CodedIndex(stream, offset, CodedIndexes.TypeDefOrRef);
			this.FieldList = new Index(stream, contents, offset, MetadataTables.Field);
			this.MethodList = new Index(stream, contents, offset, MetadataTables.MethodDef);
		}

		#region Properties
		/// <summary>A 4-byte bitmask of TypeAttributes</summary>
		public TypeAttributes Flags { get; set; }
		/// <summary>An index in to the string heap</summary>
		public StringIndex Name { get; set; }
		/// <summary>An index in to the string heap</summary>
		public StringIndex Namespace { get; set; }
		/// <summary>
		/// An index in to the TypeDef, TypeRef, or TypeSpec table, more precisely
		/// TypeDefOrRef coded index.
		/// </summary>
		public CodedIndex Extends { get; set; }
		/// <summary>
		/// An index in to the Field table, marking the first of a continuous run
		/// of fields for the type. It continues until the smaller of, the last row
		/// in the table, the next run of fields.
		/// </summary>
		public Index FieldList { get; set; }
		/// <summary>An index in to the MethodDef table, continuos list as above.</summary>
		public Index MethodList { get; set; }
		#endregion
	}
}
