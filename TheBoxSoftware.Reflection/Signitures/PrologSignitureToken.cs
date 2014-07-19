using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection.Signitures {
	using TheBoxSoftware.Reflection.Core;

	/// <summary>
	/// A class which represents a prolog for a <see cref="CustomAttributeSigniture"/>.
	/// </summary>
	internal sealed class PrologSignitureToken : SignitureToken {
		/// <summary>
		/// Initialises a new instance of the PrologSignitureToken class.
		/// </summary>
		/// <param name="signiture">The byte contents of the signiture.</param>
		/// <param name="offset">The start offset of the this token.</param>
		public PrologSignitureToken(byte[] signiture, Offset offset)
			: base(SignitureTokens.Prolog) {
			this.Value = FieldReader.ToUInt16(signiture, offset.Shift(2));
		}

		/// <summary>
		/// The value of the token.
		/// </summary>
		public UInt16 Value { get; set; }
	}
}
