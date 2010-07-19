using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection.Core.COFF {
	/// <summary>
	/// Describes a method definition loaded from the metadata tables in the pe/coff
	/// file.
	/// </summary>
	public class MethodMetadataTableRow : MetadataRow {

		public static short Size = 14;

		/// <summary>
		/// Initialises a new instance of the MethodMetadataTableRow class
		/// </summary>
		/// <param name="stream">The stream containing the metadata</param>
		/// <param name="contents">The contents of the file</param>
		/// <param name="offset">The offset of the current row</param>
		public MethodMetadataTableRow(MetadataStream stream, byte[] contents, Offset offset) {
			this.FileOffset = offset;
			this.RVA = FieldReader.ToUInt32(contents, offset.Shift(4));
			this.ImplFlags = (MethodImplFlags)FieldReader.ToUInt16(contents, offset.Shift(2));
			this.Flags = (MethodAttributes)FieldReader.ToUInt16(contents, offset.Shift(2));
			this.Name = new StringIndex(stream, offset);
			this.Signiture = new BlobIndex(stream.SizeOfBlobIndexes, contents, Reflection.Signitures.Signitures.MethodDef, offset);
			this.ParamList = new Index(stream, contents, offset, MetadataTables.Param);
		}

		#region Properties
		/// <summary>
		/// Address of the CIL method data
		/// </summary>
		public UInt32 RVA {
			get;
			set;
		}

		/// <summary>
		/// 2-byte bitmask of MethodImplAttributes
		/// </summary>
		public MethodImplFlags ImplFlags {
			get;
			set;
		}

		/// <summary>
		/// A 2-byte bitmask of MethodAttributes
		/// </summary>
		public MethodAttributes Flags {
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

		/// <summary>
		/// An index in to the param table
		/// </summary>
		public Index ParamList {
			get;
			set;
		}
		#endregion
	}
}