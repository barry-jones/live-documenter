using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection.Core.COFF {
	public class EventMapMetadataTableRow : MetadataRow {
		public EventMapMetadataTableRow() {
		}

		/// <summary>
		/// Initialises a new EventMapMetadataTableRow
		/// </summary>
		/// <param name="contents">The contents of the file</param>
		/// <param name="offset">The offset of the current row</param>
		public EventMapMetadataTableRow(MetadataStream stream, byte[] contents, Offset offset) {
			this.FileOffset = offset;
			this.Parent = new Index(stream, contents, offset, MetadataTables.TypeDef);
			this.EventList = new Index(stream, contents, offset, MetadataTables.Event);
		}

		#region Properties
		/// <summary>
		/// An index into the TypeDef table
		/// </summary>
		public Index Parent { get; set; }

		/// <summary>
		/// An index in to the Event table. Marking the first of a contiguos list
		/// of Events.
		/// </summary>
		public Index EventList { get; set; }
		#endregion
	}
}
