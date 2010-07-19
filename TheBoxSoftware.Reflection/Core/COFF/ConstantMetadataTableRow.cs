using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection.Core.COFF {
	/// <summary>
	/// Used to store compile time, constant values for fields, parameters and properties
	/// </summary>
	/// <remarks>
	/// <para>
	/// Note that constant information does not directly influence runtime behaviour, although
	/// it is visible via reflection. Compilers inspect this information, at compile time, when
	/// importing metadata, but the value of the constant itself, if used, becomes embedded in
	/// into the CIL stream the compiler emits. There are no CIL instructions to access constant
	/// table at runtime.
	/// </para>
	/// </remarks>
	public class ConstantMetadataTableRow : MetadataRow {
		/// <summary>
		/// Initialises a new instance of the ConstantMetadataTableRow
		/// </summary>
		/// <param name="contents">The contents fo the file</param>
		/// <param name="offset">The offset for the current row</param>
		public ConstantMetadataTableRow(MetadataStream stream, byte[] contents, Offset offset) {
			this.FileOffset = offset;
			this.Type = contents[offset.Shift(1)];
			this.PaddingZero = contents[offset.Shift(1)];
			this.Parent = new CodedIndex(stream, offset, CodedIndexes.HasConstant);
			this.Value = new BlobIndex(stream.SizeOfBlobIndexes, contents, TheBoxSoftware.Reflection.Signitures.Signitures.MethodDef, offset);
		}

		#region Properties
		/// <summary>
		/// The type of field that represents the constant.
		/// </summary>
		/// <remarks>
		/// For a <b>nullref</b> value for <i>FieldInit</i> in <i>ilasm</i> is <c>ELEMENT_TYPE_CLASS</c>
		/// with a 4-byte zero. Unlike uses of <c>ELEMENT_TYPE_CLASS</c> in signitures, this one is
		/// <i>not</i> followed by a type token.
		/// </remarks>
		public byte Type { get; set; }

		/// <summary>
		/// Padding
		/// </summary>
		private byte PaddingZero { get; set; }

		/// <summary>
		/// An index in to the Param, Field, or Property table. More precisely
		/// a HasConstant coded index
		/// </summary>
		public CodedIndex Parent { get; set; }

		/// <summary>
		/// An index in to the Blob heap
		/// </summary>
		public BlobIndex Value { get; set; }
		#endregion
	}
}