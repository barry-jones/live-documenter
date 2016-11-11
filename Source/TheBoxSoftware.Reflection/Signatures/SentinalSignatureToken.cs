
namespace TheBoxSoftware.Reflection.Signatures
{
    using System.Diagnostics;
    using Core;

    /// <summary>
    /// Represents a sentinal signiture token.
    /// </summary>
	[DebuggerDisplay("Sentinal")]
    internal sealed class SentinalSignatureToken : SignatureToken
    {
        /// <summary>
        /// Initialises a new instance of the sentinal signiture from the provided <paramref name="signiture"/>
        /// at the specified <paramref name="offset"/>.
        /// </summary>
        /// <param name="signiture">The signiture blob.</param>
        /// <param name="offset">The offset in the signiture.</param>
		public SentinalSignatureToken(byte[] signiture, Offset offset)
            : base(SignatureTokens.Sentinal)
        {
            ElementTypes value = (ElementTypes)GetCompressedValue(signiture, offset);
            offset.Shift(1);    // No work to do here we are just a placeholder
        }

        public static bool IsToken(byte[] signiture, int offset)
        {
            ElementTypes value = (ElementTypes)GetCompressedValue(signiture, offset);
            return (value & ElementTypes.Sentinal) != 0;
        }

        /// <summary>
        /// Produces a string representation of the sentinal signiture token.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "[Sentinal] ";
        }
    }
}