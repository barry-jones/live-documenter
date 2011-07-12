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
	public class PropertySyntax : Syntax {
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
			Visibility getVisibility = this.GetGetterVisibility();
			Visibility setVisibility = this.GetSetterVisibility();
			return ((int)getVisibility > (int)setVisibility)
				? getVisibility
				: setVisibility;
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
				switch (this.get.Attributes & TheBoxSoftware.Reflection.Core.COFF.MethodAttributes.MemberAccessMask) {
					case TheBoxSoftware.Reflection.Core.COFF.MethodAttributes.Public:
						return Visibility.Public;
					case TheBoxSoftware.Reflection.Core.COFF.MethodAttributes.Assem:
						return Visibility.Internal;
					case TheBoxSoftware.Reflection.Core.COFF.MethodAttributes.FamANDAssem:
						return Visibility.InternalProtected;
					case TheBoxSoftware.Reflection.Core.COFF.MethodAttributes.Family:
						return Visibility.Protected;
					case TheBoxSoftware.Reflection.Core.COFF.MethodAttributes.Private:
						return Visibility.Private;
					case TheBoxSoftware.Reflection.Core.COFF.MethodAttributes.FamORAssem:
						return Visibility.Internal;
					default:
						return Visibility.Internal;
				}
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
				switch (this.set.Attributes & TheBoxSoftware.Reflection.Core.COFF.MethodAttributes.MemberAccessMask) {
					case TheBoxSoftware.Reflection.Core.COFF.MethodAttributes.Public:
						return Visibility.Public;
					case TheBoxSoftware.Reflection.Core.COFF.MethodAttributes.Assem:
						return Visibility.Internal;
					case TheBoxSoftware.Reflection.Core.COFF.MethodAttributes.FamANDAssem:
						return Visibility.InternalProtected;
					case TheBoxSoftware.Reflection.Core.COFF.MethodAttributes.Family:
						return Visibility.Protected;
					case TheBoxSoftware.Reflection.Core.COFF.MethodAttributes.Private:
						return Visibility.Private;
					case TheBoxSoftware.Reflection.Core.COFF.MethodAttributes.FamORAssem:
						return Visibility.Internal;
					default:
						return Visibility.Internal;
				}
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
