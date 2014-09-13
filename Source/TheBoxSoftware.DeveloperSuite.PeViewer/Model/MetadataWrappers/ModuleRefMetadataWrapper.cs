using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.DeveloperSuite.PEViewer.Model.MetadataWrappers {
	using TheBoxSoftware.Reflection.Core.COFF;
	using TheBoxSoftware.Reflection.Core;

	internal class ModuleRefMetadataWrapper {
		public ModuleRefMetadataWrapper(MetadataStream file, List<MetadataRow> methods) {
			this.Items = new List<ModuleRefEntry>();
			foreach (ModuleRefMetadataTableRow current in methods) {
				this.Items.Add(new ModuleRefEntry(file.OwningFile.GetMetadataDirectory(), current));
			}
		}
		public List<ModuleRefEntry> Items { get; set; }
		public class ModuleRefEntry {
			public ModuleRefEntry(MetadataDirectory directory, ModuleRefMetadataTableRow row) {
				this.FileOffset = string.Format("0x{0:x}", row.FileOffset);
				this.Name = ((StringStream)directory.Streams[Streams.StringStream]).GetString(row.Name.Value);
			}

			public string FileOffset { get; set; }
			public string Name { get; set; }
		}
	}
}
