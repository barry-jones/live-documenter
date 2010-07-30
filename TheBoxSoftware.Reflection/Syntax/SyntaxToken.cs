using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection.Syntax {
	/// <summary>
	/// <para>A SyntaxToken is a string that represents an element of a syntax block. A 
	/// SyntaxToken is a string and a <see cref="SyntaxTokens"/> type.</para>
	/// </summary>
	/// <remarks>
	/// <para>A <see cref="IFormatter"/> will return a list of syntax tokens. Here is an
	/// example of how to use SyntaxTokens:
	/// </para>
	/// <code>
	/// List&lt;SyntaxToken&gt; tokens = new List&lt;SyntaxToken&gt;();
	/// 
	/// tokens.AddRange(this.GetVisibility(syntax));
	/// tokens.Add(new SyntaxToken(" ", SyntaxTokens.Text));
	/// tokens.Add(new SyntaxToken("const", SyntaxTokens.Keyword));
	/// tokens.Add(new SyntaxToken(" ", SyntaxTokens.Text));
	/// tokens.Add(this.GetType(syntax));
	/// tokens.Add(new SyntaxToken(" ", SyntaxTokens.Text));
	/// tokens.Add(new SyntaxToken(syntax.GetIdentifier(), SyntaxTokens.Text));
	/// 
	/// return tokens;
	/// </code>
	/// </remarks>
	/// <seealso cref="SyntaxTokens"/>
	public class SyntaxToken {
		private SyntaxTokens tokenType;

		/// <summary>
		/// Initialises a new instance of the SyntaxToken class.
		/// </summary>
		/// <param name="content">The tokens content</param>
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
