using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection.Signitures {
	using TheBoxSoftware.Reflection.Core;

	/// <summary>
	/// Represents a signiture for a type specification as detailed in
	/// section 23.2.14 in ECMA 335.
	/// </summary>
	internal sealed class TypeSpecificationSigniture : Signiture {
		/// <summary>
		/// Instantiates a new instance of the TypeSpecificationSigniture class.
		/// </summary>
		/// <param name="file">The file containing the signiture</param>
		/// <param name="signiture">The actual signiture contents.</param>
		public TypeSpecificationSigniture(PeCoffFile file, byte[] signiture)
			: base(Signitures.TypeSpecification) {

			this.Type = new TypeSignitureToken(file, signiture, 0);
		}

		/// <summary>
		/// Obtains the details of the type.
		/// </summary>
		/// <param name="member">The member to resolve against.</param>
		/// <returns>The details of the type having the specification.</returns>
		public TypeDetails GetTypeDetails(ReflectedMember member) {
			return this.Type.GetTypeDetails(member);
		}

		/// <summary>
		/// 
		/// </summary>
		public TypeSignitureToken Type { get; set; }
	}
}
