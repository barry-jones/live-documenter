
namespace TheBoxSoftware.Reflection.Syntax
{
    using Reflection.Signitures;

    internal class EnumSyntax : Syntax
    {
        private TypeDef _type;

        /// <summary>
        /// Initialises a new instance of the ClassSyntax class.
        /// </summary>
        /// <param name="type">The type to retrieve syntax details for.</param>
        public EnumSyntax(TypeDef type)
        {
            _type = type;
        }

        /// <summary>
        /// Obtains the identifier for the class.
        /// </summary>
        /// <returns>The type identifier.</returns>
        public string GetIdentifier()
        {
            return base.GetTypeName(this.Class);
        }

        public TypeRef GetUnderlyingType()
        {
            TypeRef underlyingType = null;

            if(_type.Fields.Count > 0)
            { // there should always be two field on an enumeration
                underlyingType = ((TypeSignitureToken)_type.Fields[0].Signiture.Tokens[0]).ElementType.ResolveToken(_type.Assembly);
            }

            return underlyingType;
        }

        /// <summary>
        /// Obtains the visibility modifier for the enumeration.
        /// </summary>
        /// <returns>The visibility modifier for the enumeration.</returns>
        public Visibility GetVisibility()
        {
            return _type.MemberAccess;
        }

        /// <summary>
        /// Access to the class reference being represented by this syntax class.
        /// </summary>
        public TypeDef Class
        {
            get { return _type; }
        }
    }
}