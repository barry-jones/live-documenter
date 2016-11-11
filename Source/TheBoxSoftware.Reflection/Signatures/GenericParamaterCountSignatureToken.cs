
namespace TheBoxSoftware.Reflection.Signatures
{
    using System.Diagnostics;
    using Core;

    /// <summary>
    /// A token that represents the number of generic parameters.
    /// </summary>
	[DebuggerDisplay("Generic Parameter Count: {Count}")]
    internal sealed class GenericParamaterCountSignatureToken : SignatureToken
    {
        private uint _count;

        /// <summary>
        /// Initialises a GenericParameterCount token from the <paramref name="signiture"/> at
        /// the specified <paramref name="offset"/>.
        /// </summary>
        /// <param name="signiture">The signiture blob.</param>
        /// <param name="offset">The offset in the signiture.</param>
		public GenericParamaterCountSignatureToken(byte[] signiture, Offset offset)
            : base(SignatureTokens.GenericParameterCount)
        {
            _count = GetCompressedValue(signiture, offset);
        }

        /// <summary>
        /// Produces a string representation of the generic parameter count token.
        /// </summary>
        /// <returns>A string</returns>
        public override string ToString()
        {
            return $"[GenParamCount: {_count}]";
        }
    }
}