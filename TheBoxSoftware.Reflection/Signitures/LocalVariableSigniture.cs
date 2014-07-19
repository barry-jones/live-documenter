using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection.Signitures {
	using TheBoxSoftware.Reflection.Core;

	internal sealed class LocalVariableSigniture : Signiture {
		internal LocalVariableSigniture(PeCoffFile file, byte[] signiture)
			: base(Signitures.LocalVariable) {
			List<SignitureToken> tokens = new List<SignitureToken>();
			Offset offset = 0;

			offset.Shift(1);	// jump passed the 0x7 indicator
			CountSignitureToken count = new CountSignitureToken(signiture, offset);
			tokens.Add(count);
			for (int i = 0; i < count.Count; i++) {
				if (ElementTypeSignitureToken.IsToken(signiture, offset, ElementTypes.TypedByRef)) {
					ElementTypeSignitureToken typedByRef = new ElementTypeSignitureToken(file, signiture, offset);
					tokens.Add(typedByRef);
				}
				else {
					while (CustomModifierToken.IsToken(signiture, offset) || ConstraintSignitureToken.IsToken(signiture, offset)) {
						if (CustomModifierToken.IsToken(signiture, offset)) {
							CustomModifierToken modifier = new CustomModifierToken(signiture, offset);
							tokens.Add(modifier);
						}
						else {
							ConstraintSignitureToken constraint = new ConstraintSignitureToken(signiture, offset);
							tokens.Add(constraint);
						}
					}
					ElementTypeSignitureToken byRef = new ElementTypeSignitureToken(file, signiture, offset);
					tokens.Add(byRef);
					ElementTypeSignitureToken type = new ElementTypeSignitureToken(file, signiture, offset);
					tokens.Add(type);
				}
			}
		}
	}
}
