
namespace TheBoxSoftware.Reflection.Signitures
{
    using System.Collections.Generic;
    using System.Text;
    using Core;

    /// <summary>
    /// Describes a field signiture, which is specified in the ECMA 23.2.4.
    /// </summary>
	internal sealed class FieldSigniture : Signiture
    {
        /// <summary>
        /// Initialises a field signiture from the specified <paramref name="signiture"/> blob.
        /// </summary>
        /// <param name="signiture">The signiture blob.</param>
		public FieldSigniture(byte[] signiture)
            : base(Signitures.Field)
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

                TypeSignitureToken type = new TypeSignitureToken(signiture, offset);
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

            foreach(SignitureToken t in Tokens)
            {
                sb.Append(t.ToString());
            }

            sb.Append("] ");

            return sb.ToString();
        }
    }
}