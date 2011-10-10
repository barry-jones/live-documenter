using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheBoxSoftware.Reflection.Signitures;

namespace TheBoxSoftware.Reflection.Syntax {
	/// <summary>
	/// 
	/// </summary>
	public class EnumSyntax : Syntax {
		/// <summary>
		/// Internal reference to a type that details of syntax elements can be obtained
		/// </summary>
		private TypeDef type;

		/// <summary>
		/// Initialises a new instance of the ClassSyntax class.
		/// </summary>
		/// <param name="type">The type to retrieve syntax details for.</param>
		public EnumSyntax(TypeDef type) {
			if(!type.IsEnumeration) {
				InvalidOperationException ex = new InvalidOperationException(string.Format(
					"The type '{0}' is not an enumeration.",
					type.GetFullyQualifiedName()
					));
				throw ex;
			}
			this.type = type;
		}

		#region Methods
		/// <summary>
		/// Obtains the identifier for the class.
		/// </summary>
		/// <returns>The type identifier.</returns>
		public string GetIdentifier() {
			return base.GetTypeName(this.Class);
		}

		public TypeRef GetUnderlyingType() {
			TypeRef underlyingType = null;

			if(this.type.Fields.Count > 0) { // there should always be two field on an enumeration
				underlyingType = ((TypeSignitureToken)this.type.Fields[0].Signiture.Tokens[0]).ElementType.ResolveToken(this.type.Assembly);
			}

			return underlyingType;
		}

		/// <summary>
		/// Obtains the visibility modifier for the enumeration.
		/// </summary>
		/// <returns>The visibility modifier for the enumeration.</returns>
		public Visibility GetVisibility() {
			return this.type.MemberAccess;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Access to the class reference being represented by this syntax class.
		/// </summary>
		public TypeDef Class {
			get { return this.type; }
		}
		#endregion
	}
}
