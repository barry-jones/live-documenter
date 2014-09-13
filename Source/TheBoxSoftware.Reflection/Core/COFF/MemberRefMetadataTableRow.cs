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
	public class MemberRefMetadataTableRow : MetadataRow {
		/// <summary>
		/// Initialises a new instance of the MemberRefMetadataTableRow
		/// </summary>
		/// <param name="contents">The contents of the file</param>
		/// <param name="offset">The offset of this row</param>
		public MemberRefMetadataTableRow(MetadataStream stream, byte[] contents, Offset offset) {
			this.FileOffset = offset;
			this.Class = new CodedIndex(stream, offset, CodedIndexes.MemberRefParent);
			this.Name = new StringIndex(stream, offset);
			this.Signiture = new BlobIndex(stream.SizeOfBlobIndexes, contents, Reflection.Signitures.Signitures.MethodDef, offset);
		}

		#region Properties
		/// <summary>
		/// An index in to the MethodDef, ModuleRef, TypeRef, or TypeSpec tables,
		/// more precisely a MemberRefParent coded index.
		/// </summary>
		public CodedIndex Class {
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
		/// An index in to the blob heap
		/// </summary>
		public BlobIndex Signiture {
			get;
			set;
		}
		#endregion
	}
}
