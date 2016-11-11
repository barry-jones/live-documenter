
namespace TheBoxSoftware.Reflection.Signitures
{
    using System.Collections.Generic;
    using System.Text;
    using Core;

    /// <summary>
    /// Describes a field signiture, which is specified in the ECMA 23.2.4.
    /// </summary>
	internal sealed class FieldSignature : Signature
    {
        /// <summary>
        /// Initialises a field signiture from the specified <paramref name="signiture"/> blob.
        /// </summary>
        /// <param name="signiture">The signiture blob.</param>
		public FieldSignature(byte[] signiture)
            : base(Signatures.Field)
        {
            Offset offset = 0;

            if(signiture[offset] != 0x06)
            {
                // 0x06 defines the Calling convention FIELD
            }
            offset.Shift(1);

            while(offset < signiture.Length)
            {
                while(CustomModifierToken.IsToken(signiture, offset))
                {
                    CustomModifierToken modifier = new CustomModifierToken(signiture, offset);
                    Tokens.Add(modifier);
                }

                TypeSignatureToken type = new TypeSignatureToken(signiture, offset);
                Tokens.Add(type);
            }
        }

        /// <summary>
        /// Produces a string representation of the field signiture.
        /// </summary>
        /// <returns>A string.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("[Field: ");

            foreach(SignatureToken t in Tokens)
            {
                sb.Append(t.ToString());
            }

            sb.Append("] ");

            return sb.ToString();
        }
    }
}