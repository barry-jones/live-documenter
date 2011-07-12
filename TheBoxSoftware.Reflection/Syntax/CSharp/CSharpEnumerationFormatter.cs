using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection.Syntax.CSharp {
	public class CSharpEnumerationFormatter : CSharpFormatter, IEnumerationFormatter {
		private EnumSyntax syntax;

		public CSharpEnumerationFormatter(EnumSyntax syntax) {
			this.syntax = syntax;
		}

		public List<SyntaxToken> Format() {
			return this.Format(this.syntax);
		}

		public List<SyntaxToken> FormatVisibility(EnumSyntax syntax) {
			return this.FormatVisibility(syntax.GetVisibility());
		}

		public SyntaxToken FormatUnderlyingType(EnumSyntax syntax) {
			throw new NotImplementedException();
		}

		public List<SyntaxToken> Format(EnumSyntax syntax) {
			List<SyntaxToken> tokens = new List<SyntaxToken>();

			tokens.AddRange(this.FormatVisibility(syntax));
			tokens.Add(new SyntaxToken(" ", SyntaxTokens.Text));
			tokens.Add(new SyntaxToken("enum", SyntaxTokens.Keyword));
			tokens.Add(new SyntaxToken(" ", SyntaxTokens.Text));
			tokens.Add(new SyntaxToken(syntax.GetIdentifier(), SyntaxTokens.Text));
			// TODO: Implement underlyign type
			// tokens.Add(new SyntaxToken(" ", SyntaxTokens.Text));
			// tokens.AddRange(this.FormatClassBase(syntax));

			return tokens;
		}
	}
}
