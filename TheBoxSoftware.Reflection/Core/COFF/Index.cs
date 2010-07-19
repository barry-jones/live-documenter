using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection.Core.COFF {
	/// <summary>
	/// Represents a simple index item, where the item is an entry in to - generall -
	/// a metadata table.
	/// </summary>
	public struct Index {
		/// <field>
		/// The value for the index.
		/// </field>
		public UInt32 Value;

		/// <summary>
		/// Private constructor which initialises the structure to a known
		/// index.
		/// </summary>
		/// <remarks>
		/// This is currently only used internally to be able to convert implicitly
		/// between an integer and an Index structure.
		/// </remarks>
		/// <param name="value">The value for the index.</param>
		private Index(UInt32 value) {
			this.Value = value;
		}

		/// <summary>
		/// Initialises a new instance of the Index.
		/// </summary>
		/// <param name="stream">The stream containing the metadata</param>
		/// <param name="contents">The contents of the file</param>
		/// <param name="offset">The offset of the current index to be read</param>
		/// <param name="table">The table this index points to</param>
		public Index(MetadataStream stream, byte[] contents, Offset offset, MetadataTables table) {
			int bytesToRead = stream.RowsInPresentTables.ContainsKey(table) && stream.RowsInPresentTables[table] > UInt16.MaxValue ? 4 : 2;
			this.Value = FieldReader.ToUInt32(contents, offset.Shift(bytesToRead), bytesToRead);
		}

		/// <summary>
		/// Calculates the required size in bytes to store an index for that table.
		/// </summary>
		/// <param name="table">The table to get the index size for.</param>
		/// <param name="stream">The stream containing the table information.</param>
		/// <returns>A byte that indicates the size of the indexes for that table in bytes.</returns>
		public static byte SizeOfIndex(MetadataTables table, MetadataStream stream) {
			return (byte)(stream.RowsInPresentTables.ContainsKey(table) && stream.RowsInPresentTables[table] > UInt16.MaxValue ? 4 : 2);
		}

		public override string ToString() {
			return this.Value.ToString();
		}

		#region Operator Overloads
		/// <summary>
		/// Implicitly allow the Index to be converted to an UInt32
		/// </summary>
		/// <param name="index">The index to convert</param>
		/// <returns>The UInt32 representation of the Index</returns>
		public static implicit operator UInt32(Index index) {
			return index.Value;
		}

		/// <summary>
		/// Implicity allow the casting from a uint32 to an Index
		/// </summary>
		/// <param name="index">The index to initialise the Index with</param>
		/// <returns>The instance of Index initialised appropriately</returns>
		public static implicit operator Index(UInt32 index) {
			return new Index(index);
		}

		/// <summary>
		/// Implicitly allow the Index to be converted to an Int32
		/// </summary>
		/// <param name="index">The index to convert</param>
		/// <returns>The UInt32 representation of the Index</returns>
		public static implicit operator Int32(Index index) {
			return (Int32)index.Value;
		}

		/// <summary>
		/// Implicity allow the casting from a int32 to an Index
		/// </summary>
		/// <param name="index">The index to initialise the Index with</param>
		/// <returns>The instance of Index initialised appropriately</returns>
		public static implicit operator Index(Int32 index) {
			return new Index((UInt32)index);
		}
		#endregion
	}

	/// <summary>
	/// Represents an index in to the metadata StringStream.
	/// </summary>
	public struct StringIndex {
		public Int32 Value;

		/// <summary>
		/// Initialises a new instance of the StringStream.
		/// </summary>
		/// <param name="stream">The stream this index is for.</param>
		/// <param name="offset">The offset for the index in the file.</param>
		public StringIndex(MetadataStream stream, Offset offset) {
			// This is always an unsigned int, reading an int here will get negative
			// indexes; so always cast to int after.
			this.Value = (int)FieldReader.ToUInt32(
				stream.OwningFile.FileContents,
				offset.Shift(stream.SizeOfStringIndexes),
				stream.SizeOfStringIndexes);
		}
	}

	/// <summary>
	/// Represents an index in to the BlobHeap. This heap contains details such as
	/// signitures and other userful metadata information.
	/// </summary>
	public struct BlobIndex {
		public UInt32 Value;
		public Signitures.Signitures SignitureType;

		public BlobIndex(byte sizeOfBlobIndexes, byte[] fileContents, Signitures.Signitures signitureType, Offset offset) {
			this.SignitureType = signitureType;
			this.Value = FieldReader.ToUInt32(
				fileContents,
				offset.Shift(sizeOfBlobIndexes),
				sizeOfBlobIndexes);
		}
	}
}