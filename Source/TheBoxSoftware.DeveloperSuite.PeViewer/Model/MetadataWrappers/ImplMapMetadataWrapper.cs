using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.DeveloperSuite.PEViewer.Model.MetadataWrappers {
	using TheBoxSoftware.Reflection.Core.COFF;
	using TheBoxSoftware.Reflection.Core;

	internal class ImplMapMetadataWrapper {
		public ImplMapMetadataWrapper(MetadataStream file, List<MetadataRow> methods) {
			this.Items = new List<ImplMapEntry>();
			foreach (ImplMapMetadataTableRow current in methods) {
				this.Items.Add(new ImplMapEntry(file.OwningFile.GetMetadataDirectory(), current));
			}
		}
		public List<ImplMapEntry> Items { get; set; }
		public class ImplMapEntry {
			public ImplMapEntry(MetadataDirectory directory, ImplMapMetadataTableRow row) {
				this.FileOffset = string.Format("0x{0:x}", row.FileOffset);
				this.ImportName = ((StringStream)directory.Streams[Streams.StringStream]).GetString(row.ImportName.Value);
				this.ImportScope = row.ImportScope.Value.ToString();
				this.MappingFlags = string.Format("0x:{0:x}", row.MappingFlags);
				this.MemberForward = row.MemberForward.ToString();
			}

			public string FileOffset { get; set; }
			public string ImportName { get; set; }
			public string ImportScope { get; set; }
			public string MappingFlags { get; set; }
			public string MemberForward { get; set; }
		}
	}
}
