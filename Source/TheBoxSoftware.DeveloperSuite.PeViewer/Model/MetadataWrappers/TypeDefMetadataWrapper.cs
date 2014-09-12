using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.DeveloperSuite.PEViewer.Model.MetadataWrappers {
	using TheBoxSoftware.Reflection.Core;
	using TheBoxSoftware.Reflection.Core.COFF;

	class TypeDefMetadataWrapper {
		/// <summary>
		/// Initialises a new instance of the MethodDefMetadataWrapper.
		/// </summary>
		/// <param name="file">The file the metadata was loaded from</param>
		/// <param name="types">The methods to wrap.</param>
		public TypeDefMetadataWrapper(MetadataStream file, List<MetadataRow> types) {
			this.Items = new List<TypeDefEntry>();
			foreach (TypeDefMetadataTableRow current in types) {
				this.Items.Add(new TypeDefEntry(file.OwningFile.GetMetadataDirectory(), current));
			}
		}

		/// <summary>
		/// The converted item list
		/// </summary>
		public List<TypeDefEntry> Items { get; set; }

		public class TypeDefEntry {
			public TypeDefEntry(MetadataDirectory directory, TypeDefMetadataTableRow row) {
				this.Extends = row.Extends.ToString();
				this.FieldsList = row.FieldList.ToString();
				this.FileOffset = string.Format("0x{0:x}", row.FileOffset);
				this.Flags = string.Format("0x{0:x}", row.Flags);
				this.MethodList = row.MethodList.ToString();
				this.Name = ((StringStream)directory.Streams[Streams.StringStream]).GetString(row.Name.Value);
				this.Namespace = ((StringStream)directory.Streams[Streams.StringStream]).GetString(row.Namespace.Value);
			}

			public string FileOffset { get; set; }
			public string Name { get; set; }
			public string Namespace { get; set; }
			public string Flags { get; set; }
			public string Extends { get; set; }
			public string FieldsList { get; set; }
			public string MethodList { get; set; }
		}
	}
}