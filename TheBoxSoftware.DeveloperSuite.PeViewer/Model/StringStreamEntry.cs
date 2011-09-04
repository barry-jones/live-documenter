using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.DeveloperSuite.PEViewer.Model {
	using TheBoxSoftware.Reflection.Core.COFF;

	internal class StringStreamEntry : Entry {
		public StringStreamEntry(StringStream stream) 
			: base(stream.Name) {
			this.Data = SimpleIndexEntry.Create<int, string>(stream.GetAllStrings());
		}
	}
}
