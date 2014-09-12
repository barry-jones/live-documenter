using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheBoxSoftware.Reflection.Core;

namespace TheBoxSoftware.Reflection.Signitures 
{
	internal sealed class FieldSigniture : Signiture 
    {
		public FieldSigniture(PeCoffFile file, byte[] signiture)
			: base(Signitures.Field) 
        {
			List<SignitureToken> tokens = new List<SignitureToken>();
			Offset offset = 0;

			if (signiture[offset] != 0x06) { // ?? whats this for
			}
			offset.Shift(1);

			while (offset < signiture.Length) 
            {
				while (CustomModifierToken.IsToken(signiture, offset)) 
                {
					CustomModifierToken modifier = new CustomModifierToken(signiture, offset);
					tokens.Add(modifier);
				}
				TypeSignitureToken type = new TypeSignitureToken(file, signiture, offset);
				tokens.Add(type);
			}
			this.Tokens = tokens;
		}
	}
}
