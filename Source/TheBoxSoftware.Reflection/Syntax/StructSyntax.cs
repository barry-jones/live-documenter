
namespace TheBoxSoftware.Reflection.Syntax
{
    internal class StructSyntax : Syntax
    {
        private TypeDef _type;

        /// <summary>
        /// Initialises a new instance of the ClassSyntax class.
        /// </summary>
        /// <param name="type">The type to retrieve syntax details for.</param>
        public StructSyntax(TypeDef type)
        {
            _type = type;
        }

        public Visibility GetVisibility()
        {
            return _type.MemberAccess;
        }

        public string GetIdentifier()
        {
            return _type.GetDisplayName(false);
        }

        public TypeRef[] GetInterfaces()
        {
            TypeRef[] interfaces = new TypeRef[_type.Implements.Count];
            for(int i = 0; i < _type.Implements.Count; i++)
            {
                interfaces[i] = _type.Implements[i];
            }
            return interfaces;
        }
    }
}