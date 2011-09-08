using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection.Syntax.VisualBasic {
	public sealed class VBEventFormatter : VBFormatter, IEventFormatter {
		private EventSyntax syntax;

		public VBEventFormatter(EventSyntax syntax) {
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
			tokens.Add(new SyntaxToken("Event", SyntaxTokens.Keyword));
			tokens.Add(new SyntaxToken(" ", SyntaxTokens.Text));
			tokens.Add(new SyntaxToken(syntax.GetIdentifier(), SyntaxTokens.Text));
			tokens.Add(new SyntaxToken(" ", SyntaxTokens.Text));
			tokens.Add(new SyntaxToken("As", SyntaxTokens.Keyword));
			tokens.Add(new SyntaxToken(" ", SyntaxTokens.Text));
			tokens.AddRange(this.FormatType(syntax));

			return tokens;
		}
	}
}
