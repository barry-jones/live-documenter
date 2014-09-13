using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection.Syntax {
	using TheBoxSoftware.Reflection.Signitures;

	internal class MethodSyntax : Syntax {
		private MethodDef method;
		private Signiture signiture;

		public MethodSyntax(MethodDef method) {
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
		/// If the method is generic, the metadata stores the name with a `1 style
		/// prefix denoting details of its generic types. This method cleans that
		/// information up. So for example a method Test'1 will return Test.
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
			return this.method.GetGenericTypes();
		}

		public TypeDetails GetReturnType() {
			ReturnTypeSignitureToken returnType = (ReturnTypeSignitureToken)this.signiture.Tokens.Find(
				t => t.TokenType == SignitureTokens.ReturnType
				);
			TypeDetails details = returnType.GetTypeDetails(this.method);
			
			return details;
		}

		/// <summary>
		/// Collects and returns all of the parameters for the associated <see cref="MethodDef" />.
		/// </summary>
		/// <returns>The list of <see cref="ParameterDetails" /> detailing the parameters</returns>
		public List<ParameterDetails> GetParameters() {
			List<ParameterDetails> details = new List<ParameterDetails>();
			List<ParamSignitureToken> definedParameters = new List<ParamSignitureToken>(this.signiture.Tokens.FindAll(
				t => t.TokenType == SignitureTokens.Param
				).ConvertAll<ParamSignitureToken>(p => (ParamSignitureToken)p).ToArray());
			List<ParamDef> parameters = this.method.Parameters;

			// if a method has a return value (sequence 0) we need to miss it out as it is not in the
			// methods normal parameter list. That information is returned by GetReturnType
			bool hasReturnParam = false;
			for (int i = 0; i < parameters.Count; i++) {
				if (parameters[i].Sequence == 0) {
					hasReturnParam = true;
					continue;
				}
				details.Add(new ParameterDetails(
					parameters[i].Name,
					definedParameters[hasReturnParam ? i - 1 : i].GetTypeDetails(this.method)
					));
			}
			return details;
		}

		public MethodDef Method { get { return this.method; } }
	}
}
