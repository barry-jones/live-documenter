using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection.Signitures {
	using TheBoxSoftware.Reflection.Core;

	[DebuggerDisplay("Generic Parameter Count: {Count}")]
	public sealed class GenericParamaterCountSignitureToken : SignitureToken {
		public GenericParamaterCountSignitureToken(int count)
			: base(SignitureTokens.GenericParameterCount) {
			this.Count = count;
		}

		public GenericParamaterCountSignitureToken(byte[] signiture, Offset offset) 
			: base(SignitureTokens.GenericParameterCount) {
			this.Count = SignitureToken.GetCompressedValue(signiture, offset);
		}

		/// <summary>
		/// The number of generic parameters in this signiture.
		/// </summary>
		public int Count { get; set; }
	}
}
