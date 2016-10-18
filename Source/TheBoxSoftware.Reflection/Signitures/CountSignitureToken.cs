using System;
using System.Diagnostics;
using TheBoxSoftware.Reflection.Core;

namespace TheBoxSoftware.Reflection.Signitures
{
    [DebuggerDisplay("Count: {Count}")]
    internal sealed class CountSignitureToken : SignitureToken
    {
        private UInt16 _count;

        /// <summary>
        /// Initialises a new Count token from the provided <paramref name="signiture"/> at 
        /// <paramref name="offset"/>.
        /// </summary>
        /// <param name="signiture">The signiture blob.</param>
        /// <param name="offset">The offset in the signiture.</param>
		public CountSignitureToken(byte[] signiture, Offset offset)
            : base(SignitureTokens.Count)
        {
            this.Count = FieldReader.ToUInt16(signiture, offset.Shift(2));
        }

        public override string ToString()
        {
            return string.Format("[Count: {0}] ", this.Count);
        }

        /// <summary>
        /// ?
        /// </summary>
		public UInt16 Count
        {
            get { return this._count; }
            set { this._count = value; }
        }
    }
}