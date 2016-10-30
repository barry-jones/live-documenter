
namespace TheBoxSoftware.Reflection.Signitures
{
    using System.Diagnostics;
    using Core;

    /// <summary>
    /// An Int32 numeric value that indicates the number of generics arguments in a signiture.
    /// </summary>
	[DebuggerDisplay("Generic Argument Count: {Count}")]
    internal sealed class GenericArgumentCountSignitureToken : SignitureToken
    {
        private int _count;

        /// <summary>
        /// Initiailses a GenericArgumentCount token from the <paramref name="signiture"/> at the
        /// specified <paramref name="offset"/>.
        /// </summary>
        /// <param name="signiture">The signiture blob.</param>
        /// <param name="offset">The offset  in the signiture blob.</param>
		public GenericArgumentCountSignitureToken(byte[] signiture, Offset offset)
            : base(SignitureTokens.GenericArgumentCount)
        {
            _count = GetCompressedValue(signiture, offset);
        }

        /// <summary>
        /// Produces a string representation of this generic argument count token.
        /// </summary>
        /// <returns>A string.</returns>
        public override string ToString()
        {
            return $"[GenericArgumentCount: {_count}] ";
        }

        /// <summary>
        /// The number of generic arguments.
        /// </summary>
        public int Count
        {
            get { return _count; }
            private set { _count = value; }
        }
    }
}