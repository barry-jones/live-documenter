
namespace TheBoxSoftware.Reflection.Syntax
{
    using Reflection.Signitures;

    internal class ParameterSyntax
    {
        private ParamDef _param;
        private ParamSignatureToken _signitureToken;

        public ParameterSyntax(ParamDef param, ParamSignatureToken signitureToken)
        {
            _param = param;
            _signitureToken = signitureToken;
        }
    }
}