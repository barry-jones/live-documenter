using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection.Signitures {
	using TheBoxSoftware.Reflection.Core;

	[DebuggerDisplay("Calling Convention: {Convention}")]
	internal sealed class CallingConventionSignitureToken : SignitureToken {
		public CallingConventionSignitureToken(int convention)
			: this((CallingConventions)convention) {
		}

		public CallingConventionSignitureToken(CallingConventions convention)
			: base(SignitureTokens.CallingConvention) {
			this.Convention = convention;
		}

		public CallingConventionSignitureToken(byte[] signiture, Offset offset)
			: base(SignitureTokens.CallingConvention) {
			this.Convention = (CallingConventions)signiture[offset.Shift(1)];
		}

		/// <summary>
		/// Describes the convention used in this token.
		/// </summary>
		public CallingConventions Convention { get; private set; }
	}
}
