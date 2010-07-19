using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection.Signitures {
	using TheBoxSoftware.Reflection.Core;

	[DebuggerDisplay("Parameter Count: {Count}")]
	public sealed class ParameterCountSignitureToken : SignitureToken {
		public ParameterCountSignitureToken(byte[] signiture, Offset offset)
			: base(SignitureTokens.ParameterCount) {
			this.Count = SignitureToken.GetCompressedValue(signiture, offset);
		}

		public int Count { get; set; }
	}
}
