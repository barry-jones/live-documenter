using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheBoxSoftware.Reflection.Core;

namespace TheBoxSoftware.Reflection.Signitures 
{
    /// <summary>
    /// A token that represents the number of generic parameters.
    /// </summary>
	[DebuggerDisplay("Generic Parameter Count: {Count}")]
	internal sealed class GenericParamaterCountSignitureToken : SignitureToken 
    {
        // 4 bytes
        private int count;

        /// <summary>
        /// Initialises a GenericParameterCount token from the <paramref name="signiture"/> at
        /// the specified <paramref name="offset"/>.
        /// </summary>
        /// <param name="signiture">The signiture blob.</param>
        /// <param name="offset">The offset in the signiture.</param>
		public GenericParamaterCountSignitureToken(byte[] signiture, Offset offset) 
			: base(SignitureTokens.GenericParameterCount) 
        {
			this.Count = SignitureToken.GetCompressedValue(signiture, offset);
		}

        /// <summary>
        /// Produces a string representation of the generic parameter count token.
        /// </summary>
        /// <returns>A string</returns>
        public override string ToString()
        {
            return string.Format("[GenParamCount: {0}]", this.Count);
        }

		/// <summary>
		/// The number of generic parameters in this signiture.
		/// </summary>
		public int Count
        {
            get { return this.count; }
            private set { this.count = value; }
        }
	}
}
