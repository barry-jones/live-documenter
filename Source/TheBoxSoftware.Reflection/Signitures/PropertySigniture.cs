using System.Collections.Generic;
using System.Text;
using TheBoxSoftware.Reflection.Core;

namespace TheBoxSoftware.Reflection.Signitures
{
    /// <summary>
    /// A rerepresentation of the property signiture as described in ECMA 23.2.5.
    /// </summary>
	internal sealed class PropertySigniture : Signiture
    {
        /// <summary>
        /// Initialises a new instance of the property signiture from the provided <paramref name="signiture"/>.
        /// </summary>
        /// <param name="signiture">The signiture blob.</param>
		public PropertySigniture(byte[] signiture)
            : base(Signitures.Property)
        {
            List<SignitureToken> tokens = new List<SignitureToken>();
            Offset offset = 0;

            ElementTypeSignitureToken property = new ElementTypeSignitureToken(signiture, offset);
            tokens.Add(property);

            ParameterCountSignitureToken paramCount = new ParameterCountSignitureToken(signiture, offset);
            tokens.Add(paramCount);

            while(CustomModifierToken.IsToken(signiture, offset))
            {
                CustomModifierToken modifier = new CustomModifierToken(signiture, offset);
                tokens.Add(modifier);
            }

            ElementTypeSignitureToken type = new ElementTypeSignitureToken(signiture, offset);
            tokens.Add(type);

            for(int i = 0; i < paramCount.Count; i++)
            {
                ParamSignitureToken param = new ParamSignitureToken(signiture, offset);
                tokens.Add(param);
            }
        }

        /// <summary>
        /// Produces a string representaion of the property signiture.
        /// </summary>
        /// <returns>A string.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("[Property: ");

            foreach(SignitureToken t in this.Tokens)
            {
                sb.Append(t.ToString());
            }

            sb.Append("]");

            return sb.ToString();
        }
    }
}