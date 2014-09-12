using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.DeveloperSuite.PEViewer.Model.MetadataWrappers {
	using TheBoxSoftware.Reflection.Core.COFF;
	using TheBoxSoftware.Reflection.Core;
	internal class FieldRVAMetadataWrapper {
		public FieldRVAMetadataWrapper(MetadataStream file, List<MetadataRow> methods) {
			this.Items = new List<FieldRVAEntry>();
			foreach (FieldRVAMetadataTableRow current in methods) {
				this.Items.Add(new FieldRVAEntry(file.OwningFile.GetMetadataDirectory(), current));
			}
		}
		public List<FieldRVAEntry> Items { get; set; }
		public class FieldRVAEntry {
			public FieldRVAEntry(MetadataDirectory directory, FieldRVAMetadataTableRow row) {
				this.FileOffset = string.Format("0x{0:x}", row.FileOffset);
				this.Field = row.Field.Value.ToString();
				this.RVA = string.Format("0x{0:x}", row.RVA);
			}

			public string FileOffset { get; set; }
			public string Field { get; set; }
			public string RVA { get; set; }
		}
	}
}
