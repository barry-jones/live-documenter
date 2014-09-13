using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.DeveloperSuite.PEViewer.Model.MetadataWrappers {
	using TheBoxSoftware.Reflection.Core.COFF;
	using TheBoxSoftware.Reflection.Core;
	internal class PropertyMapMetadataWrapper {
		public PropertyMapMetadataWrapper(MetadataStream file, List<MetadataRow> methods) {
			this.Items = new List<PropertyMapEntry>();
			foreach (PropertyMapMetadataTableRow current in methods) {
				this.Items.Add(new PropertyMapEntry(file.OwningFile.GetMetadataDirectory(), current));
			}
		}
		public List<PropertyMapEntry> Items { get; set; }
		public class PropertyMapEntry {
			public PropertyMapEntry(MetadataDirectory directory, PropertyMapMetadataTableRow row) {
				this.FileOffset = string.Format("0x{0:x}", row.FileOffset);
				this.Parent = row.Parent.Value.ToString();
				this.PropertyList = row.PropertyList.Value.ToString();
			}

			public string FileOffset { get; set; }
			public string Parent { get; set; }
			public string PropertyList { get; set; }
		}
	}
}
