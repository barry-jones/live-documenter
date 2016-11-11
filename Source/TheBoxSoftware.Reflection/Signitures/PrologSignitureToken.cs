
namespace TheBoxSoftware.Reflection.Signitures
{
    using Core;

    /// <summary>
    /// A class which represents a prolog for a <see cref="CustomAttributeSignature"/>.
    /// </summary>
    internal sealed class PrologSignitureToken : SignitureToken
    {
        private ushort _value;

        /// <summary>
        /// Initialises a new instance of the PrologSignitureToken class.
        /// </summary>
        /// <param name="signiture">The byte contents of the signiture.</param>
        /// <param name="offset">The start offset of the this token.</param>
        public PrologSignitureToken(byte[] signiture, Offset offset)
            : base(SignitureTokens.Prolog)
        {
            _value = FieldReader.ToUInt16(signiture, offset.Shift(2));
        }

        /// <summary>
        /// Produces a string representation of the prolog token.
        /// </summary>
        /// <returns>A string.</returns>
        public override string ToString()
        {
            return $"[Prolog: {_value}]";
        }

        /// <summary>
        /// The value of the token.
        /// </summary>
        public ushort Value
        {
            get { return _value; }
            private set { _value = value; }
        }
    }
}