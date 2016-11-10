
namespace TheBoxSoftware.Reflection.Signitures
{
    using System.Text;
    using Core;

    internal sealed class MethodRefSigniture : Signiture
    {
        public MethodRefSigniture(byte[] signiture) : base(Signitures.MethodRef)
        {
            Offset offset = 0;

            var calling = new CallingConventionSignitureToken(signiture, offset);
            Tokens.Add(calling);
            if((calling.Convention & CallingConventions.Generic) != 0)
            {
                var genParamCount = new GenericParamaterCountSignitureToken(signiture, offset);
                Tokens.Add(genParamCount);
            }

            var paramCount = new ParameterCountSignitureToken(signiture, offset);
            Tokens.Add(paramCount);

            var returnType = new ReturnTypeSignitureToken(signiture, offset);
            Tokens.Add(returnType);

            for(int i = 0; i < paramCount.Count; i++)
            {
                if(SentinalSignitureToken.IsToken(signiture, offset))
                {
                    i--;    // This is not a parameter
                    Tokens.Add(new SentinalSignitureToken(signiture, offset));
                }
                else
                {
                    var param = new ParamSignitureToken(signiture, offset);
                    Tokens.Add(param);
                }
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("[MethodRef: ");

            foreach(SignitureToken t in Tokens)
            {
                sb.Append(t.ToString());
            }

            sb.Append("] ");

            return sb.ToString();
        }
    }
}