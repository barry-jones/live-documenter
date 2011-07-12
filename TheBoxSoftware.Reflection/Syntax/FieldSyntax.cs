using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection.Syntax {
	public class FieldSyntax : Syntax {
		private FieldDef field;

		public FieldSyntax(FieldDef field) {
			this.field = field;
		}

		public Visibility GetVisibility() {
			switch (this.field.Flags & TheBoxSoftware.Reflection.Core.COFF.FieldAttributes.FieldAccessMask) {
				case TheBoxSoftware.Reflection.Core.COFF.FieldAttributes.Public:
					return Visibility.Public;
				case TheBoxSoftware.Reflection.Core.COFF.FieldAttributes.Assembly:
					return Visibility.Internal;
				case TheBoxSoftware.Reflection.Core.COFF.FieldAttributes.FamANDAssem:
					return Visibility.InternalProtected;
				case TheBoxSoftware.Reflection.Core.COFF.FieldAttributes.Family:
					return Visibility.Protected;
				case TheBoxSoftware.Reflection.Core.COFF.FieldAttributes.Private:
					return Visibility.Private;
				case TheBoxSoftware.Reflection.Core.COFF.FieldAttributes.FamORAssem:
					return Visibility.Internal;
				default:
					return Visibility.Internal;
			}
		}

		/// <summary>
		/// Obtains the name of type which this field is.
		/// </summary>
		/// <returns></returns>
		public TypeRef GetType() {
			TypeRef returnType = null;

			Signitures.Signiture signiture = this.field.Signiture;
			Signitures.SignitureToken token = signiture.Tokens.Find(
				t => t.TokenType == TheBoxSoftware.Reflection.Signitures.SignitureTokens.ElementType ||
					t.TokenType == TheBoxSoftware.Reflection.Signitures.SignitureTokens.Type
				);
			if (token != null) {
				TypeRef type = (token is Signitures.ElementTypeSignitureToken)
					? ((Signitures.ElementTypeSignitureToken)token).ResolveToken(this.field.Assembly)
					: ((Signitures.TypeSignitureToken)token).ResolveType(this.field.Assembly, this.field);
				if (type != null) {
					returnType = type;
				}
			}

			return returnType;
		}

		public string GetIdentifier() {
			return this.field.Name;
		}
	}
}
