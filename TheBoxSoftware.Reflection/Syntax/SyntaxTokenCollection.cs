using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection.Syntax {
	/// <summary>
	/// Manages a collection of <see cref="SyntaxToken"/>s.
	/// </summary>
	public class SyntaxTokenCollection : List<SyntaxToken> {
		/// <summary>
		/// Returns a unformatted string representation of the syntax represented
		/// by the collection of SyntaxTokens.
		/// </summary>
		/// <returns>A string representing the syntax</returns>
		public override string ToString() {
			StringBuilder syntax = new StringBuilder();

			foreach (SyntaxToken token in this) {
				syntax.Append(token.Content);
			}

			return syntax.ToString();
		}
	}
}
