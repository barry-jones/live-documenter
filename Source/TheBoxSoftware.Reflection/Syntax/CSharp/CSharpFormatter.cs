using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection.Syntax.CSharp {
	using TheBoxSoftware.Reflection.Signitures;

	/// <summary>
	/// Base class with methods that are useful for all C# formatting
	/// classes.
	/// </summary>
	internal abstract class CSharpFormatter {
		/// <summary>
		/// Collection of all the types defined in this language with a language
		/// specific short form.
		/// </summary>
		private Dictionary<string, string> defaultTypes = new Dictionary<string, string>() {
			{"System.Object", "object"},
			{"System.Boolean", "bool"},
			{"System.SByte", "sbyte"},
			{"System.Byte", "byte"},
			{"System.Char", "char"},
			{"System.Double", "double"},
			{"System.Int16", "short"},
			{"System.Int32", "int"},
			{"System.Int64", "long"},
			{"System.Single", "float"},
			{"System.String", "string"},
			{"System.UInt16", "ushort"},
			{"System.UInt32", "uint"},
			{"System.UInt64", "ulong"},
			{"System.Void", "void"}
			};

		/// <summary>
		/// Foramts the visibility modifier.
		/// </summary>
		/// <param name="syntax">The visibility to format.</param>
		/// <returns>A formatted string representing the syntaxs.</returns>
		protected List<SyntaxToken> FormatVisibility(Visibility visibility) {
			List<SyntaxToken> tokens = new List<SyntaxToken>();
			switch (visibility) {
				case Visibility.Internal:
					tokens.Add(new SyntaxToken("internal", SyntaxTokens.Keyword));
					break;
				case Visibility.InternalProtected:
					tokens.Add(new SyntaxToken("internal", SyntaxTokens.Keyword));
					tokens.Add(new SyntaxToken(" ", SyntaxTokens.Text));
					tokens.Add(new SyntaxToken("protected", SyntaxTokens.Keyword));
					break;
				case Visibility.Protected:
					tokens.Add(new SyntaxToken("protected", SyntaxTokens.Keyword));
					break;
				case Visibility.Private:
					tokens.Add(new SyntaxToken("private", SyntaxTokens.Keyword));
					break;
				case Visibility.Public:
					tokens.Add(new SyntaxToken("public", SyntaxTokens.Keyword));
					break;
				default:
					throw new NotImplementedException();
			}
			return tokens;
		}

		/// <summary>
		/// Formats the inheritance modifier for C#.
		/// </summary>
		/// <param name="inheritance">The modifier to format.</param>
		/// <returns>The formatted syntax token.</returns>
		public SyntaxToken FormatInheritance(Inheritance inheritance) {
			switch (inheritance) {
				case Inheritance.Abstract:
					return new SyntaxToken("abstract", SyntaxTokens.Keyword);
				case Inheritance.Sealed:
					return new SyntaxToken("sealed", SyntaxTokens.Keyword);
				case Inheritance.Static:
					return new SyntaxToken("static", SyntaxTokens.Keyword);
				default:
					return null;
			}
		}

		/// <summary>
		/// Returns the language specific type name for the specified <paramref name="type"/>. For
		/// example in C# System.Boolean is referenced via the bool keyword.
		/// </summary>
		/// <param name="type">The type to get the name for.</param>
		/// <returns>The SyntaxToken reprsenting the name</returns>
		protected SyntaxToken FormatTypeName(TypeRef type) {
			string name = type.GetFullyQualifiedName();
			if (this.defaultTypes.ContainsKey(name)) {
				return new SyntaxToken(this.defaultTypes[name], SyntaxTokens.Keyword);
			}
			else {
				string typeName = type.Name;
				if (type.IsGeneric) {
					int count = int.Parse(typeName.Substring(typeName.IndexOf('`') + 1));
					typeName = typeName.Substring(0, typeName.IndexOf('`'));
				}
				return new SyntaxToken(typeName, SyntaxTokens.Text);
			}
		}

		protected List<SyntaxToken> FormatTypeName(TypeRef[] types) {
			List<SyntaxToken> tokens = new List<SyntaxToken>();
			for (int i = 0; i < types.Length; i++) {
				tokens.Add(this.FormatTypeName(types[i]));
			}
			return tokens;
		}

		protected List<SyntaxToken> FormatTypeDetails(TypeDetails details) {
			List<SyntaxToken> tokens = new List<SyntaxToken>();
			if (details.IsByRef) {
				tokens.Add(new SyntaxToken("ref", SyntaxTokens.Keyword));
				tokens.Add(new SyntaxToken(" ", SyntaxTokens.Text));
			}

			// If the type is an array the type ref details are no longer valid
			if (details.IsArray || details.IsMultidemensionalArray) {
				if (details.IsArray) {
					tokens.AddRange(this.FormatTypeDetails(details.ArrayOf));
					tokens.Add(new SyntaxToken("[]", SyntaxTokens.Text));
				}
				if (details.IsMultidemensionalArray) {
					tokens.AddRange(this.FormatTypeDetails(details.ArrayOf));
					tokens.Add(new SyntaxToken("[", SyntaxTokens.Text));
					tokens.Add(new SyntaxToken(new String(',', details.ArrayShape.Rank - 1), SyntaxTokens.Text));
					tokens.Add(new SyntaxToken("]", SyntaxTokens.Text));
				}
			}
			else {
				tokens.Add(this.FormatTypeName(details.Type));
				if (details.IsGenericInstance) {
					tokens.Add(new SyntaxToken("<", SyntaxTokens.Text));
					for (int i = 0; i < details.GenericParameters.Count; i++) {
						if (i != 0) {
							tokens.Add(new SyntaxToken(", ", SyntaxTokens.Text));
						}
						tokens.AddRange(this.FormatTypeDetails(details.GenericParameters[i]));
					}
					tokens.Add(new SyntaxToken(">", SyntaxTokens.Text));
				}
			}

			if (details.IsPointer) {
				tokens.Add(new SyntaxToken("*", SyntaxTokens.Text));
			}
			return tokens;
		}

		protected List<SyntaxToken> FormatGenericParameters(List<GenericTypeRef> genericTypes) {
			List<SyntaxToken> tokens = new List<SyntaxToken>();
			tokens.Add(new SyntaxToken("<", SyntaxTokens.Text));

			for (int i = 0; i < genericTypes.Count; i++) {
				if (i != 0) {
					tokens.Add(new SyntaxToken(",", SyntaxTokens.Text));
				}
				tokens.Add(this.FormatTypeName(genericTypes[i]));
			}

			tokens.Add(new SyntaxToken(">", SyntaxTokens.Text));
			return tokens;
		}
	}
}
