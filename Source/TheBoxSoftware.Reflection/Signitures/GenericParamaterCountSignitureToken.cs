
namespace TheBoxSoftware.Reflection.Signitures
{
    using System.Diagnostics;
    using TheBoxSoftware.Reflection.Core;

    /// <summary>
    /// A token that represents the number of generic parameters.
    /// </summary>
	[DebuggerDisplay("Generic Parameter Count: {Count}")]
    internal sealed class GenericParamaterCountSignitureToken : SignitureToken
    {
        private int _count;

        /// <summary>
        /// Initialises a GenericParameterCount token from the <paramref name="signiture"/> at
        /// the specified <paramref name="offset"/>.
        /// </summary>
        /// <param name="signiture">The signiture blob.</param>
        /// <param name="offset">The offset in the signiture.</param>
		public GenericParamaterCountSignitureToken(byte[] signiture, Offset offset)
            : base(SignitureTokens.GenericParameterCount)
        {
            Count = GetCompressedValue(signiture, offset);
        }

        /// <summary>
        /// Produces a string representation of the generic parameter count token.
        /// </summary>
        /// <returns>A string</returns>
        public override string ToString()
        {
            return $"[GenParamCount: {Count}]";
        }

        /// <summary>
        /// The number of generic parameters in this signiture.
        /// </summary>
        public int Count
        {
            get { return _count; }
            private set { _count = value; }
        }
    }
}