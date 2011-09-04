using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection.Syntax.VisualBasic {
	public class VBInterfaceFormatter : VBFormatter, IInterfaceFormatter {
		private InterfaceSyntax syntax;

		public VBInterfaceFormatter(InterfaceSyntax syntax) {
			this.syntax = syntax;
		}

		public List<SyntaxToken> Format() {
			return this.Format(this.syntax);
		}

		public List<SyntaxToken> FormatVisibility(InterfaceSyntax syntax) {
			return this.FormatVisibility(syntax.GetVisibility());
		}

		public List<SyntaxToken> FormatInterfaceBase(InterfaceSyntax syntax) {
			List<SyntaxToken> tokens = new List<SyntaxToken>();

			// Create the list of types and interfaces
			List<TypeRef> baseTypesAndInterfaces = new List<TypeRef>();

			baseTypesAndInterfaces.AddRange(syntax.GetInterfaces());

			if (baseTypesAndInterfaces.Count > 0) {
				tokens.Add(new SyntaxToken("Implements", SyntaxTokens.Keyword));
				tokens.Add(new SyntaxToken(" ", SyntaxTokens.Text));
				for (int i = 0; i < baseTypesAndInterfaces.Count; i++) {
					if (i != 0) {
						tokens.Add(new SyntaxToken(", ", SyntaxTokens.Text));
					}
					tokens.Add(this.FormatTypeName(baseTypesAndInterfaces[i]));
				}
			}

			return tokens;
		}

		public List<SyntaxToken> Format(InterfaceSyntax syntax) {
			List<SyntaxToken> tokens = new List<SyntaxToken>();

			tokens.AddRange(this.FormatVisibility(syntax));
			tokens.Add(new SyntaxToken(" ", SyntaxTokens.Text));
			tokens.Add(new SyntaxToken("Interface", SyntaxTokens.Keyword));
			tokens.Add(new SyntaxToken(" ", SyntaxTokens.Text));
			tokens.Add(new SyntaxToken(syntax.GetIdentifier(), SyntaxTokens.Text));

			List<SyntaxToken> baseTokens = this.FormatInterfaceBase(syntax);
			if (baseTokens.Count > 0) {
				tokens.Add(new SyntaxToken(" _\n\t", SyntaxTokens.Text));
				tokens.AddRange(baseTokens);
			}

			return tokens;
		}
	}
}
