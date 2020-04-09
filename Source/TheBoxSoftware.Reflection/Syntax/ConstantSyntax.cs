
namespace TheBoxSoftware.Reflection.Syntax
{
    /// <summary>
    /// Class that interogates a member for the details required to declare a constant.
    /// </summary>
    internal class ConstantSyntax : Syntax
    {
        private readonly FieldDef _field;

        /// <summary>
        /// Initialises a new instance of the ConstantSyntax class.
        /// </summary>
        /// <param name="field">The field to obtain the syntax details for.</param>
        public ConstantSyntax(FieldDef field)
        {
            _field = field;
        }

        /// <summary>
        /// Not implemented, returns an empty array.
        /// </summary>
        /// <returns></returns>
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

            Signatures.Signature signiture = _field.Signiture;
            Signatures.SignatureToken token = signiture.Tokens.Find(
                t => t.TokenType == Signatures.SignatureTokens.ElementType || t.TokenType == Signatures.SignatureTokens.Type
                );

            if(token != null)
            {
                TypeRef type = (token is Signatures.TypeSignatureToken)
                    ? ((Signatures.TypeSignatureToken)token).ResolveType(_field.Assembly, null)
                    : ((Signatures.ElementTypeSignatureToken)token).ResolveToken(_field.Assembly);

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