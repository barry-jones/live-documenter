
namespace TheBoxSoftware.Reflection.Signatures
{
    using System;
    using Core;

    /// <summary>
    /// The signiture for a custom attribute as described in section 23.3 of ECMA 335.
    /// </summary>
    internal sealed class CustomAttributeSignature : Signature
    {
        /// <summary>
        /// Initialises a new instance of the CustomAttributeSigniture class.
        /// </summary>
        /// <param name="signiture">The byte contents of the signiture.</param>
        /// <exception cref="InvalidOperationException">
        /// Thrown when a value for the prolog differs from 0x0001. This indicates
        /// the incorrect signiture type is being read or the signiture contents
        /// are invalid.
        /// </exception>
        public CustomAttributeSignature(byte[] signiture) : base(Signatures.CustomAttribute)
        {
            Offset offset = 0;

            // Prolog (0x00001) always and only one instance
            PrologSignatureToken prolog = new PrologSignatureToken(signiture, offset);
            Tokens.Add(prolog);
            
            // TODO: Incomplete
            //  Fixed arguments
            //  Num named arguments
            //  Named arguments
        }
    }
}