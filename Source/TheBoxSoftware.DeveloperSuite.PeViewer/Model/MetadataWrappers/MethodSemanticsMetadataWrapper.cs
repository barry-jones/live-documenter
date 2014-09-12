using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.DeveloperSuite.PEViewer.Model.MetadataWrappers {
	using TheBoxSoftware.Reflection.Core.COFF;
	using TheBoxSoftware.Reflection.Core;

	internal class MethodSemanticsMetadataWrapper {
		public MethodSemanticsMetadataWrapper(MetadataStream file, List<MetadataRow> methods) {
			this.Items = new List<MethodSemanticsEntry>();
			foreach (MethodSemanticsMetadataTableRow current in methods) {
				this.Items.Add(new MethodSemanticsEntry(file.OwningFile.GetMetadataDirectory(), current));
			}
		}
		public List<MethodSemanticsEntry> Items { get; set; }
		public class MethodSemanticsEntry {
			public MethodSemanticsEntry(MetadataDirectory directory, MethodSemanticsMetadataTableRow row) {
				this.FileOffset = string.Format("0x{0:x}", row.FileOffset);
				this.Association = row.Association.ToString();
				this.Method = row.Method.Value.ToString();
				this.Semantics = string.Format("0x{0:x}", row.Semantics);
			}

			public string FileOffset { get; set; }
			public string Association { get; set; }
			public string Method { get; set; }
			public string Semantics { get; set; }
		}
	}
}
