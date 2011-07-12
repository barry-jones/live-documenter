using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection.Syntax.CSharp {
	public class CSharpConstantFormatter : CSharpFormatter, IConstantFormatter {
		private ConstantSyntax syntax;

		public CSharpConstantFormatter(ConstantSyntax syntax) {
			this.syntax = syntax;
		}

		public List<SyntaxToken> Format() {
			return this.Format(this.syntax);
		}

		public SyntaxToken GetType(ConstantSyntax syntax) {
			return this.FormatTypeName(syntax.GetType());
		}

		public List<SyntaxToken> GetVisibility(ConstantSyntax syntax) {
			return this.FormatVisibility(syntax.GetVisibility());
		}

		public List<SyntaxToken> Format(ConstantSyntax syntax) {
			List<SyntaxToken> tokens = new List<SyntaxToken>();

			tokens.AddRange(this.GetVisibility(syntax));
			tokens.Add(new SyntaxToken(" ", SyntaxTokens.Text));
			tokens.Add(new SyntaxToken("const", SyntaxTokens.Keyword));
			tokens.Add(new SyntaxToken(" ", SyntaxTokens.Text));
			tokens.Add(this.GetType(syntax));
			tokens.Add(new SyntaxToken(" ", SyntaxTokens.Text));
			tokens.Add(new SyntaxToken(syntax.GetIdentifier(), SyntaxTokens.Text));

			return tokens;
		}
	}
}
