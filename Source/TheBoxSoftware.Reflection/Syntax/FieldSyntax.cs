
namespace TheBoxSoftware.Reflection.Syntax
{
    internal class FieldSyntax : Syntax
    {
        private FieldDef _field;

        public FieldSyntax(FieldDef field)
        {
            _field = field;
        }

        public Visibility GetVisibility()
        {
            return _field.MemberAccess;
        }

        /// <summary>
        /// Obtains the name of type which this field is.
        /// </summary>
        /// <returns></returns>
        public new TypeRef GetType()
        {
            TypeRef returnType = null;

            Signitures.Signiture signiture = _field.Signiture;
            Signitures.SignitureToken token = signiture.Tokens.Find(
                t => t.TokenType == Signitures.SignitureTokens.ElementType ||
                    t.TokenType == Signitures.SignitureTokens.Type
                );
            if(token != null)
            {
                TypeRef type = (token is Signitures.ElementTypeSignatureToken)
                    ? ((Signitures.ElementTypeSignatureToken)token).ResolveToken(_field.Assembly)
                    : ((Signitures.TypeSignitureToken)token).ResolveType(_field.Assembly, _field);
                if(type != null)
                {
                    returnType = type;
                }
            }

            return returnType;
        }

        public string GetIdentifier()
        {
            return _field.Name;
        }
    }
}