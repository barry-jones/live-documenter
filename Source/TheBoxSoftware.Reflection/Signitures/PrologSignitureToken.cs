using System;
using TheBoxSoftware.Reflection.Core;

namespace TheBoxSoftware.Reflection.Signitures
{
    /// <summary>
    /// A class which represents a prolog for a <see cref="CustomAttributeSigniture"/>.
    /// </summary>
    internal sealed class PrologSignitureToken : SignitureToken
    {
        private UInt16 _value;

        /// <summary>
        /// Initialises a new instance of the PrologSignitureToken class.
        /// </summary>
        /// <param name="signiture">The byte contents of the signiture.</param>
        /// <param name="offset">The start offset of the this token.</param>
        public PrologSignitureToken(byte[] signiture, Offset offset)
            : base(SignitureTokens.Prolog)
        {
            this.Value = FieldReader.ToUInt16(signiture, offset.Shift(2));
        }

        /// <summary>
        /// Produces a string representation of the prolog token.
        /// </summary>
        /// <returns>A string.</returns>
        public override string ToString()
        {
            return string.Format("[Prolog: {0}]", this.Value);
        }

        /// <summary>
        /// The value of the token.
        /// </summary>
        public UInt16 Value
        {
            get { return this._value; }
            private set { this._value = value; }
        }
    }
}