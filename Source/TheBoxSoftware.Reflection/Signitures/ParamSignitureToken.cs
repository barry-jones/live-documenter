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
        /// <param name="signiture">The contents of the signiture.</param>
        /// <param name="offset">The offset of the current token.</param>
        public ParamSignitureToken(byte[] signiture, Offset offset)
            : base(SignitureTokens.Param)
        {

            while(CustomModifierToken.IsToken(signiture, offset))
            {
                Tokens.Add(new CustomModifierToken(signiture, offset));
                HasCustomModifier = true;
            }

            // After a custom modifier the parameter can be defined as a ByRef, TypedByRef or Type token.
            if(ElementTypeSignitureToken.IsToken(signiture, offset, ElementTypes.ByRef))
            {
                Tokens.Add(new ElementTypeSignitureToken(signiture, offset));    // ByRef
                TypeSignitureToken typeSig = new TypeSignitureToken(signiture, offset);
                Tokens.Add(typeSig);   // Type
                _elementType = typeSig.ElementType;
                _isTypeSigniture = true;
                IsByRef = true;
            }
            else if(ElementTypeSignitureToken.IsToken(signiture, offset, ElementTypes.TypedByRef))
            {
                ElementTypeSignitureToken elementSig = new ElementTypeSignitureToken(signiture, offset);
                Tokens.Add(elementSig);    // Type
                _elementType = elementSig;
            }
            else
            {
                TypeSignitureToken typeSig = new TypeSignitureToken(signiture, offset);
                Tokens.Add(typeSig);
                _elementType = typeSig.ElementType;
                _isTypeSigniture = true;
            }
        }

        public TypeDetails GetTypeDetails(ReflectedMember member)
        {
            TypeDetails details = new TypeDetails();

            if(this.Tokens.Last() is TypeSignitureToken)
            {
                details = ((TypeSignitureToken)Tokens.Last()).GetTypeDetails(member);
            }
            else
            {
                details.Type = ((ElementTypeSignitureToken)Tokens.Last()).ResolveToken(member.Assembly);
            }

            details.IsByRef = IsByRef;

            return details;
        }

        public TypeRef ResolveParameter(AssemblyDef assembly, ParamDef declaringParameter)
        {
            TypeRef typeRef = null;

            if(_isTypeSigniture)
            {
                TypeSignitureToken typeToken = Tokens.Last() as TypeSignitureToken;
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

            foreach(SignitureToken t in Tokens)
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