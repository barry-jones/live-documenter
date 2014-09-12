using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection.Syntax {
	internal class StructSyntax : Syntax {
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
			return this.type.MemberAccess;
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
