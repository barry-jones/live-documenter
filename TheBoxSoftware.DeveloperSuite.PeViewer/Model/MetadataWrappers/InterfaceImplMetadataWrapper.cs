using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.DeveloperSuite.PEViewer.Model.MetadataWrappers {
	using TheBoxSoftware.Reflection.Core.COFF;
	using TheBoxSoftware.Reflection.Core;

	internal class InterfaceImplMetadataWrapper {
		public InterfaceImplMetadataWrapper(MetadataStream file, List<MetadataRow> methods) {
			this.Items = new List<InerfaceImplEntry>();
			foreach (InterfaceImplMetadataTableRow current in methods) {
				this.Items.Add(new InerfaceImplEntry(file.OwningFile.GetMetadataDirectory(), current));
			}
		}
		public List<InerfaceImplEntry> Items { get; set; }
		public class InerfaceImplEntry {
			public InerfaceImplEntry(MetadataDirectory directory, InterfaceImplMetadataTableRow row) {
				this.FileOffset = string.Format("0x{0:x}", row.FileOffset);
				this.Class = row.Class.ToString();
				this.Interface = row.Interface.ToString();
			}

			public string FileOffset { get; set; }
			public string Class { get; set; }
			public string Interface { get; set; }
		}
	}
}
