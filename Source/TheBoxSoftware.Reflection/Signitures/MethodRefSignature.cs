
namespace TheBoxSoftware.Reflection.Signitures
{
    using System.Text;
    using Core;

    internal sealed class MethodRefSignature : Signature
    {
        public MethodRefSignature(byte[] signiture) : base(Signatures.MethodRef)
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
                if(SentinalSignatureToken.IsToken(signiture, offset))
                {
                    i--;    // This is not a parameter
                    Tokens.Add(new SentinalSignatureToken(signiture, offset));
                }
                else
                {
                    var param = new ParamSignatureToken(signiture, offset);
                    Tokens.Add(param);
                }
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("[MethodRef: ");

            foreach(SignatureToken t in Tokens)
            {
                sb.Append(t.ToString());
            }

            sb.Append("] ");

            return sb.ToString();
        }
    }
}