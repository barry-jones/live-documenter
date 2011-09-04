using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.DeveloperSuite.PEViewer.Model.MetadataWrappers {
	using TheBoxSoftware.Reflection.Core;
	using TheBoxSoftware.Reflection.Core.COFF;

	class AssemblyRefMetadataWrapper {
		public AssemblyRefMetadataWrapper(MetadataStream file, List<MetadataRow> methods) {
			this.Items = new List<AssemblyRefEntry>();
			foreach (AssemblyRefMetadataTableRow current in methods) {
				this.Items.Add(new AssemblyRefEntry(file.OwningFile.GetMetadataDirectory(), current));
			}
		}
		public List<AssemblyRefEntry> Items { get; set; }
		public class AssemblyRefEntry {
			public AssemblyRefEntry(MetadataDirectory directory, AssemblyRefMetadataTableRow row) {
				this.FileOffset = string.Format("0x{0:x}", row.FileOffset);
				this.Name = ((StringStream)directory.Streams[Streams.StringStream]).GetString(row.Name.Value);
				this.Version = row.GetVersion().ToString();
				this.Culture = ((StringStream)directory.Streams[Streams.StringStream]).GetString(row.Culture.Value);
				this.HashValue = row.HashValue.ToString();
				this.Flags = row.Flags.ToString();
				this.PublicKeyOrToken = row.PublicKeyOrToken.ToString();
			}

			public string FileOffset { get; set; }
			public string Name { get; set; }
			public string Version { get; set; }
			public string Culture { get; set; }
			public string HashValue { get; set; }
			public string Flags { get; set; }
			public string PublicKeyOrToken { get; set; }
		}
	}
}
