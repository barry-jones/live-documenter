
namespace TheBoxSoftware.Reflection.Signitures
{
    using System.Text;
    using Core;

    /// <summary>
    /// A signiture for a method definition as described in section 23.2.1 in
    /// ECMA 335.
    /// </summary>
    internal sealed class MethodDefSignature : Signature
    {
        /// <summary>
        /// Initialises a new instance of the MethoDefSigniture class.
        /// </summary>
        /// <param name="signiture">The byte contents of the signiture.</param>
        public MethodDefSignature(byte[] signiture) : base(Signatures.MethodDef)
        {
            Offset offset = 0;

            var calling = new CallingConventionSignatureToken(signiture, offset);
            Tokens.Add(calling);

            if((calling.Convention & CallingConventions.Generic) != 0)
            {
                var genParamCount = new GenericParamaterCountSignatureToken(signiture, offset);
                Tokens.Add(genParamCount);
            }

            var paramCount = new ParameterCountSignatureToken(signiture, offset);
            Tokens.Add(paramCount);

            var returnType = new ReturnTypeSignatureToken(signiture, offset);
            Tokens.Add(returnType);
            for(int i = 0; i < paramCount.Count; i++)
            {
                var param = new ParamSignatureToken(signiture, offset);
                Tokens.Add(param);
            }
        }

        public static CallingConventions GetCallingConvention(byte[] signiture)
        {
            return new CallingConventionSignatureToken(signiture, 0).Convention;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("[MethodDef: ");

            foreach(SignatureToken t in Tokens)
            {
                sb.Append(t.ToString());
            }

            sb.Append("] ");

            return sb.ToString();
        }
    }
}