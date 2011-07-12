using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection.Signitures {
	using TheBoxSoftware.Reflection.Core;
	using TheBoxSoftware.Reflection.Core.COFF;

	[DebuggerDisplay("Modifier: {Modifier} for [{CodedIndex.ToString()}]")]
	public sealed class CustomModifierToken : SignitureToken {
		public CustomModifierToken(byte[] signiture, Offset offset)
			: base(SignitureTokens.CustomModifier) {
			this.Modifier = (ElementTypes)SignitureToken.GetCompressedValue(signiture, offset);
			this.Index = this.ReadEncodedDefOrRefToken(signiture, offset);
		}

		public ElementTypes Modifier { get; set; }
		public CodedIndex Index { get; set; }

		private CodedIndex ReadEncodedDefOrRefToken(byte[] signiture, Offset offset) {
			// Read the typedef, typeref or typespec token
			int typeMask = 0x00000003;
			int token = SignitureToken.GetCompressedValue(signiture, offset);
			
			// Resolved values
			MetadataTables table;
			int index = token >> 2;

			switch (typeMask & token) {
				case 0: // TypeDef
					table = MetadataTables.TypeDef;
					break;
				case 1:	// TypeRef
					table = MetadataTables.TypeRef;
					break;
				case 2:	// TypeSpec
					table = MetadataTables.TypeSpec;
					break;
				default:
					throw new InvalidOperationException("Metadata Table could not be resolved for this Signiture");
			}
			return new CodedIndex(table, (uint)index);
		}

		/// <summary>
		/// Checks if the next token at the current offset can potentially be a
		/// CustomModifierToken.
		/// </summary>
		/// <param name="signiture">The signiture to preview</param>
		/// <param name="offset">The current offset in the signiture</param>
		/// <returns></returns>
		public static bool IsToken(byte[] signiture, int offset) {
			ElementTypes modifier = (ElementTypes)SignitureToken.GetCompressedValue(signiture, offset);
			return modifier == ElementTypes.CModOptional || modifier == ElementTypes.CModRequired;
		}
	}
}
