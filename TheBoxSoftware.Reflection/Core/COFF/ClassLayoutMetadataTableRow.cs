using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection.Core.COFF {
	/// <summary>
	/// Describes a layout of a type when it is required to be layed out
	/// much like unmanaged structures
	/// </summary>
	public class ClassLayoutMetadataTableRow : MetadataRow {
		/// <summary>
		/// Initialises a new instnace of the ClassLayoutMetadataTableRow
		/// </summary>
		/// <param name="contents">The contents of the file</param>
		/// <param name="offset">The offset of this row</param>
		public ClassLayoutMetadataTableRow(MetadataStream stream, byte[] contents, Offset offset) {
			this.FileOffset = offset;
			this.PackingSize = BitConverter.ToUInt16(contents, offset.Shift(2));
			this.ClassSize = BitConverter.ToUInt32(contents, offset.Shift(4));
			this.Parent = new Index(stream, contents, offset, MetadataTables.TypeDef);
		}

		#region Properties
		/// <summary>
		/// A 2 byte-constant
		/// </summary>
		public UInt16 PackingSize {
			get;
			set;
		}

		/// <summary>
		/// A 4-byte constant
		/// </summary>
		public UInt32 ClassSize {
			get;
			set;
		}

		/// <summary>
		/// An index in to the TypeDef table
		/// </summary>
		public Index Parent {
			get;
			set;
		}
		#endregion
	}
}
