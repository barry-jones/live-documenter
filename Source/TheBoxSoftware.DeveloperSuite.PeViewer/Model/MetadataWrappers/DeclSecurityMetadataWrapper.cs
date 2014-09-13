using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.DeveloperSuite.PEViewer.Model.MetadataWrappers {
	using TheBoxSoftware.Reflection.Core.COFF;
	using TheBoxSoftware.Reflection.Core;

	internal class DeclSecurityMetadataWrapper {
		public DeclSecurityMetadataWrapper(MetadataStream file, List<MetadataRow> methods) {
			this.Items = new List<DeclSecurityEntry>();
			foreach (DeclSecurityMetadataTableRow current in methods) {
				this.Items.Add(new DeclSecurityEntry(file.OwningFile.GetMetadataDirectory(), current));
			}
		}
		public List<DeclSecurityEntry> Items { get; set; }
		public class DeclSecurityEntry {
			public DeclSecurityEntry(MetadataDirectory directory, DeclSecurityMetadataTableRow row) {
				this.FileOffset = string.Format("0x{0:x}", row.FileOffset);
				this.Action = string.Format("0x:{0:x}", row.Action);
				this.Parent = row.Parent.ToString();
				this.PermissionSet = string.Format("0x{0:x}", row.PermissionSet);
			}

			public string FileOffset { get; set; }
			public string Action { get; set; }
			public string Parent { get; set; }
			public string PermissionSet { get; set; }
		}
	}
}
