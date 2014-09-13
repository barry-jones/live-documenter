using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection.Signitures {
	using TheBoxSoftware.Reflection.Core;

	/// <summary>
	/// A signiture for a method definition as described in section 23.2.1 in
	/// ECMA 335.
	/// </summary>
	internal sealed class MethodDefSigniture : Signiture {
		/// <summary>
		/// Initialises a new instance of the MethoDefSigniture class.
		/// </summary>
		/// <param name="file">The file the signiture is defined in.</param>
		/// <param name="signiture">The byte contents of the signiture.</param>
		public MethodDefSigniture(PeCoffFile file, byte[] signiture)
			: base(Signitures.MethodDef) {
			Offset offset = 0;

			CallingConventionSignitureToken callingConvention = new CallingConventionSignitureToken(signiture, offset);
			this.Tokens.Add(callingConvention);
			if ((callingConvention.Convention & CallingConventions.Generic) == CallingConventions.Generic) {
				GenericParamaterCountSignitureToken genParamCount = new GenericParamaterCountSignitureToken(signiture, offset);
				this.Tokens.Add(genParamCount);
			}
			ParameterCountSignitureToken paramCount = new ParameterCountSignitureToken(signiture, offset);
			this.Tokens.Add(paramCount);
			ReturnTypeSignitureToken returnType = new ReturnTypeSignitureToken(file, signiture, offset);
			this.Tokens.Add(returnType);
			for (int i = 0; i < paramCount.Count; i++) {
				ParamSignitureToken param = new ParamSignitureToken(file, signiture, offset);
				this.Tokens.Add(param);
			}
		}

        public CallingConventions GetCallingConvention()
        {
            return ((CallingConventionSignitureToken)this.Tokens[0]).Convention;
        }

		public static CallingConventions GetCallingConvention(PeCoffFile file, byte[] signiture) {
			return new CallingConventionSignitureToken(signiture, 0).Convention;
		}
	}
}
