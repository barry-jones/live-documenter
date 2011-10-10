using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection.Syntax.VisualBasic {
	using TheBoxSoftware.Reflection.Signitures;

	/// <summary>
	/// Base class with methods that are useful for all VB formatting
	/// classes.
	/// </summary>
	public abstract class VBFormatter {
		/// <summary>
		/// Collection of all the types defined in this language with a language
		/// specific short form.
		/// </summary>
		private Dictionary<string, string> defaultTypes = new Dictionary<string, string>() {
			{"System.Object", "Object"},
			{"System.Boolean", "Boolean"},
			{"System.SByte", "SByte"},
			{"System.Byte", "Byte"},
			{"System.Char", "Char"},
			{"System.Double", "Double"},
			{"System.Int16", "Short"},
			{"System.Int32", "Int"},
			{"System.Int64", "Long"},
			{"System.Single", "Single"},
			{"System.String", "String"},
			{"System.UInt16", "UShort"},
			{"System.UInt32", "UInt"},
			{"System.UInt64", "ULong"},
			{"System.Void", "Void"}
			};

		protected List<SyntaxToken> FormatVisibility(Visibility visibility) {
			List<SyntaxToken> tokens = new List<SyntaxToken>();
			switch (visibility) {
				case Visibility.Internal:
					tokens.Add(new SyntaxToken("Friend", SyntaxTokens.Keyword));
					break;
				case Visibility.InternalProtected:
					tokens.Add(new SyntaxToken("Friend", SyntaxTokens.Keyword));
					tokens.Add(new SyntaxToken(" ", SyntaxTokens.Text));
					tokens.Add(new SyntaxToken("Protected", SyntaxTokens.Keyword));
					break;
				case Visibility.Protected:
					tokens.Add(new SyntaxToken("Protected", SyntaxTokens.Keyword));
					break;
				case Visibility.Private:
					tokens.Add(new SyntaxToken("Private", SyntaxTokens.Keyword));
					break;
				case Visibility.Public:
					tokens.Add(new SyntaxToken("Public", SyntaxTokens.Keyword));
					break;
				default:
					throw new NotImplementedException();
			}
			return tokens;
		}

		protected SyntaxToken FormatInheritance(Inheritance inheritance) {
			switch (inheritance) {
				case Inheritance.Abstract:
					return new SyntaxToken("MustInherit", SyntaxTokens.Keyword);
				case Inheritance.Sealed:
					return new SyntaxToken("NotInheritable", SyntaxTokens.Keyword);
				case Inheritance.Static:
					return new SyntaxToken("Shared", SyntaxTokens.Keyword);
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

			// If the type is an array the type ref details are no longer valid
			if (details.IsArray || details.IsMultidemensionalArray) {
				if (details.IsArray) {
					tokens.AddRange(this.FormatTypeDetails(details.ArrayOf));
					tokens.Add(new SyntaxToken("()", SyntaxTokens.Text));
				}
				if (details.IsMultidemensionalArray) {
					tokens.AddRange(this.FormatTypeDetails(details.ArrayOf));
					tokens.Add(new SyntaxToken("(", SyntaxTokens.Text));
					tokens.Add(new SyntaxToken(new String(',', details.ArrayShape.Rank - 1), SyntaxTokens.Text));
					tokens.Add(new SyntaxToken(")", SyntaxTokens.Text));
				}
			}
			else {
				tokens.Add(this.FormatTypeName(details.Type));
				if (details.IsGenericInstance) {
					tokens.Add(new SyntaxToken("(", SyntaxTokens.Text));
					tokens.Add(new SyntaxToken("Of", SyntaxTokens.Keyword));
					tokens.Add(new SyntaxToken(" ", SyntaxTokens.Text));
					for (int i = 0; i < details.GenericParameters.Count; i++) {
						if (i != 0) {
							tokens.Add(new SyntaxToken(", ", SyntaxTokens.Text));
						}
						tokens.AddRange(this.FormatTypeDetails(details.GenericParameters[i]));
					}
					tokens.Add(new SyntaxToken(")", SyntaxTokens.Text));
				}
			}

			// Pointers are not supported in visual basic .net. This would mean that
			// this element would not be available to a vb.net application. For now
			// we will just not show it.
			//if (details.IsPointer) {
			//    tokens.Add(new SyntaxToken("*", SyntaxTokens.Text));
			//}
			return tokens;
		}

		protected bool IsMethodFunction(TypeDetails details) {
			return details.IsArray || details.Type.GetFullyQualifiedName() != "System.Void";
		}

		/// <summary>
		/// Formats the generic types for a the specified <paramref name="genericTypes"/>.
		/// </summary>
		/// <param name="genericTypes">The types to format.</param>
		/// <returns>The tokens for the generic types.</returns>
		protected List<SyntaxToken> FormatGenericParameters(List<GenericTypeRef> genericTypes) {
			List<SyntaxToken> tokens = new List<SyntaxToken>();
			tokens.Add(new SyntaxToken(" ", SyntaxTokens.Text));
			tokens.Add(new SyntaxToken("Of", SyntaxTokens.Keyword));
			tokens.Add(new SyntaxToken("(", SyntaxTokens.Text));

			for (int i = 0; i < genericTypes.Count; i++) {
				if (i != 0) {
					tokens.Add(new SyntaxToken(",", SyntaxTokens.Text));
				}
				tokens.Add(this.FormatTypeName(genericTypes[i]));
			}

			tokens.Add(new SyntaxToken(")", SyntaxTokens.Text));
			return tokens;
		}
	}
}
