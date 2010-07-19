using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection.Syntax {
	public class StructSyntax : Syntax {
		/// <summary>
		/// Internal reference to a type that details of syntax elements can be obtained
		/// </summary>
		private TypeDef type;

		/// <summary>
		/// Initialises a new instance of the ClassSyntax class.
		/// </summary>
		/// <param name="type">The type to retrieve syntax details for.</param>
		public StructSyntax(TypeDef type) {
			this.type = type;
		}

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

		public string GetIdentifier() {
			return this.type.GetDisplayName(false);
		}

		public TypeRef[] GetInterfaces() {
			TypeRef[] interfaces = new TypeRef[this.type.Implements.Count];
			for (int i = 0; i < this.type.Implements.Count; i++) {
				interfaces[i] = this.type.Implements[i];
			}
			return interfaces;
		}
	}
}
