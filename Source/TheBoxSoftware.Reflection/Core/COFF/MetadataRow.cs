using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection.Core.COFF {
	public abstract class MetadataRow {
		internal int SizeOfRow { get; set; }
		public int FileOffset { get; set; }
	}
}
