using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection.Signitures {
	using TheBoxSoftware.Reflection.Core;

	public sealed class PropertySigniture : Signiture {
		public PropertySigniture(PeCoffFile file, byte[] signiture)
			: base(Signitures.Property) {
			List<SignitureToken> tokens = new List<SignitureToken>();
			Offset offset = 0;

			ElementTypeSignitureToken property = new ElementTypeSignitureToken(file, signiture, offset);
			tokens.Add(property);
			ParameterCountSignitureToken paramCount = new ParameterCountSignitureToken(signiture, offset);
			tokens.Add(paramCount);
			while (CustomModifierToken.IsToken(signiture, offset)) {
				CustomModifierToken modifier = new CustomModifierToken(signiture, offset);
				tokens.Add(modifier);
			}
			ElementTypeSignitureToken type = new ElementTypeSignitureToken(file, signiture, offset);
			tokens.Add(type);
			for (int i = 0; i < paramCount.Count; i++) {
				ParamSignitureToken param = new ParamSignitureToken(file, signiture, offset);
				tokens.Add(param);
			}
		}
	}
}
