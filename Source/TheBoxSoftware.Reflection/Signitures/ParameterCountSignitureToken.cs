
namespace TheBoxSoftware.Reflection.Signitures
{
    using System.Diagnostics;
    using Core;

    /// <summary>
    /// A representation of a ParameterCount token in a signiture.
    /// </summary>
	[DebuggerDisplay("Parameter Count: {Count}")]
    internal sealed class ParameterCountSignitureToken : SignitureToken
    {
        private uint _count;

        /// <summary>
        /// Initialises a new paramater count token from the provided <paramref name="signiture"/>
        /// at the specified <paramref name="offset"/>.
        /// </summary>
        /// <param name="signiture">The signiture blob.</param>
        /// <param name="offset">The offset in the signiture.</param>
		public ParameterCountSignitureToken(byte[] signiture, Offset offset)
            : base(SignitureTokens.ParameterCount)
        {
            _count = GetCompressedValue(signiture, offset);
        }

        /// <summary>
        /// Produces a string representation of the parameter count token.
        /// </summary>
        /// <returns>A string.</returns>
        public override string ToString()
        {
            return $"[ParamCount: {_count}]";
        }

        /// <summary>
        /// The number of parameters.
        /// </summary>
        public uint Count
        {
            get { return _count; }
            private set { _count = value; }
        }
    }
}