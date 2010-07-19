using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection.Signitures {
	using TheBoxSoftware.Reflection.Core;

	[DebuggerDisplay("Sentinal")]
	public sealed class SentinalSignitureToken : SignitureToken {
		public SentinalSignitureToken(byte[] signiture, Offset offset)
			: base(SignitureTokens.Sentinal) {
			ElementTypes value = (ElementTypes)SignitureToken.GetCompressedValue(signiture, offset);
			offset.Shift(1);	// No work to do here we are jsut a placeholder
		}

		public static bool IsToken(byte[] signiture, int offset) {
			ElementTypes value = (ElementTypes)SignitureToken.GetCompressedValue(signiture, offset);
			return (value & ElementTypes.Sentinal) == ElementTypes.Sentinal;
		}
	}
}
