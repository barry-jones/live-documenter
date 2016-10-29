using System;
using System.Diagnostics;
using TheBoxSoftware.Reflection.Core;

namespace TheBoxSoftware.Reflection.Signitures
{
    [DebuggerDisplay("Count: {Count}")]
    internal sealed class CountSignitureToken : SignitureToken
    {
        private ushort _count;

        /// <summary>
        /// Initialises a new Count token from the provided <paramref name="signiture"/> at 
        /// <paramref name="offset"/>.
        /// </summary>
        /// <param name="signiture">The signiture blob.</param>
        /// <param name="offset">The offset in the signiture.</param>
		public CountSignitureToken(byte[] signiture, Offset offset)
            : base(SignitureTokens.Count)
        {
            _count = FieldReader.ToUInt16(signiture, offset.Shift(2));
        }

        public override string ToString()
        {
            return $"[Count: {_count}] ";
        }

        /// <summary>
        /// ?
        /// </summary>
		public ushort Count
        {
            get { return _count; }
            set { _count = value; }
        }
    }
}