using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.DeveloperSuite.PEViewer.Model.MetadataWrappers {
	using TheBoxSoftware.Reflection.Core.COFF;
	using TheBoxSoftware.Reflection.Core;

	internal class StandAloneSigMetadataWrapper {
		public StandAloneSigMetadataWrapper(MetadataStream file, List<MetadataRow> methods) {
			this.Items = new List<StandAloneSigEntry>();
			foreach (StandAloneSigMetadataTableRow current in methods) {
				this.Items.Add(new StandAloneSigEntry(file.OwningFile.GetMetadataDirectory(), current));
			}
		}
		public List<StandAloneSigEntry> Items { get; set; }
		public class StandAloneSigEntry {
			public StandAloneSigEntry(MetadataDirectory directory, StandAloneSigMetadataTableRow row) {
				this.FileOffset = string.Format("0x{0:x}", row.FileOffset);
				this.Signiture = string.Format("0x{0:x}", row.Signiture.Value);
			}

			public string FileOffset { get; set; }
			public string Signiture { get; set; }
		}
	}
}
