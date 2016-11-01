using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection.Syntax.CSharp {
	using TheBoxSoftware.Reflection.Signitures;

	internal sealed class CSharpOperatorFormatter : CSharpFormatter, IOperatorFormatter {
		private OperatorSyntax syntax;
		private Signiture signiture;

		public CSharpOperatorFormatter(OperatorSyntax syntax) {
			this.syntax = syntax;
			this.signiture = syntax.Method.Signiture;
		}

		public SyntaxTokenCollection Format() {
			return this.Format(this.syntax);
		}

		public List<SyntaxToken> FormatVisibility(OperatorSyntax syntax) {
			return this.FormatVisibility(syntax.GetVisibility());
		}

		public SyntaxToken FormatInheritance(OperatorSyntax syntax) {
			return this.FormatInheritance(syntax.GetInheritance());
		}

		public List<SyntaxToken> FormatParameters(OperatorSyntax syntax) {
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
                tokens.AddRange(FormatParameterModifiers(parameters[i]));
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

		public List<SyntaxToken> FormatReturnType(OperatorSyntax syntax) {
			string identifier = syntax.GetIdentifier();
			List<SyntaxToken> tokens = new List<SyntaxToken>();

			if (identifier == "op_Explicit") {
				tokens.Add(new SyntaxToken("explicit", SyntaxTokens.Keyword));
			}
			else if (identifier == "op_Implicit") {
				tokens.Add(new SyntaxToken("implicit", SyntaxTokens.Keyword));
			}
			else {
				tokens.AddRange(this.FormatTypeDetails(syntax.GetReturnType()));
			}

			return tokens;
		}

		public List<SyntaxToken> FormatName(OperatorSyntax syntax) {
			string representation = string.Empty;

			switch (syntax.GetIdentifier()) {
					// Equality overloads
				case "op_Equality": representation = "=="; break;
				case "op_Inequality": representation = "!="; break;
				case "op_GreaterThan": representation = ">"; break;
				case "op_LessThan": representation = "<"; break;
				case "op_GreaterThanOrEqual": representation = ">="; break;
				case "op_LessThanOrEqual": representation = "<="; break;
					// Unary overloads
				case "op_UnaryPlus": representation = "+"; break;
				case "op_UnaryNegation": representation = "-"; break;
				case "op_LogicalNot": representation = "!"; break;
				case "op_OnesComplement": representation = "~"; break;
				case "op_Increment": representation = "++"; break;
				case "op_Decrement": representation = "--"; break;
				case "op_True": representation = "true"; break;
				case "op_False": representation = "false"; break;
					// Binary Overloads
				case "op_Addition": representation = "+"; break;
				case "op_Subtraction": representation = "-"; break;
				case "op_Multiply": representation = "*"; break;
				case "op_Division": representation = "/"; break;
				case "op_Modulus": representation = "%"; break;
				case "op_BitwiseAnd": representation = "&"; break;
				case "op_BitwiseOr": representation = "|"; break;
				case "op_ExclusiveOr": representation = "^"; break;
				case "op_LeftShift": representation = "<<"; break;
				case "op_RightShift": representation = ">>"; break;

					// Concatenation operator is & in a visual basic library - cant be overloaded in C#
				case "op_Concatenate": representation = "+"; break;

				case "op_Implicit": return this.FormatTypeDetails(syntax.GetReturnType());

				case "op_Explicit": return this.FormatTypeDetails(syntax.GetReturnType());

				default:
					throw new NotImplementedException(
						"Formatting not implemented for operator '" + syntax.GetIdentifier() + "'."
						);
			}

			return new List<SyntaxToken>() { new SyntaxToken(representation, SyntaxTokens.Text) };
		}

		public SyntaxTokenCollection Format(OperatorSyntax syntax) {
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
			tokens.Add(new SyntaxToken("operator", SyntaxTokens.Keyword));
			tokens.Add(new SyntaxToken(" ", SyntaxTokens.Text));
			tokens.AddRange(this.FormatName(syntax));
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
