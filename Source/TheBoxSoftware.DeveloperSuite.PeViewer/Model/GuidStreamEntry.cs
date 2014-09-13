using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.DeveloperSuite.PEViewer.Model {
	using TheBoxSoftware.Reflection.Core.COFF;
	internal class GuidStreamEntry : Entry {
		public GuidStreamEntry(GuidStream stream)
			: base(stream.Name) {
			this.Data = SimpleIndexEntry.Create<int, Guid>(stream.GetAllGUIDs());
		}
	}
}