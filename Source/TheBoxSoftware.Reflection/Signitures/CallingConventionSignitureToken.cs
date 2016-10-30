
namespace TheBoxSoftware.Reflection.Signitures
{
    using System.Diagnostics;
    using Core;

    /// <summary>
    /// Represents a calling convention in a signiture.
    /// </summary>
	[DebuggerDisplay("Calling Convention: {Convention}")]
    internal sealed class CallingConventionSignitureToken : SignitureToken
    {
        private CallingConventions _convention;

        /// <summary>
        /// Initialises a new instance of CallingConventionSignitureToken from the signiture blob
        /// at the specified offset.
        /// </summary>
        /// <param name="signiture">The signiture blob to read from.</param>
        /// <param name="offset">The offset in the blob to read from.</param>
		public CallingConventionSignitureToken(byte[] signiture, Offset offset)
            : base(SignitureTokens.CallingConvention)
        {
            _convention = (CallingConventions)signiture[offset.Shift(1)];
        }

        /// <summary>
        /// Produces a string representation of the calling convention token.
        /// </summary>
        /// <returns>A string.</returns>
        public override string ToString()
        {
            return $"[CallingConvention: {_convention.ToString()}]";
        }

        /// <summary>
        /// Describes the convention used in this token.
        /// </summary>
        public CallingConventions Convention
        {
            get { return _convention; }
            private set { _convention = value; }
        }
    }
}