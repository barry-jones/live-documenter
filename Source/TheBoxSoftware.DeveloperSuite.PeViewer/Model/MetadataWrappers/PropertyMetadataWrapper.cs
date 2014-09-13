using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.DeveloperSuite.PEViewer.Model.MetadataWrappers {
	using TheBoxSoftware.Reflection.Core.COFF;
	using TheBoxSoftware.Reflection.Core;

	internal class PropertyMetadataWrapper {
		public PropertyMetadataWrapper(MetadataStream file, List<MetadataRow> methods) {
			this.Items = new List<PropertyEntry>();
			foreach (PropertyMetadataTableRow current in methods) {
				this.Items.Add(new PropertyEntry(file.OwningFile.GetMetadataDirectory(), current));
			}
		}
		public List<PropertyEntry> Items { get; set; }
		public class PropertyEntry {
			public PropertyEntry(MetadataDirectory directory, PropertyMetadataTableRow row) {
				this.FileOffset = string.Format("0x{0:x}", row.FileOffset);
				this.Name = ((StringStream)directory.Streams[Streams.StringStream]).GetString(row.Name.Value);
				this.Flags = string.Format("0x{0:x}", row.Flags);
				this.Type = string.Format("0x{0:x}", row.Type);
			}

			public string FileOffset { get; set; }
			public string Name { get; set; }
			public string Flags { get; set; }
			public string Type { get; set; }			
		}
	}
}
