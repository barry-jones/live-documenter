using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.DeveloperSuite.PEViewer.Model.MetadataWrappers {
	using TheBoxSoftware.Reflection.Core.COFF;
	using TheBoxSoftware.Reflection.Core;

	internal class MethodSpecMetadataWrapper {
		public MethodSpecMetadataWrapper(MetadataStream file, List<MetadataRow> methods) {
			this.Items = new List<MethodSpecEntry>();
			foreach (MethodSpecMetadataTableRow current in methods) {
				this.Items.Add(new MethodSpecEntry(file.OwningFile.GetMetadataDirectory(), current));
			}
		}
		public List<MethodSpecEntry> Items { get; set; }
		public class MethodSpecEntry {
			public MethodSpecEntry(MetadataDirectory directory, MethodSpecMetadataTableRow row) {
				this.FileOffset = string.Format("0x{0:x}", row.FileOffset);
				this.Instantiation = string.Format("0x{0:x}", row.Instantiation);
				this.Method = row.Method.ToString();
			}

			public string FileOffset { get; set; }
			public string Instantiation { get; set; }
			public string Method { get; set; }
		}
	}
}
