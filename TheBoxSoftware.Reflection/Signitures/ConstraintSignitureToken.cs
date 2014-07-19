using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection.Signitures {
	using TheBoxSoftware.Reflection.Core;

	[DebuggerDisplay("Constraint: {Constraint}")]
	internal sealed class ConstraintSignitureToken : SignitureToken {
		public ConstraintSignitureToken(byte[] signiture, Offset offset)
			: base(SignitureTokens.Constraint) {
			this.Constraint = (ElementTypes)SignitureToken.GetCompressedValue(signiture, offset);
		}

		public static bool IsToken(byte[] signiture, int offset) {
			ElementTypes type = (ElementTypes)SignitureToken.GetCompressedValue(signiture, offset);
			return (type & ElementTypes.Pinned) == ElementTypes.Pinned;
		}

		public ElementTypes Constraint { get; set; }
	}
}
