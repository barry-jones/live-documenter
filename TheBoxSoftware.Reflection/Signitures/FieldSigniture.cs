using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheBoxSoftware.Reflection.Core;

namespace TheBoxSoftware.Reflection.Signitures 
{
    /// <summary>
    /// Describes a field signiture, which is specified in the ECMA 23.2.4.
    /// </summary>
	internal sealed class FieldSigniture : Signiture 
    {
        /// <summary>
        /// Initialises a field signiture from the specified <paramref name="signiture"/> blob.
        /// </summary>
        /// <param name="file">The PeCoffFile that contains the definition.</param>
        /// <param name="signiture">The signiture blob.</param>
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


        /// <summary>
        /// Produces a string representation of the field signiture.
        /// </summary>
        /// <returns>A string.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("[Field: ");

            foreach (SignitureToken t in this.Tokens)
            {
                sb.Append(t.ToString());
            }

            sb.Append("] ");

            return sb.ToString();
        }
	}
}
