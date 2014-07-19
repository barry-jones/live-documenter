using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection.Syntax {
	internal sealed class ClassSyntax : Syntax {
		/// <summary>
		/// Internal reference to a type that details of syntax elements can be obtained
		/// </summary>
		private TypeDef type;

		/// <summary>
		/// Initialises a new instance of the ClassSyntax class.
		/// </summary>
		/// <param name="type">The type to retrieve syntax details for.</param>
		public ClassSyntax(TypeDef type) {
			this.type = type;
		}

		#region Methods
		/// <summary>
		/// Obtains the identifier for the class.
		/// </summary>
		/// <returns>The type identifier.</returns>
		public string GetIdentifier() {
			string name = string.Empty;
			name = this.type.Name;
			if (this.type.IsGeneric) {
				int count = int.Parse(name.Substring(name.IndexOf('`') + 1));
				name = name.Substring(0, name.IndexOf('`'));
			}
			return name;
		}

		/// <summary>
		/// Obtains the names of all the interfaces this class implements.
		/// </summary>
		/// <returns>An array of strings identifying the interfaces.</returns>
		public Signitures.TypeDetails[] GetInterfaces() {
			Signitures.TypeDetails[] interfaces = new Signitures.TypeDetails[this.type.Implements.Count];
			for (int i = 0; i < this.type.Implements.Count; i++) {
				if (this.type.Implements[i] is TypeSpec) {
					interfaces[i] = ((TypeSpec)this.type.Implements[i]).TypeDetails;
				}
				else {
					Signitures.TypeDetails details = new TheBoxSoftware.Reflection.Signitures.TypeDetails();
					details.Type = this.type.Implements[i];
					interfaces[i] = details;
				}
			}
			return interfaces;
		}

		/// <summary>
		/// Obtains details of the generic parameters associated with this type.
		/// </summary>
		/// <returns>The array of generic parameters.</returns>
		/// <remarks>
		/// This method is only really valid when the type <see cref="TypeRef.IsGeneric"/>
		/// property has been set to true.
		/// </remarks>
		public List<GenericTypeRef> GetGenericParameters() {
			return this.type.GetGenericTypes();
		}

		/// <summary>
		/// Obtains the name of the base type this class implements.
		/// </summary>
		/// <returns>The base class for the type.</returns>
		public Signitures.TypeDetails GetBaseClass() {
#if DEBUG
			if (this.type.InheritsFrom == null) {
				System.Diagnostics.Debug.WriteLine(string.Format("class {0} has a null base type.", this.type.Name));
				return null;
			}
#endif
			if (this.type.InheritsFrom is TypeSpec) {
				return ((TypeSpec)this.type.InheritsFrom).TypeDetails;
			}
			else {
				Signitures.TypeDetails details = new TheBoxSoftware.Reflection.Signitures.TypeDetails();
				details.Type = this.type.InheritsFrom;
				return details;
			}
		}

		/// <summary>
		/// Obtains the visibility modifier for the class.
		/// </summary>
		/// <returns>The visibility modifier for the class.</returns>
		public Visibility GetVisibility() {
			return this.type.MemberAccess;
		}

		/// <summary>
		/// Obtains the inheritance modifier for the class.
		/// </summary>
		/// <returns>The inheritance modifier.</returns>
		/// <remarks>
		/// Although the language specification does not specify a static modifier here,
		/// classes which are defined as both abstract and sealed seems to be the way to
		/// define the static modifier in the metadata. That is a static class can not have
		/// instances created and can not be derived from.
		/// </remarks>
		public Inheritance GetInheritance() {
			Inheritance classInheritance = Inheritance.Default;
			if (
				(this.type.Flags & TheBoxSoftware.Reflection.Core.COFF.TypeAttributes.Abstract) == TheBoxSoftware.Reflection.Core.COFF.TypeAttributes.Abstract &&
				(this.type.Flags & TheBoxSoftware.Reflection.Core.COFF.TypeAttributes.Sealed) == TheBoxSoftware.Reflection.Core.COFF.TypeAttributes.Sealed
				) {
				classInheritance = Inheritance.Static;
			}
			else if ((this.type.Flags & TheBoxSoftware.Reflection.Core.COFF.TypeAttributes.Abstract) == TheBoxSoftware.Reflection.Core.COFF.TypeAttributes.Abstract) {
				classInheritance = Inheritance.Abstract;
			}
			else if ((this.type.Flags & TheBoxSoftware.Reflection.Core.COFF.TypeAttributes.Sealed) == TheBoxSoftware.Reflection.Core.COFF.TypeAttributes.Sealed) {
				classInheritance = Inheritance.Sealed;
			}
			return classInheritance;
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
