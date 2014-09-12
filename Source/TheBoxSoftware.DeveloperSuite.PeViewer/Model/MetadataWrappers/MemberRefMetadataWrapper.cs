using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.DeveloperSuite.PEViewer.Model.MetadataWrappers {
	using TheBoxSoftware.Reflection.Core.COFF;
	using TheBoxSoftware.Reflection.Core;

	internal class MemberRefMetadataWrapper {
		public MemberRefMetadataWrapper(MetadataStream file, List<MetadataRow> methods) {
			this.Items = new List<MemberRefEntry>();
			foreach (MemberRefMetadataTableRow current in methods) {
				this.Items.Add(new MemberRefEntry(file.OwningFile.GetMetadataDirectory(), current));
			}
		}
		public List<MemberRefEntry> Items { get; set; }
		public class MemberRefEntry {
			public MemberRefEntry(MetadataDirectory directory, MemberRefMetadataTableRow row) {
				this.FileOffset = string.Format("0x{0:x}", row.FileOffset);
				this.Name = ((StringStream)directory.Streams[Streams.StringStream]).GetString(row.Name.Value);
				this.Class = row.Class.ToString();
				this.Signiture = string.Format("0x{0:x}", row.Signiture.Value);
			}

			public string FileOffset { get; set; }
			public string Name { get; set; }
			public string Class { get; set; }
			public string Signiture { get; set; }
		}
	}
}
