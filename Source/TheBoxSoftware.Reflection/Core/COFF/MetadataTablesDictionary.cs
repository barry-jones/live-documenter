using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace TheBoxSoftware.Reflection.Core.COFF {
	/// <summary>
	/// Represents a dictionary of all of the metadata rows loaded from an assembly.
	/// </summary>
	/// <remarks>
	/// Internally this stores the unique FileOffset against an index in a table for
	/// the stored MetadataRow items.
	/// </remarks>
	public class MetadataTablesDictionary : Dictionary<MetadataTables, MetadataRow[]> {
		/// <summary>
		/// Stores a set of values that relate a file offset (unique for any metadata row)
		/// against an index in its associated table. This is to reduce seek times when
		/// searching for elements. Whenever an entry is added to the dictionary its
		/// metadata rows should be added to this dictionary.
		/// </summary>
		private Dictionary<int, int> indexTable = new Dictionary<int, int>();

		/// <summary>
		/// Initialises a new instance of the MetadataTablesDictionary class.
		/// </summary>
		public MetadataTablesDictionary() { }

		/// <summary>
		/// Initialises a new instance of the MetadataTablesDictionary class.
		/// </summary>
		/// <param name="capacity">The starting capacity.</param>
		/// <remarks>
		/// This can be used to set the starting capacity to the number of metadata
		/// tables defined in the pe coff file. To reduce the number of internal
		/// re-dimensioning.
		/// </remarks>
		public MetadataTablesDictionary(int capacity)
			: base(capacity) {
		}

		/// <summary>
		/// Sets a populated array of <see cref="MetadataRow"/>s against its associated
		/// metadata table.
		/// </summary>
		/// <param name="table">The table the rows have been loaded from.</param>
		/// <param name="rows">The rows that make up the table.</param>
		public void SetMetadataTable(MetadataTables table, MetadataRow[] rows) {
			this[table] = rows;

			// Add each entry to the index map
			int count = rows.Length;
			for (int i = 0; i < count; i++) {
				indexTable.Add(rows[i].FileOffset, i);
			}
		}

		/// <summary>
		/// Gets the metadata row information from the specified table
		/// at the specified index.
		/// </summary>
		/// <param name="table">The table the metadata row resides in</param>
		/// <param name="index">The index of the item in the table</param>
		/// <returns>The metadata row if it exists else null</returns>
		public MetadataRow GetEntryFor(MetadataTables table, int index) {
			MetadataRow o = null;
			if (this.ContainsKey(table) && index <= this[table].Length) {
				o = this[table][index - 1];
			}
			return o;
		}

		/// <summary>
		/// Returns the index for a specified metadata row from a metadata table.
		/// </summary>
		/// <param name="table">The table to get the metadata items index from</param>
		/// <param name="entry">The entry which resides in the metadata table</param>
		/// <returns>The index of the item or -1 if not found</returns>
		public int GetIndexFor(MetadataTables table, MetadataRow entry) {
			int index = -1;
			if (this.ContainsKey(table) && this.indexTable.ContainsKey(entry.FileOffset)) {
				index = this.indexTable[entry.FileOffset];
			}
			return index;
		}

		/// <summary>
		/// Obtains a collection of generic parameter metadata rows for the specified
		/// table and index.
		/// </summary>
		/// <param name="table">The table to get the details for.</param>
		/// <param name="index">The index of that table to get the details for.</param>
		/// <returns>A list of generic parameters for the specified details.</returns>
		/// <remarks>
		/// This method will iterate over the GenericParam metadata table and search
		/// for entries whose parent is the specified table and index and return them.
		/// </remarks>
		/// <exception cref="InvalidOperationException">
		/// The <paramref name="table"/> and <paramref name="index"/> are invalid, please
		/// see the exception details for more information.
		/// </exception>
		public List<GenericParamMetadataTableRow> GetGenericParametersFor(MetadataTables table, int index) {
			if (table != MetadataTables.TypeDef && table != MetadataTables.MethodDef) {
				throw new InvalidOperationException(Resources.ExceptionMessages.Ex_GenericParametersNotValid);
			}
			
			List<GenericParamMetadataTableRow> genericParameters = new List<GenericParamMetadataTableRow>();
			if(this.ContainsKey(MetadataTables.GenericParam)) {
				MetadataRow[] rows = this[MetadataTables.GenericParam];
				int count = rows.Length;

				for (int i = 0; i < count; i++) {
					GenericParamMetadataTableRow current = (GenericParamMetadataTableRow)rows[i];
					CodedIndex owner = current.Owner;

					if (owner.Index == index && owner.Table == table) {
						genericParameters.Add(current);
					}
				}
			}
			return genericParameters;
		}
	}
}