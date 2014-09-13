using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.DeveloperSuite.PEViewer.Model.MetadataWrappers {
	using TheBoxSoftware.Reflection.Core.COFF;
	using TheBoxSoftware.Reflection.Core;

	internal class ConstantMetadataWrapper {
		public ConstantMetadataWrapper(MetadataStream file, List<MetadataRow> methods) {
			this.Items = new List<ConstantEntry>();
			foreach (ConstantMetadataTableRow current in methods) {
				this.Items.Add(new ConstantEntry(file.OwningFile.GetMetadataDirectory(), current));
			}
		}
		public List<ConstantEntry> Items { get; set; }
		public class ConstantEntry {
			public ConstantEntry(MetadataDirectory directory, ConstantMetadataTableRow row) {
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
