using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection.Syntax.VisualBasic {
	using TheBoxSoftware.Reflection.Signitures;

	public sealed class VBDelegateFormatter : VBFormatter, IDelegateFormatter {
		private DelegateSyntax syntax;

		public VBDelegateFormatter(DelegateSyntax syntax) {
			this.syntax = syntax;
		}

		public List<SyntaxToken> Format() {
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

		public List<SyntaxToken> Format(DelegateSyntax syntax) {
			List<SyntaxToken> tokens = new List<SyntaxToken>();

			tokens.AddRange(this.FormatVisibility(syntax));
			tokens.Add(new SyntaxToken(" ", SyntaxTokens.Text));
			tokens.Add(new SyntaxToken("Delegate", SyntaxTokens.Keyword));
			tokens.Add(new SyntaxToken(" ", SyntaxTokens.Text));
			if (this.IsMethodFunction(syntax.GetReturnType())) {
				tokens.Add(new SyntaxToken("Function", SyntaxTokens.Keyword));
			}
			else {
				tokens.Add(new SyntaxToken("Sub", SyntaxTokens.Keyword));
			}
			tokens.Add(new SyntaxToken(" ", SyntaxTokens.Text));
			tokens.Add(new SyntaxToken(syntax.GetIdentifier(), SyntaxTokens.Text));
			tokens.AddRange(this.FormatParameters(syntax.Method));

			// Add the generic details of the delegate
			if (this.syntax.Class.IsGeneric) {
				List<GenericTypeRef> genericTypes = this.syntax.GetGenericParameters();
				tokens.AddRange(this.FormatGenericParameters(genericTypes));
			}

			if (this.IsMethodFunction(syntax.GetReturnType())) {
				tokens.Add(new SyntaxToken(" ", SyntaxTokens.Text));
				tokens.Add(new SyntaxToken("As", SyntaxTokens.Keyword));
				tokens.Add(new SyntaxToken(" ", SyntaxTokens.Text));
				tokens.AddRange(this.FormatReturnType(syntax));
			}

			return tokens;
		}
	}
}
