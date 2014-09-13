using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.DeveloperSuite.PEViewer.Model.MetadataWrappers {
	using TheBoxSoftware.Reflection.Core.COFF;
	using TheBoxSoftware.Reflection.Core;

	internal class GenericParamMetadataWrapper {
		public GenericParamMetadataWrapper(MetadataStream file, List<MetadataRow> methods) {
			this.Items = new List<GenricParamEntry>();
			foreach (GenericParamMetadataTableRow current in methods) {
				this.Items.Add(new GenricParamEntry(file.OwningFile.GetMetadataDirectory(), current));
			}
		}
		public List<GenricParamEntry> Items { get; set; }
		public class GenricParamEntry {
			public GenricParamEntry(MetadataDirectory directory, GenericParamMetadataTableRow row) {
				this.FileOffset = string.Format("0x{0:x}", row.FileOffset);
				this.Name = ((StringStream)directory.Streams[Streams.StringStream]).GetString(row.Name.Value);
				this.Flags = string.Format("0x{0:x}", row.Flags);
				this.Number = row.Number.ToString();
				this.Owner = row.Owner.ToString();
			}

			public string FileOffset { get; set; }
			public string Name { get; set; }
			public string Flags { get; set; }
			public string Number { get; set; }
			public string Owner { get; set; }
		}
	}
}
