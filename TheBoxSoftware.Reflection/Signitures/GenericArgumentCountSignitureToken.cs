using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection.Signitures {
	using TheBoxSoftware.Reflection.Core;

	[DebuggerDisplay("Generic Argument Count: {Count}")]
	public sealed class GenericArgumentCountSignitureToken : SignitureToken {
		public GenericArgumentCountSignitureToken(byte[] signiture, Offset offset)
			: base(SignitureTokens.GenericArgumentCount) {
			this.Count = SignitureToken.GetCompressedValue(signiture, offset);
		}

		public int Count { get; set; }
	}
}
