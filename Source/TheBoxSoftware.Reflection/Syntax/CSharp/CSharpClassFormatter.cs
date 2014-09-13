using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection.Syntax.CSharp {
	/// <summary>
	/// 
	/// </summary>
	/// <remarks>
	/// <code>
	/// class-declaration:
	///		attributes(opt) class-modifiers(opt) class identifier class-base(opt) class-body
	/// </code>
	/// </remarks>
	internal sealed class CSharpClassFormatter : CSharpFormatter, IClassFormatter {
		private ClassSyntax syntax;

		public CSharpClassFormatter(ClassSyntax syntax) {
			this.syntax = syntax;
		}

		public SyntaxTokenCollection Format() {
			return this.Format(this.syntax);
		}

		/// <summary>
		/// Foramts the visibility modifier for a c# class.
		/// </summary>
		/// <param name="syntax">The syntax to format.</param>
		/// <returns>A formatted string representing the syntax of the class.</returns>
		/// <remarks>
		/// The visibility of a class is part of the modifiers section of the
		/// decleration. The modifiers are defined as:
		/// <code>
		/// class-modifiers:
		///     class-modifier
		///     class-modifiers   class-modifier
		/// class-modifier:
		///     new
		///     public
		///     protected
		///     internal
		///     private
		///     abstract
		///     sealed 
		/// </code>
		/// </remarks>
		public List<SyntaxToken> FormatVisibility(ClassSyntax syntax) {
			return this.FormatVisibility(syntax.GetVisibility());
		}

		/// <summary>
		/// Formats the inheritance modifier for a c# class.
		/// </summary>
		/// <param name="syntax">The syntax to format.</param>
		/// <returns>The formatted string representing the syntax of the class.</returns>
		/// <remarks>
		/// The inheritance of a class is part of the modifiers section of the
		/// decleration. The modifiers are defined as:
		/// <code>
		/// class-modifiers:
		///     class-modifier
		///     class-modifiers   class-modifier
		/// class-modifier:
		///     new
		///     public
		///     protected
		///     internal
		///     private
		///     abstract
		///     sealed 
		/// </code>
		/// </remarks>
		public SyntaxToken FormatInheritance(ClassSyntax syntax) {
			return this.FormatInheritance(syntax.GetInheritance());
		}

		/// <summary>
		/// Foramts the class base portion of a class decleration.
		/// </summary>
		/// <param name="syntax">The syntax details for the class.</param>
		/// <returns>The string representing the class base.</returns>
		/// <remarks>
		/// The class base decleration is:
		/// <code>
		/// class-base:
		///     :   class-type
		///     :   interface-type-list
		///     :   class-type   ,   interface-type-list
		/// interface-type-list:
		///     interface-type
		///     interface-type-list   ,   interface-type 
		/// </code>
		/// </remarks>
		public List<SyntaxToken> FormatClassBase(ClassSyntax syntax) {
			List<SyntaxToken> tokens = new List<SyntaxToken>();
			bool hasBaseType = false;

			// Create the list of types and interfaces
			List<SyntaxToken> baseTypesAndInterfaces = new List<SyntaxToken>();
			if (syntax.Class.InheritsFrom != null && syntax.Class.InheritsFrom.GetFullyQualifiedName() != "System.Object") {
				tokens.Add(new SyntaxToken(": ", SyntaxTokens.Text));
				tokens.AddRange(this.FormatTypeDetails(syntax.GetBaseClass()));
				//tokens.Add(new SyntaxToken("\n\t", SyntaxTokens.Text));
				hasBaseType = true;
			}
			Signitures.TypeDetails[] details = syntax.GetInterfaces();
			for (int i = 0; i < details.Length; i++) {
				if (!hasBaseType && i == 0) {
					tokens.Add(new SyntaxToken(": ", SyntaxTokens.Text));
				}
				else if (hasBaseType && i == 0 || i != 0) {
					tokens.Add(new SyntaxToken(",\n\t", SyntaxTokens.Text));
				}
				tokens.AddRange(this.FormatTypeDetails(details[i]));
			}

			return tokens;
		}

		/// <summary>
		/// Formats the provided <paramref name="syntax"/> instance based on a
		/// c# class decleration.
		/// </summary>
		/// <param name="syntax">The syntax for format.</param>
		/// <returns>A fully formatted c# class decleration.</returns>
		public SyntaxTokenCollection Format(ClassSyntax syntax) {
			SyntaxTokenCollection tokens = new SyntaxTokenCollection();

			SyntaxToken inheritanceModifier = this.FormatInheritance(syntax);

			tokens.AddRange(this.FormatVisibility(syntax));
			if (inheritanceModifier != null) {
				tokens.Add(new SyntaxToken(" ", SyntaxTokens.Text));
				tokens.Add(inheritanceModifier);
			}
			tokens.Add(new SyntaxToken(" ", SyntaxTokens.Text));
			tokens.Add(new SyntaxToken("class", SyntaxTokens.Keyword));
			tokens.Add(new SyntaxToken(" ", SyntaxTokens.Text));
			tokens.Add(new SyntaxToken(syntax.GetIdentifier(), SyntaxTokens.Text));
			if (this.syntax.Class.IsGeneric) {
				List<GenericTypeRef> genericTypes = this.syntax.GetGenericParameters();
				tokens.AddRange(this.FormatGenericParameters(genericTypes));
			}
			tokens.Add(new SyntaxToken(" ", SyntaxTokens.Text));
			tokens.AddRange(this.FormatClassBase(syntax));

			return tokens;
		}
	}
}
