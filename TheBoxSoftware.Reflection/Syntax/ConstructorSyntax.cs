using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection.Syntax {
	using TheBoxSoftware.Reflection.Signitures;

	public class ConstructorSyntax : Syntax {
		private MethodDef method;
		private Signiture signiture;

		public ConstructorSyntax(MethodDef method) {
			this.method = method;
			this.signiture = method.Signiture;
		}

		public Visibility GetVisibility() {
			switch (this.method.Attributes & TheBoxSoftware.Reflection.Core.COFF.MethodAttributes.MemberAccessMask) {
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

		public Inheritance GetInheritance() {
			Inheritance classInheritance = Inheritance.Default;
			if ((this.method.Attributes & TheBoxSoftware.Reflection.Core.COFF.MethodAttributes.Static) == TheBoxSoftware.Reflection.Core.COFF.MethodAttributes.Static) {
				classInheritance = Inheritance.Static;
			}
			else if ((this.method.Attributes & TheBoxSoftware.Reflection.Core.COFF.MethodAttributes.Abstract) == TheBoxSoftware.Reflection.Core.COFF.MethodAttributes.Abstract) {
				classInheritance = Inheritance.Abstract;
			}
			else if ((this.method.Attributes & TheBoxSoftware.Reflection.Core.COFF.MethodAttributes.Virtual) == TheBoxSoftware.Reflection.Core.COFF.MethodAttributes.Virtual) {
				classInheritance = Inheritance.Virtual;
			}
			return classInheritance;
		}

		/// <summary>
		/// Obtains the cleaned up identifier for the method.
		/// </summary>
		/// <returns>The name of the method.</returns>
		/// <remarks>
		/// If the method is generic, the metadata stores the name with a `1 style
		/// prefix denoting details of its generic types. This method cleans that
		/// information up. So for example a method Test'1 will return Test.
		/// </remarks>
		public string GetIdentifier() {
			string typeName = this.method.Type.Name;
			if (this.method.Type.IsGeneric) {
				int count = int.Parse(typeName.Substring(typeName.IndexOf('`') + 1));
				typeName = typeName.Substring(0, typeName.IndexOf('`'));
			}
			return typeName;
		}

		public List<ParameterDetails> GetParameters() {
			List<ParameterDetails> details = new List<ParameterDetails>();
			List<ParamSignitureToken> definedParameters = new List<ParamSignitureToken>(this.signiture.Tokens.FindAll(
				t => t.TokenType == SignitureTokens.Param
				).ConvertAll<ParamSignitureToken>(p => (ParamSignitureToken)p).ToArray());
			List<ParamDef> parameters = this.method.Parameters;

			for (int i = 0; i < parameters.Count; i++) {
				details.Add(new ParameterDetails(
					parameters[i].Name,
					definedParameters[i].GetTypeDetails(this.method)
					));
			}
			return details;
		}

		public MethodDef Method { get { return this.method; } }
	}
}
