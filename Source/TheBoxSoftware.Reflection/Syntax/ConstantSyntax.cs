
namespace TheBoxSoftware.Reflection.Syntax
{
    /// <summary>
    /// Class that interogates a member for the details required to declare a constant.
    /// </summary>
    internal class ConstantSyntax : Syntax
    {
        private FieldDef _field;

        /// <summary>
        /// Initialises a new instance of the ConstantSyntax class.
        /// </summary>
        /// <param name="field">The field to obtain the syntax details for.</param>
        public ConstantSyntax(FieldDef field)
        {
            _field = field;
        }

        public string[] GetAttributes()
        {
            return new string[0];
        }

        /// <summary>
        /// Obtains the visibility of the constant value.
        /// </summary>
        /// <returns>A visibility enumerated value.</returns>
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

            Signitures.Signature signiture = this._field.Signiture;
            Signitures.SignatureToken token = signiture.Tokens.Find(
                t => t.TokenType == Signitures.SignatureTokens.ElementType || t.TokenType == Signitures.SignatureTokens.Type
                );

            if(token != null)
            {
                TypeRef type = (token is Signitures.TypeSignatureToken)
                    ? ((Signitures.TypeSignatureToken)token).ResolveType(_field.Assembly, null)
                    : ((Signitures.ElementTypeSignatureToken)token).ResolveToken(_field.Assembly);

                returnType = type;
            }

            return returnType;
        }

        public string GetIdentifier()
        {
            return _field.Name;
        }
    }
}