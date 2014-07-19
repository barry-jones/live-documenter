using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection.Syntax {
	internal sealed class InterfaceSyntax : Syntax {
		/// <summary>
		/// Internal reference to a type that details of syntax elements can be obtained
		/// </summary>
		private TypeDef type;

		/// <summary>
		/// Initialises a new instance of the InterfaceSyntax class.
		/// </summary>
		/// <param name="type">The type to retrieve syntax details for.</param>
		/// <exception cref="InvalidOperationException">
		/// Thrown when the provided <paramref name="type"/> is not an interface.
		/// </exception>
		public InterfaceSyntax(TypeDef type) {
			if (!type.IsInterface) {
				InvalidOperationException ex = new InvalidOperationException(
					string.Format("The type '{0}' is not an interface.",
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
			return this.type.GetDisplayName(false);
		}

		/// <summary>
		/// Obtains the names of all the interfaces this class implements.
		/// </summary>
		/// <returns>An array of strings identifying the interfaces.</returns>
		public TypeRef[] GetInterfaces() {
			TypeRef[] interfaces = new TypeRef[this.type.Implements.Count];
			for (int i = 0; i < this.type.Implements.Count; i++) {
				interfaces[i] = this.type.Implements[i];
			}
			return interfaces;
		}

		/// <summary>
		/// Obtains the name of the base type this class implements.
		/// </summary>
		/// <returns>The base class for the type.</returns>
		public string GetBaseClass() {
#if DEBUG
			if (this.type.InheritsFrom == null) {
				System.Diagnostics.Debug.WriteLine(string.Format("class {0} has a null base type.", this.type.Name));
				return string.Empty;
			}
#endif
			return this.type.InheritsFrom.GetDisplayName(false);
		}

		/// <summary>
		/// Obtains the visibility modifier for the class.
		/// </summary>
		/// <returns>The visibility modifier for the class.</returns>
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
