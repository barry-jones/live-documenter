using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.DeveloperSuite.PEViewer.Model {
	using TheBoxSoftware.Reflection.Core.COFF;

	internal class CLRDirectoryEntry : Entry {
		public CLRDirectoryEntry(CLRDirectory directory)
			: base(directory.Name) {
			foreach (KeyValuePair<Streams, Stream> current in directory.Metadata.Streams) {
				this.Children.Add(Entry.Create(current.Value));
			}
		}
	}
}
