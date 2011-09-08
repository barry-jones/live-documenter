using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection.Syntax.CSharp {
	using TheBoxSoftware.Reflection.Signitures;

	public sealed class CSharpDelegateFormatter : CSharpFormatter, IDelegateFormatter {
		private DelegateSyntax syntax;

		public CSharpDelegateFormatter(DelegateSyntax syntax) {
			this.syntax = syntax;
		}

		public SyntaxTokenCollection Format() {
			return this.Format(this.syntax);
		}

		public List<SyntaxToken> FormatVisibility(DelegateSyntax syntax) {
			return this.FormatVisibility(syntax.GetVisibility());
		}

		public List<SyntaxToken> FormatReturnType(DelegateSyntax syntax) {
			return this.FormatTypeDetails(syntax.GetReturnType());
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

		public SyntaxTokenCollection Format(DelegateSyntax syntax) {
			SyntaxTokenCollection tokens = new SyntaxTokenCollection();

			tokens.AddRange(this.FormatVisibility(syntax));
			tokens.Add(new SyntaxToken(" ", SyntaxTokens.Text));
			tokens.Add(new SyntaxToken("delegate", SyntaxTokens.Keyword));
			tokens.Add(new SyntaxToken(" ", SyntaxTokens.Text));
			tokens.AddRange(this.FormatReturnType(syntax));
			tokens.Add(new SyntaxToken(" ", SyntaxTokens.Text));
			tokens.Add(new SyntaxToken(syntax.GetIdentifier(), SyntaxTokens.Text));
			if (this.syntax.Class.IsGeneric) {
				List<GenericTypeRef> genericTypes = this.syntax.GetGenericParameters();
				tokens.AddRange(this.FormatGenericParameters(genericTypes));
			}
			tokens.AddRange(this.FormatParameters(syntax.Method));

			return tokens;
		}
	}
}
