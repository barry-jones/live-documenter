using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection.Syntax {
	using TheBoxSoftware.Reflection.Signitures;

	internal class DelegateSyntax : Syntax {
		/// <summary>
		/// Internal reference to a type that details of syntax elements can be obtained
		/// </summary>
		private TypeDef type;
		/// <summary>
		/// All delegates have an invoke method and this seems to detail the return type
		/// and parameters for the delegate. So here we will store a loaded syntax instance
		/// containing those details.
		/// </summary>
		private MethodSyntax invokeMethod;

		/// <summary>
		/// Initialises a new instance of the ClassSyntax class.
		/// </summary>
		/// <param name="type">The type to retrieve syntax details for.</param>
		/// <exception cref="ArgumentException">Thrown when the type provided is not a delegate.</exception>
		/// <exception cref="ArgumentNullException">Thrown when the type provided is null.</exception>
		public DelegateSyntax(TypeDef type) {
			if (type == null) {
				throw new ArgumentNullException("type");
			}
			if (!type.IsDelegate) {
				ArgumentException ex = new ArgumentException(
					"The type provided was not a delegate."
					);
				ex.Data["name"] = type.GetFullyQualifiedName();
				throw ex;
			}
			this.type = type;
			MethodDef invokeMethod = this.type.Methods.Find(m => m.Name == "Invoke");
			this.invokeMethod = new MethodSyntax(invokeMethod);
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

		public TypeDetails GetReturnType() {
			return this.invokeMethod.GetReturnType();
		}

		public List<ParameterDetails> GetParameters() {
			return this.invokeMethod.GetParameters();
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
			return this.type.GenericTypes;
		}

		/// <summary>
		/// Obtains the identifier for the delegate.
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

		#region Properties
		/// <summary>
		/// Access to the class reference being represented by this syntax class.
		/// </summary>
		public TypeDef Class {
			get { return this.type; }
		}

		public MethodSyntax Method {
			get { return this.invokeMethod; }
		}
		#endregion
	}
}
