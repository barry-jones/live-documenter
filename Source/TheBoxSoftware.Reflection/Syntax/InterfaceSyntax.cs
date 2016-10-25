
namespace TheBoxSoftware.Reflection.Syntax
{
    using System;

    internal sealed class InterfaceSyntax : Syntax
    {
        private TypeDef _type;

        /// <summary>
        /// Initialises a new instance of the InterfaceSyntax class.
        /// </summary>
        /// <param name="type">The type to retrieve syntax details for.</param>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the provided <paramref name="type"/> is not an interface.
        /// </exception>
        public InterfaceSyntax(TypeDef type)
        {
            _type = type;
        }

        /// <summary>
        /// Obtains the identifier for the class.
        /// </summary>
        /// <returns>The type identifier.</returns>
        public string GetIdentifier()
        {
            return _type.GetDisplayName(false);
        }

        /// <summary>
        /// Obtains the names of all the interfaces this class implements.
        /// </summary>
        /// <returns>An array of strings identifying the interfaces.</returns>
        public TypeRef[] GetInterfaces()
        {
            TypeRef[] interfaces = new TypeRef[_type.Implements.Count];
            for(int i = 0; i < _type.Implements.Count; i++)
            {
                interfaces[i] = _type.Implements[i];
            }
            return interfaces;
        }

        /// <summary>
        /// Obtains the name of the base type this class implements.
        /// </summary>
        /// <returns>The base class for the type.</returns>
        public string GetBaseClass()
        {
#if DEBUG
            if(_type.InheritsFrom == null)
            {
                System.Diagnostics.Debug.WriteLine($"class {_type.Name} has a null base type.");
                return string.Empty;
            }
#endif
            return _type.InheritsFrom.GetDisplayName(false);
        }

        /// <summary>
        /// Obtains the visibility modifier for the class.
        /// </summary>
        /// <returns>The visibility modifier for the class.</returns>
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