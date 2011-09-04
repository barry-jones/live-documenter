using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection.Syntax {
	using TheBoxSoftware.Reflection.Signitures;

	public class OperatorSyntax : Syntax {
		private MethodDef method;
		private Signiture signiture;

		public OperatorSyntax(MethodDef method) {
			this.method = method;
			this.signiture = method.Signiture;
		}

		public Visibility GetVisibility() {
			return this.method.MemberAccess;
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
		/// Operators have names such as op_Equality, but when actually producing
		/// the overload in code is == (c#). This will return the name as defined
		/// metadata, e.g. op_Equality - it is up to the language specific implementation
		/// to convert that to a more suitable representation.
		/// </remarks>
		public string GetIdentifier() {
			return this.method.Name;
		}

		/// <summary>
		/// Obtains a collection of <see cref="GenericTypeRef"/> instances detailing
		/// the generic types for this method.
		/// </summary>
		/// <returns>The collection of generic parameters for the method.</returns>
		/// <remarks>
		/// This method is only valid when the <see cref="MethodDef.IsGeneric"/> property
		/// has been set to true.
		/// </remarks>
		public List<GenericTypeRef> GetGenericParameters() {
			return this.method.GenericTypes;
		}

		public TypeDetails GetReturnType() {
			ReturnTypeSignitureToken returnType = (ReturnTypeSignitureToken)this.signiture.Tokens.Find(
				t => t.TokenType == SignitureTokens.ReturnType
				);
			TypeDetails details = returnType.GetTypeDetails(this.method);
			
			return details;
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
