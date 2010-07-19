using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.DeveloperSuite.PEViewer.Model.MetadataWrappers {
	using TheBoxSoftware.Reflection.Core.COFF;
	using TheBoxSoftware.Reflection.Core;

	public class TypeSpecMetadataWrapper {
		/// <summary>
		/// Initialises a new instance of the MethodDefMetadataWrapper.
		/// </summary>
		/// <param name="file">The file the metadata was loaded from</param>
		/// <param name="methods">The methods to wrap.</param>
		public TypeSpecMetadataWrapper(MetadataStream file, List<MetadataRow> methods) {
			this.Items = new List<TypeSpecEntry>();
			foreach (TypeSpecMetadataTableRow current in methods) {
				this.Items.Add(new TypeSpecEntry(file.OwningFile.GetMetadataDirectory(), current));
			}
		}

		/// <summary>
		/// The converted item list
		/// </summary>
		public List<TypeSpecEntry> Items { get; set; }

		/// <summary>
		/// Internal class for controlling the formatting and resolution of properties in the
		/// individual <see cref="TypeSpecMetadataTableRow"/> entries.
		/// </summary>
		public class TypeSpecEntry{
			public TypeSpecEntry(MetadataDirectory directory, TypeSpecMetadataTableRow row) {
				this.FileOffset = string.Format("0x{0:x}", row.FileOffset);
				this.Signiture = string.Format("0x{0:x}", row.Signiture.Value);
			}

			public string FileOffset { get; set; }
			public string Signiture { get; set; }
		}
	}
}
