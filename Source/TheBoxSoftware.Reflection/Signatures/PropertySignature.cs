
namespace TheBoxSoftware.Reflection.Signatures
{
    using System.Text;
    using Core;

    /// <summary>
    /// A rerepresentation of the property signiture as described in ECMA 23.2.5.
    /// </summary>
	internal sealed class PropertySignature : Signature
    {
        /// <summary>
        /// Initialises a new instance of the property signiture from the provided <paramref name="signiture"/>.
        /// </summary>
        /// <param name="signiture">The signiture blob.</param>
		public PropertySignature(byte[] signiture)
            : base(Signatures.Property)
        {
            Offset offset = 0;

            ElementTypeSignatureToken property = new ElementTypeSignatureToken(signiture, offset);
            Tokens.Add(property);

            ParameterCountSignatureToken paramCount = new ParameterCountSignatureToken(signiture, offset);
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
                ParamSignatureToken param = new ParamSignatureToken(signiture, offset);
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

            foreach(SignatureToken t in Tokens)
            {
                sb.Append(t.ToString());
            }

            sb.Append("]");

            return sb.ToString();
        }
    }
}