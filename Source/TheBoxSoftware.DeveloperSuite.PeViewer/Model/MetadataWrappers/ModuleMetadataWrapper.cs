using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.DeveloperSuite.PEViewer.Model.MetadataWrappers {
	using TheBoxSoftware.Reflection.Core.COFF;
	using TheBoxSoftware.Reflection.Core;

	internal class ModuleMetadataWrapper {
		public ModuleMetadataWrapper(MetadataStream file, List<MetadataRow> modules) {
			this.Items = new List<ModuleEntry>();
			foreach (ModuleMetadataTableRow current in modules) {
				this.Items.Add(new ModuleEntry(file.OwningFile.GetMetadataDirectory(), current));
			}
		}
		public List<ModuleEntry> Items { get; set; }
		public class ModuleEntry {
			public ModuleEntry(MetadataDirectory directory, ModuleMetadataTableRow row) {
				this.FileOffset = string.Format("0x{0:x}", row.FileOffset);
				this.Name = ((StringStream)directory.Streams[Streams.StringStream]).GetString(row.Name.Value);
				this.EncBaseId = row.EncBaseId.ToString();
				this.EncId = row.EncId.ToString();
				this.Generation = row.Generation.ToString();
				this.Mvid = ((GuidStream)directory.Streams[Streams.GuidStream]).GetGuid(row.Mvid).ToString();
			}

			public string FileOffset { get; set; }
			public string Name { get; set; }
			public string EncBaseId { get; set; }
			public string EncId { get; set; }
			public string Generation { get; set; }
			public string Mvid { get; set; }
		}
	}
}
