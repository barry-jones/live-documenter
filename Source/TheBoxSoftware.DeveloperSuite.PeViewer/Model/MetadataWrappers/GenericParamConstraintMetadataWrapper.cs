using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.DeveloperSuite.PEViewer.Model.MetadataWrappers {
	using TheBoxSoftware.Reflection.Core.COFF;
	using TheBoxSoftware.Reflection.Core;

	internal class GenericParamConstraintMetadataWrapper {
		public GenericParamConstraintMetadataWrapper(MetadataStream file, List<MetadataRow> methods) {
			this.Items = new List<GenericParamConstraintEntry>();
			foreach (GenericParamConstraintMetadataTableRow current in methods) {
				this.Items.Add(new GenericParamConstraintEntry(file.OwningFile.GetMetadataDirectory(), current));
			}
		}
		public List<GenericParamConstraintEntry> Items { get; set; }
		public class GenericParamConstraintEntry {
			public GenericParamConstraintEntry(MetadataDirectory directory, GenericParamConstraintMetadataTableRow row) {
				this.FileOffset = string.Format("0x{0:x}", row.FileOffset);
				this.Constraint = row.Constraint.ToString();
				this.Owner = row.Owner.Value.ToString();
			}

			public string FileOffset { get; set; }
			public string Constraint { get; set; }
			public string Owner { get; set; }
		}
	}
}
