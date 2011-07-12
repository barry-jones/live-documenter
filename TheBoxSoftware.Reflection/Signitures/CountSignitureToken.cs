using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection.Signitures {
	using TheBoxSoftware.Reflection.Core;

	[DebuggerDisplay("Count: {Count}")]
	public sealed class CountSignitureToken : SignitureToken {
		public CountSignitureToken(byte[] signiture, Offset offset)
			: base(SignitureTokens.Count) {
			this.Count = FieldReader.ToUInt16(signiture, offset.Shift(2));
		}

		public UInt16 Count { get; set; }
	}
}
