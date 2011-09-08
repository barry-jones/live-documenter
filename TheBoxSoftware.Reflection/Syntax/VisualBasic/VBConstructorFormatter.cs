using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection.Syntax.VisualBasic {
	using TheBoxSoftware.Reflection.Signitures;

	public sealed class VBConstructorFormatter : VBFormatter, IConstructorFormatter {
		private ConstructorSyntax syntax;
		private Signiture signiture;

		public VBConstructorFormatter(ConstructorSyntax syntax) {
			this.syntax = syntax;
			this.signiture = syntax.Method.Signiture;
		}

		public SyntaxTokenCollection Format() {
			return this.Format(this.syntax);
		}

		public List<SyntaxToken> FormatVisibility(ConstructorSyntax syntax) {
			return this.FormatVisibility(syntax.GetVisibility());
		}

		public SyntaxToken FormatInheritance(ConstructorSyntax syntax) {
			return this.FormatInheritance(syntax.GetInheritance());
		}

		public List<SyntaxToken> FormatParameters(ConstructorSyntax syntax) {
			List<SyntaxToken> tokens = new List<SyntaxToken>();
			List<ParameterDetails> parameters = syntax.GetParameters();

			tokens.Add(new SyntaxToken("(", SyntaxTokens.Text));
			for (int i = 0; i < parameters.Count; i++) {
				if (i != 0) {
					tokens.Add(new SyntaxToken(",\n\t", SyntaxTokens.Text));
				}
				else {
					tokens.Add(new SyntaxToken("\n\t", SyntaxTokens.Text));
				}

				if (parameters[i].TypeDetails.IsByRef) {
					tokens.Add(new SyntaxToken("ByRef", SyntaxTokens.Keyword));
					tokens.Add(new SyntaxToken(" ", SyntaxTokens.Text));
				}
				tokens.Add(new SyntaxToken(parameters[i].Name, SyntaxTokens.Text));
				tokens.Add(new SyntaxToken(" ", SyntaxTokens.Text));
				tokens.Add(new SyntaxToken("As", SyntaxTokens.Keyword));
				tokens.Add(new SyntaxToken(" ", SyntaxTokens.Text));
				tokens.AddRange(this.FormatTypeDetails(parameters[i].TypeDetails));
			}
			if (parameters.Count > 0) {
				tokens.Add(new SyntaxToken("\n\t", SyntaxTokens.Text));
			}
			tokens.Add(new SyntaxToken(")", SyntaxTokens.Text));

			return tokens;
		}

		public SyntaxTokenCollection Format(ConstructorSyntax syntax) {
			SyntaxTokenCollection tokens = new SyntaxTokenCollection();

			SyntaxToken inheritanceModifier = this.FormatInheritance(syntax);

			tokens.AddRange(this.FormatVisibility(syntax));
			if (inheritanceModifier != null) {
				tokens.Add(new SyntaxToken(" ", SyntaxTokens.Text));
				tokens.Add(inheritanceModifier);
			}
			tokens.Add(new SyntaxToken(" ", SyntaxTokens.Text));
			tokens.Add(new SyntaxToken("Sub", SyntaxTokens.Keyword));
			tokens.Add(new SyntaxToken(" ", SyntaxTokens.Text));
			tokens.Add(new SyntaxToken("New", SyntaxTokens.Keyword));
			tokens.AddRange(this.FormatParameters(syntax));

			return tokens;
		}
	}
}
