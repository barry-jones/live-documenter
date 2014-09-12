using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection.Syntax.CSharp {
	internal sealed class CSharpPropertyFormatter : CSharpFormatter, IPropertyFormatter {
		private PropertySyntax syntax;

		public CSharpPropertyFormatter(PropertySyntax syntax) {
			this.syntax = syntax;
		}

		public SyntaxTokenCollection Format() {
			return this.Format(this.syntax);
		}

		public SyntaxToken FormatIdentifier(PropertySyntax syntax) {
			return new SyntaxToken(syntax.GetIdentifier(), SyntaxTokens.Text);
		}

		public List<SyntaxToken> FormatType(PropertySyntax syntax) {
			return this.FormatTypeDetails(syntax.GetType());
		}

		public List<SyntaxToken> FormatVisibility(PropertySyntax syntax) {
			return this.FormatVisibility(syntax.GetVisibility());
		}

		public SyntaxToken FormatInheritance(PropertySyntax syntax) {
			return this.FormatInheritance(syntax.GetInheritance());
		}

		public List<SyntaxToken> FormatGetVisibility(PropertySyntax syntax) {
			return this.FormatVisibility(syntax.GetGetterVisibility());
		}

		public List<SyntaxToken> FormatSetVisibility(PropertySyntax syntax) {
			return this.FormatVisibility(syntax.GetSetterVisibility());
		}

		public SyntaxTokenCollection Format(PropertySyntax syntax) {
			SyntaxTokenCollection tokens = new SyntaxTokenCollection();
			tokens.AddRange(this.FormatVisibility(syntax));
			tokens.Add(new SyntaxToken(" ", SyntaxTokens.Text));
			tokens.AddRange(this.FormatType(syntax));
			tokens.Add(new SyntaxToken(" ", SyntaxTokens.Text));
			tokens.Add(this.FormatIdentifier(syntax));
			tokens.Add(new SyntaxToken(" {", SyntaxTokens.Text));
			if (this.syntax.GetMethod != null) {
				tokens.Add(new SyntaxToken("\n\t", SyntaxTokens.Text));
				if (syntax.GetVisibility() != syntax.GetGetterVisibility()) {
					tokens.AddRange(this.FormatGetVisibility(syntax));
					tokens.Add(new SyntaxToken(" ", SyntaxTokens.Text));
				}
				tokens.Add(new SyntaxToken("get", SyntaxTokens.Keyword));
				tokens.Add(new SyntaxToken(";", SyntaxTokens.Text));
			}
			if (this.syntax.SetMethod != null) {
				tokens.Add(new SyntaxToken("\n\t", SyntaxTokens.Text));
				if (syntax.GetVisibility() != syntax.GetSetterVisibility()) {
					tokens.AddRange(this.FormatSetVisibility(syntax));
					tokens.Add(new SyntaxToken(" ", SyntaxTokens.Text));
				}
				tokens.Add(new SyntaxToken("set", SyntaxTokens.Keyword));
				tokens.Add(new SyntaxToken(";", SyntaxTokens.Text));
			}
			tokens.Add(new SyntaxToken("\n\t}", SyntaxTokens.Text));
			return tokens;
		}
	}
}
