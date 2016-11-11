
namespace TheBoxSoftware.Reflection.Signitures
{
    using System.Text;
    using Core;

    /// <summary>
    /// A signiture for a method definition as described in section 23.2.1 in
    /// ECMA 335.
    /// </summary>
    internal sealed class MethodDefSigniture : Signiture
    {
        /// <summary>
        /// Initialises a new instance of the MethoDefSigniture class.
        /// </summary>
        /// <param name="signiture">The byte contents of the signiture.</param>
        public MethodDefSigniture(byte[] signiture) : base(Signitures.MethodDef)
        {
            Offset offset = 0;

            var calling = new CallingConventionSignatureToken(signiture, offset);
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
                var param = new ParamSignitureToken(signiture, offset);
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

            foreach(SignitureToken t in Tokens)
            {
                sb.Append(t.ToString());
            }

            sb.Append("] ");

            return sb.ToString();
        }
    }
}