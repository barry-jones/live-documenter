using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.DeveloperSuite.PEViewer.Model.MetadataWrappers {
	using TheBoxSoftware.Reflection.Core.COFF;
	using TheBoxSoftware.Reflection.Core;

	internal class FileMetadataWrapper {
		public FileMetadataWrapper(MetadataStream file, List<MetadataRow> methods) {
			this.Items = new List<FileEntry>();
			foreach (FileMetadataTableRow current in methods) {
				this.Items.Add(new FileEntry(file.OwningFile.GetMetadataDirectory(), current));
			}
		}
		public List<FileEntry> Items { get; set; }
		public class FileEntry {
			public FileEntry(MetadataDirectory directory, FileMetadataTableRow row) {
				this.FileOffset = string.Format("0x{0:x}", row.FileOffset);
				this.Name = ((StringStream)directory.Streams[Streams.StringStream]).GetString(row.Name.Value);
				this.Flags = string.Format("0x{0:x}", row.Flags);
				this.HashValue = string.Format("0x{0:x}", row.HashValue);
			}

			public string FileOffset { get; set; }
			public string Name { get; set; }
			public string Flags { get; set; }
			public string HashValue { get; set; }
		}
	}
}
