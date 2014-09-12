using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheBoxSoftware.Reflection.Core;

namespace TheBoxSoftware.Reflection.Signitures 
{
    /// <summary>
    /// A representation of a ParameterCount token in a signiture.
    /// </summary>
	[DebuggerDisplay("Parameter Count: {Count}")]
	internal sealed class ParameterCountSignitureToken : SignitureToken
    {
        // 4 bytes
        private int count;

        /// <summary>
        /// Initialises a new paramater count token from the provided <paramref name="signiture"/>
        /// at the specified <paramref name="offset"/>.
        /// </summary>
        /// <param name="signiture">The signiture blob.</param>
        /// <param name="offset">The offset in the signiture.</param>
		public ParameterCountSignitureToken(byte[] signiture, Offset offset)
			: base(SignitureTokens.ParameterCount) 
        {
			this.Count = SignitureToken.GetCompressedValue(signiture, offset);
		}

        /// <summary>
        /// Produces a string representation of the parameter count token.
        /// </summary>
        /// <returns>A string.</returns>
        public override string ToString()
        {
            return string.Format("[ParamCount: {0}]", this.Count);
        }

        /// <summary>
        /// The number of parameters.
        /// </summary>
        public int Count
        {
            get { return this.count; }
            private set { this.count = value; }
        }
	}
}
