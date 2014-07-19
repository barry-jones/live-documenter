using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection.Syntax.VisualBasic {
	internal sealed class VBClassFormatter : VBFormatter, IClassFormatter {
		private ClassSyntax syntax;

		public VBClassFormatter(ClassSyntax syntax) {
			this.syntax = syntax;
		}

		public List<SyntaxToken> FormatVisibility(ClassSyntax syntax) {
			return this.FormatVisibility(syntax.GetVisibility());
		}

		public SyntaxToken FormatInheritance(ClassSyntax syntax) {
			return this.FormatInheritance(syntax.GetInheritance());
		}

		public List<SyntaxToken> FormatClassBase(ClassSyntax syntax) {
			List<SyntaxToken> tokens = new List<SyntaxToken>();
			bool hasBaseType = false;

			// Create the list of types and interfaces
			if (syntax.Class.InheritsFrom != null && syntax.Class.InheritsFrom.GetFullyQualifiedName() != "System.Object") {
				hasBaseType = true;
			}

			if (hasBaseType) {
				tokens.Add(new SyntaxToken("Derives", SyntaxTokens.Keyword));
				tokens.Add(new SyntaxToken(" ", SyntaxTokens.Text));
				tokens.AddRange(this.FormatTypeDetails(syntax.GetBaseClass()));
			}

			Signitures.TypeDetails[] interfaces = syntax.GetInterfaces();
			for (int i = 0; i < interfaces.Length; i++) {
				if (i == 0) {
					tokens.Add(new SyntaxToken(" _\n\t", SyntaxTokens.Text));
					tokens.Add(new SyntaxToken("Implements", SyntaxTokens.Keyword));
					tokens.Add(new SyntaxToken(" ", SyntaxTokens.Text));
				}
				else if (hasBaseType && i == 0 || i != 0) {
					tokens.Add(new SyntaxToken(", _\n\t\t", SyntaxTokens.Text));
				}
				tokens.AddRange(this.FormatTypeDetails(interfaces[i]));
			}

			return tokens;
		}

		public SyntaxTokenCollection Format(ClassSyntax syntax) {
			SyntaxTokenCollection tokens = new SyntaxTokenCollection();
			SyntaxToken inheritanceModifier = this.FormatInheritance(syntax);

			tokens.AddRange(this.FormatVisibility(syntax));
			if (inheritanceModifier != null) {
				tokens.Add(new SyntaxToken(" ", SyntaxTokens.Text));
				tokens.Add(inheritanceModifier);
			}
			tokens.Add(new SyntaxToken(" ", SyntaxTokens.Text));
			tokens.Add(new SyntaxToken("Class", SyntaxTokens.Keyword));
			tokens.Add(new SyntaxToken(" ", SyntaxTokens.Text));
			tokens.Add(new SyntaxToken(syntax.GetIdentifier(), SyntaxTokens.Text));

			if (this.syntax.Class.IsGeneric) {
				List<GenericTypeRef> genericTypes = this.syntax.GetGenericParameters();
				tokens.AddRange(this.FormatGenericParameters(genericTypes));
			}

			List<SyntaxToken> baseTokens = this.FormatClassBase(syntax);
			if (baseTokens.Count > 0) {
				tokens.Add(new SyntaxToken(" _\n\t", SyntaxTokens.Text));
				tokens.AddRange(baseTokens);
			}

			return tokens;
		}

		public SyntaxTokenCollection Format() {
			return this.Format(this.syntax);
		}
	}
}
