using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.DeveloperSuite.PEViewer.Model.MetadataWrappers {
	using TheBoxSoftware.Reflection.Core.COFF;
	using TheBoxSoftware.Reflection.Core;

	internal class FieldLayoutMetadataWrapper {
		public FieldLayoutMetadataWrapper(MetadataStream file, List<MetadataRow> methods) {
			this.Items = new List<FieldLayoutEntry>();
			foreach (FieldLayoutMetadataTableRow current in methods) {
				this.Items.Add(new FieldLayoutEntry(file.OwningFile.GetMetadataDirectory(), current));
			}
		}
		public List<FieldLayoutEntry> Items { get; set; }
		public class FieldLayoutEntry {
			public FieldLayoutEntry(MetadataDirectory directory, FieldLayoutMetadataTableRow row) {
				this.FileOffset = string.Format("0x{0:x}", row.FileOffset);
				this.Field = row.Field.Value.ToString();
				this.Offset = string.Format("0x{0:x}", row.Offset);
			}

			public string FileOffset { get; set; }
			public string Field { get; set; }
			public string Offset { get; set; }
		}
	}
}
