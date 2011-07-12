using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection.Core.COFF {
	[Flags]
	public enum HeapOffsetSizes : byte {
		StringIsLarge	= 0x01,
		GuidIsLarge		= 0x02,
		BlobIsLarge		= 0x04
	}
}
