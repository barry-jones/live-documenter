using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheBoxSoftware.Reflection.Core;

namespace TheBoxSoftware.Reflection.Signitures 
{
    /// <summary>
    /// Represents a local variable signiture in the signiture blob. Details of the signiture are
    /// available in the ECMA at 23.2.6.
    /// </summary>
	internal sealed class LocalVariableSigniture : Signiture
    {
        /// <summary>
        /// Initialise a new instance of a local variable signiture from the <paramref name="signiture"/>
        /// provided.
        /// </summary>
        /// <param name="file">The PeCoffFile that contains the signiture block.</param>
        /// <param name="signiture">The signiture blob.</param>
		internal LocalVariableSigniture(PeCoffFile file, byte[] signiture)
			: base(Signitures.LocalVariable) 
        {
			List<SignitureToken> tokens = new List<SignitureToken>();
			Offset offset = 0;

			offset.Shift(1);	// jump passed the 0x7 indicator

			CountSignitureToken count = new CountSignitureToken(signiture, offset);
			tokens.Add(count);

			for (int i = 0; i < count.Count; i++)
            {
				if (ElementTypeSignitureToken.IsToken(signiture, offset, ElementTypes.TypedByRef))
                {
					ElementTypeSignitureToken typedByRef = new ElementTypeSignitureToken(file, signiture, offset);
					tokens.Add(typedByRef);
				}
				else 
                {
					while (CustomModifierToken.IsToken(signiture, offset) || ConstraintSignitureToken.IsToken(signiture, offset)) 
                    {
						if (CustomModifierToken.IsToken(signiture, offset))
                        {
							CustomModifierToken modifier = new CustomModifierToken(signiture, offset);
							tokens.Add(modifier);
						}
						else 
                        {
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

        /// <summary>
        /// Produces a string representation of the local variable signiture.
        /// </summary>
        /// <returns>A string.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("[LocalVar: ");

            foreach (SignitureToken t in this.Tokens)
            {
                sb.Append(t.ToString());
            }

            sb.Append("] ");

            return sb.ToString();
        }
	}
}
