using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection.Core.COFF {
	/// <summary>
	/// 
	/// </summary>
	/// <seealso cref="MethodSemanticsAttributes"/>
	public class MethodSemanticsMetadataTableRow : MetadataRow {
		/// <summary>
		/// Initialises a new instance of the MethodSemanticsMetadataTableRow class
		/// </summary>
		/// <param name="stream">The stream containing the metadata</param>
		/// <param name="contents">The contents of the file</param>
		/// <param name="offset">The offset of the current row</param>
		public MethodSemanticsMetadataTableRow(MetadataStream stream, byte[] contents, Offset offset) {
			this.FileOffset = offset;
			this.Semantics = (MethodSemanticsAttributes)BitConverter.ToUInt16(contents, offset.Shift(2));
			this.Method = new Index(stream, contents, offset, MetadataTables.MethodDef);
			this.Association = new CodedIndex(stream, offset, CodedIndexes.HasSemantics);
		}

		#region Properties
		/// <summary>
		/// A 2-byte bitmask of type <see cref="MethodSemanticsAttributes"/>.
		/// </summary>
		public MethodSemanticsAttributes Semantics {
			get;
			set;
		}

		/// <summary>
		/// An index into the MethodDef table
		/// </summary>
		public Index Method {
			get;
			set;
		}

		/// <summary>
		/// An index into the Event or Property table, a HasSemantics coded index
		/// </summary>
		public CodedIndex Association {
			get;
			set;
		}
		#endregion
	}
}
