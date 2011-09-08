using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection.Syntax.CSharp {
	using TheBoxSoftware.Reflection.Signitures;

	public class CSharpMethodFormatter : CSharpFormatter, IMethodFormatter {
		private MethodSyntax syntax;
		private Signiture signiture;

		public CSharpMethodFormatter(MethodSyntax syntax) {
			this.syntax = syntax;
			this.signiture = syntax.Method.Signiture;
		}

		public SyntaxTokenCollection Format() {
			return this.Format(this.syntax);
		}

		public List<SyntaxToken> FormatVisibility(MethodSyntax syntax) {
			return this.FormatVisibility(syntax.GetVisibility());
		}

		public SyntaxToken FormatInheritance(MethodSyntax syntax) {
			return this.FormatInheritance(syntax.GetInheritance());
		}

		public List<SyntaxToken> FormatParameters(MethodSyntax syntax) {
			List<SyntaxToken> tokens = new List<SyntaxToken>();
			List<ParameterDetails> parameters = syntax.GetParameters();

			tokens.Add(new SyntaxToken("(", SyntaxTokens.Text));
			for (int i = 0; i < parameters.Count; i++) {
				if (i != 0) {
					tokens.Add(new SyntaxToken(",\n\t", SyntaxTokens.Text));
				}
				else {
					tokens.Add(new SyntaxToken("\n\t", SyntaxTokens.Text));
					if (syntax.Method.IsExtensionMethod) {
						tokens.Add(new SyntaxToken("this ", SyntaxTokens.Keyword));
					}
				}
				tokens.AddRange(this.FormatTypeDetails(parameters[i].TypeDetails));
				tokens.Add(new SyntaxToken(" ", SyntaxTokens.Text));
				tokens.Add(new SyntaxToken(parameters[i].Name, SyntaxTokens.Text));
			}
			if (parameters.Count > 0) {
				tokens.Add(new SyntaxToken("\n\t", SyntaxTokens.Text));
			}
			tokens.Add(new SyntaxToken(")", SyntaxTokens.Text));

			return tokens;
		}

		public List<SyntaxToken> FormatReturnType(MethodSyntax syntax) {
			return this.FormatTypeDetails(syntax.GetReturnType());
		}

		public SyntaxTokenCollection Format(MethodSyntax syntax) {
			SyntaxTokenCollection tokens = new SyntaxTokenCollection();

			SyntaxToken inheritanceModifier = this.FormatInheritance(syntax);

			tokens.AddRange(this.FormatVisibility(syntax));
			if (inheritanceModifier != null) {
				tokens.Add(new SyntaxToken(" ", SyntaxTokens.Text));
				tokens.Add(inheritanceModifier);
			}
			tokens.Add(new SyntaxToken(" ", SyntaxTokens.Text));
			tokens.AddRange(this.FormatReturnType(syntax));
			tokens.Add(new SyntaxToken(" ", SyntaxTokens.Text));
			tokens.Add(new SyntaxToken(syntax.GetIdentifier(), SyntaxTokens.Text));
			if (syntax.Method.IsGeneric) {
				tokens.Add(new SyntaxToken("<", SyntaxTokens.Text));
				List<GenericTypeRef> genericTypes = syntax.GetGenericParameters();
				for (int i = 0; i < genericTypes.Count; i++) {
					if (i != 0) {
						tokens.Add(new SyntaxToken(", ", SyntaxTokens.Text));
					}
					tokens.Add(this.FormatTypeName(genericTypes[i]));
				}
				tokens.Add(new SyntaxToken(">", SyntaxTokens.Text));
			}
			tokens.AddRange(this.FormatParameters(syntax));

			return tokens;
		}
	}
}
