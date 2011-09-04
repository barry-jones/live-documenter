using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection.Core.COFF {
	/// <summary>
	/// Holds information about unmanaged methods that can be reached from managed
	/// code, using PInvoke dispatch.
	/// </summary>
	public class ImplMapMetadataTableRow : MetadataRow {
		/// <summary>
		/// Initialises a new instance of the ImplMapMetadataTableRow class
		/// </summary>
		/// <permission cref="stream">The stream containing the metadata</permission>
		/// <param name="content">The content of the file</param>
		/// <param name="offset">The offset of the current row</param>
		public ImplMapMetadataTableRow(MetadataStream stream, byte[] content, Offset offset) {
			this.FileOffset = offset;
			this.MappingFlags = FieldReader.ToUInt16(content, offset.Shift(2));
			this.MemberForward = new CodedIndex(stream, offset, CodedIndexes.MemberForwarded);
			this.ImportName = new StringIndex(stream, offset);
			this.ImportScope = new Index(stream, content, offset, MetadataTables.ModuleRef);
		}

		#region Properties
		/// <summary>
		/// A 2-byte mask of PInvokeAttributes
		/// </summary>
		public UInt16 MappingFlags {
			get;
			set;
		}

		/// <summary>
		/// An index in to the Field or MethodDef table, a MemberForwarded
		/// coded index. However it only ever references the MethodDef because
		/// Field is never exported
		/// </summary>
		public CodedIndex MemberForward {
			get;
			set;
		}

		/// <summary>
		/// An index in to the string heap
		/// </summary>
		public StringIndex ImportName {
			get;
			set;
		}

		/// <summary>
		/// An index into the ModuleRef table
		/// </summary>
		public Index ImportScope {
			get;
			set;
		}
		#endregion
	}
}
