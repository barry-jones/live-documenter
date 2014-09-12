using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection.Syntax.CSharp {
	internal sealed class CSharpFieldFormatter : CSharpFormatter, IFieldFormatter {
		private FieldSyntax syntax;

		public CSharpFieldFormatter(FieldSyntax syntax) {
			this.syntax = syntax;
		}

		public SyntaxTokenCollection Format() {
			return this.Format(this.syntax);
		}

		public SyntaxToken GetType(FieldSyntax syntax) {
			return this.FormatTypeName(syntax.GetType());
		}

		public List<SyntaxToken> GetVisibility(FieldSyntax syntax) {
			return this.FormatVisibility(syntax.GetVisibility());
		}

		public SyntaxTokenCollection Format(FieldSyntax syntax) {
			SyntaxTokenCollection tokens = new SyntaxTokenCollection();

			tokens.AddRange(this.GetVisibility(syntax));
			tokens.Add(new SyntaxToken(" ", SyntaxTokens.Text));
			tokens.Add(this.GetType(syntax));
			tokens.Add(new SyntaxToken(" ", SyntaxTokens.Text));
			tokens.Add(new SyntaxToken(syntax.GetIdentifier(), SyntaxTokens.Text));

			return tokens;
		}
	}
}
