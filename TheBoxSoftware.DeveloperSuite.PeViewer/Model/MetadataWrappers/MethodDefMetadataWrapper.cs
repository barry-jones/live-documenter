using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.DeveloperSuite.PEViewer.Model.MetadataWrappers {
	using TheBoxSoftware.Reflection.Core.COFF;
	using TheBoxSoftware.Reflection.Core;

	/// <summary>
	/// Visual model for method def entries in the metadata.
	/// </summary>
	public class MethodDefMetadataWrapper {
		/// <summary>
		/// Initialises a new instance of the MethodDefMetadataWrapper.
		/// </summary>
		/// <param name="file">The file the metadata was loaded from</param>
		/// <param name="methods">The methods to wrap.</param>
		public MethodDefMetadataWrapper(MetadataStream file, List<MetadataRow> methods) {
			this.Items = new List<MethodDefEntry>();
			foreach (MethodMetadataTableRow current in methods) {
				this.Items.Add(new MethodDefEntry(file.OwningFile.GetMetadataDirectory(), current));
			}
		}

		/// <summary>
		/// The converted item list
		/// </summary>
		public List<MethodDefEntry> Items { get; set; }

		/// <summary>
		/// Internal class for controlling the formatting and resolution of properties in the
		/// individual <see cref="MethodMetadataTableRow"/> entries.
		/// </summary>
		public class MethodDefEntry {
			public MethodDefEntry(MetadataDirectory directory, MethodMetadataTableRow row) {
				this.FileOffset = string.Format("0x{0:x}", row.FileOffset);
				this.Flags = string.Format("0x{0:x}", row.Flags);
				this.ImplFlags = string.Format("0x{0:x}", row.ImplFlags);
				this.Name = ((StringStream)directory.Streams[Streams.StringStream]).GetString(row.Name.Value);
				this.ParamList = string.Format("0x{0:x}", row.ParamList.Value);
				this.RVA = string.Format("0x{0:x}", row.RVA);
				this.Signiture = string.Format("0x{0:x}", row.Signiture.Value);
			}

			public string FileOffset { get; set; }
			public string Flags { get; set; }
			public string ImplFlags { get; set; }
			public string Name { get; set; }
			public string ParamList { get; set; }
			public string RVA { get; set; }
			public string Signiture { get; set; }
		}
	}
}
