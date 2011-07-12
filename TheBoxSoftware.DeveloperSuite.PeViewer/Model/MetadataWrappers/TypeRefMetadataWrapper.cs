using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.DeveloperSuite.PEViewer.Model.MetadataWrappers {
	using TheBoxSoftware.Reflection.Core.COFF;
	using TheBoxSoftware.Reflection.Core;

	class TypeRefMetadataWrapper {
		public TypeRefMetadataWrapper(MetadataStream file, List<MetadataRow> types) {
			this.Items = new List<TypeRefEntry>();
			foreach (TypeRefMetadataTableRow current in types) {
				this.Items.Add(new TypeRefEntry(file.OwningFile.GetMetadataDirectory(), current));
			}
		}

		/// <summary>
		/// The converted item list
		/// </summary>
		public List<TypeRefEntry> Items { get; set; }

		public class TypeRefEntry {
			public TypeRefEntry(MetadataDirectory directory, TypeRefMetadataTableRow row) {
				this.FileOffset = string.Format("0x{0:x}", row.FileOffset);
				this.Name = ((StringStream)directory.Streams[Streams.StringStream]).GetString(row.Name.Value);
				this.Namespace = ((StringStream)directory.Streams[Streams.StringStream]).GetString(row.Namespace.Value);
				this.ResolutionScope = row.ResolutionScope.ToString();
			}

			public string FileOffset { get; set; }
			public string Name { get; set; }
			public string Namespace { get; set; }
			public string ResolutionScope { get; set; }
		}
	}
}
