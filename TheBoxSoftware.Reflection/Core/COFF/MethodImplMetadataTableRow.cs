using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection.Core.COFF {
	/// <summary>
	/// Allows the compiler to override the default inheritance rules provided
	/// by the CLI.
	/// </summary>
	public class MethodImplMetadataTableRow : MetadataRow {
		/// <summary>
		/// Initialises a new instance of the MethodImplMetadataTableRow class
		/// </summary>
		/// <param name="stream">The stream containing the metadata</param>
		/// <param name="contents">The contents of the file</param>
		/// <param name="offset">The offset of the current row</param>
		public MethodImplMetadataTableRow(MetadataStream stream, byte[] contents, Offset offset) {
			this.FileOffset = offset;
			this.Class = new Index(stream, contents, offset, MetadataTables.TypeDef);
			this.MethodBody = new CodedIndex(stream, offset, CodedIndexes.MethodDefOrRef);
			this.MethodDeclaration = new CodedIndex(stream, offset, CodedIndexes.MethodDefOrRef);
		}

		#region Properties
		/// <summary>
		/// An index into the TypeDef table
		/// </summary>
		public Index Class {
			get;
			set;
		}

		/// <summary>
		/// An index in to a MethodDef or MemberRef, a MethodDefOrRef
		/// encoded index
		/// </summary>
		public CodedIndex MethodBody {
			get;
			set;
		}

		/// <summary>
		/// An index in to a MethodDef or MemberRef table, a MethodDefOrRef
		/// encoded index
		/// </summary>
		public CodedIndex MethodDeclaration {
			get;
			set;
		}
		#endregion
	}
}

