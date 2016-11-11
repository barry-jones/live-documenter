
namespace TheBoxSoftware.Reflection.Signatures
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Core;

    internal class Signature
    {
        private Signatures _type;
        private List<SignatureToken> _tokens;

        public Signature()
        {
            _tokens = new List<SignatureToken>();
        }

        /// <summary>
        /// Initialises a new instance of the Signiture class.
        /// </summary>
        /// <param name="tokenType">The type of signiture being represented.</param>
        protected Signature(Signatures tokenType)
        {
            _type = tokenType;
            _tokens = new List<SignatureToken>();
        }

        /// <summary>
        /// Factory method for creating new Signatures.
        /// </summary>
        /// <param name="source">The contents of the library the signiture is being read from</param>
        /// <param name="offset">The offset to the start of the signiture</param>
        /// <param name="tokenType">The type of signiture being read.</param>
        /// <returns></returns>
        public static Signature Create(byte[] source, Offset offset, Signatures tokenType)
        {
            int startingOffset = offset;
            uint lengthOfSigniture = SignatureToken.GetCompressedValue(source, offset);    // The first byte is always the length

            // Read the full signiture
            byte[] signiture = new byte[lengthOfSigniture];
            for(int i = 0; i < lengthOfSigniture; i++)
            {
                signiture[i] = source[offset.Shift(1)];
            }

            // Instatiate the correct signiture reader and pass control
            Signature instantiatedSigniture = null;
            switch(tokenType)
            {
                case Signatures.CustomAttribute: // instantiatedSigniture = new CustomAttributeSigniture(signiture); break;
                case Signatures.MethodSpecification:  // to do: implement
                    break;
                case Signatures.LocalVariable: instantiatedSigniture = new LocalVariableSignature(signiture); break;
                case Signatures.MethodDef: instantiatedSigniture = new MethodDefSignature(signiture); break;
                case Signatures.MethodRef: instantiatedSigniture = new MethodRefSignature(signiture); break;
                case Signatures.Field: instantiatedSigniture = new FieldSignature(signiture); break;
                case Signatures.Property: instantiatedSigniture = new PropertySignature(signiture); break;
                case Signatures.TypeSpecification: instantiatedSigniture = new TypeSpecificationSignature(signiture); break;
            }
            return instantiatedSigniture;
        }

        /// <summary>
        /// Returns a collection of all the parameter tokens defined in this
        /// signiture.
        /// </summary>
        /// <returns>A collection of parameter tokens.</returns>
        public List<ParamSignatureToken> GetParameterTokens()
        {
            return (from token in Tokens
                    where token is ParamSignatureToken
                    select (ParamSignatureToken)token).ToList();
        }

        /// <summary>
        /// Returns the token that describes the return type defined in the signiture.
        /// </summary>
        /// <returns>The Token or null if no return type defined.</returns>
        public ReturnTypeSignatureToken GetReturnTypeToken()
        {
            ReturnTypeSignatureToken token = null;
            for(int i = 0; i < _tokens.Count; i++)
            {
                if(_tokens[i] is ReturnTypeSignatureToken)
                {
                    token = _tokens[i] as ReturnTypeSignatureToken;
                    break;
                }
            }
            return token;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            foreach(SignatureToken current in Tokens)
            {
                builder.Append(current.ToString());
            }
            return builder.ToString();
        }

        /// <summary>
        /// Describes the type of signiture.
        /// </summary>
        public Signatures Type
        {
            get { return _type; }
            set { _type = value; }
        }

        public List<SignatureToken> Tokens
        {
            get { return _tokens; }
            protected set { _tokens = value; }
        }
    }
}