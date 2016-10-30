
namespace TheBoxSoftware.Reflection.Signitures
{
    using System.Collections.Generic;
    using System.Linq;
    using Core;

    internal abstract class Signiture
    {
        private Signitures _type;
        private List<SignitureToken> _tokens;

        /// <summary>
        /// Initialises a new instance of the Signiture class.
        /// </summary>
        /// <param name="tokenType">The type of signiture being represented.</param>
        protected Signiture(Signitures tokenType)
        {
            _type = tokenType;
            _tokens = new List<SignitureToken>();
        }

        /// <summary>
        /// Factory method for creating new Signitures.
        /// </summary>
        /// <param name="source">The contents of the library the signiture is being read from</param>
        /// <param name="offset">The offset to the start of the signiture</param>
        /// <param name="tokenType">The type of signiture being read.</param>
        /// <returns></returns>
        public static Signiture Create(byte[] source, Offset offset, Signitures tokenType)
        {
            int startingOffset = offset;
            int lengthOfSigniture = SignitureToken.GetCompressedValue(source, offset);    // The first byte is always the length

            // Read the full signiture
            byte[] signiture = new byte[lengthOfSigniture];
            for(int i = 0; i < lengthOfSigniture; i++)
            {
                signiture[i] = source[offset.Shift(1)];
            }

            // Instatiate the correct signiture reader and pass control
            Signiture instantiatedSigniture = null;
            switch(tokenType)
            {
                case Signitures.CustomAttribute: // instantiatedSigniture = new CustomAttributeSigniture(signiture); break;
                case Signitures.MethodSpecification:  // to do: implement
                    break;
                case Signitures.LocalVariable: instantiatedSigniture = new LocalVariableSigniture(signiture); break;
                case Signitures.MethodDef: instantiatedSigniture = new MethodDefSigniture(signiture); break;
                case Signitures.MethodRef: instantiatedSigniture = new MethodRefSigniture(signiture); break;
                case Signitures.Field: instantiatedSigniture = new FieldSigniture(signiture); break;
                case Signitures.Property: instantiatedSigniture = new PropertySigniture(signiture); break;
                case Signitures.TypeSpecification: instantiatedSigniture = new TypeSpecificationSigniture(signiture); break;
            }
            return instantiatedSigniture;
        }

        /// <summary>
        /// Returns a collection of all the parameter tokens defined in this
        /// signiture.
        /// </summary>
        /// <returns>A collection of parameter tokens.</returns>
        public List<ParamSignitureToken> GetParameterTokens()
        {
            return (from token in Tokens
                    where token is ParamSignitureToken
                    select (ParamSignitureToken)token).ToList<ParamSignitureToken>();
        }

        /// <summary>
        /// Returns the token that describes the return type defined in the signiture.
        /// </summary>
        /// <returns>The Token or null if no return type defined.</returns>
        public ReturnTypeSignitureToken GetReturnTypeToken()
        {
            ReturnTypeSignitureToken token = null;
            for(int i = 0; i < _tokens.Count; i++)
            {
                if(_tokens[i] is ReturnTypeSignitureToken)
                {
                    token = _tokens[i] as ReturnTypeSignitureToken;
                    break;
                }
            }
            return token;
        }

        /// <summary>
        /// Describes the type of signiture.
        /// </summary>
        public Signitures Type
        {
            get { return _type; }
            protected set { _type = value; }
        }

        public List<SignitureToken> Tokens
        {
            get { return _tokens; }
            protected set { _tokens = value; }
        }
    }
}