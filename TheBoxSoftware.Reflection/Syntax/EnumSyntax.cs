using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

		public string GetUnderlyingType() {
			return string.Empty;
		}

		/// <summary>
		/// Obtains the visibility modifier for the enumeration.
		/// </summary>
		/// <returns>The visibility modifier for the enumeration.</returns>
		public Visibility GetVisibility() {
			switch (this.type.Flags & TheBoxSoftware.Reflection.Core.COFF.TypeAttributes.VisibilityMask) {
				case TheBoxSoftware.Reflection.Core.COFF.TypeAttributes.NestedPublic:
				case TheBoxSoftware.Reflection.Core.COFF.TypeAttributes.Public:
					return Visibility.Public;
				case TheBoxSoftware.Reflection.Core.COFF.TypeAttributes.NotPublic:
					return Visibility.Internal;
				case TheBoxSoftware.Reflection.Core.COFF.TypeAttributes.NestedFamAndAssem:
					return Visibility.InternalProtected;
				case TheBoxSoftware.Reflection.Core.COFF.TypeAttributes.NestedFamily:
					return Visibility.Protected;
				case TheBoxSoftware.Reflection.Core.COFF.TypeAttributes.NestedPrivate:
					return Visibility.Private;
				case TheBoxSoftware.Reflection.Core.COFF.TypeAttributes.NestedFamOrAssem:
					return Visibility.Internal;
				default:
					return Visibility.Internal;
			}
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
