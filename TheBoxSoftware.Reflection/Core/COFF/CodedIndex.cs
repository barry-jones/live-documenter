using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection.Core.COFF {
	/// <summary>
	/// Represents a value which specifies both a table and an index in to that table
	/// from a single UInt16 or UInt32. <see cref="Details"/>
	/// </summary>
	public struct CodedIndex {
		/// <field>
		/// The MetadataTables value indicating the table this index is for.
		/// </field>
		public MetadataTables Table;
		/// <field>
		/// The index in to the MetadataTable.
		/// </field>
		public Index Index;

		#region Constructors
		/// <summary>
		/// Initialises a new CodedIndex.
		/// </summary>
		/// <param name="stream">The stream the coded index is in.</param>
		/// <param name="offset">The offset of the coded index in the stream.</param>
		/// <param name="codedIndex">The type of coded index to create.</param>
		public CodedIndex(MetadataStream stream, Offset offset, CodedIndexes codedIndex) {
			// is represented by 2 or 4 bytes depending on code and max index size in table(s)
			// if tag bits + rid bits <= 16 bits use 2 bytes else 4 bytes
			Details details = CodedIndex.GetDetails(stream, codedIndex);
			UInt32 value = FieldReader.ToUInt32(stream.OwningFile.FileContents, 
				offset.Shift(details.RequiredNumberOfBytes()), 
				details.RequiredNumberOfBytes());

			byte table;
			details.GetCodedIndex(value, out table, out this.Index.Value);
			this.Table = CodedIndex.GetTableForCode(codedIndex, table);
		}

		/// <summary>
		/// Initialises a already determined instance of a CodedIndex class.
		/// </summary>
		/// <param name="table">The table the coded index is for</param>
		/// <param name="index">The index in the table</param>
		public CodedIndex(MetadataTables table, UInt32 index) {
			this.Table = table;
			this.Index = index;
		}
		#endregion

		public static bool operator ==(CodedIndex first, CodedIndex second)
		{
			return (first.Table == second.Table && first.Index == second.Index);
		}

		public static bool operator !=(CodedIndex first, CodedIndex second) {
			return (first.Table != second.Table || first.Index != second.Index);
		}

		#region Methods
		/// <summary>
		/// Obtains the size of the specified <paramref name="codedIndex"/>.
		/// </summary>
		/// <param name="stream">The stream that contains the metadata.</param>
		/// <param name="codedIndex">The coded index to find the size of.</param>
		/// <returns>A byte containing the number of bytes to represent the index.</returns>
		public static byte SizeOfIndex(MetadataStream stream, CodedIndexes codedIndex) {
			return (byte)CodedIndex.GetDetails(stream, codedIndex).RequiredNumberOfBytes();
		}

		/// <summary>
		/// Returns the MetadaDataTable represented by the specified code
		/// </summary>
		/// <param name="codedIndex">The coded index being used</param>
		/// <param name="code">The calculated table code</param>
		/// <returns>The metadata table</returns>
		private static MetadataTables GetTableForCode(CodedIndexes codedIndex, byte code) {
			MetadataTables table = MetadataTables.Module;
			switch (codedIndex) {
				case CodedIndexes.HasFieldMarshall:
					switch (code) {
						case 0: table = MetadataTables.Field; break;
						case 1: table = MetadataTables.Param; break;
					}
					break;
				case CodedIndexes.HasSemantics:
					switch (code) {
						case 0: table = MetadataTables.Event; break;
						case 1: table = MetadataTables.Property; break;
					}
					break;
				case CodedIndexes.MethodDefOrRef:
					switch (code) {
						case 0: table = MetadataTables.MethodDef; break;
						case 1: table = MetadataTables.MemberRef; break;
					}
					break;
				case CodedIndexes.MemberForwarded:
					switch (code) {
						case 0: table = MetadataTables.Field; break;
						case 1: table = MetadataTables.MethodDef; break;
					}
					break;
				case CodedIndexes.TypeOrMethodDef:
					switch (code) {
						case 0: table = MetadataTables.TypeDef; break;
						case 1: table = MetadataTables.MethodDef; break;
					}
					break;
				case CodedIndexes.TypeDefOrRef:
					switch (code) {
						case 0: table = MetadataTables.TypeDef; break;
						case 1: table = MetadataTables.TypeRef; break;
						case 2: table = MetadataTables.TypeSpec; break;
					}
					break;
				case CodedIndexes.HasConstant:
					switch (code) {
						case 0: table = MetadataTables.Field; break;
						case 1: table = MetadataTables.Param; break;
						case 2: table = MetadataTables.Property; break;
					}
					break;
				case CodedIndexes.HasDeclSecurity:
					switch (code) {
						case 0: table = MetadataTables.TypeDef; break;
						case 1: table = MetadataTables.MethodDef; break;
						case 2: table = MetadataTables.Assembly; break;
					}
					break;
				case CodedIndexes.Implementation:
					switch (code) {
						case 0: table = MetadataTables.File; break;
						case 1: table = MetadataTables.AssemblyRef; break;
						case 2: table = MetadataTables.ExportedType; break;
					}
					break;
				case CodedIndexes.ResolutionScope:
					switch (code) {
						case 0: table = MetadataTables.Module; break;
						case 1: table = MetadataTables.ModuleRef; break;
						case 2: table = MetadataTables.AssemblyRef; break;
						case 3: table = MetadataTables.TypeRef; break;
					}
					break;
				case CodedIndexes.MemberRefParent:
					switch (code) {
						case 0: table = MetadataTables.TypeDef; break;
						case 1: table = MetadataTables.TypeRef; break;
						case 2: table = MetadataTables.ModuleRef; break;
						case 3: table = MetadataTables.MethodDef; break;
						case 4: table = MetadataTables.TypeSpec; break;
					}
					break;
				case CodedIndexes.CustomAttributeType:
					switch (code) {
						//case 0: table = MetadataTables.TypeDef;
						//case 1: table = MetadataTables.TypeRef;
						case 2: table = MetadataTables.MethodDef; break;
						case 3: table = MetadataTables.MemberRef; break;
						//case 4: table = MetadataTables.TypeSpec;
					}
					break;
				case CodedIndexes.HasCustomAttribute:
					switch (code) {
						case 0: table = MetadataTables.MethodDef; break;
						case 1: table = MetadataTables.Field; break;
						case 2: table = MetadataTables.TypeRef; break;
						case 3: table = MetadataTables.TypeDef; break;
						case 4: table = MetadataTables.Param; break;
						case 5: table = MetadataTables.InterfaceImpl; break;
						case 6: table = MetadataTables.MemberRef; break;
						case 7: table = MetadataTables.Module; break;
						// case 8: table = MetadataTables.Permission; break;
						case 9: table = MetadataTables.Property; break;
						case 10: table = MetadataTables.Event; break;
						case 11: table = MetadataTables.StandAloneSig; break;
						case 12: table = MetadataTables.ModuleRef; break;
						case 13: table = MetadataTables.TypeSpec; break;
						case 14: table = MetadataTables.Assembly; break;
						case 15: table = MetadataTables.AssemblyRef; break;
						case 16: table = MetadataTables.File; break;
						case 17: table = MetadataTables.ExportedType; break;
						case 18: table = MetadataTables.ManifestResource; break;
					}
					break;
			}
			return table;
		}

		/// <summary>
		/// Returns details of the tables for the specified coded index
		/// </summary>
		/// <param name="details">The details structure to populate</param>
		/// <param name="codedIndex">The coded index being used</param>
		private static void GetTablesInformation(ref Details details, CodedIndexes codedIndex) {
			details.Tables = new List<MetadataTables>();
			switch (codedIndex) {
				case CodedIndexes.HasFieldMarshall:
					details.Tables.Add(MetadataTables.Field);
					details.Tables.Add(MetadataTables.Param);
					details.Mask = 0x01;
					break;
				case CodedIndexes.HasSemantics:
					details.Tables.Add(MetadataTables.Event);
					details.Tables.Add(MetadataTables.Property);
					details.Mask = 0x01;
					break;
				case CodedIndexes.MethodDefOrRef:
					details.Tables.Add(MetadataTables.MethodDef);
					details.Tables.Add(MetadataTables.MemberRef);
					details.Mask = 0x01;
					break;
				case CodedIndexes.MemberForwarded:
					details.Tables.Add(MetadataTables.Field);
					details.Tables.Add(MetadataTables.MethodDef);
					details.Mask = 0x01;
					break;
				case CodedIndexes.TypeOrMethodDef:
					details.Tables.Add(MetadataTables.TypeRef);
					details.Tables.Add(MetadataTables.MethodDef);
					details.Mask = 0x01;
					break;
				case CodedIndexes.TypeDefOrRef:
					details.Tables.Add(MetadataTables.TypeDef);
					details.Tables.Add(MetadataTables.TypeRef);
					details.Tables.Add(MetadataTables.TypeSpec);
					details.Mask = 0x03;
					break;
				case CodedIndexes.HasConstant:
					details.Tables.Add(MetadataTables.Field);
					details.Tables.Add(MetadataTables.Param);
					details.Tables.Add(MetadataTables.Property);
					details.Mask = 0x03;
					break;
				case CodedIndexes.HasDeclSecurity:
					details.Tables.Add(MetadataTables.TypeDef);
					details.Tables.Add(MetadataTables.MethodDef);
					details.Tables.Add(MetadataTables.Assembly);
					details.Mask = 0x03;
					break;
				case CodedIndexes.Implementation:
					details.Tables.Add(MetadataTables.File);
					details.Tables.Add(MetadataTables.AssemblyRef);
					details.Tables.Add(MetadataTables.ExportedType);
					details.Mask = 0x03;
					break;
				case CodedIndexes.ResolutionScope:
					details.Tables.Add(MetadataTables.Module);
					details.Tables.Add(MetadataTables.ModuleRef);
					details.Tables.Add(MetadataTables.AssemblyRef);
					details.Tables.Add(MetadataTables.TypeRef);
					details.Mask = 0x03;
					break;
				case CodedIndexes.MemberRefParent:
					details.Tables.Add(MetadataTables.TypeDef);
					details.Tables.Add(MetadataTables.TypeRef);
					details.Tables.Add(MetadataTables.ModuleRef);
					details.Tables.Add(MetadataTables.MethodDef);
					details.Tables.Add(MetadataTables.TypeSpec);
					details.Mask = 0x07;
					break;
				case CodedIndexes.CustomAttributeType:
					// tables.Add(MetadataTables.TypeDef);
					// tables.Add(MetadataTables.TypeRef);
					details.Tables.Add(MetadataTables.MethodDef);
					details.Tables.Add(MetadataTables.MemberRef);
					// tables.Add(MetadataTables.TypeSpec);
					details.Mask = 0x07;
					break;
				case CodedIndexes.HasCustomAttribute:
					details.Tables.Add(MetadataTables.MethodDef);
					details.Tables.Add(MetadataTables.Field);
					details.Tables.Add(MetadataTables.TypeRef);
					details.Tables.Add(MetadataTables.TypeDef);
					details.Tables.Add(MetadataTables.Param);
					details.Tables.Add(MetadataTables.InterfaceImpl);
					details.Tables.Add(MetadataTables.MemberRef);
					details.Tables.Add(MetadataTables.Module);
					//tables.Add(MetadataTables.Permission);
					details.Tables.Add(MetadataTables.Property);
					details.Tables.Add(MetadataTables.Event);
					details.Tables.Add(MetadataTables.StandAloneSig);
					details.Tables.Add(MetadataTables.ModuleRef);
					details.Tables.Add(MetadataTables.TypeSpec);
					details.Tables.Add(MetadataTables.Assembly);
					details.Tables.Add(MetadataTables.AssemblyRef);
					details.Tables.Add(MetadataTables.File);
					details.Tables.Add(MetadataTables.ExportedType);
					details.Tables.Add(MetadataTables.ManifestResource);
					details.Mask = 0x1F;
					break;
			}
			details.BitsToRepresentTag = details.Mask == 1 
				? (byte)1
				: (byte)Math.Round(Math.Log(details.Mask, 2), MidpointRounding.AwayFromZero);
		}

		/// <summary>
		/// Obtains the basic details required to calculate the coded index
		/// </summary>
		/// <param name="stream">The metadata stream</param>
		/// <param name="codedIndex">The coded index being used</param>
		/// <returns>Basic details</returns>
		private static Details GetDetails(MetadataStream stream, CodedIndexes codedIndex) {
			Details details = new Details();

			if (stream.codedIndexMap.ContainsKey(codedIndex)) {
				details = stream.codedIndexMap[codedIndex];
			}
			else {
				CodedIndex.GetTablesInformation(ref details, codedIndex);

				// calculate the max rows in the specific table and in prospective tables
				//details.MaxRowsInCodedTable = 0;
				details.MaxRowsInTables = 0;
				int count = details.Tables.Count;
				for (int i = 0; i < count; i++) {
					// check if the table is used, otherwise it will fail on some index checks
					if (stream.Tables.ContainsKey(details.Tables[i])) {
						if (details.MaxRowsInTables < stream.RowsInPresentTables[details.Tables[i]]) {
							details.MaxRowsInTables = (UInt32)stream.RowsInPresentTables[details.Tables[i]];
						}
					}
				}

				// Dont forget to add it to the map...
				stream.codedIndexMap[codedIndex] = details;
			}

			return details;
		}

		/// <summary>
		/// Returns a string representation of this coded index
		/// </summary>
		/// <returns>A string</returns>
		public override string ToString() {
			return string.Format("Table: {0}, Index:{1}", this.Table.ToString(), this.Index.ToString());
		}
		#endregion

		#region Internals
		/// <summary>
		/// Basic details for this coded index
		/// </summary>
		internal struct Details {
			public UInt32 Mask;
			public byte BitsToRepresentTag;
			public List<MetadataTables> Tables;
			public UInt32 MaxRowsInTables;

			/// <summary>
			/// Indicates if the coded index is larger than a 2byte value (16 bits)
			/// </summary>
			/// <returns>A boolean</returns>
			public bool IsLarge() {
				return this.MaxRowsInTables > System.Math.Pow(2, 16 - this.BitsToRepresentTag);
			}

			/// <summary>
			/// Returns the number of bytes required to hold this coded index
			/// </summary>
			/// <returns>The number of bytes</returns>
			public Int32 RequiredNumberOfBytes() {
				return this.IsLarge() ? 4 : 2;
			}

			/// <summary>
			/// Utilises the details to calculate the table code and index for this coded index
			/// </summary>
			/// <param name="value">The value from the stream</param>
			/// <param name="code">The code to be populated</param>
			/// <param name="index">The index to be populated</param>
			public void GetCodedIndex(UInt32 value, out byte code, out UInt32 index) {
				if (this.IsLarge()) {
					code = (byte)(value & this.Mask);
					index = (value & ~this.Mask) >> this.BitsToRepresentTag;
				}
				else {
					UInt16 inMask = (UInt16)this.Mask;
					UInt16 shortValue = (UInt16)value;
					code = (byte)(value & inMask);
					index = (UInt16)((shortValue & ~inMask) >> this.BitsToRepresentTag); // shift to drop the masked bits
				}
			}
		}
		#endregion
	}
}

