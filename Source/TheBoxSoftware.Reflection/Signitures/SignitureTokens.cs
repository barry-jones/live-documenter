using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection.Signitures {
	internal enum SignitureTokens {
		CallingConvention,
		GenericParameterCount,
		ParameterCount,
		Sentinal,
		Field,
		Property,
		LocalSigniture,
		Count,
		Constraint,
		ElementType,
		TypeDefOrRefEncodedToken,
		ArrayShape,
		ReturnType,
		CustomModifier,
		Param,
		Type,
		GenericArgumentCount,
		Prolog
	}
}
