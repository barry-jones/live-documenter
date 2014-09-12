using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.DeveloperSuite.PEViewer.Model.MetadataWrappers {
	using TheBoxSoftware.Reflection.Core.COFF;
	using TheBoxSoftware.Reflection.Core;

	internal class EventMapMetadataWrapper {
		public EventMapMetadataWrapper(MetadataStream file, List<MetadataRow> methods) {
			this.Items = new List<EventMapEntry>();
			foreach (EventMapMetadataTableRow current in methods) {
				this.Items.Add(new EventMapEntry(file.OwningFile.GetMetadataDirectory(), current));
			}
		}
		public List<EventMapEntry> Items { get; set; }
		public class EventMapEntry {
			public EventMapEntry(MetadataDirectory directory, EventMapMetadataTableRow row) {
				this.FileOffset = string.Format("0x{0:x}", row.FileOffset);
				this.Parent = row.Parent.Value.ToString();
				this.EventList = row.EventList.Value.ToString();				
			}

			public string FileOffset { get; set; }
			public string Parent { get; set; }
			public string EventList { get; set; }
		}
	}
}
