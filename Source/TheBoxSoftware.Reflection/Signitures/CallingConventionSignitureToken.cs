using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection.Signitures 
{
	using TheBoxSoftware.Reflection.Core;

    /// <summary>
    /// Represents a calling convention in a signiture.
    /// </summary>
	[DebuggerDisplay("Calling Convention: {Convention}")]
	internal sealed class CallingConventionSignitureToken : SignitureToken 
    {
        // 4 bytes
        private CallingConventions convention;

        /// <summary>
        /// Initialises a new instance of CallingConventionSignitureToken from the signiture blob
        /// at the specified offset.
        /// </summary>
        /// <param name="signiture">The signiture blob to read from.</param>
        /// <param name="offset">The offset in the blob to read from.</param>
		public CallingConventionSignitureToken(byte[] signiture, Offset offset)
			: base(SignitureTokens.CallingConvention) 
        {
			this.Convention = (CallingConventions)signiture[offset.Shift(1)];
		}

        /// <summary>
        /// Produces a string representation of the calling convention token.
        /// </summary>
        /// <returns>A string.</returns>
        public override string ToString()
        {
            return string.Format("[CallingConvention: {0}", this.Convention.ToString());
        }

		/// <summary>
		/// Describes the convention used in this token.
		/// </summary>
		public CallingConventions Convention 
        {
            get { return this.convention; }
            private set { this.convention = value; }
        }
	}
}
