using System;
using TheBoxSoftware.Reflection.Core;

namespace TheBoxSoftware.Reflection.Signitures
{
    /// <summary>
    /// The signiture for a custom attribute as described in section 23.3 of
    /// ECMA 335.
    /// </summary>
    internal sealed class CustomAttributeSigniture : Signiture
    {
        /// <summary>
        /// Initialises a new instance of the CustomAttributeSigniture class.
        /// </summary>
        /// <param name="file">The file the signiture is defined in.</param>
        /// <param name="signiture">The byte contents of the signiture.</param>
        /// <exception cref="InvalidOperationException">
        /// Thrown when a value for the prolog differs from 0x0001. This indicates
        /// the incorrect signiture type is being read or the signiture contents
        /// are invalid.
        /// </exception>
        public CustomAttributeSigniture(PeCoffFile file, byte[] signiture)
            : base(Signitures.CustomAttribute)
        {
            Offset offset = 0;

            // Prolog
            PrologSignitureToken prolog = new PrologSignitureToken(signiture, offset);
            if(prolog.Value != 0x0001)
            {
                InvalidOperationException ex = new InvalidOperationException(
                    "The CustomAttribute signiture happened upon an unexpected prolog value"
                    );
                throw ex;
            }
            this.Tokens.Add(prolog);

            // Fixed arguments
            // Num named arguments
            // Named arguments
        }
    }
}