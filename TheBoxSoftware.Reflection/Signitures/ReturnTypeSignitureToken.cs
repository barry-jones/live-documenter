using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection.Signitures {
	using TheBoxSoftware.Reflection.Core;

	/// <summary>
	/// Represents the group of SignitureTokens that are read together.
	/// </summary>
	public sealed class ReturnTypeSignitureToken : SignitureTokenContainer {
		/// <summary>
		/// Initialises a new instance of the ReturnTypeSignitureToken class.
		/// </summary>
		/// <param name="signiture">The signiture to read.</param>
		/// <param name="offset">The offset to start processing at.</param>
		public ReturnTypeSignitureToken(PeCoffFile file, byte[] signiture, Offset offset)
			: base(SignitureTokens.ReturnType) {
			while (CustomModifierToken.IsToken(signiture, offset)) {
				this.Tokens.Add(new CustomModifierToken(signiture, offset));
			}
			if (ElementTypeSignitureToken.IsToken(signiture, offset, ElementTypes.ByRef)) {
				this.Tokens.Add(new ElementTypeSignitureToken(file, signiture, offset));	// ByRef
				this.Tokens.Add(new TypeSignitureToken(file, signiture, offset));	// Type
			}
			else if (ElementTypeSignitureToken.IsToken(signiture, offset, ElementTypes.Void | ElementTypes.TypedByRef)) {
				this.Tokens.Add(new ElementTypeSignitureToken(file, signiture, offset));	// Void, TypedByRef
			}
			else {
				this.Tokens.Add(new TypeSignitureToken(file, signiture, offset));
			}
		}

		internal TypeRef ResolveType(AssemblyDef assembly) {
			if (this.Tokens.Last() is TypeSignitureToken) {
				return ((TypeSignitureToken)this.Tokens.Last()).ResolveType(assembly, null);
			}
			else {
				return ((ElementTypeSignitureToken)this.Tokens.Last()).ResolveToken(assembly);
			}
		}

		public TypeDetails GetTypeDetails(ReflectedMember member) {
			TypeDetails details = new TypeDetails();
			if (this.Tokens.Last() is TypeSignitureToken) {
				details = ((TypeSignitureToken)this.Tokens.Last()).GetTypeDetails(member);
			}
			else {
				details.Type = ((ElementTypeSignitureToken)this.Tokens.Last()).ResolveToken(member.Assembly);
			}
			return details;
		}
	}
}
