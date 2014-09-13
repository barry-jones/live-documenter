using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.DeveloperSuite.PEViewer.Model.MetadataWrappers {
	using TheBoxSoftware.Reflection.Core.COFF;
	using TheBoxSoftware.Reflection.Core;

	internal class EventMetadataWrapper {
		public EventMetadataWrapper(MetadataStream file, List<MetadataRow> methods) {
			this.Items = new List<EventEntry>();
			foreach (EventMetadataTableRow current in methods) {
				this.Items.Add(new EventEntry(file.OwningFile.GetMetadataDirectory(), current));
			}
		}
		public List<EventEntry> Items { get; set; }
		public class EventEntry {
			public EventEntry(MetadataDirectory directory, EventMetadataTableRow row) {
				this.FileOffset = string.Format("0x{0:x}", row.FileOffset);
				this.Name = ((StringStream)directory.Streams[Streams.StringStream]).GetString(row.Name.Value);
				this.EventFlags = string.Format("0x{0:x}", row.EventFlags);
				this.EventType = row.EventType.ToString();
			}

			public string FileOffset { get; set; }
			public string Name { get; set; }
			public string EventType { get; set; }
			public string EventFlags { get; set; }
		}
	}
}
