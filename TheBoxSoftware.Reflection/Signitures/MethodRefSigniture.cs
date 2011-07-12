using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection.Signitures {
	using TheBoxSoftware.Reflection.Core;

	public sealed class MethodRefSigniture : Signiture {
		public MethodRefSigniture(PeCoffFile file, byte[] signiture)
			: base(Signitures.MethodRef) {
			List<SignitureToken> tokens = new List<SignitureToken>();
			Offset offset = 0;

			CallingConventionSignitureToken callingConvention = new CallingConventionSignitureToken(signiture, offset);
			tokens.Add(callingConvention);
			if ((callingConvention.Convention & CallingConventions.Generic) == CallingConventions.Generic) {
				GenericParamaterCountSignitureToken genParamCount = new GenericParamaterCountSignitureToken(signiture, offset);
				tokens.Add(genParamCount);
			}
			ParameterCountSignitureToken paramCount = new ParameterCountSignitureToken(signiture, offset);
			tokens.Add(paramCount);
			ReturnTypeSignitureToken returnType = new ReturnTypeSignitureToken(file, signiture, offset);
			tokens.Add(returnType);
			for (int i = 0; i < paramCount.Count; i++) {
				if (SentinalSignitureToken.IsToken(signiture, offset)) {
					i--;	// This is not a parameter
					tokens.Add(new SentinalSignitureToken(signiture, offset));
				}
				else {
					ParamSignitureToken param = new ParamSignitureToken(file, signiture, offset);
					tokens.Add(param);
				}
			}
		}
	}
}
