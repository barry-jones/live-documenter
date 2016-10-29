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
        private ElementTypeSignitureToken _elementType;
        private bool _isTypeSigniture = false;
        private bool _isByRef;
        private bool _hasCustomModifier;

        /// <summary>
        /// Initialises a new instance of the ParamSignitureToken class from the provided <paramref name="signiture"/>
        /// at the specified <paramref name="offset"/>.
        /// </summary>
        /// <param name="file">The file which defines the signiture.</param>
        /// <param name="signiture">The contents of the signiture.</param>
        /// <param name="offset">The offset of the current token.</param>
        public ParamSignitureToken(byte[] signiture, Offset offset)
            : base(SignitureTokens.Param)
        {

            while(CustomModifierToken.IsToken(signiture, offset))
            {
                this.Tokens.Add(new CustomModifierToken(signiture, offset));
                this.HasCustomModifier = true;
            }

            // After a custom modifier the parameter can be defined as a ByRef, TypedByRef or Type token.
            if(ElementTypeSignitureToken.IsToken(signiture, offset, ElementTypes.ByRef))
            {
                this.Tokens.Add(new ElementTypeSignitureToken(signiture, offset));    // ByRef
                TypeSignitureToken typeSig = new TypeSignitureToken(signiture, offset);
                this.Tokens.Add(typeSig);   // Type
                this._elementType = typeSig.ElementType;
                this._isTypeSigniture = true;
                this.IsByRef = true;
            }
            else if(ElementTypeSignitureToken.IsToken(signiture, offset, ElementTypes.TypedByRef))
            {
                ElementTypeSignitureToken elementSig = new ElementTypeSignitureToken(signiture, offset);
                this.Tokens.Add(elementSig);    // Type
                this._elementType = elementSig;
            }
            else
            {
                TypeSignitureToken typeSig = new TypeSignitureToken(signiture, offset);
                this.Tokens.Add(typeSig);
                this._elementType = typeSig.ElementType;
                this._isTypeSigniture = true;
            }
        }

        public TypeDetails GetTypeDetails(ReflectedMember member)
        {
            TypeDetails details = new TypeDetails();

            if(this.Tokens.Last() is TypeSignitureToken)
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

            if(this._isTypeSigniture)
            {
                TypeSignitureToken typeToken = (TypeSignitureToken)this.Tokens.Last();
                typeRef = typeToken.ResolveType(assembly, declaringParameter);
            }
            else
            {
                typeRef = this._elementType.ResolveToken(assembly);
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

            if(this._isByRef)
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
            get { return this._elementType; }
        }

        /// <summary>
        /// Indicates if the ByRef ElementTypes entry is marked on this parameter.
        /// </summary>
        public bool IsByRef
        {
            get { return this._isByRef; }
            private set { this._isByRef = value; }
        }

        /// <summary>
        /// Indicates that this parameter has custom modifiers.
        /// </summary>
        public bool HasCustomModifier
        {
            get { return this._hasCustomModifier; }
            private set { this._hasCustomModifier = value; }
        }
    }
}