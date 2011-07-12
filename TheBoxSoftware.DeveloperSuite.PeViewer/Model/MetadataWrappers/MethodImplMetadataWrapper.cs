using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.DeveloperSuite.PEViewer.Model.MetadataWrappers {
	using TheBoxSoftware.Reflection.Core.COFF;
	using TheBoxSoftware.Reflection.Core;

	internal class MethodImplMetadataWrapper {
		public MethodImplMetadataWrapper(MetadataStream file, List<MetadataRow> methods) {
			this.Items = new List<MethodImplEntry>();
			foreach (MethodImplMetadataTableRow current in methods) {
				this.Items.Add(new MethodImplEntry(file.OwningFile.GetMetadataDirectory(), current));
			}
		}
		public List<MethodImplEntry> Items { get; set; }
		public class MethodImplEntry {
			public MethodImplEntry(MetadataDirectory directory, MethodImplMetadataTableRow row) {
				this.FileOffset = string.Format("0x{0:x}", row.FileOffset);
				this.Class = row.Class.Value.ToString();
				this.MethodBody = row.MethodBody.ToString();
				this.MethodDecleration = row.MethodDeclaration.ToString();
			}

			public string FileOffset { get; set; }
			public string Class { get; set; }
			public string MethodBody { get; set; }
			public string MethodDecleration { get; set; }
		}
	}
}
