using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection.Syntax {
	/// <summary>
	/// Base abstract class for all SyntaxToken types. A SyntaxToken is a
	/// parsed syntax element that describes details about the token it
	/// contains. This information can be used by display providers to
	/// format the different types appropriately.
	/// </summary>
	public class SyntaxToken {
		private SyntaxTokens tokenType;

		/// <summary>
		/// Initialises a new instance of the SyntaxToken class.
		/// </summary>
		/// <param name="tokenType">The type of token.</param>
		public SyntaxToken(string content, SyntaxTokens tokenType) {
			this.Content = content;
			this.tokenType = tokenType;
		}

		/// <summary>
		/// Represents the type of token this SyntaxToken represents.
		/// </summary>
		public SyntaxTokens TokenType {
			get { return this.tokenType; }
		}

		/// <summary>
		/// A string representing the actual displayable content for the
		/// token.
		/// </summary>
		public string Content { get; set; }
	}
}
