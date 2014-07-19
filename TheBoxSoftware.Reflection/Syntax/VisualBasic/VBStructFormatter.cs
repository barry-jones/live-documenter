using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection.Syntax.VisualBasic {
	internal sealed class VBStructFormatter : VBFormatter, IStructFormatter {
		private StructSyntax syntax;

		public VBStructFormatter(StructSyntax syntax) {
			this.syntax = syntax;
		}

		public SyntaxTokenCollection Format() {
			return this.Format(this.syntax);
		}

		public List<SyntaxToken> FormatVisibility(StructSyntax syntax) {
			return this.FormatVisibility(syntax.GetVisibility());
		}

		public List<SyntaxToken> FormatInterfaces(StructSyntax syntax) {
			List<SyntaxToken> tokens = new List<SyntaxToken>();

			// Create the list of types and interfaces
			List<TypeRef> baseTypesAndInterfaces = new List<TypeRef>();
			baseTypesAndInterfaces.AddRange(syntax.GetInterfaces());

			if (baseTypesAndInterfaces.Count > 0) {
				tokens.Add(new SyntaxToken(": ", SyntaxTokens.Text));
				for (int i = 0; i < baseTypesAndInterfaces.Count; i++) {
					if (i != 0) {
						tokens.Add(new SyntaxToken(", ", SyntaxTokens.Text));
					}
					tokens.Add(this.FormatTypeName(baseTypesAndInterfaces[i]));
				}
			}

			return tokens;
		}

		public SyntaxTokenCollection Format(StructSyntax syntax) {
			SyntaxTokenCollection tokens = new SyntaxTokenCollection();

			tokens.AddRange(this.FormatVisibility(syntax));
			tokens.Add(new SyntaxToken(" ", SyntaxTokens.Text));
			tokens.Add(new SyntaxToken("Structure", SyntaxTokens.Keyword));
			tokens.Add(new SyntaxToken(" ", SyntaxTokens.Text));
			tokens.Add(new SyntaxToken(syntax.GetIdentifier(), SyntaxTokens.Text));
			tokens.Add(new SyntaxToken(" ", SyntaxTokens.Text));
			tokens.AddRange(this.FormatInterfaces(syntax));

			return tokens;
		}
	}
}
