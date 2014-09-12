using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection.Core.COFF {
	/// <summary>
	/// Represents the details of an Assembly stored in metadata.
	/// </summary>
	/// <remarks>
	/// Each PE/COFF file can only contain a reference to zero or one Assembly
	/// metadata row.
	/// </remarks>
	public class AssemblyMetadataTableRow : MetadataRow {
		/// <summary>
		/// Initialises a new instance of the AssemblyMetadataTableRow class
		/// </summary>
		/// <param name="stream">The stream containing the metadat</param>
		/// <param name="contents">The contents of the file</param>
		/// <param name="offset">The offset of the current row</param>
		public AssemblyMetadataTableRow(MetadataStream stream, byte[] contents, Offset offset) {
			this.FileOffset = offset;
			this.HashAlgId = FieldReader.ToUInt32(contents, offset.Shift(4));
			this.MajorVersion = FieldReader.ToUInt16(contents, offset.Shift(2));
			this.MinorVersion = FieldReader.ToUInt16(contents, offset.Shift(2));
			this.BuildNumber = FieldReader.ToUInt16(contents, offset.Shift(2));
			this.RevisionNumber = FieldReader.ToUInt16(contents, offset.Shift(2));
			this.Flags = (AssemblyFlags)FieldReader.ToUInt32(contents, offset.Shift(4));
			this.PublicKey = FieldReader.ToUInt32(contents, offset.Shift(stream.SizeOfBlobIndexes), stream.SizeOfBlobIndexes);
			this.Name = new StringIndex(stream, offset);
			this.Culture = new StringIndex(stream, offset);
		}

		#region Methods
		/// <summary>
		/// Returns a populated version class with the parsed version details for this
		/// assembly.
		/// </summary>
		/// <returns>The populated <see cref="Version"/> instance.</returns>
		public Version GetVersion() {
			return new Version(
				this.MajorVersion,
				this.MinorVersion,
				this.BuildNumber,
				this.RevisionNumber);
		}
		#endregion

		#region Properties
		/// <summary>
		/// 4-byte constant of AssemblyHashAlgorithm
		/// </summary>
		public UInt32 HashAlgId { get; set; }

		/// <summary>
		/// Version details
		/// </summary>
		public UInt16 MajorVersion { get; set; }

		/// <summary>
		/// Version details
		/// </summary>
		public UInt16 MinorVersion { get; set; }

		/// <summary>
		/// Version details
		/// </summary>
		public UInt16 BuildNumber { get; set; }

		/// <summary>
		/// Version details
		/// </summary>
		public UInt16 RevisionNumber { get; set; }

		/// <summary>
		/// 4-byte bitmask of AssemblyFlags
		/// </summary>
		public AssemblyFlags Flags { get; set; }

		/// <summary>
		/// An index in to the blob heap
		/// </summary>
		public UInt32 PublicKey { get; set; }

		/// <summary>
		/// An index in to the string heap
		/// </summary>
		public StringIndex Name { get; set; }

		/// <summary>
		/// An index in to the string heap
		/// </summary>
		public StringIndex Culture { get; set; }
		#endregion
	}
}
