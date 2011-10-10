using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection.Syntax.VisualBasic {
	public sealed class VBEnumerationFormatter : VBFormatter, IEnumerationFormatter {
		private EnumSyntax syntax;

		public VBEnumerationFormatter(EnumSyntax syntax) {
			this.syntax = syntax;
		}

		public SyntaxTokenCollection Format() {
			return this.Format(this.syntax);
		}

		public List<SyntaxToken> FormatVisibility(EnumSyntax syntax) {
			return this.FormatVisibility(syntax.GetVisibility());
		}

		public SyntaxTokenCollection Format(EnumSyntax syntax) {
			SyntaxTokenCollection tokens = new SyntaxTokenCollection();

			tokens.AddRange(this.FormatVisibility(syntax));
			tokens.Add(new SyntaxToken(" ", SyntaxTokens.Text));
			tokens.Add(new SyntaxToken("Enum", SyntaxTokens.Keyword));
			tokens.Add(new SyntaxToken(" ", SyntaxTokens.Text));
			tokens.Add(new SyntaxToken(syntax.GetIdentifier(), SyntaxTokens.Text));
			tokens.Add(new SyntaxToken(" ", SyntaxTokens.Text));
			tokens.Add(new SyntaxToken("As", SyntaxTokens.Keyword));
			tokens.Add(new SyntaxToken(" ", SyntaxTokens.Text));
			tokens.Add(this.FormatTypeName(syntax.GetUnderlyingType()));

			return tokens;
		}
	}
}
