using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.DeveloperSuite.PEViewer.Model.MetadataWrappers {
	using TheBoxSoftware.Reflection.Core.COFF;
	using TheBoxSoftware.Reflection.Core;

	internal class FieldMetadataWrapper {
		public FieldMetadataWrapper(MetadataStream file, List<MetadataRow> methods) {
			this.Items = new List<FieldEntry>();
			foreach (FieldMetadataTableRow current in methods) {
				this.Items.Add(new FieldEntry(file.OwningFile.GetMetadataDirectory(), current));
			}
		}
		public List<FieldEntry> Items { get; set; }
		public class FieldEntry {
			public FieldEntry(MetadataDirectory directory, FieldMetadataTableRow row) {
				this.FileOffset = string.Format("0x{0:x}", row.FileOffset);
				this.Name = ((StringStream)directory.Streams[Streams.StringStream]).GetString(row.Name.Value);
				this.Flags = row.Flags.ToString();
				this.Signiture = string.Format("0x{0:x}", row.Signiture.Value);
			}

			public string FileOffset { get; set; }
			public string Name { get; set; }
			public string Flags { get; set; }
			public string Signiture { get; set; }
		}
	}
}
