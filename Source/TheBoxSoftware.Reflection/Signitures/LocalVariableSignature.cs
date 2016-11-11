
namespace TheBoxSoftware.Reflection.Signitures
{
    using System.Text;
    using Core;

    /// <summary>
    /// Represents a local variable signiture in the signiture blob. Details of the signiture are
    /// available in the ECMA at 23.2.6.
    /// </summary>
	internal sealed class LocalVariableSignature : Signature
    {
        /// <summary>
        /// Initialise a new instance of a local variable signiture from the <paramref name="signiture"/>
        /// provided.
        /// </summary>
        /// <param name="signiture">The signiture blob.</param>
		internal LocalVariableSignature(byte[] signiture) : base(Signatures.LocalVariable)
        {
            Offset offset = 0;

            offset.Shift(1);    // jump passed the 0x7 indicator

            CountSignatureToken count = new CountSignatureToken(signiture, offset);
            Tokens.Add(count);

            for(int i = 0; i < count.Count; i++)
            {
                if(ElementTypeSignatureToken.IsToken(signiture, offset, ElementTypes.TypedByRef))
                {
                    ElementTypeSignatureToken typedByRef = new ElementTypeSignatureToken(signiture, offset);
                    Tokens.Add(typedByRef);
                }
                else
                {
                    while(CustomModifierToken.IsToken(signiture, offset) || ConstraintSignatureToken.IsToken(signiture, offset))
                    {
                        if(CustomModifierToken.IsToken(signiture, offset))
                        {
                            CustomModifierToken modifier = new CustomModifierToken(signiture, offset);
                            Tokens.Add(modifier);
                        }
                        else
                        {
                            ConstraintSignatureToken constraint = new ConstraintSignatureToken(signiture, offset);
                            Tokens.Add(constraint);
                        }
                    }

                    ElementTypeSignatureToken byRef = new ElementTypeSignatureToken(signiture, offset);
                    Tokens.Add(byRef);

                    ElementTypeSignatureToken type = new ElementTypeSignatureToken(signiture, offset);
                    Tokens.Add(type);
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

            foreach(SignatureToken t in Tokens)
            {
                sb.Append(t.ToString());
            }

            sb.Append("] ");

            return sb.ToString();
        }
    }
}