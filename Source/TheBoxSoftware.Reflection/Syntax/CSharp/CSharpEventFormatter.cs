using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection.Syntax.CSharp {
	internal sealed class CSharpEventFormatter : CSharpFormatter, IEventFormatter {
		private EventSyntax syntax;

		public CSharpEventFormatter(EventSyntax syntax) {
			this.syntax = syntax;
		}

		public SyntaxTokenCollection Format() {
			return this.Format(this.syntax);
		}

		public SyntaxToken FormatIdentifier(EventSyntax syntax) {
			return new SyntaxToken(syntax.GetIdentifier(), SyntaxTokens.Text);
		}

		public List<SyntaxToken> FormatType(EventSyntax syntax) {
			return this.FormatTypeDetails(syntax.GetType());
		}

		public List<SyntaxToken> FormatVisibility(EventSyntax syntax) {
			return this.FormatVisibility(syntax.GetVisbility());
		}

		public SyntaxToken FormatInheritance(EventSyntax syntax) {
			return this.FormatInheritance(syntax.GetInheritance());
		}

		public SyntaxTokenCollection Format(EventSyntax syntax) {
			SyntaxTokenCollection tokens = new SyntaxTokenCollection();

			SyntaxToken inheritanceModifier = this.FormatInheritance(syntax);

			tokens.AddRange(this.FormatVisibility(syntax));
			if (inheritanceModifier != null) {
				tokens.Add(new SyntaxToken(" ", SyntaxTokens.Text));
				tokens.Add(inheritanceModifier);
			}
			tokens.Add(new SyntaxToken(" ", SyntaxTokens.Text));
			tokens.Add(new SyntaxToken("event", SyntaxTokens.Keyword));
			tokens.Add(new SyntaxToken(" ", SyntaxTokens.Text));
			tokens.AddRange(this.FormatType(syntax));
			tokens.Add(new SyntaxToken(" ", SyntaxTokens.Text));
			tokens.Add(new SyntaxToken(syntax.GetIdentifier(), SyntaxTokens.Text));
			tokens.Add(new SyntaxToken(" {\n\t", SyntaxTokens.Text));
			tokens.Add(new SyntaxToken("add", SyntaxTokens.Keyword));
			tokens.Add(new SyntaxToken(";\n\t", SyntaxTokens.Text));
			tokens.Add(new SyntaxToken("remove", SyntaxTokens.Keyword));
			tokens.Add(new SyntaxToken(";\n\t", SyntaxTokens.Text));
			tokens.Add(new SyntaxToken("}", SyntaxTokens.Text));

			return tokens;
		}
	}
}
