
namespace TheBoxSoftware.Reflection.Signatures
{
    using System.Linq;
    using System.Text;
    using Core;

    /// <summary>
    /// This class is able to parse and store details about Param entries in Signatures. The
    /// Param signiture type is detailed in ECMA 335 at section 23.2.10.
    /// </summary>
    internal sealed class ParamSignatureToken : SignatureTokenContainer
    {
        private ElementTypeSignatureToken _elementType;
        private bool _isTypeSigniture = false;
        private bool _isByRef;
        private bool _hasCustomModifier;

        /// <summary>
        /// Initialises a new instance of the ParamSignitureToken class from the provided <paramref name="signiture"/>
        /// at the specified <paramref name="offset"/>.
        /// </summary>
        /// <param name="signiture">The contents of the signiture.</param>
        /// <param name="offset">The offset of the current token.</param>
        public ParamSignatureToken(byte[] signiture, Offset offset)
            : base(SignatureTokens.Param)
        {

            while(CustomModifierToken.IsToken(signiture, offset))
            {
                Tokens.Add(new CustomModifierToken(signiture, offset));
                HasCustomModifier = true;
            }

            // After a custom modifier the parameter can be defined as a ByRef, TypedByRef or Type token.
            if(ElementTypeSignatureToken.IsToken(signiture, offset, ElementTypes.ByRef))
            {
                Tokens.Add(new ElementTypeSignatureToken(signiture, offset));    // ByRef
                TypeSignatureToken typeSig = new TypeSignatureToken(signiture, offset);
                Tokens.Add(typeSig);   // Type
                _elementType = typeSig.ElementType;
                _isTypeSigniture = true;
                IsByRef = true;
            }
            else if(ElementTypeSignatureToken.IsToken(signiture, offset, ElementTypes.TypedByRef))
            {
                ElementTypeSignatureToken elementSig = new ElementTypeSignatureToken(signiture, offset);
                Tokens.Add(elementSig);    // Type
                _elementType = elementSig;
            }
            else
            {
                TypeSignatureToken typeSig = new TypeSignatureToken(signiture, offset);
                Tokens.Add(typeSig);
                _elementType = typeSig.ElementType;
                _isTypeSigniture = true;
            }
        }

        public TypeDetails GetTypeDetails(ReflectedMember member)
        {
            TypeDetails details = new TypeDetails();
            SignatureToken token = Tokens.Last();

            if(token is TypeSignatureToken)
            {
                details = ((TypeSignatureToken)token).GetTypeDetails(member);
            }
            else
            {
                details.Type = ((ElementTypeSignatureToken)token).ResolveToken(member.Assembly);
            }

            details.IsByRef = IsByRef;

            return details;
        }

        public TypeRef ResolveParameter(AssemblyDef assembly, ParamDef declaringParameter)
        {
            TypeRef typeRef = null;

            if(_isTypeSigniture)
            {
                TypeSignatureToken typeToken = Tokens.Last() as TypeSignatureToken;
                typeRef = typeToken.ResolveType(assembly, declaringParameter);
            }
            else
            {
                typeRef = _elementType.ResolveToken(assembly);
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

            if(_isByRef)
                sb.Append("ByRef ");

            foreach(SignatureToken t in Tokens)
            {
                sb.Append(t.ToString());
            }

            sb.Append("]");

            return sb.ToString();
        }

        /// <summary>
        /// The tokenised element from this parameter... may neeed to rethinked...
        /// </summary>
        public ElementTypeSignatureToken ElementType
        {
            get { return _elementType; }
        }

        /// <summary>
        /// Indicates if the ByRef ElementTypes entry is marked on this parameter.
        /// </summary>
        public bool IsByRef
        {
            get { return _isByRef; }
            private set { _isByRef = value; }
        }

        /// <summary>
        /// Indicates that this parameter has custom modifiers.
        /// </summary>
        public bool HasCustomModifier
        {
            get { return _hasCustomModifier; }
            private set { _hasCustomModifier = value; }
        }
    }
}