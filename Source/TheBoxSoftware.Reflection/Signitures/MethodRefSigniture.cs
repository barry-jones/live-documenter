using System.Collections.Generic;
using TheBoxSoftware.Reflection.Core;

namespace TheBoxSoftware.Reflection.Signitures
{
    internal sealed class MethodRefSigniture : Signiture
    {
        public MethodRefSigniture(byte[] signiture) : base(Signitures.MethodRef)
        {
            List<SignitureToken> tokens = new List<SignitureToken>();
            Offset offset = 0;

            var calling = new CallingConventionSignitureToken(signiture, offset);
            tokens.Add(calling);
            if((calling.Convention & CallingConventions.Generic) != 0)
            {
                var genParamCount = new GenericParamaterCountSignitureToken(signiture, offset);
                tokens.Add(genParamCount);
            }

            var paramCount = new ParameterCountSignitureToken(signiture, offset);
            tokens.Add(paramCount);

            var returnType = new ReturnTypeSignitureToken(signiture, offset);
            tokens.Add(returnType);

            for(int i = 0; i < paramCount.Count; i++)
            {
                if(SentinalSignitureToken.IsToken(signiture, offset))
                {
                    i--;    // This is not a parameter
                    tokens.Add(new SentinalSignitureToken(signiture, offset));
                }
                else
                {
                    var param = new ParamSignitureToken(signiture, offset);
                    tokens.Add(param);
                }
            }
        }
    }
}