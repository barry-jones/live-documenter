using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheBoxSoftware.Reflection.Core;

namespace TheBoxSoftware.Reflection.Signitures 
{
    /// <summary>
    /// ?
    /// </summary>
	[DebuggerDisplay("Count: {Count}")]
	internal sealed class CountSignitureToken : SignitureToken 
    {
        // 4 bytes
        private UInt16 count;

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
            get { return this.count; }
            set { this.count = value; }
        }
	}
}
