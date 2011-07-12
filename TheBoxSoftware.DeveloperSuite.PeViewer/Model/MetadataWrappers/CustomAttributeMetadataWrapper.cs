using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.DeveloperSuite.PEViewer.Model.MetadataWrappers {
	using TheBoxSoftware.Reflection.Core.COFF;
	using TheBoxSoftware.Reflection.Core;

	internal class CustomAttributeMetadataWrapper {
		public CustomAttributeMetadataWrapper(MetadataStream file, List<MetadataRow> methods) {
			this.Items = new List<CustomAttributeEntry>();
			foreach (CustomAttributeMetadataTableRow current in methods) {
				this.Items.Add(new CustomAttributeEntry(file.OwningFile.GetMetadataDirectory(), current));
			}
		}
		public List<CustomAttributeEntry> Items { get; set; }
		public class CustomAttributeEntry {
			public CustomAttributeEntry(MetadataDirectory directory, CustomAttributeMetadataTableRow row) {
				this.FileOffset = string.Format("0x{0:x}", row.FileOffset);
				this.Parent = row.Parent.ToString();
				this.Type = row.Type.ToString();
				this.Value = string.Format("0x{0:x}", row.Value);
			}

			public string FileOffset { get; set; }
			public string Parent { get; set; }
			public string Type { get; set; }
			public string Value { get; set; }
		}
	}
}
