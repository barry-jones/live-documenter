using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection.Syntax {
	using TheBoxSoftware.Reflection.Signitures;

	/// <summary>
	/// Provides access to the important information for creating formatted
	/// syntax for <see cref="PropertyDef"/>s.
	/// </summary>
	internal class PropertySyntax : Syntax {
		private PropertyDef propertyDef;
		private MethodDef get;
		private MethodDef set;

		/// <summary>
		/// Initialises a new instance of the EventSyntax class.
		/// </summary>
		/// <param name="propertyDef">The details of the event to get the information from.</param>
		public PropertySyntax(PropertyDef propertyDef) {
			this.propertyDef = propertyDef;
			this.get = propertyDef.GetMethod;
			this.set = propertyDef.SetMethod;
		}

		public Visibility GetVisibility() {
			return this.propertyDef.MemberAccess;
		}

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

		public string GetIdentifier() {
			return this.propertyDef.Name;
		}

		public MethodDef GetMethod {
			get { return this.get; }
		}

		public MethodDef SetMethod {
			get { return this.set; }
		}
	}
}
