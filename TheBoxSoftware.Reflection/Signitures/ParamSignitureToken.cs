using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheBoxSoftware.Reflection.Core;

namespace TheBoxSoftware.Reflection.Signitures 
{
	/// <summary>
	/// This class is able to parse and store details about Param entries in signitures. The
	/// Param signiture type is detailed in ECMA 335 at section 23.2.10.
	/// </summary>
	internal sealed class ParamSignitureToken : SignitureTokenContainer
    {
        // 16 bytes
		private ElementTypeSignitureToken elementType;
		private bool isTypeSigniture = false;
        private bool isByRef;
        private bool hasCustomModifier;

		/// <summary>
		/// Initialises a new instance of the ParamSignitureToken class from the provided <paramref name="signiture"/>
        /// at the specified <paramref name="offset"/>.
		/// </summary>
		/// <param name="file">The file which defines the signiture.</param>
		/// <param name="signiture">The contents of the signiture.</param>
		/// <param name="offset">The offset of the current token.</param>
		public ParamSignitureToken(PeCoffFile file, byte[] signiture, Offset offset)
			: base(SignitureTokens.Param) 
        {

			while (CustomModifierToken.IsToken(signiture, offset))
            {
				this.Tokens.Add(new CustomModifierToken(signiture, offset));
				this.HasCustomModifier = true;
			}

			// After a custom modifier the parameter can be defined as a ByRef, TypedByRef or Type token.
			if (ElementTypeSignitureToken.IsToken(signiture, offset, ElementTypes.ByRef)) 
            {
				this.Tokens.Add(new ElementTypeSignitureToken(file, signiture, offset));	// ByRef
				TypeSignitureToken typeSig = new TypeSignitureToken(file, signiture, offset);
				this.Tokens.Add(typeSig);	// Type
				this.elementType = typeSig.ElementType;
				this.isTypeSigniture = true;
				this.IsByRef = true;
			}
			else if (ElementTypeSignitureToken.IsToken(signiture, offset, ElementTypes.TypedByRef)) 
            {
				ElementTypeSignitureToken elementSig = new ElementTypeSignitureToken(file, signiture, offset);
				this.Tokens.Add(elementSig);	// Type
				this.elementType = elementSig;				
			}
			else
            {
				TypeSignitureToken typeSig = new TypeSignitureToken(file, signiture, offset);
				this.Tokens.Add(typeSig);
				this.elementType = typeSig.ElementType;
				this.isTypeSigniture = true;
			}
		}

		public TypeDetails GetTypeDetails(ReflectedMember member) 
        {
			TypeDetails details = new TypeDetails();

			if (this.Tokens.Last() is TypeSignitureToken)
            {
				details = ((TypeSignitureToken)this.Tokens.Last()).GetTypeDetails(member);
			}
			else
            {
				details.Type = ((ElementTypeSignitureToken)this.Tokens.Last()).ResolveToken(member.Assembly);
			}

			details.IsByRef = this.IsByRef;
			return details;
		}

		public TypeRef ResolveParameter(AssemblyDef assembly, ParamDef declaringParameter) 
        {
			TypeRef typeRef = null;

			if (this.isTypeSigniture)
            {
				TypeSignitureToken typeToken = (TypeSignitureToken)this.Tokens.Last();
				typeRef = typeToken.ResolveType(assembly, declaringParameter);
			}
			else
            {
				typeRef = this.elementType.ResolveToken(assembly);
			}

			return typeRef;
		}

        /// <summary>
        /// produces a string representation of the param signiture token.
        /// </summary>
        /// <returns>A string.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("[Param: ");

            if (this.isByRef)
                sb.Append("ByRef ");

            foreach(SignitureToken t in this.Tokens)
            {
                sb.Append(t.ToString());
            }

            sb.Append("]");

            return sb.ToString();
        }

        /// <summary>
        /// The tokenised element from this parameter... may neeed to rethinked...
        /// </summary>
        public ElementTypeSignitureToken ElementType
        {
            get { return this.elementType; }
        }

        /// <summary>
        /// Indicates if the ByRef ElementTypes entry is marked on this parameter.
        /// </summary>
        public bool IsByRef 
        {
            get { return this.isByRef; }
            private set { this.isByRef = value; }
        }

        /// <summary>
        /// Indicates that this parameter has custom modifiers.
        /// </summary>
        public bool HasCustomModifier 
        {
            get { return this.hasCustomModifier; }
            private set { this.hasCustomModifier = value; }
        }
	}
}
