
namespace TheBoxSoftware.Reflection.Syntax
{
    using Reflection.Signitures;

    internal class ParameterSyntax
    {
        private ParamDef _param;
        private ParamSignitureToken _signitureToken;

        public ParameterSyntax(ParamDef param, ParamSignitureToken signitureToken)
        {
            _param = param;
            _signitureToken = signitureToken;
        }
    }
}