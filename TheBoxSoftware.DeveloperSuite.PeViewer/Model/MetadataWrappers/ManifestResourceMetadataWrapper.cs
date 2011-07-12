using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.DeveloperSuite.PEViewer.Model.MetadataWrappers {
	using TheBoxSoftware.Reflection.Core.COFF;
	using TheBoxSoftware.Reflection.Core;

	internal class ManifestResourceMetadataWrapper {
		public ManifestResourceMetadataWrapper(MetadataStream file, List<MetadataRow> methods) {
			this.Items = new List<ManifestResourceEntry>();
			foreach (ManifestResourceMetadataTableRow current in methods) {
				this.Items.Add(new ManifestResourceEntry(file.OwningFile.GetMetadataDirectory(), current));
			}
		}
		public List<ManifestResourceEntry> Items { get; set; }
		public class ManifestResourceEntry {
			public ManifestResourceEntry(MetadataDirectory directory, ManifestResourceMetadataTableRow row) {
				this.FileOffset = string.Format("0x{0:x}", row.FileOffset);
				this.Name = ((StringStream)directory.Streams[Streams.StringStream]).GetString(row.Name.Value);
				this.Flags = string.Format("0x{0:x}", row.Flags);
				this.Implementation = row.Implementation.ToString();
				this.Offset = string.Format("0x{0:x}", row.Offset);
			}

			public string FileOffset { get; set; }
			public string Name { get; set; }
			public string Implementation { get; set; }
			public string Offset { get; set; }
			public string Flags { get; set; }
		}
	}
}
