using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.DeveloperSuite.PEViewer.Model.MetadataWrappers {
	using TheBoxSoftware.Reflection.Core.COFF;
	using TheBoxSoftware.Reflection.Core;

	internal class FieldMarshalMetadataWrapper {
		public FieldMarshalMetadataWrapper(MetadataStream file, List<MetadataRow> methods) {
			this.Items = new List<FieldMarshalEntry>();
			foreach (FieldMarshalMetadataTableRow current in methods) {
				this.Items.Add(new FieldMarshalEntry(file.OwningFile.GetMetadataDirectory(), current));
			}
		}
		public List<FieldMarshalEntry> Items { get; set; }
		public class FieldMarshalEntry {
			public FieldMarshalEntry(MetadataDirectory directory, FieldMarshalMetadataTableRow row) {
				this.FileOffset = string.Format("0x{0:x}", row.FileOffset);
				this.NativeType = string.Format("0x{0:x}", row.NativeType);
				this.Parent = row.Parent.ToString();
			}

			public string FileOffset { get; set; }
			public string NativeType { get; set; }
			public string Parent { get; set; }
		}
	}
}
