
namespace TheBoxSoftware.Reflection.Signitures
{
    using System.Text;
    using Core;

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
            Offset offset = 0;

            ElementTypeSignatureToken property = new ElementTypeSignatureToken(signiture, offset);
            Tokens.Add(property);

            ParameterCountSignitureToken paramCount = new ParameterCountSignitureToken(signiture, offset);
            Tokens.Add(paramCount);

            while(CustomModifierToken.IsToken(signiture, offset))
            {
                CustomModifierToken modifier = new CustomModifierToken(signiture, offset);
                Tokens.Add(modifier);
            }

            ElementTypeSignatureToken type = new ElementTypeSignatureToken(signiture, offset);
            Tokens.Add(type);

            for(int i = 0; i < paramCount.Count; i++)
            {
                ParamSignitureToken param = new ParamSignitureToken(signiture, offset);
                Tokens.Add(param);
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

            foreach(SignitureToken t in Tokens)
            {
                sb.Append(t.ToString());
            }

            sb.Append("]");

            return sb.ToString();
        }
    }
}