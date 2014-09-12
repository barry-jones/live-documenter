using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheBoxSoftware.Reflection.Core;

namespace TheBoxSoftware.Reflection.Signitures {

	internal class ArrayShapeSignitureToken : SignitureToken {
		public ArrayShapeSignitureToken(byte[] signiture, Offset offset)
			: base(SignitureTokens.ArrayShape) {
			this.Rank = SignitureToken.GetCompressedValue(signiture, offset);
			this.NumSizes = SignitureToken.GetCompressedValue(signiture, offset);
			this.Sizes = new Int32[this.NumSizes];
			for (int i = 0; i < this.NumSizes; i++) {
				this.Sizes[i] = SignitureToken.GetCompressedValue(signiture, offset);
			}
			this.NumLoBounds = SignitureToken.GetCompressedValue(signiture, offset);
			this.LoBounds = new Int32[this.NumLoBounds];
			for (int i = 0; i < this.NumLoBounds; i++) {
				this.LoBounds[i] = SignitureToken.GetCompressedValue(signiture, offset);
			}
		}

		public Int32 Rank { get; set; }
		public Int32 NumSizes { get; set; }
		public Int32[] Sizes { get; set; }
		public Int32 NumLoBounds { get; set; }
		public Int32[] LoBounds { get; set; }
	}
}
