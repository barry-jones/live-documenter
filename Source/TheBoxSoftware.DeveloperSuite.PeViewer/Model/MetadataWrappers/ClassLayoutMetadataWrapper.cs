using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.DeveloperSuite.PEViewer.Model.MetadataWrappers {
	using TheBoxSoftware.Reflection.Core.COFF;
	using TheBoxSoftware.Reflection.Core;

	internal class ClassLayoutMetadataWrapper {
		public ClassLayoutMetadataWrapper(MetadataStream file, List<MetadataRow> methods) {
			this.Items = new List<ClassLayoutEntry>();
			foreach (ClassLayoutMetadataTableRow current in methods) {
				this.Items.Add(new ClassLayoutEntry(file.OwningFile.GetMetadataDirectory(), current));
			}
		}
		public List<ClassLayoutEntry> Items { get; set; }
		public class ClassLayoutEntry {
			public ClassLayoutEntry(MetadataDirectory directory, ClassLayoutMetadataTableRow row) {
				this.FileOffset = string.Format("0x{0:x}", row.FileOffset);
				this.ClassSize = string.Format("0x{0:x}", row.ClassSize);
				this.PackingSize = string.Format("0x{0:x}", row.PackingSize);
				this.Parent = string.Format("0x{0:x}", row.Parent.Value);
			}

			public string FileOffset { get; set; }
			public string ClassSize { get; set; }
			public string PackingSize { get; set; }
			public string Parent { get; set; }
		}
	}
}
