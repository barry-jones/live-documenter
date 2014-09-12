using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.DeveloperSuite.PEViewer.Model.MetadataWrappers {
	using TheBoxSoftware.Reflection.Core.COFF;
	using TheBoxSoftware.Reflection.Core;

	internal class NestedClassMetadataWrapper {
		public NestedClassMetadataWrapper(MetadataStream file, List<MetadataRow> methods) {
			this.Items = new List<NestedClassEntry>();
			foreach (NestedClassMetadataTableRow current in methods) {
				this.Items.Add(new NestedClassEntry(file.OwningFile.GetMetadataDirectory(), current));
			}
		}
		public List<NestedClassEntry> Items { get; set; }
		public class NestedClassEntry {
			public NestedClassEntry(MetadataDirectory directory, NestedClassMetadataTableRow row) {
				this.FileOffset = string.Format("0x{0:x}", row.FileOffset);
				this.EnclosingClass = row.EnclosingClass.ToString();
				this.NestedClass = row.NestedClass.ToString();
			}

			public string FileOffset { get; set; }
			public string EnclosingClass { get; set; }
			public string NestedClass { get; set; }
		}
	}
}
