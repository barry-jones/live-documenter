using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection.Syntax {
	using TheBoxSoftware.Reflection.Signitures;

	/// <summary>
	/// Provides access to details of an Indexor defined in the metadata.
	/// </summary>
	public class IndexorSyntax : Syntax {
		private PropertyDef propertyDef;
		private MethodDef get;
		private MethodDef set;

		/// <summary>
		/// Initialises a new instance of the EventSyntax class.
		/// </summary>
		/// <param name="propertyDef">The details of the event to get the information from.</param>
		public IndexorSyntax(PropertyDef propertyDef) {
			if (propertyDef.Name != "Item") {
				throw new InvalidOperationException("The provided property is not an indexor");
			}

			this.propertyDef = propertyDef;
			this.get = propertyDef.GetMethod;
			this.set = propertyDef.SetMethod;
		}

		/// <summary>
		/// Obtains the Visibility of the member.
		/// </summary>
		/// <returns>An enumerated value representing the visibility of the member.</returns>
		public Visibility GetVisibility() {
			return this.propertyDef.MemberAccess;
		}

		/// <summary>
		/// Obtains the Visibility of the getter method of the property.
		/// </summary>
		/// <returns>An enumerated value representing the visibility of the member.</returns>
		public Visibility GetGetterVisibility() {
			if (this.get == null) {
				if (this.set == null) {
					InvalidOperationException ex = new InvalidOperationException(
						"A property exists without a get or set method."
						);
					throw ex;
				}
				return this.GetSetterVisibility();
			}
			else {
				return this.get.MemberAccess;
			}
		}

		/// <summary>
		/// Obtains the Visibility of the setter method of the property.
		/// </summary>
		/// <returns>An enumerated value representing the visibility of the member.</returns>
		public Visibility GetSetterVisibility() {
			if (this.set == null) {
				if (this.get == null) {
					InvalidOperationException ex = new InvalidOperationException(
						"A property exists without a get or set method."
						);
					throw ex;
				}
				return this.GetGetterVisibility();
			}
			else {
				return this.set.MemberAccess;
			}
		}

		/// <summary>
		/// Obtains details of how this member is inherited in base classes.
		/// </summary>
		/// <returns>An enumerated value describing how the method is inherited.</returns>
		public Inheritance GetInheritance() {
			MethodDef method = this.get != null ? this.get : this.set;
			Inheritance classInheritance = Inheritance.Default;
			if ((method.Attributes & TheBoxSoftware.Reflection.Core.COFF.MethodAttributes.Static) == TheBoxSoftware.Reflection.Core.COFF.MethodAttributes.Static) {
				classInheritance = Inheritance.Static;
			}
			else if ((method.Attributes & TheBoxSoftware.Reflection.Core.COFF.MethodAttributes.Abstract) == TheBoxSoftware.Reflection.Core.COFF.MethodAttributes.Abstract) {
				classInheritance = Inheritance.Abstract;
			}
			else if ((method.Attributes & TheBoxSoftware.Reflection.Core.COFF.MethodAttributes.Virtual) == TheBoxSoftware.Reflection.Core.COFF.MethodAttributes.Virtual) {
				classInheritance = Inheritance.Virtual;
			}
			return classInheritance;
		}

		/// <summary>
		/// Obtains a class that describes the details of types defined in this property.
		/// </summary>
		/// <returns>The details of the type.</returns>
		public TypeDetails GetType() {
			TypeDetails details = null;
			if (this.get != null) {
				ReturnTypeSignitureToken returnType = (ReturnTypeSignitureToken)this.get.Signiture.Tokens.Find(
					t => t.TokenType == SignitureTokens.ReturnType
					);
				details = returnType.GetTypeDetails(this.get);
			}
			else {
				ParamSignitureToken delegateType = (ParamSignitureToken)this.set.Signiture.GetParameterTokens()[0];
				details = delegateType.GetTypeDetails(this.set);
			}
			return details;
		}

		/// <summary>
		/// Obtains the name of the property. This will always be 'Item' for
		/// indexors.
		/// </summary>
		/// <returns>The identifier of the property</returns>
		public string GetIdentifier() {
			return this.propertyDef.Name;
		}

		/// <summary>
		/// A reference to the get method for this property.
		/// </summary>
		public MethodDef GetMethod {
			get { return this.get; }
		}

		/// <summary>
		/// A reference to the set method for this property.
		/// </summary>
		public MethodDef SetMethod {
			get { return this.set; }
		}
	}
}
